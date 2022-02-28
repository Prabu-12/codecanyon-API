using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizplusApi.Models.Question
{
    public class QuizQuestion
    {
        public int QuizQuestionId{get;set;}
        [Required]
        public int QuizTopicId{get;set;}
        [Required]
        public string QuestionDetail{get;set;}
        [Required]
        [DefaultValue(0)]
        public int SerialNo{get;set;}
        [DefaultValue(0)]
        [Column(TypeName = "decimal(5, 2)")]
        public decimal PerQuestionMark{get;set;}
        [Required]
        public int QuestionTypeId{get;set;}
        [Required]
        public int QuestionLavelId{get;set;}
        [Required]
        public int QuestionCategoryId{get;set;}
        public string OptionA{get;set;}
        public string OptionB{get;set;}
        public string OptionC{get;set;}
        public string OptionD{get;set;}
        public string OptionE{get;set;}
        public string CorrectOption{get;set;}
        public string AnswerExplanation{get;set;}
        public string ImagePath{get;set;}
        public string VideoPath{get;set;}
        [DefaultValue(false)]
        public bool IsCodeSnippet{get;set;}
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