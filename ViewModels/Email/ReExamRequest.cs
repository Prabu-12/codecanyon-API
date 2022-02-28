using System.Collections.Generic;

namespace QuizplusApi.ViewModels.Email
{
    public class ReExamRequest
    {      
        public string LogoPath { get; set; }
        public string SiteUrl { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<Invitation> emails{ get; set; }
    }
}