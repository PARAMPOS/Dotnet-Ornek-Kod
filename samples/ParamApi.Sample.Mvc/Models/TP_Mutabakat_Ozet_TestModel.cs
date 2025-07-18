using System.ComponentModel.DataAnnotations;
using ParamApi.Sdk.Models.Requests;
using ParamApi.Sdk.Models.Responses;

namespace ParamApi.Sample.Mvc.Models;

/// <summary>
/// TP_Mutabakat_Ozet test sayfası için view model
/// </summary>
public class TP_Mutabakat_Ozet_TestModel
{
    /// <summary>
    /// Başlangıç Tarihi
    /// </summary>
    [Required(ErrorMessage = "Başlangıç tarihi zorunludur")]
    [Display(Name = "Başlangıç Tarihi")]
    public string Tarih_Bas { get; set; } = DateTime.Now.AddDays(-7).ToString("dd.MM.yyyy HH:mm:ss");

    /// <summary>
    /// Bitiş Tarihi
    /// </summary>
    [Required(ErrorMessage = "Bitiş tarihi zorunludur")]
    [Display(Name = "Bitiş Tarihi")]
    public string Tarih_Bit { get; set; } = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");

    /// <summary>
    /// SDK Request'ine dönüştür
    /// </summary>
    public TP_Mutabakat_Ozet_Request ToRequest(string guid)
    {
        return new TP_Mutabakat_Ozet_Request
        {
            GUID = guid,
            Tarih_Bas = Tarih_Bas,
            Tarih_Bit = Tarih_Bit
        };
    }

    /// <summary>
    /// Örnek tarih aralıkları
    /// </summary>
    public static List<(string Name, string TarihBas, string TarihBit)> SampleDateRanges => new()
    {
        ("Bugün", DateTime.Now.ToString("dd.MM.yyyy") + " 00:00:00", DateTime.Now.ToString("dd.MM.yyyy") + " 23:59:59"),
        ("Dün", DateTime.Now.AddDays(-1).ToString("dd.MM.yyyy") + " 00:00:00", DateTime.Now.AddDays(-1).ToString("dd.MM.yyyy") + " 23:59:59"),
        ("Son 7 Gün", DateTime.Now.AddDays(-7).ToString("dd.MM.yyyy") + " 00:00:00", DateTime.Now.ToString("dd.MM.yyyy") + " 23:59:59"),
        ("Bu Ay", new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("dd.MM.yyyy") + " 00:00:00", DateTime.Now.ToString("dd.MM.yyyy") + " 23:59:59"),
        ("Geçen Ay", new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("dd.MM.yyyy") + " 00:00:00", new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1).ToString("dd.MM.yyyy") + " 23:59:59")
    };
}

/// <summary>
/// TP_Mutabakat_Ozet sonuç sayfası için view model
/// </summary>
public class TP_Mutabakat_Ozet_ResultModel
{
    /// <summary>
    /// Gönderilen request bilgileri
    /// </summary>
    public TP_Mutabakat_Ozet_TestModel? Request { get; set; }

    /// <summary>
    /// API Response
    /// </summary>
    public TP_Mutabakat_Ozet_Response? Response { get; set; }

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