namespace SIS.Framework.Utilities
{
    public static class ControllerUtilities
    {
        public static string GetControllerName(object controller)
        {
            return controller.GetType()
                .Name
                .Replace(MvcContext.Get.ControllerSuffix, string.Empty);
        }

        public static string GetViewFullQualifiedName(string controllerName, string viewName)
        {
            return string.Format("../../../{0}/{1}/{2}.html", MvcContext.Get.ViewsFolder, controllerName, viewName);
        }
    }
}
