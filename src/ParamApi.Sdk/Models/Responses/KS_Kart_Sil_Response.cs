namespace ParamApi.Sdk.Models.Responses;

/// <summary>
/// Saklı kart silme yanıtı
/// Kart silme işleminin sonucunu içerir
/// </summary>
public class KS_Kart_Sil_Response
{
    /// <summary>
    /// İşlem sonucu (1: Başarılı, 0: Başarısız)
    /// </summary>
    public int Sonuc { get; set; }

    /// <summary>
    /// İşlem sonuç açıklama (maksimum 150 karakter)
    /// Sonuc > 0 ise İşlem Başarılı, aksi halde başarısız
    /// </summary>
    public string Sonuc_Str { get; set; } = string.Empty;

    /// <summary>
    /// Default constructor
    /// </summary>
    public KS_Kart_Sil_Response() { }

    /// <summary>
    /// Constructor - PARAM API response'undan model oluşturur
    /// Manual field mapping ile değerleri alır
    /// </summary>
    /// <param name="apiResponse">PARAM API yanıtı</param>
    public KS_Kart_Sil_Response(dynamic apiResponse)
    {
        if (apiResponse != null)
        {
            Sonuc = apiResponse.Sonuc ?? 0;
            Sonuc_Str = apiResponse.Sonuc_Str ?? string.Empty;
        }
    }

    /// <summary>
    /// İşlemin başarılı olup olmadığını kontrol eder
    /// </summary>
    public bool IsSuccessful => Sonuc > 0;

    /// <summary>
    /// İşlemin başarısız olup olmadığını kontrol eder
    /// </summary>
    public bool IsFailed => Sonuc <= 0;
} 