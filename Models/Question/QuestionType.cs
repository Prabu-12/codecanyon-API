using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QuizplusApi.Models.Question
{
    public class QuestionType
    {
        public int QuestionTypeId{get;set;}
        [Required]
        [StringLength(100)]
        public string QuestionTypeName{get;set;}
    }
}