namespace ParamApi.Sdk.Models.Shared;

/// <summary>
/// Saklı kart listesi bilgileri data transfer object
/// KS_Kart_Liste metodunun DT_Bilgi response'unda kullanılır
/// Saklanan kredi kartı bilgilerini içerir
/// </summary>
public class DT_KartListeBilgi
{
    /// <summary>
    /// Kart Saklama ID'si
    /// </summary>
    public string ID { get; set; } = string.Empty;

    /// <summary>
    /// Kart Saklama GUID değeri (36 karakter)
    /// </summary>
    public string KK_GUID { get; set; } = string.Empty;

    /// <summary>
    /// Saklama Tarihi (10 karakter)
    /// </summary>
    public string Tarih { get; set; } = string.Empty;

    /// <summary>
    /// Kredi Kartı BIN Kodu (16 karakter)
    /// </summary>
    public string KK_No { get; set; } = string.Empty;

    /// <summary>
    /// Kart Tipi (VISA, MASTER, vb.)
    /// </summary>
    public string KK_Tip { get; set; } = string.Empty;

    /// <summary>
    /// Kart Bankası
    /// </summary>
    public string KK_Banka { get; set; } = string.Empty;

    /// <summary>
    /// Kart Markası
    /// </summary>
    public string KK_Marka { get; set; } = string.Empty;

    /// <summary>
    /// Kart Adı
    /// </summary>
    public string Kart_Adi { get; set; } = string.Empty;

    /// <summary>
    /// Kredi kartı Hash Değeri
    /// </summary>
    public string KK_Hash { get; set; } = string.Empty;

    /// <summary>
    /// Kart Türü (Kredi Kartı, Debit Kart vb.)
    /// </summary>
    public string KK_KD { get; set; } = string.Empty;

    /// <summary>
    /// Kredi Kartı Son Kullanma Tarihi (6 karakter)
    /// </summary>
    public string KK_SK { get; set; } = string.Empty;
} 