namespace SIS.Framework.Attributes.Properties
{
    public class NumberRangeAttribute : ValidationAttribute
    {
        private readonly double minimumValue;

        private readonly double maximumValue;

        public NumberRangeAttribute(double minValue = double.MinValue, double maxValue = double.MaxValue)
        {
            this.minimumValue = minValue;
            this.maximumValue = maxValue;
        }

        public override bool IsValid(object value)
        {
            return this.minimumValue <= (double)value && this.maximumValue >= (double)value;
        }
    }
}
