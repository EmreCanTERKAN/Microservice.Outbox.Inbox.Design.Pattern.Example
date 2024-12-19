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
    //jobkey oluþturuyoruz.
    JobKey jobKey = new("OrderOutboxPublishJob");
    // Jobu tanýmlýyoruz.
    configurator.AddJob<OrderOutboxPublishJob>(options => options.WithIdentity(jobKey));

    //withidentity metodu bir trigger key istediði için trigger key tanýmladý.
    TriggerKey triggerKey = new("OrderOutboxPublishTrigger");
    
    //Bu job bu trigger ile iliþkilendirildi.
    //WithSimpleSchedule ile ne kadar zamanda tekrarlanacaðýný ve döngüsünü belirledik.
    //Burada her 5 saniyede bir ve sonsuza kadar tekrar edeceðini ayarladýk..
    configurator.AddTrigger(options => options
    .ForJob(jobKey)
    .WithIdentity(triggerKey)
    .StartAt(DateTime.UtcNow)
    .WithSimpleSchedule(builder => builder
                         .WithIntervalInSeconds(5)
                         .RepeatForever()));

});
//Burada ise yukarda tanýmlanan quartzýn host olacaðýný tanýmlýyoruz.
builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);




var host = builder.Build();
host.Run();
