using System.ComponentModel.DataAnnotations;

namespace ParamApi.Sdk.Models.Requests;

/// <summary>
/// İşlem izleme isteği
/// Yapılan işlemlerin belirli tarih aralığında izlenmesi için kullanılır
/// </summary>
public class TP_Islem_Izleme_Request
{
    /// <summary>
    /// Başlangıç Tarihi (dd.MM.yyyy HH:mm:ss formatında - örn: 20.11.2015 00:00:00)
    /// </summary>
    [Required(ErrorMessage = "Tarih_Bas alanı zorunludur")]
    [StringLength(20, ErrorMessage = "Tarih_Bas en fazla 20 karakter olabilir")]
    public string Tarih_Bas { get; set; } = string.Empty;

    /// <summary>
    /// Bitiş Tarihi (dd.MM.yyyy HH:mm:ss formatında - örn: 20.11.2015 15:15:00)
    /// </summary>
    [Required(ErrorMessage = "Tarih_Bit alanı zorunludur")]
    [StringLength(20, ErrorMessage = "Tarih_Bit en fazla 20 karakter olabilir")]
    public string Tarih_Bit { get; set; } = string.Empty;

    /// <summary>
    /// İşlem Tipi - İptal, İade, Satış
    /// Gönderilmediği taktirde hepsi döner ama süre uzar
    /// </summary>
    public string Islem_Tip { get; set; } = string.Empty;

    /// <summary>
    /// İşlem Durumu - Başarılı, Başarısız
    /// Gönderilmediği taktirde hepsi döner ama süre uzar
    /// </summary>
    public string Islem_Durum { get; set; } = string.Empty;

    /// <summary>
    /// Request'in doğrulamasını yapar
    /// </summary>
    public bool IsValid(out List<ValidationResult> validationResults)
    {
        validationResults = new List<ValidationResult>();
        var context = new ValidationContext(this);
        
        bool isValid = Validator.TryValidateObject(this, context, validationResults, true);

        // Tarih formatı validasyonu
        if (!string.IsNullOrWhiteSpace(Tarih_Bas) && !IsValidDateFormat(Tarih_Bas))
        {
            validationResults.Add(new ValidationResult(
                "Tarih_Bas formatı 'dd.MM.yyyy HH:mm:ss' şeklinde olmalıdır (örn: 20.11.2015 00:00:00)",
                new[] { nameof(Tarih_Bas) }));
            isValid = false;
        }

        if (!string.IsNullOrWhiteSpace(Tarih_Bit) && !IsValidDateFormat(Tarih_Bit))
        {
            validationResults.Add(new ValidationResult(
                "Tarih_Bit formatı 'dd.MM.yyyy HH:mm:ss' şeklinde olmalıdır (örn: 20.11.2015 15:15:00)",
                new[] { nameof(Tarih_Bit) }));
            isValid = false;
        }

        return isValid;
    }

    /// <summary>
    /// Tarih formatını kontrol eder
    /// </summary>
    private bool IsValidDateFormat(string dateString)
    {
        return DateTime.TryParseExact(dateString, "dd.MM.yyyy HH:mm:ss", 
            System.Globalization.CultureInfo.InvariantCulture, 
            System.Globalization.DateTimeStyles.None, out _);
    }
} 