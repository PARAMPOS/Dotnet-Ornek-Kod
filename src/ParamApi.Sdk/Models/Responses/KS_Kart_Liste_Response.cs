using ParamApi.Sdk.Models.Shared;
using System.Xml;
using System.Xml.Linq;

namespace ParamApi.Sdk.Models.Responses;

/// <summary>
/// Saklı kart listesi yanıtı
/// Saklanan kredi kartlarının listesini içerir
/// </summary>
public class KS_Kart_Liste_Response
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
    /// Saklı kart listesi bilgileri
    /// </summary>
    public List<DT_KartListeBilgi> DT_Bilgi { get; set; } = new();

    /// <summary>
    /// DataSet XML'ini parse ederek DT_KartListeBilgi listesini doldurur
    /// </summary>
    public void ParseDataSet(XmlNode[] xmlNodes)
    {
        try
        {
            DT_Bilgi.Clear();

            foreach (var xmlNode in xmlNodes)
            {
                if (xmlNode.OuterXml.Contains("<NewDataSet"))
                {
                    var doc = XDocument.Parse(xmlNode.OuterXml);
                    var temps = doc.Descendants().Where(x => x.Name.LocalName == "Temp");

                    foreach (var temp in temps)
                    {
                        var kartBilgi = new DT_KartListeBilgi();

                        foreach (var element in temp.Elements())
                        {
                            var elementName = element.Name.LocalName;
                            var elementValue = element.Value ?? string.Empty;

                            switch (elementName)
                            {
                                case "ID":
                                    kartBilgi.ID = elementValue;
                                    break;
                                case "KK_GUID":
                                    kartBilgi.KK_GUID = elementValue;
                                    break;
                                case "Tarih":
                                    kartBilgi.Tarih = elementValue;
                                    break;
                                case "KK_No":
                                    kartBilgi.KK_No = elementValue;
                                    break;
                                case "KK_Tip":
                                    kartBilgi.KK_Tip = elementValue;
                                    break;
                                case "KK_Banka":
                                    kartBilgi.KK_Banka = elementValue;
                                    break;
                                case "KK_Marka":
                                    kartBilgi.KK_Marka = elementValue;
                                    break;
                                case "Kart_Adi":
                                    kartBilgi.Kart_Adi = elementValue;
                                    break;
                                case "KK_Hash":
                                    kartBilgi.KK_Hash = elementValue;
                                    break;
                                case "KK_KD":
                                    kartBilgi.KK_KD = elementValue;
                                    break;
                                case "KK_SK":
                                    kartBilgi.KK_SK = elementValue;
                                    break;
                            }
                        }

                        DT_Bilgi.Add(kartBilgi);
                    }
                    break;
                }
            }
        }
        catch (Exception)
        {
            // XML parsing hatası durumunda boş liste döner
            DT_Bilgi.Clear();
        }
    }
} 