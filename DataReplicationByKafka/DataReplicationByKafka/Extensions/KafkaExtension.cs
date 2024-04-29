using Confluent.Kafka;
using DataReplicationByKafka.HostedService;

namespace DataReplicationByKafka.Extensions
{
	public static class KafkaExtension
	{
		public static IServiceCollection AddKafkaTools(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddSingleton<ProducerConfig>(provider =>
			{
				return new ProducerConfig()
				{
					BootstrapServers = configuration["Kafka:BootstrapServers"],
					Acks = Acks.Leader,
					SecurityProtocol = SecurityProtocol.Plaintext,
					MessageTimeoutMs = 300000,
					CompressionType = CompressionType.Gzip,
					AllowAutoCreateTopics = false
				};
			});

			services.AddSingleton<IProducer<string, string>>(provider =>
			{
				var config = provider.GetRequiredService<ProducerConfig>();

				return new ProducerBuilder<string, string>(config).Build();
			});

			services.AddSingleton<ConsumerConfig>(provider =>
			{
				return new ConsumerConfig
				{
					BootstrapServers = configuration["Kafka:BootstrapServersConsume"],
					GroupId = Guid.NewGuid().ToString("N"),
					AutoOffsetReset = AutoOffsetReset.Latest,
					SecurityProtocol = SecurityProtocol.Plaintext
				};
			});

			services.AddHostedService<PersonConsumer>();
			services.AddHostedService<PersonDataConsumer>();
			services.AddHostedService<ProjectTaskConsumer>();

			return services;
		}
	}
}
