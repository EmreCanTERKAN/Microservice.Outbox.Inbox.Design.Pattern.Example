using MassTransit;
using Order.Outbox.Table.Publisher.Entities;
using Quartz;
using Shared.Events;
using System.Text.Json;

namespace Order.Outbox.Table.Publisher.Jobs
{
    public class OrderOutboxPublishJob(IPublishEndpoint publishEndpoint) : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            //Veri tabanının statenin uygun olup olmadığını kontrol ediyoruz.
            if (OrderOutboxSingletonDatabase.DataReaderState)
            {
                OrderOutboxSingletonDatabase.DataReadyBusy();
                List<OrderOutBox> orderOutBoxes = (await OrderOutboxSingletonDatabase.QueryAsync<OrderOutBox>($@"SELECT * FROM OrderOutboxes WHERE ProcessedDate IS NULL ORDER BY OCCUREDON ASC")).ToList();

                foreach (var orderOutBox in orderOutBoxes)
                {
                    if (orderOutBox.Type == nameof(OrderCreatedEvent))
                    {
                        OrderCreatedEvent orderCreatedEvent = JsonSerializer.Deserialize<OrderCreatedEvent>(orderOutBox.Payload);
                        if (orderCreatedEvent is not null)
                        {
                            await publishEndpoint.Publish(orderCreatedEvent);
                            await OrderOutboxSingletonDatabase.ExecuteAsync($"UPDATE ORDEROUTBOXES SET ProcessedDate = GETDATE() WHERE ID = '{orderOutBox.Id}'");
                        }
                    }
                }
            }
            OrderOutboxSingletonDatabase.DataReaderReady();
            await Console.Out.WriteLineAsync("Order outbox table checked!");
        }
    }
}
