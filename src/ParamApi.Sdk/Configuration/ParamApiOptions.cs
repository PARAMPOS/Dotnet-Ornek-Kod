using System.ComponentModel.DataAnnotations;

namespace ParamApi.Sdk.Configuration;

/// <summary>
/// Param API yapılandırma seçenekleri
/// </summary>
public class ParamApiOptions
{
    /// <summary>
    /// Kullanılacak ortam (Test/Production)
    /// </summary>
    public ParamEnvironment Environment { get; set; } = ParamEnvironment.Test;
    
    /// <summary>
    /// CLIENT_CODE - Param'dan alınan müşteri kodu
    /// </summary>
    [Required(ErrorMessage = "CLIENT_CODE zorunludur")]
    public required string ClientCode { get; set; }
    
    /// <summary>
    /// CLIENT_USERNAME - Param'dan alınan kullanıcı adı
    /// </summary>
    [Required(ErrorMessage = "CLIENT_USERNAME zorunludur")]
    public required string Username { get; set; }
    
    /// <summary>
    /// CLIENT_PASSWORD - Param'dan alınan şifre
    /// </summary>
    [Required(ErrorMessage = "CLIENT_PASSWORD zorunludur")]
    public required string Password { get; set; }
    
    /// <summary>
    /// GUID - İşlemler için benzersiz kimlik
    /// </summary>
    public string? Guid { get; set; }
    
    /// <summary>
    /// Seçilen ortama göre TurkPos endpoint URL'ini döndürür
    /// </summary>
    public string GetTurkposEndpoint()
    {
        return Environment == ParamEnvironment.Test
            ? "https://testposws.param.com.tr/turkpos.ws/service_turkpos_prod.asmx"
            : "https://posws.param.com.tr/turkpos.ws/service_turkpos_prod.asmx";
    }
    
    /// <summary>
    /// Seçilen ortama göre KsService endpoint URL'ini döndürür
    /// </summary>
    public string GetKsServiceEndpoint()
    {
        return Environment == ParamEnvironment.Test
            ? "https://testposws.param.com.tr/out.ws/service_ks.asmx"
            : "https://posws.param.com.tr/out.ws/service_ks.asmx";
    }
} 