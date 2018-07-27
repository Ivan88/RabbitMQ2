using System;
using System.Text;
using RabbitMQ.Client;

namespace Sender
{
	class Program
	{
		static void Main(string[] args)
		{
			var factory = new ConnectionFactory { HostName = "localhost" };
			using (var connection = factory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				channel.QueueDeclare(queue: "durable_queue",
									 durable: true,
									 exclusive: false,
									 autoDelete: false,
									 arguments: null);

				do
				{
					Console.WriteLine(" [x] to exit press 'x' and enter");
					Console.WriteLine("     or type message and press enter");
					string line = Console.ReadLine();

					if (line == "x")
						break;

					var properties = channel.CreateBasicProperties();
					properties.Persistent = true;

					var body = Encoding.UTF8.GetBytes(line);

					channel.BasicPublish(exchange: "",
										 routingKey: "durable_queue",
										 basicProperties: properties,
										 body: body);

					Console.WriteLine(" [x] Sent {0}", line);
				}
				while (true);
			}

			Console.WriteLine(" Press [enter] to exit.");
			Console.ReadLine();
		}
	}
}
