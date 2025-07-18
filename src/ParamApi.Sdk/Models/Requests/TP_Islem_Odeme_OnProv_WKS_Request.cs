using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using ParamApi.Sdk.Helpers;

namespace ParamApi.Sdk.Models.Requests;

/// <summary>
/// Saklı kart ile ön provizyon işlemi isteği
/// Daha önce kaydedilen kart ile ön provizyon işlemi yapmak için kullanılır
/// </summary>
public class TP_Islem_Odeme_OnProv_WKS_Request
{
    /// <summary>
    /// Saklı Kart GUID bilgisi (36 karakter)
    /// </summary>
    [Required(ErrorMessage = "KK_GUID alanı zorunludur")]
    [StringLength(36, ErrorMessage = "KK_GUID 36 karakter olmalıdır")]
    public string KK_GUID { get; set; } = string.Empty;

    /// <summary>
    /// GUID bilgisi (36 karakter)
    /// </summary>
    [Required(ErrorMessage = "KS_Kart_No alanı zorunludur")]
    [StringLength(36, ErrorMessage = "KS_Kart_No 36 karakter olmalıdır")]
    public string KS_Kart_No { get; set; } = string.Empty;

    /// <summary>
    /// Kredi Kartı Sahibi GSM No, Başında 0 olmadan (5xxxxxxxxx)
    /// </summary>
    [Required(ErrorMessage = "KK_Sahibi_GSM alanı zorunludur")]
    [StringLength(10, ErrorMessage = "KK_Sahibi_GSM 10 karakter olmalıdır")]
    public string KK_Sahibi_GSM { get; set; } = string.Empty;

    /// <summary>
    /// Ödeme işlemi başarısız olursa yönlenecek sayfa adresi
    /// </summary>
    [Required(ErrorMessage = "Hata_URL alanı zorunludur")]
    [StringLength(256, ErrorMessage = "Hata_URL maksimum 256 karakter olabilir")]
    [Url(ErrorMessage = "Hata_URL geçerli bir URL formatında olmalıdır")]
    public string Hata_URL { get; set; } = string.Empty;

    /// <summary>
    /// Ödeme işlemi başarılı olursa yönlenecek sayfa adresi
    /// </summary>
    [Required(ErrorMessage = "Basarili_URL alanı zorunludur")]
    [StringLength(256, ErrorMessage = "Basarili_URL maksimum 256 karakter olabilir")]
    [Url(ErrorMessage = "Basarili_URL geçerli bir URL formatında olmalıdır")]
    public string Basarili_URL { get; set; } = string.Empty;

    /// <summary>
    /// Siparişe özel tekil ID (maksimum 50 karakter)
    /// </summary>
    [Required(ErrorMessage = "Siparis_ID alanı zorunludur")]
    [StringLength(50, ErrorMessage = "Siparis_ID maksimum 50 karakter olabilir")]
    public string Siparis_ID { get; set; } = string.Empty;

    /// <summary>
    /// Siparişe ait açıklama (maksimum 250 karakter)
    /// </summary>
    [Required(ErrorMessage = "Siparis_Aciklama alanı zorunludur")]
    [StringLength(250, ErrorMessage = "Siparis_Aciklama maksimum 250 karakter olabilir")]
    public string Siparis_Aciklama { get; set; } = string.Empty;

    /// <summary>
    /// Sipariş Tutarı
    /// </summary>
    [Required(ErrorMessage = "Islem_Tutar alanı zorunludur")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Islem_Tutar 0'dan büyük olmalıdır")]
    public decimal Islem_Tutar { get; set; }

    /// <summary>
    /// Komisyon Dahil Sipariş Tutarı
    /// </summary>
    [Required(ErrorMessage = "Toplam_Tutar alanı zorunludur")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Toplam_Tutar 0'dan büyük olmalıdır")]
    public decimal Toplam_Tutar { get; set; }

    /// <summary>
    /// İşlem Güvenlik Tipi: NS (NonSecure) veya 3D
    /// </summary>
    [Required(ErrorMessage = "Islem_Guvenlik_Tip alanı zorunludur")]
    public string Islem_Guvenlik_Tip { get; set; } = string.Empty;

    /// <summary>
    /// İşleme ait Sipariş ID haricinde tekil ID (opsiyonel)
    /// </summary>
    public string? Islem_ID { get; set; }

    /// <summary>
    /// IP Adresi (maksimum 50 karakter)
    /// </summary>
    [Required(ErrorMessage = "IPAdr alanı zorunludur")]
    [StringLength(50, ErrorMessage = "IPAdr maksimum 50 karakter olabilir")]
    public string IPAdr { get; set; } = string.Empty;

    /// <summary>
    /// Ödemenin gerçekleştiği sayfanın URL'si (opsiyonel, maksimum 256 karakter)
    /// </summary>
    [StringLength(256, ErrorMessage = "Ref_URL maksimum 256 karakter olabilir")]
    public string? Ref_URL { get; set; }

    /// <summary>
    /// Extra Alan 1 (opsiyonel, maksimum 250 karakter)
    /// </summary>
    [StringLength(250, ErrorMessage = "Data1 maksimum 250 karakter olabilir")]
    public string? Data1 { get; set; }

    /// <summary>
    /// Extra Alan 2 (opsiyonel, maksimum 250 karakter)
    /// </summary>
    [StringLength(250, ErrorMessage = "Data2 maksimum 250 karakter olabilir")]
    public string? Data2 { get; set; }

    /// <summary>
    /// Extra Alan 3 (opsiyonel, maksimum 250 karakter)
    /// </summary>
    [StringLength(250, ErrorMessage = "Data3 maksimum 250 karakter olabilir")]
    public string? Data3 { get; set; }

    /// <summary>
    /// Extra Alan 4 (opsiyonel, maksimum 250 karakter)
    /// </summary>
    [StringLength(250, ErrorMessage = "Data4 maksimum 250 karakter olabilir")]
    public string? Data4 { get; set; }

    /// <summary>
    /// Extra Alan 5 (opsiyonel, maksimum 250 karakter)
    /// </summary>
    [StringLength(250, ErrorMessage = "Data5 maksimum 250 karakter olabilir")]
    public string? Data5 { get; set; }

    /// <summary>
    /// Request'in geçerli olup olmadığını kontrol eder
    /// </summary>
    public bool IsValid(out List<ValidationResult> validationResults)
    {
        validationResults = new List<ValidationResult>();
        var context = new ValidationContext(this);
        
        var isValid = Validator.TryValidateObject(this, context, validationResults, true);

        // GSM numarası format kontrolü (5xxxxxxxxx formatında olmalı)
        if (!string.IsNullOrEmpty(KK_Sahibi_GSM))
        {
            var gsmRegex = new Regex(@"^5\d{9}$");
            if (!gsmRegex.IsMatch(KK_Sahibi_GSM))
            {
                validationResults.Add(new ValidationResult(
                    "KK_Sahibi_GSM 5 ile başlayıp 10 haneli olmalıdır (5xxxxxxxxx)",
                    new[] { nameof(KK_Sahibi_GSM) }));
                isValid = false;
            }
        }

        // Güvenlik tipi kontrolü (NS veya 3D)
        if (!string.IsNullOrEmpty(Islem_Guvenlik_Tip))
        {
            var securityRegex = new Regex(@"^(NS|3D)$", RegexOptions.IgnoreCase);
            if (!securityRegex.IsMatch(Islem_Guvenlik_Tip))
            {
                validationResults.Add(new ValidationResult(
                    "Islem_Guvenlik_Tip 'NS' veya '3D' olmalıdır",
                    new[] { nameof(Islem_Guvenlik_Tip) }));
                isValid = false;
            }
        }

        // Business rule: Toplam_Tutar >= Islem_Tutar olmalı
        if (Toplam_Tutar < Islem_Tutar)
        {
            validationResults.Add(new ValidationResult(
                "Toplam_Tutar, Islem_Tutar'dan küçük olamaz",
                new[] { nameof(Toplam_Tutar) }));
            isValid = false;
        }

        return isValid;
    }

    /// <summary>
    /// İşlem tutarını PARAM API formatına çevirir (virgüllü format)
    /// </summary>
    public string GetFormattedIslemTutar()
    {
        return HashHelper.FormatAmount(Islem_Tutar);
    }

    /// <summary>
    /// Toplam tutarı PARAM API formatına çevirir (virgüllü format)
    /// </summary>
    public string GetFormattedToplamTutar()
    {
        return HashHelper.FormatAmount(Toplam_Tutar);
    }
} 