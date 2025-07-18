using TurkposService;

namespace ParamApi.Sdk.Models.Responses;

/// <summary>
/// TP_Islem_Odeme_WD response wrapper sınıfı
/// Dövizli ödeme işlemi sonuçlarını sarmalamak için kullanılır
/// </summary>
public class TP_Islem_Odeme_WD_Response
{
    /// <summary>
    /// Orijinal PARAM API response
    /// </summary>
    public ST_TP_Islem_Odeme OriginalResponse { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="originalResponse">PARAM API'den gelen orijinal response</param>
    public TP_Islem_Odeme_WD_Response(ST_TP_Islem_Odeme originalResponse)
    {
        OriginalResponse = originalResponse ?? throw new ArgumentNullException(nameof(originalResponse));
    }

    /// <summary>
    /// İşlem sonucu (1: Başarılı, 0: Başarısız)
    /// </summary>
    public string Sonuc => OriginalResponse.Sonuc ?? string.Empty;

    /// <summary>
    /// İşlem sonuç açıklaması
    /// </summary>
    public string Sonuc_Str => OriginalResponse.Sonuc_Str ?? string.Empty;

    /// <summary>
    /// SID Değeri (İşlem ID)
    /// </summary>
    public string Islem_ID => OriginalResponse.Islem_ID.ToString();

    /// <summary>
    /// 3D Secure URL'si
    /// 3D işlemler için kullanılır
    /// </summary>
    public string UCD_URL => OriginalResponse.UCD_URL ?? string.Empty;

    /// <summary>
    /// Bankanın döndüğü kod
    /// </summary>
    public string Banka_Sonuc_Kod => OriginalResponse.Banka_Sonuc_Kod.ToString();

    /// <summary>
    /// İşlem başarılı mı?
    /// Sonuc = "1" ise başarılı
    /// </summary>
    public bool IsSuccess => Sonuc == "1";

    /// <summary>
    /// İşlem başarısız mı?
    /// Sonuc != "1" ise başarısız
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// NonSecure işlem mi?
    /// UCD_URL boş ise NonSecure işlemdir
    /// </summary>
    public bool IsNonSecure => string.IsNullOrEmpty(UCD_URL) && IsSuccess;

    /// <summary>
    /// 3D Secure işlemi mi?
    /// UCD_URL dolu ise 3D işlemdir
    /// </summary>
    public bool Is3DSecure => !string.IsNullOrEmpty(UCD_URL) && IsSuccess;

    /// <summary>
    /// 3D işlemi başlatıldı mı?
    /// UCD_URL varsa müşteri bu URL'e yönlendirilmelidir
    /// </summary>
    public bool Is3DStarted => Is3DSecure;

    /// <summary>
    /// Banka hatası var mı?
    /// Banka_Sonuc_Kod "0" değilse hata vardır
    /// </summary>
    public bool HasBankError => !string.IsNullOrEmpty(Banka_Sonuc_Kod) && Banka_Sonuc_Kod != "0";

    /// <summary>
    /// İşlem durumu açıklaması
    /// </summary>
    public string StatusDescription
    {
        get
        {
            if (IsNonSecure)
                return "NonSecure ödeme başarıyla tamamlandı";
            if (Is3DSecure)
                return "3D Secure doğrulama için müşteri yönlendirilecek";
            if (IsFailure)
                return $"İşlem başarısız: {Sonuc_Str}";
            return "Bilinmeyen durum";
        }
    }

    /// <summary>
    /// Response'u dictionary formatına çevirir
    /// Log ve debug amaçlı kullanım için
    /// </summary>
    /// <returns>Response alanları dictionary'si</returns>
    public Dictionary<string, object?> ToDictionary()
    {
        return new Dictionary<string, object?>
        {
            ["Sonuc"] = Sonuc,
            ["Sonuc_Str"] = Sonuc_Str,
            ["Islem_ID"] = Islem_ID,
            ["UCD_URL"] = UCD_URL,
            ["Banka_Sonuc_Kod"] = Banka_Sonuc_Kod,
            ["IsSuccess"] = IsSuccess,
            ["IsNonSecure"] = IsNonSecure,
            ["Is3DSecure"] = Is3DSecure,
            ["StatusDescription"] = StatusDescription
        };
    }

    /// <summary>
    /// Response'un string temsilini döndürür
    /// </summary>
    /// <returns>Formatted response string</returns>
    public override string ToString()
    {
        return $"TP_Islem_Odeme_WD Result: {Sonuc} - {Sonuc_Str}" +
               (IsNonSecure ? " (NonSecure Completed)" : "") +
               (Is3DSecure ? $" (3D URL: {UCD_URL})" : "") +
               (HasBankError ? $" (Bank Error: {Banka_Sonuc_Kod})" : "");
    }
} 