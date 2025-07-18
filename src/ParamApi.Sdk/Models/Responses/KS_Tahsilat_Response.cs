namespace ParamApi.Sdk.Models.Responses;

/// <summary>
/// Saklanmış kredi kartından tahsilat yanıtı
/// Ödeme işleminin sonucunu ve 3D/NonSecure bilgilerini içerir
/// </summary>
public class KS_Tahsilat_Response
{
    /// <summary>
    /// İşlem sonucu (1: Başarılı, 0: Başarısız)
    /// </summary>
    public int Sonuc { get; set; }

    /// <summary>
    /// İşlem sonuç açıklama
    /// Sonuc > 0 ise İşlem Başarılı, aksi halde başarısız
    /// </summary>
    public string Sonuc_Str { get; set; } = string.Empty;

    /// <summary>
    /// URL bilgisi
    /// NonSecure işlemler için "NONSECURE" döner
    /// 3D işlemler için 3D URL döner
    /// </summary>
    public string UCD_URL { get; set; } = string.Empty;

    /// <summary>
    /// 3D URL İşlem ID bilgisi
    /// NonSecure işlemler için Dekont_ID değeri döner
    /// </summary>
    public long Islem_ID { get; set; }

    /// <summary>
    /// İşlemin NonSecure olup olmadığını kontrol eder
    /// </summary>
    public bool IsNonSecure => UCD_URL?.Equals("NONSECURE", StringComparison.OrdinalIgnoreCase) == true;

    /// <summary>
    /// NonSecure işlem için başarı kontrolü yapar
    /// Sonuc > 0, Islem_ID > 0 ve UCD_URL='NONSECURE' olmalıdır
    /// </summary>
    public bool IsNonSecureSuccessful => IsNonSecure && Sonuc > 0 && Islem_ID > 0;

    /// <summary>
    /// 3D işlem olup olmadığını kontrol eder
    /// </summary>
    public bool Is3DTransaction => !IsNonSecure && !string.IsNullOrEmpty(UCD_URL);

    /// <summary>
    /// Genel başarı durumunu kontrol eder
    /// </summary>
    public bool IsSuccessful => Sonuc > 0;
} 