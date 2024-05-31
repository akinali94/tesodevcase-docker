using CustomerService.Configs;
using CustomerService.Mappers;
using CustomerService.Middlewares;
using CustomerService.Repositories;
using CustomerService.Services;
using CustomerService.V1.Models.RequestModels;
using CustomerService.V1.Models.Validators;
using FluentValidation;

namespace CustomerService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        // Add services to the container.
        builder.Services.AddAuthorization();
        
        //AddMongoDb Setting
        builder.Services.Configure<CustomerDbSettings>(
            builder.Configuration.GetSection("CustomerDatabase"));
        
        //
        builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();
        builder.Services.AddSingleton<ICustomerService, Services.CustomerService>();
        builder.Services.AddSingleton<ICustomerMapper, CustomerMapper>();
        
        //Fluent Validation
        builder.Services.AddScoped<IValidator<CustomerCreateModel>, CustomerCreateValidator>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        //Deleted for http redirection problem
        //app.UseHttpsRedirection();

        app.UseAuthorization();
        
        //Custom Exception
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.MapControllers();

        app.Run();
    }
}