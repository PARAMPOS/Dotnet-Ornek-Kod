namespace ParamApi.Sdk.Models.Shared;

/// <summary>
/// Mutabakat detay bilgilerini içeren data transfer objesi
/// TP_Mutabakat_Detay metodundan dönen DT_Bilgi içerisindeki veriler
/// </summary>
public class DT_MutabakatDetayBilgi
{
    /// <summary>
    /// Gün Sonu Tarihi
    /// </summary>
    public string? GUNSONU_TARIHI { get; set; }

    /// <summary>
    /// İşlem Tarihi
    /// </summary>
    public string? ISLEM_TARIHI { get; set; }

    /// <summary>
    /// Valör Tarihi
    /// </summary>
    public string? VALOR_TARIHI { get; set; }

    /// <summary>
    /// Kart Numarası (maskelenmiş)
    /// </summary>
    public string? KART_NO { get; set; }

    /// <summary>
    /// İşlem Tipi (Satış, İade, İptal vb.)
    /// </summary>
    public string? TRANSACTION_TIPI { get; set; }

    /// <summary>
    /// Provizyon Numarası
    /// </summary>
    public string? PROVIZYON_NO { get; set; }

    /// <summary>
    /// Taksit Sırası
    /// </summary>
    public int? TAKSIT_SIRASI { get; set; }

    /// <summary>
    /// Taksit Sayısı
    /// </summary>
    public int? TAKSIT_SAYISI { get; set; }

    /// <summary>
    /// Provizyon Tutarı
    /// </summary>
    public string? PROVIZYON_TUTARI { get; set; }

    /// <summary>
    /// Komisyon Tutarı
    /// </summary>
    public string? KOMISYON_TUTARI { get; set; }

    /// <summary>
    /// Komisyon Oranı
    /// </summary>
    public string? KOMISYON_ORANI { get; set; }

    /// <summary>
    /// Net Tutar
    /// </summary>
    public string? NET_TUTAR { get; set; }

    /// <summary>
    /// Sipariş Numarası
    /// </summary>
    public string? SIPARIS_NO { get; set; }

    /// <summary>
    /// Ana Kart Tipi
    /// </summary>
    public string? ANA_KART_TIPI { get; set; }

    /// <summary>
    /// Alt Kart Tipi
    /// </summary>
    public string? ALT_KART_TIPI { get; set; }
} 