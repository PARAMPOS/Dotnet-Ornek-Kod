using System.ComponentModel.DataAnnotations;

namespace ParamApi.Sdk.Models.Requests;

/// <summary>
/// TP_WMD_Pay - 3D İşlemini Tamamlama Request
/// Doğrulaması yapılan kartlardan tutar çekimi yapmak için kullanılır
/// </summary>
public class TP_WMD_PAY_Request
{
    /// <summary>
    /// Banka 3D MD değeri (TP_WMD_UCD'den dönen)
    /// </summary>
    [Required(ErrorMessage = "UCD_MD zorunludur")]
    public required string UCD_MD { get; set; }
    
    /// <summary>
    /// İşlem GUID değeri (TP_WMD_UCD'den dönen)
    /// </summary>
    [Required(ErrorMessage = "Islem_GUID zorunludur")]
    public required string Islem_GUID { get; set; }
    
    /// <summary>
    /// Sipariş ID değeri (TP_WMD_UCD'de kullanılan aynı değer)
    /// </summary>
    [Required(ErrorMessage = "Siparis_ID zorunludur")]
    public required string Siparis_ID { get; set; }
    
    /// <summary>
    /// Request validation
    /// </summary>
    public bool IsValid(out List<ValidationResult> validationResults)
    {
        validationResults = new List<ValidationResult>();
        var context = new ValidationContext(this);
        return Validator.TryValidateObject(this, context, validationResults, true);
    }
} 