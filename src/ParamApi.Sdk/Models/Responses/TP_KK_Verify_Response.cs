namespace ParamApi.Sdk.Models.Responses;

/// <summary>
/// Kredi kartı doğrulama yanıtı
/// Kart doğrulama işleminin sonucunu ve 3D Secure bilgilerini içerir
/// </summary>
public class TP_KK_Verify_Response
{
    /// <summary>
    /// SID Değeri (İşlem ID)
    /// </summary>
    public long Islem_ID { get; set; }

    /// <summary>
    /// 3D URL si
    /// 3D Secure doğrulama için yönlendirme URL'i
    /// </summary>
    public string UCD_URL { get; set; } = string.Empty;

    /// <summary>
    /// İşlem sonucu
    /// 0'dan büyükse 3D Secure doğrulama gerekli
    /// </summary>
    public int Sonuc { get; set; }

    /// <summary>
    /// İşlem sonuç açıklama
    /// </summary>
    public string Sonuc_Str { get; set; } = string.Empty;

    /// <summary>
    /// Bankanın döndüğü kod
    /// </summary>
    public int Banka_Sonuc_Kod { get; set; }

    /// <summary>
    /// Default constructor
    /// </summary>
    public TP_KK_Verify_Response() { }

    /// <summary>
    /// Constructor - PARAM API response'undan model oluşturur
    /// Manual field mapping ile değerleri alır
    /// </summary>
    /// <param name="apiResponse">PARAM API yanıtı</param>
    public TP_KK_Verify_Response(dynamic apiResponse)
    {
        if (apiResponse != null)
        {
            Islem_ID = apiResponse.Islem_ID ?? 0;
            UCD_URL = apiResponse.UCD_URL ?? string.Empty;
            Sonuc = apiResponse.Sonuc ?? 0;
            Sonuc_Str = apiResponse.Sonuc_Str ?? string.Empty;
            Banka_Sonuc_Kod = apiResponse.Banka_Sonuc_Kod ?? 0;
        }
    }

    /// <summary>
    /// İşlemin başarılı olup olmadığını kontrol eder
    /// Sonuc > 0 ise başarılı kabul edilir
    /// </summary>
    public bool IsSuccessful => Sonuc > 0;

    /// <summary>
    /// İşlemin başarısız olup olmadığını kontrol eder
    /// </summary>
    public bool IsFailed => Sonuc <= 0;

    /// <summary>
    /// 3D Secure doğrulama gerekip gerekmediğini kontrol eder
    /// Sonuc > 0 ve UCD_URL dolu ise 3D Secure gerekli
    /// </summary>
    public bool Is3DSecureRequired => IsSuccessful && !string.IsNullOrEmpty(UCD_URL);

    /// <summary>
    /// 3D Secure yönlendirme URL'i var mı kontrolü
    /// </summary>
    public bool Has3DUrl => !string.IsNullOrEmpty(UCD_URL);

    /// <summary>
    /// İşlem sonucunun detaylı açıklaması
    /// </summary>
    public string GetDetailedResult()
    {
        if (IsFailed)
            return $"İşlem başarısız: {Sonuc_Str}";
        
        if (Is3DSecureRequired)
            return $"3D Secure doğrulama gerekli: {Sonuc_Str}";
        
        return $"İşlem başarılı: {Sonuc_Str}";
    }
} 