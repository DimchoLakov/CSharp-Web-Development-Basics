using SIS.Framework.ActionResults.Interfaces;

namespace SIS.Framework.ActionResults
{
    public class RedirectResult : IRedirectable
    {
        public RedirectResult(string redirectUrl)
        {
            this.RedirectUrl = redirectUrl;
        }

        public string Invoke()
        {
            return this.RedirectUrl;
        }

        public string RedirectUrl { get; }
    }
}
