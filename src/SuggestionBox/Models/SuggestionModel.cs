using System;
using System.Collections.Generic;
using SuggestionBox.Code;

namespace SuggestionBox.Models
{
	public class SuggestionModel
	{
		public virtual string Body { get; set; }
		public virtual bool CanBeApproved { get; set; }
		public virtual bool CanBeCompleted { get; set; }
		public virtual bool CanBeDeleted { get; set; }
		public virtual bool CanBeDenied { get; set; }
		public virtual IList<CommentModel> Comments { get; set; }
		public virtual DateTime Date { get; set; }
		public virtual int Id { get; set; }
		public virtual SuggestionStatus Status { get; set; }
		public virtual string Title { get; set; }
	}
}