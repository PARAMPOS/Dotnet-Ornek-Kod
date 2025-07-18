namespace ParamApi.Sdk.Configuration;

/// <summary>
/// İptal/İade işlemlerinde kullanılan durum türleri
/// </summary>
public enum IptalIadeDurum
{
    /// <summary>
    /// İptal işlemi - Ödeme işleminin gerçekleştiği gün yapılır
    /// </summary>
    IPTAL,
    
    /// <summary>
    /// İade işlemi - Ödeme işlemi gün sonuna girdikten sonraki günlerde yapılır
    /// </summary>
    IADE
}

/// <summary>
/// IptalIadeDurum enum'ı için extension metodları
/// </summary>
public static class IptalIadeDurumExtensions
{
    /// <summary>
    /// Enum değerini PARAM API'nin beklediği string formatına çevirir
    /// </summary>
    /// <param name="durum">İptal/İade durumu</param>
    /// <returns>PARAM API string değeri</returns>
    public static string ToParamString(this IptalIadeDurum durum)
    {
        return durum switch
        {
            IptalIadeDurum.IPTAL => "IPTAL",
            IptalIadeDurum.IADE => "IADE",
            _ => throw new ArgumentOutOfRangeException(nameof(durum), durum, "Geçersiz IptalIadeDurum değeri")
        };
    }
    
    /// <summary>
    /// PARAM API string değerini enum'a çevirir
    /// </summary>
    /// <param name="durumStr">PARAM API string değeri</param>
    /// <returns>IptalIadeDurum enum değeri</returns>
    public static IptalIadeDurum FromParamString(string durumStr)
    {
        return durumStr?.ToUpperInvariant() switch
        {
            "IPTAL" => IptalIadeDurum.IPTAL,
            "IADE" => IptalIadeDurum.IADE,
            _ => throw new ArgumentException($"Geçersiz durum değeri: {durumStr}", nameof(durumStr))
        };
    }
} 