using System.Data;
using System.Xml;
using ParamApi.Sdk.Models.Shared;
using TurkposService;

namespace ParamApi.Sdk.Models.Responses;

/// <summary>
/// Özel oran son kullanıcı listesi response modeli
/// </summary>
public class TP_Ozel_Oran_SK_Liste_Response
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
    /// Özel oran SK bilgileri listesi
    /// </summary>
    public List<DT_OzelOranSKBilgi> OzelOranSKBilgileri { get; }

    /// <summary>
    /// İşlemin başarılı olup olmadığını döner
    /// </summary>
    public bool IsSuccess
    {
        get
        {
            if (int.TryParse(Sonuc, out int sonucInt))
                return sonucInt > 0;
            return false;
        }
    }

    /// <summary>
    /// PARAM API response'undan wrapper oluşturur
    /// </summary>
    /// <param name="response">PARAM API ST_Genel_Sonuc response</param>
    public TP_Ozel_Oran_SK_Liste_Response(ST_Genel_Sonuc response)
    {
        ArgumentNullException.ThrowIfNull(response);

        Sonuc = response.Sonuc?.ToString();
        Sonuc_Str = response.Sonuc_Str;
        OzelOranSKBilgileri = new List<DT_OzelOranSKBilgi>();

        // DT_Bilgi'den özel oran SK bilgilerini parse et
        if (response.DT_Bilgi?.Any != null)
        {
            try
            {
                ParseDTBilgi(response.DT_Bilgi.Any);
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

                // DT_Ozel_Oranlar_SK tablosunu kontrol et
                if (dataSet.Tables.Contains("DT_Ozel_Oranlar_SK"))
                {
                    var table = dataSet.Tables["DT_Ozel_Oranlar_SK"];
                    foreach (DataRow row in table.Rows)
                    {
                        var ozelOranSKBilgi = new DT_OzelOranSKBilgi
                        {
                            Ozel_Oran_SK_ID = row.IsNull("Ozel_Oran_SK_ID") ? 0 : Convert.ToInt64(row["Ozel_Oran_SK_ID"]),
                            GUID = row.IsNull("GUID") ? null : row["GUID"].ToString(),
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

                        OzelOranSKBilgileri.Add(ozelOranSKBilgi);
                    }
                }
                break;
            }
        }
    }



    /// <summary>
    /// Belirtilen kart markası için bilgileri getirir
    /// </summary>
    /// <param name="kartMarkasi">Kart markası adı</param>
    /// <returns>Kart markası bilgisi veya null</returns>
    public DT_OzelOranSKBilgi? GetKartMarkasiBilgi(string kartMarkasi)
    {
        return OzelOranSKBilgileri.FirstOrDefault(x => 
            string.Equals(x.Kredi_Karti_Banka, kartMarkasi, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Belirtilen SanalPOS ID için bilgileri getirir
    /// </summary>
    /// <param name="sanalPosId">SanalPOS ID</param>
    /// <returns>SanalPOS bilgisi veya null</returns>
    public DT_OzelOranSKBilgi? GetSanalPosBilgi(int sanalPosId)
    {
        return OzelOranSKBilgileri.FirstOrDefault(x => x.SanalPOS_ID == sanalPosId);
    }

    /// <summary>
    /// Müşteri ödeme sayfası için tüm uygun taksit seçeneklerini getirir
    /// </summary>
    /// <returns>Kart markası ve taksit seçenekleri</returns>
    public Dictionary<string, List<(int TaksitSayisi, string Oran)>> GetAllCustomerPaymentOptions()
    {
        var options = new Dictionary<string, List<(int, string)>>();

        foreach (var oranBilgi in OzelOranSKBilgileri)
        {
            if (!string.IsNullOrEmpty(oranBilgi.Kredi_Karti_Banka))
            {
                options[oranBilgi.Kredi_Karti_Banka] = oranBilgi.GetCustomerPaymentOptions();
            }
        }

        return options;
    }
} 