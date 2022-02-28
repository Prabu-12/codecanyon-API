using System.ComponentModel.DataAnnotations;

namespace QuizplusApi.Models.Quiz
{
    public class QuizParticipant
    {
        public int QuizParticipantId{get;set;}
        [StringLength(100)]
        public string Email{get;set;}
        public int QuizTopicId{get;set;}
    }
}