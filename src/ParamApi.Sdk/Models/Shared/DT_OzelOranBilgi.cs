namespace ParamApi.Sdk.Models.Shared;

/// <summary>
/// Özel oran bilgi modeli
/// Firma tarafından üye işyerine özel verilmiş sanal pos oranları
/// </summary>
public class DT_OzelOranBilgi
{
    /// <summary>
    /// Özel Oran ID si
    /// </summary>
    public long Ozel_Oran_ID { get; set; }

    /// <summary>
    /// Üye İşyerine ait Anahtar
    /// </summary>
    public string? GUID { get; set; }

    /// <summary>
    /// Özel Oranların aktif olacağı tarih (Örn. dd.MM.yyyy)
    /// </summary>
    public string? Tarih_Bas { get; set; }

    /// <summary>
    /// Özel Oranların pasif olacağı tarih (Örn. dd.MM.yyyy)
    /// </summary>
    public string? Tarih_Bit { get; set; }

    /// <summary>
    /// Sanal Pos numarası
    /// </summary>
    public int SanalPOS_ID { get; set; }

    /// <summary>
    /// Kredi Kartı Markası (Bonus, Axess vs)
    /// </summary>
    public string? Kredi_Karti_Banka { get; set; }

    /// <summary>
    /// Kredi Kartı Marka Görseli
    /// </summary>
    public string? Kredi_Karti_Banka_Gorsel { get; set; }

    /// <summary>
    /// Tek Çekim Oranı
    /// </summary>
    public string? MO_01 { get; set; }

    /// <summary>
    /// 2. Taksit Oranı
    /// Not: -1 veya -2 ise bu taksit seçeneği kullanılamaz
    /// </summary>
    public string? MO_02 { get; set; }

    /// <summary>
    /// 3. Taksit Oranı
    /// Not: -1 veya -2 ise bu taksit seçeneği kullanılamaz
    /// </summary>
    public string? MO_03 { get; set; }

    /// <summary>
    /// 4. Taksit Oranı
    /// Not: -1 veya -2 ise bu taksit seçeneği kullanılamaz
    /// </summary>
    public string? MO_04 { get; set; }

    /// <summary>
    /// 5. Taksit Oranı
    /// Not: -1 veya -2 ise bu taksit seçeneği kullanılamaz
    /// </summary>
    public string? MO_05 { get; set; }

    /// <summary>
    /// 6. Taksit Oranı
    /// Not: -1 veya -2 ise bu taksit seçeneği kullanılamaz
    /// </summary>
    public string? MO_06 { get; set; }

    /// <summary>
    /// 7. Taksit Oranı
    /// Not: -1 veya -2 ise bu taksit seçeneği kullanılamaz
    /// </summary>
    public string? MO_07 { get; set; }

    /// <summary>
    /// 8. Taksit Oranı
    /// Not: -1 veya -2 ise bu taksit seçeneği kullanılamaz
    /// </summary>
    public string? MO_08 { get; set; }

    /// <summary>
    /// 9. Taksit Oranı
    /// Not: -1 veya -2 ise bu taksit seçeneği kullanılamaz
    /// </summary>
    public string? MO_09 { get; set; }

    /// <summary>
    /// 10. Taksit Oranı
    /// Not: -1 veya -2 ise bu taksit seçeneği kullanılamaz
    /// </summary>
    public string? MO_10 { get; set; }

    /// <summary>
    /// 11. Taksit Oranı
    /// Not: -1 veya -2 ise bu taksit seçeneği kullanılamaz
    /// </summary>
    public string? MO_11 { get; set; }

    /// <summary>
    /// 12. Taksit Oranı
    /// Not: -1 veya -2 ise bu taksit seçeneği kullanılamaz
    /// </summary>
    public string? MO_12 { get; set; }

    /// <summary>
    /// Belirtilen taksit sayısının bu kart markası için kullanılabilir olup olmadığını kontrol eder
    /// </summary>
    /// <param name="taksitSayisi">Kontrol edilecek taksit sayısı (1-12)</param>
    /// <returns>Taksit kullanılabilirse true, değilse false</returns>
    public bool IsTaksitAvailable(int taksitSayisi)
    {
        var oran = taksitSayisi switch
        {
            1 => MO_01,
            2 => MO_02,
            3 => MO_03,
            4 => MO_04,
            5 => MO_05,
            6 => MO_06,
            7 => MO_07,
            8 => MO_08,
            9 => MO_09,
            10 => MO_10,
            11 => MO_11,
            12 => MO_12,
            _ => null
        };

        // Oran null, boş veya negatif değer (-1, -2) ise kullanılamaz
        if (string.IsNullOrWhiteSpace(oran) || 
            (decimal.TryParse(oran, out var oranDecimal) && oranDecimal < 0))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Belirtilen taksit sayısı için oranı döner
    /// </summary>
    /// <param name="taksitSayisi">Taksit sayısı (1-12)</param>
    /// <returns>Oran string değeri veya null</returns>
    public string? GetTaksitOrani(int taksitSayisi)
    {
        return taksitSayisi switch
        {
            1 => MO_01,
            2 => MO_02,
            3 => MO_03,
            4 => MO_04,
            5 => MO_05,
            6 => MO_06,
            7 => MO_07,
            8 => MO_08,
            9 => MO_09,
            10 => MO_10,
            11 => MO_11,
            12 => MO_12,
            _ => null
        };
    }
} 