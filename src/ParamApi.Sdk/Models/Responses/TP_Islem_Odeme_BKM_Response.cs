using TurkposService;

namespace ParamApi.Sdk.Models.Responses;

/// <summary>
/// BKM Express ödeme işlemi response modeli
/// </summary>
public class TP_Islem_Odeme_BKM_Response
{
    /// <summary>
    /// BKM Express URL'si
    /// </summary>
    public string? Redirect_URL { get; }

    /// <summary>
    /// İşlem sonucu (0'dan büyükse başarılı)
    /// </summary>
    public int Response_Code { get; }

    /// <summary>
    /// İşlem sonuç açıklaması
    /// </summary>
    public string? Response_Message { get; }

    /// <summary>
    /// İşlemin başarılı olup olmadığını döner
    /// </summary>
    public bool IsSuccess => Response_Code > 0;

    /// <summary>
    /// PARAM API response'undan model oluşturur
    /// </summary>
    /// <param name="paramResponse">PARAM API ST_TP_Islem_Odeme_BKM response'u</param>
    public TP_Islem_Odeme_BKM_Response(ST_TP_Islem_Odeme_BKM paramResponse)
    {
        ArgumentNullException.ThrowIfNull(paramResponse);

        Redirect_URL = paramResponse.Redirect_URL;
        Response_Code = paramResponse.Response_Code;
        Response_Message = paramResponse.Response_Message;
    }

    /// <summary>
    /// Response bilgilerini string olarak döner
    /// </summary>
    public override string ToString()
    {
        return $"Response_Code: {Response_Code}, Response_Message: {Response_Message}, Redirect_URL: {Redirect_URL}";
    }
} 