using MassTransit;
using Order.Outbox.Table.Publisher.Jobs;
using Quartz;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMassTransit(configurator =>
{
    configurator.UsingRabbitMq((context, _configure) =>
    {
        _configure.Host(builder.Configuration["RabbitMQ"]);
    });
});

builder.Services.AddQuartz(configurator =>
{
    //jobkey olu�turuyoruz.
    JobKey jobKey = new("OrderOutboxPublishJob");
    // Jobu tan�ml�yoruz.
    configurator.AddJob<OrderOutboxPublishJob>(options => options.WithIdentity(jobKey));

    //withidentity metodu bir trigger key istedi�i i�in trigger key tan�mlad�.
    TriggerKey triggerKey = new("OrderOutboxPublishTrigger");
    
    //Bu job bu trigger ile ili�kilendirildi.
    //WithSimpleSchedule ile ne kadar zamanda tekrarlanaca��n� ve d�ng�s�n� belirledik.
    //Burada her 5 saniyede bir ve sonsuza kadar tekrar edece�ini ayarlad�k..
    configurator.AddTrigger(options => options
    .ForJob(jobKey)
    .WithIdentity(triggerKey)
    .StartAt(DateTime.UtcNow)
    .WithSimpleSchedule(builder => builder
                         .WithIntervalInSeconds(5)
                         .RepeatForever()));

});
//Burada ise yukarda tan�mlanan quartz�n host olaca��n� tan�ml�yoruz.
builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);




var host = builder.Build();
host.Run();
