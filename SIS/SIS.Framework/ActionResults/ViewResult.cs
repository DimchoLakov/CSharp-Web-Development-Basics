using SIS.Framework.ActionResults.Interfaces;

namespace SIS.Framework.ActionResults
{
    public class ViewResult : IViewable
    {
        public ViewResult(IRenderable view)
        {
            this.View = view;
        }

        public string Invoke()
        {
            return this.View.Render();
        }

        public IRenderable View { get; set; }
    }
}
