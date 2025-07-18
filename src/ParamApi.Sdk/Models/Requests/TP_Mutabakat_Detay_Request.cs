using System.ComponentModel.DataAnnotations;

namespace ParamApi.Sdk.Models.Requests;

/// <summary>
/// Mutabakat detay sorgulama request modeli
/// Belirli tarihte üye işyerinin mutabakat detaylarını almak için kullanılır
/// GUID parametresi ParamApiOptions'dan otomatik alınır
/// </summary>
public class TP_Mutabakat_Detay_Request
{
    /// <summary>
    /// İşlem Tarihi (dd.MM.yyyy HH:mm:ss – 14.04.2021 00:00:16)
    /// </summary>
    [Required(ErrorMessage = "Tarih zorunludur")]
    [StringLength(20, ErrorMessage = "Tarih maksimum 20 karakter olmalıdır")]
    public required string Tarih { get; set; }

    /// <summary>
    /// Request'in geçerli olup olmadığını kontrol eder
    /// </summary>
    /// <param name="validationResults">Validation hata listesi</param>
    /// <returns>Geçerli ise true</returns>
    public bool IsValid(out List<ValidationResult> validationResults)
    {
        validationResults = new List<ValidationResult>();
        var context = new ValidationContext(this);
        return Validator.TryValidateObject(this, context, validationResults, true);
    }
} 