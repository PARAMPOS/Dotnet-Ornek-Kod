using System.ComponentModel.DataAnnotations;

namespace ParamApi.Sample.Mvc.Models;

/// <summary>
/// Ön Provizyon Kapama (TP_Islem_Odeme_OnProv_Kapa) test modeli
/// </summary>
public class TP_Islem_Odeme_OnProv_Kapa_TestModel
{
    /// <summary>
    /// Provizyon ID (Opsiyonel)
    /// Boş geçilebilir
    /// </summary>
    [Display(Name = "Provizyon ID")]
    public string? Prov_ID { get; set; }

    /// <summary>
    /// Kapama Yapılacak Tutar (Zorunlu)
    /// </summary>
    [Required(ErrorMessage = "Provizyon tutarı zorunludur")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Tutar 0'dan büyük olmalıdır")]
    [Display(Name = "Provizyon Tutarı")]
    public decimal Prov_Tutar { get; set; } = 100.00m;

    /// <summary>
    /// Sipariş ID Değeri (Zorunlu)
    /// </summary>
    [Required(ErrorMessage = "Sipariş ID zorunludur")]
    [Display(Name = "Sipariş ID")]
    public string Siparis_ID { get; set; } = $"ONPROV_KAPA_{DateTime.Now:yyyyMMddHHmmss}";
}

/// <summary>
/// Ön Provizyon Kapama sonuç modeli
/// </summary>
public class TP_Islem_Odeme_OnProv_Kapa_ResultModel
{
    public string Sonuc { get; set; } = string.Empty;
    public string Sonuc_Str { get; set; } = string.Empty;
    public string Prov_ID { get; set; } = string.Empty;
    public long Dekont_ID { get; set; }
    public int Banka_Sonuc_Kod { get; set; }
    public string Siparis_ID { get; set; } = string.Empty;
    public string Bank_Trans_ID { get; set; } = string.Empty;
    public string Bank_AuthCode { get; set; } = string.Empty;
    public string Bank_HostMsg { get; set; } = string.Empty;
    public string Bank_Extra { get; set; } = string.Empty;
    public string Bank_HostRefNum { get; set; } = string.Empty;
    public bool IsSuccessful { get; set; }
    public bool IsBankSuccessful { get; set; }
    
    // Test bilgileri
    public TP_Islem_Odeme_OnProv_Kapa_TestModel TestData { get; set; } = new();
} 