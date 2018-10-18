using System.IO;
using System.Text;
using SIS.Framework.ActionResults.Interfaces;

namespace SIS.Framework.Views
{
    public class View : IRenderable
    {
        private readonly string fullyQualifiedTemplateName;

        public View(string fullyQualifiedTemplateName)
        {
            this.fullyQualifiedTemplateName = fullyQualifiedTemplateName;
        }

        private string ReadFile(string fullyQualifiedTemplateName)
        {
            if (!File.Exists(fullyQualifiedTemplateName))
            {
                throw new FileNotFoundException($"View with path {fullyQualifiedTemplateName} does not exist!");
            }

            var content = File.ReadAllText(fullyQualifiedTemplateName, Encoding.UTF8);
            
            return content;
        }

        public string Render()
        {
            var fullHtml = this.ReadFile(this.fullyQualifiedTemplateName);

            return fullHtml;
        }
    }
}
