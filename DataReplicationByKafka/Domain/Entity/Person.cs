using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entity
{
	public class Person
	{
        public int Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        [JsonIgnore]
        public PersonData PersonData { get; set; }

		[JsonIgnore]
		public virtual ICollection<ProjectTask> Tasks { get; set; }
    }
}
