using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QuizplusApi.Models.Question
{
    public class QuestionCategory
    {
        public int QuestionCategoryId{get;set;}
        [Required]
		[StringLength(500)]
        public string QuestionCategoryName{get;set;}
        [Required]
		public bool IsActive { get; set; }
		[DefaultValue(false)]
		public bool IsMigrationData { get; set; }
		[Required]
		public int AddedBy { get; set; }
		[Required]
		public DateTime DateAdded { get; set; }
		public DateTime? LastUpdatedDate { get; set; }
		public int? LastUpdatedBy { get; set; }
    }
}