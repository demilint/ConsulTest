using WebApi.Configs;
using Winton.Extensions.Configuration.Consul;
using Winton.Extensions.Configuration.Consul.Parsers;

namespace WebApi6
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            builder.Configuration.AddJsonFile("appsettings.HotReload.json", optional: false, reloadOnChange: true);
            builder.Configuration.AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);
            builder.Configuration.AddJsonFile($"appsettings.{env}.HotReload.json", optional: true, reloadOnChange: true);

            builder.Services.Configure<HotReloadConfigs>(builder.Configuration);
            builder.Services.Configure<QuartzConfig>(builder.Configuration.GetSection("Quartz"));

            builder.Configuration.AddConsul("WebApi", options =>
            {
                options.Parser = new SimpleConfigurationParser();
                options.Optional = true;
                options.ReloadOnChange = true;
                options.ReloadOnChange = true;
                options.OnLoadException = exceptionContext =>
                {
                    exceptionContext.Ignore = true;
                    Console.WriteLine("Log error", exceptionContext.Exception);
                };
            });

            builder.Services.AddControllers();
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

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}