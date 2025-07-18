using System.ComponentModel.DataAnnotations;

namespace ParamApi.Sdk.Models.Requests;

/// <summary>
/// TP_Islem_Sorgulama4 metodu için istek modeli
/// İşlemin başarılı, başarısız, iptal veya iade durumunda olduğunu öğrenmek için kullanılır
/// </summary>
public class TP_Islem_Sorgulama4_Request
{
    /// <summary>
    /// Başarılı işlem sonrası POST edilen Dekont_ID (opsiyonel)
    /// </summary>
    public string? Dekont_ID { get; set; }

    /// <summary>
    /// Başarılı işlem sonrası POST edilen Sipariş ID (opsiyonel)
    /// </summary>
    public string? Siparis_ID { get; set; }

    /// <summary>
    /// TP_Islem_Odeme metoduna gönderilen İşlem ID (opsiyonel)
    /// </summary>
    public string? Islem_ID { get; set; }

    /// <summary>
    /// Request'in geçerli olup olmadığını kontrol eder
    /// En az bir parametre (Dekont_ID, Siparis_ID veya Islem_ID) gönderilmelidir
    /// </summary>
    /// <param name="validationResults">Validation hata listesi</param>
    /// <returns>Geçerli ise true</returns>
    public bool IsValid(out List<ValidationResult> validationResults)
    {
        validationResults = new List<ValidationResult>();
        var context = new ValidationContext(this);

        // Custom validation: En az bir parametre olmalı
        if (string.IsNullOrWhiteSpace(Dekont_ID) && 
            string.IsNullOrWhiteSpace(Siparis_ID) && 
            string.IsNullOrWhiteSpace(Islem_ID))
        {
            validationResults.Add(new ValidationResult(
                "En az bir parametre gönderilmelidir: Dekont_ID, Siparis_ID veya Islem_ID",
                new[] { nameof(Dekont_ID), nameof(Siparis_ID), nameof(Islem_ID) }));
            return false;
        }

        return Validator.TryValidateObject(this, context, validationResults, true);
    }
} 