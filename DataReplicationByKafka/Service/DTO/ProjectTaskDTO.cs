using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
	public class ProjectTaskDTO
	{
		[Required]
		public string Description { get; set; }

		[Required]
		public string TimeStart { get; set; }

		public string? TimeEnd { get; set; }

		[Required]
		public string Email { get; set; }
	}
}
