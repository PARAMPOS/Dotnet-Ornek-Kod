using System.ComponentModel.DataAnnotations;

namespace ParamApi.Sdk.Models.Requests;

/// <summary>
/// Saklı kart silme isteği
/// Daha önce kaydedilen kredi kartını sistemden silmek için kullanılır
/// </summary>
public class KS_Kart_Sil_Request
{
    /// <summary>
    /// Saklı kartın GUID değeri (36 karakter)
    /// Silinecek kartın tekil tanımlayıcısı
    /// </summary>
    [Required(ErrorMessage = "KS_GUID alanı zorunludur")]
    [StringLength(36, ErrorMessage = "KS_GUID 36 karakter olmalıdır")]
    public string KS_GUID { get; set; } = string.Empty;

    /// <summary>
    /// Saklanacak Kredi Kartına ait tarafınızdan iletilecek tekil ID değeri (200 karakter)
    /// Kart silinirken kontrol amaçlı kullanılan ID
    /// </summary>
    [Required(ErrorMessage = "KK_Islem_ID alanı zorunludur")]
    [StringLength(200, ErrorMessage = "KK_Islem_ID maksimum 200 karakter olabilir")]
    public string KK_Islem_ID { get; set; } = string.Empty;

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

        // GUID format kontrolü (basit kontrol)
        if (!string.IsNullOrEmpty(KS_GUID) && KS_GUID.Length == 36)
        {
            // GUID format kontrolü yapmak için Guid.TryParse kullanabiliriz
            if (!Guid.TryParse(KS_GUID, out _))
            {
                validationResults.Add(new ValidationResult(
                    "KS_GUID geçerli bir GUID formatında olmalıdır",
                    new[] { nameof(KS_GUID) }));
                isValid = false;
            }
        }

        return isValid;
    }
} 