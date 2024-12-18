using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Stock.Service.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEventConsumer>
    {
        public async Task Consume(ConsumeContext<OrderCreatedEventConsumer> context)
        {
            // Sade bir şekilde inbox patterne odaklanacağımız için stock işlemlerini mış gibi yapıyoruz.
            await Console.Out.WriteLineAsync(JsonSerializer.Serialize(context.Message));
        }
    }
}
