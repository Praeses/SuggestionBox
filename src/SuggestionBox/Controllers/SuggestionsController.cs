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
    public class SuggestionsController : Controller
    {
        protected readonly string _adminGroup;

        public virtual bool UserIsAdmin
        {
            get { return User.IsInRole(_adminGroup); }
        }

        public SuggestionsController()
        {
            _adminGroup = WebConfigurationManager.AppSettings["AdminGroup"];
        }

        private CommentModel SetupComment(CommentModel model)
        {
            model.CanBeDeleted = UserIsAdmin && model.Status != CommentStatus.Deleted;
            model.CanBeUndeleted = UserIsAdmin && model.Status == CommentStatus.Deleted;

            return model;
        }

        private SuggestionModel ConvertSuggestionToModel(Suggestion suggestion)
        {
            var model = Mapper.Map<Suggestion, SuggestionModel>(suggestion);
            model.CanBeDeleted = UserIsAdmin && model.Status != SuggestionStatus.Deleted;
            model.CanBeApproved = UserIsAdmin && model.Status != SuggestionStatus.Approved;
            model.CanBeCompleted = UserIsAdmin && model.Status != SuggestionStatus.Completed;
            model.CanBeDenied = UserIsAdmin && model.Status != SuggestionStatus.Denied;

            model.Comments = model.Comments
                .Where(c => c.Status != CommentStatus.Deleted || UserIsAdmin)
                .Select(SetupComment)
                .OrderBy(c => c.Date).ToList();

            return model;
        }

        [HttpPost]
        public ActionResult Add(SuggestionModel model)
        {
            var suggestion = Mapper.Map<SuggestionModel, Suggestion>(model);
            suggestion.Date = DateTime.Now;
            suggestion.Status = (int)SuggestionStatus.Active;

            using (var container = new SuggestionBoxContainer())
            {
                container.Suggestions.AddObject(suggestion);
                container.SaveChanges();
                container.AcceptAllChanges();

                SuggestionEmailer.SendEmail(suggestion);

                var suggestions = container.Suggestions
                    .Where(s => s.Status != (int)SuggestionStatus.Deleted || UserIsAdmin)
                    .OrderByDescending(s => s.Id).Select(ConvertSuggestionToModel).ToList();

                return PartialView("Partials/_SuggestionList", suggestions);
            }
        }

        [HttpPost]
        public ActionResult Approve(SuggestionModel model, string commentText)
        {
            using (var container = new SuggestionBoxContainer())
            {
                var suggestion = container.Suggestions.FirstOrDefault(s => s.Id == model.Id);

                if (UserIsAdmin)
                {
                    var comment = new Comment
                                    {
                                        Body = commentText,
                                        Date = DateTime.Now,
                                        Status = (int)CommentStatus.Active
                                    };
                    suggestion.Comments.Add(comment);
                    suggestion.Status = (int)SuggestionStatus.Approved;

                    container.SaveChanges();
                    container.AcceptAllChanges();
                }

                return PartialView("Partials/_View", ConvertSuggestionToModel(suggestion));
            }
        }

        [HttpPost]
        public ActionResult Complete(SuggestionModel model, string commentText)
        {
            using (var container = new SuggestionBoxContainer())
            {
                var suggestion = container.Suggestions.FirstOrDefault(s => s.Id == model.Id);

                if (UserIsAdmin)
                {
                    var comment = new Comment
                                    {
                                        Body = commentText,
                                        Date = DateTime.Now,
                                        Status = (int)CommentStatus.Active
                                    };
                    suggestion.Comments.Add(comment);
                    suggestion.Status = (int)SuggestionStatus.Completed;

                    container.SaveChanges();
                    container.AcceptAllChanges();
                }

                return PartialView("Partials/_View", ConvertSuggestionToModel(suggestion));
            }
        }

        [HttpPost]
        public ActionResult Delete(SuggestionModel model, string commentText)
        {
            using (var container = new SuggestionBoxContainer())
            {
                var suggestion = container.Suggestions.FirstOrDefault(s => s.Id == model.Id);

                if (UserIsAdmin)
                {
                    var comment = new Comment
                                    {
                                        Body = commentText,
                                        Date = DateTime.Now,
                                        Status = (int)CommentStatus.Active
                                    };
                    suggestion.Comments.Add(comment);
                    suggestion.Status = (int)SuggestionStatus.Deleted;

                    container.SaveChanges();
                    container.AcceptAllChanges();
                }

                return PartialView("Partials/_View", ConvertSuggestionToModel(suggestion));
            }
        }

        [HttpPost]
        public ActionResult Deny(SuggestionModel model, string commentText)
        {
            using (var container = new SuggestionBoxContainer())
            {
                var suggestion = container.Suggestions.FirstOrDefault(s => s.Id == model.Id);

                if (UserIsAdmin)
                {
                    var comment = new Comment
                                    {
                                        Body = commentText,
                                        Date = DateTime.Now,
                                        Status = (int)CommentStatus.Active
                                    };
                    suggestion.Comments.Add(comment);
                    suggestion.Status = (int)SuggestionStatus.Denied;

                    container.SaveChanges();
                    container.AcceptAllChanges();
                }

                return PartialView("Partials/_View", ConvertSuggestionToModel(suggestion));
            }
        }

        [HttpGet]
        public ActionResult Index()
        {
            using (var container = new SuggestionBoxContainer())
            {
                var suggestions = container.Suggestions
                    .Where(s => s.Status != (int)SuggestionStatus.Deleted || UserIsAdmin)
                    .OrderByDescending(s => s.Comments.Count == 0 ? s.Date : s.Comments.Max(c => c.Date)).ToList();

                return View(suggestions.Select(ConvertSuggestionToModel).ToList());
            }
        }

        [HttpGet]
        public ActionResult View(int id)
        {
            using (var container = new SuggestionBoxContainer())
            {
                var suggestion = container.Suggestions
                    .Where(s => s.Status != (int)SuggestionStatus.Deleted || UserIsAdmin)
                    .FirstOrDefault(s => s.Id == id);

                return View(ConvertSuggestionToModel(suggestion ?? new Suggestion()));
            }
        }
    }
}
