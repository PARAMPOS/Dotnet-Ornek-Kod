using System.ComponentModel.DataAnnotations;
using ParamApi.Sdk.Configuration;

namespace ParamApi.Sample.Mvc.Models;

/// <summary>
/// TP_Islem_Odeme_WD test sayfası için view model
/// Dövizli ödeme işlemi test edilir (Sadece yabancı kartlar)
/// </summary>
public class TP_Islem_Odeme_WD_TestModel
{
    /// <summary>
    /// Döviz Kodu
    /// </summary>
    [Required(ErrorMessage = "Döviz kodu zorunludur")]
    [Display(Name = "Döviz Kodu")]
    public DovizKodu Doviz_Kodu { get; set; } = DovizKodu.USD;

    /// <summary>
    /// Kredi Kartı Sahibi
    /// </summary>
    [Required(ErrorMessage = "Kart sahibi adı zorunludur")]
    [Display(Name = "Kart Sahibi")]
    [MaxLength(100, ErrorMessage = "Kart sahibi maksimum 100 karakter olabilir")]
    public string KK_Sahibi { get; set; } = "JOHN DOE";

    /// <summary>
    /// Kredi Kartı Numarası
    /// </summary>
    [Required(ErrorMessage = "Kart numarası zorunludur")]
    [Display(Name = "Kart Numarası")]
    public string KK_No { get; set; } = "5406675406675403";

    /// <summary>
    /// Son Kullanma Ay
    /// </summary>
    [Required(ErrorMessage = "Son kullanma ayı zorunludur")]
    [Display(Name = "Son Kullanma Ayı")]
    [Range(1, 12, ErrorMessage = "Ay 1-12 arasında olmalıdır")]
    public int KK_SK_Ay { get; set; } = 12;

    /// <summary>
    /// Son Kullanma Yıl
    /// </summary>
    [Required(ErrorMessage = "Son kullanma yılı zorunludur")]
    [Display(Name = "Son Kullanma Yılı")]
    [RegularExpression(@"^\d{4}$", ErrorMessage = "Yıl 4 haneli sayı olmalıdır")]
    public string KK_SK_Yil { get; set; } = "2030";

    /// <summary>
    /// CVC Kodu
    /// </summary>
    [Required(ErrorMessage = "CVC kodu zorunludur")]
    [Display(Name = "CVC")]
    [StringLength(3, MinimumLength = 3, ErrorMessage = "CVC 3 haneli olmalıdır")]
    public string KK_CVC { get; set; } = "000";

    /// <summary>
    /// GSM Numarası
    /// </summary>
    [Required(ErrorMessage = "GSM numarası zorunludur")]
    [Display(Name = "GSM Numarası")]
    [RegularExpression(@"^5\d{9}$", ErrorMessage = "GSM numarası 5 ile başlayan 10 haneli olmalıdır")]
    public string KK_Sahibi_GSM { get; set; } = "5555555555";

    /// <summary>
    /// Sipariş ID
    /// </summary>
    [Required(ErrorMessage = "Sipariş ID zorunludur")]
    [Display(Name = "Sipariş ID")]
    [MaxLength(50, ErrorMessage = "Sipariş ID maksimum 50 karakter olabilir")]
    public string Siparis_ID { get; set; } = $"WD_{DateTime.Now:yyyyMMddHHmmss}";

    /// <summary>
    /// Sipariş Açıklama
    /// </summary>
    [Required(ErrorMessage = "Sipariş açıklama zorunludur")]
    [Display(Name = "Sipariş Açıklama")]
    [MaxLength(250, ErrorMessage = "Sipariş açıklama maksimum 250 karakter olabilir")]
    public string Siparis_Aciklama { get; set; } = "Dövizli ödeme test işlemi";

    /// <summary>
    /// İşlem Tutarı
    /// </summary>
    [Required(ErrorMessage = "İşlem tutarı zorunludur")]
    [Range(0.01, 999999.99, ErrorMessage = "İşlem tutarı 0.01 - 999999.99 arasında olmalıdır")]
    [Display(Name = "İşlem Tutarı")]
    public decimal Islem_Tutar { get; set; } = 10.00m;

    /// <summary>
    /// Toplam Tutar
    /// </summary>
    [Required(ErrorMessage = "Toplam tutar zorunludur")]
    [Range(0.01, 999999.99, ErrorMessage = "Toplam tutar 0.01 - 999999.99 arasında olmalıdır")]
    [Display(Name = "Toplam Tutar")]
    public decimal Toplam_Tutar { get; set; } = 10.00m;

    /// <summary>
    /// Güvenlik Tipi
    /// </summary>
    [Required(ErrorMessage = "Güvenlik tipi zorunludur")]
    [Display(Name = "Güvenlik Tipi")]
    public string Islem_Guvenlik_Tip { get; set; } = "3D";

    /// <summary>
    /// Başarılı URL
    /// </summary>
    [Required(ErrorMessage = "Başarılı URL zorunludur")]
    [Display(Name = "Başarılı URL")]
    public string Basarili_URL { get; set; } = "https://localhost:7118/Test/TP_Islem_Odeme_WD_Success";

    /// <summary>
    /// Hata URL
    /// </summary>
    [Required(ErrorMessage = "Hata URL zorunludur")]
    [Display(Name = "Hata URL")]
    public string Hata_URL { get; set; } = "https://localhost:7118/Test/Error";

    /// <summary>
    /// IP Adresi
    /// </summary>
    [Required(ErrorMessage = "IP adresi zorunludur")]
    [Display(Name = "IP Adresi")]
    public string IPAdr { get; set; } = "127.0.0.1";

    /// <summary>
    /// İşlem ID
    /// </summary>
    [Display(Name = "İşlem ID")]
    public string? Islem_ID { get; set; }

    /// <summary>
    /// Referans URL
    /// </summary>
    [Display(Name = "Referans URL")]
    public string? Ref_URL { get; set; } = "https://localhost:7118/Test";

    /// <summary>
    /// Extra Data 1
    /// </summary>
    [Display(Name = "Data 1")]
    public string? Data1 { get; set; }

    /// <summary>
    /// Extra Data 2
    /// </summary>
    [Display(Name = "Data 2")]
    public string? Data2 { get; set; }

    /// <summary>
    /// Extra Data 3
    /// </summary>
    [Display(Name = "Data 3")]
    public string? Data3 { get; set; }

    /// <summary>
    /// Extra Data 4
    /// </summary>
    [Display(Name = "Data 4")]
    public string? Data4 { get; set; }

    /// <summary>
    /// Extra Data 5
    /// </summary>
    [Display(Name = "Data 5")]
    public string? Data5 { get; set; }
}

/// <summary>
/// TP_Islem_Odeme_WD sonuç sayfası için view model
/// </summary>
public class TP_Islem_Odeme_WD_ResultModel
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
    /// İşlem sonucu
    /// </summary>
    public string? Sonuc { get; set; }

    /// <summary>
    /// İşlem sonuç açıklaması
    /// </summary>
    public string? Sonuc_Str { get; set; }

    /// <summary>
    /// 3D Secure URL'si
    /// </summary>
    public string? UCD_URL { get; set; }

    /// <summary>
    /// İşlem ID
    /// </summary>
    public string? Islem_ID { get; set; }

    /// <summary>
    /// Banka sonuç kodu
    /// </summary>
    public string? Banka_Sonuc_Kod { get; set; }

    /// <summary>
    /// Sipariş ID
    /// </summary>
    public string? Siparis_ID { get; set; }

    /// <summary>
    /// Test zamanı
    /// </summary>
    public DateTime TestTime { get; set; } = DateTime.Now;

    /// <summary>
    /// Gönderilen veriler
    /// </summary>
    public TP_Islem_Odeme_WD_TestModel? RequestData { get; set; }

    /// <summary>
    /// Decode edilmiş HTML (3D için)
    /// </summary>
    public string? DecodedHtml { get; set; }

    /// <summary>
    /// NonSecure işlem mi?
    /// </summary>
    public bool IsNonSecure => IsSuccess && string.IsNullOrEmpty(UCD_URL);

    /// <summary>
    /// 3D Secure işlemi mi?
    /// </summary>
    public bool Is3DTransaction => IsSuccess && !string.IsNullOrEmpty(UCD_URL);
} 