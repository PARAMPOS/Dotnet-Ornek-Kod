using System.ComponentModel.DataAnnotations;

namespace ParamApi.Sdk.Models.Requests;

/// <summary>
/// Kredi kartı saklama isteği
/// Kredi kartı bilgilerini güvenli olarak saklamak için kullanılır
/// </summary>
public class KS_Kart_Ekle_Request
{
    /// <summary>
    /// Kredi Kartı Sahibinin Adı Soyadı
    /// </summary>
    [Required(ErrorMessage = "KK_Sahibi alanı zorunludur")]
    [StringLength(150, ErrorMessage = "KK_Sahibi en fazla 150 karakter olabilir")]
    public string KK_Sahibi { get; set; } = string.Empty;

    /// <summary>
    /// Kredi Kartı Numarası (16 haneli)
    /// </summary>
    [Required(ErrorMessage = "KK_No alanı zorunludur")]
    [StringLength(16, MinimumLength = 16, ErrorMessage = "KK_No tam 16 haneli olmalıdır")]
    [RegularExpression(@"^\d{16}$", ErrorMessage = "KK_No sadece rakamlardan oluşmalıdır")]
    public string KK_No { get; set; } = string.Empty;

    /// <summary>
    /// Kredi Kartının Son Kullanma Tarihi (Ay) - 2 haneli
    /// </summary>
    [Required(ErrorMessage = "KK_SK_Ay alanı zorunludur")]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "KK_SK_Ay tam 2 haneli olmalıdır")]
    [RegularExpression(@"^(0[1-9]|1[0-2])$", ErrorMessage = "KK_SK_Ay 01-12 arasında olmalıdır")]
    public string KK_SK_Ay { get; set; } = string.Empty;

    /// <summary>
    /// Kredi Kartının Son Kullanma Tarihi (Yıl) - 4 haneli veya 2 haneli
    /// </summary>
    [Required(ErrorMessage = "KK_SK_Yil alanı zorunludur")]
    [StringLength(4, MinimumLength = 2, ErrorMessage = "KK_SK_Yil 2 veya 4 haneli olmalıdır")]
    [RegularExpression(@"^\d{2,4}$", ErrorMessage = "KK_SK_Yil sadece rakamlardan oluşmalıdır")]
    public string KK_SK_Yil { get; set; } = string.Empty;

    /// <summary>
    /// Saklanacak Kredi Kartı adı (Opsiyonel)
    /// </summary>
    [StringLength(150, ErrorMessage = "KK_Kart_Adi en fazla 150 karakter olabilir")]
    public string? KK_Kart_Adi { get; set; }

    /// <summary>
    /// Saklanacak Kredi Kartına ait tekil ID değeri (Opsiyonel)
    /// </summary>
    [StringLength(200, ErrorMessage = "KK_Islem_ID en fazla 200 karakter olabilir")]
    public string? KK_Islem_ID { get; set; }

    /// <summary>
    /// Request'in geçerli olup olmadığını kontrol eder
    /// </summary>
    public bool IsValid(out List<ValidationResult> validationResults)
    {
        validationResults = new List<ValidationResult>();
        var context = new ValidationContext(this);
        return Validator.TryValidateObject(this, context, validationResults, true);
    }
} 