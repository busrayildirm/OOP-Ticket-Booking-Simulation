// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;


class Etkinlik
{
    public string Ad;
    public DateTime Tarih;
    public string Mekan;
    public string Kategori;
    public int Kapasite;
    public int Satilan;
    public double TabanFiyat;
    public double ToplamGelir;
    public double ToplamIade;

    public Etkinlik(string ad, DateTime tarih, string mekan, string kategori, int kapasite, double fiyat)
    {
        Ad = ad;
        Tarih = tarih;
        Mekan = mekan;
        Kategori = kategori;
        Kapasite = kapasite;
        TabanFiyat = fiyat;
        Satilan = 0;
        ToplamGelir = 0;
        ToplamIade = 0;
    }

    public double Doluluk()
    {
        return (double)Satilan / Kapasite;
    }

 
    public virtual double FiyatHesapla(DateTime satinAlma)
    {
        double fiyat = TabanFiyat;

        if (Doluluk() > 0.8)
            fiyat *= 1.25; // %25 artış
        else if (Doluluk() > 0.5)
            fiyat *= 1.10; // %10 artış

        if ((Tarih - satinAlma).TotalDays >= 14)
            fiyat *= 0.9;

        return Math.Round(fiyat, 2);
    }

    public virtual double IadeOrani(DateTime iptal)
    {
        double saat = (Tarih - iptal).TotalHours;
        if (saat > 72) return 1.0;
        else if (saat > 24) return 0.5;
        else return 0.0;
    }
}


class Konser : Etkinlik
{
    public Konser(string ad, DateTime tarih, string mekan, int kapasite, double fiyat)
        : base(ad, tarih, mekan, "Konser", kapasite, fiyat) { }

    public override double FiyatHesapla(DateTime satinAlma)
    {
     
        return base.FiyatHesapla(satinAlma) + 5;
    }
}

class Tiyatro : Etkinlik
{
    public Tiyatro(string ad, DateTime tarih, string mekan, int kapasite, double fiyat)
        : base(ad, tarih, mekan, "Tiyatro", kapasite, fiyat) { }
}

class Spor : Etkinlik
{
    public Spor(string ad, DateTime tarih, string mekan, int kapasite, double fiyat)
        : base(ad, tarih, mekan, "Spor", kapasite, fiyat) { }
}

class Konferans : Etkinlik
{
    public Konferans(string ad, DateTime tarih, string mekan, int kapasite, double fiyat)
        : base(ad, tarih, mekan, "Konferans", kapasite, fiyat) { }

    public override double IadeOrani(DateTime iptal)
    {
        double oran = base.IadeOrani(iptal);
        return Math.Max(0, oran - 0.1); // %10 daha az iade
    }
}


class Kullanici
{
    public string Ad;
    public double Cuzdan;
    public int ToplamBilet;
    public Dictionary<string, int> Biletler = new();

    public Kullanici(string ad, double cuzdan)
    {
        Ad = ad;
        Cuzdan = cuzdan;
        ToplamBilet = 0;
    }

    public void BiletAl(Etkinlik e, int adet, DateTime tarih)
    {
        int kategoriLimit = (e.Kategori == "Konser" || e.Kategori == "Spor") ? 3 : 2;

        if (ToplamBilet + adet > 8)
        {
            Console.WriteLine($"{Ad}: Toplam 8 bilet sınırı aşıldı!");
            return;
        }

        if (Biletler.ContainsKey(e.Kategori) && Biletler[e.Kategori] + adet > kategoriLimit)
        {
            Console.WriteLine($"{Ad}: {e.Kategori} kategorisi için limit doldu!");
            return;
        }

        double fiyat = e.FiyatHesapla(tarih) * adet;

        if (Cuzdan < fiyat)
        {
            Console.WriteLine($"{Ad}: Yetersiz bakiye!");
            return;
        }

        Cuzdan -= fiyat;
        e.Satilan += adet;
        e.ToplamGelir += fiyat;
        ToplamBilet += adet;

        if (Biletler.ContainsKey(e.Kategori))
            Biletler[e.Kategori] += adet;
        else
            Biletler[e.Kategori] = adet;

        Console.WriteLine($"{Ad}: {adet} adet {e.Ad} bileti alındı. (Toplam {fiyat:F2} ₺)");
    }

    public void BiletIadeEt(Etkinlik e, DateTime iptal)
    {
        if (!Biletler.ContainsKey(e.Kategori) || Biletler[e.Kategori] == 0)
        {
            Console.WriteLine($"{Ad}: İade edilecek bilet yok.");
            return;
        }

        double oran = e.IadeOrani(iptal);
        if (oran == 0)
        {
            Console.WriteLine($"{Ad}: Etkinliğe az kaldı, iade yapılamaz.");
            return;
        }

        double iade = e.FiyatHesapla(iptal) * oran;
        Cuzdan += iade;
        e.ToplamIade += iade;
        e.Satilan--;
        e.ToplamGelir -= iade;
        Biletler[e.Kategori]--;
        ToplamBilet--;

        Console.WriteLine($"{Ad}: %{oran * 100} oranında {iade:F2} ₺ iade edildi.");
    }
}



class Raporlama
{
    public List<Etkinlik> Etkinlikler;
    public List<Kullanici> Kullanicilar;

    public Raporlama(List<Etkinlik> etkinlikler, List<Kullanici> kullanicilar)
    {
        Etkinlikler = etkinlikler;
        Kullanicilar = kullanicilar;
    }

    public void KategoriRaporu()
    {
        Console.WriteLine("\n Kategori Bazlı Rapor ");
        foreach (var e in Etkinlikler)
        {
            double doluluk = e.Doluluk() * 100;
            Console.WriteLine($"{e.Kategori,-10}: {e.Satilan} bilet, %{doluluk:F1} doluluk, Net gelir: {e.ToplamGelir - e.ToplamIade:F2} ₺");
        }
    }

    public void KullaniciRaporu()
    {
        Console.WriteLine("\n Kullanıcı Bazlı Rapor ");
        foreach (var k in Kullanicilar)
        {
            Console.WriteLine($"{k.Ad} (Cüzdan: {k.Cuzdan:F2} ₺)");
            foreach (var b in k.Biletler)
                Console.WriteLine($"   {b.Key}: {b.Value} bilet");
        }
    }
}



class Program
{
    static void Main()
    {
        // Etkinlikler
        Konser konser = new Konser("RockFest", DateTime.Now.AddDays(20), "Ankara Arena", 100, 200);
        Tiyatro tiyatro = new Tiyatro("Kral Lear", DateTime.Now.AddDays(10), "İzmir Sahne", 50, 150);
        Spor mac = new Spor("Derbi", DateTime.Now.AddDays(25), "İstanbul Stadyumu", 200, 250);
        Konferans seminer = new Konferans("Yapay Zeka 2025", DateTime.Now.AddDays(30), "Kayseri Kongre", 80, 300);

        // Kullanıcılar
        Kullanici ali = new Kullanici("Ali", 1000);
        Kullanici ayse = new Kullanici("Ayşe", 1200);

        // Bilet alma ve iade işlemleri
        ali.BiletAl(konser, 2, DateTime.Now);
        ayse.BiletAl(tiyatro, 2, DateTime.Now.AddDays(-5)); // erken kayıt indirimi
        ayse.BiletIadeEt(tiyatro, DateTime.Now.AddDays(7)); // iade

        // Raporlama
        List<Etkinlik> etkinlikler = new() { konser, tiyatro, mac, seminer };
        List<Kullanici> kullanicilar = new() { ali, ayse };
        Raporlama rapor = new Raporlama(etkinlikler, kullanicilar);

        rapor.KategoriRaporu();
        rapor.KullaniciRaporu();
    }
}
