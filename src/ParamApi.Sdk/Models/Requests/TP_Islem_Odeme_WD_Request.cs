using System.ComponentModel.DataAnnotations;
using System.Globalization;
using ParamApi.Sdk.Configuration;

namespace ParamApi.Sdk.Models.Requests;

/// <summary>
/// TP_Islem_Odeme_WD (Dövizli Ödeme İşlemi) request model
/// Bu metot sadece yabancı kartlar ile çalışmaktadır
/// </summary>
public class TP_Islem_Odeme_WD_Request
{
    /// <summary>
    /// Döviz Kodu (Zorunlu)
    /// 1000: TRL, 1001: USD, 1002: EUR, 1003: GBP
    /// </summary>
    [Required(ErrorMessage = "Doviz_Kodu zorunludur")]
    public DovizKodu Doviz_Kodu { get; set; } = DovizKodu.TRL;

    /// <summary>
    /// Kredi Kartı Sahibi (Zorunlu)
    /// Maksimum 100 karakter
    /// </summary>
    [Required(ErrorMessage = "KK_Sahibi zorunludur")]
    [MaxLength(100, ErrorMessage = "KK_Sahibi maksimum 100 karakter olabilir")]
    public required string KK_Sahibi { get; set; }

    /// <summary>
    /// Kredi Kartı Numarası (Zorunlu)
    /// 16 haneli kart numarası
    /// </summary>
    [Required(ErrorMessage = "KK_No zorunludur")]
    [StringLength(16, MinimumLength = 16, ErrorMessage = "KK_No 16 haneli olmalıdır")]
    public required string KK_No { get; set; }

    /// <summary>
    /// Son Kullanma Ay (Zorunlu)
    /// 2 haneli ay (01-12)
    /// </summary>
    [Required(ErrorMessage = "KK_SK_Ay zorunludur")]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "KK_SK_Ay 2 haneli olmalıdır")]
    [RegularExpression(@"^(0[1-9]|1[0-2])$", ErrorMessage = "KK_SK_Ay geçerli bir ay olmalıdır (01-12)")]
    public required string KK_SK_Ay { get; set; }

    /// <summary>
    /// Son Kullanma Yıl (Zorunlu)
    /// 4 haneli yıl
    /// </summary>
    [Required(ErrorMessage = "KK_SK_Yil zorunludur")]
    [StringLength(4, MinimumLength = 4, ErrorMessage = "KK_SK_Yil 4 haneli olmalıdır")]
    public required string KK_SK_Yil { get; set; }

    /// <summary>
    /// CVC Kodu (Zorunlu)
    /// 3 haneli CVC kodu
    /// </summary>
    [Required(ErrorMessage = "KK_CVC zorunludur")]
    [StringLength(3, MinimumLength = 3, ErrorMessage = "KK_CVC 3 haneli olmalıdır")]
    public required string KK_CVC { get; set; }

    /// <summary>
    /// Kredi Kartı Sahibi GSM No (Zorunlu)
    /// Başında 0 olmadan 10 haneli (5xxxxxxxxx)
    /// </summary>
    [Required(ErrorMessage = "KK_Sahibi_GSM zorunludur")]
    [StringLength(10, MinimumLength = 10, ErrorMessage = "KK_Sahibi_GSM 10 haneli olmalıdır")]
    [RegularExpression(@"^5\d{9}$", ErrorMessage = "KK_Sahibi_GSM geçerli bir cep telefonu numarası olmalıdır (5xxxxxxxxx)")]
    public required string KK_Sahibi_GSM { get; set; }

    /// <summary>
    /// Hata URL'si (Zorunlu)
    /// Ödeme başarısız olursa yönlenecek sayfa adresi
    /// Maksimum 256 karakter
    /// </summary>
    [Required(ErrorMessage = "Hata_URL zorunludur")]
    [MaxLength(256, ErrorMessage = "Hata_URL maksimum 256 karakter olabilir")]
    [Url(ErrorMessage = "Hata_URL geçerli bir URL olmalıdır")]
    public required string Hata_URL { get; set; }

    /// <summary>
    /// Başarılı URL'si (Zorunlu)
    /// Ödeme başarılı olursa yönlenecek sayfa adresi
    /// Maksimum 256 karakter
    /// </summary>
    [Required(ErrorMessage = "Basarili_URL zorunludur")]
    [MaxLength(256, ErrorMessage = "Basarili_URL maksimum 256 karakter olabilir")]
    [Url(ErrorMessage = "Basarili_URL geçerli bir URL olmalıdır")]
    public required string Basarili_URL { get; set; }

    /// <summary>
    /// Sipariş ID'si (Zorunlu)
    /// Maksimum 50 karakter tekil ID
    /// </summary>
    [Required(ErrorMessage = "Siparis_ID zorunludur")]
    [MaxLength(50, ErrorMessage = "Siparis_ID maksimum 50 karakter olabilir")]
    public required string Siparis_ID { get; set; }

    /// <summary>
    /// Sipariş Açıklaması (Zorunlu)
    /// Maksimum 250 karakter
    /// </summary>
    [Required(ErrorMessage = "Siparis_Aciklama zorunludur")]
    [MaxLength(250, ErrorMessage = "Siparis_Aciklama maksimum 250 karakter olabilir")]
    public required string Siparis_Aciklama { get; set; }

    /// <summary>
    /// İşlem Tutarı (Zorunlu)
    /// Sadece virgüllü kuruş formatında (1000,50)
    /// </summary>
    [Required(ErrorMessage = "Islem_Tutar zorunludur")]
    public decimal Islem_Tutar { get; set; }

    /// <summary>
    /// Toplam Tutar (Zorunlu)
    /// Komisyon dahil tutar, virgüllü kuruş formatında (1000,50)
    /// </summary>
    [Required(ErrorMessage = "Toplam_Tutar zorunludur")]
    public decimal Toplam_Tutar { get; set; }

    /// <summary>
    /// İşlem Güvenlik Tipi (Zorunlu)
    /// "NS" (NonSecure) veya "3D" 
    /// </summary>
    [Required(ErrorMessage = "Islem_Guvenlik_Tip zorunludur")]
    [RegularExpression(@"^(NS|3D)$", ErrorMessage = "Islem_Guvenlik_Tip 'NS' veya '3D' olmalıdır")]
    public required string Islem_Guvenlik_Tip { get; set; } = "3D";

    /// <summary>
    /// IP Adresi (Zorunlu)
    /// Maksimum 50 karakter
    /// </summary>
    [Required(ErrorMessage = "IPAdr zorunludur")]
    [MaxLength(50, ErrorMessage = "IPAdr maksimum 50 karakter olabilir")]
    public required string IPAdr { get; set; }

    /// <summary>
    /// İşlem ID (Opsiyonel)
    /// İşleme ait sipariş ID haricinde tekil ID
    /// </summary>
    public string? Islem_ID { get; set; }

    /// <summary>
    /// Referans URL (Opsiyonel)
    /// Ödemenin gerçekleştiği sayfanın URL'si
    /// Maksimum 256 karakter
    /// </summary>
    [MaxLength(256, ErrorMessage = "Ref_URL maksimum 256 karakter olabilir")]
    public string? Ref_URL { get; set; }

    /// <summary>
    /// Extra Alan 1 (Opsiyonel)
    /// Maksimum 250 karakter
    /// </summary>
    [MaxLength(250, ErrorMessage = "Data1 maksimum 250 karakter olabilir")]
    public string? Data1 { get; set; }

    /// <summary>
    /// Extra Alan 2 (Opsiyonel)
    /// Maksimum 250 karakter
    /// </summary>
    [MaxLength(250, ErrorMessage = "Data2 maksimum 250 karakter olabilir")]
    public string? Data2 { get; set; }

    /// <summary>
    /// Extra Alan 3 (Opsiyonel)
    /// Maksimum 250 karakter
    /// </summary>
    [MaxLength(250, ErrorMessage = "Data3 maksimum 250 karakter olabilir")]
    public string? Data3 { get; set; }

    /// <summary>
    /// Extra Alan 4 (Opsiyonel)
    /// Maksimum 250 karakter
    /// </summary>
    [MaxLength(250, ErrorMessage = "Data4 maksimum 250 karakter olabilir")]
    public string? Data4 { get; set; }

    /// <summary>
    /// Extra Alan 5 (Opsiyonel)
    /// Maksimum 250 karakter
    /// </summary>
    [MaxLength(250, ErrorMessage = "Data5 maksimum 250 karakter olabilir")]
    public string? Data5 { get; set; }

    /// <summary>
    /// İşlem tutarını PARAM API formatına çevirir (virgüllü format)
    /// </summary>
    /// <returns>Virgüllü format tutar</returns>
    public string GetFormattedIslemTutar()
    {
        return Islem_Tutar.ToString("F2", new CultureInfo("tr-TR"));
    }

    /// <summary>
    /// Toplam tutarı PARAM API formatına çevirir (virgüllü format)
    /// </summary>
    /// <returns>Virgüllü format tutar</returns>
    public string GetFormattedToplamTutar()
    {
        return Toplam_Tutar.ToString("F2", new CultureInfo("tr-TR"));
    }

    /// <summary>
    /// Request validation kontrolü
    /// </summary>
    /// <param name="validationResults">Validation sonuçları</param>
    /// <returns>Validation başarılı mı?</returns>
    public bool IsValid(out List<ValidationResult> validationResults)
    {
        validationResults = new List<ValidationResult>();
        var context = new ValidationContext(this);
        return Validator.TryValidateObject(this, context, validationResults, true);
    }
} 