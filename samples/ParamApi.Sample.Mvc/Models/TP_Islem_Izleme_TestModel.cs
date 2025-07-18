using System.ComponentModel.DataAnnotations;
using ParamApi.Sdk.Models.Requests;
using ParamApi.Sdk.Models.Responses;

namespace ParamApi.Sample.Mvc.Models;

/// <summary>
/// TP_Islem_Izleme test sayfası için view model
/// </summary>
public class TP_Islem_Izleme_TestModel
{
    /// <summary>
    /// Başlangıç Tarihi
    /// </summary>
    [Required(ErrorMessage = "Başlangıç tarihi zorunludur")]
    [Display(Name = "Başlangıç Tarihi")]
    public string Tarih_Bas { get; set; } = DateTime.Now.AddDays(-1).ToString("dd.MM.yyyy HH:mm:ss");

    /// <summary>
    /// Bitiş Tarihi
    /// </summary>
    [Required(ErrorMessage = "Bitiş tarihi zorunludur")]
    [Display(Name = "Bitiş Tarihi")]
    public string Tarih_Bit { get; set; } = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");

    /// <summary>
    /// İşlem Tipi
    /// </summary>
    [Display(Name = "İşlem Tipi")]
    public string? Islem_Tip { get; set; }

    /// <summary>
    /// İşlem Durumu
    /// </summary>
    [Display(Name = "İşlem Durumu")]
    public string? Islem_Durum { get; set; }

    /// <summary>
    /// SDK Request'ine dönüştür
    /// </summary>
    public TP_Islem_Izleme_Request ToRequest()
    {
        return new TP_Islem_Izleme_Request
        {
            Tarih_Bas = Tarih_Bas,
            Tarih_Bit = Tarih_Bit,
            Islem_Tip = Islem_Tip ?? string.Empty,
            Islem_Durum = Islem_Durum ?? string.Empty
        };
    }

    /// <summary>
    /// İşlem tipleri
    /// </summary>
    public static List<(string Value, string Text)> IslemTipleri => new()
    {
        ("", "Tümü"),
        ("İptal", "İptal"),
        ("İade", "İade"),
        ("Satış", "Satış")
    };

    /// <summary>
    /// İşlem durumları
    /// </summary>
    public static List<(string Value, string Text)> IslemDurumlari => new()
    {
        ("", "Tümü"),
        ("Başarılı", "Başarılı"),
        ("Başarısız", "Başarısız")
    };

    /// <summary>
    /// Örnek tarih aralıkları
    /// </summary>
    public static List<(string Name, string TarihBas, string TarihBit)> SampleDateRanges => new()
    {
        ("Bugün", DateTime.Now.ToString("dd.MM.yyyy") + " 00:00:00", DateTime.Now.ToString("dd.MM.yyyy") + " 23:59:59"),
        ("Dün", DateTime.Now.AddDays(-1).ToString("dd.MM.yyyy") + " 00:00:00", DateTime.Now.AddDays(-1).ToString("dd.MM.yyyy") + " 23:59:59"),
        ("Son 3 Gün", DateTime.Now.AddDays(-3).ToString("dd.MM.yyyy") + " 00:00:00", DateTime.Now.ToString("dd.MM.yyyy") + " 23:59:59"),
        ("Son Hafta", DateTime.Now.AddDays(-7).ToString("dd.MM.yyyy") + " 00:00:00", DateTime.Now.ToString("dd.MM.yyyy") + " 23:59:59")
    };
}

/// <summary>
/// TP_Islem_Izleme sonuç sayfası için view model
/// </summary>
public class TP_Islem_Izleme_ResultModel
{
    /// <summary>
    /// Gönderilen request bilgileri
    /// </summary>
    public TP_Islem_Izleme_TestModel? Request { get; set; }

    /// <summary>
    /// API Response
    /// </summary>
    public TP_Islem_Izleme_Response? Response { get; set; }

    /// <summary>
    /// İşlem başarılı mı?
    /// </summary>
    public bool IsSuccess => Response?.Sonuc > 0;

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