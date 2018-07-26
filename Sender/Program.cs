using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

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
				channel.QueueDeclare(queue: "hello Ivan",
									 durable: false,
									 exclusive: false,
									 autoDelete: false,
									 arguments: null);

				do
				{
					Console.WriteLine("to exit press 'x' and enter");
					Console.WriteLine("or type message and press enter");
					string line = Console.ReadLine();

					if (line == "x")
						break;

					var body = Encoding.UTF8.GetBytes(line);

					channel.BasicPublish(exchange: "",
										 routingKey: "hello ivan",
										 basicProperties: null,
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
