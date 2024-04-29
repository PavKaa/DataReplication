using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.DTO;
using Service.Implementation;
using Service.Interface;
using System.Net;
using System.Text.Json.Serialization;

namespace DataReplicationByKafka.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProjectTaskController : Controller
	{
		private readonly IProjectTaskService _service;
		private readonly IProducer<string, string> _producer;
		private readonly IConfiguration _configuration;

		public ProjectTaskController(IProjectTaskService service, IProducer<string, string> producer, IConfiguration configuration)
		{
			_service = service;
			_producer = producer;
			_configuration = configuration;
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] ProjectTaskDTO model)
		{
			if (ModelState.IsValid)
			{
				var response = await _service.Create(model);

				if (response.StatusCode == HttpStatusCode.OK)
				{
					var jsonString = JsonConvert.SerializeObject(model);

					var topicPartition = new TopicPartition(_configuration["Kafka:ProjectTaskTopic"], int.Parse(_configuration["Kafka:PartitionToProduce"]));

					_producer.Produce(topicPartition, new Message<string, string>
					{
						Key = Guid.NewGuid().ToString("N"),
						Value = jsonString
					});

					return Created(Url.Action("GetAll"), response.Data);
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
				return Ok(response.Data);
			}

			return StatusCode((int)response.StatusCode);
		}
	}
}
