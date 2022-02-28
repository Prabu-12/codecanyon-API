using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace QuizplusApi.Models.Menu
{
    public class AppMenu
    {
		public int AppMenuId { get; set; }

		[Required]
		[StringLength(100)]
		public string MenuTitle { get; set; }
		[Required]
		[StringLength(500)]
		public string Url { get; set; }

		[Required]
		public int SortOrder { get; set; }
		[Required]
		[StringLength(100)]
		public string IconClass { get; set; }
		[Required]
		public bool IsActive { get; set; }

		[DefaultValue(false)]
		public bool IsMigrationData { get; set; }

		[Required]
		public DateTime DateAdded { get; set; }
		
		[Required]
		public int AddedBy { get; set; }
		public DateTime? LastUpdatedDate { get; set; }
		public int? LastUpdatedBy { get; set; }
	}
}
