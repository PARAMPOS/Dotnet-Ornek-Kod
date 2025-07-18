using System.ComponentModel.DataAnnotations;

namespace ParamApi.Sdk.Models.Requests;

/// <summary>
/// BIN sorgulama isteği
/// Kredi kartına ait kart-banka bilgisini ve SanalPOS_ID değerini döner
/// </summary>
public class BIN_SanalPos_Request
{
    /// <summary>
    /// BIN değeri (6 ya da 8 haneli)
    /// Boş bırakılırsa tüm BIN kodları döner
    /// Dolu gönderilirse o BIN koduna ait bilgiler döner
    /// </summary>
    [StringLength(8, MinimumLength = 6, ErrorMessage = "BIN değeri 6 veya 8 haneli olmalıdır")]
    public string? BIN { get; set; }

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