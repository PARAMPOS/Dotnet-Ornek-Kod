namespace ParamApi.Sample.Mvc.Models;

/// <summary>
/// TP_Islem_Odeme_WD 3D callback parametreleri için model
/// </summary>
public class TP_Islem_Odeme_WD_CallbackModel
{
    /// <summary>
    /// Sonuç değeri
    /// </summary>
    public string TURKPOS_RETVAL_Sonuc { get; set; } = string.Empty;

    /// <summary>
    /// Sonuç açıklaması
    /// </summary>
    public string TURKPOS_RETVAL_Sonuc_Str { get; set; } = string.Empty;

    /// <summary>
    /// Guid bilgisi
    /// </summary>
    public string TURKPOS_RETVAL_GUID { get; set; } = string.Empty;

    /// <summary>
    /// İşlem Tarihi
    /// </summary>
    public string TURKPOS_RETVAL_Islem_Tarih { get; set; } = string.Empty;

    /// <summary>
    /// Dekont Numarası
    /// </summary>
    public string TURKPOS_RETVAL_Dekont_ID { get; set; } = string.Empty;

    /// <summary>
    /// Müşteriden tahsil edilen tutar
    /// </summary>
    public string TURKPOS_RETVAL_Tahsilat_Tutari { get; set; } = string.Empty;

    /// <summary>
    /// Net tutar. Tahsilat tutarından komisyon tutarının çıkarılmış halidir.
    /// </summary>
    public string TURKPOS_RETVAL_Odeme_Tutari { get; set; } = string.Empty;

    /// <summary>
    /// Servise gönderdiğiniz işleme ait tekil Siparis_ID
    /// </summary>
    public string TURKPOS_RETVAL_Siparis_ID { get; set; } = string.Empty;

    /// <summary>
    /// Servise gönderdiğiniz Islem_ID
    /// </summary>
    public string TURKPOS_RETVAL_Islem_ID { get; set; } = string.Empty;

    /// <summary>
    /// Metottaki Data1, Data2, Data3, Data4, Data5 parametrelerinin "|" ile birleştirilmiş hali
    /// Ext_Data = Data1 & "|" & Data2 & "|" & Data3 & "|" & Data4 & "|" & Data5
    /// </summary>
    public string TURKPOS_RETVAL_Ext_Data { get; set; } = string.Empty;

    /// <summary>
    /// Kod banka hatasını dönmektedir
    /// </summary>
    public string TURKPOS_RETVAL_Banka_Sonuc_Kod { get; set; } = string.Empty;

    /// <summary>
    /// Kredi kartı numarası (İlk 6, son 4)
    /// </summary>
    public string TURKPOS_RETVAL_KK_No { get; set; } = string.Empty;

    /// <summary>
    /// İşlemin başarılı olup olmadığını kontrol eder
    /// Başarılı işlem: Sonuc > 0, Islem_ID > 0, Dekont_ID > 0
    /// </summary>
    public bool IsSuccess
    {
        get
        {
            // Sonuc > 0 kontrolü
            if (!int.TryParse(TURKPOS_RETVAL_Sonuc, out var sonuc) || sonuc <= 0)
                return false;

            // Islem_ID > 0 kontrolü
            if (!int.TryParse(TURKPOS_RETVAL_Islem_ID, out var islemId) || islemId <= 0)
                return false;

            // Dekont_ID "0" dan büyük olmalı (kredi kartından çekim tamamlanmış anlamına gelir)
            if (!int.TryParse(TURKPOS_RETVAL_Dekont_ID, out var dekontId) || dekontId <= 0)
                return false;

            return true;
        }
    }
} 