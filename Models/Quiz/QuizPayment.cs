using System;
using System.ComponentModel.DataAnnotations;

namespace QuizplusApi.Models.Quiz
{
    public class QuizPayment
    {
        public int QuizPaymentId{get;set;}
        [Required]
        public int QuizTopicId{get;set;}
        [Required]
        [StringLength(100)]
        public string Email {get; set;}
        [Required]
        [StringLength(50)]
        public string Currency{get;set;}
        [Required]
        [StringLength(200)]
        public string SessionId{get;set;}
        [Required]
		public int AddedBy {get; set;}
		[Required]
		public DateTime DateAdded { get; set; }
    }
}