using System.ComponentModel.DataAnnotations;

namespace QuizplusApi.Models.Quiz
{
    public class QuizMarkOption
    {
        public int QuizMarkOptionId{get;set;}
        [Required]
        [StringLength(100)]
        public string QuizMarkOptionName{get;set;}
    }
}