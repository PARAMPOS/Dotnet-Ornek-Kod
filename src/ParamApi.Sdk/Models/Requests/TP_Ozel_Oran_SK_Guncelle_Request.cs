using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ParamApi.Sdk.Models.Requests;

/// <summary>
/// Özel oran son kullanıcı güncelleme request modeli
/// Üye işyerinin müşteri oranlarını güncellemesi için kullanılır
/// </summary>
public class TP_Ozel_Oran_SK_Guncelle_Request
{
    /// <summary>
    /// Özel oran SK ID (TP_Ozel_Oran_SK_Liste'den dönen ID değeri)
    /// </summary>
    [Required(ErrorMessage = "Ozel_Oran_SK_ID zorunludur")]
    public required long Ozel_Oran_SK_ID { get; set; }

    /// <summary>
    /// Tek Çekim Oranı
    /// Format: Virgüllü (1,25 gibi) - nokta otomatik virgüle çevrilir
    /// </summary>
    [Required(ErrorMessage = "MO_1 zorunludur")]
    public required string MO_1 { get; set; }

    /// <summary>
    /// 2. Taksit Oranı (nokta otomatik virgüle çevrilir)
    /// </summary>
    [Required(ErrorMessage = "MO_2 zorunludur")]
    public required string MO_2 { get; set; }

    /// <summary>
    /// 3. Taksit Oranı (nokta otomatik virgüle çevrilir)
    /// </summary>
    [Required(ErrorMessage = "MO_3 zorunludur")]
    public required string MO_3 { get; set; }

    /// <summary>
    /// 4. Taksit Oranı (nokta otomatik virgüle çevrilir)
    /// </summary>
    [Required(ErrorMessage = "MO_4 zorunludur")]
    public required string MO_4 { get; set; }

    /// <summary>
    /// 5. Taksit Oranı (nokta otomatik virgüle çevrilir)
    /// </summary>
    [Required(ErrorMessage = "MO_5 zorunludur")]
    public required string MO_5 { get; set; }

    /// <summary>
    /// 6. Taksit Oranı (nokta otomatik virgüle çevrilir)
    /// </summary>
    [Required(ErrorMessage = "MO_6 zorunludur")]
    public required string MO_6 { get; set; }

    /// <summary>
    /// 7. Taksit Oranı (nokta otomatik virgüle çevrilir)
    /// </summary>
    [Required(ErrorMessage = "MO_7 zorunludur")]
    public required string MO_7 { get; set; }

    /// <summary>
    /// 8. Taksit Oranı (nokta otomatik virgüle çevrilir)
    /// </summary>
    [Required(ErrorMessage = "MO_8 zorunludur")]
    public required string MO_8 { get; set; }

    /// <summary>
    /// 9. Taksit Oranı (nokta otomatik virgüle çevrilir)
    /// </summary>
    [Required(ErrorMessage = "MO_9 zorunludur")]
    public required string MO_9 { get; set; }

    /// <summary>
    /// 10. Taksit Oranı (nokta otomatik virgüle çevrilir)
    /// </summary>
    [Required(ErrorMessage = "MO_10 zorunludur")]
    public required string MO_10 { get; set; }

    /// <summary>
    /// 11. Taksit Oranı (nokta otomatik virgüle çevrilir)
    /// </summary>
    [Required(ErrorMessage = "MO_11 zorunludur")]
    public required string MO_11 { get; set; }

    /// <summary>
    /// 12. Taksit Oranı (nokta otomatik virgüle çevrilir)
    /// </summary>
    [Required(ErrorMessage = "MO_12 zorunludur")]
    public required string MO_12 { get; set; }

    /// <summary>
    /// Request'in geçerli olup olmadığını kontrol eder
    /// </summary>
    /// <param name="validationResults">Validation hata listesi</param>
    /// <returns>Geçerli ise true</returns>
    public bool IsValid(out List<ValidationResult> validationResults)
    {
        validationResults = new List<ValidationResult>();
        var context = new ValidationContext(this);
        
        bool isValid = Validator.TryValidateObject(this, context, validationResults, true);
        
        // Ek format validasyonları
        ValidateRateFormat(nameof(MO_1), MO_1, validationResults);
        ValidateRateFormat(nameof(MO_2), MO_2, validationResults);
        ValidateRateFormat(nameof(MO_3), MO_3, validationResults);
        ValidateRateFormat(nameof(MO_4), MO_4, validationResults);
        ValidateRateFormat(nameof(MO_5), MO_5, validationResults);
        ValidateRateFormat(nameof(MO_6), MO_6, validationResults);
        ValidateRateFormat(nameof(MO_7), MO_7, validationResults);
        ValidateRateFormat(nameof(MO_8), MO_8, validationResults);
        ValidateRateFormat(nameof(MO_9), MO_9, validationResults);
        ValidateRateFormat(nameof(MO_10), MO_10, validationResults);
        ValidateRateFormat(nameof(MO_11), MO_11, validationResults);
        ValidateRateFormat(nameof(MO_12), MO_12, validationResults);
        
        return isValid && validationResults.Count == 0;
    }

    /// <summary>
    /// Oran formatını valide eder (basit format kontrolü)
    /// </summary>
    private static void ValidateRateFormat(string fieldName, string value, List<ValidationResult> validationResults)
    {
        if (string.IsNullOrWhiteSpace(value))
            return;
    }

    /// <summary>
    /// Tüm oranları PARAM API formatına dönüştürür (nokta -> virgül)
    /// </summary>
    public void NormalizeRateFormats()
    {
        MO_1 = NormalizeRate(MO_1);
        MO_2 = NormalizeRate(MO_2);
        MO_3 = NormalizeRate(MO_3);
        MO_4 = NormalizeRate(MO_4);
        MO_5 = NormalizeRate(MO_5);
        MO_6 = NormalizeRate(MO_6);
        MO_7 = NormalizeRate(MO_7);
        MO_8 = NormalizeRate(MO_8);
        MO_9 = NormalizeRate(MO_9);
        MO_10 = NormalizeRate(MO_10);
        MO_11 = NormalizeRate(MO_11);
        MO_12 = NormalizeRate(MO_12);
    }

    /// <summary>
    /// Tek bir oran değerini normalize eder (nokta -> virgül)
    /// </summary>
    private static string NormalizeRate(string rate)
    {
        if (string.IsNullOrWhiteSpace(rate))
            return rate;

        // Nokta varsa virgüle çevir
        return rate.Replace('.', ',');
    }

    /// <summary>
    /// Taksit oranını güncelleme helper metodu
    /// </summary>
    /// <param name="taksitSayisi">Taksit sayısı (1-12)</param>
    /// <param name="oran">Yeni oran değeri</param>
    public void SetTaksitOrani(int taksitSayisi, string oran)
    {
        switch (taksitSayisi)
        {
            case 1: MO_1 = oran; break;
            case 2: MO_2 = oran; break;
            case 3: MO_3 = oran; break;
            case 4: MO_4 = oran; break;
            case 5: MO_5 = oran; break;
            case 6: MO_6 = oran; break;
            case 7: MO_7 = oran; break;
            case 8: MO_8 = oran; break;
            case 9: MO_9 = oran; break;
            case 10: MO_10 = oran; break;
            case 11: MO_11 = oran; break;
            case 12: MO_12 = oran; break;
            default: throw new ArgumentException("Taksit sayısı 1-12 arasında olmalıdır", nameof(taksitSayisi));
        }
    }

    /// <summary>
    /// Taksit oranını getirme helper metodu
    /// </summary>
    /// <param name="taksitSayisi">Taksit sayısı (1-12)</param>
    /// <returns>Oran değeri</returns>
    public string GetTaksitOrani(int taksitSayisi)
    {
        return taksitSayisi switch
        {
            1 => MO_1,
            2 => MO_2,
            3 => MO_3,
            4 => MO_4,
            5 => MO_5,
            6 => MO_6,
            7 => MO_7,
            8 => MO_8,
            9 => MO_9,
            10 => MO_10,
            11 => MO_11,
            12 => MO_12,
            _ => throw new ArgumentException("Taksit sayısı 1-12 arasında olmalıdır", nameof(taksitSayisi))
        };
    }
} 