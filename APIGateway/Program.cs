using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace APIGateway;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddControllers();
        
        //Ocelot Configurations
        builder.Configuration.AddJsonFile("ocelot.json", false, true);
        builder.Services.AddOcelot();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        //
        //app.UseHttpsRedirection();

        app.UseAuthorization();
        
        //ADDED
        app.UseRouting();

        app.MapControllers();

        app.UseOcelot().Wait();
        
        app.Run();
    }
}