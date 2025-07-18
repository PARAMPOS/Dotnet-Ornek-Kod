using System.ComponentModel.DataAnnotations;

namespace ParamApi.Sample.Mvc.Models;

/// <summary>
/// TP_Islem_Odeme_BKM test sayfası için view model
/// BKM Express ödeme sistemi test edilir
/// </summary>
public class TP_Islem_Odeme_BKM_TestModel
{
    /// <summary>
    /// Müşteri Bilgisi (Ad Soyad/Firma Adı)
    /// </summary>
    [Display(Name = "Müşteri Bilgisi")]
    [MaxLength(250, ErrorMessage = "Müşteri bilgisi maksimum 250 karakter olabilir")]
    public string? Customer_Info { get; set; } = "John Doe";

    /// <summary>
    /// Müşteri GSM Numarası
    /// </summary>
    [Required(ErrorMessage = "GSM numarası zorunludur")]
    [Display(Name = "GSM Numarası")]
    [RegularExpression(@"^5\d{9}$", ErrorMessage = "GSM numarası 5 ile başlayan 10 haneli olmalıdır")]
    public string Customer_GSM { get; set; } = "5555555555";

    /// <summary>
    /// Sipariş ID
    /// </summary>
    [Required(ErrorMessage = "Sipariş ID zorunludur")]
    [Display(Name = "Sipariş ID")]
    [MaxLength(50, ErrorMessage = "Sipariş ID maksimum 50 karakter olabilir")]
    public string Order_ID { get; set; } = $"BKM_{DateTime.Now:yyyyMMddHHmmss}";

    /// <summary>
    /// Sipariş Açıklama
    /// </summary>
    [Display(Name = "Sipariş Açıklama")]
    [MaxLength(250, ErrorMessage = "Sipariş açıklama maksimum 250 karakter olabilir")]
    public string? Order_Description { get; set; } = "BKM Express test ödemesi";

    /// <summary>
    /// Ödeme Tutarı
    /// </summary>
    [Required(ErrorMessage = "Ödeme tutarı zorunludur")]
    [Range(0.01, 999999.99, ErrorMessage = "Ödeme tutarı 0.01 - 999999.99 arasında olmalıdır")]
    [Display(Name = "Ödeme Tutarı")]
    public decimal Amount { get; set; } = 10.00m;

    /// <summary>
    /// Başarılı URL
    /// </summary>
    [Required(ErrorMessage = "Başarılı URL zorunludur")]
    [Display(Name = "Başarılı URL")]
    public string Success_URL { get; set; } = "https://localhost:7118/Test/TP_Islem_Odeme_BKM_Success";

    /// <summary>
    /// Hata URL
    /// </summary>
    [Required(ErrorMessage = "Hata URL zorunludur")]
    [Display(Name = "Hata URL")]
    public string Error_URL { get; set; } = "https://localhost:7118/Test/Error";

    /// <summary>
    /// IP Adresi
    /// </summary>
    [Required(ErrorMessage = "IP adresi zorunludur")]
    [Display(Name = "IP Adresi")]
    public string IPAddress { get; set; } = "127.0.0.1";

    /// <summary>
    /// İşlem ID
    /// </summary>
    [Display(Name = "İşlem ID")]
    public string? Transaction_ID { get; set; }

    /// <summary>
    /// Referans URL
    /// </summary>
    [Display(Name = "Referans URL")]
    public string? Referrer_URL { get; set; } = "https://localhost:7118/Test";
}

/// <summary>
/// TP_Islem_Odeme_BKM sonuç sayfası için view model
/// </summary>
public class TP_Islem_Odeme_BKM_ResultModel
{
    /// <summary>
    /// İşlem başarılı mı?
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Hata mesajı
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Response Code (0'dan büyükse başarılı)
    /// </summary>
    public int Response_Code { get; set; }

    /// <summary>
    /// Response Message
    /// </summary>
    public string? Response_Message { get; set; }

    /// <summary>
    /// BKM Express Redirect URL'si
    /// </summary>
    public string? Redirect_URL { get; set; }

    /// <summary>
    /// Sipariş ID
    /// </summary>
    public string? Order_ID { get; set; }

    /// <summary>
    /// Test zamanı
    /// </summary>
    public DateTime TestTime { get; set; } = DateTime.Now;

    /// <summary>
    /// Gönderilen veriler
    /// </summary>
    public TP_Islem_Odeme_BKM_TestModel? RequestData { get; set; }

    /// <summary>
    /// BKM Express işlemi başlatılabilir mi?
    /// </summary>
    public bool CanRedirectToBkm => IsSuccess && !string.IsNullOrEmpty(Redirect_URL);
} 