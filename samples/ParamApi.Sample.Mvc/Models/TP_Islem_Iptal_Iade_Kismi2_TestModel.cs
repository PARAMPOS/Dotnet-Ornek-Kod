using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParamApi.Sdk.Configuration;

namespace ParamApi.Sample.Mvc.Models;

/// <summary>
/// TP_Islem_Iptal_Iade_Kismi2 test sayfası için view model
/// </summary>
public class TP_Islem_Iptal_Iade_Kismi2_TestModel
{
    /// <summary>
    /// İşlem durumu (İptal veya İade)
    /// </summary>
    [Required(ErrorMessage = "İşlem durumu seçiniz")]
    [Display(Name = "İşlem Durumu")]
    public IptalIadeDurum Durum { get; set; } = IptalIadeDurum.IPTAL;

    /// <summary>
    /// İptal/İade edilecek işlemin Sipariş ID'si
    /// </summary>
    [Required(ErrorMessage = "Sipariş ID zorunludur")]
    [Display(Name = "Sipariş ID")]
    [StringLength(50, ErrorMessage = "Sipariş ID maksimum 50 karakter olmalıdır")]
    public string Siparis_ID { get; set; } = "ORDER-" + DateTime.Now.Ticks.ToString().Substring(10);

    /// <summary>
    /// İptal/İade tutarı
    /// İPTAL için tam tutar, İADE için tam veya kısmi tutar
    /// </summary>
    [Required(ErrorMessage = "Tutar zorunludur")]
    [Display(Name = "İptal/İade Tutarı")]
    [Range(0.01, 999999.99, ErrorMessage = "Tutar 0.01 ile 999,999.99 arasında olmalıdır")]
    [DataType(DataType.Currency)]
    public decimal Tutar { get; set; } = 1.00m;

    /// <summary>
    /// İşlem açıklaması (bilgilendirme amaçlı)
    /// </summary>
    [Display(Name = "İşlem Açıklaması")]
    public string? Aciklama { get; set; }

    /// <summary>
    /// Durum dropdown için seçenekler
    /// </summary>
    public static List<SelectListItem> DurumOptions => new()
    {
        new SelectListItem { Value = nameof(IptalIadeDurum.IPTAL), Text = "İPTAL - Aynı gün yapılan ödemeyi iptal et" },
        new SelectListItem { Value = nameof(IptalIadeDurum.IADE), Text = "İADE - Geçmiş ödemeleri iade et" }
    };

    /// <summary>
    /// Test için örnek sipariş ID'leri
    /// </summary>
    public static List<string> SampleOrderIds => new()
    {
        "ORDER-123456789",
        "TEST-ORDER-001",
        "PARAM-TEST-20241201",
        "SAMPLE-ORDER-999",
        "ORDER-" + DateTime.Now.ToString("yyyyMMddHHmm")
    };
}

/// <summary>
/// TP_Islem_Iptal_Iade_Kismi2 sonuç sayfası için view model
/// </summary>
public class TP_Islem_Iptal_Iade_Kismi2_ResultModel
{
    /// <summary>
    /// Gönderilen request bilgileri
    /// </summary>
    public TP_Islem_Iptal_Iade_Kismi2_TestModel? Request { get; set; }

    /// <summary>
    /// API Response
    /// </summary>
    public ParamApi.Sdk.Models.Responses.TP_Islem_Iptal_Iade_Kismi2_Response? Response { get; set; }

    /// <summary>
    /// İşlem başarılı mı?
    /// </summary>
    public bool IsSuccess => Response?.IsSuccess ?? false;

    /// <summary>
    /// Hata mesajı (varsa)
    /// </summary>
    public string? ErrorMessage => Response?.ErrorMessage;

    /// <summary>
    /// İşlem zamanı
    /// </summary>
    public DateTime ProcessTime { get; set; } = DateTime.Now;

    /// <summary>
    /// İşlem süresi (ms)
    /// </summary>
    public long ProcessDuration { get; set; }
} 