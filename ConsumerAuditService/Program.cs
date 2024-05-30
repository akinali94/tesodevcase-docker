
using ConsumerAuditService.Configs;
using ConsumerAuditService.Services;

namespace ConsumerAuditService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();
        
        //MongoDb Settings
        builder.Services.Configure<MongoDbSettings>(
            builder.Configuration.GetSection("AuditDatabase"));
        
        builder.Services.AddSingleton<ConsumerService>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        //Kafka
        builder.Services.AddHostedService<ConsumerHostedService>();

        var app = builder.Build();

        // // Configure the HTTP request pipeline.
        // if (app.Environment.IsDevelopment())
        // {
        //     app.UseSwagger();
        //     app.UseSwaggerUI();
        // }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.Run();
    }
}