using TurkposService;

namespace ParamApi.Sdk.Models.Responses;

/// <summary>
/// Kısmi iptal/iade işlemi response modeli
/// </summary>
public class TP_Islem_Iptal_Iade_Kismi2_Response
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
    /// Bankanın döndüğü kod
    /// </summary>
    public string? Banka_Sonuc_Kod { get; }

    /// <summary>
    /// Banka Auth Code değeri
    /// </summary>
    public string? Bank_AuthCode { get; }

    /// <summary>
    /// Banka Transaction ID değeri
    /// </summary>
    public string? Bank_Trans_ID { get; }

    /// <summary>
    /// Banka Extra değeri
    /// </summary>
    public string? Bank_Extra { get; }

    /// <summary>
    /// Banka HostRefNum değeri
    /// </summary>
    public string? Bank_HostRefNum { get; }

    /// <summary>
    /// İşlemin başarılı olup olmadığını döner
    /// </summary>
    public bool IsSuccess => 
        !string.IsNullOrEmpty(Sonuc) && 
        int.TryParse(Sonuc, out var sonucInt) && 
        sonucInt > 0;

    /// <summary>
    /// İşlemin hata mesajını döner (başarısızsa)
    /// </summary>
    public string? ErrorMessage => IsSuccess ? null : Sonuc_Str;

    /// <summary>
    /// PARAM API response'undan model oluşturur
    /// </summary>
    /// <param name="paramResponse">PARAM API ST_Sonuc_II response'u</param>
    internal TP_Islem_Iptal_Iade_Kismi2_Response(ST_Sonuc_II paramResponse)
    {
        Sonuc = paramResponse?.Sonuc;
        Sonuc_Str = paramResponse?.Sonuc_Str;
        Banka_Sonuc_Kod = paramResponse?.Banka_Sonuc_Kod;
        Bank_AuthCode = paramResponse?.Bank_AuthCode;
        Bank_Trans_ID = paramResponse?.Bank_Trans_ID;
        Bank_Extra = paramResponse?.Bank_Extra;
        Bank_HostRefNum = paramResponse?.Bank_HostRefNum;
    }

    /// <summary>
    /// Response bilgilerini string formatında döner
    /// </summary>
    /// <returns>Response özeti</returns>
    public override string ToString()
    {
        if (IsSuccess)
        {
            return $"✅ İptal/İade Başarılı - {Sonuc_Str} (Auth: {Bank_AuthCode}, TransID: {Bank_Trans_ID})";
        }
        
        return $"❌ İptal/İade Hatası - {ErrorMessage}";
    }
} 