using TurkposService;

namespace ParamApi.Sdk.Models.Responses;

/// <summary>
/// TP_WMD_Pay Response Wrapper - Developer Friendly
/// Uses WSDL-generated ST_WMD_Pay class
/// </summary>
public class TP_WMD_PAY_Response
{
    private readonly ST_WMD_Pay _result;

    public TP_WMD_PAY_Response(ST_WMD_Pay result)
    {
        _result = result ?? throw new ArgumentNullException(nameof(result));
    }

    /// <summary>
    /// Ham PARAM API response
    /// </summary>
    public ST_WMD_Pay Raw => _result;

    /// <summary>
    /// İşlem sonucu (büyük 0 başarı, küçük eşit 0 hata)
    /// </summary>
    public int Sonuc => _result.Sonuc;

    /// <summary>
    /// İşlem sonuç açıklaması
    /// </summary>
    public string Sonuc_Ack => _result.Sonuc_Ack ?? string.Empty;

    /// <summary>
    /// Dekont ID
    /// </summary>
    public string Dekont_ID => _result.Dekont_ID ?? string.Empty;

    /// <summary>
    /// Sipariş ID
    /// </summary>
    public string Siparis_ID => _result.Siparis_ID ?? string.Empty;

    /// <summary>
    /// 3D MD değeri
    /// </summary>
    public string UCD_MD => _result.UCD_MD ?? string.Empty;

    /// <summary>
    /// Banka Transaction ID
    /// </summary>
    public string Bank_Trans_ID => _result.Bank_Trans_ID ?? string.Empty;

    /// <summary>
    /// Banka Authorization Code
    /// </summary>
    public string Bank_AuthCode => _result.Bank_AuthCode ?? string.Empty;

    /// <summary>
    /// Banka Host Message
    /// </summary>
    public string Bank_HostMsg => _result.Bank_HostMsg ?? string.Empty;

    /// <summary>
    /// Banka Extra bilgileri
    /// </summary>
    public string Bank_Extra => _result.Bank_Extra ?? string.Empty;

    /// <summary>
    /// Banka sonuç kodu
    /// </summary>
    public int Bank_Sonuc_Kod => _result.Bank_Sonuc_Kod;

    /// <summary>
    /// Banka RRN değeri
    /// </summary>
    public string Bank_HostRefNum => _result.Bank_HostRefNum ?? string.Empty;

    /// <summary>
    /// Komisyon oranı
    /// </summary>
    public decimal Komisyon_Oran => (decimal)_result.Komisyon_Oran;

    /// <summary>
    /// İşlem başarılı mı? (Sonuc büyük 0 ve Dekont_ID büyük 0)
    /// </summary>
    public bool IsSuccessful => Sonuc > 0 && !string.IsNullOrEmpty(Dekont_ID) && Dekont_ID != "0";

    /// <summary>
    /// Hata mesajı (başarısız işlemlerde)
    /// </summary>
    public string ErrorMessage => IsSuccessful ? string.Empty : Sonuc_Ack;
} 