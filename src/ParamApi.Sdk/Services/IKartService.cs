using ParamApi.Sdk.Models.Requests;
using ParamApi.Sdk.Models.Responses;
using ParamApi.Sdk.Models.Shared;

namespace ParamApi.Sdk.Services;

/// <summary>
/// Param KS (Kart Saklama) servis arayüzü
/// Kredi kartı saklama ve yönetimi işlemleri için kullanılır
/// </summary>
public interface IKartService : IDisposable
{
    /// <summary>
    /// KS_Kart_Ekle - Kredi Kartı Saklama
    /// Kredi kartı bilgilerini güvenli olarak saklamak için kullanılır
    /// GUID bilgisi ParamApiOptions'dan alınır
    /// </summary>
    /// <param name="request">Kart saklama request modeli</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Kart saklama response wrapper</returns>
    Task<KS_Kart_Ekle_Response> KS_Kart_EkleAsync(
        KS_Kart_Ekle_Request request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// KS_Tahsilat - Saklanmış Kredi Kartından Tahsilat
    /// Daha önce KS_Kart_Ekle ile saklanan kartlardan ödeme almak için kullanılır
    /// GUID bilgisi ParamApiOptions'dan alınır
    /// </summary>
    /// <param name="request">Tahsilat request modeli</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Tahsilat response wrapper</returns>
    Task<KS_Tahsilat_Response> KS_TahsilatAsync(
        KS_Tahsilat_Request request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// KS_Kart_Liste - Saklı Kart Listesi
    /// Saklı kartların listelenmesi için kullanılır
    /// GUID bilgisi ParamApiOptions'dan alınır
    /// </summary>
    /// <param name="request">Kart listesi request modeli</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Kart listesi response wrapper</returns>
    Task<KS_Kart_Liste_Response> KS_Kart_ListeAsync(
        KS_Kart_Liste_Request request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// KS_Kart_Sil - Saklı Kart Silme
    /// Daha önce kaydedilen kredi kartını sistemden silmek için kullanılır
    /// </summary>
    /// <param name="request">Kart silme request modeli</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Silme işlemi sonucu</returns>
    Task<KS_Kart_Sil_Response> KS_Kart_SilAsync(
        KS_Kart_Sil_Request request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// GetTestCards - PARAM API Test Kartları
    /// Test ortamında kullanılabilecek kart bilgilerini döner
    /// Gerçek işlemler için kullanılmamalı, sadece test amaçlı
    /// </summary>
    /// <returns>Test kart bilgileri listesi</returns>
    List<TestCardInfo> GetTestCards();
} 