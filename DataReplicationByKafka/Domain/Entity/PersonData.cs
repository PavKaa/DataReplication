using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entity
{
	public class PersonData
	{
        public int Id { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public DateOnly Birthday { get; set; }

        public int PersonId { get; set; }

		[JsonIgnore]
		public Person Person { get; set; }
    }
}
