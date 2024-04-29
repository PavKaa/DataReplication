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
	internal class ProjectTaskConfiguration : IEntityTypeConfiguration<ProjectTask>
	{
		public void Configure(EntityTypeBuilder<ProjectTask> builder)
		{
			builder.HasOne(pt => pt.Person)
				   .WithMany(p => p.Tasks)
				   .HasForeignKey(pt => pt.PersonId)
				   .IsRequired(true)
				   .OnDelete(DeleteBehavior.Cascade);
		}
	}
}
