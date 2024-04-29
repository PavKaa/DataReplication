using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.DTO;
using Service.Interface;
using System.Net;

namespace DataReplicationByKafka.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PersonDataController : Controller
	{
		private readonly IPersonDataService _service;
		private readonly IProducer<string, string> _producer;
		private readonly IConfiguration _configuration;

		public PersonDataController(IPersonDataService service, IProducer<string, string> producer, IConfiguration configuration)
		{
			_service = service;
			_producer = producer;
			_configuration = configuration;
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] PersonDataDTO model)
		{
			if (ModelState.IsValid)
			{
				 var response = await _service.Create(model);

				if (response.StatusCode == HttpStatusCode.OK)
				{
					var jsonString = JsonConvert.SerializeObject(model);

					var topicPartition = new TopicPartition(_configuration["Kafka:PersonDataTopic"], int.Parse(_configuration["Kafka:PartitionToProduce"]));

					_producer.Produce(topicPartition, new Message<string, string>
					{
						Key = Guid.NewGuid().ToString("N"),
						Value = jsonString
					});

					return Created(Url.Action("GetAll"), new
					{
						Id = response.Data.Id,
						Firstname = response.Data.Firstname,
						Lastname = response.Data.Lastname,
						Age = (DateTime.Now.Year - response.Data.Birthday.Year) - (DateTime.Now.DayOfYear < response.Data.Birthday.DayOfYear ? 1 : 0)
					});
				}

				return StatusCode((int)response.StatusCode, response.Message);
			}

			return BadRequest("Invalid data");
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var response = await _service.GetAll();

			if (response.StatusCode == HttpStatusCode.OK)
			{
				return Ok(response.Data.Select(pd => new
				{
					Id = pd.Id,
					Firstname = pd.Firstname,
					Lastname = pd.Lastname,
					Age = (DateTime.Now.Year - pd.Birthday.Year) - (DateTime.Now.DayOfYear < pd.Birthday.DayOfYear ? 1 : 0),
					PersonId = pd.PersonId
				}));
			}

			return StatusCode((int)response.StatusCode);
		}
	}
}
