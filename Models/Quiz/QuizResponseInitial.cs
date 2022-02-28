using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizplusApi.Models.Quiz
{
    public class QuizResponseInitial
    {
        public int QuizResponseInitialId{get;set;}
        [Required]
        public int UserId{get;set;}
        [Required]
		[StringLength(100)]
		public string Email { get; set; }
        [Required]
        public int AttemptCount{get;set;}
        [Required]
        public bool IsExamined{get;set;}
        [Required]
        public int QuizTopicId{get;set;}
        [Required]
        public string QuizTitle{get;set;}
        [DefaultValue(0)]
        [Column(TypeName = "decimal(5, 2)")]
        public decimal QuizMark{get;set;}
        [Column(TypeName = "decimal(5, 2)")]
        [DefaultValue(0)]
        public decimal QuizPassMarks{get;set;}
        [DefaultValue(0)]
        [Column(TypeName = "decimal(5, 2)")]
        public decimal UserObtainedQuizMark{get;set;}
        [Column(TypeName = "decimal(5, 2)")]
        [DefaultValue(0)]
        public double QuizTime{get;set;}
        [Column(TypeName = "decimal(5, 2)")]
        public double? TimeTaken{get;set;}
        [Required]
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
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