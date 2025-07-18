using ParamApi.Sdk.Models.Shared;

namespace ParamApi.Sdk.Models.Responses;

/// <summary>
/// TP_Islem_Sorgulama4 metodu için yanıt modeli
/// İşlem durumu sorgulama sonucunu içerir
/// </summary>
public class TP_Islem_Sorgulama4_Response
{
    /// <summary>
    /// İşlem sonucu (1: Başarılı, 0: Başarısız)
    /// </summary>
    public int Sonuc { get; set; }

    /// <summary>
    /// İşlem sonuç açıklaması
    /// </summary>
    public string? Sonuc_Str { get; set; }

    /// <summary>
    /// İşlem sorgulama detay bilgileri listesi
    /// DataSet parsing ile DT_IslemSorgulama4Bilgi nesnelerine dönüştürülür
    /// </summary>
    public List<DT_IslemSorgulama4Bilgi> DT_Bilgi { get; set; } = new();

    /// <summary>
    /// İşlemin başarılı olup olmadığını döner
    /// </summary>
    public bool IsSuccess => Sonuc == 1;

    /// <summary>
    /// İlk işlem kaydını getirir (genellikle tek kayıt döner)
    /// </summary>
    public DT_IslemSorgulama4Bilgi? FirstTransaction => DT_Bilgi.FirstOrDefault();

    /// <summary>
    /// İşlem durumunu kontrol eder
    /// </summary>
    public string GetTransactionStatus()
    {
        var firstTx = FirstTransaction;
        if (firstTx == null) return "Bilinmiyor";

        return firstTx.Durum switch
        {
            "SUCCESS" => "Başarılı",
            "PARTIAL_REFUND" => "Kısmi İade",
            "FAIL" => "Başarısız",
            "BANK_FAIL" => "Banka Hatası",
            "CANCEL" => "İptal",
            "REFUND" => "İade",
            _ => firstTx.Durum ?? "Bilinmiyor"
        };
    }

    /// <summary>
    /// İşlem türünü kontrol eder
    /// </summary>
    public string GetTransactionType()
    {
        var firstTx = FirstTransaction;
        if (firstTx == null) return "Bilinmiyor";

        return firstTx.Islem_Tipi switch
        {
            "SALE" => "Satış",
            "PRE_AUTH" => "Ön Provizyon",
            "POST_AUTH" => "Provizyon Kapama",
            _ => firstTx.Islem_Tipi ?? "Bilinmiyor"
        };
    }

    /// <summary>
    /// Ödeme sonucunu kontrol eder
    /// </summary>
    public string GetPaymentResult()
    {
        var firstTx = FirstTransaction;
        if (firstTx == null) return "Bilinmiyor";

        return firstTx.Odeme_Sonuc switch
        {
            "1" => "Başarılı",
            "12" => "İptal",
            "13" => "İade",
            _ => firstTx.Odeme_Sonuc_Aciklama ?? "Bilinmiyor"
        };
    }
} 