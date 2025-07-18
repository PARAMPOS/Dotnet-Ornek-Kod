using ParamApi.Sdk.Models.Shared;
using System.Xml;
using System.Xml.Linq;

namespace ParamApi.Sdk.Models.Responses;

/// <summary>
/// BIN sorgulama yanıtı
/// Kredi kartına ait kart-banka bilgilerini ve SanalPOS_ID değerlerini içerir
/// </summary>
public class BIN_SanalPos_Response
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
    /// BIN bilgileri listesi
    /// </summary>
    public List<DT_BinBilgi> DT_Bilgi { get; set; } = new();

    /// <summary>
    /// DataSet XML'ini parse ederek DT_BinBilgi listesini doldurur
    /// </summary>
    public void ParseDataSet(XmlNode[] xmlNodes)
    {
        if (xmlNodes == null || xmlNodes.Length == 0) return;

        try
        {
            foreach (var xmlNode in xmlNodes)
            {
                if (xmlNode?.InnerXml == null) continue;

                var doc = XDocument.Parse(xmlNode.InnerXml);
                var tempElements = doc.Descendants("Temp");

                foreach (var temp in tempElements)
                {
                    var binBilgi = new DT_BinBilgi
                    {
                        BIN = temp.Element("BIN")?.Value ?? string.Empty,
                        SanalPOS_ID = temp.Element("SanalPOS_ID")?.Value ?? string.Empty,
                        Kart_Banka = temp.Element("Kart_Banka")?.Value ?? string.Empty,
                        DKK = temp.Element("DKK")?.Value ?? string.Empty,
                        Kart_Tip = temp.Element("Kart_Tip")?.Value ?? string.Empty,
                        Kart_Org = temp.Element("Kart_Org")?.Value ?? string.Empty,
                        Banka_Kodu = temp.Element("Banka_Kodu")?.Value ?? string.Empty
                    };

                    DT_Bilgi.Add(binBilgi);
                }
            }
        }
        catch (Exception ex)
        {
            // XML parsing hatası durumunda boş liste döndür
            Console.WriteLine($"BIN_SanalPos DataSet parsing error: {ex.Message}");
        }
    }
} 