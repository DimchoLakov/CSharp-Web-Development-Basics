using System.Collections.Concurrent;
using System.Collections.Generic;
using SIS.HTTP.Sessions.Interfaces;

namespace SIS.HTTP.Sessions
{
    public class HttpSession : IHttpSession
    {
        private readonly Dictionary<string, object> parameters;

        public HttpSession(string id)
        {
            this.parameters = new Dictionary<string, object>();
            this.Id = id;
        }

        public string Id { get; }

        public object GetParameter(string name)
        {
            if (!this.ContainsParameter(name))
            {
                return null;
            }

            return this.parameters[name];
        }

        public bool ContainsParameter(string name)
        {
            return this.parameters.ContainsKey(name);
        }

        public void AddParameter(string name, object parameter)
        {
            this.parameters[name] = parameter;
        }

        public void ClearParameters()
        {
            this.parameters.Clear();
        }
    }
}
