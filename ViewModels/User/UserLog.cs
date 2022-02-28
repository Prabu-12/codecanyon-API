using System;

namespace QuizplusApi.ViewModels.UserLog
{
    public class UserLog
    {	
		public int Count { get; set; }
		public DateTime Date { get; set; }
		public string Month { get; set; }	
		public string Browser { get; set; }
        public string Platform { get; set; }
        public string QuizTitle { get; set; }
	}
}