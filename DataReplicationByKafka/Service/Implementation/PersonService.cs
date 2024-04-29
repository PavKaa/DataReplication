using DAL;
using Domain.Entity;
using Domain.Response;
using Microsoft.EntityFrameworkCore;
using Service.DTO;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementation
{
	public class PersonService : IPersonService
	{
		private readonly ApplicationDbContext _context;

		public PersonService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<BaseResponse<Person>> Create(PersonDTO model)
		{
			var response = new BaseResponse<Person>();

			try
			{
				var persons = await _context.Persons.Where(p => p.Email == model.Email).FirstOrDefaultAsync();

				if(persons != null)
				{
					response.Message = "Person with current email already exists!";
					response.StatusCode = HttpStatusCode.BadRequest;

					return response;
				}

				using var pdkf2 = new Rfc2898DeriveBytes(model.Password, new byte[0], 1000, HashAlgorithmName.SHA256);

				var person = new Person
				{
					Email = model.Email,
					Password = Convert.ToBase64String(pdkf2.GetBytes(32))
				};

                await Console.Out.WriteLineAsync("Entity created");

                await _context.Persons.AddAsync(person);
				await _context.SaveChangesAsync();

				await Console.Out.WriteLineAsync("Entity saved to db");

				response.Data = person;
				response.StatusCode = HttpStatusCode.OK;

				return response;
			}
			catch (Exception ex)
			{
				response.Message = "Internal server error";
				response.StatusCode = HttpStatusCode.InternalServerError;

				return response;
			}
		}

		public async Task<BaseResponse<List<Person>>> GetAll()
		{
			var response = new BaseResponse<List<Person>>();

			try
			{
				response.Data = await _context.Persons.ToListAsync();
				response.StatusCode = HttpStatusCode.OK;

				return response;
			}
			catch (Exception ex)
			{
				response.Message = "Internal server error";
				response.StatusCode = HttpStatusCode.InternalServerError;

				return response;
			}
		}
	}
}
