using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Windows;

namespace RabbitMQServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string rabbitMqUrl = "10.0.12.180";
                var factory = new ConnectionFactory()
                {
                    HostName = "10.0.12.180",
                    //Port = 15672,
                    UserName = "rabbit1",
                    Password = "rabbit1",
                    //HostName = rabbitMqUrl,
                    //UserName = "rabbit1",
                    //Password = "rabbit1",
                    Port = AmqpTcpEndpoint.UseDefaultPort,
                    VirtualHost = "/",
                    //RequestedHeartbeat = new TimeSpan(60),
                    //Ssl = { ServerName = rabbitMqUrl, Enabled = false }
                };

                //Uri uri = new Uri("amqp://dan:dan@localhost:5672/");
                //factory.Uri = uri;


                //using (var connection = factory.CreateConnection())
                //using (var channel = connection.CreateModel())
                //{
                //    channel.QueueDeclare(queue: "hello",
                //        durable: false,
                //        exclusive: false,
                //        autoDelete: false,
                //        arguments: null);

                //    // 如果接收一个消息，但是没有应答，则客户端不会收到下一个消息
                //    channel.BasicQos(0, 1, false);

                //    var consumer = new EventingBasicConsumer(channel);
                //    consumer.Received += (model, ea) =>
                //    {
                //        var body = ea.Body;
                //        var message = Encoding.UTF8.GetString(body.ToArray());
                //        tbReceive.Text += message + "\r\n";
                //    };
                //    channel.BasicConsume(queue: "hello",
                //        autoAck: true,
                //        consumer: consumer);
                //}





                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare("logs", ExchangeType.Direct);


                        var queueName = channel.QueueDeclare().QueueName;
                        channel.QueueBind(queueName, "logs", "ols");

                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, args) =>
                        {
                            byte[] body = args.Body.ToArray();
                            var msg = Encoding.UTF8.GetString(body);
                            tbReceive.Text += msg + "\r\n";

                            //手动发送ack,必须在同一个channel里发送
                            channel.BasicAck(args.DeliveryTag, false);
                        };
                        channel.BasicConsume(queueName, false, consumer);
                    }
                }




            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
