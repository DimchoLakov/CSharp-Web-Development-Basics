using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SIS.Framework.Services
{
    public class DependencyContainer : IDependencyContainer
    {
        private readonly IDictionary<Type, Type> dependencyDictionary;

        public DependencyContainer()
        {
            this.dependencyDictionary = new Dictionary<Type, Type>();
        }

        private Type this[Type key]
        {
            get
            {
                return this.dependencyDictionary.ContainsKey(key) ? this.dependencyDictionary[key] : null;
            }
        }

        public T CreateInstance<T>()
        {
            return (T)CreateInstance(typeof(T));
        }

        public object CreateInstance(Type type)
        {
            Type instanceType = this[type] ?? type;

            if (instanceType.IsInterface || instanceType.IsAbstract)
            {
                throw new InvalidOperationException($"Type {type.FullName} cannot be instantiated.");
            }

            ConstructorInfo constructor = instanceType
                .GetConstructors()
                .OrderByDescending(x => x.GetParameters().Length)
                .First();

            ParameterInfo[] constructorParameters = constructor.GetParameters();
            object[] constructorParameterObjects = new object[constructorParameters.Length];

            for (int i = 0; i < constructorParameters.Length; i++)
            {
                constructorParameterObjects[i] = this.CreateInstance(constructorParameters[i].ParameterType);
            }

            return constructor.Invoke(constructorParameterObjects);
        }

        public void RegisterDependency<TSource, TDestination>()
        {
            this.dependencyDictionary[typeof(TSource)] = typeof(TDestination);
        }
    }
}
