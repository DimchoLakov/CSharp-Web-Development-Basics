using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SIS.Framework.ActionResults.Interfaces;
using SIS.Framework.Attributes.Action;
using SIS.Framework.Attributes.Methods;
using SIS.Framework.Attributes.Properties;
using SIS.Framework.Controllers;
using SIS.Framework.Services;
using SIS.HTTP.Enums;
using SIS.HTTP.Extensions;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer.Api.Interfaces;
using SIS.WebServer.Results;

namespace SIS.Framework.Routers
{
    public class ControllerRouter : IHttpHandler
    {
        private const string UnsupportedActionMessage = "The view result is not supported.";

        private readonly IDependencyContainer dependencyContainer;

        public ControllerRouter(IDependencyContainer dependencyContainer)
        {
            this.dependencyContainer = dependencyContainer;
        }

        private Controller GetController(string controllerName, IHttpRequest request)
        {
            if (controllerName != null)
            {
                var controllerType = Assembly
                    .GetEntryAssembly()
                    .GetTypes()
                    .FirstOrDefault
                             (
                                 x => x.Name.ToLower() == controllerName.ToLower() + MvcContext.Get.ControllerSuffix.ToLower()
                             );

                var controller = (Controller)this.dependencyContainer.CreateInstance(controllerType);

                if (controller != null)
                {
                    controller.Request = request;
                }

                return controller;
            }

            return null;
        }

        private MethodInfo GetMethod(string requestMethod, Controller controller, string actionName)
        {
            MethodInfo action = null;

            foreach (var methodInfo in this.GetSuitableMethods(controller, actionName))
            {
                var attributes = methodInfo
                    .GetCustomAttributes()
                    .Where(ca => ca is HttpMethodAttribute)
                    .Cast<HttpMethodAttribute>();

                if (!attributes.Any() && requestMethod.ToUpper() == "GET")
                {
                    return methodInfo;
                }

                foreach (var attribute in attributes)
                {
                    if (attribute.IsValid(requestMethod))
                    {
                        return methodInfo;
                    }
                }
            }

            return action;
        }

        private IEnumerable<MethodInfo> GetSuitableMethods(Controller controller, string actionName)
        {
            if (controller == null)
            {
                return new MethodInfo[0];
            }

            return controller.GetType()
                .GetMethods()
                .Where(m => m.Name.ToLower() == actionName.ToLower());
        }

        private IHttpResponse PrepareResponse(IActionResult actionResult)
        {
            var invocationResult = actionResult.Invoke();

            if (actionResult is IViewable)
            {
                return new HtmlResult(invocationResult, HttpResponseStatusCode.Ok);
            }

            if (actionResult is IRedirectable)
            {
                return new RedirectResult(invocationResult);
            }

            throw new InvalidOperationException(UnsupportedActionMessage);
        }

        public IHttpResponse Handle(IHttpRequest request)
        {
            var controllerName = string.Empty;
            var actionName = string.Empty;
            var requestMethod = request.RequestMethod.ToString();

            if (request.Path == "/")
            {
                controllerName = "Home";
                actionName = "Index";
            }
            else if (requestMethod.ToUpper() == "POST")
            {
                var requestUrlSplit = request.Path.Split("/", StringSplitOptions.RemoveEmptyEntries);
                controllerName = requestUrlSplit[0].Capitalize();
                actionName = "Do" + requestUrlSplit[1].Capitalize();
            }
            else
            {
                var requestUrlSplit = request.Path.Split("/", StringSplitOptions.RemoveEmptyEntries);
                controllerName = requestUrlSplit[0].Capitalize();
                actionName = requestUrlSplit[1].Capitalize();
            }

            var controller = this.GetController(controllerName, request);
            var action = this.GetMethod(requestMethod, controller, actionName);

            if (controller == null || action == null)
            {
                throw new NullReferenceException();
            }

            object[] actionParameters = this.MapActionParameters(controller, action, request);

            IActionResult actionResult = this.InvokeAction(controller, action, actionParameters);

            return this.Authorize(controller, action) ?? this.PrepareResponse(actionResult);
        }

        private IActionResult InvokeAction(Controller controller, MethodInfo action, object[] actionParameters)
        {
            return (IActionResult)action.Invoke(controller, actionParameters);
        }

        private object[] MapActionParameters(Controller controller, MethodInfo action, IHttpRequest request)
        {
            var actionParameteres = action.GetParameters();
            object[] mappedActionParameters = new object[actionParameteres.Length];
            for (int i = 0; i < actionParameteres.Length; i++)
            {
                var actionParameter = actionParameteres[i];

                if (actionParameter.ParameterType.IsPrimitive ||
                    actionParameter.ParameterType == typeof(string))
                {
                    var mappedActionParameter = new object();
                    mappedActionParameter = this.ProcessPrimitiveParameter(actionParameter, request);
                    if (mappedActionParameter == null)
                    {
                        break;
                    }
                }
                else
                {
                    var bindingModel = this.ProcessBindingModelParameter(actionParameter, request);
                    controller.ModelState.IsValid = this.IsValidModel(
                        bindingModel,
                        actionParameter.ParameterType);
                    mappedActionParameters[i] = bindingModel;
                }

            }

            return mappedActionParameters;
        }

        private bool? IsValidModel(object bindingModel, Type bindingModelType)
        {
            var properties = bindingModelType.GetProperties();

            foreach (var property in properties)
            {
                var propertyValidationAttributes = property
                    .GetCustomAttributes()
                    .Where(ca => ca is ValidationAttribute)
                    .Cast<ValidationAttribute>()
                    .ToList();

                foreach (var validationAttribute in propertyValidationAttributes)
                {
                    var propertyValue = property.GetValue(bindingModel);

                    if (!validationAttribute.IsValid(propertyValue))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private object ProcessBindingModelParameter(ParameterInfo parameter, IHttpRequest request)
        {
            var bindingModelType = parameter.ParameterType;

            var bindingModelInstance = Activator.CreateInstance(bindingModelType);
            var bindingModelProperties = bindingModelType.GetProperties();

            foreach (var property in bindingModelProperties)
            {
                try
                {
                    object value = this.GetParameterFromRequestData(request, property.Name.ToLower());
                    property.SetValue(bindingModelInstance, Convert.ChangeType(value, property.PropertyType));
                }
                catch (Exception)
                {
                    Console.WriteLine($"The {property.Name} could not be mapped.");
                }
            }

            return Convert.ChangeType(bindingModelInstance, bindingModelType);
        }

        private object ProcessPrimitiveParameter(ParameterInfo parameter, IHttpRequest request)
        {
            object value = this.GetParameterFromRequestData(request, parameter.Name);
            return Convert.ChangeType(value, parameter.ParameterType);
        }

        private object GetParameterFromRequestData(IHttpRequest request, string key)
        {
            var queryDataKey = request.QueryData.FirstOrDefault(x => x.Key.ToLower() == key).Key;
            var queryDataKeyExists = queryDataKey != null;

            if (queryDataKeyExists)
            {
                return request.QueryData[queryDataKey];
            }

            var formDataKey = request.FormData.FirstOrDefault(x => x.Key.ToLower() == key).Key;
            var formDataKeyExists = formDataKey != null;

            if (formDataKeyExists)
            {
                return request.FormData[formDataKey];
            }

            return null;
        }

        private IHttpResponse Authorize(Controller controller, MethodInfo action)
        {
            if (action
                .GetCustomAttributes()
                .Where(a => a is AuthorizeAttribute)
                .Cast<AuthorizeAttribute>()
                .Any(a => !a.IsAuthorized(controller.Identity)))
            {
                return new UnauthorizedResult();
            }

            return null;
        }
    }
}
