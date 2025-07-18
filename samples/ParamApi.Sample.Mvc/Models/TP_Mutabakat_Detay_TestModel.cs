using System.ComponentModel.DataAnnotations;
using ParamApi.Sdk.Models.Requests;
using ParamApi.Sdk.Models.Responses;

namespace ParamApi.Sample.Mvc.Models;

/// <summary>
/// TP_Mutabakat_Detay test sayfası için view model
/// </summary>
public class TP_Mutabakat_Detay_TestModel
{
    /// <summary>
    /// İşlem Tarihi
    /// </summary>
    [Required(ErrorMessage = "İşlem tarihi zorunludur")]
    [Display(Name = "İşlem Tarihi")]
    public string Tarih { get; set; } = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");

    /// <summary>
    /// SDK Request'ine dönüştür
    /// </summary>
    public TP_Mutabakat_Detay_Request ToRequest()
    {
        return new TP_Mutabakat_Detay_Request
        {
            Tarih = Tarih
        };
    }

    /// <summary>
    /// Örnek tarihler
    /// </summary>
    public static List<(string Name, string Tarih)> SampleDates => new()
    {
        ("Bugün", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")),
        ("Dün", DateTime.Now.AddDays(-1).ToString("dd.MM.yyyy HH:mm:ss")),
        ("2 Gün Önce", DateTime.Now.AddDays(-2).ToString("dd.MM.yyyy HH:mm:ss")),
        ("1 Hafta Önce", DateTime.Now.AddDays(-7).ToString("dd.MM.yyyy HH:mm:ss")),
        ("1 Ay Önce", DateTime.Now.AddMonths(-1).ToString("dd.MM.yyyy HH:mm:ss"))
    };
}

/// <summary>
/// TP_Mutabakat_Detay sonuç sayfası için view model
/// </summary>
public class TP_Mutabakat_Detay_ResultModel
{
    /// <summary>
    /// Gönderilen request bilgileri
    /// </summary>
    public TP_Mutabakat_Detay_TestModel? Request { get; set; }

    /// <summary>
    /// API Response
    /// </summary>
    public TP_Mutabakat_Detay_Response? Response { get; set; }

    /// <summary>
    /// İşlem başarılı mı?
    /// </summary>
    public bool IsSuccess => Response?.Sonuc == "1";

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