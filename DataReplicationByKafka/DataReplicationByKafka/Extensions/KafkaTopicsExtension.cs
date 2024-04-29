using Confluent.Kafka.Admin;
using Confluent.Kafka;

namespace DataReplicationByKafka.Extensions
{
	public static class KafkaTopicsExtension
	{
		public static WebApplicationBuilder CreateTopics(this WebApplicationBuilder builder)
		{
			var adminClientConfig = new AdminClientConfig
			{
				BootstrapServers = builder.Configuration["Kafka:BootstrapServers"]
			};

			using (var adminClient = new AdminClientBuilder(adminClientConfig).Build())
			{
				var topics = new List<TopicSpecification>
				{
					new TopicSpecification
					{
						Name = builder.Configuration["Kafka:PersonTopic"],
						NumPartitions = 2,
						ReplicationFactor = 2
					},
					new TopicSpecification
					{
						Name = builder.Configuration["Kafka:PersonDataTopic"],
						NumPartitions = 2,
						ReplicationFactor = 2
					},
					new TopicSpecification
					{
						Name = builder.Configuration["Kafka:ProjectTaskTopic"],
						NumPartitions = 2,
						ReplicationFactor = 2
					}
				};

				try
				{
					adminClient.CreateTopicsAsync(topics).Wait();
				}
				catch (CreateTopicsException ex)
				{
					foreach (var result in ex.Results)
					{
						if (result.Error.Code != ErrorCode.TopicAlreadyExists)
						{
                            Console.WriteLine($"Error creating topic {result.Topic}: {result.Error.Reason}");
						}
					}
				}
				catch(Exception ex)
				{
					Console.WriteLine($"Error while attemp to create topics: {ex.Message}");
				}
			}

			return builder;
		}
	}
}
