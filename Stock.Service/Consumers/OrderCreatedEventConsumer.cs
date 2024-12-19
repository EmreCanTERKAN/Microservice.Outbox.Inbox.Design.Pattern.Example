using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Events;
using Stock.Service.Models.Contexts;
using Stock.Service.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Stock.Service.Consumers
{
    public class OrderCreatedEventConsumer(StockDbContext stockDbContext) : IConsumer<OrderCreatedEvent>
    {
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            await stockDbContext.OrderInBoxes.AddAsync(new()
            {
                Processed = false,
                Payload = JsonSerializer.Serialize(context.Message)
            });
            await stockDbContext.SaveChangesAsync();
            await Console.Out.WriteLineAsync(JsonSerializer.Serialize(context.Message));

            List<OrderInbox> orderInboxes = await stockDbContext.OrderInBoxes.Where(i => i.Processed == false).ToListAsync();

            foreach (var orderInbox in orderInboxes)
            {
                OrderCreatedEvent orderCreatedEvent = JsonSerializer.Deserialize<OrderCreatedEvent>(orderInbox.Payload);
                await Console.Out.WriteAsync($"{orderCreatedEvent.OrderId} order id değerine karşılık olan siparişin stok işlmeleri başarıyla tamamlanmıştır.");
                orderInbox.Processed = true;
                await stockDbContext.SaveChangesAsync();
            }
        }
    }
}
