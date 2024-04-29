using DAL;
using Domain.Entity;
using Domain.Response;
using Microsoft.EntityFrameworkCore;
using Service.DTO;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementation
{
	public class PersonDataService : IPersonDataService
	{
		private readonly ApplicationDbContext _context;

		public PersonDataService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<BaseResponse<PersonData>> Create(PersonDataDTO model)
		{
			var response = new BaseResponse<PersonData>();

			try
			{
				var person = await _context.Persons.Where(p => p.Email == model.Email).FirstOrDefaultAsync();

				if(person == null)
				{
					response.Message = "Person with current email does not exist!";
					response.StatusCode = HttpStatusCode.BadRequest;

					return response;
				}

				var culture = new CultureInfo("ru-RU");

				if (DateOnly.TryParse(model.Birthday, culture, DateTimeStyles.None, out var result))
				{
                    var personData = new PersonData
					{
						Firstname = model.Firstname,
						Lastname = model.Lastname,
						Birthday = result,
						PersonId = person.Id
					};

					await _context.PersonsData.AddAsync(personData);
					await _context.SaveChangesAsync();

					response.Data = personData;
					response.StatusCode = HttpStatusCode.OK;

					return response;
				}

				response.Message = "Uncorrect date format!";
				response.StatusCode = HttpStatusCode.BadRequest;
				return response;

			}
			catch (Exception ex)
			{
				response.Message = "Internal server error";
				response.StatusCode = HttpStatusCode.InternalServerError;

				return response;
			}
		}

		public async Task<BaseResponse<List<PersonData>>> GetAll()
		{
			var response = new BaseResponse<List<PersonData>>();

			try
			{
				response.Data = await _context.PersonsData.ToListAsync();
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
