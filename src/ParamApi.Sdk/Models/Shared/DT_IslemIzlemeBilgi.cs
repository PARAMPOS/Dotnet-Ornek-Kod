namespace ParamApi.Sdk.Models.Shared;

/// <summary>
/// İşlem izleme bilgilerini içeren data transfer object
/// TP_Islem_Izleme metodunun DT_Bilgi response'unda kullanılır
/// </summary>
public class DT_IslemIzlemeBilgi
{
    /// <summary>
    /// Sanal Pos İşlem ID
    /// </summary>
    public long SanalPOS_Islem_ID { get; set; }

    /// <summary>
    /// İptal ve iade işlemlerinde orjinal işlemin ORDER ID sidir
    /// </summary>
    public long SanalPOS_Islem_ID_Orj { get; set; }

    /// <summary>
    /// İşlem sonuç açıklama
    /// </summary>
    public string Sonuc_Str { get; set; } = string.Empty;

    /// <summary>
    /// Satış/İade/İptal döner
    /// </summary>
    public string Tip_Str { get; set; } = string.Empty;

    /// <summary>
    /// Tarih
    /// </summary>
    public string Tarih { get; set; } = string.Empty;

    /// <summary>
    /// Ödeme yapan bilgisi
    /// </summary>
    public string Odeme_Yapan_Bilgisi { get; set; } = string.Empty;

    /// <summary>
    /// Ödeme yapan adı, soyadı
    /// </summary>
    public string Odeme_Yapan_AdSoyad { get; set; } = string.Empty;

    /// <summary>
    /// Ödeme yapan GSM numarası
    /// </summary>
    public string Odeme_Yapan_GSM { get; set; } = string.Empty;

    /// <summary>
    /// Ödeme yapan TC numarası
    /// </summary>
    public string Odeme_Yapan_TC { get; set; } = string.Empty;

    /// <summary>
    /// Ödemeye ait açıklama
    /// </summary>
    public string Odeme_Aciklama { get; set; } = string.Empty;

    /// <summary>
    /// Taksit Bilgisi
    /// </summary>
    public int Taksit { get; set; }

    /// <summary>
    /// Sanal Pos
    /// </summary>
    public string SanalPOS_Banka { get; set; } = string.Empty;

    /// <summary>
    /// Komisyon Oranı
    /// </summary>
    public string Komisyon_Oran { get; set; } = string.Empty;

    /// <summary>
    /// Komisyon Tutar
    /// </summary>
    public string Komisyon_Tutar { get; set; } = string.Empty;

    /// <summary>
    /// Net Tutar
    /// </summary>
    public string Net_Tutar { get; set; } = string.Empty;

    /// <summary>
    /// Tutar Bilgisi
    /// </summary>
    public string Tutar { get; set; } = string.Empty;

    /// <summary>
    /// NS (NonSecure) veya 3D döner
    /// </summary>
    public string Islem_Guvenlik { get; set; } = string.Empty;

    /// <summary>
    /// İşlemin Dekont ID si
    /// </summary>
    public int Dekont_ID { get; set; }

    /// <summary>
    /// İptal ve iade işlemlerinde orjinal işlemin ORDER ID sidir
    /// </summary>
    public string ORJ_ORDER_ID { get; set; } = string.Empty;

    /// <summary>
    /// Sonuç Değeri
    /// </summary>
    public int Sonuc { get; set; }

    /// <summary>
    /// Kaynak
    /// </summary>
    public string Kaynak { get; set; } = string.Empty;

    /// <summary>
    /// Toplam iade tutarı
    /// </summary>
    public string Toplam_Iade_Tutar { get; set; } = string.Empty;

    /// <summary>
    /// Tarih
    /// </summary>
    public string ORJ_Tarih { get; set; } = string.Empty;

    /// <summary>
    /// Para birimi
    /// </summary>
    public string PB { get; set; } = string.Empty;
} 