using TurkposService;

namespace ParamApi.Sdk.Models.Responses;

/// <summary>
/// Saklı kart ile ön provizyon işlemi yanıtı
/// Daha önce kaydedilen kart ile ön provizyon işlemi sonucunu içerir
/// </summary>
public class TP_Islem_Odeme_OnProv_WKS_Response
{
    /// <summary>
    /// İşlem sonucu
    /// </summary>
    public int Sonuc { get; set; }

    /// <summary>
    /// İşlem sonuç açıklama
    /// </summary>
    public string Sonuc_Str { get; set; } = string.Empty;

    /// <summary>
    /// SID Değeri (İşlem ID)
    /// </summary>
    public long Islem_ID { get; set; }

    /// <summary>
    /// İşlem GUID Değeri
    /// </summary>
    public string Islem_GUID { get; set; } = string.Empty;

    /// <summary>
    /// Banka 3D HTML içeriği
    /// 3D güvenlik doğrulaması için kullanılan URL
    /// </summary>
    public string UCD_URL { get; set; } = string.Empty;

    /// <summary>
    /// Banka 3D Doğrulama Öncesi MD değeri
    /// </summary>
    public string UCD_MD { get; set; } = string.Empty;

    /// <summary>
    /// Bankanın döndüğü kod (String olarak)
    /// Banka Transaction ID değeri
    /// </summary>
    public string Banka_Sonuc_Kod { get; set; } = string.Empty;

    /// <summary>
    /// Sipariş ID değeri
    /// </summary>
    public string Siparis_ID { get; set; } = string.Empty;

    /// <summary>
    /// Constructor - PARAM API response'undan model oluşturur
    /// </summary>
    public TP_Islem_Odeme_OnProv_WKS_Response() { }

    /// <summary>
    /// Constructor - PARAM API response'undan model oluşturur
    /// Manual field mapping - PARAM API response'undan değerleri alır
    /// </summary>
    public TP_Islem_Odeme_OnProv_WKS_Response(dynamic apiResponse)
    {
        if (apiResponse != null)
        {
            Sonuc = apiResponse.Sonuc ?? 0;
            Sonuc_Str = apiResponse.Sonuc_Str ?? string.Empty;
            Islem_ID = apiResponse.Islem_ID ?? 0L;
            Islem_GUID = apiResponse.Islem_GUID ?? string.Empty;
            UCD_URL = apiResponse.UCD_URL ?? string.Empty;
            UCD_MD = apiResponse.UCD_MD ?? string.Empty;
            Banka_Sonuc_Kod = apiResponse.Banka_Sonuc_Kod?.ToString() ?? string.Empty;
            Siparis_ID = apiResponse.Siparis_ID ?? string.Empty;
        }
    }

    /// <summary>
    /// Banka sonuç kodunu integer olarak döner
    /// Parse edilemezse 0 döner
    /// </summary>
    public int GetBankaSonucKodAsInt()
    {
        return int.TryParse(Banka_Sonuc_Kod, out var result) ? result : 0;
    }

    /// <summary>
    /// İşlemin başarılı olup olmadığını kontrol eder
    /// </summary>
    public bool IsSuccessful => Sonuc > 0;

    /// <summary>
    /// 3D güvenlik doğrulaması gerekli mi kontrol eder
    /// UCD_URL dolu ise 3D doğrulama gereklidir
    /// </summary>
    public bool Requires3DAuthentication => !string.IsNullOrEmpty(UCD_URL);

    /// <summary>
    /// NonSecure işlem mi kontrol eder
    /// UCD_URL boş ise NonSecure işlemdir
    /// </summary>
    public bool IsNonSecureTransaction => string.IsNullOrEmpty(UCD_URL);
} 