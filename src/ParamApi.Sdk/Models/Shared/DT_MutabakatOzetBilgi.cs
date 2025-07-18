namespace ParamApi.Sdk.Models.Shared;

/// <summary>
/// Mutabakat özet bilgilerini içeren data transfer objesi
/// TP_Mutabakat_Ozet metodundan dönen DT_Bilgi içerisindeki veriler
/// </summary>
public class DT_MutabakatOzetBilgi
{
    /// <summary>
    /// Başarılı işlem sayısı
    /// </summary>
    public int? Basarili_Islem_Sayi { get; set; }

    /// <summary>
    /// Başarılı işlem toplam tutarı
    /// </summary>
    public string? Basarili_Islem_Toplam_Tutar { get; set; }

    /// <summary>
    /// İptal işlem sayısı
    /// </summary>
    public int? Iptal_Islem_Sayi { get; set; }

    /// <summary>
    /// İptal işlem toplam tutarı
    /// </summary>
    public string? Iptal_Islem_Toplam_Tutar { get; set; }

    /// <summary>
    /// İade işlem sayısı
    /// </summary>
    public int? Iade_Islem_Sayi { get; set; }

    /// <summary>
    /// İade işlem toplam tutarı
    /// </summary>
    public string? Iade_Islem_Toplam_Tutar { get; set; }
} 