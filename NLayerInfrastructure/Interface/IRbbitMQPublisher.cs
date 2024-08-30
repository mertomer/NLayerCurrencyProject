using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayerCore.Interfaces
{
    public interface IRabbitMQPublisher
    {
        void Publish(string message);
    }
}
