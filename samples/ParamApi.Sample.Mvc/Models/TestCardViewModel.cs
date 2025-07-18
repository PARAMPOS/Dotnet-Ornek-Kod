namespace ParamApi.Sample.Mvc.Models;

/// <summary>
/// Test kartları için view model
/// </summary>
public class TestCardViewModel
{
    public string CardName { get; set; } = string.Empty;
    public string CardNumber { get; set; } = string.Empty;
    public string ExpiryMonth { get; set; } = string.Empty;
    public string ExpiryYear { get; set; } = string.Empty;
    public string CvcCode { get; set; } = string.Empty;
    public string CardHolder { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CardType { get; set; } = string.Empty;
    public bool IsSuccess { get; set; }
} 