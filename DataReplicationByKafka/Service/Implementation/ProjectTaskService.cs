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
	public class ProjectTaskService : IProjectTaskService
	{
		private readonly ApplicationDbContext _context;

		public ProjectTaskService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<BaseResponse<ProjectTask>> Create(ProjectTaskDTO model)
		{
			var response = new BaseResponse<ProjectTask>();

			try
			{
				var person = await _context.Persons.Where(p => p.Email == model.Email).FirstOrDefaultAsync();

				if (person == null)
				{
					response.Message = "Person with current email does not exist!";
					response.StatusCode = HttpStatusCode.BadRequest;

					return response;
				}

				var culture = new CultureInfo("ru-RU");

				if (DateOnly.TryParse(model.TimeStart, culture, DateTimeStyles.None, out var timeStart))
				{
					var projectTask = new ProjectTask
					{
						Description = model.Description,
						TimeStart = timeStart,
						PersonId = person.Id
					};

					if(model.TimeEnd != null && DateOnly.TryParse(model.TimeEnd, culture, DateTimeStyles.None, out var timeEnd))
					{
						projectTask.TimeEnd = timeEnd;
					}

					await _context.Tasks.AddAsync(projectTask);
					await _context.SaveChangesAsync();

					response.Data = projectTask;
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

		public async Task<BaseResponse<List<ProjectTask>>> GetAll()
		{
			var response = new BaseResponse<List<ProjectTask>>();

			try
			{
				response.Data = await _context.Tasks.ToListAsync();
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
