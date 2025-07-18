namespace ParamApi.Sample.Mvc.Models;

/// <summary>
/// 3D Secure HTML içeriği için view model
/// </summary>
public class ThreeDSecureViewModel
{
    public string HtmlContent { get; set; } = string.Empty;
    public string MD { get; set; } = string.Empty;
    public string IslemGUID { get; set; } = string.Empty;
    public string SiparisID { get; set; } = string.Empty;
    public decimal IslemTutar { get; set; }
    
    /// <summary>
    /// HTML içeriğindeki özel karakterleri decode eder
    /// </summary>
    public string GetDecodedHtml()
    {
        if (string.IsNullOrEmpty(HtmlContent))
            return string.Empty;
            
        return System.Net.WebUtility.HtmlDecode(HtmlContent)
            .Replace("&lt;", "<")
            .Replace("&gt;", ">")
            .Replace("&quot;", "\"")
            .Replace("&amp;", "&")
            .Replace("&#39;", "'")
            .Replace("&apos;", "'");
    }
} 