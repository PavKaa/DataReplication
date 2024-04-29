using Confluent.Kafka;
using Newtonsoft.Json;
using Service.DTO;
using Service.Interface;

namespace DataReplicationByKafka.HostedService
{
	public class PersonDataConsumer : BackgroundService
	{
		private readonly ConsumerConfig _config;
		private readonly IServiceScopeFactory _serviceScopeFactory;
		private readonly IConfiguration _configuration;

		public PersonDataConsumer(ConsumerConfig config, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
		{
			_config = config;
			_serviceScopeFactory = serviceScopeFactory;
			_configuration = configuration;
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			Task.Run(() => StartAsync(stoppingToken));

			return Task.CompletedTask;
		}

		private async Task StartAsync(CancellationToken stoppingToken)
		{
			using (var scope = _serviceScopeFactory.CreateScope())
			using (var consumer = new ConsumerBuilder<string, string>(_config).Build())
			{
				consumer.Assign(new TopicPartition(_configuration["Kafka:PersonDataTopic"], int.Parse(_configuration["Kafka:PartitionToConsume"])));

				var service = scope.ServiceProvider.GetRequiredService<IPersonDataService>();

				while (!stoppingToken.IsCancellationRequested)
				{
					var message = consumer.Consume();

					Console.WriteLine($"Consumed message: {message.Message.Key} : {message.Message.Value} at {message.Topic}");

					var model = JsonConvert.DeserializeObject<PersonDataDTO>(message.Message.Value);

					if (model != null)
					{
						await service.Create(model);
					}
				}
			}
		}
	}
}
