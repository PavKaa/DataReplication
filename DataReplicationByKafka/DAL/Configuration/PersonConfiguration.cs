using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configuration
{
	internal class PersonConfiguration : IEntityTypeConfiguration<Person>
	{
		public void Configure(EntityTypeBuilder<Person> builder)
		{
			builder.HasOne(p => p.PersonData)
				   .WithOne(pd => pd.Person);

			builder.HasMany(p => p.Tasks)
				   .WithOne(pt => pt.Person);
		}
	}
}
