namespace ParamApi.Sdk.Models.Responses;

/// <summary>
/// Kredi kartı saklama yanıtı
/// Kart saklama işleminin sonucunu ve KS_GUID değerini içerir
/// </summary>
public class KS_Kart_Ekle_Response
{
    /// <summary>
    /// İşlem sonucu (1: Başarılı, 0: Başarısız)
    /// </summary>
    public int Sonuc { get; set; }

    /// <summary>
    /// İşlem sonuç açıklama
    /// Sonuc > 0 ise İşlem Başarılı, aksi halde başarısız
    /// </summary>
    public string Sonuc_Str { get; set; } = string.Empty;

    /// <summary>
    /// Kart saklama GUID değeri
    /// Saklanan kartın benzersiz kimlik numarası
    /// </summary>
    public string KS_GUID { get; set; } = string.Empty;
} 