using DAL.Configuration;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<Person> Persons { get; set; }

		public DbSet<PersonData> PersonsData { get; set; }

		public DbSet<ProjectTask> Tasks { get; set; }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
			
        }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new PersonConfiguration());
			modelBuilder.ApplyConfiguration(new PersonDataConfiguration());
			modelBuilder.ApplyConfiguration(new ProjectTaskConfiguration());

			base.OnModelCreating(modelBuilder);
		}
	}
}
