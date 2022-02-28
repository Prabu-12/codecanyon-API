using System.ComponentModel.DataAnnotations;

namespace QuizplusApi.Models.Quiz
{
    public class QuizParticipantOption
    {
        public int QuizParticipantOptionId{get;set;}
        [Required]
        [StringLength(100)]
        public string QuizParticipantOptionName{get;set;}
    }
}