using System.ComponentModel.DataAnnotations;
using ParamApi.Sdk.Helpers;

namespace ParamApi.Sdk.Models.Requests;

/// <summary>
/// TP_WMD_UCD (Nonsecure/3D Ödeme) request modeli
/// PARAM API format kurallarına uygun
/// </summary>
public class TP_WMD_UCD_Request
{
    /// <summary>
    /// Kredi Kartı Sahibi (maks 100 karakter)
    /// </summary>
    [Required(ErrorMessage = "KK_Sahibi zorunludur")]
    [MaxLength(100, ErrorMessage = "KK_Sahibi maksimum 100 karakter olabilir")]
    public required string KK_Sahibi { get; set; }

    /// <summary>
    /// Kredi Kartı Numarası (16 hane)
    /// </summary>
    [Required(ErrorMessage = "KK_No zorunludur")]
    [StringLength(16, MinimumLength = 16, ErrorMessage = "KK_No 16 hane olmalıdır")]
    public required string KK_No { get; set; }

    /// <summary>
    /// Son Kullanma Ayı (2 hane: 01-12)
    /// </summary>
    [Required(ErrorMessage = "KK_SK_Ay zorunludur")]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "KK_SK_Ay 2 hane olmalıdır")]
    [RegularExpression(@"^(0[1-9]|1[0-2])$", ErrorMessage = "KK_SK_Ay 01-12 arasında olmalıdır")]
    public required string KK_SK_Ay { get; set; }

    /// <summary>
    /// Son Kullanma Yılı (4 hane)
    /// </summary>
    [Required(ErrorMessage = "KK_SK_Yil zorunludur")]
    [StringLength(4, MinimumLength = 4, ErrorMessage = "KK_SK_Yil 4 hane olmalıdır")]
    public required string KK_SK_Yil { get; set; }

    /// <summary>
    /// CVC Kodu (3 hane)
    /// </summary>
    [Required(ErrorMessage = "KK_CVC zorunludur")]
    [StringLength(3, MinimumLength = 3, ErrorMessage = "KK_CVC 3 hane olmalıdır")]
    public required string KK_CVC { get; set; }

    /// <summary>
    /// Kredi Kartı Sahibi GSM No (5xxxxxxxxx formatında, başında 0 olmadan)
    /// </summary>
    [StringLength(10, MinimumLength = 10, ErrorMessage = "KK_Sahibi_GSM 10 hane olmalıdır")]
    [RegularExpression(@"^5\d{9}$", ErrorMessage = "KK_Sahibi_GSM 5 ile başlayan 10 haneli olmalıdır")]
    public string? KK_Sahibi_GSM { get; set; }

    /// <summary>
    /// Ödeme işlemi başarısız olursa yönlenecek sayfa adresi (maks 256 karakter)
    /// </summary>
    [Required(ErrorMessage = "Hata_URL zorunludur")]
    [MaxLength(256, ErrorMessage = "Hata_URL maksimum 256 karakter olabilir")]
    [Url(ErrorMessage = "Hata_URL geçerli bir URL olmalıdır")]
    public required string Hata_URL { get; set; }

    /// <summary>
    /// Ödeme işlemi başarılı olursa yönlenecek sayfa adresi (maks 256 karakter)
    /// </summary>
    [Required(ErrorMessage = "Basarili_URL zorunludur")]
    [MaxLength(256, ErrorMessage = "Basarili_URL maksimum 256 karakter olabilir")]
    [Url(ErrorMessage = "Basarili_URL geçerli bir URL olmalıdır")]
    public required string Basarili_URL { get; set; }

    /// <summary>
    /// Siparişe özel tekil ID (maks 50 karakter)
    /// Bu değeri daha önce gönderdiyseniz sistem yeni Siparis_ID atar
    /// </summary>
    [Required(ErrorMessage = "Siparis_ID zorunludur")]
    [MaxLength(50, ErrorMessage = "Siparis_ID maksimum 50 karakter olabilir")]
    public required string Siparis_ID { get; set; }

    /// <summary>
    /// Siparişe ait açıklama (maks 250 karakter)
    /// </summary>
    [MaxLength(250, ErrorMessage = "Siparis_Aciklama maksimum 250 karakter olabilir")]
    public string? Siparis_Aciklama { get; set; }

    /// <summary>
    /// Seçilen Taksit Sayısı (tek çekim için 1)
    /// </summary>
    [Required(ErrorMessage = "Taksit zorunludur")]
    [Range(1, 99, ErrorMessage = "Taksit 1-99 arasında olmalıdır")]
    public int Taksit { get; set; } = 1;

    /// <summary>
    /// Sipariş Tutarı (decimal - otomatik virgüllü formata çevrilir)
    /// </summary>
    [Required(ErrorMessage = "Islem_Tutar zorunludur")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Islem_Tutar 0'dan büyük olmalıdır")]
    public decimal Islem_Tutar { get; set; }

    /// <summary>
    /// Komisyon Dahil Sipariş Tutarı (decimal - otomatik virgüllü formata çevrilir)
    /// Eğer null ise Islem_Tutar ile aynı değer kullanılır
    /// </summary>
    public decimal? Toplam_Tutar { get; set; }

    /// <summary>
    /// Güvenlik Tipi: "NS" (NonSecure) veya "3D"
    /// </summary>
    [Required(ErrorMessage = "Islem_Guvenlik_Tip zorunludur")]
    [RegularExpression(@"^(NS|3D)$", ErrorMessage = "Islem_Guvenlik_Tip 'NS' veya '3D' olmalıdır")]
    public required string Islem_Guvenlik_Tip { get; set; }

    /// <summary>
    /// İşleme ait Sipariş ID haricinde tekil ID (opsiyonel)
    /// </summary>
    public string? Islem_ID { get; set; }

    /// <summary>
    /// IP Adresi (maks 50 karakter)
    /// </summary>
    [Required(ErrorMessage = "IPAdr zorunludur")]
    [MaxLength(50, ErrorMessage = "IPAdr maksimum 50 karakter olabilir")]
    public required string IPAdr { get; set; }

    /// <summary>
    /// Ödemenin gerçekleştiği sayfanın URL'si (maks 256 karakter)
    /// </summary>
    [MaxLength(256, ErrorMessage = "Ref_URL maksimum 256 karakter olabilir")]
    public string? Ref_URL { get; set; }

    /// <summary>
    /// Extra Alan 1 (maks 250 karakter)
    /// MCC "5960, 6300, 6363" için TCKN/VKN/YKN
    /// </summary>
    [MaxLength(250, ErrorMessage = "Data1 maksimum 250 karakter olabilir")]
    public string? Data1 { get; set; }

    /// <summary>
    /// Extra Alan 2 (maks 250 karakter)
    /// </summary>
    [MaxLength(250, ErrorMessage = "Data2 maksimum 250 karakter olabilir")]
    public string? Data2 { get; set; }

    /// <summary>
    /// Extra Alan 3 (maks 250 karakter)
    /// </summary>
    [MaxLength(250, ErrorMessage = "Data3 maksimum 250 karakter olabilir")]
    public string? Data3 { get; set; }

    /// <summary>
    /// Extra Alan 4 (maks 250 karakter)
    /// </summary>
    [MaxLength(250, ErrorMessage = "Data4 maksimum 250 karakter olabilir")]
    public string? Data4 { get; set; }

    /// <summary>
    /// Extra Alan 5 (maks 250 karakter)
    /// </summary>
    [MaxLength(250, ErrorMessage = "Data5 maksimum 250 karakter olabilir")]
    public string? Data5 { get; set; }

    /// <summary>
    /// Komisyon oranı ile toplam tutarı otomatik hesaplar
    /// </summary>
    /// <param name="komisyonOran">Komisyon oranı (yüzde)</param>
    public void SetToplamTutarWithKomisyon(decimal komisyonOran)
    {
        Toplam_Tutar = HashHelper.CalculateToplamTutar(Islem_Tutar, komisyonOran);
    }

    /// <summary>
    /// İşlem tutarını PARAM API formatına çevirir (virgüllü format)
    /// </summary>
    /// <returns>Virgüllü format string</returns>
    public string GetFormattedIslemTutar()
    {
        return HashHelper.FormatAmount(Islem_Tutar);
    }

    /// <summary>
    /// Toplam tutarı PARAM API formatına çevirir (virgüllü format)
    /// </summary>
    /// <returns>Virgüllü format string</returns>
    public string GetFormattedToplamTutar()
    {
        var tutar = Toplam_Tutar ?? Islem_Tutar;
        return HashHelper.FormatAmount(tutar);
    }

    /// <summary>
    /// Request'in geçerli olup olmadığını kontrol eder
    /// </summary>
    /// <returns>Validation sonucu</returns>
    public bool IsValid(out List<ValidationResult> validationResults)
    {
        validationResults = new List<ValidationResult>();
        var context = new ValidationContext(this, serviceProvider: null, items: null);
        return Validator.TryValidateObject(this, context, validationResults, true);
    }
} 