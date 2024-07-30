using CalisanYonetimSistemi.EntityLayer.Concrete;
using CalisanYonetimSistemi.EntityLayer.Concrete.Support;

namespace CalisanYonetimSistemi.BussinesLayer.Abstract
{
    public interface IPerformansDegerlendirmeManager : IManager<PerformansDegerlendirme, int>
    {
        public Task<Dictionary<string, PerformansTipiOrtPuan>> DepartmanOrtPerformansPuan();

        public Task<Dictionary<string, AylikPerformansTrendi>> DepartmanOrtPerformansAylikTrendi(int year);
        public Task<Dictionary<string, AylikPerformansTrendi>> BireyselOrtPerformansAylikTrendi(int year);
        public Task<Dictionary<string, Dictionary<string, string>>> BireyselPerformansDegisimAnomali(int year);
        public Task<Dictionary<string, Dictionary<string, string>>> DepartmanPerformansDegisimAnomali(int year);



    }
}
