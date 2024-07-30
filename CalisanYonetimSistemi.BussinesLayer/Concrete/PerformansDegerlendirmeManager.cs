using CalisanYonetimSistemi.BussinesLayer.Abstract;
using CalisanYonetimSistemi.EntityLayer.Concrete;
using CalisanYonetimSistemi.EntityLayer.Concrete.Support;
using Microsoft.EntityFrameworkCore;

namespace CalisanYonetimSistemi.BussinesLayer.Concrete
{
    public class PerformansDegerlendirmeManager : Manager<PerformansDegerlendirme, int>, IPerformansDegerlendirmeManager
    {
        public async Task<Dictionary<string, PerformansTipiOrtPuan>> DepartmanOrtPerformansPuan()
        {
            var users = await _repo.dbContext.Users.ToListAsync();


            var sonuc = users
                .GroupBy(u => u.Departman)
                .ToDictionary(
                                g => g.Key,
                                g => new PerformansTipiOrtPuan
                                {
                                    UretkenlikOrtalama = g.Sum(u => u.ToplamUretkenlikPuan) / g.Count(),
                                    TakimCalismasiOrtalama = g.Sum(u => u.ToplamTakimCalismasiPuan) / g.Count(),
                                    AnalitiklikOrtalama = g.Sum(u => u.ToplamAnalitiklikPuan) / g.Count()
                                });
            if (sonuc == null || sonuc.Count == 0)
            {
                throw new Exception("Departman çalışanlarının puanlanmış performansları bulunmamaktadır. !");
            }


            return sonuc;

        }

        public async Task<Dictionary<string, AylikPerformansTrendi>> DepartmanOrtPerformansAylikTrendi(int year)
        {

            var performansDegerlendirmeler = await _repo.dbContext.PerformansDegerlendirmeleri
                .Where(p => p.CreateDate.Year == year)
                .ToListAsync();

            var performansVeKullaniciBilgileri =
                from pd in performansDegerlendirmeler
                join u in _repo.dbContext.Users on pd.UserId equals u.Id
                select new
                {
                    Departman = u.Departman,
                    Ay = pd.CreateDate.Month,
                    PerformansTipi = pd.PerformansTipi,
                    OrtalamaPuan = (double?)pd.PerformansPuani
                };

            var performansDegisimler = performansVeKullaniciBilgileri
                .GroupBy(p => new { p.Departman, p.Ay, p.PerformansTipi })
                .Select(g => new
                {
                    Departman = g.Key.Departman,
                    Ay = g.Key.Ay,
                    PerformansTipi = g.Key.PerformansTipi,
                    OrtalamaPuan = g.Average(p => (double?)p.OrtalamaPuan)
                })
                .ToList();

            var departmanOrtPuanlari = new Dictionary<string, AylikPerformansTrendi>();

            var aylar = new[] { "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık" };

            foreach (var departman in performansDegisimler.GroupBy(p => p.Departman))  // Departman = modelimin keyi
            {
                var aylikPerformansTrendi = new AylikPerformansTrendi();  // Modelimin value si

                for (int i = 1; i <= 12; i++)
                {
                    var aylikDepartmanDegerlendirme = departman.Where(p => p.Ay == i);   // İlgili departmanın 1. ayını getir


                    aylikPerformansTrendi.Uretkenlik[aylar[i - 1]] = aylikDepartmanDegerlendirme  // İlgili departmanın 1. ayın üretkenlik ort puanını, modelimin valuesinin ocak keyinin valuesini yerleştir. 
                    .Where(p => p.PerformansTipi == PerformanTipi.Uretkenlik)
                    .Select(p => p.OrtalamaPuan)
                    .FirstOrDefault();

                    aylikPerformansTrendi.TakimCalismasi[aylar[i - 1]] = aylikDepartmanDegerlendirme  // İlgili departmanın 1. ayın üretkenlik ort puanını, modelimin valuesinin ocak keyinin valuesini yerleştir. 
                    .Where(p => p.PerformansTipi == PerformanTipi.TakimCalismasi)
                    .Select(p => p.OrtalamaPuan)
                    .FirstOrDefault();

                    aylikPerformansTrendi.Analitiklik[aylar[i - 1]] = aylikDepartmanDegerlendirme  // İlgili departmanın 1. ayın üretkenlik ort puanını, modelimin valuesinin ocak keyinin valuesini yerleştir. 
                    .Where(p => p.PerformansTipi == PerformanTipi.Analitiklik)
                    .Select(p => p.OrtalamaPuan)
                    .FirstOrDefault();

                }

                departmanOrtPuanlari[departman.Key] = aylikPerformansTrendi;  //IT ilk 12 ay için 3 tip puanın da ortalamasını aldı. Sırayla diğer departmanlar
            }

            if (departmanOrtPuanlari == null || departmanOrtPuanlari.Count == 0)
            {
                throw new Exception("Departman çalışanlarının puanlanmış performansları bulunmamaktadır. !");
            }

            return departmanOrtPuanlari;
        }

        public async Task<Dictionary<string, AylikPerformansTrendi>> BireyselOrtPerformansAylikTrendi(int year)
        {
            var performansDegerlendirmeler = await _repo.dbContext.PerformansDegerlendirmeleri
                .Where(p => p.CreateDate.Year == year)
                .ToListAsync();

            var performansVeKullaniciBilgileri =
                from pd in performansDegerlendirmeler
                join u in _repo.dbContext.Users on pd.UserId equals u.Id
                select new
                {
                    UserId = u.Id,
                    Ad = u.Ad,
                    Ay = pd.CreateDate.Month,
                    PerformansTipi = pd.PerformansTipi,
                    OrtalamaPuan = (double?)pd.PerformansPuani
                };

            var performansDegisimler = performansVeKullaniciBilgileri
                .GroupBy(p => new { p.Ad, p.UserId, p.Ay, p.PerformansTipi })
                .Select(g => new
                {
                    UserId = g.Key.UserId,
                    Ad = g.Key.Ad,
                    Ay = g.Key.Ay,
                    PerformansTipi = g.Key.PerformansTipi,
                    OrtalamaPuan = g.Average(p => p.OrtalamaPuan)
                })
                .ToList();

            var bireyselOrtPuanlari = new Dictionary<string, AylikPerformansTrendi>();

            var aylar = new[] { "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık" };

            foreach (var ad in performansDegisimler.GroupBy(p => new { p.UserId, p.Ad }))  // ad = modelimin keyi
            {
                var aylikBireyselPerformansTrendi = new AylikPerformansTrendi();  // Modelimin value si

                for (int i = 1; i <= 12; i++)
                {
                    var aylikBireyselDegerlendirme = ad.Where(p => p.Ay == i);   // İlgili adın 1. ayını getir

                    aylikBireyselPerformansTrendi.Uretkenlik[aylar[i - 1]] = aylikBireyselDegerlendirme  // İlgili calisanin 1. ayın üretkenlik ort puanını, modelimin valuesinin ocak keyinin valuesini yerleştir. 
                    .Where(p => p.PerformansTipi == PerformanTipi.Uretkenlik)
                    .Select(p => p.OrtalamaPuan)
                    .FirstOrDefault();

                    aylikBireyselPerformansTrendi.TakimCalismasi[aylar[i - 1]] = aylikBireyselDegerlendirme  // İlgili calisanin 1. ayın üretkenlik ort puanını, modelimin valuesinin ocak keyinin valuesini yerleştir. 
                    .Where(p => p.PerformansTipi == PerformanTipi.TakimCalismasi)
                    .Select(p => p.OrtalamaPuan)
                    .FirstOrDefault();

                    aylikBireyselPerformansTrendi.Analitiklik[aylar[i - 1]] = aylikBireyselDegerlendirme  // İlgili calisanin 1. ayın üretkenlik ort puanını, modelimin valuesinin ocak keyinin valuesini yerleştir. 
                    .Where(p => p.PerformansTipi == PerformanTipi.Analitiklik)
                    .Select(p => p.OrtalamaPuan)
                    .FirstOrDefault();
                }
                var kullaniciAnahtari = $"{ad.Key.Ad} (ID: {ad.Key.UserId})";
                bireyselOrtPuanlari[kullaniciAnahtari] = aylikBireyselPerformansTrendi;  //Ali ilk 12 ay için 3 tip puanın da ortalamasını aldı. Sırayla diğer isimler
            }

            if (bireyselOrtPuanlari == null || bireyselOrtPuanlari.Count == 0)
            {
                throw new Exception("Departman çalışanlarının ilgili yıl için puanlanmış performansları bulunmamaktadır. !");
            }


            return bireyselOrtPuanlari;
        }


        public async Task<Dictionary<string, Dictionary<string, string>>> BireyselPerformansDegisimAnomali(int year)
        {

            var performansDegerlendirmeler = await _repo.dbContext.PerformansDegerlendirmeleri
                .Where(p => p.CreateDate.Year == year)
                .ToListAsync();

            var performansVeKullaniciBilgileri =
                from pd in performansDegerlendirmeler
                join u in _repo.dbContext.Users on pd.UserId equals u.Id
                select new
                {
                    UserId = u.Id,
                    Ad = u.Ad,
                    Ay = pd.CreateDate.Month,
                    PerformansTipi = pd.PerformansTipi,
                    OrtalamaPuan = (double?)pd.PerformansPuani
                };

            var performansDegisimler = performansVeKullaniciBilgileri
                .GroupBy(p => new { p.UserId, p.Ad, p.Ay, p.PerformansTipi })
                .Select(g => new
                {
                    UserId = g.Key.UserId,
                    Ad = g.Key.Ad,
                    Ay = g.Key.Ay,
                    PerformansTipi = g.Key.PerformansTipi,
                    OrtalamaPuan = g.Average(p => p.OrtalamaPuan)
                })
                .ToList();

            var kullaniciDegisimler = new Dictionary<string, Dictionary<string, string>>();
            var aylar = new[] { "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık" };

            foreach (var kullanici in performansDegisimler.GroupBy(p => new { p.UserId, p.Ad }))
            {
                var degisimler = new Dictionary<string, string>();

                for (int i = 1; i < 12; i++)
                {
                    var oncekiAyDegerlendirme = kullanici.FirstOrDefault(p => p.Ay == i);
                    var suankiAyDegerlendirme = kullanici.FirstOrDefault(p => p.Ay == i + 1);

                    if (oncekiAyDegerlendirme != null && suankiAyDegerlendirme != null)
                    {
                        var performansTipleri = Enum.GetValues(typeof(PerformanTipi)).Cast<PerformanTipi>();

                        foreach (var performansTipi in performansTipleri)
                        {
                            var oncekiAyPuan = oncekiAyDegerlendirme.PerformansTipi == performansTipi ? oncekiAyDegerlendirme.OrtalamaPuan : (double?)null;
                            var suankiAyPuan = suankiAyDegerlendirme.PerformansTipi == performansTipi ? suankiAyDegerlendirme.OrtalamaPuan : (double?)null;

                            if (oncekiAyPuan.HasValue && suankiAyPuan.HasValue)
                            {
                                var degisim = (suankiAyPuan.Value - oncekiAyPuan.Value) / oncekiAyPuan.Value * 100;
                                if (Math.Abs(degisim) >= 50)
                                {
                                    var degisimTipi = performansTipi.ToString();
                                    degisimler[$"{aylar[i - 1]} {degisimTipi}"] = degisim > 0 ? "+" : "-";
                                }
                            }
                        }
                    }
                }

                if (degisimler.Any())
                {
                    var kullaniciAnahtari = $"{kullanici.Key.Ad} (ID: {kullanici.Key.UserId})";
                    kullaniciDegisimler[kullaniciAnahtari] = degisimler;
                }
            }

            return kullaniciDegisimler;
        }




        public async Task<Dictionary<string, Dictionary<string, string>>> DepartmanPerformansDegisimAnomali(int year)
        {
            var performansDegerlendirmeler = await _repo.dbContext.PerformansDegerlendirmeleri
               .Where(p => p.CreateDate.Year == year)
               .ToListAsync();


            var performansVeKullaniciBilgileri =
                from pd in performansDegerlendirmeler
                join u in _repo.dbContext.Users on pd.UserId equals u.Id
                select new
                {
                    Departman = u.Departman,
                    Ay = pd.CreateDate.Month,
                    PerformansTipi = pd.PerformansTipi,
                    OrtalamaPuan = (double?)pd.PerformansPuani
                };

            var performansDegisimler = performansVeKullaniciBilgileri
                .GroupBy(p => new { p.Departman, p.Ay, p.PerformansTipi })
                .Select(g => new
                {
                    Departman = g.Key.Departman,
                    Ay = g.Key.Ay,
                    PerformansTipi = g.Key.PerformansTipi,
                    OrtalamaPuan = g.Average(p => p.OrtalamaPuan)
                })
                .ToList();

            var departmanDegisimler = new Dictionary<string, Dictionary<string, string>>();
            var aylar = new[] { "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık" };

            foreach (var departman in performansDegisimler.GroupBy(p => p.Departman))
            {
                var degisimler = new Dictionary<string, string>();

                for (int i = 1; i < 12; i++)
                {
                    var oncekiAyDegerlendirme = departman.FirstOrDefault(p => p.Ay == i);
                    var suankiAyDegerlendirme = departman.FirstOrDefault(p => p.Ay == i + 1);

                    if (oncekiAyDegerlendirme != null && suankiAyDegerlendirme != null)
                    {
                        var performansTipleri = Enum.GetValues(typeof(PerformanTipi)).Cast<PerformanTipi>();

                        foreach (var performansTipi in performansTipleri)
                        {
                            var oncekiAyPuan = oncekiAyDegerlendirme.PerformansTipi == performansTipi ? oncekiAyDegerlendirme.OrtalamaPuan : (double?)null;
                            var suankiAyPuan = suankiAyDegerlendirme.PerformansTipi == performansTipi ? suankiAyDegerlendirme.OrtalamaPuan : (double?)null;

                            if (oncekiAyPuan.HasValue && suankiAyPuan.HasValue)
                            {
                                var degisim = (suankiAyPuan.Value - oncekiAyPuan.Value) / oncekiAyPuan.Value * 100;
                                if (Math.Abs(degisim) >= 50)
                                {
                                    var degisimTipi = performansTipi.ToString();
                                    degisimler[$"{aylar[i - 1]} {degisimTipi}"] = degisim > 0 ? "+" : "-";
                                }
                            }
                        }
                    }
                }

                if (degisimler.Any())
                {
                    departmanDegisimler[departman.Key] = degisimler;
                }
            }

            return departmanDegisimler;
        }

    }
}

