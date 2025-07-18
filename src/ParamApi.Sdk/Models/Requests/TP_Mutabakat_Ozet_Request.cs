using System.ComponentModel.DataAnnotations;

namespace ParamApi.Sdk.Models.Requests;

/// <summary>
/// Mutabakat özet sorgulama request modeli
/// Belirli tarihler arasındaki üye işyerinin işlemleri özet biçiminde almak için kullanılır
/// </summary>
public class TP_Mutabakat_Ozet_Request
{
    /// <summary>
    /// Üye İşyerine ait Anahtar (36 karakter)
    /// </summary>
    [Required(ErrorMessage = "GUID zorunludur")]
    [StringLength(36, ErrorMessage = "GUID 36 karakter olmalıdır")]
    public required string GUID { get; set; }

    /// <summary>
    /// Başlangıç Tarihi (dd.MM.yyyy HH:mm:ss – 20.11.2015 00:00:00)
    /// </summary>
    [Required(ErrorMessage = "Tarih_Bas zorunludur")]
    [StringLength(20, ErrorMessage = "Tarih_Bas maksimum 20 karakter olmalıdır")]
    public required string Tarih_Bas { get; set; }

    /// <summary>
    /// Bitiş Tarihi (dd.MM.yyyy HH:mm:ss – 20.11.2015 15:15:00)
    /// </summary>
    [Required(ErrorMessage = "Tarih_Bit zorunludur")]
    [StringLength(20, ErrorMessage = "Tarih_Bit maksimum 20 karakter olmalıdır")]
    public required string Tarih_Bit { get; set; }

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