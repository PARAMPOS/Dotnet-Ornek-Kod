using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ParamApi.Sdk.Configuration;
using ParamApi.Sdk.Extensions;
using ParamApi.Sdk.Models.Requests;
using ParamApi.Sdk.Services;

namespace ParamApi.Sdk.Examples;

/// <summary>
/// Param API SDK kullanım örneği
/// TP_WMD_UCD (Nonsecure/3D Ödeme) metodu
/// </summary>
public class PaymentExample
{
    public static async Task Main(string[] args)
    {
        // 1. DI Container Setup
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // Test ortamı için SDK'yı kaydet
                services.AddParamApiClient(ParamEnvironment.Test, options =>
                {
                    options.ClientCode = "10738";
                    options.Username = "Test";
                    options.Password = "Test";
                    options.Guid = "0c13d406-873b-403b-9c09-a5766840d98c";
                });

                // Production için:
                // services.AddParamApiClient(ParamEnvironment.Production, options =>
                // {
                //     options.ClientCode = "YOUR_PROD_CLIENT_CODE";
                //     options.Username = "YOUR_PROD_USERNAME";
                //     options.Password = "YOUR_PROD_PASSWORD";
                //     options.Guid = "YOUR_PROD_GUID";
                // });
            })
            .Build();

        // 2. Service'i al
        var turkposService = host.Services.GetRequiredService<ITurkposService>();

        try
        {
            // 3. TP_WMD_UCD Ödeme İsteği Oluştur
            var paymentRequest = new TP_WMD_UCD_Request
            {
                // Kart Bilgileri
                KK_Sahibi = "Test Kullanici",
                KK_No = "4506347011634997",    // Test kartı
                KK_SK_Ay = "12",
                KK_SK_Yil = "2026",
                KK_CVC = "000",
                
                // Ödeme Bilgileri
                Siparis_ID = $"TEST_{DateTime.Now:yyyyMMddHHmmss}",
                Siparis_Aciklama = "Test ödeme işlemi",
                Islem_Tutar = 100.50m,        // 100,50 TL
                Toplam_Tutar = 105.00m,       // Komisyon dahil
                Taksit = 1,                   // Peşin
                
                // URL'ler
                Hata_URL = "https://example.com/error",
                Basarili_URL = "https://example.com/success",
                
                // Güvenlik
                Islem_Guvenlik_Tip = 3,       // 3D güvenli
                IPAdr = "127.0.0.1",
                
                // Opsiyonel
                KK_Sahibi_GSM = "5321234567",
                Ref_URL = "https://example.com"
            };

            Console.WriteLine("🚀 TP_WMD_UCD Ödeme İşlemi Başlatılıyor...");
            Console.WriteLine($"📋 Sipariş ID: {paymentRequest.Siparis_ID}");
            Console.WriteLine($"💰 Tutar: {paymentRequest.Islem_Tutar:C2}");
            Console.WriteLine($"🌍 Ortam: Test");

            // 4. Ödeme İşlemini Gerçekleştir
            var response = await turkposService.TP_WMD_UCDAsync(paymentRequest);

            // 5. Sonucu İşle
            Console.WriteLine("\n📊 ÖDEME SONUCU:");
            Console.WriteLine($"✅ Sonuç: {response.Sonuc}");
            Console.WriteLine($"📝 Açıklama: {response.Sonuc_Str}");
            Console.WriteLine($"🆔 İşlem ID: {response.Islem_ID}");

            // 6. İşlem Tipine Göre Aksiyon Al
            if (response.IsSuccessful)
            {
                if (response.IsNonSecure)
                {
                    // NonSecure işlem başarılı
                    Console.WriteLine("✅ NONSECURE ÖDEME BAŞARILI!");
                    Console.WriteLine($"🧾 Dekont No: {response.Islem_ID}");
                }
                else if (response.Is3DSecure)
                {
                    // 3D işlem için HTML'i göster
                    Console.WriteLine("🔐 3D GÜVENLİ ÖDEME - BANKAYA YÖNLENDİRİLİYOR");
                    Console.WriteLine("🌐 Banka HTML içeriği alındı");
                    
                    // Gerçek uygulamada bu HTML'i web sayfasında gösterirsiniz:
                    // return View("Payment3D", new { HtmlContent = response.UCD_HTML });
                    
                    Console.WriteLine($"📄 HTML Uzunluğu: {response.UCD_HTML?.Length ?? 0} karakter");
                }
            }
            else
            {
                // Hata durumu
                Console.WriteLine("❌ ÖDEME BAŞARISIZ!");
                Console.WriteLine($"🚨 Hata: {response.Sonuc_Str}");
            }

            // 7. Hash Doğrulama Örneği
            Console.WriteLine("\n🔐 HASH DOĞRULAMA:");
            var testData = "TestHashData123";
            var hash = await turkposService.SHA2B64Async(testData);
            Console.WriteLine($"📊 Test Data: {testData}");
            Console.WriteLine($"🔑 SHA2B64 Hash: {hash}");

        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"❌ Validation Hatası: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 Genel Hata: {ex.Message}");
            Console.WriteLine($"📋 Stack Trace: {ex.StackTrace}");
        }
        finally
        {
            // 8. Kaynakları Temizle
            turkposService?.Dispose();
        }

        Console.WriteLine("\n✨ Örnek tamamlandı. Çıkmak için bir tuşa basın...");
        Console.ReadKey();
    }
}

/// <summary>
/// Farklı senaryolar için örnekler
/// </summary>
public static class PaymentScenarios
{
    /// <summary>
    /// Taksitli ödeme örneği
    /// </summary>
    public static TP_WMD_UCD_Request CreateInstallmentPayment()
    {
        return new TP_WMD_UCD_Request
        {
            KK_Sahibi = "Test Kullanici",
            KK_No = "4506347011634997",
            KK_SK_Ay = "12",
            KK_SK_Yil = "2026",
            KK_CVC = "000",
            Siparis_ID = $"TAKSIT_{DateTime.Now:yyyyMMddHHmmss}",
            Siparis_Aciklama = "6 taksitli ödeme",
            Islem_Tutar = 600.00m,
            Toplam_Tutar = 630.00m,        // Komisyon dahil
            Taksit = 6,                    // 6 taksit
            Hata_URL = "https://example.com/error",
            Basarili_URL = "https://example.com/success",
            Islem_Guvenlik_Tip = 3,
            IPAdr = "127.0.0.1"
        };
    }

    /// <summary>
    /// NonSecure ödeme örneği (Sadece belirli MCC kodları için)
    /// </summary>
    public static TP_WMD_UCD_Request CreateNonSecurePayment()
    {
        return new TP_WMD_UCD_Request
        {
            // Maskeli kart bilgileri (Sigorta firmaları için)
            KK_Sahibi = "Test Kullanici",
            KK_No = "450634******4997",      // İlk 6 + son 4 hane
            KK_SK_Ay = "**",                 // Maskeli
            KK_SK_Yil = "****",              // Maskeli
            KK_CVC = "000",
            Siparis_ID = $"NS_{DateTime.Now:yyyyMMddHHmmss}",
            Siparis_Aciklama = "NonSecure ödeme",
            Islem_Tutar = 250.00m,
            Toplam_Tutar = 250.00m,
            Taksit = 1,
            Hata_URL = "https://example.com/error",
            Basarili_URL = "https://example.com/success",
            Islem_Guvenlik_Tip = 0,         // NonSecure
            IPAdr = "127.0.0.1",
            Data1 = "12345678901"           // TCKN/VKN
        };
    }

    /// <summary>
    /// Komisyon hesaplama örneği
    /// </summary>
    public static decimal CalculateCommissionAmount(decimal orderAmount, decimal commissionRate)
    {
        // PARAM API Komisyon Formülü:
        // Toplam_Tutar = Islem_Tutar + ((Islem_Tutar x Komisyon_Oran) / 100)
        return orderAmount + ((orderAmount * commissionRate) / 100);
    }
}

/// <summary>
/// TP_Islem_Odeme_OnProv_WMD - Ön Provizyon İşlemi Örneği
/// Önce tutar bloke edilir, sonra kapama işlemi ile çekim yapılır
/// </summary>
public static class OnProvizyonExample
{
    /// <summary>
    /// Ön Provizyon işlemi başlatma örneği
    /// </summary>
    public static async Task<TP_Islem_Odeme_OnProv_WMD_Response> StartOnProvisionAsync(
        ITurkposService turkposService,
        ParamApiOptions options)
    {
        // Ön provizyon request'i oluştur
        var request = new TP_Islem_Odeme_OnProv_WMD_Request
        {
            // Kart Bilgileri
            KK_Sahibi = "John Doe",
            KK_No = "4506347011634997", // Test kartı
            KK_SK_Ay = "12",
            KK_SK_Yil = "2026",
            KK_CVC = "000",
            KK_Sahibi_GSM = "5551234567",
            
            // Ödeme Bilgileri
            Siparis_ID = $"ONPROV_{DateTime.Now:yyyyMMddHHmmss}",
            Siparis_Aciklama = "Test ön provizyon işlemi",
            Islem_Tutar = 100.00m,
            Toplam_Tutar = 105.00m, // Komisyon dahil
            Taksit = 1,
            
            // URL'ler (3D işlemler için gerekli)
            Hata_URL = "https://mywebsite.com/payment/error",
            Basarili_URL = "https://mywebsite.com/payment/success",
            
            // Güvenlik Tipi
            Islem_Guvenlik_Tip = "3D", // 3D Secure
            // Islem_Guvenlik_Tip = "NS", // NonSecure için
            
            // IP ve URL bilgileri
            IPAdr = "127.0.0.1",
            Ref_URL = "https://mywebsite.com/checkout",
            
            // Extra data (opsiyonel)
            Data1 = "Rezervasyon No: 12345",
            Data2 = "Müşteri: Premium",
            Data3 = "Kanal: Web"
        };
        
        // Hash hesaplama (CalculateOnProvizyonHash kullanarak)
        request.CalculateHash(options);
        
        // İşlemi başlat
        var response = await turkposService.TP_Islem_Odeme_OnProv_WMDAsync(request);
        
        Console.WriteLine("=== ÖN PROVİZYON İŞLEMİ ===");
        Console.WriteLine($"Sipariş ID: {request.Siparis_ID}");
        Console.WriteLine($"Tutar: {request.Islem_Tutar:C2}");
        Console.WriteLine($"Toplam: {request.Toplam_Tutar:C2}");
        Console.WriteLine($"Güvenlik: {request.Islem_Guvenlik_Tip}");
        Console.WriteLine();
        
        if (response.IsSuccessful)
        {
            if (response.IsNonSecure)
            {
                Console.WriteLine("✅ NonSecure Ön Provizyon Başarılı!");
                Console.WriteLine($"İşlem ID: {response.Islem_ID}");
                Console.WriteLine($"UCD_MD: {response.UCD_MD}");
                Console.WriteLine($"Sipariş ID: {response.Siparis_ID}");
                Console.WriteLine();
                Console.WriteLine("⚠️ DİKKAT: Bu değerleri saklayın!");
                Console.WriteLine("Kapama işlemi için UCD_MD ve İşlem ID gerekli!");
            }
            else if (response.Is3DStarted)
            {
                Console.WriteLine("🔐 3D Secure Ön Provizyon Başlatıldı!");
                Console.WriteLine($"İşlem GUID: {response.Islem_GUID}");
                Console.WriteLine($"UCD_MD: {response.UCD_MD}");
                Console.WriteLine();
                Console.WriteLine("👤 Kullanıcı 3D doğrulamaya yönlendiriliyor...");
                Console.WriteLine("UCD_HTML içeriğini web sayfasında gösterin:");
                Console.WriteLine(response.UCD_HTML?.Substring(0, Math.Min(200, response.UCD_HTML.Length)));
                Console.WriteLine();
                Console.WriteLine("⚠️ 3D doğrulama sonrası Success URL'de işlem durumunu kontrol edin!");
            }
        }
        else
        {
            Console.WriteLine($"❌ Ön Provizyon Hatası!");
            Console.WriteLine($"Hata Kodu: {response.Sonuc}");
            Console.WriteLine($"Hata Mesajı: {response.ErrorMessage}");
        }
        
        return response;
    }
}

/// <summary>
/// TP_WMD_PAY - 3D İşlem Tamamlama Örneği
/// 3D doğrulama sonrası tutar çekimi için kullanılır
/// </summary>
public static class ThreeDPaymentCompletion
{
    /// <summary>
    /// 3D ödeme tamamlama örneği
    /// </summary>
    public static async Task CompleteThreeDPayment(ITurkposService turkposService)
    {
        try
        {
            // Bu değerler TP_WMD_UCD çağrısından gelir
            // Gerçek uygulamada kullanıcı bankadan döndükten sonra alınır
            var payRequest = new TP_WMD_PAY_Request
            {
                UCD_MD = "1000000089300991",           // TP_WMD_UCD'den dönen MD değeri
                Islem_GUID = "sample-guid-from-ucd",   // TP_WMD_UCD'den dönen İşlem GUID
                Siparis_ID = "TEST_20241201120000"     // Aynı sipariş ID
            };

            Console.WriteLine("🔐 3D İşlem Tamamlanıyor...");
            Console.WriteLine($"📋 Sipariş ID: {payRequest.Siparis_ID}");
            Console.WriteLine($"🆔 İşlem GUID: {payRequest.Islem_GUID}");

            // TP_WMD_PAY ile işlemi tamamla
            var payResponse = await turkposService.TP_WMD_PAYAsync(payRequest);

            Console.WriteLine("\n📊 3D ÖDEME TAMAMLAMA SONUCU:");
            
            if (payResponse.IsSuccessful)
            {
                Console.WriteLine("✅ 3D ÖDEME BAŞARILI!");
                Console.WriteLine($"🧾 Dekont ID: {payResponse.Dekont_ID}");
                Console.WriteLine($"🏦 Banka Transaction ID: {payResponse.Bank_Trans_ID}");
                Console.WriteLine($"🔑 Authorization Code: {payResponse.Bank_AuthCode}");
                Console.WriteLine($"📱 RRN: {payResponse.Bank_HostRefNum}");
                Console.WriteLine($"💰 Komisyon Oranı: %{payResponse.Komisyon_Oran}");
                Console.WriteLine($"🏛️ Banka Mesajı: {payResponse.Bank_HostMsg}");
                
                if (!string.IsNullOrEmpty(payResponse.Bank_Extra))
                {
                    Console.WriteLine($"🎁 Banka Extra: {payResponse.Bank_Extra}");
                }
            }
            else
            {
                Console.WriteLine("❌ 3D ÖDEME BAŞARISIZ!");
                Console.WriteLine($"🚨 Hata: {payResponse.ErrorMessage}");
                Console.WriteLine($"🔢 Banka Sonuç Kodu: {payResponse.Bank_Sonuc_Kod}");
            }
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"❌ Validation Hatası: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 3D Tamamlama Hatası: {ex.Message}");
        }
    }

    /// <summary>
    /// 3D işlem akışının tam örneği
    /// </summary>
    public static async Task FullThreeDPaymentFlow(ITurkposService turkposService)
    {
        try
        {
            Console.WriteLine("🚀 TAM 3D ÖDEME AKIŞI BAŞLATILIYOR...\n");

            // 1. TP_WMD_UCD ile 3D işlem başlat
            var ucdRequest = new TP_WMD_UCD_Request
            {
                KK_Sahibi = "Test Kullanici",
                KK_No = "4506347011634997",
                KK_SK_Ay = "12",
                KK_SK_Yil = "2026",
                KK_CVC = "000",
                Siparis_ID = $"3D_{DateTime.Now:yyyyMMddHHmmss}",
                Siparis_Aciklama = "3D Güvenli Ödeme",
                Islem_Tutar = 150.75m,
                Toplam_Tutar = 158.28m,
                Taksit = 1,
                Hata_URL = "https://example.com/error",
                Basarili_URL = "https://example.com/success",
                Islem_Guvenlik_Tip = 3,  // 3D Güvenli
                IPAdr = "127.0.0.1"
            };

            Console.WriteLine("1️⃣ TP_WMD_UCD çağrılıyor...");
            var ucdResponse = await turkposService.TP_WMD_UCDAsync(ucdRequest);

            if (ucdResponse.IsSuccessful && ucdResponse.Is3DSecure)
            {
                Console.WriteLine("✅ 3D HTML alındı - Kullanıcı bankaya yönlendiriliyor");
                Console.WriteLine($"📄 HTML Uzunluğu: {ucdResponse.UCD_HTML?.Length ?? 0} karakter\n");

                // 2. Simülasyon: Kullanıcı bankada doğrulama yaptı ve geri döndü
                Console.WriteLine("2️⃣ Kullanıcı banka doğrulamasını tamamladı (simülasyon)...");

                // 3. TP_WMD_PAY ile işlemi tamamla
                var payRequest = new TP_WMD_PAY_Request
                {
                    UCD_MD = ucdResponse.UCD_MD,
                    Islem_GUID = ucdResponse.Islem_GUID,
                    Siparis_ID = ucdRequest.Siparis_ID
                };

                Console.WriteLine("3️⃣ TP_WMD_PAY ile işlem tamamlanıyor...");
                var payResponse = await turkposService.TP_WMD_PAYAsync(payRequest);

                if (payResponse.IsSuccessful)
                {
                    Console.WriteLine("\n🎉 3D ÖDEME TAM AKIŞI BAŞARILI!");
                    Console.WriteLine($"🧾 Final Dekont: {payResponse.Dekont_ID}");
                    Console.WriteLine($"💰 Ödenen Tutar: {ucdRequest.Islem_Tutar:C2}");
                }
                else
                {
                    Console.WriteLine($"\n❌ 3D Tamamlama Hatası: {payResponse.ErrorMessage}");
                }
            }
            else
            {
                Console.WriteLine($"❌ 3D Başlatma Hatası: {ucdResponse.Sonuc_Str}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 3D Akış Hatası: {ex.Message}");
        }
    }
}

/// <summary>
/// TP_Islem_Odeme_OnProv_Kapa - Ön Provizyon Kapama Örneği
/// Ön provizyon işlemini satışa dönüştürür
/// </summary>
public static class OnProvizyonKapamaExample
{
    /// <summary>
    /// Ön provizyon kapama işlemi örneği
    /// </summary>
    public static async Task<TP_Islem_Odeme_OnProv_Kapa_Response> CloseProvisionAsync(
        ITurkposService turkposService)
    {
        try
        {
            // Ön provizyon kapama request'i oluştur
            var request = new TP_Islem_Odeme_OnProv_Kapa_Request
            {
                // Provizyon ID (Ön provizyon işleminden alınan ID)
                Prov_ID = "f7184b1f-c4c2-4d2e-8428-fc6014a00900", // Opsiyonel
                
                // Kapama tutarı (virgüllü format otomatik dönüştürülür)
                Prov_Tutar = 100.00m,
                
                // Sipariş ID
                Siparis_ID = "ONPROV_20241201120000"
            };

            Console.WriteLine("💳 ÖN PROVİZYON KAPAMA İŞLEMİ");
            Console.WriteLine($"📋 Sipariş ID: {request.Siparis_ID}");
            Console.WriteLine($"🆔 Provizyon ID: {request.Prov_ID ?? "Boş"}");
            Console.WriteLine($"💰 Kapama Tutarı: {request.Prov_Tutar:C2}");
            Console.WriteLine();

            // Kapama işlemini gerçekleştir
            var response = await turkposService.TP_Islem_Odeme_OnProv_KapaAsync(request);

            Console.WriteLine("📊 KAPAMA SONUCU:");
            
            if (response.IsSuccessful)
            {
                Console.WriteLine("✅ ÖN PROVİZYON KAPAMA BAŞARILI!");
                Console.WriteLine($"🧾 Dekont ID: {response.Dekont_ID}");
                Console.WriteLine($"🆔 Provizyon ID: {response.Prov_ID}");
                Console.WriteLine($"📝 Sonuç: {response.Sonuc_Str}");
                
                if (response.IsBankSuccessful)
                {
                    Console.WriteLine("🏦 Banka işlemi başarılı!");
                    Console.WriteLine($"🔑 Banka Yetki Kodu: {response.Bank_AuthCode}");
                    Console.WriteLine($"🏛️ Transaction ID: {response.Bank_Trans_ID}");
                    Console.WriteLine($"📱 Host Referans: {response.Bank_HostRefNum}");
                    
                    if (!string.IsNullOrEmpty(response.Bank_HostMsg))
                    {
                        Console.WriteLine($"💬 Banka Mesajı: {response.Bank_HostMsg}");
                    }
                }
                else
                {
                    Console.WriteLine($"⚠️ Banka Hatası (Kod: {response.Banka_Sonuc_Kod})");
                }
            }
            else
            {
                Console.WriteLine("❌ ÖN PROVİZYON KAPAMA BAŞARISIZ!");
                Console.WriteLine($"🚨 Hata: {response.Sonuc_Str}");
                Console.WriteLine($"🔢 Sonuç Kodu: {response.Sonuc}");
                Console.WriteLine($"🏦 Banka Kodu: {response.Banka_Sonuc_Kod}");
            }

            return response;
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"❌ Validation Hatası: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 Kapama İşlemi Hatası: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Tam ön provizyon akışı örneği (Başlatma → Kapama)
    /// </summary>
    public static async Task FullProvisionFlow(ITurkposService turkposService, ParamApiOptions options)
    {
        try
        {
            Console.WriteLine("🚀 TAM ÖN PROVİZYON AKIŞI BAŞLATILIYOR...\n");

            // 1. Ön provizyon başlat
            Console.WriteLine("1️⃣ ÖN PROVİZYON BAŞLATILIYOR...");
            var provisionResponse = await OnProvizyonExample.StartOnProvisionAsync(turkposService, options);

            if (!provisionResponse.IsSuccessful)
            {
                Console.WriteLine("❌ Ön provizyon başarısız, akış durduruluyor.");
                return;
            }

            // 2. NonSecure ise direkt kapama yapılabilir
            if (provisionResponse.IsNonSecure)
            {
                Console.WriteLine("\n2️⃣ NON-SECURE ÖN PROVİZYON KAPAMA...");
                
                // Kısa bekleme (isteğe bağlı)
                await Task.Delay(3000);
                
                var kapamaRequest = new TP_Islem_Odeme_OnProv_Kapa_Request
                {
                    Prov_ID = provisionResponse.Islem_ID, // NonSecure'da İşlem ID kullanılabilir
                    Prov_Tutar = 100.00m, // Aynı tutar veya daha az
                    Siparis_ID = provisionResponse.Siparis_ID
                };

                var kapamaResponse = await turkposService.TP_Islem_Odeme_OnProv_KapaAsync(kapamaRequest);
                
                if (kapamaResponse.IsSuccessful)
                {
                    Console.WriteLine("✅ TAM AKIŞ BAŞARILI! Ön provizyon satışa dönüştürüldü.");
                    Console.WriteLine($"🎯 Final Dekont ID: {kapamaResponse.Dekont_ID}");
                }
                else
                {
                    Console.WriteLine("❌ Kapama başarısız!");
                }
            }
            // 3D işlemler için kullanıcı bankadan döndükten sonra kapama yapılır
            else if (provisionResponse.Is3DStarted)
            {
                Console.WriteLine("\n⏳ 3D SECURE: Kullanıcı bankadan döndükten sonra kapama yapılacak...");
                Console.WriteLine("📝 NOT: Success URL'de işlem durumunu kontrol edin ve kapama yapın.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 Tam Akış Hatası: {ex.Message}");
        }
    }
}

/// <summary>
/// TP_Islem_Iptal_OnProv - Ön Provizyon İptal Örneği
/// Satış işlemi yapılmamış provizyon iptali için kullanılır
/// </summary>
public static class OnProvizyonIptalExample
{
    /// <summary>
    /// Ön provizyon iptal işlemi örneği
    /// </summary>
    public static async Task<TP_Islem_Iptal_OnProv_Response> CancelProvisionAsync(
        ITurkposService turkposService)
    {
        try
        {
            // Ön provizyon iptal request'i oluştur
            var request = new TP_Islem_Iptal_OnProv_Request
            {
                // Provizyon ID (Ön provizyon işleminden alınan ID - opsiyonel)
                Prov_ID = "dfbf4673-83d8-436a-a922-ae8ae7e66bb8",
                
                // Sipariş ID (Zorunlu)
                Siparis_ID = "ORDER_12345_2024"
            };

            Console.WriteLine("🚫 ÖN PROVİZYON İPTAL İŞLEMİ BAŞLATILIYOR...");
            Console.WriteLine($"📌 Prov ID: {request.Prov_ID}");
            Console.WriteLine($"📌 Sipariş ID: {request.Siparis_ID}");

            // İptal işlemini çağır
            var response = await turkposService.TP_Islem_Iptal_OnProvAsync(request);

            Console.WriteLine("📄 === ÖN PROVİZYON İPTAL SONUCU ===");
            Console.WriteLine($"✅ İşlem Sonucu: {response.Sonuc}");
            Console.WriteLine($"📝 Açıklama: {response.Sonuc_Str}");
            Console.WriteLine($"🏦 Banka Sonuç Kodu: {response.Banka_Sonuc_Kod}");

            // Sonuç kontrolü
            if (response.IsFullySuccessful)
            {
                Console.WriteLine("🎉 ÖN PROVİZYON İPTALİ BAŞARILI!");
                Console.WriteLine("💡 Provizyon başarıyla iptal edildi.");
            }
            else if (response.IsSuccessful)
            {
                Console.WriteLine("⚠️ İşlem başarılı ama banka sonuç kodu dikkat gerektirir.");
            }
            else
            {
                Console.WriteLine("❌ ÖN PROVİZYON İPTALİ BAŞARISIZ!");
                Console.WriteLine($"❌ Hata: {response.Sonuc_Str}");
            }

            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 Ön Provizyon İptal Hatası: {ex.Message}");
            return null!;
        }
    }

    /// <summary>
    /// Toplu ön provizyon iptal örneği
    /// </summary>
    public static async Task<List<TP_Islem_Iptal_OnProv_Response>> BulkCancelProvisionsAsync(
        ITurkposService turkposService,
        List<string> siparisIdList)
    {
        var results = new List<TP_Islem_Iptal_OnProv_Response>();

        Console.WriteLine($"🔄 {siparisIdList.Count} adet ön provizyon iptal edilecek...");

        foreach (var siparisId in siparisIdList)
        {
            try
            {
                var request = new TP_Islem_Iptal_OnProv_Request
                {
                    Siparis_ID = siparisId
                    // Prov_ID boş bırakılıyor (opsiyonel)
                };

                var response = await turkposService.TP_Islem_Iptal_OnProvAsync(request);
                results.Add(response);

                Console.WriteLine($"✅ {siparisId}: {response.Sonuc_Str}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ {siparisId}: Hata - {ex.Message}");
            }

            // API rate limiting için kısa bekleme
            await Task.Delay(100);
        }

        var successCount = results.Count(r => r.IsFullySuccessful);
        Console.WriteLine($"📊 Sonuç: {successCount}/{results.Count} başarılı iptal");

        return results;
    }
}

/// <summary>
/// TP_Islem_Odeme_WD - Dövizli Ödeme İşlemi Örneği
/// Bu metot sadece yabancı kartlar ile çalışmaktadır
/// </summary>
public static class DovizliOdemeExample
{
    /// <summary>
    /// Dövizli ödeme işlemi örneği (USD ile)
    /// </summary>
    public static async Task<TP_Islem_Odeme_WD_Response> PayWithForeignCurrencyAsync(
        ITurkposService turkposService)
    {
        try
        {
            // Dövizli ödeme request'i oluştur (USD örneği)
            var request = new TP_Islem_Odeme_WD_Request
            {
                // Döviz kodu (USD)
                Doviz_Kodu = DovizKodu.TurkPara_USD,
                
                // Kart bilgileri (Yabancı kart)
                KK_Sahibi = "JOHN DOE",
                KK_No = "4546711234567894", // Test kartı
                KK_SK_Ay = "12",
                KK_SK_Yil = "2026",
                KK_CVC = "123",
                KK_Sahibi_GSM = "5551234567",
                
                // URL'ler
                Hata_URL = "https://yoursite.com/payment/error",
                Basarili_URL = "https://yoursite.com/payment/success",
                
                // Sipariş bilgileri
                Siparis_ID = $"WD_ORDER_{DateTime.Now.Ticks}",
                Siparis_Aciklama = "USD ile dövizli ödeme testi",
                
                // Tutar bilgileri (USD olarak)
                Islem_Tutar = 100.00m, // 100 USD
                Toplam_Tutar = 100.00m, // Komisyon yok
                
                // Güvenlik tipi
                Islem_Guvenlik_Tip = "3D", // 3D Secure
                
                // IP ve opsiyonel alanlar
                IPAdr = "127.0.0.1",
                Islem_ID = "WD001",
                Ref_URL = "https://yoursite.com/products/item1",
                Data1 = "USD Payment",
                Data2 = "Foreign Card",
                Data3 = "Test Data"
            };

            Console.WriteLine("🌍 Dövizli ödeme işlemi başlatılıyor...");
            Console.WriteLine($"💰 Tutar: {request.Islem_Tutar} {request.Doviz_Kodu}");
            Console.WriteLine($"💳 Kart: {request.KK_Sahibi}");
            Console.WriteLine($"🔒 Güvenlik: {request.Islem_Guvenlik_Tip}");

            // API çağrısı
            var response = await turkposService.TP_Islem_Odeme_WDAsync(request);

            // Sonuç analizi
            Console.WriteLine("\n📋 İşlem Sonucu:");
            Console.WriteLine($"✅ Başarılı: {response.IsSuccess}");
            Console.WriteLine($"📝 Açıklama: {response.Sonuc_Str}");
            Console.WriteLine($"🆔 İşlem ID: {response.Islem_ID}");
            Console.WriteLine($"🏦 Banka Kodu: {response.Banka_Sonuc_Kod}");

            if (response.Is3DSecure)
            {
                Console.WriteLine($"🔐 3D URL: {response.UCD_URL}");
                Console.WriteLine("👤 Müşteri bu URL'e yönlendirilmelidir!");
            }
            else if (response.IsNonSecure)
            {
                Console.WriteLine("✅ NonSecure ödeme başarıyla tamamlandı!");
            }

            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 Dövizli Ödeme Hatası: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Euro ile dövizli ödeme örneği
    /// </summary>
    public static async Task<TP_Islem_Odeme_WD_Response> PayWithEuroAsync(
        ITurkposService turkposService)
    {
        try
        {
            var request = new TP_Islem_Odeme_WD_Request
            {
                Doviz_Kodu = DovizKodu.TurkPara_EUR, // Euro
                KK_Sahibi = "MARIA GARCIA",
                KK_No = "5555444433332222",
                KK_SK_Ay = "10",
                KK_SK_Yil = "2025",
                KK_CVC = "456",
                KK_Sahibi_GSM = "5559876543",
                Hata_URL = "https://yoursite.com/payment/error",
                Basarili_URL = "https://yoursite.com/payment/success",
                Siparis_ID = $"EUR_ORDER_{DateTime.Now.Ticks}",
                Siparis_Aciklama = "Euro ile ödeme",
                Islem_Tutar = 50.00m, // 50 EUR
                Toplam_Tutar = 50.00m,
                Islem_Guvenlik_Tip = "NS", // NonSecure
                IPAdr = "192.168.1.100",
                Data1 = "EUR Payment"
            };

            Console.WriteLine("🇪🇺 Euro ile ödeme işlemi başlatılıyor...");
            var response = await turkposService.TP_Islem_Odeme_WDAsync(request);
            
            Console.WriteLine($"📊 Durum: {response.StatusDescription}");
            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 Euro Ödeme Hatası: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// İngiliz Sterlini ile ödeme örneği
    /// </summary>
    public static async Task<TP_Islem_Odeme_WD_Response> PayWithGbpAsync(
        ITurkposService turkposService)
    {
        try
        {
            var request = new TP_Islem_Odeme_WD_Request
            {
                Doviz_Kodu = DovizKodu.TurkPara_GBP, // İngiliz Sterlini
                KK_Sahibi = "DAVID SMITH",
                KK_No = "4111111111111111",
                KK_SK_Ay = "08",
                KK_SK_Yil = "2027",
                KK_CVC = "789",
                KK_Sahibi_GSM = "5556789012",
                Hata_URL = "https://yoursite.com/payment/error",
                Basarili_URL = "https://yoursite.com/payment/success",
                Siparis_ID = $"GBP_ORDER_{DateTime.Now.Ticks}",
                Siparis_Aciklama = "İngiliz Sterlini ile ödeme",
                Islem_Tutar = 75.50m, // 75.50 GBP
                Toplam_Tutar = 75.50m,
                Islem_Guvenlik_Tip = "3D", // 3D Secure
                IPAdr = "10.0.0.1",
                Ref_URL = "https://yoursite.com/uk-products",
                Data1 = "GBP Payment",
                Data2 = "UK Market"
            };

            Console.WriteLine("🇬🇧 İngiliz Sterlini ile ödeme işlemi başlatılıyor...");
            var response = await turkposService.TP_Islem_Odeme_WDAsync(request);
            
            Console.WriteLine($"📊 Durum: {response.StatusDescription}");
            Console.WriteLine($"🔗 3D URL: {response.UCD_URL}");
            
            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 GBP Ödeme Hatası: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Döviz kodu seçim helper'ı
    /// </summary>
    public static void ShowCurrencyOptions()
    {
        Console.WriteLine("🌍 Desteklenen Dövizler:");
        Console.WriteLine("1000 - TRL (Türk Lirası)");
        Console.WriteLine("1001 - USD (Amerikan Doları)");
        Console.WriteLine("1002 - EUR (Euro)");
        Console.WriteLine("1003 - GBP (İngiliz Sterlini)");
        Console.WriteLine("\n⚠️  Bu metot sadece yabancı kartlar ile çalışır!");
        Console.WriteLine("📝 Test kartları: 4546711234567894, 5555444433332222, 4111111111111111");
    }

    /// <summary>
    /// Dövizli ödeme response callback işleme örneği
    /// </summary>
    public static void ProcessCurrencyPaymentCallback(Dictionary<string, string> callbackData)
    {
        Console.WriteLine("📞 Dövizli ödeme callback işleniyor...");
        
        // PARAM'dan dönen callback verileri
        if (callbackData.ContainsKey("TURKPOS_RETVAL_Sonuc"))
        {
            var sonuc = callbackData["TURKPOS_RETVAL_Sonuc"];
            var sonucStr = callbackData.GetValueOrDefault("TURKPOS_RETVAL_Sonuc_Str", "");
            var guid = callbackData.GetValueOrDefault("TURKPOS_RETVAL_GUID", "");
            var islemTarih = callbackData.GetValueOrDefault("TURKPOS_RETVAL_Islem_Tarih", "");
            var dekontId = callbackData.GetValueOrDefault("TURKPOS_RETVAL_Dekont_ID", "");
            var tahsilatTutari = callbackData.GetValueOrDefault("TURKPOS_RETVAL_Tahsilat_Tutari", "");
            var odemeTutari = callbackData.GetValueOrDefault("TURKPOS_RETVAL_Odeme_Tutari", "");
            var siparisId = callbackData.GetValueOrDefault("TURKPOS_RETVAL_Siparis_ID", "");
            var islemId = callbackData.GetValueOrDefault("TURKPOS_RETVAL_Islem_ID", "");
            var extData = callbackData.GetValueOrDefault("TURKPOS_RETVAL_Ext_Data", "");
            var bankaSonucKod = callbackData.GetValueOrDefault("TURKPOS_RETVAL_Banka_Sonuc_Kod", "");
            var kkNo = callbackData.GetValueOrDefault("TURKPOS_RETVAL_KK_No", "");

            Console.WriteLine($"💰 Sonuç: {sonuc} - {sonucStr}");
            Console.WriteLine($"📅 Tarih: {islemTarih}");
            Console.WriteLine($"💳 Kart: {kkNo}");
            Console.WriteLine($"💵 Tahsilat: {tahsilatTutari}");
            Console.WriteLine($"💸 Net Tutar: {odemeTutari}");
            Console.WriteLine($"🆔 Dekont: {dekontId}");
            Console.WriteLine($"📦 Sipariş: {siparisId}");

            if (sonuc == "1")
            {
                Console.WriteLine("✅ Dövizli ödeme başarıyla tamamlandı!");
                // Sipariş tamamlama işlemleri
            }
            else
            {
                Console.WriteLine("❌ Dövizli ödeme başarısız!");
                // Hata işlemleri
            }
        }
    }
}

/// <summary>
/// TP_Islem_Odeme_BKM - BKM Express Ödeme İşlemi Örneği
/// BKM Express aracılığı ile ödeme işleminin başlatıldığı metottur
/// </summary>
public static class BkmExpressOdemeExample
{
    /// <summary>
    /// BKM Express ödeme işlemi örneği
    /// </summary>
    public static async Task<TP_Islem_Odeme_BKM_Response> ProcessBkmExpressPaymentAsync(
        ITurkposService turkposService)
    {
        try
        {
            // BKM Express ödeme request'i oluştur
            var request = new TP_Islem_Odeme_BKM_Request
            {
                // Müşteri bilgileri
                Customer_Info = "Ahmet Mehmet ÖZCAN",
                Customer_GSM = "5551234567", // Başında 0 olmadan
                
                // URL'ler
                Error_URL = "https://yoursite.com/payment/error",
                Success_URL = "https://yoursite.com/payment/success",
                
                // Sipariş bilgileri
                Order_ID = $"BKM_ORDER_{DateTime.Now:yyyyMMddHHmmss}",
                Order_Description = "BKM Express ile online alışveriş",
                Amount = 125.50m, // 125,50 TL
                
                // Opsiyonel bilgiler
                Transaction_ID = $"TXN_{DateTime.Now.Ticks}",
                IPAddress = "127.0.0.1",
                Referrer_URL = "https://yoursite.com/products/electronics"
            };

            Console.WriteLine("🏦 BKM Express ödeme işlemi başlatılıyor...");
            Console.WriteLine($"💰 Tutar: {request.Amount:C2}");
            Console.WriteLine($"📱 GSM: {request.Customer_GSM}");
            Console.WriteLine($"📦 Sipariş: {request.Order_ID}");
            Console.WriteLine($"👤 Müşteri: {request.Customer_Info}");

            // BKM Express işlemini başlat
            var response = await turkposService.TP_Islem_Odeme_BKMAsync(request);

            Console.WriteLine("\n📊 BKM EXPRESS SONUCU:");
            Console.WriteLine($"✅ Response Code: {response.Response_Code}");
            Console.WriteLine($"📝 Response Message: {response.Response_Message}");

            if (response.IsSuccess)
            {
                Console.WriteLine("🎉 BKM EXPRESS İŞLEMİ BAŞARILI!");
                Console.WriteLine($"🔗 BKM URL: {response.Redirect_URL}");
                Console.WriteLine();
                Console.WriteLine("📋 İşlem Adımları:");
                Console.WriteLine("1. ✅ BKM Express URL'si alındı");
                Console.WriteLine("2. ➡️  Müşteri bu URL'e yönlendirilecek");
                Console.WriteLine("3. 🏦 BKM Express'e giriş yapacak");
                Console.WriteLine("4. 💳 Ödeme işlemini tamamlayacak");
                Console.WriteLine("5. 🔄 Success/Error URL'e yönlendirilecek");
                
                // Gerçek uygulamada bu URL'e redirect yapılır
                Console.WriteLine($"\n🌐 Redirect URL: {response.Redirect_URL}");
                Console.WriteLine("⚠️  Bu URL'i kullanıcıya gösterin veya redirect yapın!");
            }
            else
            {
                Console.WriteLine("❌ BKM EXPRESS İŞLEM HATASI!");
                Console.WriteLine($"🚨 Hata: {response.Response_Message}");
                Console.WriteLine($"🔢 Hata Kodu: {response.Response_Code}");
            }

            return response;
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"❌ BKM Validation Hatası: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 BKM Express Genel Hata: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// BKM Express callback işleme örneği
    /// </summary>
    public static void ProcessBkmExpressCallback(Dictionary<string, string> callbackData)
    {
        Console.WriteLine("📞 BKM Express callback işleniyor...");
        
        // PARAM'dan dönen BKM callback verileri
        if (callbackData.ContainsKey("TURKPOS_RETVAL_Sonuc"))
        {
            var sonuc = callbackData["TURKPOS_RETVAL_Sonuc"];
            var sonucStr = callbackData.GetValueOrDefault("TURKPOS_RETVAL_Sonuc_Str", "");
            var guid = callbackData.GetValueOrDefault("TURKPOS_RETVAL_GUID", "");
            var islemTarih = callbackData.GetValueOrDefault("TURKPOS_RETVAL_Islem_Tarih", "");
            var dekontId = callbackData.GetValueOrDefault("TURKPOS_RETVAL_Dekont_ID", "");
            var tahsilatTutari = callbackData.GetValueOrDefault("TURKPOS_RETVAL_Tahsilat_Tutari", "");
            var odemeTutari = callbackData.GetValueOrDefault("TURKPOS_RETVAL_Odeme_Tutari", "");
            var siparisId = callbackData.GetValueOrDefault("TURKPOS_RETVAL_Siparis_ID", "");
            var islemId = callbackData.GetValueOrDefault("TURKPOS_RETVAL_Islem_ID", "");
            var bankaSonucKod = callbackData.GetValueOrDefault("TURKPOS_RETVAL_Banka_Sonuc_Kod", "");
            var bkmTransactionId = callbackData.GetValueOrDefault("TURKPOS_RETVAL_BKM_Transaction_ID", "");

            Console.WriteLine($"🏦 BKM Sonuç: {sonuc} - {sonucStr}");
            Console.WriteLine($"📅 İşlem Tarihi: {islemTarih}");
            Console.WriteLine($"💰 Tahsilat Tutarı: {tahsilatTutari}");
            Console.WriteLine($"💸 Net Ödeme: {odemeTutari}");
            Console.WriteLine($"🧾 Dekont ID: {dekontId}");
            Console.WriteLine($"📦 Sipariş ID: {siparisId}");
            Console.WriteLine($"🔄 BKM Transaction: {bkmTransactionId}");

            if (sonuc == "1")
            {
                Console.WriteLine("✅ BKM Express ödeme başarıyla tamamlandı!");
                
                // Başarılı ödeme işlemleri
                Console.WriteLine("🎯 Başarılı ödeme sonrası yapılacaklar:");
                Console.WriteLine("- Sipariş durumunu 'Ödendi' olarak güncelle");
                Console.WriteLine("- Müşteriye ödeme onay e-postası gönder");
                Console.WriteLine("- Teslimat sürecini başlat");
                Console.WriteLine("- Faturalandırma işlemlerini yap");
            }
            else
            {
                Console.WriteLine("❌ BKM Express ödeme başarısız!");
                Console.WriteLine($"🏦 Banka Sonuç Kodu: {bankaSonucKod}");
                
                // Başarısız ödeme işlemleri
                Console.WriteLine("⚠️ Başarısız ödeme sonrası yapılacaklar:");
                Console.WriteLine("- Sipariş durumunu 'Ödeme Başarısız' olarak güncelle");
                Console.WriteLine("- Müşteriye hata bilgisi gönder");
                Console.WriteLine("- Stok rezervasyonunu iptal et");
                Console.WriteLine("- Log kayıtları oluştur");
            }
        }
        else
        {
            Console.WriteLine("⚠️ Geçersiz BKM callback verisi!");
        }
    }

    /// <summary>
    /// BKM Express hash doğrulama örneği
    /// </summary>
    public static bool ValidateBkmExpressHash(
        string clientCode,
        string guid,
        string amount,
        string orderId,
        string errorUrl,
        string successUrl,
        string receivedHash)
    {
        try
        {
            // Hash hesapla
            var calculatedHash = HashHelper.CalculateTP_Islem_Odeme_BKM_Hash(
                clientCode,
                guid,
                amount,
                orderId,
                errorUrl,
                successUrl);

            var isValid = calculatedHash == receivedHash;
            
            Console.WriteLine("🔐 BKM Express Hash Doğrulama:");
            Console.WriteLine($"🧮 Hesaplanan: {calculatedHash}");
            Console.WriteLine($"📨 Gelen: {receivedHash}");
            Console.WriteLine($"✅ Geçerli: {isValid}");

            return isValid;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 Hash doğrulama hatası: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// BKM Express test senaryoları
    /// </summary>
    public static async Task RunBkmExpressTestScenariosAsync(ITurkposService turkposService)
    {
        Console.WriteLine("🧪 BKM EXPRESS TEST SENARYOLARI BAŞLATILIYOR...\n");

        try
        {
            // Senaryo 1: Normal BKM Express ödeme
            Console.WriteLine("📋 Senaryo 1: Normal BKM Express Ödeme");
            await ProcessBkmExpressPaymentAsync(turkposService);

            Console.WriteLine("\n" + new string('=', 50) + "\n");

            // Senaryo 2: Yüksek tutarlı ödeme
            Console.WriteLine("📋 Senaryo 2: Yüksek Tutarlı BKM Express Ödeme");
            await ProcessHighAmountBkmPayment(turkposService);

            Console.WriteLine("\n" + new string('=', 50) + "\n");

            // Senaryo 3: Hash doğrulama testi
            Console.WriteLine("📋 Senaryo 3: Hash Doğrulama Testi");
            TestBkmHashValidation();

        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 Test senaryoları hatası: {ex.Message}");
        }
    }

    /// <summary>
    /// Yüksek tutarlı BKM Express ödeme örneği
    /// </summary>
    private static async Task<TP_Islem_Odeme_BKM_Response> ProcessHighAmountBkmPayment(
        ITurkposService turkposService)
    {
        var request = new TP_Islem_Odeme_BKM_Request
        {
            Customer_Info = "Premium Müşteri YILMAZ",
            Customer_GSM = "5339876543",
            Error_URL = "https://premium-shop.com/payment/error",
            Success_URL = "https://premium-shop.com/payment/success",
            Order_ID = $"PREMIUM_BKM_{DateTime.Now:yyyyMMddHHmmss}",
            Order_Description = "Premium ürün alışverişi - BKM Express",
            Amount = 2500.00m, // 2.500,00 TL
            Transaction_ID = $"PRM_{DateTime.Now.Ticks}",
            IPAddress = "192.168.1.100",
            Referrer_URL = "https://premium-shop.com/premium-products"
        };

        Console.WriteLine($"💎 Premium BKM işlemi - Tutar: {request.Amount:C2}");
        
        var response = await turkposService.TP_Islem_Odeme_BKMAsync(request);
        
        if (response.IsSuccess)
        {
            Console.WriteLine("✅ Premium BKM Express başarılı!");
            Console.WriteLine($"🔗 BKM URL: {response.Redirect_URL}");
        }
        else
        {
            Console.WriteLine($"❌ Premium BKM hatası: {response.Response_Message}");
        }

        return response;
    }

    /// <summary>
    /// BKM Express hash doğrulama test senaryosu
    /// </summary>
    private static void TestBkmHashValidation()
    {
        Console.WriteLine("🔐 BKM Express Hash Doğrulama Testleri:");

        // Test parametreleri
        var clientCode = "10738";
        var guid = "0c13d406-873b-403b-9c09-a5766840d98c";
        var amount = "100,00";
        var orderId = "TEST_BKM_12345";
        var errorUrl = "https://test.com/error";
        var successUrl = "https://test.com/success";

        // Hash hesapla
        var calculatedHash = HashHelper.CalculateTP_Islem_Odeme_BKM_Hash(
            clientCode, guid, amount, orderId, errorUrl, successUrl);

        Console.WriteLine($"🔑 Test Hash: {calculatedHash}");

        // Pozitif test
        var isValidPositive = ValidateBkmExpressHash(
            clientCode, guid, amount, orderId, errorUrl, successUrl, calculatedHash);
        Console.WriteLine($"✅ Pozitif Test: {isValidPositive}");

        // Negatif test
        var wrongHash = "WrongHashValue123=";
        var isValidNegative = ValidateBkmExpressHash(
            clientCode, guid, amount, orderId, errorUrl, successUrl, wrongHash);
        Console.WriteLine($"❌ Negatif Test: {isValidNegative}");
    }
}

/// <summary>
/// TP_Islem_Iptal_Iade_Kismi2 - Kısmi İptal/İade İşlemi Örneği
/// Başarılı bir kredi kartı işleminin iptal veya iadesini yapmak için kullanılır
/// </summary>
public static class KismiIptalIadeExample
{
    /// <summary>
    /// Kısmi iptal işlemi örneği
    /// </summary>
    public static async Task<TP_Islem_Iptal_Iade_Kismi2_Response> ProcessPartialCancellationAsync(
        ITurkposService turkposService)
    {
        try
        {
            // Kısmi iptal request'i oluştur
            var request = new TP_Islem_Iptal_Iade_Kismi2_Request
            {
                Durum = IptalIadeDurum.IPTAL, // İptal işlemi (aynı gün)
                Siparis_ID = "3000159380", // Orijinal sipariş ID
                Tutar = 50.00m // Kısmi iptal tutarı (toplam tutardan düşük olabilir)
            };

            Console.WriteLine("🔄 Kısmi İptal İşlemi Başlatılıyor...");
            Console.WriteLine($"📋 Sipariş ID: {request.Siparis_ID}");
            Console.WriteLine($"💰 İptal Tutarı: {request.Tutar:C2}");
            Console.WriteLine($"🔧 İşlem Türü: {request.Durum}");

            // PARAM API çağrısı
            var response = await turkposService.TP_Islem_Iptal_Iade_Kismi2Async(request);

            if (response.IsSuccess)
            {
                Console.WriteLine("✅ Kısmi İptal İşlemi Başarılı!");
                Console.WriteLine($"📝 Sonuç: {response.Sonuc_Str}");
                Console.WriteLine($"🏦 Banka Auth Code: {response.Bank_AuthCode}");
                Console.WriteLine($"🆔 Banka Transaction ID: {response.Bank_Trans_ID}");
                
                if (!string.IsNullOrEmpty(response.Bank_Extra))
                {
                    Console.WriteLine($"📊 Banka Ek Bilgi: {response.Bank_Extra}");
                }
            }
            else
            {
                Console.WriteLine($"❌ Kısmi İptal Hatası: {response.Sonuc_Str}");
                Console.WriteLine($"🔢 Hata Kodu: {response.Sonuc}");
                Console.WriteLine($"🏦 Banka Sonuç Kodu: {response.Banka_Sonuc_Kod}");
            }

            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 Beklenmeyen Hata: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Kısmi iade işlemi örneği
    /// </summary>
    public static async Task<TP_Islem_Iptal_Iade_Kismi2_Response> ProcessPartialRefundAsync(
        ITurkposService turkposService)
    {
        try
        {
            // Kısmi iade request'i oluştur
            var request = new TP_Islem_Iptal_Iade_Kismi2_Request
            {
                Durum = IptalIadeDurum.IADE, // İade işlemi (gün sonundan sonra)
                Siparis_ID = "3000159380", // Orijinal sipariş ID
                Tutar = 25.50m // Kısmi iade tutarı
            };

            Console.WriteLine("🔄 Kısmi İade İşlemi Başlatılıyor...");
            Console.WriteLine($"📋 Sipariş ID: {request.Siparis_ID}");
            Console.WriteLine($"💰 İade Tutarı: {request.Tutar:C2}");
            Console.WriteLine($"🔧 İşlem Türü: {request.Durum}");

            // PARAM API çağrısı
            var response = await turkposService.TP_Islem_Iptal_Iade_Kismi2Async(request);

            if (response.IsSuccess)
            {
                Console.WriteLine("✅ Kısmi İade İşlemi Başarılı!");
                Console.WriteLine($"📝 Sonuç: {response.Sonuc_Str}");
                Console.WriteLine($"🏦 Banka Auth Code: {response.Bank_AuthCode}");
                Console.WriteLine($"🆔 Banka Transaction ID: {response.Bank_Trans_ID}");
                Console.WriteLine($"📍 Host Ref Number: {response.Bank_HostRefNum}");
            }
            else
            {
                Console.WriteLine($"❌ Kısmi İade Hatası: {response.Sonuc_Str}");
                Console.WriteLine($"🔢 Hata Kodu: {response.Sonuc}");
            }

            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 Beklenmeyen Hata: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Tam iptal işlemi örneği
    /// </summary>
    public static async Task<TP_Islem_Iptal_Iade_Kismi2_Response> ProcessFullCancellationAsync(
        ITurkposService turkposService)
    {
        try
        {
            // Tam iptal request'i oluştur
            var request = new TP_Islem_Iptal_Iade_Kismi2_Request
            {
                Durum = IptalIadeDurum.IPTAL, // İptal işlemi
                Siparis_ID = "3000159380", // Orijinal sipariş ID
                Tutar = 100.00m // Tam tutar (IPTAL için tüm tutar yazılmalı)
            };

            Console.WriteLine("🔄 Tam İptal İşlemi Başlatılıyor...");
            Console.WriteLine($"📋 Sipariş ID: {request.Siparis_ID}");
            Console.WriteLine($"💰 İptal Tutarı: {request.Tutar:C2}");

            // PARAM API çağrısı
            var response = await turkposService.TP_Islem_Iptal_Iade_Kismi2Async(request);

            if (response.IsSuccess)
            {
                Console.WriteLine("✅ Tam İptal İşlemi Başarılı!");
                Console.WriteLine($"📝 Sonuç: {response.Sonuc_Str}");
                Console.WriteLine($"🏦 Banka Detayları: Auth Code: {response.Bank_AuthCode}");
            }
            else
            {
                Console.WriteLine($"❌ Tam İptal Hatası: {response.Sonuc_Str}");
            }

            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 Beklenmeyen Hata: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Enum kullanım örnekleri
    /// </summary>
    public static void DemonstrateEnumUsage()
    {
        Console.WriteLine("📚 IptalIadeDurum Enum Kullanımı:");
        
        // Enum değerlerini string'e çevir
        var iptalString = IptalIadeDurum.IPTAL.ToParamString();
        var iadeString = IptalIadeDurum.IADE.ToParamString();
        
        Console.WriteLine($"🔧 İptal Enum → String: {iptalString}");
        Console.WriteLine($"🔧 İade Enum → String: {iadeString}");
        
        // Validation kontrolü
        var validRequest = new TP_Islem_Iptal_Iade_Kismi2_Request
        {
            Durum = IptalIadeDurum.IPTAL,
            Siparis_ID = "TEST123",
            Tutar = 10.50m
        };
        
        if (validRequest.IsValid(out var results))
        {
            Console.WriteLine("✅ Request geçerli");
        }
        else
        {
            Console.WriteLine($"❌ Request geçersiz: {string.Join(", ", results.Select(r => r.ErrorMessage))}");
        }
    }
}

// TP_Ozel_Oran_Liste işlemi örneği
Console.WriteLine("=== TP_Ozel_Oran_Liste Özel Oran Listesi ===");

try
{
    var ozelOranResponse = await turkposService.TP_Ozel_Oran_ListeAsync();
    
    if (ozelOranResponse.IsSuccess)
    {
        Console.WriteLine($"✓ Özel Oran Listesi Başarılı: {ozelOranResponse.Sonuc_Str}");
        Console.WriteLine($"Toplam {ozelOranResponse.OzelOranBilgileri.Count} kart markası bulundu");
        
        foreach (var oranBilgi in ozelOranResponse.OzelOranBilgileri)
        {
            Console.WriteLine($"\n📊 Kart Markası: {oranBilgi.Kredi_Karti_Banka}");
            Console.WriteLine($"   SanalPOS ID: {oranBilgi.SanalPOS_ID}");
            Console.WriteLine($"   Tek Çekim: {oranBilgi.MO_01}%");
            Console.WriteLine($"   2 Taksit: {oranBilgi.MO_02}%");
            Console.WriteLine($"   3 Taksit: {oranBilgi.MO_03}%");
            
            // Kullanılabilir taksit seçeneklerini kontrol et
            var kullanilabilirTaksitler = new List<int>();
            for (int i = 1; i <= 12; i++)
            {
                if (oranBilgi.IsTaksitAvailable(i))
                {
                    kullanilabilirTaksitler.Add(i);
                }
            }
            Console.WriteLine($"   Kullanılabilir Taksit Sayıları: {string.Join(", ", kullanilabilirTaksitler)}");
        }
        
        // Belirli bir kart markası için oran sorgulama
        var axessOrani = ozelOranResponse.GetOzelOranByKartMarkasi("Axess");
        if (axessOrani != null)
        {
            Console.WriteLine($"\n🔍 Axess Kartı için 6 Taksit Oranı: {axessOrani.GetTaksitOrani(6)}%");
        }
        
        // 3 taksit için uygun kartları listele
        var ucTaksitKartlari = ozelOranResponse.GetAvailableCardsForInstallment(3);
        Console.WriteLine($"\n📈 3 Taksit için uygun kart markaları ({ucTaksitKartlari.Count} adet):");
        foreach (var kart in ucTaksitKartlari)
        {
            Console.WriteLine($"   - {kart.Kredi_Karti_Banka}: {kart.GetTaksitOrani(3)}%");
        }
    }
    else
    {
        Console.WriteLine($"✗ Özel Oran Listesi Başarısız: {ozelOranResponse.Sonuc_Str}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"✗ Özel Oran Listesi Hatası: {ex.Message}");
}

Console.WriteLine("\n" + new string('=', 80));

// TP_Ozel_Oran_SK_Liste işlemi örneği
Console.WriteLine("=== TP_Ozel_Oran_SK_Liste Özel Oran Son Kullanıcı Listesi ===");

try
{
    var ozelOranSKResponse = await turkposService.TP_Ozel_Oran_SK_ListeAsync();
    
    if (ozelOranSKResponse.IsSuccess)
    {
        Console.WriteLine($"✓ Özel Oran SK Listesi Başarılı: {ozelOranSKResponse.Sonuc_Str}");
        Console.WriteLine($"Toplam {ozelOranSKResponse.OzelOranSKBilgileri.Count} kart markası bulundu");
        
        foreach (var oranSKBilgi in ozelOranSKResponse.OzelOranSKBilgileri)
        {
            Console.WriteLine($"\n💳 Kart Markası: {oranSKBilgi.Kredi_Karti_Banka}");
            Console.WriteLine($"   SanalPOS ID: {oranSKBilgi.SanalPOS_ID}");
            Console.WriteLine($"   Tek Çekim: {oranSKBilgi.MO_01}%");
            Console.WriteLine($"   2 Taksit: {oranSKBilgi.MO_02}%");
            Console.WriteLine($"   3 Taksit: {oranSKBilgi.MO_03}%");
            Console.WriteLine($"   6 Taksit: {oranSKBilgi.MO_06}%");
            
            // Müşteri için uygun taksit seçeneklerini göster
            var customerOptions = oranSKBilgi.GetCustomerPaymentOptions();
            if (customerOptions.Any())
            {
                Console.WriteLine($"   👤 Müşteri Ödeme Seçenekleri: {string.Join(", ", customerOptions.Select(x => $"{x.TaksitSayisi} taksit: {x.Oran}%"))}");
            }
        }
        
        // Müşteri ödeme sayfası için tüm kart markalarının taksit seçenekleri
        Console.WriteLine("\n📋 Müşteri Ödeme Sayfası için Taksit Seçenekleri:");
        var allPaymentOptions = ozelOranSKResponse.GetAllCustomerPaymentOptions();
        foreach (var kartMarkasi in allPaymentOptions.Keys)
        {
            var options = allPaymentOptions[kartMarkasi];
            if (options.Any())
            {
                Console.WriteLine($"   🔸 {kartMarkasi}: {string.Join(", ", options.Select(x => $"{x.TaksitSayisi}T:{x.Oran}%"))}");
            }
        }
        
        // Belirli bir kart markası için bilgi al
        var axessSKBilgi = ozelOranSKResponse.GetKartMarkasiBilgi("Axess");
        if (axessSKBilgi != null)
        {
            Console.WriteLine($"\n🔍 Axess Kartı Son Kullanıcı Oranları:");
            Console.WriteLine($"   Tek Çekim: {axessSKBilgi.MO_01}%");
            Console.WriteLine($"   3 Taksit: {axessSKBilgi.MO_03}%");
            Console.WriteLine($"   6 Taksit: {axessSKBilgi.MO_06}%");
        }
    }
    else
    {
        Console.WriteLine($"✗ Özel Oran SK Listesi Başarısız: {ozelOranSKResponse.Sonuc_Str}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"✗ Özel Oran SK Listesi Hatası: {ex.Message}");
}

Console.WriteLine("\n" + new string('=', 80));

// TP_Ozel_Oran_SK_Guncelle işlemi örneği
Console.WriteLine("=== TP_Ozel_Oran_SK_Guncelle Özel Oran Güncelleme ===");

try
{
    // Önce mevcut oranları listele (ÖRNEKleme amaçlı)
    var ozelOranSKResponse = await turkposService.TP_Ozel_Oran_SK_ListeAsync();
    
    if (ozelOranSKResponse.IsSuccess && ozelOranSKResponse.OzelOranSKBilgileri.Any())
    {
        var firstOranBilgi = ozelOranSKResponse.OzelOranSKBilgileri.First();
        Console.WriteLine($"🔄 Güncelleme yapılacak kart markası: {firstOranBilgi.Kredi_Karti_Banka}");
        Console.WriteLine($"📋 Mevcut ID: {firstOranBilgi.Ozel_Oran_SK_ID}");
        
        // Özel oran güncelleme request'i oluştur
        var guncelleRequest = new TP_Ozel_Oran_SK_Guncelle_Request
        {
            Ozel_Oran_SK_ID = firstOranBilgi.Ozel_Oran_SK_ID,
            MO_1 = "0,000", // Tek çekim - komisyonsuz
            MO_2 = "1,25",  // 2 taksit - %1,25 müşteri oranı
            MO_3 = "2,50",  // 3 taksit - %2,50 müşteri oranı
            MO_4 = "100",   // 4 taksit - değişiklik yok (100 = aynı kalması)
            MO_5 = "100",   // 5 taksit - değişiklik yok
            MO_6 = "4,75",  // 6 taksit - %4,75 müşteri oranı
            MO_7 = "-1",    // 7 taksit - kullanılamaz (-1)
            MO_8 = "-1",    // 8 taksit - kullanılamaz
            MO_9 = "-1",    // 9 taksit - kullanılamaz
            MO_10 = "-1",   // 10 taksit - kullanılamaz
            MO_11 = "-1",   // 11 taksit - kullanılamaz
            MO_12 = "-1"    // 12 taksit - kullanılamaz
        };
        
        Console.WriteLine("🔧 Yeni müşteri oranları:");
        Console.WriteLine($"   - Tek Çekim: {guncelleRequest.MO_1}%");
        Console.WriteLine($"   - 2 Taksit: {guncelleRequest.MO_2}%");
        Console.WriteLine($"   - 3 Taksit: {guncelleRequest.MO_3}%");
        Console.WriteLine($"   - 6 Taksit: {guncelleRequest.MO_6}%");
        Console.WriteLine($"   - Diğer taksitler: Kullanılamaz veya değişiklik yok");
        
        // PARAM API çağrısı
        var guncelleResponse = await turkposService.TP_Ozel_Oran_SK_GuncelleAsync(guncelleRequest);
        
        if (guncelleResponse.IsSuccess)
        {
            Console.WriteLine("✅ Özel Oran Güncelleme Başarılı!");
            Console.WriteLine($"📝 Sonuç: {guncelleResponse.Sonuc_Str}");
            Console.WriteLine($"🔢 Sonuç Kodu: {guncelleResponse.Sonuc}");
            
            Console.WriteLine("\n💡 Güncelleme Sonrası Durum:");
            Console.WriteLine("   - Müşteriler artık güncellenmiş oranları görecek");
            Console.WriteLine("   - Firma oranı ile müşteri oranı arasındaki fark üye işyerinden tahsil edilecek");
            Console.WriteLine("   - 7-12 taksit seçenekleri müşteri için görünmeyecek");
        }
        else
        {
            Console.WriteLine($"❌ Özel Oran Güncelleme Hatası: {guncelleResponse.Sonuc_Str}");
            Console.WriteLine($"🔢 Hata Kodu: {guncelleResponse.Sonuc}");
        }
    }
    else
    {
        Console.WriteLine("⚠️ Güncellenebilir oran bulunamadı. Önce TP_Ozel_Oran_SK_Liste çağrısı yapılmalı.");
        
        // Test amaçlı sabit değerlerle örnek göster
        var testGuncelleRequest = new TP_Ozel_Oran_SK_Guncelle_Request
        {
            Ozel_Oran_SK_ID = 6, // Örnek ID
            MO_1 = "0.000",  // Nokta formatı da kabul edilir, otomatik virgüle çevrilir
            MO_2 = "1.25",   
            MO_3 = "2.50",   
            MO_4 = "3.75",   
            MO_5 = "5.00",   
            MO_6 = "6.25",   
            MO_7 = "-1",     // Kullanılamaz
            MO_8 = "-1",     
            MO_9 = "-1",     
            MO_10 = "-1",    
            MO_11 = "-1",    
            MO_12 = "-1"     
        };
        
        Console.WriteLine("📋 Test Request Örneği:");
        Console.WriteLine($"   ID: {testGuncelleRequest.Ozel_Oran_SK_ID}");
        Console.WriteLine($"   Tek Çekim: {testGuncelleRequest.MO_1}%");
        Console.WriteLine($"   2 Taksit: {testGuncelleRequest.MO_2}%");
        Console.WriteLine($"   3 Taksit: {testGuncelleRequest.MO_3}%");
        
        // Format normalizasyonu göster
        Console.WriteLine("\n🔄 Format Normalizasyonu:");
        Console.WriteLine($"   Önce: {testGuncelleRequest.MO_2} (nokta formatı)");
        testGuncelleRequest.NormalizeRateFormats();
        Console.WriteLine($"   Sonra: {testGuncelleRequest.MO_2} (virgül formatı)");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ TP_Ozel_Oran_SK_Guncelle Hatası: {ex.Message}");
}

Console.WriteLine("\n" + new string('=', 80));

// Oran güncelleme helper metodları örneği
Console.WriteLine("=== Oran Güncelleme Helper Metodları ===");

try
{
    var request = new TP_Ozel_Oran_SK_Guncelle_Request
    {
        Ozel_Oran_SK_ID = 6,
        MO_1 = "0,00",
        MO_2 = "1,25", 
        MO_3 = "2,50",
        MO_4 = "3,75",
        MO_5 = "5,00",
        MO_6 = "6,25",
        MO_7 = "-1",
        MO_8 = "-1",
        MO_9 = "-1",
        MO_10 = "-1",
        MO_11 = "-1",
        MO_12 = "-1"
    };
    
    Console.WriteLine("🔧 Helper Metod Kullanımı:");
    
    // Belirli taksit oranını güncelle
    request.SetTaksitOrani(3, "2,75"); // 3 taksiti %2,75 yap
    Console.WriteLine($"   3 Taksit güncellendi: {request.GetTaksitOrani(3)}%");
    
    // Toplu güncelleme senaryosu
    Console.WriteLine("\n📊 Müşteri Dostu Oran Planı:");
    request.SetTaksitOrani(1, "0,00");   // Tek çekim komisyonsuz
    request.SetTaksitOrani(2, "0,50");   // 2 taksit çok düşük
    request.SetTaksitOrani(3, "1,00");   // 3 taksit cazip
    request.SetTaksitOrani(6, "3,50");   // 6 taksit makul
    request.SetTaksitOrani(9, "100");    // 9 taksit değişiklik yok
    request.SetTaksitOrani(12, "100");   // 12 taksit değişiklik yok
    
    for (int i = 1; i <= 12; i++)
    {
        var oran = request.GetTaksitOrani(i);
        var aciklama = oran switch
        {
            "100" => "Değişiklik yok",
            "-1" => "Kullanılamaz",
            _ => $"{oran}% müşteri oranı"
        };
        Console.WriteLine($"   {i,2} Taksit: {aciklama}");
    }
    
    // Validation test
    if (request.IsValid(out var validationResults))
    {
        Console.WriteLine("\n✅ Request geçerli - API çağrısı yapılabilir");
    }
    else
    {
        Console.WriteLine($"\n❌ Request geçersiz: {string.Join(", ", validationResults.Select(r => r.ErrorMessage))}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Helper Metod Hatası: {ex.Message}");
}

Console.WriteLine("\n" + new string('=', 80));

// TP_Mutabakat_Ozet işlemi örneği
Console.WriteLine("=== TP_Mutabakat_Ozet Mutabakat Özet Sorgulama ===");

try
{
    // Mutabakat özet sorgulama request'i oluştur
    var mutabakatRequest = new TP_Mutabakat_Ozet_Request
    {
        GUID = "0c13d406-873b-403b-9c09-a5766840d98c", // Test GUID
        Tarih_Bas = "20.11.2024 00:00:00", // Başlangıç tarihi
        Tarih_Bit = "20.11.2024 23:59:59"  // Bitiş tarihi
    };
    
    Console.WriteLine("📊 Mutabakat özet sorgulanıyor...");
    Console.WriteLine($"📅 Tarih Aralığı: {mutabakatRequest.Tarih_Bas} - {mutabakatRequest.Tarih_Bit}");
    Console.WriteLine($"🔑 GUID: {mutabakatRequest.GUID}");
    
    // PARAM API çağrısı
    var mutabakatResponse = await turkposService.TP_Mutabakat_OzetAsync(mutabakatRequest);
    
    if (mutabakatResponse.IsSuccess)
    {
        Console.WriteLine("✅ Mutabakat Özet Sorgulaması Başarılı!");
        Console.WriteLine($"📝 Sonuç: {mutabakatResponse.Sonuc_Str}");
        Console.WriteLine($"📋 Toplam {mutabakatResponse.MutabakatOzetBilgileri.Count} özet bilgi bulundu");
        
        foreach (var ozetBilgi in mutabakatResponse.MutabakatOzetBilgileri)
        {
            Console.WriteLine("\n💰 Mutabakat Özet Detayları:");
            Console.WriteLine("─────────────────────────────────────");
            
            // Başarılı işlemler
            Console.WriteLine($"✅ Başarılı İşlemler:");
            Console.WriteLine($"   📊 Sayı: {ozetBilgi.Basarili_Islem_Sayi:N0} işlem");
            Console.WriteLine($"   💵 Tutar: {ozetBilgi.Basarili_Islem_Toplam_Tutar} TL");
            
            // İptal işlemler
            Console.WriteLine($"\n❌ İptal İşlemler:");
            Console.WriteLine($"   📊 Sayı: {ozetBilgi.Iptal_Islem_Sayi:N0} işlem");
            Console.WriteLine($"   💵 Tutar: {ozetBilgi.Iptal_Islem_Toplam_Tutar} TL");
            
            // İade işlemler
            Console.WriteLine($"\n🔄 İade İşlemler:");
            Console.WriteLine($"   📊 Sayı: {ozetBilgi.Iade_Islem_Sayi:N0} işlem");
            Console.WriteLine($"   💵 Tutar: {ozetBilgi.Iade_Islem_Toplam_Tutar} TL");
        }
        
        // Helper metodları kullanımı
        Console.WriteLine("\n🔍 Helper Metodlar ile Analiz:");
        var totalSuccessfulAmount = mutabakatResponse.GetTotalSuccessfulAmount();
        var totalSuccessfulCount = mutabakatResponse.GetTotalSuccessfulCount();
        
        if (totalSuccessfulAmount != null && totalSuccessfulCount.HasValue)
        {
            Console.WriteLine($"💹 En önemli metrik - Başarılı işlemler:");
            Console.WriteLine($"   📈 Toplam başarılı tutar: {totalSuccessfulAmount} TL");
            Console.WriteLine($"   🔢 Toplam başarılı sayı: {totalSuccessfulCount:N0} işlem");
            
            // Ortalama işlem tutarı hesapla
            if (totalSuccessfulCount > 0 && decimal.TryParse(totalSuccessfulAmount, out decimal tutar))
            {
                var avgAmount = tutar / totalSuccessfulCount.Value;
                Console.WriteLine($"   📊 Ortalama işlem tutarı: {avgAmount:F2} TL");
            }
        }
        
        Console.WriteLine("\n📈 İş Analizi Önerileri:");
        if (totalSuccessfulCount > 0)
        {
            Console.WriteLine("   ✅ İşlemler başarıyla gerçekleşiyor");
            Console.WriteLine("   💡 Günlük mutabakat kontrolü yapılabilir");
            Console.WriteLine("   📊 Trend analizi için haftalık özet çıkarılabilir");
        }
        else
        {
            Console.WriteLine("   ⚠️ Belirtilen tarih aralığında başarılı işlem bulunmuyor");
            Console.WriteLine("   💡 Tarih aralığını genişletmeyi deneyin");
            Console.WriteLine("   📅 Farklı günlerde kontrol yapın");
        }
    }
    else
    {
        Console.WriteLine($"❌ Mutabakat Özet Sorgulaması Hatası: {mutabakatResponse.Sonuc_Str}");
        Console.WriteLine($"🔢 Hata Kodu: {mutabakatResponse.Sonuc}");
        
        Console.WriteLine("\n🔍 Muhtemel Çözümler:");
        Console.WriteLine("   • GUID bilgisini kontrol edin");
        Console.WriteLine("   • Tarih formatını kontrol edin (dd.MM.yyyy HH:mm:ss)");
        Console.WriteLine("   • Tarih aralığının geçerli olduğundan emin olun");
        Console.WriteLine("   • API erişim yetkilerinizi kontrol edin");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ TP_Mutabakat_Ozet Hatası: {ex.Message}");
    
    if (ex.Message.Contains("GUID"))
    {
        Console.WriteLine("💡 GUID formatının doğru olduğundan emin olun (36 karakter)");
    }
    else if (ex.Message.Contains("Tarih"))
    {
        Console.WriteLine("💡 Tarih formatının doğru olduğundan emin olun (dd.MM.yyyy HH:mm:ss)");
    }
    
    Console.WriteLine("\n📋 Doğru kullanım örneği:");
    Console.WriteLine("   GUID: 0c13d406-873b-403b-9c09-a5766840d98c");
    Console.WriteLine("   Tarih_Bas: 20.11.2024 00:00:00");
    Console.WriteLine("   Tarih_Bit: 20.11.2024 23:59:59");
}

Console.WriteLine("\n" + new string('=', 80));

// Tarih aralığı örnekleri
Console.WriteLine("=== Mutabakat Özet Tarih Aralığı Örnekleri ===");

try
{
    var today = DateTime.Now;
    var yesterday = today.AddDays(-1);
    var oneWeekAgo = today.AddDays(-7);
    var oneMonthAgo = today.AddMonths(-1);
    
    Console.WriteLine("📅 Farklı Tarih Aralıkları:");
    
    // Dünkü işlemler
    var dunRequest = new TP_Mutabakat_Ozet_Request
    {
        GUID = "0c13d406-873b-403b-9c09-a5766840d98c",
        Tarih_Bas = yesterday.ToString("dd.MM.yyyy 00:00:00"),
        Tarih_Bit = yesterday.ToString("dd.MM.yyyy 23:59:59")
    };
    Console.WriteLine($"🗓️ Dün ({yesterday:dd.MM.yyyy}):");
    Console.WriteLine($"   Başlangıç: {dunRequest.Tarih_Bas}");
    Console.WriteLine($"   Bitiş: {dunRequest.Tarih_Bit}");
    
    // Bu haftaki işlemler
    var haftaRequest = new TP_Mutabakat_Ozet_Request
    {
        GUID = "0c13d406-873b-403b-9c09-a5766840d98c",
        Tarih_Bas = oneWeekAgo.ToString("dd.MM.yyyy 00:00:00"),
        Tarih_Bit = today.ToString("dd.MM.yyyy 23:59:59")
    };
    Console.WriteLine($"\n📊 Son 7 Gün ({oneWeekAgo:dd.MM.yyyy} - {today:dd.MM.yyyy}):");
    Console.WriteLine($"   Başlangıç: {haftaRequest.Tarih_Bas}");
    Console.WriteLine($"   Bitiş: {haftaRequest.Tarih_Bit}");
    
    // Bu ayki işlemler
    var ayRequest = new TP_Mutabakat_Ozet_Request
    {
        GUID = "0c13d406-873b-403b-9c09-a5766840d98c",
        Tarih_Bas = oneMonthAgo.ToString("dd.MM.yyyy 00:00:00"),
        Tarih_Bit = today.ToString("dd.MM.yyyy 23:59:59")
    };
    Console.WriteLine($"\n📈 Son 30 Gün ({oneMonthAgo:dd.MM.yyyy} - {today:dd.MM.yyyy}):");
    Console.WriteLine($"   Başlangıç: {ayRequest.Tarih_Bas}");
    Console.WriteLine($"   Bitiş: {ayRequest.Tarih_Bit}");
    
    // Validation kontrolü
    if (dunRequest.IsValid(out var validationResults))
    {
        Console.WriteLine("\n✅ Tüm tarih formatları geçerli");
    }
    else
    {
        Console.WriteLine($"\n❌ Validation hatası: {string.Join(", ", validationResults.Select(r => r.ErrorMessage))}");
    }
    
    Console.WriteLine("\n💡 Mutabakat Özet Kullanım Önerileri:");
    Console.WriteLine("   📅 Günlük mutabakat için: Dün 00:00:00 - 23:59:59");
    Console.WriteLine("   📊 Haftalık rapor için: 7 gün öncesi - bugün");
    Console.WriteLine("   📈 Aylık analiz için: 30 gün öncesi - bugün");
    Console.WriteLine("   🔄 Gerçek zamanlı için: Bugün 00:00:00 - şu anki saat");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Tarih işleme hatası: {ex.Message}");
}

Console.WriteLine("\n" + new string('=', 80));

// TP_Mutabakat_Detay işlemi örneği
Console.WriteLine("=== TP_Mutabakat_Detay Mutabakat Detay Sorgulama ===");

try
{
    // Mutabakat detay sorgulama request'i oluştur
    var mutabakatDetayRequest = new TP_Mutabakat_Detay_Request
    {
        Tarih = "14.04.2021 00:00:16" // İşlem tarihi
    };
    
    Console.WriteLine("🔍 Mutabakat detay sorgulanıyor...");
    Console.WriteLine($"📅 İşlem Tarihi: {mutabakatDetayRequest.Tarih}");
    Console.WriteLine($"🔑 GUID: ParamApiOptions'dan alınacak");
    
    // PARAM API çağrısı
    var mutabakatDetayResponse = await turkposService.TP_Mutabakat_DetayAsync(mutabakatDetayRequest);
    
    if (mutabakatDetayResponse.IsSuccess)
    {
        Console.WriteLine("✅ Mutabakat Detay Sorgulaması Başarılı!");
        Console.WriteLine($"📝 Sonuç: {mutabakatDetayResponse.Sonuc_Str}");
        Console.WriteLine($"📋 Toplam {mutabakatDetayResponse.MutabakatDetayBilgileri.Count} detay bulundu");
        
        // Detayları göster
        if (mutabakatDetayResponse.MutabakatDetayBilgileri.Any())
        {
            Console.WriteLine("\n💳 İşlem Detayları:");
            Console.WriteLine("─────────────────────────────────────");
            
            foreach (var detay in mutabakatDetayResponse.MutabakatDetayBilgileri.Take(5)) // İlk 5 detayı göster
            {
                Console.WriteLine($"\n🔸 İşlem #{mutabakatDetayResponse.MutabakatDetayBilgileri.IndexOf(detay) + 1}:");
                Console.WriteLine($"   📅 İşlem Tarihi: {detay.ISLEM_TARIHI}");
                Console.WriteLine($"   📅 Gün Sonu Tarihi: {detay.GUNSONU_TARIHI}");
                Console.WriteLine($"   📅 Valör Tarihi: {detay.VALOR_TARIHI}");
                Console.WriteLine($"   💳 Kart No: {detay.KART_NO}");
                Console.WriteLine($"   🏷️ İşlem Tipi: {detay.TRANSACTION_TIPI}");
                Console.WriteLine($"   🔢 Provizyon No: {detay.PROVIZYON_NO}");
                Console.WriteLine($"   📊 Taksit Sayısı: {detay.TAKSIT_SAYISI}");
                Console.WriteLine($"   💰 Provizyon Tutarı: {detay.PROVIZYON_TUTARI} TL");
                Console.WriteLine($"   💸 Komisyon Tutarı: {detay.KOMISYON_TUTARI} TL");
                Console.WriteLine($"   📈 Komisyon Oranı: {detay.KOMISYON_ORANI}%");
                Console.WriteLine($"   💵 Net Tutar: {detay.NET_TUTAR} TL");
                Console.WriteLine($"   📦 Sipariş No: {detay.SIPARIS_NO}");
                Console.WriteLine($"   🏦 Ana Kart Tipi: {detay.ANA_KART_TIPI}");
                Console.WriteLine($"   🏷️ Alt Kart Tipi: {detay.ALT_KART_TIPI}");
            }
            
            if (mutabakatDetayResponse.MutabakatDetayBilgileri.Count > 5)
            {
                Console.WriteLine($"\n   ... ve {mutabakatDetayResponse.MutabakatDetayBilgileri.Count - 5} detay daha");
            }
            
            // Özet istatistikler
            Console.WriteLine("\n📊 Özet İstatistikler:");
            Console.WriteLine("─────────────────────────────────────");
            
            var toplamProvizyon = mutabakatDetayResponse.MutabakatDetayBilgileri
                .Where(d => decimal.TryParse(d.PROVIZYON_TUTARI, out _))
                .Sum(d => decimal.Parse(d.PROVIZYON_TUTARI));
            
            var toplamKomisyon = mutabakatDetayResponse.MutabakatDetayBilgileri
                .Where(d => decimal.TryParse(d.KOMISYON_TUTARI, out _))
                .Sum(d => decimal.Parse(d.KOMISYON_TUTARI));
            
            var toplamNet = mutabakatDetayResponse.MutabakatDetayBilgileri
                .Where(d => decimal.TryParse(d.NET_TUTAR, out _))
                .Sum(d => decimal.Parse(d.NET_TUTAR));
            
            Console.WriteLine($"💰 Toplam Provizyon Tutarı: {toplamProvizyon:F2} TL");
            Console.WriteLine($"💸 Toplam Komisyon Tutarı: {toplamKomisyon:F2} TL");
            Console.WriteLine($"💵 Toplam Net Tutar: {toplamNet:F2} TL");
            Console.WriteLine($"📈 Ortalama Komisyon: {(toplamKomisyon / mutabakatDetayResponse.MutabakatDetayBilgileri.Count):F4} TL");
            
            // İşlem tipi dağılımı
            var islemTipleri = mutabakatDetayResponse.MutabakatDetayBilgileri
                .GroupBy(d => d.TRANSACTION_TIPI)
                .Select(g => new { Tip = g.Key, Adet = g.Count() })
                .OrderByDescending(x => x.Adet);
            
            Console.WriteLine("\n🏷️ İşlem Tipi Dağılımı:");
            foreach (var tip in islemTipleri)
            {
                Console.WriteLine($"   • {tip.Tip}: {tip.Adet} adet");
            }
            
            // Kart tipi dağılımı
            var kartTipleri = mutabakatDetayResponse.MutabakatDetayBilgileri
                .GroupBy(d => d.ANA_KART_TIPI)
                .Select(g => new { Tip = g.Key, Adet = g.Count() })
                .OrderByDescending(x => x.Adet);
            
            Console.WriteLine("\n🏦 Kart Tipi Dağılımı:");
            foreach (var tip in kartTipleri)
            {
                Console.WriteLine($"   • {tip.Tip}: {tip.Adet} adet");
            }
        }
        else
        {
            Console.WriteLine("📭 Belirtilen tarihte işlem detayı bulunamadı");
            Console.WriteLine("\n💡 Öneriler:");
            Console.WriteLine("   • Tarih formatını kontrol edin (dd.MM.yyyy HH:mm:ss)");
            Console.WriteLine("   • Farklı bir tarih deneyin");
            Console.WriteLine("   • İşlem yapıldığından emin olduğunuz bir tarihi seçin");
        }
    }
    else
    {
        Console.WriteLine($"❌ Mutabakat Detay Sorgulaması Hatası: {mutabakatDetayResponse.Sonuc_Str}");
        Console.WriteLine($"🔢 Hata Kodu: {mutabakatDetayResponse.Sonuc}");
        
        Console.WriteLine("\n🔍 Muhtemel Çözümler:");
        Console.WriteLine("   • GUID bilgisini kontrol edin");
        Console.WriteLine("   • Tarih formatını kontrol edin (dd.MM.yyyy HH:mm:ss)");
        Console.WriteLine("   • Geçerli bir işlem tarihi seçin");
        Console.WriteLine("   • API erişim yetkilerinizi kontrol edin");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ TP_Mutabakat_Detay Hatası: {ex.Message}");
    
    if (ex.Message.Contains("Tarih"))
    {
        Console.WriteLine("💡 Tarih formatının doğru olduğundan emin olun (dd.MM.yyyy HH:mm:ss)");
    }
    
    Console.WriteLine("\n📋 Doğru kullanım örneği:");
    Console.WriteLine("   Tarih: 14.04.2021 00:00:16");
    Console.WriteLine("   GUID: ParamApiOptions'dan otomatik alınır");
}

Console.WriteLine("\n" + new string('=', 80));

// Mutabakat detay tarih örnekleri
Console.WriteLine("=== Mutabakat Detay Tarih Örnekleri ===");

try
{
    var today = DateTime.Now;
    var yesterday = today.AddDays(-1);
    var oneWeekAgo = today.AddDays(-7);
    
    Console.WriteLine("📅 Farklı Tarih Örnekleri:");
    
    // Bugünkü detaylar
    var bugunRequest = new TP_Mutabakat_Detay_Request
    {
        Tarih = today.ToString("dd.MM.yyyy 00:00:00")
    };
    Console.WriteLine($"🗓️ Bugün: {bugunRequest.Tarih}");
    
    // Dünkü detaylar
    var dunDetayRequest = new TP_Mutabakat_Detay_Request
    {
        Tarih = yesterday.ToString("dd.MM.yyyy 00:00:00")
    };
    Console.WriteLine($"🗓️ Dün: {dunDetayRequest.Tarih}");
    
    // Geçen hafta
    var haftaDetayRequest = new TP_Mutabakat_Detay_Request
    {
        Tarih = oneWeekAgo.ToString("dd.MM.yyyy 00:00:00")
    };
    Console.WriteLine($"🗓️ Geçen Hafta: {haftaDetayRequest.Tarih}");
    
    Console.WriteLine("\n💡 Mutabakat Detay Kullanım Önerileri:");
    Console.WriteLine("   📅 Günlük detay kontrolü için: Belirli gün 00:00:00");
    Console.WriteLine("   🔍 İşlem araştırması için: Şüpheli işlem tarihi");
    Console.WriteLine("   📊 Analiz için: Yoğun işlem günleri");
    Console.WriteLine("   🏦 Banka mutabakatı için: Gün sonu 23:59:59");
    
    if (bugunRequest.IsValid(out var validationResults))
    {
        Console.WriteLine("\n✅ Tüm tarih formatları geçerli");
    }
    else
    {
        Console.WriteLine($"\n❌ Validation hatası: {string.Join(", ", validationResults.Select(r => r.ErrorMessage))}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Detay tarih işleme hatası: {ex.Message}");
} 

Console.WriteLine("\n" + new string('=', 80));

// TP_Islem_Sorgulama4 işlemi örneği
Console.WriteLine("=== TP_Islem_Sorgulama4 İşlem Durumu Sorgulama ===");

try
{
    // Dekont_ID ile sorgulama
    var dekonIdRequest = new TP_Islem_Sorgulama4_Request
    {
        Dekont_ID = "3000159388"
    };
    
    Console.WriteLine("🔍 Dekont_ID ile işlem durumu sorgulanıyor...");
    Console.WriteLine($"📋 Dekont ID: {dekonIdRequest.Dekont_ID}");
    Console.WriteLine($"🔑 GUID: ParamApiOptions'dan alınacak");
    
    // PARAM API çağrısı
    var dekonIdResponse = await turkposService.TP_Islem_Sorgulama4Async(dekonIdRequest);
    
    if (dekonIdResponse.IsSuccess && dekonIdResponse.DT_Bilgi.Any())
    {
        Console.WriteLine("✅ Dekont_ID ile Sorgulama Başarılı!");
        Console.WriteLine($"📝 Sonuç: {dekonIdResponse.Sonuc_Str}");
        Console.WriteLine($"📋 Bulunan İşlem Sayısı: {dekonIdResponse.DT_Bilgi.Count}");
        
        foreach (var islem in dekonIdResponse.DT_Bilgi)
        {
            Console.WriteLine("\n💳 İşlem Detayları:");
            Console.WriteLine("─────────────────────────────────────");
            Console.WriteLine($"🔸 Ödeme Sonucu: {islem.Odeme_Sonuc} - {islem.Odeme_Sonuc_Aciklama}");
            
            // İşlem durumunu kontrol et ve emoji ile göster
            switch (islem.Durum?.ToUpper())
            {
                case "SUCCESS":
                    Console.WriteLine("✅ İşlem Durumu: Başarılı");
                    break;
                case "FAIL":
                    Console.WriteLine("❌ İşlem Durumu: Başarısız");
                    break;
                case "BANK_FAIL":
                    Console.WriteLine("🏦 İşlem Durumu: Banka Hatası");
                    break;
                case "CANCEL":
                    Console.WriteLine("⛔ İşlem Durumu: İptal Edildi");
                    break;
                case "REFUND":
                    Console.WriteLine("💰 İşlem Durumu: İade Edildi");
                    break;
                case "PARTIAL_REFUND":
                    Console.WriteLine("💸 İşlem Durumu: Kısmi İade");
                    break;
                default:
                    Console.WriteLine($"ℹ️ İşlem Durumu: {islem.Durum ?? "Bilinmeyen"}");
                    break;
            }
            
            Console.WriteLine($"📅 İşlem Tarihi: {islem.Tarih}");
            Console.WriteLine($"📋 Dekont ID: {islem.Dekont_ID}");
            Console.WriteLine($"🛒 Sipariş ID: {islem.Siparis_ID}");
            Console.WriteLine($"🆔 İşlem ID: {islem.Islem_ID}");
            Console.WriteLine($"💳 Kart No: {islem.KK_No}");
            Console.WriteLine($"💰 Toplam Tutar: {islem.Toplam_Tutar} TL");
            Console.WriteLine($"💸 Komisyon Tutarı: {islem.Komisyon_Tutar} TL");
            Console.WriteLine($"📈 Komisyon Oranı: {islem.Komisyon_Oran}%");
            Console.WriteLine($"🔢 Taksit Sayısı: {islem.Taksit ?? 1}");
            Console.WriteLine($"💵 Toplam İade Tutarı: {islem.Toplam_Iade_Tutar} TL");
            Console.WriteLine($"🏦 Banka Sonuç Açıklaması: {islem.Banka_Sonuc_Aciklama}");
            Console.WriteLine($"📊 Ext Data: {islem.Ext_Data}");
        }
    }
    else if (dekonIdResponse.IsSuccess && !dekonIdResponse.DT_Bilgi.Any())
    {
        Console.WriteLine("📭 Belirtilen Dekont_ID için işlem bulunamadı");
        Console.WriteLine("\n💡 Öneriler:");
        Console.WriteLine("   • Dekont_ID'nin doğru olduğunu kontrol edin");
        Console.WriteLine("   • Farklı bir Dekont_ID deneyin");
        Console.WriteLine("   • Sipariş_ID veya Islem_ID ile deneyebilirsiniz");
    }
    else
    {
        Console.WriteLine($"❌ Dekont_ID Sorgulaması Hatası: {dekonIdResponse.Sonuc_Str}");
        Console.WriteLine($"🔢 Hata Kodu: {dekonIdResponse.Sonuc}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ TP_Islem_Sorgulama4 (Dekont_ID) Hatası: {ex.Message}");
}

Console.WriteLine("\n" + new string('-', 60));

// Sipariş ID ile sorgulama
try
{
    var siparisIdRequest = new TP_Islem_Sorgulama4_Request
    {
        Siparis_ID = "ORDER123456"
    };
    
    Console.WriteLine("🔍 Sipariş_ID ile işlem durumu sorgulanıyor...");
    Console.WriteLine($"🛒 Sipariş ID: {siparisIdRequest.Siparis_ID}");
    
    var siparisIdResponse = await turkposService.TP_Islem_Sorgulama4Async(siparisIdRequest);
    
    if (siparisIdResponse.IsSuccess && siparisIdResponse.DT_Bilgi.Any())
    {
        Console.WriteLine("✅ Sipariş_ID ile Sorgulama Başarılı!");
        
        foreach (var islem in siparisIdResponse.DT_Bilgi)
        {
            Console.WriteLine("\n💳 İşlem Özeti:");
            Console.WriteLine($"🔸 Sipariş: {islem.Siparis_ID}");
            Console.WriteLine($"🔸 Durum: {islem.Durum}");
            Console.WriteLine($"🔸 Tutar: {islem.Toplam_Tutar} TL");
            Console.WriteLine($"🔸 Tarih: {islem.Tarih}");
            
            // Ek bilgiler varsa göster
            if (!string.IsNullOrEmpty(islem.ID))
            {
                Console.WriteLine($"🔸 ID: {islem.ID}");
            }
            
            if (!string.IsNullOrEmpty(islem.Ortak_Odeme_ID))
            {
                Console.WriteLine($"🔸 Ortak Ödeme ID: {islem.Ortak_Odeme_ID}");
            }
            
            if (!string.IsNullOrEmpty(islem.Odeme_Yapan_GSM))
            {
                Console.WriteLine($"🔸 Ödeme Yapan GSM: {islem.Odeme_Yapan_GSM}");
            }
            
            if (!string.IsNullOrEmpty(islem.Iade_Tarih))
            {
                Console.WriteLine($"🔸 İade Tarihi: {islem.Iade_Tarih}");
            }
            
            if (!string.IsNullOrEmpty(islem.Islem_Tipi))
            {
                Console.WriteLine($"🔸 İşlem Tipi: {islem.Islem_Tipi}");
            }
            
            if (!string.IsNullOrEmpty(islem.SanalPOS_Tip))
            {
                Console.WriteLine($"🔸 Sanal POS Tipi: {islem.SanalPOS_Tip}");
            }
            
            if (!string.IsNullOrEmpty(islem.SPS_UID))
            {
                Console.WriteLine($"🔸 SPS UID: {islem.SPS_UID}");
            }
            
            if (!string.IsNullOrEmpty(islem.SanalPOS_ID))
            {
                Console.WriteLine($"🔸 Sanal POS ID: {islem.SanalPOS_ID}");
            }
            
            if (!string.IsNullOrEmpty(islem.Islem_GUID))
            {
                Console.WriteLine($"🔸 İşlem GUID: {islem.Islem_GUID}");
            }
        }
    }
    else
    {
        Console.WriteLine($"📭 Belirtilen Sipariş_ID için işlem bulunamadı: {siparisIdResponse.Sonuc_Str}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ TP_Islem_Sorgulama4 (Sipariş_ID) Hatası: {ex.Message}");
}

Console.WriteLine("\n" + new string('-', 60));

// İşlem ID ile sorgulama
try
{
    var islemIdRequest = new TP_Islem_Sorgulama4_Request
    {
        Islem_ID = "TXN789123"
    };
    
    Console.WriteLine("🔍 Islem_ID ile işlem durumu sorgulanıyor...");
    Console.WriteLine($"🆔 İşlem ID: {islemIdRequest.Islem_ID}");
    
    var islemIdResponse = await turkposService.TP_Islem_Sorgulama4Async(islemIdRequest);
    
    if (islemIdResponse.IsSuccess)
    {
        Console.WriteLine($"✅ Islem_ID Sorgulaması: {islemIdResponse.Sonuc_Str}");
        
        if (islemIdResponse.DT_Bilgi.Any())
        {
            var islem = islemIdResponse.DT_Bilgi.First();
            Console.WriteLine($"🔸 İşlem Durumu: {islem.Durum}");
            Console.WriteLine($"🔸 Banka Sonuç: {islem.Banka_Sonuc_Aciklama}");