using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ParamApi.Sdk.Models.Requests;

/// <summary>
/// TP_Islem_Odeme_OnProv_Kapa (Ön Provizyon Kapama) request model
/// Ön provizyon işlemini satışa dönüştürür
/// </summary>
public class TP_Islem_Odeme_OnProv_Kapa_Request
{
    /// <summary>
    /// Provizyon ID (Opsiyonel)
    /// Boş geçilebilir
    /// </summary>
    public string? Prov_ID { get; set; }

    /// <summary>
    /// Kapama Yapılacak Tutar (Zorunlu)
    /// Sadece virgüllü kuruş formatında: 1000,50
    /// </summary>
    [Required(ErrorMessage = "Prov_Tutar zorunludur")]
    public decimal Prov_Tutar { get; set; }

    /// <summary>
    /// Sipariş ID Değeri (Zorunlu)
    /// </summary>
    [Required(ErrorMessage = "Siparis_ID zorunludur")]
    public required string Siparis_ID { get; set; }

    /// <summary>
    /// Tutar değerini PARAM API formatına çevirir (virgüllü)
    /// </summary>
    /// <returns>Virgülle formatlanmış tutar</returns>
    public string GetFormattedProvTutar()
    {
        return Prov_Tutar.ToString("F2", new CultureInfo("tr-TR"));
    }

    /// <summary>
    /// Request validation
    /// </summary>
    /// <param name="validationResults">Validation sonuçları</param>
    /// <returns>Validation başarılı ise true</returns>
    public bool IsValid(out ICollection<ValidationResult> validationResults)
    {
        validationResults = new List<ValidationResult>();
        var context = new ValidationContext(this);
        return Validator.TryValidateObject(this, context, validationResults, true);
    }
} 