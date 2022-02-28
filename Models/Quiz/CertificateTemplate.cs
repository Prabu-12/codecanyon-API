using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QuizplusApi.Models.Quiz
{
    public class CertificateTemplate
    {
        public int CertificateTemplateId{get;set;}
        [Required]
        public string Title{get;set;}
        public string Heading{get;set;}
        [Required]
        public string MainText{get;set;}
        public string PublishDate{get;set;}
        public string TopLeftImagePath{get;set;}
        public string TopRightImagePath{get;set;}
        public string BottomMiddleImagePath{get;set;}
        public string BackgroundImagePath{get;set;}
        public string LeftSignatureText{get;set;}
        public string LeftSignatureImagePath{get;set;}
        public string RightSignatureText{get;set;}
        public string RightSignatureImagePath{get;set;}
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