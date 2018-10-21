using System.Collections.Generic;
using System.IO;
using System.Text;
using SIS.Framework.ActionResults.Interfaces;

namespace SIS.Framework.Views
{
    public class View : IRenderable
    {
        private readonly string fullyQualifiedTemplateName;

        private readonly IDictionary<string, object> viewData;

        public View(string fullyQualifiedTemplateName, IDictionary<string, object> data)
        {
            this.fullyQualifiedTemplateName = fullyQualifiedTemplateName;
            this.viewData = data;
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
            var renderedHtml = this.RenderHtml(fullHtml);

            return renderedHtml;
        }

        private string RenderHtml(string fullHtml)
        {
            var renderedHtml = fullHtml;

            foreach (var parameter in this.viewData)
            {
                renderedHtml = renderedHtml.Replace($"{{{{{{{parameter.Key}}}}}}}", parameter.Value.ToString());
            }

            return renderedHtml;
        }
    }
}
