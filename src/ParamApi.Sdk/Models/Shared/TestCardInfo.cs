namespace ParamApi.Sdk.Models.Shared;

/// <summary>
/// PARAM API test kart bilgileri
/// Test ortamında kullanılabilecek kart bilgilerini içerir
/// </summary>
public class TestCardInfo
{
    /// <summary>
    /// Banka adı
    /// </summary>
    public string Banka { get; set; } = string.Empty;

    /// <summary>
    /// Test kart numarası
    /// </summary>
    public string KartNumarasi { get; set; } = string.Empty;

    /// <summary>
    /// Son kullanma tarihi (MM/YY formatında)
    /// </summary>
    public string SonKullanmaTarihi { get; set; } = string.Empty;

    /// <summary>
    /// CVV güvenlik numarası
    /// </summary>
    public string CVV { get; set; } = string.Empty;

    /// <summary>
    /// Ticari kart olup olmadığı
    /// </summary>
    public bool TicariKart { get; set; }

    /// <summary>
    /// Kart tipi (Kredi Kartı/Debit)
    /// </summary>
    public string KartTipi { get; set; } = string.Empty;

    /// <summary>
    /// Kart markası (VISA/MASTERCARD/TROY)
    /// </summary>
    public string KartMarkasi { get; set; } = string.Empty;

    /// <summary>
    /// 3D Secure şifresi (varsa)
    /// </summary>
    public string? DS3_Sifresi { get; set; }

    /// <summary>
    /// Kart açıklaması (özel notlar)
    /// </summary>
    public string? Aciklama { get; set; }

    /// <summary>
    /// Ay ve yıl bilgisini ayrı ayrı döner
    /// </summary>
    public (string Ay, string Yil) GetExpireDateParts()
    {
        if (string.IsNullOrEmpty(SonKullanmaTarihi) || !SonKullanmaTarihi.Contains('/'))
            return ("12", "2026"); // Default değer

        var parts = SonKullanmaTarihi.Split('/');
        if (parts.Length == 2)
        {
            var ay = parts[0].PadLeft(2, '0'); // 01-12 formatında
            var yil = "20" + parts[1]; // 2026 formatında
            return (ay, yil);
        }

        return ("12", "2026");
    }

    /// <summary>
    /// Test kartının 3D Secure destekleyip desteklemediğini kontrol eder
    /// </summary>
    public bool Is3DSecureSupported => !string.IsNullOrEmpty(DS3_Sifresi);

    /// <summary>
    /// Kart tip kategorisi (Kredi/Debit)
    /// </summary>
    public string KartKategorisi => KartTipi.Contains("Kredi") ? "Kredi" : "Debit";

    /// <summary>
    /// Yabancı kart olup olmadığını kontrol eder
    /// </summary>
    public bool YabanciKart => Banka.Equals("Yabancı Kart", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// PARAM API test kartlarının tüm listesini döner
    /// Test ortamında kullanılabilecek tüm kart bilgilerini içerir
    /// </summary>
    /// <returns>Test kart bilgileri listesi</returns>
    public static List<TestCardInfo> GetAllTestCards()
    {
        return new List<TestCardInfo>
        {
            // Ziraat Bankası
            new TestCardInfo
            {
                Banka = "Ziraat Bankası",
                KartNumarasi = "4546711234567894",
                SonKullanmaTarihi = "12/26",
                CVV = "000",
                TicariKart = false,
                KartTipi = "Kredi Kartı",
                KartMarkasi = "VISA"
            },

            // Akbank
            new TestCardInfo
            {
                Banka = "Akbank",
                KartNumarasi = "5571135571135575",
                SonKullanmaTarihi = "12/26",
                CVV = "000",
                TicariKart = false,
                KartTipi = "Kredi Kartı",
                KartMarkasi = "MASTERCARD"
            },

            // Halk Bankası - VISA
            new TestCardInfo
            {
                Banka = "Halk Bankası",
                KartNumarasi = "4531444531442283",
                SonKullanmaTarihi = "12/26",
                CVV = "001",
                TicariKart = false,
                KartTipi = "Kredi Kartı",
                KartMarkasi = "VISA"
            },

            // Halk Bankası - Debit MASTERCARD
            new TestCardInfo
            {
                Banka = "Halk Bankası",
                KartNumarasi = "5818775818772285",
                SonKullanmaTarihi = "12/26",
                CVV = "001",
                TicariKart = false,
                KartTipi = "Debit",
                KartMarkasi = "MASTERCARD"
            },

            // İş Bankası - Ticari VISA
            new TestCardInfo
            {
                Banka = "İş Bankası",
                KartNumarasi = "4508034508034509",
                SonKullanmaTarihi = "12/26",
                CVV = "000",
                TicariKart = true,
                KartTipi = "Kredi Kartı",
                KartMarkasi = "VISA"
            },

            // İş Bankası - TROY 3DS
            new TestCardInfo
            {
                Banka = "İş Bankası",
                KartNumarasi = "6501738564461396",
                SonKullanmaTarihi = "12/26",
                CVV = "000",
                TicariKart = false,
                KartTipi = "Kredi Kartı",
                KartMarkasi = "TROY",
                DS3_Sifresi = "aygünyıl",
                Aciklama = "3DS Şifre örneği: 102824"
            },

            // İş Bankası - MASTERCARD
            new TestCardInfo
            {
                Banka = "İş Bankası",
                KartNumarasi = "5406675406675403",
                SonKullanmaTarihi = "12/26",
                CVV = "000",
                TicariKart = false,
                KartTipi = "Kredi Kartı",
                KartMarkasi = "MASTERCARD"
            },

            // DenizBank
            new TestCardInfo
            {
                Banka = "DenizBank",
                KartNumarasi = "5200190009721495",
                SonKullanmaTarihi = "01/30",
                CVV = "462",
                TicariKart = false,
                KartTipi = "Kredi Kartı",
                KartMarkasi = "MASTERCARD"
            },

            // QNB Finansbank - VISA
            new TestCardInfo
            {
                Banka = "QNB Finansbank",
                KartNumarasi = "4400664070419701",
                SonKullanmaTarihi = "01/25",
                CVV = "123",
                TicariKart = false,
                KartTipi = "Kredi Kartı",
                KartMarkasi = "VISA"
            },

            // QNB Finansbank - Debit VISA (Ticari)
            new TestCardInfo
            {
                Banka = "QNB Finansbank",
                KartNumarasi = "4159560047417732",
                SonKullanmaTarihi = "08/24",
                CVV = "123",
                TicariKart = true,
                KartTipi = "Debit",
                KartMarkasi = "VISA"
            },

            // Vakıf Bank - 1
            new TestCardInfo
            {
                Banka = "Vakıf Bank",
                KartNumarasi = "5421190122090656",
                SonKullanmaTarihi = "04/28",
                CVV = "916",
                TicariKart = false,
                KartTipi = "Kredi Kartı",
                KartMarkasi = "MASTERCARD"
            },

            // Vakıf Bank - 2
            new TestCardInfo
            {
                Banka = "Vakıf Bank",
                KartNumarasi = "5421190122944522",
                SonKullanmaTarihi = "04/28",
                CVV = "466",
                TicariKart = false,
                KartTipi = "Kredi Kartı",
                KartMarkasi = "MASTERCARD"
            },

            // Vakıf Bank - 3
            new TestCardInfo
            {
                Banka = "Vakıf Bank",
                KartNumarasi = "5521010140829928",
                SonKullanmaTarihi = "12/29",
                CVV = "691",
                TicariKart = false,
                KartTipi = "Kredi Kartı",
                KartMarkasi = "MASTERCARD"
            },

            // Yapı Kredi Bankası - VISA 1
            new TestCardInfo
            {
                Banka = "Yapı Kredi Bankası",
                KartNumarasi = "4506349043174632",
                SonKullanmaTarihi = "02/29",
                CVV = "000",
                TicariKart = false,
                KartTipi = "Kredi Kartı",
                KartMarkasi = "VISA"
            },

            // Yapı Kredi Bankası - VISA 2
            new TestCardInfo
            {
                Banka = "Yapı Kredi Bankası",
                KartNumarasi = "4506344230780754",
                SonKullanmaTarihi = "10/28",
                CVV = "000",
                TicariKart = false,
                KartTipi = "Kredi Kartı",
                KartMarkasi = "VISA"
            },

            // Yapı Kredi Bankası - MASTERCARD 1
            new TestCardInfo
            {
                Banka = "Yapı Kredi Bankası",
                KartNumarasi = "4506344109967938",
                SonKullanmaTarihi = "01/29",
                CVV = "000",
                TicariKart = false,
                KartTipi = "Kredi Kartı",
                KartMarkasi = "MASTERCARD"
            },

            // Yapı Kredi Bankası - MASTERCARD 2
            new TestCardInfo
            {
                Banka = "Yapı Kredi Bankası",
                KartNumarasi = "5400611063484835",
                SonKullanmaTarihi = "05/28",
                CVV = "000",
                TicariKart = false,
                KartTipi = "Kredi Kartı",
                KartMarkasi = "MASTERCARD"
            },

            // Yapı Kredi Bankası - MASTERCARD 3
            new TestCardInfo
            {
                Banka = "Yapı Kredi Bankası",
                KartNumarasi = "5400611072814659",
                SonKullanmaTarihi = "08/28",
                CVV = "000",
                TicariKart = false,
                KartTipi = "Kredi Kartı",
                KartMarkasi = "MASTERCARD"
            },

            // Yapı Kredi Bankası - MASTERCARD 4
            new TestCardInfo
            {
                Banka = "Yapı Kredi Bankası",
                KartNumarasi = "5400611056942989",
                SonKullanmaTarihi = "10/28",
                CVV = "000",
                TicariKart = false,
                KartTipi = "Kredi Kartı",
                KartMarkasi = "MASTERCARD"
            },

            // Yabancı Kart - MASTERCARD
            new TestCardInfo
            {
                Banka = "Yabancı Kart",
                KartNumarasi = "5163103002982563",
                SonKullanmaTarihi = "12/26",
                CVV = "000",
                TicariKart = false,
                KartTipi = "Kredi Kartı",
                KartMarkasi = "MASTERCARD"
            },

            // Yabancı Kart - Debit MASTERCARD
            new TestCardInfo
            {
                Banka = "Yabancı Kart",
                KartNumarasi = "5486742060635314",
                SonKullanmaTarihi = "12/26",
                CVV = "000",
                TicariKart = false,
                KartTipi = "Debit",
                KartMarkasi = "MASTERCARD"
            },

            // Yabancı Kart - Debit TROY 3DS
            new TestCardInfo
            {
                Banka = "Yabancı Kart",
                KartNumarasi = "9792004525458548",
                SonKullanmaTarihi = "12/26",
                CVV = "000",
                TicariKart = false,
                KartTipi = "Debit",
                KartMarkasi = "TROY",
                DS3_Sifresi = "aygünyıl",
                Aciklama = "3DS Şifre örneği: 102824"
            }
        };
    }
} 