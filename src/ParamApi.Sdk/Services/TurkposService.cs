using System.ServiceModel;
using System.Data;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;
using ParamApi.Sdk.Configuration;
using ParamApi.Sdk.Models.Requests;
using ParamApi.Sdk.Models.Responses;
using ParamApi.Sdk.Models.Shared;
using ParamApi.Sdk.Helpers;
using TurkposService;

namespace ParamApi.Sdk.Services;

/// <summary>
/// Param TurkPos servis implementasyonu
/// Runtime'da endpoint değişimi destekler
/// </summary>
public class TurkposService : ITurkposService
{
    private readonly TurkPosWSPRODSoapClient _client;
    private readonly ParamApiOptions _options;

    public TurkposService(IOptions<ParamApiOptions> options)
    {
        _options = options.Value;
        
        // Runtime'da endpoint'i ayarla
        var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
        binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
        
        var endpoint = new EndpointAddress(_options.GetTurkposEndpoint());
        _client = new TurkPosWSPRODSoapClient(binding, endpoint);
    }

    /// <summary>
    /// Güvenlik bilgilerini oluşturur
    /// </summary>
    private ST_WS_Guvenlik CreateSecurity()
    {
        return new ST_WS_Guvenlik
        {
            CLIENT_CODE = _options.ClientCode,
            CLIENT_USERNAME = _options.Username,
            CLIENT_PASSWORD = _options.Password
        };
    }

    /// <inheritdoc />
    public async Task<TP_WMD_UCD_Response> TP_WMD_UCDAsync(
        TP_WMD_UCD_Request request,
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
        
        // Format amounts to PARAM API format (virgüllü)
        var islemTutarFormatted = request.GetFormattedIslemTutar();
        var toplamTutarFormatted = request.GetFormattedToplamTutar();

        // Calculate hash using HashHelper
        var islemHash = await HashHelper.CalculateTP_WMD_UCD_HashAsync(
            _client,
            _options.ClientCode,
            _options.Guid ?? string.Empty,
            request.Taksit,
            islemTutarFormatted,
            toplamTutarFormatted,
            request.Siparis_ID,
            cancellationToken);

        var response = await _client.TP_WMD_UCDAsync(
            security,
            _options.Guid ?? string.Empty,
            request.KK_Sahibi,
            request.KK_No,
            request.KK_SK_Ay,
            request.KK_SK_Yil,
            request.KK_CVC,
            request.KK_Sahibi_GSM ?? string.Empty,
            request.Hata_URL,
            request.Basarili_URL,
            request.Siparis_ID,
            request.Siparis_Aciklama ?? string.Empty,
            request.Taksit,
            islemTutarFormatted,
            toplamTutarFormatted,
            islemHash,
            request.Islem_Guvenlik_Tip,
            request.Islem_ID ?? string.Empty,
            request.IPAdr,
            request.Ref_URL ?? string.Empty,
            request.Data1 ?? string.Empty,
            request.Data2 ?? string.Empty,
            request.Data3 ?? string.Empty,
            request.Data4 ?? string.Empty,
            request.Data5 ?? string.Empty);

        return new TP_WMD_UCD_Response(response);
    }

    /// <inheritdoc />
    public async Task<string> SHA2B64Async(string data, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(data);
        
        // PARAM API'nin SHA2B64 metodunu kullan
        return await HashHelper.ComputeSHA2B64Async(_client, data, cancellationToken);
        
        // Alternatif olarak PARAM API'nin kendi metodunu kullanabilirsiniz:
        // return await _client.SHA2B64Async(data);
    }

    /// <inheritdoc />
    public async Task<TP_WMD_PAY_Response> TP_WMD_PAYAsync(
        TP_WMD_PAY_Request request,
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

        var response = await _client.TP_WMD_PayAsync(
            security,
            _options.Guid ?? string.Empty,
            request.UCD_MD,
            request.Islem_GUID,
            request.Siparis_ID);

        return new TP_WMD_PAY_Response(response);
    }

    /// <inheritdoc />
    public async Task<TP_Islem_Odeme_OnProv_WMD_Response> TP_Islem_Odeme_OnProv_WMDAsync(
        TP_Islem_Odeme_OnProv_WMD_Request request,
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
        
        // Format amounts to PARAM API format (virgüllü)
        var islemTutarFormatted = request.GetFormattedIslemTutar();
        var toplamTutarFormatted = request.GetFormattedToplamTutar();

        // Calculate hash using HashHelper - Ön Provizyon Hash Formula
        var islemHash = await HashHelper.CalculateOnProvizyonHashAsync(
            _client,
            _options.ClientCode,
            _options.Guid ?? string.Empty,
            islemTutarFormatted,
            toplamTutarFormatted,
            request.Siparis_ID,
            request.Hata_URL,
            request.Basarili_URL,
            cancellationToken);

        var response = await _client.TP_Islem_Odeme_OnProv_WMDAsync(
            security,
            _options.Guid ?? string.Empty,
            request.KK_Sahibi,
            request.KK_No,
            request.KK_SK_Ay,
            request.KK_SK_Yil,
            request.KK_CVC,
            request.KK_Sahibi_GSM,
            request.Hata_URL,
            request.Basarili_URL,
            request.Siparis_ID,
            request.Siparis_Aciklama ?? string.Empty,
            islemTutarFormatted,
            toplamTutarFormatted,
            islemHash,
            request.Islem_Guvenlik_Tip,
            request.Islem_ID ?? string.Empty,
            request.IPAdr,
            request.Ref_URL ?? string.Empty,
            request.Data1 ?? string.Empty,
            request.Data2 ?? string.Empty,
            request.Data3 ?? string.Empty,
            request.Data4 ?? string.Empty,
            request.Data5 ?? string.Empty,
            request.Taksit);

        return new TP_Islem_Odeme_OnProv_WMD_Response(response);
    }

    /// <inheritdoc />
    public async Task<TP_Islem_Odeme_OnProv_Kapa_Response> TP_Islem_Odeme_OnProv_KapaAsync(
        TP_Islem_Odeme_OnProv_Kapa_Request request,
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
        
        // Format amount to PARAM API format (virgüllü)
        var provTutarFormatted = request.GetFormattedProvTutar();

        var response = await _client.TP_Islem_Odeme_OnProv_KapaAsync(
            security,
            _options.Guid ?? string.Empty,
            request.Prov_ID ?? string.Empty,
            provTutarFormatted,
            request.Siparis_ID);

        return new TP_Islem_Odeme_OnProv_Kapa_Response(response);
    }

    /// <inheritdoc />
    public async Task<TP_Islem_Iptal_OnProv_Response> TP_Islem_Iptal_OnProvAsync(
        TP_Islem_Iptal_OnProv_Request request,
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

        var response = await _client.TP_Islem_Iptal_OnProvAsync(
            security,
            _options.Guid ?? string.Empty,
            request.Prov_ID ?? string.Empty,
            request.Siparis_ID);

        return new TP_Islem_Iptal_OnProv_Response(response);
    }

    /// <inheritdoc />
    public async Task<TP_Islem_Odeme_WD_Response> TP_Islem_Odeme_WDAsync(
        TP_Islem_Odeme_WD_Request request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Request validation
        if (!request.IsValid(out var validationResults))
        {
            var errors = string.Join(", ", validationResults.Select(vr => vr.ErrorMessage));
            throw new ArgumentException($"Request validation failed: {errors}", nameof(request));
        }

        // Güvenlik nesnesi oluştur
        var security = new ST_WS_Guvenlik
        {
            CLIENT_CODE = _options.ClientCode,
            CLIENT_USERNAME = _options.Username,
            CLIENT_PASSWORD = _options.Password
        };

        // Amount'u PARAM API formatına çevir
        var formattedIslemTutar = HashHelper.FormatAmount(request.Islem_Tutar);
        var formattedToplamTutar = HashHelper.FormatAmount(request.Toplam_Tutar);

        // Hash hesapla
        var islemHash = await HashHelper.CalculateTP_Islem_Odeme_WD_HashAsync(
            _client,
            _options.ClientCode,
            _options.Guid ?? string.Empty,
            formattedIslemTutar,
            formattedToplamTutar,
            request.Siparis_ID,
            request.Hata_URL,
            request.Basarili_URL,
            cancellationToken);

        // PARAM API çağrısı
        var response = await _client.TP_Islem_Odeme_WDAsync(
            security,
            (int)request.Doviz_Kodu,
            _options.Guid ?? string.Empty,
            request.KK_Sahibi,
            request.KK_No,
            request.KK_SK_Ay,
            request.KK_SK_Yil,
            request.KK_CVC,
            request.KK_Sahibi_GSM ?? string.Empty,
            request.Hata_URL,
            request.Basarili_URL,
            request.Siparis_ID,
            request.Siparis_Aciklama,
            formattedIslemTutar,
            formattedToplamTutar,
            islemHash,
            request.Islem_Guvenlik_Tip ?? "NS",
            request.Islem_ID ?? string.Empty,
            request.IPAdr,
            request.Ref_URL ?? string.Empty,
            request.Data1 ?? string.Empty,
            request.Data2 ?? string.Empty,
            request.Data3 ?? string.Empty,
            request.Data4 ?? string.Empty,
            request.Data5 ?? string.Empty);

        return new TP_Islem_Odeme_WD_Response(response);
    }

    /// <inheritdoc />
    public async Task<TP_Islem_Odeme_BKM_Response> TP_Islem_Odeme_BKMAsync(
        TP_Islem_Odeme_BKM_Request request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Request validation
        if (!request.IsValid(out var validationResults))
        {
            var errors = string.Join(", ", validationResults.Select(vr => vr.ErrorMessage));
            throw new ArgumentException($"Request validation failed: {errors}", nameof(request));
        }

        // Güvenlik nesnesi oluştur
        var security = CreateSecurity();

        // Amount'u PARAM API formatına çevir
        var formattedAmount = request.GetFormattedAmount();

        // Hash hesapla
        var paymentHash = await HashHelper.CalculateTP_Islem_Odeme_BKM_HashAsync(
            _client,
            _options.ClientCode,
            _options.Guid ?? string.Empty,
            formattedAmount,
            request.Order_ID,
            request.Error_URL,
            request.Success_URL,
            cancellationToken);

        // PARAM API çağrısı
        var response = await _client.TP_Islem_Odeme_BKMAsync(
            security,
            _options.Guid ?? string.Empty,
            request.Customer_Info ?? string.Empty,
            request.Customer_GSM,
            request.Error_URL,
            request.Success_URL,
            request.Order_ID,
            request.Order_Description ?? string.Empty,
            formattedAmount,
            paymentHash,
            request.Transaction_ID ?? string.Empty,
            request.IPAddress,
            request.Referrer_URL ?? string.Empty);

        return new TP_Islem_Odeme_BKM_Response(response);
    }

    /// <inheritdoc />
    public async Task<TP_Islem_Iptal_Iade_Kismi2_Response> TP_Islem_Iptal_Iade_Kismi2Async(
        TP_Islem_Iptal_Iade_Kismi2_Request request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Request validation
        if (!request.IsValid(out var validationResults))
        {
            var errors = string.Join(", ", validationResults.Select(vr => vr.ErrorMessage));
            throw new ArgumentException($"Request validation failed: {errors}", nameof(request));
        }

        // Güvenlik nesnesi oluştur
        var security = CreateSecurity();

        // PARAM API çağrısı
        var response = await _client.TP_Islem_Iptal_Iade_Kismi2Async(
            security,
            _options.Guid ?? string.Empty,
            request.GetDurumString(),
            request.Siparis_ID,
            request.GetTutarDouble()
        ).ConfigureAwait(false);

        return new TP_Islem_Iptal_Iade_Kismi2_Response(response);
    }

    /// <inheritdoc />
    public async Task<TP_Ozel_Oran_Liste_Response> TP_Ozel_Oran_ListeAsync(
        CancellationToken cancellationToken = default)
    {
        // GUID kontrolü - options'dan al
        if (string.IsNullOrWhiteSpace(_options.Guid))
        {
            throw new ArgumentException("GUID bilgisi ParamApiOptions'da tanımlanmalıdır.", nameof(_options.Guid));
        }

        // Güvenlik nesnesi oluştur
        var security = CreateSecurity();

        // PARAM API çağrısı
        var response = await _client.TP_Ozel_Oran_ListeAsync(
            security,
            _options.Guid);

        return new TP_Ozel_Oran_Liste_Response(response);
    }

    /// <inheritdoc />
    public async Task<TP_Ozel_Oran_SK_Liste_Response> TP_Ozel_Oran_SK_ListeAsync(
        CancellationToken cancellationToken = default)
    {
        // GUID kontrolü - options'dan al
        if (string.IsNullOrWhiteSpace(_options.Guid))
        {
            throw new ArgumentException("GUID bilgisi ParamApiOptions'da tanımlanmalıdır.", nameof(_options.Guid));
        }

        // Güvenlik nesnesi oluştur
        var security = CreateSecurity();

        // PARAM API çağrısı
        var response = await _client.TP_Ozel_Oran_SK_ListeAsync(
            security,
            _options.Guid);

        return new TP_Ozel_Oran_SK_Liste_Response(response);
    }

    /// <inheritdoc />
    public async Task<TP_Ozel_Oran_SK_Guncelle_Response> TP_Ozel_Oran_SK_GuncelleAsync(
        TP_Ozel_Oran_SK_Guncelle_Request request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Request validation
        if (!request.IsValid(out var validationResults))
        {
            var errors = string.Join(", ", validationResults.Select(vr => vr.ErrorMessage));
            throw new ArgumentException($"Request validation failed: {errors}", nameof(request));
        }

        // GUID kontrolü - options'dan al
        if (string.IsNullOrWhiteSpace(_options.Guid))
        {
            throw new ArgumentException("GUID bilgisi ParamApiOptions'da tanımlanmalıdır.", nameof(_options.Guid));
        }

        // Format normalizasyonu (nokta -> virgül)
        request.NormalizeRateFormats();

        // Güvenlik nesnesi oluştur
        var security = CreateSecurity();

        // PARAM API çağrısı
        var response = await _client.TP_Ozel_Oran_SK_GuncelleAsync(
            security,
            _options.Guid,
            request.Ozel_Oran_SK_ID.ToString(),
            request.MO_1,
            request.MO_2,
            request.MO_3,
            request.MO_4,
            request.MO_5,
            request.MO_6,
            request.MO_7,
            request.MO_8,
            request.MO_9,
            request.MO_10,
            request.MO_11,
            request.MO_12);

        return new TP_Ozel_Oran_SK_Guncelle_Response(response);
    }

    /// <inheritdoc />
    public async Task<TP_Mutabakat_Ozet_Response> TP_Mutabakat_OzetAsync(
        TP_Mutabakat_Ozet_Request request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Request validation
        if (!request.IsValid(out var validationResults))
        {
            var errors = string.Join(", ", validationResults.Select(vr => vr.ErrorMessage));
            throw new ArgumentException($"Request validation failed: {errors}", nameof(request));
        }

        // GUID kontrolü - options'dan al
        if (string.IsNullOrWhiteSpace(_options.Guid))
        {
            throw new ArgumentException("GUID bilgisi ParamApiOptions'da tanımlanmalıdır.", nameof(_options.Guid));
        }

        // Güvenlik nesnesi oluştur
        var security = CreateSecurity();

        // PARAM API çağrısı
        var apiResponse = await _client.TP_Mutabakat_OzetAsync(
            security,
            _options.Guid,
            request.Tarih_Bas,
            request.Tarih_Bit);

        // Direkt response oluştur - ST_Genel_Sonuc kullanmak yerine değerleri tek tek al
        var response = new TP_Mutabakat_Ozet_Response();

        if (apiResponse != null)
        {
            response.Sonuc = apiResponse.Sonuc;
            response.Sonuc_Str = apiResponse.Sonuc_Str;

            // DT_Bilgi DataSet'ini parse et
            if (apiResponse.DT_Bilgi?.Any != null)
            {
                response.ParseDataSet(apiResponse.DT_Bilgi.Any);
            }
        }

        return response;
    }

    /// <summary>
    /// Mutabakat detay bilgilerini getirir
    /// </summary>
    public async Task<TP_Mutabakat_Detay_Response> TP_Mutabakat_DetayAsync(TP_Mutabakat_Detay_Request request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Validation
        if (!request.IsValid(out var validationResults))
        {
            var errors = string.Join(", ", validationResults.Select(vr => vr.ErrorMessage));
            throw new ArgumentException($"Request validation failed: {errors}", nameof(request));
        }

        // GUID kontrolü - options'dan al
        if (string.IsNullOrWhiteSpace(_options.Guid))
        {
            throw new ArgumentException("GUID bilgisi ParamApiOptions'da tanımlanmalıdır.", nameof(_options.Guid));
        }

        // Güvenlik nesnesi oluştur
        var security = CreateSecurity();

        // PARAM API çağrısı
        var result = await _client.TP_Mutabakat_DetayAsync(
            security,
            _options.Guid,
            request.Tarih
        ).ConfigureAwait(false);

        // Response dönüştür
        var response = new TP_Mutabakat_Detay_Response
        {
            Sonuc = result.Sonuc ?? string.Empty,
            Sonuc_Str = result.Sonuc_Str ?? string.Empty
        };

        // DataSet parsing - DT_Bilgi XML elementlerini parse et
        if (result.DT_Bilgi?.Any?.Length > 0)
        {
            response.ParseDataSet(result.DT_Bilgi.Any);
        }

        return response;
    }

    /// <summary>
    /// İşlem durumu sorgulama
    /// İşlemin başarılı, başarısız, iptal veya iade durumunda olduğunu öğrenmek için kullanılır
    /// </summary>
    public async Task<TP_Islem_Sorgulama4_Response> TP_Islem_Sorgulama4Async(TP_Islem_Sorgulama4_Request request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Validation
        if (!request.IsValid(out var validationResults))
        {
            var errors = string.Join(", ", validationResults.Select(vr => vr.ErrorMessage));
            throw new ArgumentException($"Request validation failed: {errors}", nameof(request));
        }

        // GUID kontrolü - options'dan al
        if (string.IsNullOrWhiteSpace(_options.Guid))
        {
            throw new ArgumentException("GUID bilgisi ParamApiOptions'da tanımlanmalıdır.", nameof(_options.Guid));
        }

        // Güvenlik nesnesi oluştur
        var security = CreateSecurity();

        // PARAM API çağrısı
        var result = await _client.TP_Islem_Sorgulama4Async(
            security,
            _options.Guid,
            request.Dekont_ID ?? string.Empty,
            request.Siparis_ID ?? string.Empty,
            request.Islem_ID ?? string.Empty
        ).ConfigureAwait(false);

        // Response dönüştür
        var response = new TP_Islem_Sorgulama4_Response
        {
            Sonuc = int.TryParse(result.Sonuc, out var sonucInt) ? sonucInt : 0,
            Sonuc_Str = result.Sonuc_Str ?? string.Empty
        };
        
        if (result.DT_Bilgi != null)
        {
            var islemBilgi = new DT_IslemSorgulama4Bilgi
            {
                Odeme_Sonuc = result.DT_Bilgi.Odeme_Sonuc.ToString(),
                Odeme_Sonuc_Aciklama = result.DT_Bilgi.Odeme_Sonuc_Aciklama,
                Tarih = result.DT_Bilgi.Tarih,
                Dekont_ID = result.DT_Bilgi.Dekont_ID,
                Siparis_ID = result.DT_Bilgi.Siparis_ID,
                Islem_ID = result.DT_Bilgi.Islem_ID,
                Komisyon_Oran = result.DT_Bilgi.Komisyon_Oran.ToString(),
                Komisyon_Tutar = result.DT_Bilgi.Komisyon_Tutar.ToString(),
                Toplam_Tutar = result.DT_Bilgi.Toplam_Tutar.ToString(),
                Banka_Sonuc_Aciklama = result.DT_Bilgi.Banka_Sonuc_Aciklama,
                Taksit = result.DT_Bilgi.Taksit,
                Ext_Data = result.DT_Bilgi.Ext_Data,
                Toplam_Iade_Tutar = result.DT_Bilgi.Toplam_Iade_Tutar.ToString(),
                KK_No = result.DT_Bilgi.KK_No,
                Durum = result.DT_Bilgi.Durum
            };

            response.DT_Bilgi.Add(islemBilgi);
        }

        return response;
    }

    /// <summary>
    /// İşlem izleme
    /// Yapılan işlemlerin belirli tarih aralığında izlenmesi için kullanılır
    /// </summary>
    public async Task<TP_Islem_Izleme_Response> TP_Islem_IzlemeAsync(TP_Islem_Izleme_Request request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Validation
        if (!request.IsValid(out var validationResults))
        {
            var errors = string.Join(", ", validationResults.Select(vr => vr.ErrorMessage));
            throw new ArgumentException($"Request validation failed: {errors}", nameof(request));
        }

        // GUID kontrolü - options'dan al
        if (string.IsNullOrWhiteSpace(_options.Guid))
        {
            throw new ArgumentException("GUID bilgisi ParamApiOptions'da tanımlanmalıdır.", nameof(_options.Guid));
        }

        // Güvenlik nesnesi oluştur
        var security = CreateSecurity();

        // PARAM API çağrısı
        var result = await _client.TP_Islem_IzlemeAsync(
            security,
            _options.Guid,
            request.Tarih_Bas,
            request.Tarih_Bit,
            request.Islem_Tip,
            request.Islem_Durum
        ).ConfigureAwait(false);

        // Response dönüştür
        var response = new TP_Islem_Izleme_Response
        {
            Sonuc = int.TryParse(result.Sonuc, out var sonuc) ? sonuc : 0,
            Sonuc_Str = result.Sonuc_Str ?? string.Empty
        };

        // DataSet'i parse et
        if (result.DT_Bilgi?.Any != null)
        {
            response.ParseDataSet(result.DT_Bilgi.Any);
        }

        return response;
    }

    /// <summary>
    /// Dekont e-posta gönderimi
    /// Dekontun e-posta olarak gönderilmesini sağlar
    /// </summary>
    public async Task<TP_Islem_Dekont_Gonder_Response> TP_Islem_Dekont_GonderAsync(TP_Islem_Dekont_Gonder_Request request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Validation
        if (!request.IsValid(out var validationResults))
        {
            var errors = string.Join(", ", validationResults.Select(vr => vr.ErrorMessage));
            throw new ArgumentException($"Request validation failed: {errors}", nameof(request));
        }

        // GUID kontrolü - options'dan al
        if (string.IsNullOrWhiteSpace(_options.Guid))
        {
            throw new ArgumentException("GUID bilgisi ParamApiOptions'da tanımlanmalıdır.", nameof(_options.Guid));
        }

        // Güvenlik nesnesi oluştur
        var security = CreateSecurity();

        // PARAM API çağrısı
        var result = await _client.TP_Islem_Dekont_GonderAsync(
            security,
            _options.Guid,
            request.Dekont_ID,
            request.E_Posta ?? string.Empty
        ).ConfigureAwait(false);

        // Response dönüştür
        var response = new TP_Islem_Dekont_Gonder_Response
        {
            Sonuc = int.TryParse(result.Sonuc, out var sonuc) ? sonuc : 0,
            Sonuc_Str = result.Sonuc_Str ?? string.Empty
        };

        return response;
    }

    /// <summary>
    /// BIN sorgulama
    /// Kredi kartına ait kart-banka bilgisini ve SanalPOS_ID değerini döner
    /// </summary>
    public async Task<BIN_SanalPos_Response> BIN_SanalPosAsync(BIN_SanalPos_Request request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Validation
        if (!request.IsValid(out var validationResults))
        {
            var errors = string.Join(", ", validationResults.Select(vr => vr.ErrorMessage));
            throw new ArgumentException($"Request validation failed: {errors}", nameof(request));
        }

        // Güvenlik nesnesi oluştur
        var security = CreateSecurity();

        // PARAM API çağrısı
        var result = await _client.BIN_SanalPosAsync(
            security,
            request.BIN ?? string.Empty
        ).ConfigureAwait(false);

        // Response dönüştür
        var response = new BIN_SanalPos_Response
        {
            Sonuc = int.TryParse(result.Sonuc, out var sonuc) ? sonuc : 0,
            Sonuc_Str = result.Sonuc_Str ?? string.Empty
        };

        // DataSet'i parse et
        if (result.DT_Bilgi?.Any != null)
        {
            response.ParseDataSet(result.DT_Bilgi.Any);
        }

        return response;
    }

    /// <summary>
    /// TP_Islem_Odeme_OnProv_WKS - Saklı kart ile ön provizyon işlemi
    /// Daha önce kaydedilen kart ile ön provizyon işlemi yapmak için kullanılır
    /// </summary>
    public async Task<TP_Islem_Odeme_OnProv_WKS_Response> TP_Islem_Odeme_OnProv_WKSAsync(
        TP_Islem_Odeme_OnProv_WKS_Request request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Request validation
        if (!request.IsValid(out var validationResults))
        {
            var errors = string.Join(", ", validationResults.Select(vr => vr.ErrorMessage));
            throw new ArgumentException($"Request validation failed: {errors}", nameof(request));
        }

        // GUID kontrolü - options'dan al
        if (string.IsNullOrWhiteSpace(_options.Guid))
        {
            throw new ArgumentException("GUID bilgisi ParamApiOptions'da tanımlanmalıdır.", nameof(_options.Guid));
        }

        // Güvenlik nesnesi oluştur
        var security = CreateSecurity();

        // Tutarları PARAM API formatına çevir (virgüllü format)
        var formattedIslemTutar = request.GetFormattedIslemTutar();
        var formattedToplamTutar = request.GetFormattedToplamTutar();

        // Hash hesapla - TP_Islem_Odeme_OnProv_WKS hash formülü
        var islemHash = await HashHelper.CalculateTP_Islem_Odeme_OnProv_WKS_HashAsync(
            _client,
            _options.ClientCode,
            _options.Guid,
            formattedIslemTutar,
            formattedToplamTutar,
            request.Siparis_ID,
            request.Hata_URL,
            request.Basarili_URL,
            cancellationToken);

        // PARAM API çağrısı
        var result = await _client.TP_Islem_Odeme_OnProv_WKSAsync(
            security,
            _options.Guid,
            request.KK_GUID,
            request.KS_Kart_No,
            request.KK_Sahibi_GSM,
            request.Hata_URL,
            request.Basarili_URL,
            request.Siparis_ID,
            request.Siparis_Aciklama,
            formattedIslemTutar,
            formattedToplamTutar,
            islemHash,
            request.Islem_Guvenlik_Tip,
            request.Islem_ID ?? string.Empty,
            request.IPAdr,
            request.Ref_URL ?? string.Empty,
            request.Data1 ?? string.Empty,
            request.Data2 ?? string.Empty,
            request.Data3 ?? string.Empty,
            request.Data4 ?? string.Empty,
            request.Data5 ?? string.Empty
        ).ConfigureAwait(false);

        // Response dönüştür
        var response = new TP_Islem_Odeme_OnProv_WKS_Response(result);

        return response;
    }

    /// <inheritdoc />
    public async Task<TP_KK_Verify_Response> TP_KK_VerifyAsync(
        TP_KK_Verify_Request request,
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

        try
        {
            // PARAM TurkPos API çağrısı - Kart doğrulama
            var result = await _client.TP_KK_VerifyAsync(
                security,
                request.KK_No,
                request.KK_SK_Ay,
                request.KK_SK_Yil,
                request.KK_CVC,
                request.Return_URL ?? string.Empty,
                request.Data1 ?? string.Empty,
                request.Data2 ?? string.Empty,
                request.Data3 ?? string.Empty,
                request.Data4 ?? string.Empty,
                request.Data5 ?? string.Empty
            ).ConfigureAwait(false);

            // Response dönüştür - PARAM API mixed types manual conversion
            var response = new TP_KK_Verify_Response
            {
                Islem_ID = result.Islem_ID,
                UCD_URL = result.UCD_URL ?? string.Empty,
                Sonuc = int.TryParse(result.Sonuc, out var sonuc) ? sonuc : 0,
                Sonuc_Str = result.Sonuc_Str ?? string.Empty,
                Banka_Sonuc_Kod = result.Banka_Sonuc_Kod
            };

            return response;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"TP_KK_Verify işlemi sırasında hata oluştu: {ex.Message}", ex);
        }
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