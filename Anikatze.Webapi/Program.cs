using Anikatze.Application.Dtos;
using Anikatze.Application.Infrastracture;
using Anikatze.Application.Models;
using Anikatze.Application.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        
        // Use configuration for connection string
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<AnikatzeContext>(opt =>
            opt.UseNpgsql(connectionString));

        builder.Services.AddAutoMapper(typeof(MappingProfile));
        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<CourseService>();
        builder.Services.AddScoped<LectionService>();
        builder.Services.AddScoped<UserLectionCompletionService>();
        builder.Services.AddScoped<QuizService>();
        builder.Services.AddScoped<BlobService>();
        builder.Services.AddScoped<UserQuizService>();
        builder.Services.AddScoped<BitmovinService>();
        builder.Services.AddLogging(config =>
        {
            config.AddConsole();
            config.AddDebug();
        });

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Anikatze API", Version = "v1" });
        });

        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Anikatze API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        }
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.MapControllers();
        app.MapFallbackToFile("index.html");
        app.Run();
    }
}
