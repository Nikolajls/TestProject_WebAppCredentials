using Azure.Identity;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using TestProject.CQRS;
using TestProject.Services.Math;
using TestProject.Web.HealthCheck;
using TestProject.Web.Infrastucture;
using TestProject.Web.Infrastucture.Authentication;

namespace TestProject.Web
{
  public class Program
  {
    public static async Task Main(string[] args)
    {
      //Logger that will log to console during bootstrapping
      Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();
      Log.Information("Starting up API instance!");

      try
      {
        var builder = WebApplication.CreateBuilder(args);
        builder.WebHost.ConfigureAppConfiguration((context, builder) =>
        {
          AddKeyVaultIfConfigured(context, builder);
          builder.AddEnvironmentVariables();
        });

        builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));


        var services = builder.Services;
        var configuration = builder.Configuration;

        services.AddControllersWithViews();

        services.AddOptions();
        services.Configure<BasicAuthenticationInfo>(configuration.GetSection("AuthenticationInfo"));
        services.Configure<AppConfiguration>(configuration.GetSection("AppConfiguration"));


        //Mediatr
        services.AddMediatR(typeof(AssemblyAnchor).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssembly(typeof(AssemblyAnchor).Assembly);

        //Authentcation
        services.AddAuthentication()
                  .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", options => { });

        services.AddAuthorization(options =>
        {
          options.AddPolicy("BasicAuthentication", new AuthorizationPolicyBuilder("BasicAuthentication").RequireAuthenticatedUser().Build());
        });
        services.AddSingleton<IMathService, MathService>();

        services.AddProblemDetails();
        services.AddSwaggerGen();

        services.AddHealthChecks()
       .AddCheck<KeyVaultHealthCheck>("KeyVaultConnection", tags: new[] { "KeyVault" });




        var app = builder.Build();



        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
          app.UseExceptionHandler("/Home/Error");
          // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
          app.UseHsts();
        }
        else
        {
          app.UseSwagger();
          app.UseSwaggerUI();
        }

        app.UseProblemDetails();



        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.MapHealthChecks("/health");
        app.MapHealthChecks("/health/KeyVaultConnection", new HealthCheckOptions
        {
          Predicate = healthCheck => healthCheck.Tags.Contains("KeyVault")
        });

        app.Run();
      }
      catch (Exception ex)
      {
        Log.Fatal(ex, "Unhandled exception - {Message}", ex.Message);
      }
      finally
      {
        Log.Information("Shut down complete");
        Log.CloseAndFlush();
        await Task.Delay(1000);
      }
    }

    private static void AddKeyVaultIfConfigured(WebHostBuilderContext context, IConfigurationBuilder builder)
    {
      var apiConfiguration = new AppConfiguration();
      context.Configuration.Bind("AppConfiguration", apiConfiguration);

      if (apiConfiguration != null && !string.IsNullOrEmpty(apiConfiguration.KeyVaultUrl))
      {
        builder.AddAzureKeyVault(new Uri(apiConfiguration.KeyVaultUrl), new DefaultAzureCredential());
        Log.Information("KeyVault configuration found - {Url}", apiConfiguration.KeyVaultUrl);
      }
      else
      {
        Log.Information("KeyVault Url not found - KeyVault not being used!");
      }
    }
  }
}