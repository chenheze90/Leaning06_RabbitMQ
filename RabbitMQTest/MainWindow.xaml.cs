using RabbitMQ.Client;
using System;
using System.Text;
using System.Windows;

namespace RabbitMQTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            btnSend_Click(null, null);
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
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
                    ////HostName = rabbitMqUrl,
                    ////UserName = "rabbit1",
                    ////Password = "rabbit1",
                    Port = AmqpTcpEndpoint.UseDefaultPort,
                    VirtualHost = "/",
                    //RequestedHeartbeat = new TimeSpan(60),
                    //Ssl = { ServerName = rabbitMqUrl, Enabled = false }                    
                };
                //using (var connection = factory.CreateConnection())
                //using (var channel = connection.CreateModel())
                //{
                //    channel.QueueDeclare(queue: "hello",
                //                         durable: false,
                //                         exclusive: false,
                //                         autoDelete: false,
                //                         arguments: null);

                //    var body = Encoding.UTF8.GetBytes(txbMessage.Text);
                //    channel.BasicPublish(exchange: "",
                //                             routingKey: "hello",
                //                             basicProperties: null,
                //                             body: body);
                //}

                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {

                        channel.ExchangeDeclare("logs", ExchangeType.Direct);


                        string msg = null;
                        while (msg != txbMessage.Text)
                        {
                            msg = txbMessage.Text;
                            var body = Encoding.UTF8.GetBytes(msg);
                            channel.BasicPublish(exchange: "logs", routingKey: "hello", basicProperties: null, body: body);
                        }

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
