namespace SIS.Framework.Models
{
    public class Model
    {
        private bool? isValid;

        public bool? IsValid
        {
            get { return this.isValid; }
            set { this.isValid = this.isValid ?? value; }
        }
    }
}
