using System.ComponentModel.DataAnnotations;

namespace ParamApi.Sample.Mvc.Models;

/// <summary>
/// TP_Islem_Odeme_OnProv_WMD test sayfası için model
/// </summary>
public class TP_Islem_Odeme_OnProv_WMD_TestModel
{
    // Kart Bilgileri
    [Required(ErrorMessage = "Kart sahibi zorunludur")]
    [Display(Name = "Kart Sahibi")]
    public string KK_Sahibi { get; set; } = "Test Kullanici";

    [Required(ErrorMessage = "Kart numarası zorunludur")]
    [Display(Name = "Kart Numarası")]
    public string KK_No { get; set; } = "4506347011634997";

    [Required(ErrorMessage = "Son kullanma ayı zorunludur")]
    [Range(1, 12, ErrorMessage = "Geçerli bir ay giriniz (1-12)")]
    [Display(Name = "Son Kullanma Ayı")]
    public int KK_SK_Ay { get; set; } = 12;

    [Required(ErrorMessage = "Son kullanma yılı zorunludur")]
    [Range(2024, 2040, ErrorMessage = "Geçerli bir yıl giriniz")]
    [Display(Name = "Son Kullanma Yılı")]
    public int KK_SK_Yil { get; set; } = 2026;

    [Required(ErrorMessage = "CVC kodu zorunludur")]
    [StringLength(3, MinimumLength = 3, ErrorMessage = "CVC 3 haneli olmalıdır")]
    [Display(Name = "CVC")]
    public string KK_CVC { get; set; } = "000";

    [Required(ErrorMessage = "GSM numarası zorunludur")]
    [Display(Name = "GSM Numarası")]
    public string KK_Sahibi_GSM { get; set; } = "5555555555";

    // İşlem Bilgileri
    [Required(ErrorMessage = "Sipariş ID zorunludur")]
    [Display(Name = "Sipariş ID")]
    public string Siparis_ID { get; set; } = $"ONPROV_{DateTime.Now:yyyyMMddHHmmss}";

    [Display(Name = "Sipariş Açıklama")]
    public string? Siparis_Aciklama { get; set; } = "Test ön provizyon işlemi";

    [Required(ErrorMessage = "İşlem tutarı zorunludur")]
    [Range(0.01, 999999.99, ErrorMessage = "İşlem tutarı 0.01 - 999999.99 arasında olmalıdır")]
    [Display(Name = "İşlem Tutarı")]
    public decimal Islem_Tutar { get; set; } = 1.00m;

    [Required(ErrorMessage = "Toplam tutar zorunludur")]
    [Range(0.01, 999999.99, ErrorMessage = "Toplam tutar 0.01 - 999999.99 arasında olmalıdır")]
    [Display(Name = "Toplam Tutar")]
    public decimal Toplam_Tutar { get; set; } = 1.00m;

    [Required(ErrorMessage = "Taksit sayısı zorunludur")]
    [Range(1, 12, ErrorMessage = "Taksit sayısı 1-12 arasında olmalıdır")]
    [Display(Name = "Taksit")]
    public int Taksit { get; set; } = 1;

    [Required(ErrorMessage = "Güvenlik tipi zorunludur")]
    [Display(Name = "Güvenlik Tipi")]
    public string Islem_Guvenlik_Tip { get; set; } = "3D";

    // URL Bilgileri
    [Required(ErrorMessage = "Başarılı URL zorunludur")]
    [Display(Name = "Başarılı URL")]
    public string Basarili_URL { get; set; } = "https://localhost:7118/Test/Success";

    [Required(ErrorMessage = "Hata URL zorunludur")]
    [Display(Name = "Hata URL")]
    public string Hata_URL { get; set; } = "https://localhost:7118/Test/Error";

    // Diğer Bilgiler
    [Display(Name = "İşlem ID")]
    public string? Islem_ID { get; set; }

    [Required(ErrorMessage = "IP adresi zorunludur")]
    [Display(Name = "IP Adresi")]
    public string IPAdr { get; set; } = "127.0.0.1";

    [Display(Name = "Referans URL")]
    public string? Ref_URL { get; set; } = "https://localhost:7118/Test";

    // Extra Data Alanları
    [Display(Name = "Data 1")]
    public string? Data1 { get; set; }

    [Display(Name = "Data 2")]
    public string? Data2 { get; set; }

    [Display(Name = "Data 3")]
    public string? Data3 { get; set; }

    [Display(Name = "Data 4")]
    public string? Data4 { get; set; }

    [Display(Name = "Data 5")]
    public string? Data5 { get; set; }
}

/// <summary>
/// Test sonuçları için model
/// </summary>
public class TP_Islem_Odeme_OnProv_WMD_ResultModel
{
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public string? Sonuc { get; set; }
    public string? Sonuc_Str { get; set; }
    public string? UCD_HTML { get; set; }
    public string? UCD_MD { get; set; }
    public string? Islem_ID { get; set; }
    public string? Islem_GUID { get; set; }
    public string? Siparis_ID { get; set; }
    public string? Bank_Trans_ID { get; set; }
    public string? Bank_AuthCode { get; set; }
    public string? Bank_HostMsg { get; set; }
    public int Banka_Sonuc_Kod { get; set; }
    public string? Bank_Extra { get; set; }
    public string? Bank_HostRefNum { get; set; }
    public DateTime TestTime { get; set; } = DateTime.Now;
} 