using System;
using SuggestionBox.Code;

namespace SuggestionBox.Models
{
	public class CommentModel
	{
		public virtual string Body { get; set; }
		public virtual bool CanBeDeleted { get; set; }
		public virtual bool CanBeUndeleted { get; set; }
		public virtual DateTime Date { get; set; }
		public virtual int Id { get; set; }
		public virtual CommentStatus Status { get; set; }
		public virtual int SuggestionId { get; set; }
	}
}