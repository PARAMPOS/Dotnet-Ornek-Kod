using TurkposService;

namespace ParamApi.Sdk.Models.Responses;

/// <summary>
/// TP_Islem_Odeme_OnProv_Kapa response wrapper sınıfı
/// Ön provizyon kapama işlemi sonuçlarını sarmalamak için kullanılır
/// </summary>
public class TP_Islem_Odeme_OnProv_Kapa_Response
{
    /// <summary>
    /// Orijinal PARAM API response
    /// </summary>
    public ST_TP_Islem_Odeme_OnProvKapa OriginalResponse { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="originalResponse">PARAM API'den gelen orijinal response</param>
    public TP_Islem_Odeme_OnProv_Kapa_Response(ST_TP_Islem_Odeme_OnProvKapa originalResponse)
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
    /// Provizyon ID'si
    /// </summary>
    public string Prov_ID => OriginalResponse.Prov_ID ?? string.Empty;

    /// <summary>
    /// İşlemin Dekont ID'si
    /// </summary>
    public long Dekont_ID => OriginalResponse.Dekont_ID;

    /// <summary>
    /// Bankanın döndüğü kod
    /// </summary>
    public int Banka_Sonuc_Kod => OriginalResponse.Banka_Sonuc_Kod;

    /// <summary>
    /// Sipariş ID değeri
    /// </summary>
    public string Siparis_ID => OriginalResponse.Siparis_ID ?? string.Empty;

    /// <summary>
    /// Banka işlem ID'si
    /// </summary>
    public string Bank_Trans_ID => OriginalResponse.Bank_Trans_ID ?? string.Empty;

    /// <summary>
    /// Banka yetki kodu
    /// </summary>
    public string Bank_AuthCode => OriginalResponse.Bank_AuthCode ?? string.Empty;

    /// <summary>
    /// Banka host mesajı
    /// </summary>
    public string Bank_HostMsg => OriginalResponse.Bank_HostMsg ?? string.Empty;

    /// <summary>
    /// Banka ekstra bilgisi
    /// </summary>
    public string Bank_Extra => OriginalResponse.Bank_Extra ?? string.Empty;

    /// <summary>
    /// Banka host referans numarası
    /// </summary>
    public string Bank_HostRefNum => OriginalResponse.Bank_HostRefNum ?? string.Empty;

    /// <summary>
    /// İşlem başarılı mı?
    /// </summary>
    public bool IsSuccessful => Sonuc == "1";

    /// <summary>
    /// İşlem başarısız mı?
    /// </summary>
    public bool IsError => !IsSuccessful;

    /// <summary>
    /// Banka işlem sonucu başarılı mı?
    /// </summary>
    public bool IsBankSuccessful => Banka_Sonuc_Kod == 0;

    /// <summary>
    /// String representation
    /// </summary>
    public override string ToString()
    {
        return $"TP_Islem_Odeme_OnProv_Kapa Response - Sonuç: {Sonuc_Str}, Prov ID: {Prov_ID}, Dekont ID: {Dekont_ID}, Banka Kodu: {Banka_Sonuc_Kod}";
    }
} 