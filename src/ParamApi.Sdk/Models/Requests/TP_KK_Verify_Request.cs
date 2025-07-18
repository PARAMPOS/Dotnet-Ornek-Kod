using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ParamApi.Sdk.Models.Requests;

/// <summary>
/// Kredi kartı doğrulama isteği
/// Kart doğrulama ve 3D Secure işlemleri için kullanılır
/// </summary>
public class TP_KK_Verify_Request
{
    /// <summary>
    /// Kredi Kartı numarası (16 karakter)
    /// </summary>
    [Required(ErrorMessage = "KK_No alanı zorunludur")]
    [StringLength(16, ErrorMessage = "KK_No 16 karakter olmalıdır")]
    [RegularExpression(@"^\d{16}$", ErrorMessage = "KK_No sadece 16 haneli sayı olmalıdır")]
    public string KK_No { get; set; } = string.Empty;

    /// <summary>
    /// 2 haneli Son Kullanma Ay (01-12)
    /// </summary>
    [Required(ErrorMessage = "KK_SK_Ay alanı zorunludur")]
    [StringLength(2, ErrorMessage = "KK_SK_Ay 2 karakter olmalıdır")]
    [RegularExpression(@"^(0[1-9]|1[0-2])$", ErrorMessage = "KK_SK_Ay 01-12 arasında olmalıdır")]
    public string KK_SK_Ay { get; set; } = string.Empty;

    /// <summary>
    /// 4 haneli Son Kullanma Yıl
    /// </summary>
    [Required(ErrorMessage = "KK_SK_Yil alanı zorunludur")]
    [StringLength(4, ErrorMessage = "KK_SK_Yil 4 karakter olmalıdır")]
    [RegularExpression(@"^\d{4}$", ErrorMessage = "KK_SK_Yil 4 haneli yıl formatında olmalıdır")]
    public string KK_SK_Yil { get; set; } = string.Empty;

    /// <summary>
    /// CVC Kodu (3 karakter)
    /// </summary>
    [Required(ErrorMessage = "KK_CVC alanı zorunludur")]
    [StringLength(3, ErrorMessage = "KK_CVC 3 karakter olmalıdır")]
    [RegularExpression(@"^\d{3}$", ErrorMessage = "KK_CVC 3 haneli sayı olmalıdır")]
    public string KK_CVC { get; set; } = string.Empty;

    /// <summary>
    /// Sonuç post parametrelerinin döndüğü URL (maksimum 256 karakter)
    /// Opsiyonel - 3D Secure sonrası yönlendirme için
    /// </summary>
    [StringLength(256, ErrorMessage = "Return_URL maksimum 256 karakter olabilir")]
    public string? Return_URL { get; set; }

    /// <summary>
    /// Extra Alan 1 (maksimum 250 karakter)
    /// </summary>
    [StringLength(250, ErrorMessage = "Data1 maksimum 250 karakter olabilir")]
    public string? Data1 { get; set; }

    /// <summary>
    /// Extra Alan 2 (maksimum 250 karakter)
    /// </summary>
    [StringLength(250, ErrorMessage = "Data2 maksimum 250 karakter olabilir")]
    public string? Data2 { get; set; }

    /// <summary>
    /// Extra Alan 3 (maksimum 250 karakter)
    /// </summary>
    [StringLength(250, ErrorMessage = "Data3 maksimum 250 karakter olabilir")]
    public string? Data3 { get; set; }

    /// <summary>
    /// Extra Alan 4 (maksimum 250 karakter)
    /// </summary>
    [StringLength(250, ErrorMessage = "Data4 maksimum 250 karakter olabilir")]
    public string? Data4 { get; set; }

    /// <summary>
    /// Extra Alan 5 (maksimum 250 karakter)
    /// </summary>
    [StringLength(250, ErrorMessage = "Data5 maksimum 250 karakter olabilir")]
    public string? Data5 { get; set; }

    /// <summary>
    /// Request'in geçerli olup olmadığını kontrol eder
    /// </summary>
    /// <param name="validationResults">Validation sonuçları</param>
    /// <returns>Validation başarılı ise true</returns>
    public bool IsValid(out List<ValidationResult> validationResults)
    {
        validationResults = new List<ValidationResult>();
        var context = new ValidationContext(this);
        
        var isValid = Validator.TryValidateObject(this, context, validationResults, true);

        // Ek validasyonlar
        if (!string.IsNullOrEmpty(KK_SK_Yil))
        {
            if (int.TryParse(KK_SK_Yil, out var year))
            {
                var currentYear = DateTime.Now.Year;
                if (year < currentYear)
                {
                    validationResults.Add(new ValidationResult(
                        "Kredi kartının son kullanma tarihi geçmiş olmamalıdır",
                        new[] { nameof(KK_SK_Yil) }));
                    isValid = false;
                }
            }
        }

        // URL validation (eğer verilmişse)
        if (!string.IsNullOrEmpty(Return_URL))
        {
            if (!Uri.TryCreate(Return_URL, UriKind.Absolute, out _))
            {
                validationResults.Add(new ValidationResult(
                    "Return_URL geçerli bir URL formatında olmalıdır",
                    new[] { nameof(Return_URL) }));
                isValid = false;
            }
        }

        return isValid;
    }
} 