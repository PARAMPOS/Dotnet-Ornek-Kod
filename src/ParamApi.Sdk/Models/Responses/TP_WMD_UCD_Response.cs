using TurkposService;

namespace ParamApi.Sdk.Models.Responses;

/// <summary>
/// TP_WMD_UCD response wrapper sınıfı
/// Connected Services ST_WMD_UCD modelini sarmalayarak kullanıcı dostu interface sağlar
/// </summary>
public class TP_WMD_UCD_Response
{
    private readonly ST_WMD_UCD _originalResponse;

    /// <summary>
    /// Constructor - Connected Services response'unu wrap eder
    /// </summary>
    /// <param name="originalResponse">ST_WMD_UCD Connected Services response</param>
    public TP_WMD_UCD_Response(ST_WMD_UCD originalResponse)
    {
        _originalResponse = originalResponse ?? throw new ArgumentNullException(nameof(originalResponse));
    }

    /// <summary>
    /// Orijinal PARAM API response objesine erişim
    /// </summary>
    public ST_WMD_UCD OriginalResponse => _originalResponse;

    /// <summary>
    /// SID Değeri - İşlem ID
    /// </summary>
    public long Islem_ID => _originalResponse.Islem_ID;

    /// <summary>
    /// İşlem GUID değeri
    /// </summary>
    public string? Islem_GUID => _originalResponse.Islem_GUID;

    /// <summary>
    /// 3D HTML kod içeriği
    /// NonSecure işlemlerde "NONSECURE" değeri döner
    /// </summary>
    public string? UCD_HTML => _originalResponse.UCD_HTML;

    /// <summary>
    /// 3D MD değeri (3D işlemlerde kullanılır)
    /// </summary>
    public string? UCD_MD => _originalResponse.UCD_MD;

    /// <summary>
    /// İşlem sonucu (0'dan büyükse başarılı)
    /// </summary>
    public string? Sonuc => _originalResponse.Sonuc;

    /// <summary>
    /// İşlem sonucu açıklama metni
    /// </summary>
    public string? Sonuc_Str => _originalResponse.Sonuc_Str;

    /// <summary>
    /// Banka Transaction ID değeri
    /// </summary>
    public string? Bank_Trans_ID => _originalResponse.Bank_Trans_ID;

    /// <summary>
    /// Banka Auth Code değeri
    /// </summary>
    public string? Bank_AuthCode => _originalResponse.Bank_AuthCode;

    /// <summary>
    /// Banka HostMsg değeri
    /// </summary>
    public string? Bank_HostMsg => _originalResponse.Bank_HostMsg;

    /// <summary>
    /// Bankanın döndüğü sonuç kodu
    /// </summary>
    public int Banka_Sonuc_Kod => _originalResponse.Banka_Sonuc_Kod;

    /// <summary>
    /// Banka Extra değeri (XML içerebilir)
    /// </summary>
    public string? Bank_Extra => _originalResponse.Bank_Extra;

    /// <summary>
    /// İşlemin başarılı olup olmadığını kontrol eder
    /// </summary>
    public bool IsSuccessful
    {
        get
        {
            // Sonuc değeri parse edilebilir ve 0'dan büyük ise başarılı
            return int.TryParse(Sonuc, out var sonucInt) && sonucInt > 0;
        }
    }

    /// <summary>
    /// NonSecure işlem olup olmadığını kontrol eder
    /// </summary>
    public bool IsNonSecure => UCD_HTML == "NONSECURE";

    /// <summary>
    /// 3D işlem olup olmadığını kontrol eder
    /// </summary>
    public bool Is3D => IsSuccessful && !IsNonSecure && !string.IsNullOrEmpty(UCD_HTML);

    /// <summary>
    /// NonSecure işleminin başarılı olup olmadığını kontrol eder
    /// Başarılı NonSecure: Sonuc > 0, Islem_ID > 0 ve UCD_HTML='NONSECURE'
    /// </summary>
    public bool IsNonSecureSuccessful
    {
        get
        {
            return IsSuccessful && Islem_ID > 0 && IsNonSecure;
        }
    }

    /// <summary>
    /// 3D işleminin başarılı başlatılıp başlatılmadığını kontrol eder
    /// </summary>
    public bool Is3DInitiated
    {
        get
        {
            return IsSuccessful && Is3D && !string.IsNullOrEmpty(Islem_GUID);
        }
    }

    /// <summary>
    /// İşlem hatalı ise hata mesajını döner
    /// </summary>
    public string? GetErrorMessage()
    {
        if (IsSuccessful)
            return null;

        return !string.IsNullOrEmpty(Sonuc_Str) 
            ? Sonuc_Str 
            : "Bilinmeyen hata oluştu";
    }

    /// <summary>
    /// 3D işlemi için gerekli HTML içeriğini döner
    /// Sadece 3D işlemlerde kullanılır
    /// </summary>
    public string? Get3DRedirectHtml()
    {
        return Is3D ? UCD_HTML : null;
    }

    /// <summary>
    /// TP_WMD_Pay metodunda kullanılacak MD değerini döner
    /// </summary>
    public string? GetMDForPayment()
    {
        return Is3D ? UCD_MD : null;
    }

    /// <summary>
    /// İşlem özet bilgilerini string olarak döner
    /// </summary>
    public override string ToString()
    {
        var status = IsSuccessful ? "Başarılı" : "Başarısız";
        var type = IsNonSecure ? "NonSecure" : Is3D ? "3D" : "Bilinmiyor";
        
        return $"TP_WMD_UCD Response - Durum: {status}, Tip: {type}, İşlem ID: {Islem_ID}, Sonuç: {Sonuc_Str}";
    }

    /// <summary>
    /// Detaylı işlem bilgilerini döner
    /// </summary>
    public Dictionary<string, object?> GetDetails()
    {
        return new Dictionary<string, object?>
        {
            { "Islem_ID", Islem_ID },
            { "Islem_GUID", Islem_GUID },
            { "Sonuc", Sonuc },
            { "Sonuc_Str", Sonuc_Str },
            { "IsSuccessful", IsSuccessful },
            { "IsNonSecure", IsNonSecure },
            { "Is3D", Is3D },
            { "Banka_Sonuc_Kod", Banka_Sonuc_Kod },
            { "Bank_Trans_ID", Bank_Trans_ID },
            { "Bank_AuthCode", Bank_AuthCode }
        };
    }
} 