using System.ComponentModel.DataAnnotations;

namespace QuizplusApi.ViewModels.Question
{
    public class QuestionUpload
    {
        [Required]
        public string QuestionDetail{get;set;}
        [Required]
        public decimal PerQuestionMark{get;set;}
        [Required]
        public int QuestionTypeId{get;set;}
        [Required]
        public int QuestionLavelId{get;set;}
        [Required]
        public int QuestionCategoryId{get;set;}
        public string OptionA{get;set;}
        public string OptionB{get;set;}
        public string OptionC{get;set;}
        public string OptionD{get;set;}
        public string OptionE{get;set;}
        public string CorrectOption{get;set;}
        public string AnswerExplanation{get;set;}
        
    }
}