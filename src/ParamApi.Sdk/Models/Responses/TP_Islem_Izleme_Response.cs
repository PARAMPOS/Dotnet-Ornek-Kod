using ParamApi.Sdk.Models.Shared;
using System.Xml;
using System.Xml.Linq;

namespace ParamApi.Sdk.Models.Responses;

/// <summary>
/// İşlem izleme yanıtı
/// Belirli tarih aralığında yapılan işlem bilgilerini içerir
/// </summary>
public class TP_Islem_Izleme_Response
{
    /// <summary>
    /// İşlem sonucu (1: Başarılı, 0: Başarısız)
    /// </summary>
    public int Sonuc { get; set; }

    /// <summary>
    /// İşlem sonuç açıklama
    /// </summary>
    public string Sonuc_Str { get; set; } = string.Empty;

    /// <summary>
    /// İşlem izleme bilgileri listesi
    /// </summary>
    public List<DT_IslemIzlemeBilgi> DT_Bilgi { get; set; } = new();

    /// <summary>
    /// Parametre alan constructor
    /// </summary>
    public TP_Islem_Izleme_Response()
    {
    }



    /// <summary>
    /// DataSet XML'ini parse ederek DT_Bilgi listesini doldurur
    /// </summary>
    internal void ParseDataSet(XmlElement[] xmlElements)
    {
        DT_Bilgi.Clear();

        try
        {
            foreach (var xmlElement in xmlElements)
            {
                if (xmlElement?.OuterXml == null) continue;

                var doc = XDocument.Parse(xmlElement.OuterXml);
                var tempElements = doc.Descendants("Temp");

                foreach (var temp in tempElements)
                {
                    var bilgi = new DT_IslemIzlemeBilgi
                    {
                        SanalPOS_Islem_ID = ParseLong(temp.Element("SanalPOS_Islem_ID")?.Value),
                        SanalPOS_Islem_ID_Orj = ParseLong(temp.Element("SanalPOS_Islem_ID_Orj")?.Value),
                        Sonuc_Str = temp.Element("Sonuc_Str")?.Value ?? string.Empty,
                        Tip_Str = temp.Element("Tip_Str")?.Value ?? string.Empty,
                        Tarih = temp.Element("Tarih")?.Value ?? string.Empty,
                        Odeme_Yapan_Bilgisi = temp.Element("Odeme_Yapan_Bilgisi")?.Value ?? string.Empty,
                        Odeme_Yapan_AdSoyad = temp.Element("Odeme_Yapan_AdSoyad")?.Value ?? string.Empty,
                        Odeme_Yapan_GSM = temp.Element("Odeme_Yapan_GSM")?.Value ?? string.Empty,
                        Odeme_Yapan_TC = temp.Element("Odeme_Yapan_TC")?.Value ?? string.Empty,
                        Odeme_Aciklama = temp.Element("Odeme_Aciklama")?.Value ?? string.Empty,
                        Taksit = ParseInt(temp.Element("Taksit")?.Value),
                        SanalPOS_Banka = temp.Element("SanalPOS_Banka")?.Value ?? string.Empty,
                        Komisyon_Oran = temp.Element("Komisyon_Oran")?.Value ?? string.Empty,
                        Komisyon_Tutar = temp.Element("Komisyon_Tutar")?.Value ?? string.Empty,
                        Net_Tutar = temp.Element("Net_Tutar")?.Value ?? string.Empty,
                        Tutar = temp.Element("Tutar")?.Value ?? string.Empty,
                        Islem_Guvenlik = temp.Element("Islem_Guvenlik")?.Value ?? string.Empty,
                        Dekont_ID = ParseInt(temp.Element("Dekont_ID")?.Value),
                        ORJ_ORDER_ID = temp.Element("ORJ_ORDER_ID")?.Value ?? string.Empty,
                        Sonuc = ParseInt(temp.Element("Sonuc")?.Value),
                        Kaynak = temp.Element("Kaynak")?.Value ?? string.Empty,
                        Toplam_Iade_Tutar = temp.Element("Toplam_Iade_Tutar")?.Value ?? string.Empty,
                        ORJ_Tarih = temp.Element("ORJ_Tarih")?.Value ?? string.Empty,
                        PB = temp.Element("PB")?.Value ?? string.Empty
                    };

                    DT_Bilgi.Add(bilgi);
                }
            }
        }
        catch (Exception ex)
        {
            // XML parsing hatası durumunda boş liste döner
            System.Diagnostics.Debug.WriteLine($"DataSet parsing error: {ex.Message}");
        }
    }

    /// <summary>
    /// String'i long'a dönüştürür, hata durumunda 0 döner
    /// </summary>
    private static long ParseLong(string? value)
    {
        return long.TryParse(value, out var result) ? result : 0;
    }

    /// <summary>
    /// String'i int'e dönüştürür, hata durumunda 0 döner
    /// </summary>
    private static int ParseInt(string? value)
    {
        return int.TryParse(value, out var result) ? result : 0;
    }
} 