using System.ComponentModel.DataAnnotations;

namespace ParamApi.Sdk.Models.Requests;

/// <summary>
/// Dekont e-posta gönderimi isteği
/// Dekontun e-posta olarak gönderilmesini sağlar
/// </summary>
public class TP_Islem_Dekont_Gonder_Request
{
    /// <summary>
    /// İşlemin Dekont ID'si
    /// </summary>
    [Required(ErrorMessage = "Dekont_ID alanı zorunludur")]
    public string Dekont_ID { get; set; } = string.Empty;

    /// <summary>
    /// Kişi e-posta adresi
    /// Gönderilmezse işyerinin kayıtlı e-posta adresine gönderilir
    /// </summary>
    [StringLength(100, ErrorMessage = "E_Posta en fazla 100 karakter olabilir")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
    public string? E_Posta { get; set; }

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