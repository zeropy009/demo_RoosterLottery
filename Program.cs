using demo_RoosterLottery.Jobs;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add MemoryCache
builder.Services.AddMemoryCache();

// Add Job Backgrond
builder.Services.AddQuartz(option =>
{
    var jobKey = JobKey.Create(nameof(LotteryJob));
    option
        .AddJob<LotteryJob>(jobKey)
        .AddTrigger(trigger =>
                        trigger
                            .ForJob(jobKey)
                            //.WithCronSchedule("0 * * * *")
                            .WithSchedule(SimpleScheduleBuilder.RepeatMinutelyForever())
        );
});
builder.Services.AddQuartzHostedService(option =>
{
    option.WaitForJobsToComplete = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
