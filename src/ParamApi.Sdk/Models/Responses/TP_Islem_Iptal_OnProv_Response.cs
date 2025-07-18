using TurkposService;

namespace ParamApi.Sdk.Models.Responses;

/// <summary>
/// TP_Islem_Iptal_OnProv response wrapper sınıfı
/// Ön provizyon iptal işlemi sonuçlarını sarmalamak için kullanılır
/// </summary>
public class TP_Islem_Iptal_OnProv_Response
{
    /// <summary>
    /// Orijinal PARAM API response
    /// </summary>
    public ST_Sonuc OriginalResponse { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="originalResponse">PARAM API'den gelen orijinal response</param>
    public TP_Islem_Iptal_OnProv_Response(ST_Sonuc originalResponse)
    {
        OriginalResponse = originalResponse ?? throw new ArgumentNullException(nameof(originalResponse));
    }

    /// <summary>
    /// İşlem sonucu (1: Başarılı, 0: Başarısız)
    /// </summary>
    public string Sonuc => OriginalResponse.Sonuc ?? string.Empty;

    /// <summary>
    /// İşlem sonuç açıklama
    /// </summary>
    public string Sonuc_Str => OriginalResponse.Sonuc_Str ?? string.Empty;

    /// <summary>
    /// Bankanın döndüğü kod
    /// </summary>
    public string Banka_Sonuc_Kod => OriginalResponse.Banka_Sonuc_Kod ?? string.Empty;

    /// <summary>
    /// İşlem başarılı mı?
    /// </summary>
    public bool IsSuccessful => Sonuc == "1";

    /// <summary>
    /// İşlem başarısız mı?
    /// </summary>
    public bool IsError => !IsSuccessful;

    /// <summary>
    /// Sonuc değerini integer olarak döndürür
    /// </summary>
    public int SonucAsInt => int.TryParse(Sonuc, out var result) ? result : 0;

    /// <summary>
    /// Banka sonuç kodunu integer olarak döndürür
    /// </summary>
    public int BankaSonucKodAsInt => int.TryParse(Banka_Sonuc_Kod, out var result) ? result : -1;

    /// <summary>
    /// Banka işlem sonucu başarılı mı?
    /// Banka_Sonuc_Kod = "00" olmalı
    /// </summary>
    public bool IsBankSuccessful => Banka_Sonuc_Kod == "00";

    /// <summary>
    /// Genel başarı kontrolü (hem işlem hem banka başarılı)
    /// </summary>
    public bool IsFullySuccessful => IsSuccessful && IsBankSuccessful;

    /// <summary>
    /// String representation
    /// </summary>
    public override string ToString()
    {
        return $"TP_Islem_Iptal_OnProv Response - Sonuç: {Sonuc_Str}, Banka Kodu: {Banka_Sonuc_Kod}, Başarılı: {IsFullySuccessful}";
    }
} 