using System.ComponentModel.DataAnnotations;

namespace QuizplusApi.ViewModels.Question
{
    public class CsvUploadHelper
    {
        public string Path{get;set;}
        public int QuizTopicId{get;set;}
        public int AddedBy { get; set; }
    }
}