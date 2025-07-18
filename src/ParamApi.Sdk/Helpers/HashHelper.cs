using System.Security.Cryptography;
using System.Text;
using TurkposService;

namespace ParamApi.Sdk.Helpers;

/// <summary>
/// PARAM API hash hesaplama yardımcı sınıfı
/// PARAM API'nin kendi SHA2B64 metodunu kullanır
/// </summary>
public static class HashHelper
{
    /// <summary>
    /// PARAM API'nin SHA2B64 metodunu kullanarak hash hesaplar
    /// </summary>
    /// <param name="client">TurkPosWSPRODSoapClient instance</param>
    /// <param name="data">Hash'lenecek string</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>PARAM API'den dönen SHA2B64 hash değeri</returns>
    public static async Task<string> ComputeSHA2B64Async(
        TurkPosWSPRODSoapClient client, 
        string data, 
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(data))
            return string.Empty;

        try
        {
            var result = await client.SHA2B64Async(data);
            return result ?? string.Empty;
        }
        catch (Exception)
        {
            // PARAM API bağlantı hatası durumunda boş string döner
            return string.Empty;
        }
    }

    /// <summary>
    /// TP_WMD_UCD metodu için hash hesaplama
    /// Hash Formula: CLIENT_CODE + GUID + Taksit + Islem_Tutar + Toplam_Tutar + Siparis_ID
    /// </summary>
    public static async Task<string> CalculateTP_WMD_UCD_HashAsync(
        TurkPosWSPRODSoapClient client,
        string clientCode,
        string guid,
        int taksit,
        string islemTutar,
        string toplamTutar,
        string siparisId,
        CancellationToken cancellationToken = default)
    {
        var hashString = $"{clientCode}{guid}{taksit}{islemTutar}{toplamTutar}{siparisId}";
        return await ComputeSHA2B64Async(client, hashString, cancellationToken);
    }

    /// <summary>
    /// Ön Provizyon için hash hesaplama
    /// Hash Formula: CLIENT_CODE + GUID + Islem_Tutar + Toplam_Tutar + Siparis_ID + Hata_URL + Basarili_URL  
    /// </summary>
    public static async Task<string> CalculateOnProvizyonHashAsync(
        TurkPosWSPRODSoapClient client,
        string clientCode,
        string guid,
        string islemTutar,
        string toplamTutar,
        string siparisId,
        string hataUrl,
        string basariliUrl,
        CancellationToken cancellationToken = default)
    {
        var hashString = $"{clientCode}{guid}{islemTutar}{toplamTutar}{siparisId}{hataUrl}{basariliUrl}";
        return await ComputeSHA2B64Async(client, hashString, cancellationToken);
    }

    /// <summary>
    /// TP_Islem_Odeme_WD (Dövizli İşlem) için hash hesaplama
    /// Hash Formula: CLIENT_CODE + GUID + Islem_Tutar + Toplam_Tutar + Siparis_ID + Hata_URL + Basarili_URL
    /// </summary>
    public static async Task<string> CalculateTP_Islem_Odeme_WD_HashAsync(
        TurkPosWSPRODSoapClient client,
        string clientCode,
        string guid,
        string islemTutar,
        string toplamTutar,
        string siparisId,
        string hataUrl,
        string basariliUrl,
        CancellationToken cancellationToken = default)
    {
        var hashString = $"{clientCode}{guid}{islemTutar}{toplamTutar}{siparisId}{hataUrl}{basariliUrl}";
        return await ComputeSHA2B64Async(client, hashString, cancellationToken);
    }

    /// <summary>
    /// TP_Islem_Odeme_BKM için hash hesaplama
    /// Hash Formula: CLIENT_CODE + GUID + Amount + Order_ID + Error_URL + Success_URL
    /// </summary>
    public static async Task<string> CalculateTP_Islem_Odeme_BKM_HashAsync(
        TurkPosWSPRODSoapClient client,
        string clientCode,
        string guid,
        string amount,
        string orderId,
        string errorUrl,
        string successUrl,
        CancellationToken cancellationToken = default)
    {
        var hashString = $"{clientCode}{guid}{amount}{orderId}{errorUrl}{successUrl}";
        return await ComputeSHA2B64Async(client, hashString, cancellationToken);
    }

    /// <summary>
    /// Tutarı PARAM API formatına çevirir (virgüllü format)
    /// Örnek: 1000.50 -> "1000,50"
    /// </summary>
    /// <param name="amount">Tutar</param>
    /// <returns>Virgüllü format string</returns>
    public static string FormatAmount(decimal amount)
    {
        return amount.ToString("F2", new System.Globalization.CultureInfo("tr-TR"));
    }

    /// <summary>
    /// TP_Islem_Odeme_OnProv_WKS için hash hesaplama
    /// Hash Formula: CLIENT_CODE + GUID + Islem_Tutar + Toplam_Tutar + Siparis_ID + Hata_URL + Basarili_URL
    /// </summary>
    public static async Task<string> CalculateTP_Islem_Odeme_OnProv_WKS_HashAsync(
        TurkPosWSPRODSoapClient client,
        string clientCode,
        string guid,
        string islemTutar,
        string toplamTutar,
        string siparisId,
        string hataUrl,
        string basariliUrl,
        CancellationToken cancellationToken = default)
    {
        var hashString = $"{clientCode}{guid}{islemTutar}{toplamTutar}{siparisId}{hataUrl}{basariliUrl}";
        return await ComputeSHA2B64Async(client, hashString, cancellationToken);
    }

    /// <summary>
    /// Komisyon dahil tutarı hesaplar
    /// Toplam_Tutar = Islem_Tutar + ((Islem_Tutar x Komisyon_Oran) / 100)
    /// </summary>
    /// <param name="islemTutar">İşlem tutarı</param>
    /// <param name="komisyonOran">Komisyon oranı (yüzde)</param>
    /// <returns>Komisyon dahil tutar</returns>
    public static decimal CalculateToplamTutar(decimal islemTutar, decimal komisyonOran)
    {
        return islemTutar + ((islemTutar * komisyonOran) / 100);
    }
} 