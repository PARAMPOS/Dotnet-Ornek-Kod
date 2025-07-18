using System.Data;
using System.Xml;
using ParamApi.Sdk.Models.Shared;
using TurkposService;

namespace ParamApi.Sdk.Models.Responses;

/// <summary>
/// Özel oran listesi response modeli
/// </summary>
public class TP_Ozel_Oran_Liste_Response
{
    /// <summary>
    /// İşlem sonucu (1'den büyükse başarılı)
    /// </summary>
    public string? Sonuc { get; }

    /// <summary>
    /// İşlem sonuç açıklaması
    /// </summary>
    public string? Sonuc_Str { get; }

    /// <summary>
    /// Özel oran bilgileri listesi
    /// </summary>
    public List<DT_OzelOranBilgi> OzelOranBilgileri { get; }

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
    /// PARAM API response'undan model oluşturur
    /// </summary>
    /// <param name="paramResponse">PARAM API ST_Genel_Sonuc response</param>
    public TP_Ozel_Oran_Liste_Response(ST_Genel_Sonuc paramResponse)
    {
        Sonuc = paramResponse?.Sonuc;
        Sonuc_Str = paramResponse?.Sonuc_Str;
        OzelOranBilgileri = new List<DT_OzelOranBilgi>();

        // DT_Bilgi'den özel oran bilgilerini parse et
        if (paramResponse?.DT_Bilgi?.Any != null)
        {
            try
            {
                ParseDTBilgi(paramResponse.DT_Bilgi.Any);
            }
            catch (Exception ex)
            {
                // Parse hatası durumunda boş liste döner
                Console.WriteLine($"DT_Bilgi parse hatası: {ex.Message}");
            }
        }
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

                // DT_Ozel_Oranlar tablosunu kontrol et
                if (dataSet.Tables.Contains("DT_Ozel_Oranlar"))
                {
                    var table = dataSet.Tables["DT_Ozel_Oranlar"];
                    foreach (DataRow row in table.Rows)
                    {
                        var ozelOranBilgi = new DT_OzelOranBilgi
                        {
                            Ozel_Oran_ID = row.IsNull("Ozel_Oran_ID") ? 0 : Convert.ToInt64(row["Ozel_Oran_ID"]),
                            GUID = row.IsNull("GUID") ? null : row["GUID"].ToString(),
                            Tarih_Bas = row.IsNull("Tarih_Bas") ? null : row["Tarih_Bas"].ToString(),
                            Tarih_Bit = row.IsNull("Tarih_Bit") ? null : row["Tarih_Bit"].ToString(),
                            SanalPOS_ID = row.IsNull("SanalPOS_ID") ? 0 : Convert.ToInt32(row["SanalPOS_ID"]),
                            Kredi_Karti_Banka = row.IsNull("Kredi_Karti_Banka") ? null : row["Kredi_Karti_Banka"].ToString(),
                            Kredi_Karti_Banka_Gorsel = row.IsNull("Kredi_Karti_Banka_Gorsel") ? null : row["Kredi_Karti_Banka_Gorsel"].ToString(),
                            MO_01 = row.IsNull("MO_01") ? null : row["MO_01"].ToString(),
                            MO_02 = row.IsNull("MO_02") ? null : row["MO_02"].ToString(),
                            MO_03 = row.IsNull("MO_03") ? null : row["MO_03"].ToString(),
                            MO_04 = row.IsNull("MO_04") ? null : row["MO_04"].ToString(),
                            MO_05 = row.IsNull("MO_05") ? null : row["MO_05"].ToString(),
                            MO_06 = row.IsNull("MO_06") ? null : row["MO_06"].ToString(),
                            MO_07 = row.IsNull("MO_07") ? null : row["MO_07"].ToString(),
                            MO_08 = row.IsNull("MO_08") ? null : row["MO_08"].ToString(),
                            MO_09 = row.IsNull("MO_09") ? null : row["MO_09"].ToString(),
                            MO_10 = row.IsNull("MO_10") ? null : row["MO_10"].ToString(),
                            MO_11 = row.IsNull("MO_11") ? null : row["MO_11"].ToString(),
                            MO_12 = row.IsNull("MO_12") ? null : row["MO_12"].ToString()
                        };

                        OzelOranBilgileri.Add(ozelOranBilgi);
                    }
                }
                break;
            }
        }
    }

    /// <summary>
    /// Belirtilen kart markası için özel oran bilgisini döner
    /// </summary>
    /// <param name="kartMarkasi">Kart markası adı (Axess, Maximum vs.)</param>
    /// <returns>Özel oran bilgisi veya null</returns>
    public DT_OzelOranBilgi? GetOzelOranByKartMarkasi(string kartMarkasi)
    {
        return OzelOranBilgileri.FirstOrDefault(x => 
            string.Equals(x.Kredi_Karti_Banka, kartMarkasi, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Belirtilen SanalPOS_ID için özel oran bilgisini döner
    /// </summary>
    /// <param name="sanalPosId">Sanal POS ID</param>
    /// <returns>Özel oran bilgisi veya null</returns>
    public DT_OzelOranBilgi? GetOzelOranBySanalPosId(int sanalPosId)
    {
        return OzelOranBilgileri.FirstOrDefault(x => x.SanalPOS_ID == sanalPosId);
    }

    /// <summary>
    /// Kullanılabilir taksit seçenekleri olan kart markalarını döner
    /// </summary>
    /// <param name="taksitSayisi">Kontrol edilecek taksit sayısı</param>
    /// <returns>Belirtilen taksit için uygun kart markaları</returns>
    public List<DT_OzelOranBilgi> GetAvailableCardsForInstallment(int taksitSayisi)
    {
        return OzelOranBilgileri.Where(x => x.IsTaksitAvailable(taksitSayisi)).ToList();
    }
} 