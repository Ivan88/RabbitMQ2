using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Receiver
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

				string message = "Helo ivan";

				var body = Encoding.UTF8.GetBytes(message);

				channel.BasicPublish(exchange: "",
									 routingKey: "hello ivan",
									 basicProperties: null,
									 body: body);

				Console.WriteLine(" [x] Sent {0}", message);
			}

			Console.WriteLine("Enter to exit.");
			Console.ReadLine();
		}
	}
}
