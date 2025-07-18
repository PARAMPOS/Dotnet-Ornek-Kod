using TurkposService;

namespace ParamApi.Sdk.Models.Responses;

/// <summary>
/// Özel oran son kullanıcı güncelleme response modeli
/// </summary>
public class TP_Ozel_Oran_SK_Guncelle_Response
{
    /// <summary>
    /// İşlem sonucu (1'den büyükse başarılı)
    /// </summary>
    public string? Sonuc { get; }

    /// <summary>
    /// İşlem sonuç açıklaması
    /// </summary>
    public string? Sonuc_Str { get; }

    /// <summary>
    /// İşlemin başarılı olup olmadığını döner
    /// </summary>
    public bool IsSuccess
    {
        get
        {
            if (int.TryParse(Sonuc, out int sonucInt))
            {
                return sonucInt >= 1;
            }
            return false;
        }
    }

    /// <summary>
    /// PARAM API response'undan wrapper oluşturur
    /// </summary>
    /// <param name="response">PARAM API ST_Sonuc response</param>
    public TP_Ozel_Oran_SK_Guncelle_Response(ST_Sonuc response)
    {
        ArgumentNullException.ThrowIfNull(response);

        Sonuc = response.Sonuc?.ToString();
        Sonuc_Str = response.Sonuc_Str;
    }

    /// <summary>
    /// Response'un string temsilini döner
    /// </summary>
    /// <returns>Sonuc ve Sonuc_Str bilgileri</returns>
    public override string ToString()
    {
        return $"Sonuc: {Sonuc}, Sonuc_Str: {Sonuc_Str}";
    }

    /// <summary>
    /// Hatalı güncelleme response'u oluşturur (test amaçlı)
    /// </summary>
    /// <param name="errorMessage">Hata mesajı</param>
    /// <param name="errorCode">Hata kodu</param>
    /// <returns>Hatalı response</returns>
    public static TP_Ozel_Oran_SK_Guncelle_Response CreateError(string errorMessage, int errorCode = 0)
    {
        var mockResponse = new ST_Sonuc
        {
            Sonuc = errorCode.ToString(),
            Sonuc_Str = errorMessage
        };
        return new TP_Ozel_Oran_SK_Guncelle_Response(mockResponse);
    }
} 