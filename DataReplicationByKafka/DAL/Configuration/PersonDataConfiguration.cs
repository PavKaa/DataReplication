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
	internal class PersonDataConfiguration : IEntityTypeConfiguration<PersonData>
	{
		public void Configure(EntityTypeBuilder<PersonData> builder)
		{
			builder.HasOne(pd => pd.Person)
				   .WithOne(p => p.PersonData)
				   .HasForeignKey<PersonData>(pd => pd.PersonId)
				   .IsRequired(true)
				   .OnDelete(DeleteBehavior.Cascade);
		}
	}
}
