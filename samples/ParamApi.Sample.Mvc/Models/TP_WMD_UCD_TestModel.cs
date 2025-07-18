using System.ComponentModel.DataAnnotations;

namespace ParamApi.Sample.Mvc.Models;

/// <summary>
/// TP_WMD_UCD test formu için view model
/// </summary>
public class TP_WMD_UCD_TestModel
{
    [Required(ErrorMessage = "Kart sahibi adı zorunludur")]
    [Display(Name = "Kart Sahibi")]
    public string KK_Sahibi { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kart numarası zorunludur")]
    [Display(Name = "Kart Numarası")]
    [CreditCard(ErrorMessage = "Geçerli bir kart numarası giriniz")]
    public string KK_No { get; set; } = string.Empty;

    [Required(ErrorMessage = "Son kullanma ayı zorunludur")]
    [Display(Name = "Son Kullanma Ayı")]
    [Range(1, 12, ErrorMessage = "Ay 1-12 arasında olmalıdır")]
    public int KK_SK_Ay { get; set; }

    [Required(ErrorMessage = "Son kullanma yılı zorunludur")]
    [Display(Name = "Son Kullanma Yılı")]
    [Range(2024, 2050, ErrorMessage = "Geçerli bir yıl giriniz")]
    public int KK_SK_Yil { get; set; }

    [Required(ErrorMessage = "CVC kodu zorunludur")]
    [Display(Name = "CVC")]
    [StringLength(4, MinimumLength = 3, ErrorMessage = "CVC 3-4 haneli olmalıdır")]
    public string KK_CVC { get; set; } = string.Empty;

    [Display(Name = "GSM Numarası")]
    [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
    public string? KK_Sahibi_GSM { get; set; }

    [Required(ErrorMessage = "Hata URL'si zorunludur")]
    [Display(Name = "Hata URL")]
    [Url(ErrorMessage = "Geçerli bir URL giriniz")]
    public string Hata_URL { get; set; } = "https://localhost:7118/Test/Error";

    [Required(ErrorMessage = "Başarılı URL'si zorunludur")]
    [Display(Name = "Başarılı URL")]
    [Url(ErrorMessage = "Geçerli bir URL giriniz")]
    public string Basarili_URL { get; set; } = "https://localhost:7118/Test/Success";

    [Required(ErrorMessage = "Sipariş ID zorunludur")]
    [Display(Name = "Sipariş ID")]
    public string Siparis_ID { get; set; } = $"TEST_{DateTime.Now:yyyyMMddHHmmss}";

    [Display(Name = "Sipariş Açıklaması")]
    public string? Siparis_Aciklama { get; set; } = "Test İşlemi";

    [Required(ErrorMessage = "Taksit sayısı zorunludur")]
    [Display(Name = "Taksit Sayısı")]
    [Range(1, 12, ErrorMessage = "Taksit 1-12 arasında olmalıdır")]
    public int Taksit { get; set; } = 1;

    [Required(ErrorMessage = "İşlem tutarı zorunludur")]
    [Display(Name = "İşlem Tutarı")]
    [Range(0.01, 999999.99, ErrorMessage = "Tutar 0.01 ile 999999.99 arasında olmalıdır")]
    public decimal Islem_Tutar { get; set; } = 1.00m;

    [Required(ErrorMessage = "Toplam tutar zorunludur")]
    [Display(Name = "Toplam Tutar")]
    [Range(0.01, 999999.99, ErrorMessage = "Tutar 0.01 ile 999999.99 arasında olmalıdır")]
    public decimal Toplam_Tutar { get; set; } = 1.00m;

    [Required(ErrorMessage = "Güvenlik tipi zorunludur")]
    [Display(Name = "Güvenlik Tipi")]
    public string Islem_Guvenlik_Tip { get; set; } = "3D";

    [Display(Name = "İşlem ID")]
    public string? Islem_ID { get; set; }

    [Required(ErrorMessage = "IP adresi zorunludur")]
    [Display(Name = "IP Adresi")]
    public string IPAdr { get; set; } = "127.0.0.1";

    [Display(Name = "Referans URL")]
    public string? Ref_URL { get; set; }

    [Display(Name = "Data1")]
    public string? Data1 { get; set; }

    [Display(Name = "Data2")]
    public string? Data2 { get; set; }

    [Display(Name = "Data3")]
    public string? Data3 { get; set; }

    [Display(Name = "Data4")]
    public string? Data4 { get; set; }

    [Display(Name = "Data5")]
    public string? Data5 { get; set; }
}

/// <summary>
/// Test sonuçları için model
/// </summary>
public class TP_WMD_UCD_ResultModel
{
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public string? Sonuc { get; set; }
    public string? Sonuc_Str { get; set; }
    public string? UCD_URL { get; set; }
    public string? UCD_MD { get; set; }
    public string? Islem_ID { get; set; }
    public string? Dekont_ID { get; set; }
    public string? Siparis_ID { get; set; }
    public DateTime TestTime { get; set; } = DateTime.Now;
} 