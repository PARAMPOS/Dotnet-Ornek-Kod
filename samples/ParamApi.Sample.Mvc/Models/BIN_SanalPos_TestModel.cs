using System.ComponentModel.DataAnnotations;
using ParamApi.Sdk.Models.Requests;
using ParamApi.Sdk.Models.Responses;

namespace ParamApi.Sample.Mvc.Models;

/// <summary>
/// BIN_SanalPos test sayfası için view model
/// </summary>
public class BIN_SanalPos_TestModel
{
    /// <summary>
    /// BIN Değeri (6 veya 8 haneli)
    /// </summary>
    [Display(Name = "BIN Değeri (6-8 hane, boş bırakılırsa tüm BIN'ler döner)")]
    [StringLength(8, MinimumLength = 6, ErrorMessage = "BIN değeri 6 veya 8 haneli olmalıdır")]
    public string? BIN { get; set; }

    /// <summary>
    /// SDK Request'ine dönüştür
    /// </summary>
    public BIN_SanalPos_Request ToRequest()
    {
        return new BIN_SanalPos_Request
        {
            BIN = string.IsNullOrWhiteSpace(BIN) ? null : BIN
        };
    }

    /// <summary>
    /// Örnek BIN değerleri (6 haneli)
    /// </summary>
    public static List<(string BIN, string BankName)> Sample6DigitBins => new()
    {
        ("540667", "Akbank - Visa"),
        ("540061", "Akbank - Visa"),
        ("374427", "Akbank - American Express"),
        ("510318", "Garanti BBVA - MasterCard"),
        ("552608", "Garanti BBVA - MasterCard"),
        ("454360", "İş Bankası - Visa"),
        ("453810", "İş Bankası - Visa"),
        ("549549", "Yapı Kredi - Visa"),
        ("402360", "Yapı Kredi - Visa"),
        ("654321", "Test BIN - Genel"),
        ("123456", "Test BIN - Örnek")
    };

    /// <summary>
    /// Örnek BIN değerleri (8 haneli)
    /// </summary>
    public static List<(string BIN, string BankName)> Sample8DigitBins => new()
    {
        ("54066701", "Akbank - Visa Detay"),
        ("54006101", "Akbank - Visa Detay"),
        ("51031801", "Garanti BBVA - MasterCard Detay"),
        ("55260801", "Garanti BBVA - MasterCard Detay"),
        ("45436001", "İş Bankası - Visa Detay"),
        ("45381001", "İş Bankası - Visa Detay"),
        ("54954901", "Yapı Kredi - Visa Detay"),
        ("40236001", "Yapı Kredi - Visa Detay")
    };

    /// <summary>
    /// Tüm örnek BIN'ler
    /// </summary>
    public static List<(string BIN, string BankName)> AllSampleBins =>
        Sample6DigitBins.Concat(Sample8DigitBins).ToList();
}

/// <summary>
/// BIN_SanalPos sonuç sayfası için view model
/// </summary>
public class BIN_SanalPos_ResultModel
{
    /// <summary>
    /// Gönderilen request bilgileri
    /// </summary>
    public BIN_SanalPos_TestModel? Request { get; set; }

    /// <summary>
    /// API Response
    /// </summary>
    public BIN_SanalPos_Response? Response { get; set; }

    /// <summary>
    /// İşlem başarılı mı?
    /// </summary>
    public bool IsSuccess => Response?.Sonuc > 0;

    /// <summary>
    /// Hata mesajı (varsa)
    /// </summary>
    public string? ErrorMessage => IsSuccess ? null : Response?.Sonuc_Str;

    /// <summary>
    /// İşlem zamanı
    /// </summary>
    public DateTime ProcessTime { get; set; } = DateTime.Now;

    /// <summary>
    /// İşlem süresi (ms)
    /// </summary>
    public long ProcessDuration { get; set; }
} 