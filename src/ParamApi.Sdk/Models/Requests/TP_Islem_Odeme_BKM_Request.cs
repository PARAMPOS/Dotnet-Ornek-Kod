using System.ComponentModel.DataAnnotations;

namespace ParamApi.Sdk.Models.Requests;

/// <summary>
/// BKM Express aracılığı ile ödeme işleminin başlatılacağı request modeli
/// </summary>
public class TP_Islem_Odeme_BKM_Request
{
    /// <summary>
    /// Müşteri bilgisi (ad soyad/firma adı)
    /// </summary>
    public string? Customer_Info { get; set; }

    /// <summary>
    /// Müşteri GSM No, başında 0 olmadan (5xxxxxxxxx)
    /// </summary>
    [Required(ErrorMessage = "Customer_GSM gereklidir")]
    [RegularExpression(@"^5\d{9}$", ErrorMessage = "Customer_GSM 5 ile başlamalı ve 10 haneli olmalıdır")]
    public required string Customer_GSM { get; set; }

    /// <summary>
    /// Ödeme işlemi başarısız olursa yönlenecek sayfa adresi
    /// </summary>
    [Required(ErrorMessage = "Error_URL gereklidir")]
    [MaxLength(250)]
    [Url(ErrorMessage = "Error_URL geçerli bir URL olmalıdır")]
    public required string Error_URL { get; set; }

    /// <summary>
    /// Ödeme işlemi başarılı olırsa yönlenecek sayfa adresi
    /// </summary>
    [Required(ErrorMessage = "Success_URL gereklidir")]
    [MaxLength(250)]
    [Url(ErrorMessage = "Success_URL geçerli bir URL olmalıdır")]
    public required string Success_URL { get; set; }

    /// <summary>
    /// Siparişe özel tekil ID
    /// </summary>
    [Required(ErrorMessage = "Order_ID gereklidir")]
    public required string Order_ID { get; set; }

    /// <summary>
    /// Siparişe ait açıklama
    /// </summary>
    [MaxLength(250)]
    public string? Order_Description { get; set; }

    /// <summary>
    /// Sipariş Tutarı (decimal, otomatik olarak virgüllü formata çevrilir)
    /// </summary>
    [Required(ErrorMessage = "Amount gereklidir")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount 0'dan büyük olmalıdır")]
    public decimal Amount { get; set; }

    /// <summary>
    /// İşleme ait Sipariş ID haricinde tekil ID
    /// </summary>
    public string? Transaction_ID { get; set; }

    /// <summary>
    /// IP Adresi
    /// </summary>
    [Required(ErrorMessage = "IPAddress gereklidir")]
    [MaxLength(250)]
    public required string IPAddress { get; set; }

    /// <summary>
    /// Ödemenin gerçekleştiği sayfanın URL'si
    /// </summary>
    [MaxLength(256)]
    public string? Referrer_URL { get; set; }

    /// <summary>
    /// Amount'u PARAM API formatında virgüllü string'e çevirir
    /// </summary>
    /// <returns>Virgüllü tutar formatı (örn: "100,50")</returns>
    public string GetFormattedAmount()
    {
        return Amount.ToString("F2", System.Globalization.CultureInfo.GetCultureInfo("tr-TR"));
    }

    /// <summary>
    /// Request'in geçerli olup olmadığını kontrol eder
    /// </summary>
    public bool IsValid(out ICollection<ValidationResult> validationResults)
    {
        var context = new ValidationContext(this);
        validationResults = new List<ValidationResult>();
        return Validator.TryValidateObject(this, context, validationResults, true);
    }
} 