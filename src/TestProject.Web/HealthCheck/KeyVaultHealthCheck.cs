using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using TestProject.Web.Infrastucture;

namespace TestProject.Web.HealthCheck
{
  public class KeyVaultHealthCheck : IHealthCheck
  {
    private readonly ILogger<KeyVaultHealthCheck> _logger;
    private readonly AppConfiguration _appConfiguration;

    public KeyVaultHealthCheck(ILogger<KeyVaultHealthCheck> logger, IOptions<AppConfiguration> options)
    {
      _logger = logger;
      _appConfiguration = options.Value;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
      if (string.IsNullOrEmpty(_appConfiguration.KeyVaultUrl))
      {
        _logger.LogInformation("KeyVaultHealthCheck without application being configured to run towards keyvault");
        return HealthCheckResult.Healthy("KeyVault not configured - healthy");
      }

      var isHealthy = await CanAccessKeyVault();
      if (isHealthy)
      {
        return HealthCheckResult.Healthy("Connection to keyvaut is available");
      }

      return new HealthCheckResult(context.Registration.FailureStatus, "Unable to connect to SAP");
    }

    private async Task<bool> CanAccessKeyVault()
    {
      _logger.LogInformation("Querying keyvault");
      try
      {
        var client = new SecretClient(vaultUri: new Uri(_appConfiguration.KeyVaultUrl), credential: new DefaultAzureCredential());

        var secretsTasks = new string[] { "AuthenticationInfo--Username" }
              .Select(name => client.GetSecretAsync(name));

        await Task.WhenAll(secretsTasks);
        var secretsDict = secretsTasks.ToDictionary(e => e.Result.Value.Name, e => e.Result.Value.Value);

        //Optinally write to console to confirm
        _logger.LogInformation("Finished keyvault access");
        return true;
      }
      catch (Exception e)
      {
        _logger.LogError(e, "Error acccessing KeyVault - Message: {Message}", e.Message);
        return false;
      }
    }
  }
}