using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Receiver
{
	class Consumer
	{
		static void Main(string[] args)
		{
			var factory = new ConnectionFactory { HostName = "localhost" };
			using (var connection = factory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				channel.ExchangeDeclare(exchange: "logs", type: "fanout");

				var queueName = channel.QueueDeclare().QueueName;
				channel.QueueBind(queue: queueName,
								  exchange: "logs",
								  routingKey: "");

				//channel.QueueDeclare(queue: "durable_queue",
				//					 durable: true,
				//					 exclusive: false,
				//					 autoDelete: false,
				//					 arguments: null);
				Console.WriteLine(" [*] Waiting for logs.");

				var consumer = new EventingBasicConsumer(channel);

				consumer.Received += (model, ea) =>
				{
					var body = ea.Body;
					var message = Encoding.UTF8.GetString(body);
					Console.WriteLine(" [x] Received {0}", message);

					//int dots = message.Split('.').Length - 1;
					//Thread.Sleep(dots >= 0 ? dots * 1000 : 0);

					Console.WriteLine(" [x] Done");

					//channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
				};

				channel.BasicConsume(queue: queueName,
									 autoAck: true,
									 consumer: consumer);

				Console.WriteLine(" Press [enter] to exit.");
				Console.ReadLine();
			}
		}
	}
}
