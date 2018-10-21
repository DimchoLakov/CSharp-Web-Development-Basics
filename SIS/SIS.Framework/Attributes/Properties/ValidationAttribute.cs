using System;

namespace SIS.Framework.Attributes.Properties
{
    public abstract class ValidationAttribute : Attribute
    {
        public abstract bool IsValid(object value);
    }
}
