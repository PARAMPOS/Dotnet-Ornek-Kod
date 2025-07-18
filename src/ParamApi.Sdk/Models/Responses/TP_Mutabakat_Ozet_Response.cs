using System.Data;
using System.Xml;
using ParamApi.Sdk.Models.Shared;

namespace ParamApi.Sdk.Models.Responses;

/// <summary>
/// Mutabakat özet sorgulama response modeli
/// PARAM API'den dönen mutabakat özet bilgilerini içerir
/// </summary>
public class TP_Mutabakat_Ozet_Response
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
    /// Mutabakat özet bilgilerinin listesi
    /// DT_Bilgi DataSet'inden parse edilen veriler
    /// </summary>
    public List<DT_MutabakatOzetBilgi> MutabakatOzetBilgileri { get; }

    /// <summary>
    /// İşlemin başarılı olup olmadığını döner
    /// </summary>
    public bool IsSuccess
    {
        get
        {
            if (int.TryParse(Sonuc, out int sonucInt))
            {
                return sonucInt > 0;
            }
            return false;
        }
    }



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
        foreach (var element in dtBilgiElements)
        {
            if (element.LocalName == "NewDataSet")
            {
                // DataSet olarak parse et
                var dataSet = new DataSet();
                using var reader = new StringReader(element.OuterXml);
                dataSet.ReadXml(reader);

                // DT_Mutabakat_Ozet tablosunu kontrol et
                if (dataSet.Tables.Contains("DT_Mutabakat_Ozet"))
                {
                    var table = dataSet.Tables["DT_Mutabakat_Ozet"];
                    foreach (DataRow row in table.Rows)
                    {
                        var mutabakatOzetBilgi = new DT_MutabakatOzetBilgi
                        {
                            Basarili_Islem_Sayi = row.IsNull("Basarili_Islem_Sayi") ? null : Convert.ToInt32(row["Basarili_Islem_Sayi"]),
                            Basarili_Islem_Toplam_Tutar = row.IsNull("Basarili_Islem_Toplam_Tutar") ? null : row["Basarili_Islem_Toplam_Tutar"].ToString(),
                            Iptal_Islem_Sayi = row.IsNull("Iptal_Islem_Sayi") ? null : Convert.ToInt32(row["Iptal_Islem_Sayi"]),
                            Iptal_Islem_Toplam_Tutar = row.IsNull("Iptal_Islem_Toplam_Tutar") ? null : row["Iptal_Islem_Toplam_Tutar"].ToString(),
                            Iade_Islem_Sayi = row.IsNull("Iade_Islem_Sayi") ? null : Convert.ToInt32(row["Iade_Islem_Sayi"]),
                            Iade_Islem_Toplam_Tutar = row.IsNull("Iade_Islem_Toplam_Tutar") ? null : row["Iade_Islem_Toplam_Tutar"].ToString()
                        };

                        MutabakatOzetBilgileri.Add(mutabakatOzetBilgi);
                    }
                }
                break;
            }
        }
    }

    /// <summary>
    /// Toplam başarılı işlem tutarını döner
    /// </summary>
    /// <returns>Başarılı işlem toplam tutarı</returns>
    public string? GetTotalSuccessfulAmount()
    {
        return MutabakatOzetBilgileri.FirstOrDefault()?.Basarili_Islem_Toplam_Tutar;
    }

    /// <summary>
    /// Toplam başarılı işlem sayısını döner
    /// </summary>
    /// <returns>Başarılı işlem sayısı</returns>
    public int? GetTotalSuccessfulCount()
    {
        return MutabakatOzetBilgileri.FirstOrDefault()?.Basarili_Islem_Sayi;
    }

    /// <summary>
    /// Parametresiz constructor
    /// </summary>
    public TP_Mutabakat_Ozet_Response()
    {
        MutabakatOzetBilgileri = new List<DT_MutabakatOzetBilgi>();
    }
} 