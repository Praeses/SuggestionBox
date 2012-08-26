using System.Web.Mvc;
using MarkdownSharp;

namespace SuggestionBox.Controllers
{
    public class MarkdownController : Controller
    {
        //
        // POST: /Markdown/Transform
        [HttpPost]
        public ActionResult Transform(string text)
        {
            var markdown = new Markdown();

            return Content(markdown.Transform(text));
        }
    }
}
