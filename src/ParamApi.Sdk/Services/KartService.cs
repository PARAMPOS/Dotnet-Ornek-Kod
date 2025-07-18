using System.ServiceModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;
using ParamApi.Sdk.Configuration;
using ParamApi.Sdk.Models.Requests;
using ParamApi.Sdk.Models.Responses;
using ParamApi.Sdk.Models.Shared;
using KsService;

namespace ParamApi.Sdk.Services;

/// <summary>
/// Param KS (Kart Saklama) servis implementasyonu
/// Runtime'da endpoint değişimi destekler
/// </summary>
public class KartService : IKartService
{
    private readonly TP_KSSoapClient _client;
    private readonly ParamApiOptions _options;

    public KartService(IOptions<ParamApiOptions> options)
    {
        _options = options.Value;
        
        // Runtime'da endpoint'i ayarla
        var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
        binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
        
        var endpoint = new EndpointAddress(_options.GetKsServiceEndpoint());
        _client = new TP_KSSoapClient(binding, endpoint);
    }

    /// <summary>
    /// Güvenlik bilgilerini oluşturur
    /// </summary>
    private KsService.ST_WS_Guvenlik CreateSecurity()
    {
        return new KsService.ST_WS_Guvenlik
        {
            CLIENT_CODE = int.Parse(_options.ClientCode),
            CLIENT_USERNAME = _options.Username,
            CLIENT_PASSWORD = _options.Password
        };
    }

    /// <inheritdoc />
    public async Task<KS_Kart_Ekle_Response> KS_Kart_EkleAsync(
        KS_Kart_Ekle_Request request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Request validation
        if (!request.IsValid(out var validationResults))
        {
            var errors = string.Join(", ", validationResults.Select(v => v.ErrorMessage));
            throw new ArgumentException($"Request validation failed: {errors}", nameof(request));
        }

        // GUID kontrolü - options'dan al
        if (string.IsNullOrWhiteSpace(_options.Guid))
        {
            throw new ArgumentException("GUID bilgisi ParamApiOptions'da tanımlanmalıdır.", nameof(_options.Guid));
        }

        var security = CreateSecurity();

        // PARAM KS API çağrısı
        var result = await _client.KS_Kart_EkleAsync(
            security,
            _options.Guid,
            request.KK_Sahibi,
            request.KK_No,
            request.KK_SK_Ay,
            request.KK_SK_Yil,
            request.KK_Kart_Adi ?? string.Empty,
            request.KK_Islem_ID ?? string.Empty
        ).ConfigureAwait(false);

        // Response dönüştür
        var response = new KS_Kart_Ekle_Response
        {
            Sonuc = result.Sonuc,
            Sonuc_Str = result.Sonuc_Str ?? string.Empty,
            KS_GUID = result.KS_GUID ?? string.Empty
        };

        return response;
    }

    /// <inheritdoc />
    public async Task<KS_Tahsilat_Response> KS_TahsilatAsync(
        KS_Tahsilat_Request request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Request validation
        if (!request.IsValid(out var validationResults))
        {
            var errors = string.Join(", ", validationResults.Select(v => v.ErrorMessage));
            throw new ArgumentException($"Request validation failed: {errors}", nameof(request));
        }

        // GUID kontrolü - options'dan al
        if (string.IsNullOrWhiteSpace(_options.Guid))
        {
            throw new ArgumentException("GUID bilgisi ParamApiOptions'da tanımlanmalıdır.", nameof(_options.Guid));
        }

        var security = CreateSecurity();

        // Tutarları PARAM API formatına dönüştür (virgüllü format)
        var formattedIslemTutar = request.GetFormattedIslemTutar();
        var formattedToplamTutar = request.GetFormattedToplamTutar();

        // PARAM KS API çağrısı
        var result = await _client.KS_TahsilatAsync(
            security,
            _options.Guid,
            request.KS_GUID,
            request.CVV ?? string.Empty,
            request.KK_Sahibi_GSM,
            request.Hata_URL,
            request.Basarili_URL,
            request.Siparis_ID,
            request.Siparis_Aciklama ?? string.Empty,
            request.Taksit,
            formattedIslemTutar,
            formattedToplamTutar,
            request.Islem_Guvenlik_Tip,
            request.Islem_ID ?? string.Empty,
            request.IPAdr,
            request.Ref_URL ?? string.Empty,
            request.Data1 ?? string.Empty,
            request.Data2 ?? string.Empty,
            request.Data3 ?? string.Empty,
            request.Data4 ?? string.Empty,
            request.KK_Islem_ID ?? string.Empty
        ).ConfigureAwait(false);

        // Response dönüştür
        var response = new KS_Tahsilat_Response
        {
            Sonuc = int.TryParse(result.Sonuc, out var sonuc) ? sonuc : 0,
            Sonuc_Str = result.Sonuc_Str ?? string.Empty,
            UCD_URL = result.UCD_URL ?? string.Empty,
            Islem_ID = result.Islem_ID
        };

        return response;
    }

    /// <inheritdoc />
    public async Task<KS_Kart_Liste_Response> KS_Kart_ListeAsync(
        KS_Kart_Liste_Request request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Request validation
        if (!request.IsValid(out var validationResults))
        {
            var errors = string.Join(", ", validationResults.Select(v => v.ErrorMessage));
            throw new ArgumentException($"Request validation failed: {errors}", nameof(request));
        }

        // GUID kontrolü - options'dan al
        if (string.IsNullOrWhiteSpace(_options.Guid))
        {
            throw new ArgumentException("GUID bilgisi ParamApiOptions'da tanımlanmalıdır.", nameof(_options.Guid));
        }

        var security = CreateSecurity();

        // PARAM KS API çağrısı
        var result = await _client.KS_Kart_ListeAsync(
            security,
            _options.Guid,
            request.KK_Kart_Adi,
            request.KK_Islem_ID ?? string.Empty
        ).ConfigureAwait(false);

        // Response dönüştür
        var response = new KS_Kart_Liste_Response
        {
            Sonuc = result.Sonuc,
            Sonuc_Str = result.Sonuc_Str ?? string.Empty
        };

        // DataSet'i parse et
        if (result.DT_Bilgi?.Any != null)
        {
            response.ParseDataSet(result.DT_Bilgi.Any);
        }

        return response;
    }

    /// <inheritdoc />
    public async Task<KS_Kart_Sil_Response> KS_Kart_SilAsync(
        KS_Kart_Sil_Request request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Request validation
        if (!request.IsValid(out var validationResults))
        {
            var errors = string.Join(", ", validationResults.Select(v => v.ErrorMessage));
            throw new ArgumentException($"Request validation failed: {errors}", nameof(request));
        }

        var security = CreateSecurity();

        // PARAM KS API çağrısı - Kart silme
        var result = await _client.KS_Kart_SilAsync(
            security,
            request.KS_GUID,
            request.KK_Islem_ID
        ).ConfigureAwait(false);

        // Response dönüştür
        var response = new KS_Kart_Sil_Response
        {
            Sonuc = result.Sonuc,
            Sonuc_Str = result.Sonuc_Str ?? string.Empty
        };

        return response;
    }

    /// <inheritdoc />
    public List<TestCardInfo> GetTestCards()
    {
        return TestCardInfo.GetAllTestCards();
    }

    /// <summary>
    /// Kaynakları temizler
    /// </summary>
    public void Dispose()
    {
        // WCF client'ı Close ile temizlenir
        try
        {
            if (_client?.State == CommunicationState.Opened)
            {
                _client.Close();
            }
        }
        catch (Exception)
        {
            _client?.Abort();
        }
    }
} 