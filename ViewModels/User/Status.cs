using System;

namespace QuizplusApi.ViewModels.User
{
    public class Status
    {
        public int TotalStudents { get; set; }
        public int TotalQuizes { get; set; }
        public int LiveQuizes { get; set; }
        public int TotalQuestions { get; set; }
    }
}