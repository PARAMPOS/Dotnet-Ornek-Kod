using System.ComponentModel.DataAnnotations;
using ParamApi.Sdk.Helpers;

namespace ParamApi.Sdk.Models.Requests;

/// <summary>
/// TP_Islem_Odeme_OnProv_WMD - Ön Provizyon İşlemi Başlatma Request
/// Nonsecure/3D ödeme işleminin başlatılacağı metottur
/// </summary>
public class TP_Islem_Odeme_OnProv_WMD_Request
{
    /// <summary>
    /// Kredi Kartı Sahibi
    /// </summary>
    [Required(ErrorMessage = "KK_Sahibi zorunludur")]
    [MaxLength(100, ErrorMessage = "KK_Sahibi maksimum 100 karakter olabilir")]
    public required string KK_Sahibi { get; set; }

    /// <summary>
    /// Kredi Kartı numarası
    /// </summary>
    [Required(ErrorMessage = "KK_No zorunludur")]
    [StringLength(16, MinimumLength = 16, ErrorMessage = "KK_No 16 karakter olmalıdır")]
    public required string KK_No { get; set; }

    /// <summary>
    /// 2 hane Son Kullanma Ay
    /// </summary>
    [Required(ErrorMessage = "KK_SK_Ay zorunludur")]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "KK_SK_Ay 2 karakter olmalıdır")]
    public required string KK_SK_Ay { get; set; }

    /// <summary>
    /// 4 haneli Son Kullanma Yıl
    /// </summary>
    [Required(ErrorMessage = "KK_SK_Yil zorunludur")]
    [StringLength(4, MinimumLength = 4, ErrorMessage = "KK_SK_Yil 4 karakter olmalıdır")]
    public required string KK_SK_Yil { get; set; }

    /// <summary>
    /// CVC Kodu
    /// </summary>
    [Required(ErrorMessage = "KK_CVC zorunludur")]
    [StringLength(3, MinimumLength = 3, ErrorMessage = "KK_CVC 3 karakter olmalıdır")]
    public required string KK_CVC { get; set; }

    /// <summary>
    /// Kredi Kartı Sahibi GSM No (5xxxxxxxxx format, başında 0 yok)
    /// </summary>
    [Required(ErrorMessage = "KK_Sahibi_GSM zorunludur")]
    [StringLength(10, MinimumLength = 10, ErrorMessage = "KK_Sahibi_GSM 10 karakter olmalıdır")]
    [RegularExpression(@"^5\d{9}$", ErrorMessage = "KK_Sahibi_GSM 5 ile başlamalı ve 10 haneli olmalıdır")]
    public required string KK_Sahibi_GSM { get; set; }

    /// <summary>
    /// Ödeme işlemi başarısız olursa yönlenecek sayfa adresi
    /// </summary>
    [Required(ErrorMessage = "Hata_URL zorunludur")]
    [MaxLength(256, ErrorMessage = "Hata_URL maksimum 256 karakter olabilir")]
    [Url(ErrorMessage = "Hata_URL geçerli bir URL olmalıdır")]
    public required string Hata_URL { get; set; }

    /// <summary>
    /// Ödeme işlemi başarılı olursa yönlenecek sayfa adresi
    /// </summary>
    [Required(ErrorMessage = "Basarili_URL zorunludur")]
    [MaxLength(256, ErrorMessage = "Basarili_URL maksimum 256 karakter olabilir")]
    [Url(ErrorMessage = "Basarili_URL geçerli bir URL olmalıdır")]
    public required string Basarili_URL { get; set; }

    /// <summary>
    /// Siparişe özel tekil ID
    /// </summary>
    [Required(ErrorMessage = "Siparis_ID zorunludur")]
    [MaxLength(50, ErrorMessage = "Siparis_ID maksimum 50 karakter olabilir")]
    public required string Siparis_ID { get; set; }

    /// <summary>
    /// Siparişe ait açıklama (opsiyonel)
    /// </summary>
    [MaxLength(250, ErrorMessage = "Siparis_Aciklama maksimum 250 karakter olabilir")]
    public string? Siparis_Aciklama { get; set; }

    /// <summary>
    /// Seçilen Taksit Sayısı (tek çekim için 1)
    /// </summary>
    [Range(1, 99, ErrorMessage = "Taksit 1-99 arasında olmalıdır")]
    public int Taksit { get; set; } = 1;

    /// <summary>
    /// Sipariş Tutarı (decimal olarak)
    /// </summary>
    [Range(0.01, double.MaxValue, ErrorMessage = "Islem_Tutar sıfırdan büyük olmalıdır")]
    public required decimal Islem_Tutar { get; set; }

    /// <summary>
    /// Komisyon Dahil Sipariş Tutarı (decimal olarak)
    /// </summary>
    [Range(0.01, double.MaxValue, ErrorMessage = "Toplam_Tutar sıfırdan büyük olmalıdır")]
    public required decimal Toplam_Tutar { get; set; }

    /// <summary>
    /// İşlem Güvenlik Tipi (NS: NonSecure, 3D: 3D Secure)
    /// </summary>
    [Required(ErrorMessage = "Islem_Guvenlik_Tip zorunludur")]
    [RegularExpression(@"^(NS|3D)$", ErrorMessage = "Islem_Guvenlik_Tip 'NS' veya '3D' olmalıdır")]
    public required string Islem_Guvenlik_Tip { get; set; }

    /// <summary>
    /// İşleme ait Sipariş ID haricinde tekil ID (opsiyonel)
    /// </summary>
    public string? Islem_ID { get; set; }

    /// <summary>
    /// IP Adresi
    /// </summary>
    [Required(ErrorMessage = "IPAdr zorunludur")]
    [MaxLength(50, ErrorMessage = "IPAdr maksimum 50 karakter olabilir")]
    public required string IPAdr { get; set; }

    /// <summary>
    /// Ödemenin gerçekleştiği sayfanın URL'si (opsiyonel)
    /// </summary>
    [MaxLength(256, ErrorMessage = "Ref_URL maksimum 256 karakter olabilir")]
    [Url(ErrorMessage = "Ref_URL geçerli bir URL olmalıdır")]
    public string? Ref_URL { get; set; }

    /// <summary>
    /// Extra Alan 1 (opsiyonel)
    /// </summary>
    [MaxLength(250, ErrorMessage = "Data1 maksimum 250 karakter olabilir")]
    public string? Data1 { get; set; }

    /// <summary>
    /// Extra Alan 2 (opsiyonel)
    /// </summary>
    [MaxLength(250, ErrorMessage = "Data2 maksimum 250 karakter olabilir")]
    public string? Data2 { get; set; }

    /// <summary>
    /// Extra Alan 3 (opsiyonel)
    /// </summary>
    [MaxLength(250, ErrorMessage = "Data3 maksimum 250 karakter olabilir")]
    public string? Data3 { get; set; }

    /// <summary>
    /// Extra Alan 4 (opsiyonel)
    /// </summary>
    [MaxLength(250, ErrorMessage = "Data4 maksimum 250 karakter olabilir")]
    public string? Data4 { get; set; }

    /// <summary>
    /// Extra Alan 5 (opsiyonel)
    /// </summary>
    [MaxLength(250, ErrorMessage = "Data5 maksimum 250 karakter olabilir")]
    public string? Data5 { get; set; }

    /// <summary>
    /// İşlem tutarını PARAM API formatına çevirir (virgüllü format)
    /// </summary>
    /// <returns>Virgüllü format string (örn: "1000,50")</returns>
    public string GetFormattedIslemTutar()
    {
        return HashHelper.FormatAmount(Islem_Tutar);
    }

    /// <summary>
    /// Toplam tutarı PARAM API formatına çevirir (virgüllü format)
    /// </summary>
    /// <returns>Virgüllü format string (örn: "1000,50")</returns>
    public string GetFormattedToplamTutar()
    {
        return HashHelper.FormatAmount(Toplam_Tutar);
    }

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
} 