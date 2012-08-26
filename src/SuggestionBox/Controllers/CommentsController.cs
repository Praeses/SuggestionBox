using System;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using AutoMapper;
using SuggestionBox.Code;
using SuggestionBox.Data;
using SuggestionBox.Models;

namespace SuggestionBox.Controllers
{
    public class CommentsController : Controller
    {
        protected readonly string AdminGroup;

        public virtual bool UserIsAdmin
        {
            get { return User.IsInRole(AdminGroup); }
        }

        public CommentsController()
        {
            AdminGroup = WebConfigurationManager.AppSettings["AdminGroup"];
        }

        private CommentModel ConvertCommentToModel(Comment comment)
        {
            var model = Mapper.Map<Comment, CommentModel>(comment);
            model.CanBeDeleted = UserIsAdmin && model.Status != CommentStatus.Deleted;
            model.CanBeUndeleted = UserIsAdmin && model.Status == CommentStatus.Deleted;

            return model;
        }

        [HttpPost]
        public ActionResult Add(CommentModel model)
        {
            var comment = new Comment
            {
                Body = model.Body,
                Date = DateTime.Now,
                SuggestionId = model.SuggestionId,
                Status = (int)CommentStatus.Active
            };

            using (var container = new SuggestionBoxContainer())
            {
                var suggestion = container.Suggestions.FirstOrDefault(s => s.Id == model.SuggestionId);
                suggestion.Comments.Add(comment);
                container.SaveChanges();
                container.AcceptAllChanges();

                var comments = suggestion.Comments
                    .Where(c => c.Status != (int)CommentStatus.Deleted || UserIsAdmin)
                    .OrderBy(c => c.Date).ToList();

                return PartialView("Partials/_CommentList",
                                   comments.Select(ConvertCommentToModel).ToList());
            }
        }

        [HttpPost]
        public ActionResult Delete(CommentModel model)
        {
            using (var container = new SuggestionBoxContainer())
            {
                var comment = container.Comments.FirstOrDefault(c => c.Id == model.Id);

                if (UserIsAdmin)
                {
                    comment.Status = (int)CommentStatus.Deleted;

                    container.SaveChanges();
                    container.AcceptAllChanges();
                }

                var comments = comment.Suggestion.Comments
                    .Where(c => c.Status != (int)CommentStatus.Deleted || UserIsAdmin)
                    .OrderBy(c => c.Date)
                    .Select(ConvertCommentToModel).ToList();

                return PartialView("Partials/_CommentList", comments);
            }
        }

        [HttpPost]
        public ActionResult Undelete(CommentModel model)
        {
            using (var container = new SuggestionBoxContainer())
            {
                var comment = container.Comments.FirstOrDefault(c => c.Id == model.Id);

                if (UserIsAdmin)
                {
                    comment.Status = (int)CommentStatus.Active;

                    container.SaveChanges();
                    container.AcceptAllChanges();
                }

                var comments = comment.Suggestion.Comments
                    .Where(c => c.Status != (int)CommentStatus.Deleted || UserIsAdmin)
                    .OrderBy(c => c.Date)
                    .Select(ConvertCommentToModel).ToList();

                return PartialView("Partials/_CommentList", comments);
            }
        }
    }
}
