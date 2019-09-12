using System;
using RabbitMQ.Client;
using System.Text;
using System.Threading;

class Program
{
    public static void Main()
    {
        Timer t = new Timer(TimerCallback, null, 0, 5000);
        Console.ReadLine();        
    }

    private static void TimerCallback(Object o)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

            Random random = new Random();
            int num = random.Next();

            string message = "Hello World! - SEND "+DateTime.Now.ToString()+" - ID:"+ num.ToString();
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "", routingKey: "hello", basicProperties: null, body: body);
            Console.WriteLine(" Sent {0}", message);
        }

        Console.WriteLine(" Press enter to exit.");
        Console.ReadLine();
    }
}
