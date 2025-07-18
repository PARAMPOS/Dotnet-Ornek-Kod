using System.Xml.Serialization;
using TurkposService;

namespace ParamApi.Sdk.Models.Responses;

/// <summary>
/// TP_Islem_Odeme_OnProv_WMD Response Wrapper
/// Hem NS hem de 3D işlemleri için kullanılır
/// WSDL'den gelen ST_WMD_UCD tipini kullanır
/// </summary>
public class TP_Islem_Odeme_OnProv_WMD_Response
{
    private readonly ST_WMD_UCD _result;

    internal TP_Islem_Odeme_OnProv_WMD_Response(ST_WMD_UCD result)
    {
        _result = result ?? throw new ArgumentNullException(nameof(result));
    }

    /// <summary>
    /// Ham PARAM API response
    /// </summary>
    public ST_WMD_UCD Raw => _result;

    /// <summary>
    /// İşlem sonucu (string formatında, "1" başarı, "0" veya negatif hata)
    /// </summary>
    public string? Sonuc => _result.Sonuc;

    /// <summary>
    /// İşlem sonuç açıklaması
    /// </summary>
    public string? Sonuc_Str => _result.Sonuc_Str;

    /// <summary>
    /// İşlem ID (SID değeri)
    /// </summary>
    public long Islem_ID => _result.Islem_ID;

    /// <summary>
    /// İşlem GUID değeri
    /// </summary>
    public string? Islem_GUID => _result.Islem_GUID;

    /// <summary>
    /// 3D HTML kod veya "NONSECURE" değeri
    /// NS işlemler için "NONSECURE", 3D işlemler için HTML içeriği
    /// </summary>
    public string? UCD_HTML => _result.UCD_HTML;

    /// <summary>
    /// 3D MD değeri (sadece 3D işlemler için)
    /// </summary>
    public string? UCD_MD => _result.UCD_MD;

    /// <summary>
    /// Banka Transaction ID
    /// </summary>
    public string? Bank_Trans_ID => _result.Bank_Trans_ID;

    /// <summary>
    /// Banka Auth Code
    /// </summary>
    public string? Bank_AuthCode => _result.Bank_AuthCode;

    /// <summary>
    /// Banka Host Message
    /// </summary>
    public string? Bank_HostMsg => _result.Bank_HostMsg;

    /// <summary>
    /// Banka sonuç kodu
    /// </summary>
    public int Banka_Sonuc_Kod => _result.Banka_Sonuc_Kod;

    /// <summary>
    /// Banka extra bilgileri
    /// </summary>
    public string? Bank_Extra => _result.Bank_Extra;

    /// <summary>
    /// Sipariş ID
    /// </summary>
    public string? Siparis_ID => _result.Siparis_ID;

    /// <summary>
    /// Banka Host Reference Number (RRN)
    /// </summary>
    public string? Bank_HostRefNum => _result.Bank_HostRefNum;

    /// <summary>
    /// İşlem başarılı mı? (Genel kontrol - Sonuc string olarak "1" veya pozitif sayı)
    /// </summary>
    public bool IsSuccessful => !string.IsNullOrEmpty(Sonuc) && (Sonuc == "1" || (int.TryParse(Sonuc, out var sonucInt) && sonucInt > 0));

    /// <summary>
    /// Sonuc değerini integer olarak döndürür
    /// </summary>
    public int SonucAsInt => int.TryParse(Sonuc, out var result) ? result : 0;

    /// <summary>
    /// NonSecure işlem mi?
    /// </summary>
    public bool IsNonSecure => UCD_HTML == "NONSECURE";

    /// <summary>
    /// 3D Secure işlem mi?
    /// </summary>
    public bool Is3DSecure => !string.IsNullOrEmpty(UCD_HTML) && UCD_HTML != "NONSECURE";

    /// <summary>
    /// NonSecure işlem başarılı mı?
    /// Sonuc > 0, Islem_ID > 0 ve UCD_HTML = "NONSECURE" olmalı
    /// </summary>
    public bool IsNonSecureSuccessful => IsSuccessful && Islem_ID > 0 && UCD_HTML == "NONSECURE";

    /// <summary>
    /// 3D işlem başlatıldı mı?
    /// Sonuc > 0 ve UCD_HTML != "NONSECURE" olmalı
    /// </summary>
    public bool Is3DStarted => IsSuccessful && !string.IsNullOrEmpty(UCD_HTML) && UCD_HTML != "NONSECURE";

    /// <summary>
    /// İşlem tamamlandı mı? (NS için tamamlanmış, 3D için başlatılmış)
    /// </summary>
    public bool IsCompleted => IsNonSecureSuccessful || Is3DStarted;

    /// <summary>
    /// Hata mesajı (başarısız işlemler için)
    /// </summary>
    public string? ErrorMessage => !IsSuccessful ? Sonuc_Str : null;
} 