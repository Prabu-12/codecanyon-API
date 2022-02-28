using System.ComponentModel.DataAnnotations;

namespace QuizplusApi.Models.Quiz
{
    public class ReportType
    {
        public int ReportTypeId{get;set;}
        [Required]
        [StringLength(200)]
        public string ReportTypeName{get;set;}
    }
}