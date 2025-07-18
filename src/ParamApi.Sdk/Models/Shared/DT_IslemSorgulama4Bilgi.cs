using System.Xml.Serialization;

namespace ParamApi.Sdk.Models.Shared;

/// <summary>
/// TP_Islem_Sorgulama4 metodu için işlem sorgulama bilgilerini içeren model
/// </summary>
[XmlType("DT_Islem_Sorgulama4")]
public class DT_IslemSorgulama4Bilgi
{
    /// <summary>
    /// Ödeme sonucu (1: Başarılı - 12: İptal - 13: İade)
    /// </summary>
    [XmlElement("Odeme_Sonuc")]
    public string? Odeme_Sonuc { get; set; }

    /// <summary>
    /// Ödeme sonuç açıklama
    /// </summary>
    [XmlElement("Odeme_Sonuc_Aciklama")]
    public string? Odeme_Sonuc_Aciklama { get; set; }

    /// <summary>
    /// İşlemin gerçekleşme tarihi
    /// </summary>
    [XmlElement("Tarih")]
    public string? Tarih { get; set; }

    /// <summary>
    /// Dekont Bilgisi (Başarısız ise "0" döner, başarılı ise "0"dan büyük döner)
    /// </summary>
    [XmlElement("Dekont_ID")]
    public string? Dekont_ID { get; set; }

    /// <summary>
    /// Sipariş ID
    /// </summary>
    [XmlElement("Siparis_ID")]
    public string? Siparis_ID { get; set; }

    /// <summary>
    /// Metoda gönderilen İşlem ID'si
    /// </summary>
    [XmlElement("Islem_ID")]
    public string? Islem_ID { get; set; }

    /// <summary>
    /// Komisyon oranı
    /// </summary>
    [XmlElement("Komisyon_Oran")]
    public string? Komisyon_Oran { get; set; }

    /// <summary>
    /// Komisyon tutarı
    /// </summary>
    [XmlElement("Komisyon_Tutar")]
    public string? Komisyon_Tutar { get; set; }

    /// <summary>
    /// Komisyon dahil Sipariş tutarı
    /// </summary>
    [XmlElement("Toplam_Tutar")]
    public string? Toplam_Tutar { get; set; }

    /// <summary>
    /// Banka açıklaması
    /// </summary>
    [XmlElement("Banka_Sonuc_Aciklama")]
    public string? Banka_Sonuc_Aciklama { get; set; }

    /// <summary>
    /// Taksit bilgisi
    /// </summary>
    [XmlElement("Taksit")]
    public int? Taksit { get; set; }

    /// <summary>
    /// Ödeme Metodundaki Data1, Data2, Data3, Data4, Data5 alanlarının "|" ile birleştirilmiş hali
    /// </summary>
    [XmlElement("Ext_Data")]
    public string? Ext_Data { get; set; }

    /// <summary>
    /// Ortak ödeme ID Değeri
    /// </summary>
    [XmlElement("Ortak_Odeme_ID")]
    public string? Ortak_Odeme_ID { get; set; }

    /// <summary>
    /// Toplam İade tutarı
    /// </summary>
    [XmlElement("Toplam_Iade_Tutar")]
    public string? Toplam_Iade_Tutar { get; set; }

    /// <summary>
    /// ID Değeri
    /// </summary>
    [XmlElement("ID")]
    public string? ID { get; set; }

    /// <summary>
    /// Kredi Kartı numarası (maskelenmiş)
    /// </summary>
    [XmlElement("KK_No")]
    public string? KK_No { get; set; }

    /// <summary>
    /// İşlem Durumu (SUCCESS - PARTIAL_REFUND - FAIL - BANK_FAIL - CANCEL - REFUND)
    /// </summary>
    [XmlElement("Durum")]
    public string? Durum { get; set; }

    /// <summary>
    /// Ödeme yapan GSM numarası
    /// </summary>
    [XmlElement("Odeme_Yapan_GSM")]
    public string? Odeme_Yapan_GSM { get; set; }

    /// <summary>
    /// İade tarihi
    /// </summary>
    [XmlElement("Iade_Tarih")]
    public string? Iade_Tarih { get; set; }

    /// <summary>
    /// İşlem Türü (SALE - PRE_AUTH - POST_AUTH)
    /// </summary>
    [XmlElement("Islem_Tipi")]
    public string? Islem_Tipi { get; set; }

    /// <summary>
    /// Sanal POS Tipi
    /// </summary>
    [XmlElement("SanalPOS_Tip")]
    public string? SanalPOS_Tip { get; set; }

    /// <summary>
    /// SPS UID
    /// </summary>
    [XmlElement("SPS_UID")]
    public string? SPS_UID { get; set; }

    /// <summary>
    /// Sanal POS ID
    /// </summary>
    [XmlElement("SanalPOS_ID")]
    public string? SanalPOS_ID { get; set; }

    /// <summary>
    /// İşlem GUID Değeri
    /// </summary>
    [XmlElement("Islem_GUID")]
    public string? Islem_GUID { get; set; }
} 