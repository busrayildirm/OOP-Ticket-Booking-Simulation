# 🎫 Etkinlik ve Bilet Yönetim Sistemi (Event Management)

Bu proje, C# dili ile geliştirilmiş; konser, tiyatro, spor ve konferans gibi etkinliklerin bilet satış, iade ve raporlama süreçlerini yöneten kapsamlı bir otomasyon sistemidir. Proje, **Sanal Metotlar (Virtual Methods)** ve **Polimorfizm** prensiplerini etkin bir şekilde kullanmaktadır.

## 🏗️ Nesne Yönelimli Yapı ve Özellikler

Sistem dört ana bileşen üzerine kurgulanmıştır:

### 1. Etkinlik (Temel Sınıf)
Tüm etkinliklerin ortak özelliklerini (ad, tarih, mekan, kapasite, taban fiyat vb.) tutar. 
- **FiyatHesapla():** Dinamik fiyatlandırma ve erken kayıt indirimi uygular.
- **IadeOrani():** Etkinliğe kalan süreye göre dinamik iade oranları döndürür.
- **Doluluk():** Anlık doluluk oranını hesaplar.

### 2. Özelleştirilmiş Alt Sınıflar (Kalıtım)
Temel sınıftaki metotlar ihtiyaca göre `override` edilerek özelleştirilmiştir:
- **Konser:** Servis ücreti eklenmiş özel fiyatlandırma.
- **Konferans:** Farklılaştırılmış iade politikası.
- **Tiyatro & Spor:** Temel sistem özelliklerini doğrudan kullanan yapılar.

### 3. Kullanıcı ve Cüzdan Yönetimi
- **BiletAl():** Kullanıcı bazlı limit kontrolü (Max 8 bilet) ve bakiye kontrolü yapar.
- **BiletIadeEt():** İade tutarını hesaplayarak kullanıcı cüzdanına geri yükler.

### 4. Gelişmiş Raporlama Sistemi
Sistemin genel durumunu özetleyen iki ana rapor sunar:
- **Kategori Bazlı Rapor:** Doluluk oranları, satış sayıları ve net gelir analizi.
- **Kullanıcı Bazlı Rapor:** Satın alınan biletlerin ve mevcut bakiyelerin takibi.

## 🛠️ Teknik Özellikler
- **Dil:** C#
- **Mimari:** OOP (Virtual/Override, Inheritance, Encapsulation)


