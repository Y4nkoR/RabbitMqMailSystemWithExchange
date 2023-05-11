using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQSharedClasses.Interfaces
{
    public interface IRabbitMQService
    {
        public IConnection GetConnection();
        public string GetMailExchangeName();
    }
}
