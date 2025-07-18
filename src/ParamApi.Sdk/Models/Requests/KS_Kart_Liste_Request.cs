using System.ComponentModel.DataAnnotations;

namespace ParamApi.Sdk.Models.Requests;

/// <summary>
/// Saklı kart listesi isteği
/// Saklı kartların listelenmesi için kullanılır
/// </summary>
public class KS_Kart_Liste_Request
{
    /// <summary>
    /// Saklanan kart adı
    /// Bu kart adına sahip tüm kartlar listelenir
    /// </summary>
    [Required(ErrorMessage = "KK_Kart_Adi alanı zorunludur")]
    public string KK_Kart_Adi { get; set; } = string.Empty;

    /// <summary>
    /// Saklanan karta ait tekil ID (Opsiyonel)
    /// Belirli bir işlem ID'si ile filtreleme yapmak için kullanılır
    /// </summary>
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