using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace QuizplusApi.Models.Menu
{
	public class MenuMapping
    {
		public int MenuMappingId { get; set; }
		[Required]
		public int UserRoleId { get; set; }
		[Required]
		public int AppMenuId { get; set; }
		[Required]
		public bool IsActive { get; set; }
		[DefaultValue(false)]
		public bool IsMigrationData { get; set; }
		[Required]
		public DateTime DateAdded { get; set; }
		[Required]
		public int AddedBy { get; set; }
	}
}
