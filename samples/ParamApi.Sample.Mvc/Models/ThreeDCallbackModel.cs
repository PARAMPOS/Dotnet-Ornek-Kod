namespace ParamApi.Sample.Mvc.Models;

/// <summary>
/// 3D Secure callback parametreleri için model
/// </summary>
public class ThreeDCallbackModel
{
    public string md { get; set; } = string.Empty;
    public int mdStatus { get; set; }
    public string orderId { get; set; } = string.Empty;
    public string transactionAmount { get; set; } = string.Empty;
    public string islemGUID { get; set; } = string.Empty;
    public string islemHash { get; set; } = string.Empty;
    
    /// <summary>
    /// 3D doğrulamanın başarılı olup olmadığını kontrol eder
    /// </summary>
    public bool Is3DSuccessful => mdStatus == 1;
} 