using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParamApi.Sdk.Models.Requests;
using ParamApi.Sdk.Models.Responses;
using ParamApi.Sdk.Models.Shared;

namespace ParamApi.Sample.Mvc.Models;

/// <summary>
/// TP_Ozel_Oran metodları test sayfası için view model
/// </summary>
public class TP_Ozel_Oran_TestModel
{
    /// <summary>
    /// Hangi test tipinin seçili olduğu
    /// </summary>
    public OzelOranTestTipi TestTipi { get; set; } = OzelOranTestTipi.Liste;

    /// <summary>
    /// TP_Ozel_Oran_SK_Guncelle için güncelleme bilgileri
    /// </summary>
    public TP_Ozel_Oran_SK_Guncelle_Model? GuncelleModel { get; set; }

    /// <summary>
    /// Test tipi seçenekleri
    /// </summary>
    public static List<SelectListItem> TestTipiOptions => new()
    {
        new SelectListItem { Value = nameof(OzelOranTestTipi.Liste), Text = "TP_Ozel_Oran_Liste - Özel Oran Listesi" },
        new SelectListItem { Value = nameof(OzelOranTestTipi.SK_Liste), Text = "TP_Ozel_Oran_SK_Liste - Son Kullanıcı Listesi" },
        new SelectListItem { Value = nameof(OzelOranTestTipi.SK_Guncelle), Text = "TP_Ozel_Oran_SK_Guncelle - Son Kullanıcı Güncelleme" }
    };
}

/// <summary>
/// TP_Ozel_Oran_SK_Guncelle için model
/// </summary>
public class TP_Ozel_Oran_SK_Guncelle_Model
{
    /// <summary>
    /// Özel Oran SK ID (TP_Ozel_Oran_SK_Liste'den gelen)
    /// </summary>
    [Required(ErrorMessage = "Özel Oran SK ID zorunludur")]
    [Display(Name = "Özel Oran SK ID")]
    public long Ozel_Oran_SK_ID { get; set; }

    /// <summary>
    /// Tek Çekim Oranı
    /// </summary>
    [Required(ErrorMessage = "Tek çekim oranı zorunludur")]
    [Display(Name = "Tek Çekim (MO_1)")]
    public string MO_1 { get; set; } = "1,25";

    /// <summary>
    /// 2 Taksit Oranı
    /// </summary>
    [Required(ErrorMessage = "2 taksit oranı zorunludur")]
    [Display(Name = "2 Taksit (MO_2)")]
    public string MO_2 { get; set; } = "2,50";

    /// <summary>
    /// 3 Taksit Oranı
    /// </summary>
    [Required(ErrorMessage = "3 taksit oranı zorunludur")]
    [Display(Name = "3 Taksit (MO_3)")]
    public string MO_3 { get; set; } = "3,75";

    /// <summary>
    /// 4 Taksit Oranı
    /// </summary>
    [Required(ErrorMessage = "4 taksit oranı zorunludur")]
    [Display(Name = "4 Taksit (MO_4)")]
    public string MO_4 { get; set; } = "5,00";

    /// <summary>
    /// 5 Taksit Oranı
    /// </summary>
    [Required(ErrorMessage = "5 taksit oranı zorunludur")]
    [Display(Name = "5 Taksit (MO_5)")]
    public string MO_5 { get; set; } = "6,25";

    /// <summary>
    /// 6 Taksit Oranı
    /// </summary>
    [Required(ErrorMessage = "6 taksit oranı zorunludur")]
    [Display(Name = "6 Taksit (MO_6)")]
    public string MO_6 { get; set; } = "7,50";

    /// <summary>
    /// 7 Taksit Oranı
    /// </summary>
    [Required(ErrorMessage = "7 taksit oranı zorunludur")]
    [Display(Name = "7 Taksit (MO_7)")]
    public string MO_7 { get; set; } = "8,75";

    /// <summary>
    /// 8 Taksit Oranı
    /// </summary>
    [Required(ErrorMessage = "8 taksit oranı zorunludur")]
    [Display(Name = "8 Taksit (MO_8)")]
    public string MO_8 { get; set; } = "10,00";

    /// <summary>
    /// 9 Taksit Oranı
    /// </summary>
    [Required(ErrorMessage = "9 taksit oranı zorunludur")]
    [Display(Name = "9 Taksit (MO_9)")]
    public string MO_9 { get; set; } = "11,25";

    /// <summary>
    /// 10 Taksit Oranı
    /// </summary>
    [Required(ErrorMessage = "10 taksit oranı zorunludur")]
    [Display(Name = "10 Taksit (MO_10)")]
    public string MO_10 { get; set; } = "12,50";

    /// <summary>
    /// 11 Taksit Oranı
    /// </summary>
    [Required(ErrorMessage = "11 taksit oranı zorunludur")]
    [Display(Name = "11 Taksit (MO_11)")]
    public string MO_11 { get; set; } = "13,75";

    /// <summary>
    /// 12 Taksit Oranı
    /// </summary>
    [Required(ErrorMessage = "12 taksit oranı zorunludur")]
    [Display(Name = "12 Taksit (MO_12)")]
    public string MO_12 { get; set; } = "15,00";

    /// <summary>
    /// SDK Request'ine dönüştür
    /// </summary>
    public TP_Ozel_Oran_SK_Guncelle_Request ToRequest()
    {
        return new TP_Ozel_Oran_SK_Guncelle_Request
        {
            Ozel_Oran_SK_ID = Ozel_Oran_SK_ID,
            MO_1 = MO_1,
            MO_2 = MO_2,
            MO_3 = MO_3,
            MO_4 = MO_4,
            MO_5 = MO_5,
            MO_6 = MO_6,
            MO_7 = MO_7,
            MO_8 = MO_8,
            MO_9 = MO_9,
            MO_10 = MO_10,
            MO_11 = MO_11,
            MO_12 = MO_12
        };
    }
}

/// <summary>
/// Test tipi enum
/// </summary>
public enum OzelOranTestTipi
{
    Liste,
    SK_Liste,
    SK_Guncelle
}

/// <summary>
/// TP_Ozel_Oran sonuç sayfası için view model
/// </summary>
public class TP_Ozel_Oran_ResultModel
{
    /// <summary>
    /// Test tipi
    /// </summary>
    public OzelOranTestTipi TestTipi { get; set; }

    /// <summary>
    /// Gönderilen request bilgileri
    /// </summary>
    public TP_Ozel_Oran_TestModel? Request { get; set; }

    /// <summary>
    /// TP_Ozel_Oran_Liste Response
    /// </summary>
    public TP_Ozel_Oran_Liste_Response? ListeResponse { get; set; }

    /// <summary>
    /// TP_Ozel_Oran_SK_Liste Response
    /// </summary>
    public TP_Ozel_Oran_SK_Liste_Response? SK_ListeResponse { get; set; }

    /// <summary>
    /// TP_Ozel_Oran_SK_Guncelle Response
    /// </summary>
    public TP_Ozel_Oran_SK_Guncelle_Response? SK_GuncelleResponse { get; set; }

    /// <summary>
    /// İşlem başarılı mı?
    /// </summary>
    public bool IsSuccess => TestTipi switch
    {
        OzelOranTestTipi.Liste => ListeResponse?.IsSuccess ?? false,
        OzelOranTestTipi.SK_Liste => SK_ListeResponse?.IsSuccess ?? false,
        OzelOranTestTipi.SK_Guncelle => SK_GuncelleResponse?.IsSuccess ?? false,
        _ => false
    };

    /// <summary>
    /// Hata mesajı (varsa)
    /// </summary>
    public string? ErrorMessage => TestTipi switch
    {
        OzelOranTestTipi.Liste => IsSuccess ? null : ListeResponse?.Sonuc_Str,
        OzelOranTestTipi.SK_Liste => IsSuccess ? null : SK_ListeResponse?.Sonuc_Str,
        OzelOranTestTipi.SK_Guncelle => IsSuccess ? null : SK_GuncelleResponse?.Sonuc_Str,
        _ => "Bilinmeyen test tipi"
    };

    /// <summary>
    /// İşlem zamanı
    /// </summary>
    public DateTime ProcessTime { get; set; } = DateTime.Now;

    /// <summary>
    /// İşlem süresi (ms)
    /// </summary>
    public long ProcessDuration { get; set; }
} 