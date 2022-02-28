using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QuizplusApi.Models.Question
{
    public class QuestionLavel
    {
        public int QuestionLavelId{get;set;}
        [Required]
        [StringLength(100)]
        public string QuestionLavelName{get;set;}
    }
}