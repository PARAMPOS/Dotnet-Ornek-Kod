namespace ParamApi.Sdk.Models.Shared;

/// <summary>
/// BIN bilgileri data transfer object
/// BIN_SanalPos metodunun DT_Bilgi response'unda kullanılır
/// Kredi kartına ait kart-banka bilgilerini içerir
/// </summary>
public class DT_BinBilgi
{
    /// <summary>
    /// BIN değeri (6 veya 8 haneli kart başlangıç numarası)
    /// </summary>
    public string BIN { get; set; } = string.Empty;

    /// <summary>
    /// SanalPOS ID değeri
    /// </summary>
    public string SanalPOS_ID { get; set; } = string.Empty;

    /// <summary>
    /// Banka adı
    /// </summary>
    public string Kart_Banka { get; set; } = string.Empty;

    /// <summary>
    /// Debit kredi kartı (0: Kredi, 1: Debit)
    /// </summary>
    public string DKK { get; set; } = string.Empty;

    /// <summary>
    /// Kart tipi (Örn: Kredi Kartı, Debit Kartı)
    /// </summary>
    public string Kart_Tip { get; set; } = string.Empty;

    /// <summary>
    /// Kart organizasyonu (VISA, MASTER, TROY, AMEX)
    /// </summary>
    public string Kart_Org { get; set; } = string.Empty;

    /// <summary>
    /// Banka kodu
    /// </summary>
    public string Banka_Kodu { get; set; } = string.Empty;
} 