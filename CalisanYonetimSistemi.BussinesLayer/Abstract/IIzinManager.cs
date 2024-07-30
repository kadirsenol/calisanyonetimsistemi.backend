using CalisanYonetimSistemi.EntityLayer.Concrete;
using CalisanYonetimSistemi.EntityLayer.Concrete.Support;
using CalisanYonetimSistemi.EntityLayer.Concrete.VMs.IzinVm;

namespace CalisanYonetimSistemi.BussinesLayer.Abstract
{
    public interface IIzinManager : IManager<IzinTalep, int>
    {
        public Task<bool> CheckIzinTarihi(IzinTalepInsertVm izinTalepInsertVm, int userid);
        public Task<int> IzinGunSayisi(IzinTalep izinTalep);

        public Task<Izinler> GetIzinler(int userid);

        public Task<Dictionary<string, Dictionary<string, int>>> DepartmanBazliIzinKullanimi();
        public Task<Dictionary<string, Dictionary<int, int>>> IzinAnamoli();


    }
}
