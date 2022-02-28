using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizplusApi.Models.Quiz
{
    public class QuizResponseDetail
    {
        public long QuizResponseDetailId{get;set;}
        [Required]
        public int QuizResponseInitialId{get;set;}
        [Required]
        public int QuizQuestionId{get;set;}
        [Required]
        public string QuestionDetail{get;set;}
        public string UserAnswer{get;set;}
        [DefaultValue(false)]
		public bool IsAnswerSkipped { get; set; }
        public string CorrectAnswer{get;set;}
        public string AnswerExplanation{get;set;}
        [DefaultValue(0)]
        [Column(TypeName = "decimal(5, 2)")]
        public decimal QuestionMark{get;set;}
        [DefaultValue(0)]
        [Column(TypeName = "decimal(5, 2)")]
        public decimal UserObtainedQuestionMark{get;set;}
        public string ImagePath{get;set;}
        public string VideoPath{get;set;}
        [Required]
        [DefaultValue(false)]
        public bool IsExamined{get;set;}

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