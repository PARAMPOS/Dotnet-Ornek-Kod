using System.ComponentModel.DataAnnotations;

namespace ParamApi.Sample.Mvc.Models;

/// <summary>
/// Ön Provizyon İptal (TP_Islem_Iptal_OnProv) test modeli
/// </summary>
public class TP_Islem_Iptal_OnProv_TestModel
{
    /// <summary>
    /// Provizyon ID (Opsiyonel)
    /// Boş geçilebilir
    /// </summary>
    [Display(Name = "Provizyon ID")]
    public string? Prov_ID { get; set; }

    /// <summary>
    /// Sipariş ID Değeri (Zorunlu)
    /// </summary>
    [Required(ErrorMessage = "Sipariş ID zorunludur")]
    [Display(Name = "Sipariş ID")]
    public string Siparis_ID { get; set; } = $"ONPROV_IPTAL_{DateTime.Now:yyyyMMddHHmmss}";
}

/// <summary>
/// Ön Provizyon İptal sonuç modeli
/// </summary>
public class TP_Islem_Iptal_OnProv_ResultModel
{
    public string Sonuc { get; set; } = string.Empty;
    public string Sonuc_Str { get; set; } = string.Empty;
    public string Banka_Sonuc_Kod { get; set; } = string.Empty;
    public bool IsSuccessful { get; set; }
    public bool IsBankSuccessful { get; set; }
    public bool IsFullySuccessful { get; set; }
    public int SonucAsInt { get; set; }
    public int BankaSonucKodAsInt { get; set; }
    
    // Test bilgileri
    public TP_Islem_Iptal_OnProv_TestModel TestData { get; set; } = new();
} 