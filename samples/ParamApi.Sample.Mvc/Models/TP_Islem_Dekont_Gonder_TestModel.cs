using System.ComponentModel.DataAnnotations;
using ParamApi.Sdk.Models.Requests;
using ParamApi.Sdk.Models.Responses;

namespace ParamApi.Sample.Mvc.Models;

/// <summary>
/// TP_Islem_Dekont_Gonder test sayfası için view model
/// </summary>
public class TP_Islem_Dekont_Gonder_TestModel
{
    /// <summary>
    /// Dekont ID
    /// </summary>
    [Required(ErrorMessage = "Dekont ID zorunludur")]
    [Display(Name = "Dekont ID")]
    public string Dekont_ID { get; set; } = string.Empty;

    /// <summary>
    /// E-posta Adresi
    /// </summary>
    [Display(Name = "E-posta Adresi (Opsiyonel)")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
    public string? E_Posta { get; set; }

    /// <summary>
    /// SDK Request'ine dönüştür
    /// </summary>
    public TP_Islem_Dekont_Gonder_Request ToRequest()
    {
        return new TP_Islem_Dekont_Gonder_Request
        {
            Dekont_ID = Dekont_ID,
            E_Posta = string.IsNullOrWhiteSpace(E_Posta) ? null : E_Posta
        };
    }

    /// <summary>
    /// Örnek dekont ID'leri
    /// </summary>
    public static List<string> SampleDecontIds => new()
    {
        "123456789",
        "987654321",
        "111222333",
        "444555666",
        "777888999"
    };

    /// <summary>
    /// Test e-postaları
    /// </summary>
    public static List<string> SampleEmails => new()
    {
        "test@example.com",
        "admin@param.com.tr",
        "info@test.com",
        "user@domain.com"
    };
}

/// <summary>
/// TP_Islem_Dekont_Gonder sonuç sayfası için view model
/// </summary>
public class TP_Islem_Dekont_Gonder_ResultModel
{
    /// <summary>
    /// Gönderilen request bilgileri
    /// </summary>
    public TP_Islem_Dekont_Gonder_TestModel? Request { get; set; }

    /// <summary>
    /// API Response
    /// </summary>
    public TP_Islem_Dekont_Gonder_Response? Response { get; set; }

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