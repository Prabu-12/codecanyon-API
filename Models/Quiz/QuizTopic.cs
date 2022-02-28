using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizplusApi.Models.Quiz
{
    public class QuizTopic
    {
        public int QuizTopicId{get;set;}
        [Required]
        [StringLength(1000)]
        public string QuizTitle{get;set;}
        [Column(TypeName = "decimal(5, 2)")]
        [DefaultValue(0)]
        public decimal QuizTime{get;set;}
        [Column(TypeName = "decimal(5, 2)")]
        public decimal QuizTotalMarks{get;set;}
        [Column(TypeName = "decimal(5, 2)")]
        [DefaultValue(0)]
        public decimal QuizPassMarks{get;set;}
        [Required]
        public int QuizMarkOptionId{get;set;}
        [Required]
        public int QuizParticipantOptionId{get;set;}
        public int? CertificateTemplateId{get;set;}
        [DefaultValue(false)]
        public bool AllowMultipleInputByUser{get;set;}
        [DefaultValue(false)]
        public bool AllowMultipleAnswer{get;set;}
        [DefaultValue(false)]
        public bool AllowMultipleAttempt{get;set;}
        [DefaultValue(false)]
        public bool AllowCorrectOption{get;set;}
        public DateTime? QuizscheduleStartTime { get; set; }
        public DateTime? QuizscheduleEndTime { get; set; }
        [DefaultValue(0)]
        public int QuizPrice{get;set;}
        [Required]
        [DefaultValue(false)]
		public bool IsRunning { get; set; }
        [StringLength(100)]
        public string Categories{ get; set; }
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