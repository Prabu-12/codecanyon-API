namespace QuizplusApi.ViewModels.Email
{
    public class ForgetPassword
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string LogoPath { get; set; }
        public string SiteUrl { get; set; }
        public string Body { get; set; }
    }
}