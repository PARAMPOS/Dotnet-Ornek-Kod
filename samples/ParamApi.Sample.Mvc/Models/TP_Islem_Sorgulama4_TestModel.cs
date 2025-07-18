using System.ComponentModel.DataAnnotations;
using ParamApi.Sdk.Models.Requests;
using ParamApi.Sdk.Models.Responses;

namespace ParamApi.Sample.Mvc.Models;

/// <summary>
/// TP_Islem_Sorgulama4 test sayfası için view model
/// </summary>
public class TP_Islem_Sorgulama4_TestModel
{
    /// <summary>
    /// Dekont ID
    /// </summary>
    [Display(Name = "Dekont ID")]
    public string? Dekont_ID { get; set; }

    /// <summary>
    /// Sipariş ID
    /// </summary>
    [Display(Name = "Sipariş ID")]
    public string? Siparis_ID { get; set; }

    /// <summary>
    /// İşlem ID
    /// </summary>
    [Display(Name = "İşlem ID")]
    public string? Islem_ID { get; set; }

    /// <summary>
    /// SDK Request'ine dönüştür
    /// </summary>
    public TP_Islem_Sorgulama4_Request ToRequest()
    {
        return new TP_Islem_Sorgulama4_Request
        {
            Dekont_ID = string.IsNullOrWhiteSpace(Dekont_ID) ? null : Dekont_ID,
            Siparis_ID = string.IsNullOrWhiteSpace(Siparis_ID) ? null : Siparis_ID,
            Islem_ID = string.IsNullOrWhiteSpace(Islem_ID) ? null : Islem_ID
        };
    }

    /// <summary>
    /// En az bir parametre girilmiş mi kontrol et
    /// </summary>
    public bool HasValidInput => !string.IsNullOrWhiteSpace(Dekont_ID) || 
                                !string.IsNullOrWhiteSpace(Siparis_ID) || 
                                !string.IsNullOrWhiteSpace(Islem_ID);

    /// <summary>
    /// Örnek değerler
    /// </summary>
    public static List<(string Type, string Value)> SampleValues => new()
    {
        ("Dekont ID", "123456789"),
        ("Sipariş ID", "ORDER-123456789"),
        ("İşlem ID", "TXN-123456789"),
        ("Dekont ID", "987654321"),
        ("Sipariş ID", "TEST-ORDER-001")
    };
}

/// <summary>
/// TP_Islem_Sorgulama4 sonuç sayfası için view model
/// </summary>
public class TP_Islem_Sorgulama4_ResultModel
{
    /// <summary>
    /// Gönderilen request bilgileri
    /// </summary>
    public TP_Islem_Sorgulama4_TestModel? Request { get; set; }

    /// <summary>
    /// API Response
    /// </summary>
    public TP_Islem_Sorgulama4_Response? Response { get; set; }

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