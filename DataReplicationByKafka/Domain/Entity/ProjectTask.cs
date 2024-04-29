using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entity
{
	public class ProjectTask
	{
        public int Id { get; set; }

        public string Description { get; set; }

        public DateOnly TimeStart { get; set; }

        public DateOnly? TimeEnd { get; set; }

        public int PersonId { get; set; }

		[JsonIgnore]
		public Person Person { get; set; }
    }
}
