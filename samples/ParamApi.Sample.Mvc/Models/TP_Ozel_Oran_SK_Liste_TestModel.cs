using ParamApi.Sdk.Models.Responses;

namespace ParamApi.Sample.Mvc.Models;

/// <summary>
/// TP_Ozel_Oran_SK_Liste test sayfası için view model
/// </summary>
public class TP_Ozel_Oran_SK_Liste_TestModel
{
    // Bu method parametre almıyor, sadece çağrılıyor
}

/// <summary>
/// TP_Ozel_Oran_SK_Liste sonuç sayfası için view model
/// </summary>
public class TP_Ozel_Oran_SK_Liste_ResultModel
{
    /// <summary>
    /// API Response
    /// </summary>
    public TP_Ozel_Oran_SK_Liste_Response? Response { get; set; }

    /// <summary>
    /// İşlem başarılı mı?
    /// </summary>
    public bool IsSuccess => Response?.IsSuccess ?? false;

    /// <summary>
    /// Hata mesajı (varsa)
    /// </summary>
    public string? ErrorMessage => IsSuccess ? null : Response?.Sonuc_Str;

    /// <summary>
    /// İşlem zamanı
    /// </summary>
    public DateTime ProcessTime { get; set; } = DateTime.Now;

    /// <summary>
    /// İşlem süresi (ms)
    /// </summary>
    public long ProcessDuration { get; set; }
} 