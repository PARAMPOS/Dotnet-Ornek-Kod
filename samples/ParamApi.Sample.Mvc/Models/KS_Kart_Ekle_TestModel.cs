using System.ComponentModel.DataAnnotations;

namespace ParamApi.Sample.Mvc.Models;

/// <summary>
/// KS_Kart_Ekle test sayfası için view model
/// Kredi kartı saklama işlemi test edilir
/// </summary>
public class KS_Kart_Ekle_TestModel
{
    /// <summary>
    /// Kredi Kartı Sahibinin Adı Soyadı
    /// </summary>
    [Required(ErrorMessage = "Kart sahibi adı zorunludur")]
    [Display(Name = "Kart Sahibi")]
    public string KK_Sahibi { get; set; } = "JOHN DOE";

    /// <summary>
    /// Kredi Kartı Numarası (16 haneli)
    /// </summary>
    [Required(ErrorMessage = "Kart numarası zorunludur")]
    [Display(Name = "Kart Numarası")]
    [StringLength(19, ErrorMessage = "Kart numarası 16 haneli olmalıdır")]
    public string KK_No { get; set; } = string.Empty;

    /// <summary>
    /// Son Kullanma Ay (01-12)
    /// </summary>
    [Required(ErrorMessage = "Son kullanma ayı zorunludur")]
    [Display(Name = "Son Kullanma Ay")]
    [RegularExpression(@"^(0[1-9]|1[0-2])$", ErrorMessage = "Ay 01-12 arasında olmalıdır")]
    public string KK_SK_Ay { get; set; } = "12";

    /// <summary>
    /// Son Kullanma Yıl (4 haneli)
    /// </summary>
    [Required(ErrorMessage = "Son kullanma yılı zorunludur")]
    [Display(Name = "Son Kullanma Yıl")]
    [RegularExpression(@"^\d{4}$", ErrorMessage = "Yıl 4 haneli sayı olmalıdır")]
    public string KK_SK_Yil { get; set; } = "2030";

    /// <summary>
    /// Kart Adı (Opsiyonel)
    /// </summary>
    [Display(Name = "Kart Adı")]
    public string? KK_Kart_Adi { get; set; } = "Test Kartı";

    /// <summary>
    /// İşlem ID (Opsiyonel)
    /// </summary>
    [Display(Name = "İşlem ID")]
    public string? KK_Islem_ID { get; set; } = $"KART_EKLE_{DateTime.Now:yyyyMMddHHmmss}";
}

/// <summary>
/// KS_Kart_Ekle sonuç sayfası için view model
/// </summary>
public class KS_Kart_Ekle_ResultModel
{
    /// <summary>
    /// İşlem sonucu
    /// </summary>
    public int Sonuc { get; set; }

    /// <summary>
    /// İşlem sonuç açıklama
    /// </summary>
    public string Sonuc_Str { get; set; } = string.Empty;

    /// <summary>
    /// Saklanan kartın GUID değeri
    /// </summary>
    public string KS_GUID { get; set; } = string.Empty;

    /// <summary>
    /// Request bilgileri (debug için)
    /// </summary>
    public KS_Kart_Ekle_TestModel? RequestData { get; set; }

    /// <summary>
    /// İşlem başarılı mı?
    /// </summary>
    public bool IsSuccess => Sonuc > 0;

    /// <summary>
    /// Hata mesajı
    /// </summary>
    public string? ErrorMessage { get; set; }
} 