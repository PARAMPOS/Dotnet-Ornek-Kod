using System.ComponentModel.DataAnnotations;

namespace ParamApi.Sample.Mvc.Models;

/// <summary>
/// KS_Tahsilat test sayfası için view model
/// Saklı kart ile ödeme işlemi test edilir
/// </summary>
public class KS_Tahsilat_TestModel
{
    /// <summary>
    /// Saklı kartın GUID değeri
    /// </summary>
    [Required(ErrorMessage = "KS_GUID zorunludur")]
    [Display(Name = "KS_GUID")]
    public string KS_GUID { get; set; } = string.Empty;

    /// <summary>
    /// CVV Kodu (3D işlemler için)
    /// </summary>
    [Display(Name = "CVV Kodu")]
    [StringLength(3, ErrorMessage = "CVV en fazla 3 haneli olabilir")]
    public string? CVV { get; set; } = "123";

    /// <summary>
    /// Cep Telefonu (5xxxxxxxxx formatında)
    /// </summary>
    [Required(ErrorMessage = "Cep telefonu zorunludur")]
    [Display(Name = "Cep Telefonu")]
    [RegularExpression(@"^5\d{9}$", ErrorMessage = "Cep telefonu 5 ile başlamalı ve 10 haneli olmalıdır")]
    public string KK_Sahibi_GSM { get; set; } = "5555555555";

    /// <summary>
    /// Sipariş ID
    /// </summary>
    [Required(ErrorMessage = "Sipariş ID zorunludur")]
    [Display(Name = "Sipariş ID")]
    public string Siparis_ID { get; set; } = $"KS_TAHSILAT_{DateTime.Now:yyyyMMddHHmmss}";

    /// <summary>
    /// Sipariş Açıklama
    /// </summary>
    [Display(Name = "Sipariş Açıklama")]
    public string? Siparis_Aciklama { get; set; } = "Saklı kart ile test ödemesi";

    /// <summary>
    /// Taksit Sayısı
    /// </summary>
    [Required(ErrorMessage = "Taksit sayısı zorunludur")]
    [Display(Name = "Taksit Sayısı")]
    [Range(1, 12, ErrorMessage = "Taksit 1-12 arasında olmalıdır")]
    public int Taksit { get; set; } = 1;

    /// <summary>
    /// İşlem Tutarı
    /// </summary>
    [Required(ErrorMessage = "İşlem tutarı zorunludur")]
    [Display(Name = "İşlem Tutarı")]
    [Range(0.01, 999999.99, ErrorMessage = "İşlem tutarı 0.01 - 999999.99 arasında olmalıdır")]
    public decimal Islem_Tutar { get; set; } = 100.00m;

    /// <summary>
    /// Toplam Tutar (Komisyon Dahil)
    /// </summary>
    [Required(ErrorMessage = "Toplam tutar zorunludur")]
    [Display(Name = "Toplam Tutar")]
    [Range(0.01, 999999.99, ErrorMessage = "Toplam tutar 0.01 - 999999.99 arasında olmalıdır")]
    public decimal Toplam_Tutar { get; set; } = 100.00m;

    /// <summary>
    /// Güvenlik Tipi (NS/3D)
    /// </summary>
    [Required(ErrorMessage = "Güvenlik tipi zorunludur")]
    [Display(Name = "Güvenlik Tipi")]
    public string Islem_Guvenlik_Tip { get; set; } = "3D";

    /// <summary>
    /// İşlem ID (Opsiyonel)
    /// </summary>
    [Display(Name = "İşlem ID")]
    public string? Islem_ID { get; set; } = $"KS_ISLEM_{DateTime.Now:yyyyMMddHHmmss}";

    /// <summary>
    /// Referans URL
    /// </summary>
    [Display(Name = "Referans URL")]
    public string? Ref_URL { get; set; }

    /// <summary>
    /// Extra Alan 1
    /// </summary>
    [Display(Name = "Extra Alan 1")]
    public string? Data1 { get; set; }

    /// <summary>
    /// Extra Alan 2
    /// </summary>
    [Display(Name = "Extra Alan 2")]
    public string? Data2 { get; set; }

    /// <summary>
    /// Extra Alan 3
    /// </summary>
    [Display(Name = "Extra Alan 3")]
    public string? Data3 { get; set; }

    /// <summary>
    /// Extra Alan 4
    /// </summary>
    [Display(Name = "Extra Alan 4")]
    public string? Data4 { get; set; }

    /// <summary>
    /// Kart İşlem ID
    /// </summary>
    [Display(Name = "Kart İşlem ID")]
    public string? KK_Islem_ID { get; set; }
}

/// <summary>
/// KS_Tahsilat sonuç sayfası için view model
/// </summary>
public class KS_Tahsilat_ResultModel
{
    /// <summary>
    /// İşlem sonucu
    /// </summary>
    public int Sonuc { get; set; }

    /// <summary>
    /// İşlem sonuç açıklama
    /// </summary>
    public string Sonuc_Str { get; set; } = string.Empty;

    /// <summary>
    /// URL bilgisi (NONSECURE/3D URL)
    /// </summary>
    public string UCD_URL { get; set; } = string.Empty;

    /// <summary>
    /// İşlem ID
    /// </summary>
    public long Islem_ID { get; set; }

    /// <summary>
    /// Request bilgileri (debug için)
    /// </summary>
    public KS_Tahsilat_TestModel? RequestData { get; set; }

    /// <summary>
    /// İşlem başarılı mı?
    /// </summary>
    public bool IsSuccess => Sonuc > 0;

    /// <summary>
    /// NonSecure işlem mi?
    /// </summary>
    public bool IsNonSecure => UCD_URL?.Equals("NONSECURE", StringComparison.OrdinalIgnoreCase) == true;

    /// <summary>
    /// 3D işlem mi?
    /// </summary>
    public bool Is3DTransaction => !IsNonSecure && !string.IsNullOrEmpty(UCD_URL);

    /// <summary>
    /// NonSecure işlem başarılı mı?
    /// </summary>
    public bool IsNonSecureSuccessful => IsNonSecure && Sonuc > 0 && Islem_ID > 0;

    /// <summary>
    /// Hata mesajı
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// 3D HTML içeriği (decode edilmiş)
    /// </summary>
    public string? DecodedHtml { get; set; }
}

 