using System.ComponentModel.DataAnnotations;

namespace ParamApi.Sdk.Models.Requests;

/// <summary>
/// Başarılı bir kredi kartı işleminin kısmi iptal veya iadesini yapmak için kullanılan request modeli
/// İptal işlemi ödeme işleminin gerçekleştiği gün, iade işlemi ödeme işlemi gün sonuna girdikten sonraki günlerde yapılır
/// </summary>
public class TP_Islem_Iptal_Iade_Kismi2_Request
{
    /// <summary>
    /// İşlem durumu (İptal veya İade)
    /// </summary>
    [Required(ErrorMessage = "Durum zorunludur")]
    public IptalIadeDurum Durum { get; set; }

    /// <summary>
    /// İşlemin Sipariş ID Değeri
    /// </summary>
    [Required(ErrorMessage = "Siparis_ID zorunludur")]
    public required string Siparis_ID { get; set; }

    /// <summary>
    /// İptal/İade Tutarı
    /// İPTAL için tüm tutar yazılmalıdır
    /// İADE için tüm tutar veya daha küçük tutar (kısmi) yazılmalıdır
    /// </summary>
    [Required(ErrorMessage = "Tutar zorunludur")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Tutar 0'dan büyük olmalıdır")]
    public decimal Tutar { get; set; }

    /// <summary>
    /// Model validasyonunu kontrol eder
    /// </summary>
    /// <param name="validationResults">Validation sonuçları</param>
    /// <returns>Model geçerliyse true</returns>
    public bool IsValid(out ICollection<ValidationResult> validationResults)
    {
        validationResults = new List<ValidationResult>();
        var context = new ValidationContext(this);
        return Validator.TryValidateObject(this, context, validationResults, true);
    }

    /// <summary>
    /// Durum enum'ını PARAM API string formatına çevirir
    /// </summary>
    /// <returns>PARAM API durum string değeri</returns>
    public string GetDurumString() => Durum.ToParamString();

    /// <summary>
    /// Tutarı double formatına çevirir (PARAM API uyumluluğu için)
    /// </summary>
    /// <returns>Double tutar değeri</returns>
    public double GetTutarDouble() => (double)Tutar;
} 