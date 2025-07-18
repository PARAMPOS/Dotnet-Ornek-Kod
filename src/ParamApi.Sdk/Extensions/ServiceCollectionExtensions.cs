using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ParamApi.Sdk.Configuration;
using ParamApi.Sdk.Services;

namespace ParamApi.Sdk.Extensions;

/// <summary>
/// Param API SDK için Dependency Injection extension'ları
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Param API Client'ını DI container'a ekler
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="environment">Kullanılacak ortam</param>
    /// <param name="configureOptions">Yapılandırma seçenekleri</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddParamApiClient(
        this IServiceCollection services,
        ParamEnvironment environment,
        Action<ParamApiOptions> configureOptions)
    {
        // Options'ları yapılandır
        services.Configure<ParamApiOptions>(options =>
        {
            options.Environment = environment;
            configureOptions(options);
        });

        // Validation için options validation ekle
        services.AddSingleton<IValidateOptions<ParamApiOptions>, ValidateParamApiOptions>();

        // TurkPos servisini kaydet
        services.AddScoped<ITurkposService, ParamApi.Sdk.Services.TurkposService>();
        
        // KS (Kart Saklama) servisini kaydet
        services.AddScoped<IKartService, ParamApi.Sdk.Services.KartService>();

        return services;
    }

    /// <summary>
    /// Param API Client'ını configuration'dan otomatik yapılandırma ile ekler
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configureOptions">Yapılandırma seçenekleri</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddParamApiClient(
        this IServiceCollection services,
        Action<ParamApiOptions> configureOptions)
    {
        // Default test environment ile ekle
        return services.AddParamApiClient(ParamEnvironment.Test, configureOptions);
    }
}

/// <summary>
/// ParamApiOptions validation sınıfı
/// </summary>
internal class ValidateParamApiOptions : IValidateOptions<ParamApiOptions>
{
    public ValidateOptionsResult Validate(string? name, ParamApiOptions options)
    {
        var failures = new List<string>();

        if (string.IsNullOrWhiteSpace(options.ClientCode))
            failures.Add("CLIENT_CODE boş olamaz");

        if (string.IsNullOrWhiteSpace(options.Username))
            failures.Add("CLIENT_USERNAME boş olamaz");

        if (string.IsNullOrWhiteSpace(options.Password))
            failures.Add("CLIENT_PASSWORD boş olamaz");

        return failures.Count > 0
            ? ValidateOptionsResult.Fail(failures)
            : ValidateOptionsResult.Success;
    }
} 