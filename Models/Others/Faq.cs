using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QuizplusApi.Models.Others
{
    public class Faq
    {
        public int FaqId{get;set;}
		[Required]
		[StringLength(1000)]
        public string Title{get;set;}
		[Required]
        public string Description{get;set;}
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