using System.ComponentModel.DataAnnotations;

namespace ParamApi.Sdk.Models.Requests;

/// <summary>
/// Saklanmış kredi kartından tahsilat isteği
/// Daha önce KS_Kart_Ekle ile saklanan kartlardan ödeme almak için kullanılır
/// </summary>
public class KS_Tahsilat_Request
{
    /// <summary>
    /// KS_Kart_Ekle metodundan dönen GUID değeri
    /// </summary>
    [Required(ErrorMessage = "KS_GUID alanı zorunludur")]
    [StringLength(36, ErrorMessage = "KS_GUID 36 karakter olmalıdır")]
    public string KS_GUID { get; set; } = string.Empty;

    /// <summary>
    /// 3D işlemler için CVV değeri (3 haneli)
    /// CVV değeri boş geçilebilir
    /// </summary>
    [StringLength(3, ErrorMessage = "CVV en fazla 3 haneli olabilir")]
    public string? CVV { get; set; }

    /// <summary>
    /// Kredi Kartı Sahibi GSM No (Başında 0 olmadan - 5xxxxxxxxx formatında)
    /// </summary>
    [Required(ErrorMessage = "KK_Sahibi_GSM alanı zorunludur")]
    [StringLength(10, MinimumLength = 10, ErrorMessage = "KK_Sahibi_GSM tam 10 haneli olmalıdır")]
    [RegularExpression(@"^5\d{9}$", ErrorMessage = "KK_Sahibi_GSM 5 ile başlayıp 10 haneli olmalıdır")]
    public string KK_Sahibi_GSM { get; set; } = string.Empty;

    /// <summary>
    /// Ödeme işlemi başarısız olursa yönlenecek sayfa adresi
    /// </summary>
    [Required(ErrorMessage = "Hata_URL alanı zorunludur")]
    [StringLength(250, ErrorMessage = "Hata_URL en fazla 250 karakter olabilir")]
    [Url(ErrorMessage = "Hata_URL geçerli bir URL olmalıdır")]
    public string Hata_URL { get; set; } = string.Empty;

    /// <summary>
    /// Ödeme işlemi başarılı olursa yönlenecek sayfa adresi
    /// </summary>
    [Required(ErrorMessage = "Basarili_URL alanı zorunludur")]
    [StringLength(250, ErrorMessage = "Basarili_URL en fazla 250 karakter olabilir")]
    [Url(ErrorMessage = "Basarili_URL geçerli bir URL olmalıdır")]
    public string Basarili_URL { get; set; } = string.Empty;

    /// <summary>
    /// Siparişe özel tekil ID
    /// Bu değeri daha önce gönderdiyseniz sistem yeni Siparis_ID atar
    /// </summary>
    [Required(ErrorMessage = "Siparis_ID alanı zorunludur")]
    public string Siparis_ID { get; set; } = string.Empty;

    /// <summary>
    /// Siparişe ait açıklama (Opsiyonel)
    /// </summary>
    [StringLength(250, ErrorMessage = "Siparis_Aciklama en fazla 250 karakter olabilir")]
    public string? Siparis_Aciklama { get; set; }

    /// <summary>
    /// Seçilen Taksit Sayısı (Tek çekim için 1 gönderiniz)
    /// </summary>
    [Required(ErrorMessage = "Taksit alanı zorunludur")]
    [Range(1, 12, ErrorMessage = "Taksit 1-12 arasında olmalıdır")]
    public int Taksit { get; set; } = 1;

    /// <summary>
    /// İşlem Tutarı (virgüllü kuruş formatında - 1000,50)
    /// </summary>
    [Required(ErrorMessage = "Islem_Tutar alanı zorunludur")]
    public decimal Islem_Tutar { get; set; }

    /// <summary>
    /// Komisyon Dahil Sipariş Tutarı (virgüllü kuruş formatında - 1000,50)
    /// </summary>
    [Required(ErrorMessage = "Toplam_Tutar alanı zorunludur")]
    public decimal Toplam_Tutar { get; set; }

    /// <summary>
    /// Güvenlik tipi: NS (NonSecure) veya 3D
    /// </summary>
    [Required(ErrorMessage = "Islem_Guvenlik_Tip alanı zorunludur")]
    [RegularExpression(@"^(NS|3D)$", ErrorMessage = "Islem_Guvenlik_Tip NS veya 3D olmalıdır")]
    public string Islem_Guvenlik_Tip { get; set; } = "NS";

    /// <summary>
    /// İşleme ait Sipariş ID haricinde tekil ID (Opsiyonel)
    /// </summary>
    public string? Islem_ID { get; set; }

    /// <summary>
    /// IP Adresi
    /// </summary>
    [Required(ErrorMessage = "IPAdr alanı zorunludur")]
    [StringLength(256, ErrorMessage = "IPAdr en fazla 256 karakter olabilir")]
    public string IPAdr { get; set; } = string.Empty;

    /// <summary>
    /// Ödemenin gerçekleştiği sayfanın URL'si (Opsiyonel)
    /// </summary>
    [StringLength(256, ErrorMessage = "Ref_URL en fazla 256 karakter olabilir")]
    [Url(ErrorMessage = "Ref_URL geçerli bir URL olmalıdır")]
    public string? Ref_URL { get; set; }

    /// <summary>
    /// Extra Alan 1 (Opsiyonel)
    /// </summary>
    [StringLength(250, ErrorMessage = "Data1 en fazla 250 karakter olabilir")]
    public string? Data1 { get; set; }

    /// <summary>
    /// Extra Alan 2 (Opsiyonel)
    /// </summary>
    [StringLength(250, ErrorMessage = "Data2 en fazla 250 karakter olabilir")]
    public string? Data2 { get; set; }

    /// <summary>
    /// Extra Alan 3 (Opsiyonel)
    /// </summary>
    [StringLength(250, ErrorMessage = "Data3 en fazla 250 karakter olabilir")]
    public string? Data3 { get; set; }

    /// <summary>
    /// Extra Alan 4 (Opsiyonel)
    /// </summary>
    [StringLength(250, ErrorMessage = "Data4 en fazla 250 karakter olabilir")]
    public string? Data4 { get; set; }

    /// <summary>
    /// Saklanmış Kredi Kartına ait tekil ID değeri (Opsiyonel)
    /// </summary>
    [StringLength(200, ErrorMessage = "KK_Islem_ID en fazla 200 karakter olabilir")]
    public string? KK_Islem_ID { get; set; }

    /// <summary>
    /// İşlem tutarını PARAM API formatına dönüştürür (virgüllü format)
    /// </summary>
    public string GetFormattedIslemTutar()
    {
        return Islem_Tutar.ToString("F2").Replace(".", ",");
    }

    /// <summary>
    /// Toplam tutarı PARAM API formatına dönüştürür (virgüllü format)
    /// </summary>
    public string GetFormattedToplamTutar()
    {
        return Toplam_Tutar.ToString("F2").Replace(".", ",");
    }

    /// <summary>
    /// Request'in geçerli olup olmadığını kontrol eder
    /// </summary>
    public bool IsValid(out List<ValidationResult> validationResults)
    {
        validationResults = new List<ValidationResult>();
        var context = new ValidationContext(this);
        
        var isValid = Validator.TryValidateObject(this, context, validationResults, true);
        
        // Ek validasyonlar
        if (Toplam_Tutar < Islem_Tutar)
        {
            validationResults.Add(new ValidationResult("Toplam_Tutar, Islem_Tutar'dan küçük olamaz"));
            isValid = false;
        }
        
        return isValid;
    }
} 