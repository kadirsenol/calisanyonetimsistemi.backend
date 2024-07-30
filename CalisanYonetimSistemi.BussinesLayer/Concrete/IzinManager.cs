using CalisanYonetimSistemi.BussinesLayer.Abstract;
using CalisanYonetimSistemi.EntityLayer.Concrete;
using CalisanYonetimSistemi.EntityLayer.Concrete.Support;
using CalisanYonetimSistemi.EntityLayer.Concrete.VMs.IzinVm;
using Microsoft.EntityFrameworkCore;

namespace CalisanYonetimSistemi.BussinesLayer.Concrete
{
    public class IzinManager : Manager<IzinTalep, int>, IIzinManager
    {
        public async Task<bool> CheckIzinTarihi(IzinTalepInsertVm izinTalepInsertVm, int userid)
        {
            IzinTalep izinTalebi = await _repo.dbContext.IzinTalepleri.Where(p => p.UserId == userid
            && p.BaslangicTarih.ToString() == izinTalepInsertVm.BaslangicTarih
            && p.BitisTarih.ToString() == izinTalepInsertVm.BitisTarih).FirstOrDefaultAsync();

            if (izinTalebi != null)
            {
                throw new Exception("Aynı tarihte bir adet izin talebiniz mevcut !");
            }
            else
            {
                return true;
            }
        }

        public async Task<Dictionary<string, Dictionary<string, int>>> DepartmanBazliIzinKullanimi()
        {
            var izinler = await (from i in _repo.dbContext.IzinTalepleri
                                 join u in _repo.dbContext.Users on i.UserId equals u.Id
                                 where i.Onay == true // Onaylanmış izinler
                                 select new
                                 {
                                     u.Departman,
                                     i.IzinTuru,
                                     i.BaslangicTarih,
                                     i.BitisTarih
                                 })
                         .ToListAsync(); // Veriyi veritabanından çek

            var departmanIzinKullanimi = izinler
                .Select(x => new
                {
                    x.Departman,
                    x.IzinTuru,
                    ToplamGunSayisi = (x.BitisTarih.DayNumber - x.BaslangicTarih.DayNumber) + 1 // Gün sayısını hesapladım +1 koyulmasi gerekiyor.
                })
                .GroupBy(x => x.Departman)
                .ToDictionary(
                    g => g.Key,
                    g => new Dictionary<string, int>
                    {
                { "TatilIzinKullanimi", g.Where(x => x.IzinTuru == IzinTuru.Tatil).Sum(x => x.ToplamGunSayisi) },
                { "HastalikIzinKullanimi", g.Where(x => x.IzinTuru == IzinTuru.Hastalik).Sum(x => x.ToplamGunSayisi) },
                { "OzelIzinKullanimi", g.Where(x => x.IzinTuru == IzinTuru.Ozel).Sum(x => x.ToplamGunSayisi) }
                    }
                );

            return departmanIzinKullanimi;
        }

        public async Task<Izinler> GetIzinler(int userid)
        {
            var Bakiyeler = await _repo.dbContext.Users.Where(p => p.Id == userid).Select(p => new
            {
                HastalikIzinToplamGunu = p.HastalikIzinBakiye,
                OzelIzinToplamGunu = p.OzelIzinBakiye,
                TatilIzinToplamGunu = p.TatilIzinBakiye
            })
             .FirstOrDefaultAsync();

            if (Bakiyeler == null)
            {
                throw new Exception("İlgili çalışan bulunamadı.");
            }

            Izinler izinler = new Izinler()
            {
                HastalikIzinBakiye = Bakiyeler.HastalikIzinToplamGunu,
                OzelIzinBakiye = Bakiyeler.OzelIzinToplamGunu,
                TatilIzinBakiye = Bakiyeler.TatilIzinToplamGunu
            };

            return izinler;
        }

        public async Task<Dictionary<string, Dictionary<int, int>>> IzinAnamoli()
        {
            var izinTalepler = await (from izin in _repo.dbContext.IzinTalepleri
                                      join user in _repo.dbContext.Users
                                      on izin.UserId equals user.Id
                                      select new
                                      {
                                          UserId = user.Id,
                                          UserName = user.Ad,
                                          StartDate = izin.BaslangicTarih
                                      })
                                      .AsNoTracking()
                                      .ToListAsync();


            var result = izinTalepler
                .GroupBy(it => new { it.UserId, it.UserName, Ay = it.StartDate.Month })
                .Where(g => g.Count() >= 3)
                .GroupBy(g => new { g.Key.UserId, g.Key.UserName })
                .Select(g => new
                {
                    User = $"{g.Key.UserName} id:{g.Key.UserId}",
                    MonthlyCounts = g
                        .Select(x => new
                        {
                            Month = x.Key.Ay,
                            Count = x.Count()
                        })
                        .GroupBy(x => x.Month)
                        .ToDictionary(x => x.Key, x => x.Sum(y => y.Count))
                })
                .ToDictionary(x => x.User, x => x.MonthlyCounts);

            if (result == null || result.Count == 0)
            {
                throw new Exception("İzin taleplerinde olağan dışı bir durum yok. !");
            }


            return result;
        }


        public async Task<int> IzinGunSayisi(IzinTalep izinTalep)
        {
            int GunSayisi = izinTalep.BitisTarih.Day - izinTalep.BaslangicTarih.Day;

            return GunSayisi;

        }
    }
}
