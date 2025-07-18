using System.ComponentModel.DataAnnotations;

namespace ParamApi.Sdk.Models.Requests;

/// <summary>
/// TP_Islem_Iptal_OnProv (Ön Provizyon İptali) request model
/// Satış işlemi yapılmamış provizyon iptali için kullanılır
/// </summary>
public class TP_Islem_Iptal_OnProv_Request
{
    /// <summary>
    /// Provizyon ID (Opsiyonel)
    /// Boş geçilebilir
    /// </summary>
    public string? Prov_ID { get; set; }

    /// <summary>
    /// Sipariş ID Değeri (Zorunlu)
    /// </summary>
    [Required(ErrorMessage = "Siparis_ID zorunludur")]
    public required string Siparis_ID { get; set; }

    /// <summary>
    /// Request validation kontrolü
    /// </summary>
    /// <param name="validationResults">Validation sonuçları</param>
    /// <returns>Validation başarılı mı?</returns>
    public bool IsValid(out ICollection<ValidationResult> validationResults)
    {
        validationResults = new List<ValidationResult>();
        var context = new ValidationContext(this);
        return Validator.TryValidateObject(this, context, validationResults, true);
    }

    /// <summary>
    /// String representation
    /// </summary>
    public override string ToString()
    {
        return $"TP_Islem_Iptal_OnProv - Prov ID: {Prov_ID ?? "Boş"}, Sipariş ID: {Siparis_ID}";
    }
} 