namespace QuizplusApi.ViewModels.Helper
{
    public class LogInResponse
    {
        public string Token { get; set; }
        public object Obj{get;set;}
    }
    public class Confirmation
    {
        public string Status { get; set; }
        public string ResponseMsg { get; set; }
    }
}