using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizplusApi.Models.User
{
    public class LogHistory
    {
		public long LogHistoryId { get; set; }
		[StringLength(500)]		
		public string LogCode { get; set; }	
		public DateTime LogDate { get; set; }		
		public int UserId { get; set; }	
		public DateTime LogInTime { get; set; }
		public DateTime? LogOutTime { get; set; }
		[StringLength(50)]
		public string Ip { get; set; }
		[StringLength(50)]
		public string Browser { get; set; }
		[StringLength(50)]
		public string BrowserVersion { get; set; }
		[StringLength(50)]
		public string Platform { get; set; }
	}
}
