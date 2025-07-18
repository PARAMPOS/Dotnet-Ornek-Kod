# Param API SDK

.NET 8 için Param TurkPos XML servisine erişim sağlayan tip güvenli SDK.

## 🌐 PARAM API Dokümantasyonu

**Resmi API Dokümantasyonu:** https://dev.param.com.tr/tr/api

Param API, e-ticaret siteleri ve uzaktan ödeme gerektiren tüm platformlar için kapsamlı ödeme çözümleri sunar. ParamPOS sistemi, çevrimiçi ödemeleri hızlı, güvenli ve sorunsuz bir deneyime dönüştürür.

### 🔗 Servis Endpoint'leri

| Ortam | TurkPos Endpoint | KS Service Endpoint |
|-------|------------------|---------------------|
| **Test** | `https://testposws.param.com.tr/turkpos.ws/service_turkpos_prod.asmx` | `https://testposws.param.com.tr/out.ws/service_ks.asmx` |


### 🧪 Test Hesap Bilgileri

| Parametre | Değer |
|-----------|-------|
| CLIENT_CODE | 10738 |
| CLIENT_USERNAME | Test |
| CLIENT_PASSWORD | Test |
| GUID | 0c13d406-873b-403b-9c09-a5766840d98c |

## 🚀 Kullanım

### Temel Ödeme İşlemi

```csharp
// Ödeme isteği oluştur
var request = new TP_WMD_UCD_Request
{
    KK_Sahibi = "Test Kullanici",
    KK_No = "4506347011634997",
    KK_SK_Ay = "12",
    KK_SK_Yil = "2026",
    KK_CVC = "000",
    Siparis_ID = "ORDER_12345",
    Siparis_Aciklama = "Test ödeme",
    Islem_Tutar = 100.50m,
    Toplam_Tutar = 105.00m,
    Taksit = 1,
    Hata_URL = "https://example.com/error",
    Basarili_URL = "https://example.com/success",
    Islem_Guvenlik_Tip = 3, // 3D Güvenli
    IPAdr = "127.0.0.1"
};

// Ödeme işlemini gerçekleştir
var response = await turkposService.TP_WMD_UCDAsync(request);

if (response.IsSuccessful)
{
    if (response.IsNonSecure)
    {
        // NonSecure ödeme başarılı
        Console.WriteLine($"✅ Dekont No: {response.Islem_ID}");
    }
    else if (response.Is3DSecure)
    {
        // 3D işlem - Kullanıcıyı bankaya yönlendir
        Console.WriteLine("🔐 3D işlem - Banka sayfasına yönlendiriliyor");
    }
}
```

### Controller'da Kullanım

```csharp
[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly ITurkposService _turkposService;

    public PaymentController(ITurkposService turkposService)
    {
        _turkposService = turkposService;
    }

    [HttpPost("process")]
    public async Task<IActionResult> ProcessPayment(PaymentRequest request)
    {
        var result = await _turkposService.TP_WMD_UCDAsync(request);
        return Ok(result);
    }
}
```

## 🏗️ Bu Projenin Yapısı

```
src/ParamApi.Sdk/
├── Configuration/          # Ortam ve yapılandırma yönetimi
│   ├── ParamApiOptions.cs     # Ana config sınıfı
│   ├── ParamEnvironment.cs    # Test/Production enum
│   ├── DovizKodu.cs          # Döviz kodları
│   └── IptalIadeDurum.cs     # İptal/İade durumları
├── Services/               # Ana servis client'ları
│   ├── ITurkposService.cs     # TurkPos servis arayüzü
│   ├── TurkposService.cs      # TurkPos servis implementasyonu
│   ├── IKartService.cs        # Kart saklama servis arayüzü
│   └── KartService.cs         # Kart saklama implementasyonu
├── Models/                 # PARAM API model'leri
│   ├── Requests/              # Request sınıfları
│   ├── Responses/             # Response sınıfları
│   └── Shared/                # Ortak model'ler
├── Extensions/             # Dependency Injection
│   └── ServiceCollectionExtensions.cs
├── Helpers/               # Yardımcı sınıflar
│   └── HashHelper.cs          # SHA256 hash hesaplama
└── Connected Services/     # SOAP client'ları
    ├── TurkposService/        # TurkPos SOAP referansları
    └── KsService/             # KS SOAP referansları
```

## 🛠️ Services

### ITurkposService - Ana Ödeme Servisi

**Ödeme İşlemleri:**
- `TP_WMD_UCDAsync` - Ana ödeme metodu (3D/NonSecure)
- `TP_WMD_PAYAsync` - 3D işlem tamamlama
- `TP_Islem_Odeme_WDAsync` - Dövizli ödeme
- `TP_Islem_Odeme_BKMAsync` - BKM Express ödeme

**Ön Provizyon İşlemleri:**
- `TP_Islem_Odeme_OnProv_WMDAsync` - Ön provizyon başlatma
- `TP_Islem_Odeme_OnProv_KapaAsync` - Ön provizyon kapama
- `TP_Islem_Iptal_OnProvAsync` - Ön provizyon iptal

**İşlem Yönetimi:**
- `TP_Islem_Iptal_Iade_Kismi2Async` - Kısmi iptal/iade
- `TP_Islem_Sorgulama4Async` - İşlem durumu sorgulama
- `TP_Islem_IzlemeAsync` - İşlem izleme

**Raporlama:**
- `TP_Mutabakat_OzetAsync` - Mutabakat özet
- `TP_Mutabakat_DetayAsync` - Mutabakat detay
- `TP_Ozel_Oran_ListeAsync` - Özel oran listesi

**Diğer:**
- `BIN_SanalPosAsync` - BIN sorgulama
- `SHA2B64Async` - Hash hesaplama

### IKartService - Kart Saklama Servisi

**Kart Yönetimi:**
- `KS_Kart_EkleAsync` - Kredi kartı saklama
- `KS_Kart_ListeAsync` - Saklı kart listesi
- `KS_Kart_SilAsync` - Saklı kart silme

**Ödeme İşlemleri:**
- `KS_TahsilatAsync` - Saklı karttan tahsilat

**Test Desteği:**
- `GetTestCards` - Test kartları listesi

## 📦 SDK Nasıl Entegre Edilir

### 1. Dependency Injection Kaydı

#### Test Ortamı
```csharp
// Program.cs
builder.Services.AddParamApiClient(ParamEnvironment.Test, options =>
{
    options.ClientCode = "10738";
    options.Username = "Test";
    options.Password = "Test";
    options.Guid = "0c13d406-873b-403b-9c09-a5766840d98c";
});
```

#### Production Ortamı
```csharp
// Program.cs
builder.Services.AddParamApiClient(ParamEnvironment.Production, options =>
{
    options.ClientCode = "YOUR_PROD_CLIENT_CODE";
    options.Username = "YOUR_PROD_USERNAME";
    options.Password = "YOUR_PROD_PASSWORD";
    options.Guid = "YOUR_PROD_GUID";
});
```

### 3. Configuration ile Entegrasyon

```csharp
// appsettings.json
{
  "ParamApi": {
    "Environment": "Test",
    "ClientCode": "10738",
    "Username": "Test",
    "Password": "Test",
    "Guid": "0c13d406-873b-403b-9c09-a5766840d98c"
  }
}

// Program.cs
builder.Services.Configure<ParamApiOptions>(
    builder.Configuration.GetSection("ParamApi"));

builder.Services.AddParamApiClient(ParamEnvironment.Test, options => { });
```

### 4. Servis Kullanımı

```csharp
public class PaymentService
{
    private readonly ITurkposService _turkposService;
    private readonly IKartService _kartService;

    public PaymentService(ITurkposService turkposService, IKartService kartService)
    {
        _turkposService = turkposService;
        _kartService = kartService;
    }

    public async Task<bool> ProcessPaymentAsync(PaymentData data)
    {
        var request = new TP_WMD_UCD_Request
        {
            // ... request verilerini doldur
        };

        var response = await _turkposService.TP_WMD_UCDAsync(request);
        return response.IsSuccessful;
    }
}
```

---

**🔗 Daha fazla bilgi için:** https://dev.param.com.tr/tr/api

**📧 Teknik Destek:** entegrasyon@param.com.tr