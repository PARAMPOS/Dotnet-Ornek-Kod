using System.Data;
using System.Xml;
using ParamApi.Sdk.Models.Shared;

namespace ParamApi.Sdk.Models.Responses;

/// <summary>
/// Mutabakat detay sorgulama response modeli
/// PARAM API'den dönen mutabakat detay bilgilerini içerir
/// </summary>
public class TP_Mutabakat_Detay_Response
{
    /// <summary>
    /// İşlem sonucu (1'den büyükse başarılı)
    /// </summary>
    public string? Sonuc { get; set; }

    /// <summary>
    /// İşlem sonuç açıklaması
    /// </summary>
    public string? Sonuc_Str { get; set; }

    /// <summary>
    /// Mutabakat detay bilgilerinin listesi
    /// DT_Bilgi DataSet'inden parse edilen veriler
    /// </summary>
    public List<DT_MutabakatDetayBilgi> MutabakatDetayBilgileri { get; set; } = new();

    /// <summary>
    /// DT_Bilgi XML verilerini parse eder (TurkposService'den çağırılabilir)
    /// </summary>
    /// <param name="dtBilgiElements">XML element dizisi</param>
    public void ParseDataSet(XmlElement[] dtBilgiElements)
    {
        ParseDTBilgi(dtBilgiElements);
    }

    /// <summary>
    /// DT_Bilgi XML verilerini parse eder
    /// </summary>
    private void ParseDTBilgi(XmlElement[] dtBilgiElements)
    {
        if (dtBilgiElements?.Length > 0)
        {
            try
            {
                // İlk element'i DataSet olarak deserialize et
                var dataSet = new DataSet();
                using var reader = new XmlNodeReader(dtBilgiElements[0]);
                dataSet.ReadXml(reader);

                // DT_Mutabakat_Detay tablosunu bul
                if (dataSet.Tables.Contains("DT_Mutabakat_Detay"))
                {
                    var table = dataSet.Tables["DT_Mutabakat_Detay"];
                    foreach (DataRow row in table.Rows)
                    {
                        var detayBilgi = new DT_MutabakatDetayBilgi
                        {
                            GUNSONU_TARIHI = row["GUNSONU_TARIHI"]?.ToString(),
                            ISLEM_TARIHI = row["ISLEM_TARIHI"]?.ToString(),
                            VALOR_TARIHI = row["VALOR_TARIHI"]?.ToString(),
                            KART_NO = row["KART_NO"]?.ToString(),
                            TRANSACTION_TIPI = row["TRANSACTION_TIPI"]?.ToString(),
                            PROVIZYON_NO = row["PROVIZYON_NO"]?.ToString(),
                            TAKSIT_SIRASI = row["TAKSIT_SIRASI"] != DBNull.Value && 
                                           int.TryParse(row["TAKSIT_SIRASI"]?.ToString(), out var taksitSirasi) 
                                           ? taksitSirasi : null,
                            TAKSIT_SAYISI = row["TAKSIT_SAYISI"] != DBNull.Value && 
                                           int.TryParse(row["TAKSIT_SAYISI"]?.ToString(), out var taksitSayisi) 
                                           ? taksitSayisi : null,
                            PROVIZYON_TUTARI = row["PROVIZYON_TUTARI"]?.ToString(),
                            KOMISYON_TUTARI = row["KOMISYON_TUTARI"]?.ToString(),
                            KOMISYON_ORANI = row["KOMISYON_ORANI"]?.ToString(),
                            NET_TUTAR = row["NET_TUTAR"]?.ToString(),
                            SIPARIS_NO = row["SIPARIS_NO"]?.ToString(),
                            ANA_KART_TIPI = row["ANA_KART_TIPI"]?.ToString(),
                            ALT_KART_TIPI = row["ALT_KART_TIPI"]?.ToString()
                        };

                        MutabakatDetayBilgileri.Add(detayBilgi);
                    }
                }
            }
            catch (Exception ex)
            {
                // Parse hatası durumunda boş liste kalır
                Console.WriteLine($"DT_Bilgi parse hatası: {ex.Message}");
            }
        }
    }
} 