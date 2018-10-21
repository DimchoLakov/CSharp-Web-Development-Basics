using System.Collections.Generic;

namespace SIS.Framework.Models
{
    public class ViewModel
    {
        public ViewModel()
        {
            this.Data = new Dictionary<string, object>();
        }

        public Dictionary<string, object> Data { get; set; }

        public object this[string key]
        {
            get { return this.Data[key];}
            set { this.Data[key] = value; }
        }
    }
}
