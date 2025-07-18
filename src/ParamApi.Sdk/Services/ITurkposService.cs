using TurkposService;
using ParamApi.Sdk.Models.Requests;
using ParamApi.Sdk.Models.Responses;

namespace ParamApi.Sdk.Services;

/// <summary>
/// Param TurkPos servis interface'i
/// PARAM API method isimlerini aynen korur
/// </summary>
public interface ITurkposService : IDisposable
{
    /// <summary>
    /// TP_WMD_UCD - Nonsecure/3D Ödeme İşlemi
    /// PARAM API'nin temel ödeme metodu (3D Model)
    /// </summary>
    /// <param name="request">TP_WMD_UCD request modeli</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>TP_WMD_UCD response wrapper</returns>
    Task<TP_WMD_UCD_Response> TP_WMD_UCDAsync(
        TP_WMD_UCD_Request request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// SHA2B64 Hash hesaplama - PARAM API'nin hash metodu
    /// </summary>
    /// <param name="data">Hash'lenecek veri</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Base64 encoded SHA256 hash</returns>
    Task<string> SHA2B64Async(string data, CancellationToken cancellationToken = default);

    /// <summary>
    /// TP_WMD_Pay - 3D İşlemini Tamamlama
    /// Doğrulaması yapılan kartlardan tutar çekimi yapmak için kullanılır
    /// </summary>
    /// <param name="request">TP_WMD_PAY request modeli</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>TP_WMD_PAY response wrapper</returns>
    Task<TP_WMD_PAY_Response> TP_WMD_PAYAsync(
        TP_WMD_PAY_Request request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// TP_Islem_Odeme_OnProv_WMD - Ön Provizyon İşlemi Başlatma
    /// Nonsecure/3D ödeme işleminin başlatılacağı metottur
    /// </summary>
    /// <param name="request">Ön provizyon request modeli</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Ön provizyon response wrapper</returns>
    Task<TP_Islem_Odeme_OnProv_WMD_Response> TP_Islem_Odeme_OnProv_WMDAsync(
        TP_Islem_Odeme_OnProv_WMD_Request request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// TP_Islem_Odeme_OnProv_Kapa - Ön Provizyon Kapama İşlemi
    /// Ön provizyon işlemini satışa dönüştürür
    /// </summary>
    /// <param name="request">Provizyon kapama request modeli</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Provizyon kapama response wrapper</returns>
    Task<TP_Islem_Odeme_OnProv_Kapa_Response> TP_Islem_Odeme_OnProv_KapaAsync(
        TP_Islem_Odeme_OnProv_Kapa_Request request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// TP_Islem_Iptal_OnProv - Ön Provizyon İptal İşlemi
    /// Satış işlemi yapılmamış provizyon iptali için kullanılır
    /// </summary>
    /// <param name="request">Ön provizyon iptal request modeli</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Ön provizyon iptal response wrapper</returns>
    Task<TP_Islem_Iptal_OnProv_Response> TP_Islem_Iptal_OnProvAsync(
        TP_Islem_Iptal_OnProv_Request request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// TP_Islem_Odeme_WD - Dövizli Ödeme İşlemi
    /// Bu metot sadece yabancı kartlar ile çalışmaktadır
    /// </summary>
    /// <param name="request">Dövizli ödeme request modeli</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Dövizli ödeme response wrapper</returns>
    Task<TP_Islem_Odeme_WD_Response> TP_Islem_Odeme_WDAsync(
        TP_Islem_Odeme_WD_Request request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// TP_Islem_Odeme_BKM - BKM Express Ödeme İşlemi
    /// BKM Express aracılığı ile ödeme işleminin başlatılacağı metottur
    /// </summary>
    /// <param name="request">BKM Express ödeme request modeli</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>BKM Express ödeme response wrapper</returns>
    Task<TP_Islem_Odeme_BKM_Response> TP_Islem_Odeme_BKMAsync(
        TP_Islem_Odeme_BKM_Request request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// TP_Islem_Iptal_Iade_Kismi2 - Kısmi İptal/İade İşlemi
    /// Başarılı bir kredi kartı işleminin iptal veya iadesini yapmak için kullanılır
    /// İptal işlemi ödeme işleminin gerçekleştiği gün, iade işlemi ödeme işlemi gün sonuna girdikten sonraki günlerde yapılır
    /// </summary>
    /// <param name="request">Kısmi iptal/iade request modeli</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Kısmi iptal/iade response wrapper</returns>
    Task<TP_Islem_Iptal_Iade_Kismi2_Response> TP_Islem_Iptal_Iade_Kismi2Async(
        TP_Islem_Iptal_Iade_Kismi2_Request request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// TP_Ozel_Oran_Liste - Özel Oran Listesi
    /// Firma tarafından üye işyerine özel verilmiş sanal pos oranları listelenir
    /// Üye işyeri bu oranlar üzerinde değişiklik yapabilir (Kullanıcı Pos Oranları)
    /// GUID bilgisi ParamApiOptions'dan alınır
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Özel oran listesi response wrapper</returns>
    Task<TP_Ozel_Oran_Liste_Response> TP_Ozel_Oran_ListeAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// TP_Ozel_Oran_SK_Liste - Özel Oran Son Kullanıcı Listesi
    /// Özel oran son kullanıcı liste, standart olarak Firma Pos Oranları deki metottan dönen oranların aynısı döner
    /// Üye işyerinin müşterisine göstereceği komisyon oranlarını listeler
    /// GUID bilgisi ParamApiOptions'dan alınır
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Özel oran son kullanıcı listesi response wrapper</returns>
    Task<TP_Ozel_Oran_SK_Liste_Response> TP_Ozel_Oran_SK_ListeAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// TP_Ozel_Oran_SK_Guncelle - Özel Oran Son Kullanıcı Güncelleme
    /// Üye işyerinin müşteri oranlarını güncellemesi için kullanılır
    /// Firma oranından daha düşük oran verilebilir, fark üye işyeri karşılar
    /// GUID bilgisi ParamApiOptions'dan alınır
    /// </summary>
    /// <param name="request">Özel oran güncelleme request modeli</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Güncelleme işlemi response wrapper</returns>
    Task<TP_Ozel_Oran_SK_Guncelle_Response> TP_Ozel_Oran_SK_GuncelleAsync(
        TP_Ozel_Oran_SK_Guncelle_Request request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// TP_Mutabakat_Ozet - Mutabakat Özet Sorgulama
    /// Belirli tarihler arasındaki üye işyerinin işlemleri özet biçiminde alabilirsiniz
    /// Başarılı, iptal ve iade işlemlerinin sayı ve tutar özet bilgilerini döner
    /// </summary>
    /// <param name="request">Mutabakat özet sorgu bilgileri</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Mutabakat özet response wrapper</returns>
    Task<TP_Mutabakat_Ozet_Response> TP_Mutabakat_OzetAsync(
        TP_Mutabakat_Ozet_Request request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// TP_Mutabakat_Detay - Mutabakat Detay Sorgulama
    /// Belirli tarihte üye işyerinin mutabakat detaylarını almak için kullanılır
    /// İşlem detaylarını, kart bilgilerini, tutarları döner
    /// GUID bilgisi ParamApiOptions'dan alınır
    /// </summary>
    /// <param name="request">Mutabakat detay sorgu bilgileri</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Mutabakat detay response wrapper</returns>
    Task<TP_Mutabakat_Detay_Response> TP_Mutabakat_DetayAsync(
        TP_Mutabakat_Detay_Request request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// İşlem durumu sorgulama
    /// İşlemin başarılı, başarısız, iptal veya iade durumunda olduğunu öğrenmek için kullanılır
    /// Dekont_ID, Siparis_ID ve Islem_ID değerlerinden herhangi birini göndererek işlem durumunu sorgulayabilirsiniz
    /// </summary>
    Task<TP_Islem_Sorgulama4_Response> TP_Islem_Sorgulama4Async(TP_Islem_Sorgulama4_Request request, CancellationToken cancellationToken = default);

    /// <summary>
    /// TP_Islem_Izleme - İşlem İzleme
    /// Yapılan işlemlerin belirli tarih aralığında izlenmesi için kullanılır
    /// İşlem tipi, taksit sayısı, komisyon oran, komisyon tutar, net tutar, tutar, dekont ID,
    /// ödeme yapan bilgisi, işlem güvenlik bilgilerine (NS (NonSecure) veya 3D) ulaşılır
    /// GUID bilgisi ParamApiOptions'dan alınır
    /// </summary>
    /// <param name="request">İşlem izleme request modeli</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>İşlem izleme response wrapper</returns>
    Task<TP_Islem_Izleme_Response> TP_Islem_IzlemeAsync(
        TP_Islem_Izleme_Request request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// TP_Islem_Dekont_Gonder - Dekont E-posta Gönderimi
    /// Dekontun e-posta olarak gönderilmesini sağlar
    /// E-posta adresi belirtilmezse işyerinin kayıtlı e-posta adresine gönderilir
    /// GUID bilgisi ParamApiOptions'dan alınır
    /// </summary>
    /// <param name="request">Dekont gönderimi request modeli</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Dekont gönderimi response wrapper</returns>
    Task<TP_Islem_Dekont_Gonder_Response> TP_Islem_Dekont_GonderAsync(
        TP_Islem_Dekont_Gonder_Request request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// BIN_SanalPos - BIN Sorgulama
    /// Kredi kartına ait kart-banka bilgisini ve SanalPOS_ID değerini döner
    /// BIN değerinin boş gönderilmesi durumunda bütün kayıtlar dönecektir
    /// </summary>
    /// <param name="request">BIN sorgulama request modeli</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>BIN sorgulama response wrapper</returns>
    Task<BIN_SanalPos_Response> BIN_SanalPosAsync(
        BIN_SanalPos_Request request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// TP_Islem_Odeme_OnProv_WKS - Saklı kart ile ön provizyon işlemi
    /// Daha önce kaydedilen kart ile ön provizyon işlemi yapmak için kullanılır
    /// GUID bilgisi ParamApiOptions'dan alınır
    /// </summary>
    /// <param name="request">Saklı kart ön provizyon request modeli</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Ön provizyon işlem sonucu</returns>
    Task<TP_Islem_Odeme_OnProv_WKS_Response> TP_Islem_Odeme_OnProv_WKSAsync(
        TP_Islem_Odeme_OnProv_WKS_Request request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// TP_KK_Verify - Kredi Kartı Doğrulama
    /// Kart doğrulama ve 3D Secure işlemleri için kullanılır
    /// Sonuc > 0 ise UCD_URL'e yönlendirme yapılmalıdır
    /// </summary>
    /// <param name="request">Kart doğrulama request modeli</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Kart doğrulama response wrapper</returns>
    Task<TP_KK_Verify_Response> TP_KK_VerifyAsync(
        TP_KK_Verify_Request request,
        CancellationToken cancellationToken = default);
} 