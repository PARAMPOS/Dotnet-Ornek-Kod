using Microsoft.AspNetCore.Mvc;
using ParamApi.Sample.Mvc.Models;
using ParamApi.Sdk.Services;
using ParamApi.Sdk.Models.Requests;
using ParamApi.Sdk.Models.Responses;
using ParamApi.Sdk.Models.Shared;
using System.Diagnostics;

namespace ParamApi.Sample.Mvc.Controllers;

/// <summary>
/// Param API test controller'ı
/// </summary>
public class TestController : Controller
{
    private readonly ITurkposService _turkposService;
    private readonly IKartService _kartService;
    private readonly ILogger<TestController> _logger;

    public TestController(
        ITurkposService turkposService,
        IKartService kartService,
        ILogger<TestController> logger)
    {
        _turkposService = turkposService;
        _kartService = kartService;
        _logger = logger;
    }

    /// <summary>
    /// Ana test sayfası
    /// </summary>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Test kartlarını getirir
    /// </summary>
    [HttpGet]
    public IActionResult GetTestCards()
    {
        try
        {
            var testCards = _kartService.GetTestCards();
            var viewModels = testCards.Select(card =>
            {
                var expireParts = card.GetExpireDateParts();
                return new TestCardViewModel
                {
                    CardName = card.Banka,
                    CardNumber = card.KartNumarasi,
                    ExpiryMonth = expireParts.Ay,
                    ExpiryYear = expireParts.Yil,
                    CvcCode = card.CVV,
                    CardHolder = "TEST KULLANICI",
                    Description = card.Aciklama ?? $"{card.KartMarkasi} - {card.KartTipi}",
                    CardType = card.KartMarkasi,
                    IsSuccess = !card.YabanciKart // Yabancı kartlar başarısız test için
                };
            }).ToList();
            
            return Json(viewModels);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Test kartları alınırken hata oluştu");
            return Json(new List<TestCardViewModel>());
        }
    }

    /// <summary>
    /// TP_WMD_UCD test formu
    /// </summary>
    [HttpGet]
    public IActionResult TP_WMD_UCD()
    {
        var model = new TP_WMD_UCD_TestModel();
        return View(model);
    }

    /// <summary>
    /// TP_WMD_UCD test işlemi
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TP_WMD_UCD(TP_WMD_UCD_TestModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            // Test model'ini SDK request'ine dönüştür
            var request = new TP_WMD_UCD_Request
            {
                KK_Sahibi = model.KK_Sahibi,
                KK_No = model.KK_No?.Replace(" ", "").Replace("-", ""), // Boşluk ve tire temizle
                KK_SK_Ay = model.KK_SK_Ay.ToString(),
                KK_SK_Yil = model.KK_SK_Yil.ToString(),
                KK_CVC = model.KK_CVC,
                KK_Sahibi_GSM = model.KK_Sahibi_GSM,
                Hata_URL = model.Hata_URL,
                Basarili_URL = model.Basarili_URL,
                Siparis_ID = model.Siparis_ID,
                Siparis_Aciklama = model.Siparis_Aciklama,
                Taksit = model.Taksit,
                Islem_Tutar = model.Islem_Tutar,
                Toplam_Tutar = model.Toplam_Tutar,
                Islem_Guvenlik_Tip = model.Islem_Guvenlik_Tip,
                Islem_ID = model.Islem_ID,
                IPAdr = model.IPAdr,
                Ref_URL = model.Ref_URL,
                Data1 = model.Data1,
                Data2 = model.Data2,
                Data3 = model.Data3,
                Data4 = model.Data4,
                Data5 = model.Data5
            };

            // SDK üzerinden API çağrısı yap
            var response = await _turkposService.TP_WMD_UCDAsync(request);
            
            // Debug bilgileri
            _logger.LogInformation("TP_WMD_UCD Response - Sonuç: {Sonuc}, UCD_HTML: {UcdHtml}, UCD_MD: {UcdMd}, Is3D: {Is3D}, IsNonSecure: {IsNonSecure}", 
                response.Sonuc, 
                response.UCD_HTML?.Substring(0, Math.Min(50, response.UCD_HTML?.Length ?? 0)) + "...",
                response.UCD_MD?.Substring(0, Math.Min(20, response.UCD_MD?.Length ?? 0)) + "...",
                response.Is3D,
                response.IsNonSecure);

            // 3D Secure işlemi kontrolü - SDK'daki property'leri kullan
            if (response.Is3D && response.Is3DInitiated)
            {
                // 3D Secure gerekli - TempData'ya bilgileri kaydet
                TempData["ThreeDHtml"] = response.UCD_HTML; // UCD_HTML 3D form'u içerir
                TempData["OrderId"] = model.Siparis_ID;
                TempData["Amount"] = model.Islem_Tutar.ToString("F2");
                TempData["IsSecure"] = true;
                TempData["IslemGUID"] = response.Islem_GUID;
                TempData["UCD_MD"] = response.UCD_MD; // PAY işlemi için gerekli
                
                _logger.LogInformation("3D Secure işlemi başlatıldı: {SiparisId}, UCD_HTML uzunluk: {HtmlLength}", 
                    model.Siparis_ID, response.UCD_HTML?.Length ?? 0);
            }
            else if (response.IsNonSecure && response.IsNonSecureSuccessful)
            {
                // NonSecure işlem başarılı
                TempData["IsSecure"] = false;
                _logger.LogInformation("NonSecure işlem tamamlandı: {SiparisId}", model.Siparis_ID);
            }
            else
            {
                // Hatalı işlem
                TempData["IsSecure"] = false;
                _logger.LogWarning("İşlem başarısız: {SiparisId}, Sonuç: {Sonuc}, Açıklama: {SonucStr}", 
                    model.Siparis_ID, response.Sonuc, response.Sonuc_Str);
            }

            // Sonuç model'ini oluştur
            var resultModel = new TP_WMD_UCD_ResultModel
            {
                IsSuccess = true,
                Sonuc = response.Sonuc,
                Sonuc_Str = response.Sonuc_Str,
                UCD_URL = response.UCD_HTML,
                UCD_MD = response.UCD_MD,
                Islem_ID = response.Islem_ID.ToString(),
                Dekont_ID = response.Islem_GUID,
                Siparis_ID = model.Siparis_ID
            };

            return View("TP_WMD_UCD_Result", resultModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "TP_WMD_UCD test hatası: {Message}", ex.Message);

            var errorModel = new TP_WMD_UCD_ResultModel
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            };

            return View("TP_WMD_UCD_Result", errorModel);
        }
    }

    /// <summary>
    /// Başarılı ödeme callback'i - 3D Secure sonrası
    /// </summary>
    [HttpGet]
    [HttpPost]
    public async Task<IActionResult> Success()
    {
        _logger.LogInformation("Başarılı ödeme callback'i alındı");
        
        // Query string ve POST parametrelerini al
        var parameters = new Dictionary<string, string>();
        foreach (var param in Request.Query)
        {
            parameters[param.Key] = param.Value.ToString();
        }

        if (Request.Method == "POST" && Request.HasFormContentType)
        {
            foreach (var param in Request.Form)
            {
                parameters[param.Key] = param.Value.ToString();
            }
        }

        ViewBag.Parameters = parameters;

        // 3D Secure callback parametrelerini kontrol et
        var callbackModel = new ThreeDCallbackModel
        {
            md = parameters.GetValueOrDefault("md", ""),
            mdStatus = int.TryParse(parameters.GetValueOrDefault("mdStatus", "0"), out var status) ? status : 0,
            orderId = parameters.GetValueOrDefault("orderId", ""),
            transactionAmount = parameters.GetValueOrDefault("transactionAmount", ""),
            islemGUID = parameters.GetValueOrDefault("islemGUID", ""),
            islemHash = parameters.GetValueOrDefault("islemHash", "")
        };

        ViewBag.CallbackModel = callbackModel;

        // 3D Secure başarılı ise TP_WMD_PAY işlemini otomatik tetikle
        if (callbackModel.Is3DSuccessful && !string.IsNullOrEmpty(callbackModel.md) && !string.IsNullOrEmpty(callbackModel.islemGUID))
        {
            try
            {
                // Hangi metottan geldiğini belirle
                var methodType = TempData["MethodType"]?.ToString() ?? "UCD";
                ViewBag.MethodType = methodType;
                
                _logger.LogInformation("3D Secure başarılı, TP_WMD_PAY işlemi başlatılıyor. OrderId: {OrderId}, Method: {Method}", callbackModel.orderId, methodType);

                var payRequest = new TP_WMD_PAY_Request
                {
                    UCD_MD = callbackModel.md,
                    Islem_GUID = callbackModel.islemGUID,
                    Siparis_ID = callbackModel.orderId
                };

                var payResponse = await _turkposService.TP_WMD_PAYAsync(payRequest);

                ViewBag.PayResponse = payResponse;
                ViewBag.PaySuccess = true;

                if (payResponse.IsSuccessful)
                {
                    if (methodType == "OnProv_WMD")
                    {
                        _logger.LogInformation("3D Ön Provizyon tamamlandı. OrderId: {OrderId}, Dekont: {Dekont}", callbackModel.orderId, payResponse.Dekont_ID);
                    }
                    else
                    {
                        _logger.LogInformation("3D Ödeme tamamlandı. OrderId: {OrderId}, Dekont: {Dekont}", callbackModel.orderId, payResponse.Dekont_ID);
                    }
                }

                _logger.LogInformation("TP_WMD_PAY işlemi tamamlandı. Sonuç: {Sonuc}, Açıklama: {SonucAck}", 
                    payResponse.Sonuc, payResponse.Sonuc_Ack);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "TP_WMD_PAY işlemi sırasında hata oluştu: {Message}", ex.Message);
                ViewBag.PayError = ex.Message;
                ViewBag.PaySuccess = false;
            }
        }

        return View();
    }

    /// <summary>
    /// Hatalı ödeme callback'i
    /// </summary>
    [HttpGet]
    [HttpPost]
    public IActionResult Error()
    {
        _logger.LogWarning("Hatalı ödeme callback'i alındı");
        
        // Query string parametrelerini al
        var parameters = new Dictionary<string, string>();
        foreach (var param in Request.Query)
        {
            parameters[param.Key] = param.Value.ToString();
        }

        // POST parametrelerini al
        if (Request.Method == "POST" && Request.HasFormContentType)
        {
            foreach (var param in Request.Form)
            {
                parameters[param.Key] = param.Value.ToString();
            }
        }

        ViewBag.Parameters = parameters;
        return View();
    }

    /// <summary>
    /// 3D Secure HTML içeriğini gösterir
    /// </summary>
    [HttpGet]
    public IActionResult ThreeDSecure(string htmlContent, string md, string islemGUID, string siparisID, decimal islemTutar)
    {
        var model = new ThreeDSecureViewModel
        {
            HtmlContent = htmlContent ?? string.Empty,
            MD = md ?? string.Empty,
            IslemGUID = islemGUID ?? string.Empty,
            SiparisID = siparisID ?? string.Empty,
            IslemTutar = islemTutar
        };

        return View(model);
    }

    #region TP_Islem_Odeme_OnProv_WMD Tests

    /// <summary>
    /// TP_Islem_Odeme_OnProv_WMD test sayfası - GET
    /// </summary>
    [HttpGet]
    public IActionResult TP_Islem_Odeme_OnProv_WMD()
    {
        var model = new TP_Islem_Odeme_OnProv_WMD_TestModel();
        return View(model);
    }

    /// <summary>
    /// TP_Islem_Odeme_OnProv_WMD test işlemi - POST
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TP_Islem_Odeme_OnProv_WMD(TP_Islem_Odeme_OnProv_WMD_TestModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Test model'ini SDK request'ine dönüştür
            var request = new TP_Islem_Odeme_OnProv_WMD_Request
            {
                KK_Sahibi = model.KK_Sahibi,
                KK_No = model.KK_No?.Replace(" ", "").Replace("-", ""), // Boşluk ve tire temizle
                KK_SK_Ay = model.KK_SK_Ay.ToString("D2"),
                KK_SK_Yil = model.KK_SK_Yil.ToString(),
                KK_CVC = model.KK_CVC,
                KK_Sahibi_GSM = model.KK_Sahibi_GSM,
                Siparis_ID = model.Siparis_ID,
                Siparis_Aciklama = model.Siparis_Aciklama,
                Islem_Tutar = model.Islem_Tutar,
                Toplam_Tutar = model.Toplam_Tutar,
                Taksit = model.Taksit,
                Hata_URL = model.Hata_URL,
                Basarili_URL = model.Basarili_URL,
                Islem_Guvenlik_Tip = model.Islem_Guvenlik_Tip,
                Islem_ID = model.Islem_ID,
                IPAdr = model.IPAdr,
                Ref_URL = model.Ref_URL,
                Data1 = model.Data1,
                Data2 = model.Data2,
                Data3 = model.Data3,
                Data4 = model.Data4,
                Data5 = model.Data5
            };

            // SDK üzerinden API çağrısı yap
            var response = await _turkposService.TP_Islem_Odeme_OnProv_WMDAsync(request);
            
            // Debug bilgileri
            _logger.LogInformation("TP_Islem_Odeme_OnProv_WMD Response - Sonuç: {Sonuc}, UCD_HTML: {UcdHtml}, UCD_MD: {UcdMd}, Is3DSecure: {Is3DSecure}, IsNonSecure: {IsNonSecure}", 
                response.Sonuc, 
                response.UCD_HTML?.Substring(0, Math.Min(50, response.UCD_HTML?.Length ?? 0)) + "...",
                response.UCD_MD?.Substring(0, Math.Min(20, response.UCD_MD?.Length ?? 0)) + "...",
                response.Is3DSecure,
                response.IsNonSecure);

            // 3D Secure işlemi kontrolü
            if (response.IsSuccessful && response.Is3DSecure)
            {
                // 3D Secure gerekli - TempData'ya bilgileri kaydet
                TempData["ThreeDHtml"] = response.UCD_HTML;
                TempData["OrderId"] = model.Siparis_ID;
                TempData["Amount"] = model.Islem_Tutar.ToString("F2");
                TempData["IsSecure"] = true;
                TempData["IslemGUID"] = response.Islem_GUID;
                TempData["UCD_MD"] = response.UCD_MD;
                TempData["MethodType"] = "OnProv_WMD"; // Metot tipini belirt
                
                _logger.LogInformation("3D Secure işlemi başlatıldı: {SiparisId}", model.Siparis_ID);
            }
            else
            {
                // NonSecure işlem veya hata
                TempData["IsSecure"] = false;
                if (response.IsSuccessful)
                {
                    _logger.LogInformation("NonSecure ön provizyon tamamlandı: {SiparisId}", model.Siparis_ID);
                }
                else
                {
                    _logger.LogWarning("Ön provizyon işlemi başarısız: {SiparisId}, Hata: {Hata}", model.Siparis_ID, response.Sonuc_Str);
                }
            }

            // Sonuç model'ini oluştur
            var resultModel = new TP_Islem_Odeme_OnProv_WMD_ResultModel
            {
                IsSuccess = response.IsSuccessful,
                Sonuc = response.Sonuc,
                Sonuc_Str = response.Sonuc_Str,
                UCD_HTML = response.UCD_HTML,
                UCD_MD = response.UCD_MD,
                Islem_ID = response.Islem_ID.ToString(),
                Islem_GUID = response.Islem_GUID,
                Siparis_ID = model.Siparis_ID,
                Bank_Trans_ID = response.Bank_Trans_ID,
                Bank_AuthCode = response.Bank_AuthCode,
                Bank_HostMsg = response.Bank_HostMsg,
                Banka_Sonuc_Kod = response.Banka_Sonuc_Kod,
                Bank_Extra = response.Bank_Extra,
                Bank_HostRefNum = response.Bank_HostRefNum
            };

            if (!response.IsSuccessful)
            {
                resultModel.ErrorMessage = response.Sonuc_Str;
            }

            _logger.LogInformation("TP_Islem_Odeme_OnProv_WMD test sonucu: {SiparisId}, Başarılı: {Basarili}", model.Siparis_ID, response.IsSuccessful);

            return View("TP_Islem_Odeme_OnProv_WMD_Result", resultModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "TP_Islem_Odeme_OnProv_WMD test hatası: {SiparisId}", model.Siparis_ID);
            
            var errorModel = new TP_Islem_Odeme_OnProv_WMD_ResultModel
            {
                IsSuccess = false,
                ErrorMessage = $"Test hatası: {ex.Message}",
                Siparis_ID = model.Siparis_ID
            };
            
            return View("TP_Islem_Odeme_OnProv_WMD_Result", errorModel);
        }
    }

    #endregion

    #region Ön Provizyon Kapama İşlemleri

    /// <summary>
    /// TP_Islem_Odeme_OnProv_Kapa test formu
    /// </summary>
    [HttpGet]
    public IActionResult TP_Islem_Odeme_OnProv_Kapa()
    {
        var model = new TP_Islem_Odeme_OnProv_Kapa_TestModel();
        return View(model);
    }

    /// <summary>
    /// TP_Islem_Odeme_OnProv_Kapa test işlemi
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TP_Islem_Odeme_OnProv_Kapa(TP_Islem_Odeme_OnProv_Kapa_TestModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            // Test model'ini SDK request'ine dönüştür
            var request = new TP_Islem_Odeme_OnProv_Kapa_Request
            {
                Prov_ID = string.IsNullOrWhiteSpace(model.Prov_ID) ? null : model.Prov_ID,
                Prov_Tutar = model.Prov_Tutar,
                Siparis_ID = model.Siparis_ID
            };

            // SDK üzerinden API çağrısı yap
            var response = await _turkposService.TP_Islem_Odeme_OnProv_KapaAsync(request);
            
            _logger.LogInformation("TP_Islem_Odeme_OnProv_Kapa Response - Sonuç: {Sonuc}, Sipariş ID: {SiparisId}, Dekont ID: {DekontId}", 
                response.Sonuc, response.Siparis_ID, response.Dekont_ID);

            // Sonuç model'ini oluştur
            var resultModel = new TP_Islem_Odeme_OnProv_Kapa_ResultModel
            {
                Sonuc = response.Sonuc,
                Sonuc_Str = response.Sonuc_Str,
                Prov_ID = response.Prov_ID,
                Dekont_ID = response.Dekont_ID,
                Banka_Sonuc_Kod = response.Banka_Sonuc_Kod,
                Siparis_ID = response.Siparis_ID,
                Bank_Trans_ID = response.Bank_Trans_ID,
                Bank_AuthCode = response.Bank_AuthCode,
                Bank_HostMsg = response.Bank_HostMsg,
                Bank_Extra = response.Bank_Extra,
                Bank_HostRefNum = response.Bank_HostRefNum,
                IsSuccessful = response.IsSuccessful,
                IsBankSuccessful = response.IsBankSuccessful,
                TestData = model
            };

            return View("TP_Islem_Odeme_OnProv_Kapa_Result", resultModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "TP_Islem_Odeme_OnProv_Kapa test hatası: {Message}", ex.Message);

            var errorModel = new TP_Islem_Odeme_OnProv_Kapa_ResultModel
            {
                Sonuc = "0",
                Sonuc_Str = $"Hata: {ex.Message}",
                IsSuccessful = false,
                IsBankSuccessful = false,
                TestData = model
            };

            return View("TP_Islem_Odeme_OnProv_Kapa_Result", errorModel);
        }
    }

    #endregion

    #region Ön Provizyon İptal İşlemleri

    /// <summary>
    /// TP_Islem_Iptal_OnProv test formu
    /// </summary>
    [HttpGet]
    public IActionResult TP_Islem_Iptal_OnProv()
    {
        var model = new TP_Islem_Iptal_OnProv_TestModel();
        return View(model);
    }

    /// <summary>
    /// TP_Islem_Iptal_OnProv test işlemi
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TP_Islem_Iptal_OnProv(TP_Islem_Iptal_OnProv_TestModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            // Test model'ini SDK request'ine dönüştür
            var request = new TP_Islem_Iptal_OnProv_Request
            {
                Prov_ID = string.IsNullOrWhiteSpace(model.Prov_ID) ? null : model.Prov_ID,
                Siparis_ID = model.Siparis_ID
            };

            // SDK üzerinden API çağrısı yap
            var response = await _turkposService.TP_Islem_Iptal_OnProvAsync(request);
            
            _logger.LogInformation("TP_Islem_Iptal_OnProv Response - Sonuç: {Sonuc}, Sipariş ID: {SiparisId}, Banka Kodu: {BankaKod}", 
                response.Sonuc, model.Siparis_ID, response.Banka_Sonuc_Kod);

            // Sonuç model'ini oluştur
            var resultModel = new TP_Islem_Iptal_OnProv_ResultModel
            {
                Sonuc = response.Sonuc,
                Sonuc_Str = response.Sonuc_Str,
                Banka_Sonuc_Kod = response.Banka_Sonuc_Kod,
                IsSuccessful = response.IsSuccessful,
                IsBankSuccessful = response.IsBankSuccessful,
                IsFullySuccessful = response.IsFullySuccessful,
                SonucAsInt = response.SonucAsInt,
                BankaSonucKodAsInt = response.BankaSonucKodAsInt,
                TestData = model
            };

            return View("TP_Islem_Iptal_OnProv_Result", resultModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "TP_Islem_Iptal_OnProv test hatası: {Message}", ex.Message);

            var errorModel = new TP_Islem_Iptal_OnProv_ResultModel
            {
                Sonuc = "0",
                Sonuc_Str = $"Hata: {ex.Message}",
                Banka_Sonuc_Kod = "-1",
                IsSuccessful = false,
                IsBankSuccessful = false,
                IsFullySuccessful = false,
                TestData = model
            };

            return View("TP_Islem_Iptal_OnProv_Result", errorModel);
        }
    }

    #endregion

    #region KartService Metodları

    /// <summary>
    /// KS_Kart_Ekle test formu
    /// </summary>
    [HttpGet]
    public IActionResult KS_Kart_Ekle()
    {
        var model = new KS_Kart_Ekle_TestModel();
        return View(model);
    }

    /// <summary>
    /// KS_Kart_Ekle test işlemi
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> KS_Kart_Ekle(KS_Kart_Ekle_TestModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            // Test model'ini SDK request'ine dönüştür
            var request = new KS_Kart_Ekle_Request
            {
                KK_Sahibi = model.KK_Sahibi,
                KK_No = model.KK_No?.Replace(" ", "").Replace("-", ""), // Boşluk ve tire temizle
                KK_SK_Ay = model.KK_SK_Ay,
                KK_SK_Yil = model.KK_SK_Yil,
                KK_Kart_Adi = model.KK_Kart_Adi,
                KK_Islem_ID = model.KK_Islem_ID
            };

            // SDK üzerinden API çağrısı yap
            var response = await _kartService.KS_Kart_EkleAsync(request);
            
            _logger.LogInformation("KS_Kart_Ekle Response - Sonuç: {Sonuc}, KS_GUID: {KsGuid}", 
                response.Sonuc, response.KS_GUID);

            // Sonuç model'ini oluştur
            var resultModel = new KS_Kart_Ekle_ResultModel
            {
                Sonuc = response.Sonuc,
                Sonuc_Str = response.Sonuc_Str,
                KS_GUID = response.KS_GUID,
                RequestData = model
            };

            return View("KS_Kart_Ekle_Result", resultModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "KS_Kart_Ekle test hatası: {Message}", ex.Message);

            var errorModel = new KS_Kart_Ekle_ResultModel
            {
                Sonuc = 0,
                Sonuc_Str = "Hata oluştu",
                ErrorMessage = ex.Message,
                RequestData = model
            };

            return View("KS_Kart_Ekle_Result", errorModel);
        }
    }

    /// <summary>
    /// KS_Tahsilat test formu
    /// </summary>
    [HttpGet]
    public IActionResult KS_Tahsilat()
    {
        var model = new KS_Tahsilat_TestModel();
        return View(model);
    }

    /// <summary>
    /// KS_Tahsilat test işlemi
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> KS_Tahsilat(KS_Tahsilat_TestModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            // URL'leri oluştur
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var successUrl = $"{baseUrl}/Test/KS_Tahsilat_Success";
            var errorUrl = $"{baseUrl}/Test/Error";

            // Test model'ini SDK request'ine dönüştür
            var request = new KS_Tahsilat_Request
            {
                KS_GUID = model.KS_GUID,
                CVV = model.CVV,
                KK_Sahibi_GSM = model.KK_Sahibi_GSM,
                Hata_URL = errorUrl,
                Basarili_URL = successUrl,
                Siparis_ID = model.Siparis_ID,
                Siparis_Aciklama = model.Siparis_Aciklama,
                Taksit = model.Taksit,
                Islem_Tutar = model.Islem_Tutar,
                Toplam_Tutar = model.Toplam_Tutar,
                Islem_Guvenlik_Tip = model.Islem_Guvenlik_Tip,
                Islem_ID = model.Islem_ID,
                IPAdr = Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1",
                Ref_URL = model.Ref_URL,
                Data1 = model.Data1,
                Data2 = model.Data2,
                Data3 = model.Data3,
                Data4 = model.Data4,
                KK_Islem_ID = model.KK_Islem_ID
            };

            // SDK üzerinden API çağrısı yap
            var response = await _kartService.KS_TahsilatAsync(request);
            
            _logger.LogInformation("KS_Tahsilat Response - Sonuç: {Sonuc}, UCD_URL: {UcdUrl}, Islem_ID: {IslemId}", 
                response.Sonuc, response.UCD_URL, response.Islem_ID);

            // 3D Secure işlemi kontrolü
            if (response.Is3DTransaction)
            {
                // 3D Secure gerekli - HTML decode et
                var decodedHtml = DecodeHtml(response.UCD_URL);
                
                // TempData'ya bilgileri kaydet
                TempData["ThreeDHtml"] = decodedHtml;
                TempData["OrderId"] = model.Siparis_ID;
                TempData["Amount"] = model.Islem_Tutar.ToString("F2");
                TempData["IsSecure"] = true;
                TempData["MethodType"] = "KS_Tahsilat";
                
                _logger.LogInformation("KS_Tahsilat 3D Secure işlemi başlatıldı: {SiparisId}", model.Siparis_ID);
            }
            else if (response.IsNonSecureSuccessful)
            {
                // NonSecure işlem başarılı
                TempData["IsSecure"] = false;
                TempData["MethodType"] = "KS_Tahsilat";
                _logger.LogInformation("KS_Tahsilat NonSecure işlem tamamlandı: {SiparisId}", model.Siparis_ID);
            }
            else
            {
                // Hatalı işlem
                TempData["IsSecure"] = false;
                TempData["MethodType"] = "KS_Tahsilat";
                _logger.LogWarning("KS_Tahsilat işlem başarısız: {SiparisId}, Sonuç: {Sonuc}", 
                    model.Siparis_ID, response.Sonuc);
            }

            // Sonuç model'ini oluştur
            var resultModel = new KS_Tahsilat_ResultModel
            {
                Sonuc = response.Sonuc,
                Sonuc_Str = response.Sonuc_Str,
                UCD_URL = response.UCD_URL,
                Islem_ID = response.Islem_ID,
                RequestData = model,
                DecodedHtml = response.Is3DTransaction ? DecodeHtml(response.UCD_URL) : null
            };

            return View("KS_Tahsilat_Result", resultModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "KS_Tahsilat test hatası: {Message}", ex.Message);

            var errorModel = new KS_Tahsilat_ResultModel
            {
                Sonuc = 0,
                Sonuc_Str = "Hata oluştu",
                ErrorMessage = ex.Message,
                RequestData = model
            };

            return View("KS_Tahsilat_Result", errorModel);
        }
    }

    /// <summary>
    /// KS_Tahsilat başarılı callback'i - 3D Secure sonrası
    /// </summary>
    [HttpGet]
    [HttpPost]
    public IActionResult KS_Tahsilat_Success()
    {
        _logger.LogInformation("KS_Tahsilat başarılı callback'i alındı");
        
        // Query string ve POST parametrelerini al
        var parameters = new Dictionary<string, string>();
        
        // Query string parametrelerini ekle
        foreach (var param in Request.Query)
        {
            parameters[param.Key] = param.Value.ToString();
        }
        
        // POST parametrelerini ekle (varsa)
        if (Request.Method == "POST" && Request.HasFormContentType)
        {
            foreach (var param in Request.Form)
            {
                parameters[param.Key] = param.Value.ToString();
            }
        }

        // KS_Tahsilat callback model'ini oluştur
        var callbackModel = new KS_Tahsilat_CallbackModel
        {
            TURKPOS_RETVAL_Sonuc = parameters.GetValueOrDefault("TURKPOS_RETVAL_Sonuc"),
            TURKPOS_RETVAL_Sonuc_Str = parameters.GetValueOrDefault("TURKPOS_RETVAL_Sonuc_Str"),
            TURKPOS_RETVAL_GUID = parameters.GetValueOrDefault("TURKPOS_RETVAL_GUID"),
            TURKPOS_RETVAL_Islem_Tarih = parameters.GetValueOrDefault("TURKPOS_RETVAL_Islem_Tarih"),
            TURKPOS_RETVAL_Dekont_ID = parameters.GetValueOrDefault("TURKPOS_RETVAL_Dekont_ID"),
            TURKPOS_RETVAL_Tahsilat_Tutari = parameters.GetValueOrDefault("TURKPOS_RETVAL_Tahsilat_Tutari"),
            TURKPOS_RETVAL_Odeme_Tutari = parameters.GetValueOrDefault("TURKPOS_RETVAL_Odeme_Tutari"),
            TURKPOS_RETVAL_Siparis_ID = parameters.GetValueOrDefault("TURKPOS_RETVAL_Siparis_ID"),
            TURKPOS_RETVAL_Islem_ID = parameters.GetValueOrDefault("TURKPOS_RETVAL_Islem_ID"),
            TURKPOS_RETVAL_Ext_Data = parameters.GetValueOrDefault("TURKPOS_RETVAL_Ext_Data")
        };

        _logger.LogInformation("KS_Tahsilat callback parametreleri - Sonuç: {Sonuc}, Dekont_ID: {DekontId}, Sipariş_ID: {SiparisId}", 
            callbackModel.TURKPOS_RETVAL_Sonuc, 
            callbackModel.TURKPOS_RETVAL_Dekont_ID, 
            callbackModel.TURKPOS_RETVAL_Siparis_ID);

        // KS_Tahsilat özel success sayfasına git
        TempData["KS_Tahsilat_Callback"] = System.Text.Json.JsonSerializer.Serialize(callbackModel);
        ViewBag.Parameters = parameters;
        
        return View("KS_Tahsilat_Success");
    }

    #endregion

    #region TP_Islem_Odeme_WD İşlemleri

    /// <summary>
    /// TP_Islem_Odeme_WD test formu
    /// </summary>
    [HttpGet]
    public IActionResult TP_Islem_Odeme_WD()
    {
        var model = new TP_Islem_Odeme_WD_TestModel();
        return View(model);
    }

    /// <summary>
    /// TP_Islem_Odeme_WD test işlemi
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TP_Islem_Odeme_WD(TP_Islem_Odeme_WD_TestModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            // Test model'ini SDK request'ine dönüştür
            var request = new TP_Islem_Odeme_WD_Request
            {
                Doviz_Kodu = model.Doviz_Kodu,
                KK_Sahibi = model.KK_Sahibi,
                KK_No = model.KK_No?.Replace(" ", "").Replace("-", ""), // Boşluk ve tire temizle
                KK_SK_Ay = model.KK_SK_Ay.ToString("D2"),
                KK_SK_Yil = model.KK_SK_Yil,
                KK_CVC = model.KK_CVC,
                KK_Sahibi_GSM = model.KK_Sahibi_GSM,
                Hata_URL = model.Hata_URL,
                Basarili_URL = model.Basarili_URL,
                Siparis_ID = model.Siparis_ID,
                Siparis_Aciklama = model.Siparis_Aciklama,
                Islem_Tutar = model.Islem_Tutar,
                Toplam_Tutar = model.Toplam_Tutar,
                Islem_Guvenlik_Tip = model.Islem_Guvenlik_Tip,
                IPAdr = model.IPAdr,
                Islem_ID = model.Islem_ID,
                Ref_URL = model.Ref_URL,
                Data1 = model.Data1,
                Data2 = model.Data2,
                Data3 = model.Data3,
                Data4 = model.Data4,
                Data5 = model.Data5
            };

            // SDK üzerinden API çağrısı yap
            var response = await _turkposService.TP_Islem_Odeme_WDAsync(request);
            
            // Debug bilgileri
            _logger.LogInformation("TP_Islem_Odeme_WD Response - Sonuç: {Sonuc}, UCD_URL: {UcdUrl}, Is3DSecure: {Is3DSecure}, IsNonSecure: {IsNonSecure}", 
                response.Sonuc, 
                response.UCD_URL?.Substring(0, Math.Min(50, response.UCD_URL?.Length ?? 0)) + "...",
                response.Is3DSecure,
                response.IsNonSecure);

            // 3D Secure işlemi kontrolü
            if (response.IsSuccess && response.Is3DSecure)
            {
                // 3D Secure gerekli - TempData'ya bilgileri kaydet
                TempData["ThreeDHtml"] = response.UCD_URL;
                TempData["OrderId"] = model.Siparis_ID;
                TempData["Amount"] = model.Islem_Tutar.ToString("F2");
                TempData["IsSecure"] = true;
                TempData["IslemID"] = response.Islem_ID;
                TempData["MethodType"] = "TP_Islem_Odeme_WD";
                
                _logger.LogInformation("3D Secure işlemi başlatıldı: {SiparisId}", model.Siparis_ID);
            }
            else
            {
                // NonSecure işlem veya hata
                TempData["IsSecure"] = false;
                if (response.IsSuccess)
                {
                    _logger.LogInformation("NonSecure dövizli ödeme tamamlandı: {SiparisId}", model.Siparis_ID);
                }
                else
                {
                    _logger.LogWarning("Dövizli ödeme işlemi başarısız: {SiparisId}, Hata: {Hata}", model.Siparis_ID, response.Sonuc_Str);
                }
            }

            // Sonuç model'ini oluştur
            var resultModel = new TP_Islem_Odeme_WD_ResultModel
            {
                IsSuccess = response.IsSuccess,
                Sonuc = response.Sonuc,
                Sonuc_Str = response.Sonuc_Str,
                UCD_URL = response.UCD_URL,
                Islem_ID = response.Islem_ID,
                Banka_Sonuc_Kod = response.Banka_Sonuc_Kod,
                Siparis_ID = model.Siparis_ID,
                RequestData = model,
                DecodedHtml = response.Is3DSecure ? DecodeHtml(response.UCD_URL) : null
            };

            if (!response.IsSuccess)
            {
                resultModel.ErrorMessage = response.Sonuc_Str;
            }

            _logger.LogInformation("TP_Islem_Odeme_WD test sonucu: {SiparisId}, Başarılı: {Basarili}", model.Siparis_ID, response.IsSuccess);

            return View("TP_Islem_Odeme_WD_Result", resultModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "TP_Islem_Odeme_WD test hatası: {SiparisId}", model.Siparis_ID);
            
            var errorModel = new TP_Islem_Odeme_WD_ResultModel
            {
                IsSuccess = false,
                ErrorMessage = $"Test hatası: {ex.Message}",
                Siparis_ID = model.Siparis_ID,
                RequestData = model
            };
            
            return View("TP_Islem_Odeme_WD_Result", errorModel);
        }
    }

    /// <summary>
    /// TP_Islem_Odeme_WD başarılı callback'i - 3D Secure sonrası
    /// </summary>
    [HttpGet]
    [HttpPost]
    public IActionResult TP_Islem_Odeme_WD_Success()
    {
        _logger.LogInformation("TP_Islem_Odeme_WD başarılı callback'i alındı");
        
        // Query string ve POST parametrelerini al
        var parameters = new Dictionary<string, string>();
        
        // Query string parametrelerini ekle
        foreach (var param in Request.Query)
        {
            parameters[param.Key] = param.Value.ToString();
        }
        
        // POST parametrelerini ekle (varsa)
        if (Request.Method == "POST" && Request.HasFormContentType)
        {
            foreach (var param in Request.Form)
            {
                parameters[param.Key] = param.Value.ToString();
            }
        }

        // TP_Islem_Odeme_WD callback model'ini oluştur
        var callbackModel = new TP_Islem_Odeme_WD_CallbackModel
        {
            TURKPOS_RETVAL_Sonuc = parameters.GetValueOrDefault("TURKPOS_RETVAL_Sonuc"),
            TURKPOS_RETVAL_Sonuc_Str = parameters.GetValueOrDefault("TURKPOS_RETVAL_Sonuc_Str"),
            TURKPOS_RETVAL_GUID = parameters.GetValueOrDefault("TURKPOS_RETVAL_GUID"),
            TURKPOS_RETVAL_Islem_Tarih = parameters.GetValueOrDefault("TURKPOS_RETVAL_Islem_Tarih"),
            TURKPOS_RETVAL_Dekont_ID = parameters.GetValueOrDefault("TURKPOS_RETVAL_Dekont_ID"),
            TURKPOS_RETVAL_Tahsilat_Tutari = parameters.GetValueOrDefault("TURKPOS_RETVAL_Tahsilat_Tutari"),
            TURKPOS_RETVAL_Odeme_Tutari = parameters.GetValueOrDefault("TURKPOS_RETVAL_Odeme_Tutari"),
            TURKPOS_RETVAL_Siparis_ID = parameters.GetValueOrDefault("TURKPOS_RETVAL_Siparis_ID"),
            TURKPOS_RETVAL_Islem_ID = parameters.GetValueOrDefault("TURKPOS_RETVAL_Islem_ID"),
            TURKPOS_RETVAL_Ext_Data = parameters.GetValueOrDefault("TURKPOS_RETVAL_Ext_Data"),
            TURKPOS_RETVAL_Banka_Sonuc_Kod = parameters.GetValueOrDefault("TURKPOS_RETVAL_Banka_Sonuc_Kod"),
            TURKPOS_RETVAL_KK_No = parameters.GetValueOrDefault("TURKPOS_RETVAL_KK_No")
        };

        _logger.LogInformation("TP_Islem_Odeme_WD callback parametreleri - Sonuç: {Sonuc}, Dekont_ID: {DekontId}, Sipariş_ID: {SiparisId}", 
            callbackModel.TURKPOS_RETVAL_Sonuc, 
            callbackModel.TURKPOS_RETVAL_Dekont_ID, 
            callbackModel.TURKPOS_RETVAL_Siparis_ID);

        // TP_Islem_Odeme_WD özel success sayfasına git
        TempData["TP_Islem_Odeme_WD_Callback"] = System.Text.Json.JsonSerializer.Serialize(callbackModel);
        ViewBag.Parameters = parameters;
        
        return View("TP_Islem_Odeme_WD_Success");
    }

    #endregion

    #region TP_Islem_Odeme_BKM İşlemleri

    /// <summary>
    /// TP_Islem_Odeme_BKM test formu
    /// </summary>
    [HttpGet]
    public IActionResult TP_Islem_Odeme_BKM()
    {
        var model = new TP_Islem_Odeme_BKM_TestModel();
        return View(model);
    }

    /// <summary>
    /// TP_Islem_Odeme_BKM test işlemi
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TP_Islem_Odeme_BKM(TP_Islem_Odeme_BKM_TestModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            // Test model'ini SDK request'ine dönüştür
            var request = new TP_Islem_Odeme_BKM_Request
            {
                Customer_Info = model.Customer_Info,
                Customer_GSM = model.Customer_GSM,
                Error_URL = model.Error_URL,
                Success_URL = model.Success_URL,
                Order_ID = model.Order_ID,
                Order_Description = model.Order_Description,
                Amount = model.Amount,
                Transaction_ID = model.Transaction_ID,
                IPAddress = model.IPAddress,
                Referrer_URL = model.Referrer_URL
            };

            // SDK üzerinden API çağrısı yap
            var response = await _turkposService.TP_Islem_Odeme_BKMAsync(request);
            
            // Debug bilgileri
            _logger.LogInformation("TP_Islem_Odeme_BKM Response - Response_Code: {ResponseCode}, Response_Message: {ResponseMessage}, Redirect_URL: {RedirectUrl}", 
                response.Response_Code, 
                response.Response_Message, 
                response.Redirect_URL?.Substring(0, Math.Min(50, response.Redirect_URL?.Length ?? 0)) + "...");

            // BKM Express işlemi kontrolü
            if (response.IsSuccess && !string.IsNullOrEmpty(response.Redirect_URL))
            {
                // BKM Express yönlendirme gerekli - TempData'ya bilgileri kaydet
                TempData["BkmRedirectUrl"] = response.Redirect_URL;
                TempData["OrderId"] = model.Order_ID;
                TempData["Amount"] = model.Amount.ToString("F2");
                TempData["IsBkmExpress"] = true;
                TempData["MethodType"] = "TP_Islem_Odeme_BKM";
                
                _logger.LogInformation("BKM Express işlemi başlatıldı: {OrderId}", model.Order_ID);
            }
            else
            {
                // Hata durumu
                _logger.LogWarning("BKM Express işlemi başarısız: {OrderId}, Hata: {Hata}", model.Order_ID, response.Response_Message);
            }

            // Sonuç model'ini oluştur
            var resultModel = new TP_Islem_Odeme_BKM_ResultModel
            {
                IsSuccess = response.IsSuccess,
                Response_Code = response.Response_Code,
                Response_Message = response.Response_Message,
                Redirect_URL = response.Redirect_URL,
                Order_ID = model.Order_ID,
                RequestData = model
            };

            if (!response.IsSuccess)
            {
                resultModel.ErrorMessage = response.Response_Message;
            }

            _logger.LogInformation("TP_Islem_Odeme_BKM test sonucu: {OrderId}, Başarılı: {Basarili}", model.Order_ID, response.IsSuccess);

            return View("TP_Islem_Odeme_BKM_Result", resultModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "TP_Islem_Odeme_BKM test hatası: {OrderId}", model.Order_ID);
            
            var errorModel = new TP_Islem_Odeme_BKM_ResultModel
            {
                IsSuccess = false,
                ErrorMessage = $"Test hatası: {ex.Message}",
                Order_ID = model.Order_ID,
                RequestData = model
            };
            
            return View("TP_Islem_Odeme_BKM_Result", errorModel);
        }
    }

    /// <summary>
    /// TP_Islem_Odeme_BKM başarılı callback'i - BKM Express sonrası
    /// </summary>
    [HttpGet]
    [HttpPost]
    public IActionResult TP_Islem_Odeme_BKM_Success()
    {
        _logger.LogInformation("TP_Islem_Odeme_BKM başarılı callback'i alındı");
        
        // Query string ve POST parametrelerini al
        var parameters = new Dictionary<string, string>();
        
        // Query string parametrelerini ekle
        foreach (var param in Request.Query)
        {
            parameters[param.Key] = param.Value.ToString();
        }
        
        // POST parametrelerini ekle (varsa)
        if (Request.Method == "POST" && Request.HasFormContentType)
        {
            foreach (var param in Request.Form)
            {
                parameters[param.Key] = param.Value.ToString();
            }
        }

        // TP_Islem_Odeme_BKM callback model'ini oluştur
        var callbackModel = new TP_Islem_Odeme_BKM_CallbackModel
        {
            TURKPOS_RETVAL_Sonuc = parameters.GetValueOrDefault("TURKPOS_RETVAL_Sonuc"),
            TURKPOS_RETVAL_Sonuc_Str = parameters.GetValueOrDefault("TURKPOS_RETVAL_Sonuc_Str"),
            TURKPOS_RETVAL_GUID = parameters.GetValueOrDefault("TURKPOS_RETVAL_GUID"),
            TURKPOS_RETVAL_Islem_Tarih = parameters.GetValueOrDefault("TURKPOS_RETVAL_Islem_Tarih"),
            TURKPOS_RETVAL_Dekont_ID = parameters.GetValueOrDefault("TURKPOS_RETVAL_Dekont_ID"),
            TURKPOS_RETVAL_Tahsilat_Tutari = parameters.GetValueOrDefault("TURKPOS_RETVAL_Tahsilat_Tutari"),
            TURKPOS_RETVAL_Odeme_Tutari = parameters.GetValueOrDefault("TURKPOS_RETVAL_Odeme_Tutari"),
            TURKPOS_RETVAL_Siparis_ID = parameters.GetValueOrDefault("TURKPOS_RETVAL_Siparis_ID"),
            TURKPOS_RETVAL_Islem_ID = parameters.GetValueOrDefault("TURKPOS_RETVAL_Islem_ID"),
            TURKPOS_RETVAL_Banka_Sonuc_Kod = parameters.GetValueOrDefault("TURKPOS_RETVAL_Banka_Sonuc_Kod")
        };

        _logger.LogInformation("TP_Islem_Odeme_BKM callback parametreleri - Sonuç: {Sonuc}, Dekont_ID: {DekontId}, Sipariş_ID: {SiparisId}", 
            callbackModel.TURKPOS_RETVAL_Sonuc, 
            callbackModel.TURKPOS_RETVAL_Dekont_ID, 
            callbackModel.TURKPOS_RETVAL_Siparis_ID);

        // TP_Islem_Odeme_BKM özel success sayfasına git
        TempData["TP_Islem_Odeme_BKM_Callback"] = System.Text.Json.JsonSerializer.Serialize(callbackModel);
        ViewBag.Parameters = parameters;
        
        return View("TP_Islem_Odeme_BKM_Success");
    }

    #endregion

    #region TP_Islem_Iptal_Iade_Kismi2 - Kısmi İptal/İade İşlemi

    /// <summary>
    /// TP_Islem_Iptal_Iade_Kismi2 test sayfası (GET)
    /// </summary>
    [HttpGet]
    public IActionResult TP_Islem_Iptal_Iade_Kismi2()
    {
        var model = new TP_Islem_Iptal_Iade_Kismi2_TestModel();
        return View(model);
    }

    /// <summary>
    /// TP_Islem_Iptal_Iade_Kismi2 işlem (POST)
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TP_Islem_Iptal_Iade_Kismi2(TP_Islem_Iptal_Iade_Kismi2_TestModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var stopwatch = Stopwatch.StartNew();
        var resultModel = new TP_Islem_Iptal_Iade_Kismi2_ResultModel
        {
            Request = model
        };

        try
        {
            // Request oluştur
            var request = new TP_Islem_Iptal_Iade_Kismi2_Request
            {
                Durum = model.Durum,
                Siparis_ID = model.Siparis_ID,
                Tutar = model.Tutar
            };

            // API çağrısı yap
            var response = await _turkposService.TP_Islem_Iptal_Iade_Kismi2Async(request);
            
            stopwatch.Stop();
            resultModel.Response = response;
            resultModel.ProcessDuration = stopwatch.ElapsedMilliseconds;

            _logger.LogInformation("TP_Islem_Iptal_Iade_Kismi2 işlemi tamamlandı - Sipariş ID: {SiparisId}, Durum: {Durum}, Sonuç: {Sonuc}", 
                model.Siparis_ID, model.Durum, response.Sonuc);

            return View("TP_Islem_Iptal_Iade_Kismi2_Result", resultModel);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            resultModel.ProcessDuration = stopwatch.ElapsedMilliseconds;
            
            _logger.LogError(ex, "TP_Islem_Iptal_Iade_Kismi2 hatası - Sipariş ID: {SiparisId}", model.Siparis_ID);

            // Hata durumunda mock response oluştur - reflection kullanarak private constructor'ı çağır
            var responseType = typeof(TP_Islem_Iptal_Iade_Kismi2_Response);
            var mockParamResponse = new TurkposService.ST_Sonuc_II
            {
                Sonuc = "0",
                Sonuc_Str = $"API Hatası: {ex.Message}",
                Banka_Sonuc_Kod = "ERROR"
            };
            
            var mockResponse = (TP_Islem_Iptal_Iade_Kismi2_Response)Activator.CreateInstance(
                responseType, 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
                null,
                new object[] { mockParamResponse },
                null)!;

            resultModel.Response = mockResponse;

            return View("TP_Islem_Iptal_Iade_Kismi2_Result", resultModel);
        }
    }

    #endregion

    #region TP_Ozel_Oran - Özel Oran İşlemleri

    /// <summary>
    /// TP_Ozel_Oran test sayfası (GET)
    /// </summary>
    [HttpGet]
    public IActionResult TP_Ozel_Oran()
    {
        var model = new TP_Ozel_Oran_TestModel();
        return View(model);
    }

    /// <summary>
    /// TP_Ozel_Oran işlem (POST)
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TP_Ozel_Oran(TP_Ozel_Oran_TestModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var stopwatch = Stopwatch.StartNew();
        var resultModel = new TP_Ozel_Oran_ResultModel
        {
            TestTipi = model.TestTipi,
            Request = model
        };

        try
        {
            switch (model.TestTipi)
            {
                case OzelOranTestTipi.Liste:
                    // TP_Ozel_Oran_Liste çağrısı
                    var listeResponse = await _turkposService.TP_Ozel_Oran_ListeAsync();
                    resultModel.ListeResponse = listeResponse;
                    
                    _logger.LogInformation("TP_Ozel_Oran_Liste işlemi tamamlandı - Sonuç: {Sonuc}, Özel Oran Sayısı: {Sayı}", 
                        listeResponse.Sonuc, listeResponse.OzelOranBilgileri.Count);
                    break;

                case OzelOranTestTipi.SK_Liste:
                    // TP_Ozel_Oran_SK_Liste çağrısı
                    var skListeResponse = await _turkposService.TP_Ozel_Oran_SK_ListeAsync();
                    resultModel.SK_ListeResponse = skListeResponse;
                    
                    _logger.LogInformation("TP_Ozel_Oran_SK_Liste işlemi tamamlandı - Sonuç: {Sonuc}, Özel Oran SK Sayısı: {Sayı}", 
                        skListeResponse.Sonuc, skListeResponse.OzelOranSKBilgileri.Count);
                    break;

                case OzelOranTestTipi.SK_Guncelle:
                    // TP_Ozel_Oran_SK_Guncelle çağrısı
                    if (model.GuncelleModel == null)
                    {
                        ModelState.AddModelError("", "Güncelleme bilgileri eksik");
                        return View(model);
                    }

                    var request = model.GuncelleModel.ToRequest();
                    var guncelleResponse = await _turkposService.TP_Ozel_Oran_SK_GuncelleAsync(request);
                    resultModel.SK_GuncelleResponse = guncelleResponse;
                    
                    _logger.LogInformation("TP_Ozel_Oran_SK_Guncelle işlemi tamamlandı - SK ID: {SkId}, Sonuç: {Sonuc}", 
                        model.GuncelleModel.Ozel_Oran_SK_ID, guncelleResponse.Sonuc);
                    break;

                default:
                    throw new ArgumentException("Geçersiz test tipi", nameof(model.TestTipi));
            }
            
            stopwatch.Stop();
            resultModel.ProcessDuration = stopwatch.ElapsedMilliseconds;

            return View("TP_Ozel_Oran_Result", resultModel);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            resultModel.ProcessDuration = stopwatch.ElapsedMilliseconds;
            
            _logger.LogError(ex, "TP_Ozel_Oran {TestTipi} hatası", model.TestTipi);

            // Hata durumunda mock response oluştur
            switch (model.TestTipi)
            {
                case OzelOranTestTipi.Liste:
                    var mockListeResponse = new TurkposService.ST_Genel_Sonuc
                    {
                        Sonuc = "0",
                        Sonuc_Str = $"API Hatası: {ex.Message}"
                    };
                    resultModel.ListeResponse = new TP_Ozel_Oran_Liste_Response(mockListeResponse);
                    break;

                case OzelOranTestTipi.SK_Liste:
                    var mockSKListeResponse = new TurkposService.ST_Genel_Sonuc
                    {
                        Sonuc = "0",
                        Sonuc_Str = $"API Hatası: {ex.Message}"
                    };
                    resultModel.SK_ListeResponse = new TP_Ozel_Oran_SK_Liste_Response(mockSKListeResponse);
                    break;

                case OzelOranTestTipi.SK_Guncelle:
                    resultModel.SK_GuncelleResponse = TP_Ozel_Oran_SK_Guncelle_Response.CreateError($"API Hatası: {ex.Message}");
                    break;
            }

            return View("TP_Ozel_Oran_Result", resultModel);
        }
    }

    #endregion

    #region TP_Ozel_Oran_Liste - Özel Oran Listesi

    /// <summary>
    /// TP_Ozel_Oran_Liste test sayfası (GET)
    /// </summary>
    [HttpGet]
    public IActionResult TP_Ozel_Oran_Liste()
    {
        var model = new TP_Ozel_Oran_Liste_TestModel();
        return View(model);
    }

    /// <summary>
    /// TP_Ozel_Oran_Liste işlem (POST)
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TP_Ozel_Oran_Liste(TP_Ozel_Oran_Liste_TestModel model)
    {
        var stopwatch = Stopwatch.StartNew();
        var resultModel = new TP_Ozel_Oran_Liste_ResultModel();

        try
        {
            var response = await _turkposService.TP_Ozel_Oran_ListeAsync();
            
            stopwatch.Stop();
            resultModel.Response = response;
            resultModel.ProcessDuration = stopwatch.ElapsedMilliseconds;

            _logger.LogInformation("TP_Ozel_Oran_Liste işlemi tamamlandı - Sonuç: {Sonuc}, Özel Oran Sayısı: {Sayı}", 
                response.Sonuc, response.OzelOranBilgileri.Count);

            return View("TP_Ozel_Oran_Liste_Result", resultModel);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            resultModel.ProcessDuration = stopwatch.ElapsedMilliseconds;
            
            _logger.LogError(ex, "TP_Ozel_Oran_Liste hatası");

            // Hata durumunda mock response oluştur
            var mockResponse = new TurkposService.ST_Genel_Sonuc
            {
                Sonuc = "0",
                Sonuc_Str = $"API Hatası: {ex.Message}"
            };
            resultModel.Response = new TP_Ozel_Oran_Liste_Response(mockResponse);

            return View("TP_Ozel_Oran_Liste_Result", resultModel);
        }
    }

    #endregion

    #region TP_Ozel_Oran_SK_Liste - Son Kullanıcı Özel Oran Listesi

    /// <summary>
    /// TP_Ozel_Oran_SK_Liste test sayfası (GET)
    /// </summary>
    [HttpGet]
    public IActionResult TP_Ozel_Oran_SK_Liste()
    {
        var model = new TP_Ozel_Oran_SK_Liste_TestModel();
        return View(model);
    }

    /// <summary>
    /// TP_Ozel_Oran_SK_Liste işlem (POST)
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TP_Ozel_Oran_SK_Liste(TP_Ozel_Oran_SK_Liste_TestModel model)
    {
        var stopwatch = Stopwatch.StartNew();
        var resultModel = new TP_Ozel_Oran_SK_Liste_ResultModel();

        try
        {
            var response = await _turkposService.TP_Ozel_Oran_SK_ListeAsync();
            
            stopwatch.Stop();
            resultModel.Response = response;
            resultModel.ProcessDuration = stopwatch.ElapsedMilliseconds;

            _logger.LogInformation("TP_Ozel_Oran_SK_Liste işlemi tamamlandı - Sonuç: {Sonuc}, SK Oran Sayısı: {Sayı}", 
                response.Sonuc, response.OzelOranSKBilgileri.Count);

            return View("TP_Ozel_Oran_SK_Liste_Result", resultModel);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            resultModel.ProcessDuration = stopwatch.ElapsedMilliseconds;
            
            _logger.LogError(ex, "TP_Ozel_Oran_SK_Liste hatası");

            // Hata durumunda mock response oluştur
            var mockResponse = new TurkposService.ST_Genel_Sonuc
            {
                Sonuc = "0",
                Sonuc_Str = $"API Hatası: {ex.Message}"
            };
            resultModel.Response = new TP_Ozel_Oran_SK_Liste_Response(mockResponse);

            return View("TP_Ozel_Oran_SK_Liste_Result", resultModel);
        }
    }

    #endregion

    #region TP_Ozel_Oran_SK_Guncelle - Son Kullanıcı Özel Oran Güncelleme

    /// <summary>
    /// TP_Ozel_Oran_SK_Guncelle test sayfası (GET)
    /// </summary>
    [HttpGet]
    public IActionResult TP_Ozel_Oran_SK_Guncelle()
    {
        var model = new TP_Ozel_Oran_SK_Guncelle_TestModel();
        return View(model);
    }

    /// <summary>
    /// TP_Ozel_Oran_SK_Guncelle işlem (POST)
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TP_Ozel_Oran_SK_Guncelle(TP_Ozel_Oran_SK_Guncelle_TestModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var stopwatch = Stopwatch.StartNew();
        var resultModel = new TP_Ozel_Oran_SK_Guncelle_ResultModel
        {
            Request = model
        };

        try
        {
            var request = model.ToRequest();
            var response = await _turkposService.TP_Ozel_Oran_SK_GuncelleAsync(request);
            
            stopwatch.Stop();
            resultModel.Response = response;
            resultModel.ProcessDuration = stopwatch.ElapsedMilliseconds;

            _logger.LogInformation("TP_Ozel_Oran_SK_Guncelle işlemi tamamlandı - SK ID: {SkId}, Sonuç: {Sonuc}", 
                model.Ozel_Oran_SK_ID, response.Sonuc);

            return View("TP_Ozel_Oran_SK_Guncelle_Result", resultModel);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            resultModel.ProcessDuration = stopwatch.ElapsedMilliseconds;
            
            _logger.LogError(ex, "TP_Ozel_Oran_SK_Guncelle hatası - SK ID: {SkId}", model.Ozel_Oran_SK_ID);

            // Hata durumunda mock response oluştur
            resultModel.Response = TP_Ozel_Oran_SK_Guncelle_Response.CreateError($"API Hatası: {ex.Message}");

            return View("TP_Ozel_Oran_SK_Guncelle_Result", resultModel);
        }
    }

    #endregion

    #region TP_Mutabakat_Ozet - Mutabakat Özet Sorgulama

    /// <summary>
    /// TP_Mutabakat_Ozet test sayfası (GET)
    /// </summary>
    [HttpGet]
    public IActionResult TP_Mutabakat_Ozet()
    {
        var model = new TP_Mutabakat_Ozet_TestModel();
        return View(model);
    }

    /// <summary>
    /// TP_Mutabakat_Ozet işlem (POST)
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TP_Mutabakat_Ozet(TP_Mutabakat_Ozet_TestModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var stopwatch = Stopwatch.StartNew();
        var resultModel = new TP_Mutabakat_Ozet_ResultModel
        {
            Request = model
        };

        try
        {
            var request = model.ToRequest(_turkposService.GetType().GetProperty("Guid")?.GetValue(_turkposService)?.ToString() ?? "");
            var response = await _turkposService.TP_Mutabakat_OzetAsync(request);
            
            stopwatch.Stop();
            resultModel.Response = response;
            resultModel.ProcessDuration = stopwatch.ElapsedMilliseconds;

            _logger.LogInformation("TP_Mutabakat_Ozet işlemi tamamlandı - Tarih: {TarihBas} - {TarihBit}, Sonuç: {Sonuc}", 
                model.Tarih_Bas, model.Tarih_Bit, response.Sonuc);

            return View("TP_Mutabakat_Ozet_Result", resultModel);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            resultModel.ProcessDuration = stopwatch.ElapsedMilliseconds;
            
            _logger.LogError(ex, "TP_Mutabakat_Ozet hatası - Tarih: {TarihBas} - {TarihBit}", model.Tarih_Bas, model.Tarih_Bit);

            // Hata durumunda mock response oluştur
            resultModel.Response = new TP_Mutabakat_Ozet_Response
            {
                Sonuc = "0",
                Sonuc_Str = $"API Hatası: {ex.Message}"
            };

            return View("TP_Mutabakat_Ozet_Result", resultModel);
        }
    }

    #endregion

    #region TP_Mutabakat_Detay - Mutabakat Detay Sorgulama

    /// <summary>
    /// TP_Mutabakat_Detay test sayfası (GET)
    /// </summary>
    [HttpGet]
    public IActionResult TP_Mutabakat_Detay()
    {
        var model = new TP_Mutabakat_Detay_TestModel();
        return View(model);
    }

    /// <summary>
    /// TP_Mutabakat_Detay işlem (POST)
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TP_Mutabakat_Detay(TP_Mutabakat_Detay_TestModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var stopwatch = Stopwatch.StartNew();
        var resultModel = new TP_Mutabakat_Detay_ResultModel
        {
            Request = model
        };

        try
        {
            var request = model.ToRequest();
            var response = await _turkposService.TP_Mutabakat_DetayAsync(request);
            
            stopwatch.Stop();
            resultModel.Response = response;
            resultModel.ProcessDuration = stopwatch.ElapsedMilliseconds;

            _logger.LogInformation("TP_Mutabakat_Detay işlemi tamamlandı - Tarih: {Tarih}, Sonuç: {Sonuc}", 
                model.Tarih, response.Sonuc);

            return View("TP_Mutabakat_Detay_Result", resultModel);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            resultModel.ProcessDuration = stopwatch.ElapsedMilliseconds;
            
            _logger.LogError(ex, "TP_Mutabakat_Detay hatası - Tarih: {Tarih}", model.Tarih);

            // Hata durumunda mock response oluştur
            resultModel.Response = new TP_Mutabakat_Detay_Response
            {
                Sonuc = "0",
                Sonuc_Str = $"API Hatası: {ex.Message}"
            };

            return View("TP_Mutabakat_Detay_Result", resultModel);
        }
    }

    #endregion

    #region TP_Islem_Sorgulama4 - İşlem Durumu Sorgulama

    /// <summary>
    /// TP_Islem_Sorgulama4 test sayfası (GET)
    /// </summary>
    [HttpGet]
    public IActionResult TP_Islem_Sorgulama4()
    {
        var model = new TP_Islem_Sorgulama4_TestModel();
        return View(model);
    }

    /// <summary>
    /// TP_Islem_Sorgulama4 işlem (POST)
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TP_Islem_Sorgulama4(TP_Islem_Sorgulama4_TestModel model)
    {
        if (!ModelState.IsValid || !model.HasValidInput)
        {
            if (!model.HasValidInput)
            {
                ModelState.AddModelError("", "En az bir parametre girilmelidir: Dekont ID, Sipariş ID veya İşlem ID");
            }
            return View(model);
        }

        var stopwatch = Stopwatch.StartNew();
        var resultModel = new TP_Islem_Sorgulama4_ResultModel
        {
            Request = model
        };

        try
        {
            var request = model.ToRequest();
            var response = await _turkposService.TP_Islem_Sorgulama4Async(request);
            
            stopwatch.Stop();
            resultModel.Response = response;
            resultModel.ProcessDuration = stopwatch.ElapsedMilliseconds;

            _logger.LogInformation("TP_Islem_Sorgulama4 işlemi tamamlandı - Dekont: {DecontId}, Sipariş: {SiparisId}, İşlem: {IslemId}, Sonuç: {Sonuc}", 
                model.Dekont_ID, model.Siparis_ID, model.Islem_ID, response.Sonuc);

            return View("TP_Islem_Sorgulama4_Result", resultModel);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            resultModel.ProcessDuration = stopwatch.ElapsedMilliseconds;
            
            _logger.LogError(ex, "TP_Islem_Sorgulama4 hatası - Dekont: {DecontId}, Sipariş: {SiparisId}, İşlem: {IslemId}", 
                model.Dekont_ID, model.Siparis_ID, model.Islem_ID);

            // Hata durumunda mock response oluştur
            resultModel.Response = new TP_Islem_Sorgulama4_Response
            {
                Sonuc = 0,
                Sonuc_Str = $"API Hatası: {ex.Message}"
            };

            return View("TP_Islem_Sorgulama4_Result", resultModel);
        }
    }

    #endregion

    #region TP_Islem_Izleme - İşlem İzleme

    /// <summary>
    /// TP_Islem_Izleme test sayfası (GET)
    /// </summary>
    [HttpGet]
    public IActionResult TP_Islem_Izleme()
    {
        var model = new TP_Islem_Izleme_TestModel();
        return View(model);
    }

    /// <summary>
    /// TP_Islem_Izleme işlem (POST)
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TP_Islem_Izleme(TP_Islem_Izleme_TestModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var stopwatch = Stopwatch.StartNew();
        var resultModel = new TP_Islem_Izleme_ResultModel
        {
            Request = model
        };

        try
        {
            var request = model.ToRequest();
            var response = await _turkposService.TP_Islem_IzlemeAsync(request);
            
            stopwatch.Stop();
            resultModel.Response = response;
            resultModel.ProcessDuration = stopwatch.ElapsedMilliseconds;

            _logger.LogInformation("TP_Islem_Izleme işlemi tamamlandı - Tarih: {TarihBas} - {TarihBit}, Tip: {IslemTip}, Durum: {IslemDurum}, Sonuç: {Sonuc}", 
                model.Tarih_Bas, model.Tarih_Bit, model.Islem_Tip, model.Islem_Durum, response.Sonuc);

            return View("TP_Islem_Izleme_Result", resultModel);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            resultModel.ProcessDuration = stopwatch.ElapsedMilliseconds;
            
            _logger.LogError(ex, "TP_Islem_Izleme hatası - Tarih: {TarihBas} - {TarihBit}", model.Tarih_Bas, model.Tarih_Bit);

            // Hata durumunda mock response oluştur
            resultModel.Response = new TP_Islem_Izleme_Response
            {
                Sonuc = 0,
                Sonuc_Str = $"API Hatası: {ex.Message}"
            };

            return View("TP_Islem_Izleme_Result", resultModel);
        }
    }

    #endregion

    #region TP_Islem_Dekont_Gonder - Dekont E-posta Gönderimi

    /// <summary>
    /// TP_Islem_Dekont_Gonder test sayfası (GET)
    /// </summary>
    [HttpGet]
    public IActionResult TP_Islem_Dekont_Gonder()
    {
        var model = new TP_Islem_Dekont_Gonder_TestModel();
        return View(model);
    }

    /// <summary>
    /// TP_Islem_Dekont_Gonder işlem (POST)
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TP_Islem_Dekont_Gonder(TP_Islem_Dekont_Gonder_TestModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var stopwatch = Stopwatch.StartNew();
        var resultModel = new TP_Islem_Dekont_Gonder_ResultModel
        {
            Request = model
        };

        try
        {
            var request = model.ToRequest();
            var response = await _turkposService.TP_Islem_Dekont_GonderAsync(request);
            
            stopwatch.Stop();
            resultModel.Response = response;
            resultModel.ProcessDuration = stopwatch.ElapsedMilliseconds;

            _logger.LogInformation("TP_Islem_Dekont_Gonder işlemi tamamlandı - Dekont ID: {DecontId}, E-posta: {Email}, Sonuç: {Sonuc}", 
                model.Dekont_ID, model.E_Posta, response.Sonuc);

            return View("TP_Islem_Dekont_Gonder_Result", resultModel);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            resultModel.ProcessDuration = stopwatch.ElapsedMilliseconds;
            
            _logger.LogError(ex, "TP_Islem_Dekont_Gonder hatası - Dekont ID: {DecontId}", model.Dekont_ID);

            // Hata durumunda mock response oluştur
            resultModel.Response = new TP_Islem_Dekont_Gonder_Response
            {
                Sonuc = 0,
                Sonuc_Str = $"API Hatası: {ex.Message}"
            };

            return View("TP_Islem_Dekont_Gonder_Result", resultModel);
        }
    }

    #endregion

    #region BIN_SanalPos - BIN Sorgulama

    /// <summary>
    /// BIN_SanalPos test sayfası (GET)
    /// </summary>
    [HttpGet]
    public IActionResult BIN_SanalPos()
    {
        var model = new BIN_SanalPos_TestModel();
        return View(model);
    }

    /// <summary>
    /// BIN_SanalPos işlem (POST)
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BIN_SanalPos(BIN_SanalPos_TestModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var stopwatch = Stopwatch.StartNew();
        var resultModel = new BIN_SanalPos_ResultModel
        {
            Request = model
        };

        try
        {
            var request = model.ToRequest();
            var response = await _turkposService.BIN_SanalPosAsync(request);
            
            stopwatch.Stop();
            resultModel.Response = response;
            resultModel.ProcessDuration = stopwatch.ElapsedMilliseconds;

            _logger.LogInformation("BIN_SanalPos işlemi tamamlandı - BIN: {Bin}, Sonuç: {Sonuc}", 
                model.BIN ?? "Tümü", response.Sonuc);

            return View("BIN_SanalPos_Result", resultModel);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            resultModel.ProcessDuration = stopwatch.ElapsedMilliseconds;
            
            _logger.LogError(ex, "BIN_SanalPos hatası - BIN: {Bin}", model.BIN ?? "Tümü");

            // Hata durumunda mock response oluştur
            resultModel.Response = new BIN_SanalPos_Response
            {
                Sonuc = 0,
                Sonuc_Str = $"API Hatası: {ex.Message}"
            };

            return View("BIN_SanalPos_Result", resultModel);
        }
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// HTML içeriğindeki özel karakterleri decode eder
    /// </summary>
    private static string DecodeHtml(string htmlContent)
    {
        if (string.IsNullOrEmpty(htmlContent))
            return string.Empty;
            
        return System.Net.WebUtility.HtmlDecode(htmlContent)
            .Replace("&lt;", "<")
            .Replace("&gt;", ">")
            .Replace("&quot;", "\"")
            .Replace("&amp;", "&")
            .Replace("&#39;", "'")
            .Replace("&apos;", "'");
    }

    #endregion
} 