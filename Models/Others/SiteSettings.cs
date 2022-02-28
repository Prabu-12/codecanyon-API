using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QuizplusApi.Models.Others
{
    public class SiteSettings
    {
        public int SiteSettingsId{get;set;}
        [Required]
        [StringLength(200)]
        public string SiteTitle{get;set;}
        [Required]
        [StringLength(1000)]
        public string WelComeMessage{get;set;}
        [Required]
        [StringLength(500)]
        public string CopyRightText{get;set;}
        [StringLength(2000)]
        public string LogoPath{get;set;}
        [StringLength(2000)]
        public string FaviconPath{get;set;}
        [StringLength(200)]
        public string AppBarColor{get;set;}
        [StringLength(200)]
        public string FooterColor{get;set;}
        [StringLength(200)]
        public string BodyColor{get;set;}
        [DefaultValue(true)]
		public bool AllowWelcomeEmail { get; set; }
        [DefaultValue(true)]
		public bool AllowFaq { get; set; }
        [DefaultValue(true)]
		public bool AllowRightClick { get; set; }
        [DefaultValue(false)]
		public bool EndExam { get; set; }
        [DefaultValue(true)]
		public bool LogoOnExamPage { get; set; }
        [DefaultValue(true)]
		public bool PaidRegistration { get; set; }
        public int? RegistrationPrice{get;set;}
        [StringLength(50)]
        public string Currency{get;set;}
        [StringLength(50)]
        public string CurrencySymbol{get;set;}
        [StringLength(500)]
        public string StripePubKey{get;set;}
        [StringLength(500)]
        public string StripeSecretKey{get;set;}
        [StringLength(200)]
        public string ClientUrl{get;set;}

        [StringLength(200)]
        public string DefaultEmail{get;set;}
        [StringLength(200)]
        public string DisplayName{get;set;}
        [StringLength(200)]
        public string Password{get;set;}
        [StringLength(100)]
        public string Host{get;set;}
        public int Port{get;set;}
        [Required]
		public bool IsActive { get; set; }
		[DefaultValue(false)]
		public bool IsMigrationData { get; set; }
		public int AddedBy { get; set; }
		public DateTime DateAdded { get; set; }
		public DateTime? LastUpdatedDate { get; set; }
		public int? LastUpdatedBy { get; set; }
    }
}