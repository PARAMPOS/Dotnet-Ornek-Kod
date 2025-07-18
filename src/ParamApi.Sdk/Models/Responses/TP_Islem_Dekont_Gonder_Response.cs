namespace ParamApi.Sdk.Models.Responses;

/// <summary>
/// Dekont e-posta gönderimi yanıtı
/// İşlem sonucu bilgilerini içerir
/// </summary>
public class TP_Islem_Dekont_Gonder_Response
{
    /// <summary>
    /// İşlem sonucu (1: Başarılı, 0: Başarısız)
    /// </summary>
    public int Sonuc { get; set; }

    /// <summary>
    /// İşlem sonuç açıklama
    /// </summary>
    public string Sonuc_Str { get; set; } = string.Empty;
} 