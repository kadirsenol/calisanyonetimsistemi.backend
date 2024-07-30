using AutoMapper;
using CalisanYonetimSistemi.EntityLayer.Concrete;
using CalisanYonetimSistemi.EntityLayer.Concrete.VMs.IzinVm;
using CalisanYonetimSistemi.EntityLayer.Concrete.VMs.UserVm;
using CalisanYonetimSistemi.EntityLayer.Concrete.VMs.YoneticiVms.PerformansVm;
using CalisanYonetimSistemi.EntityLayer.Concrete.VMs.YoneticiVms.UserVm;

namespace CalisanYonetimSistemi.WebApiLayer.MyExtensions.AutoMapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<UserLoginVm, User>().ReverseMap();
            CreateMap<UserRegisterVm, User>().ReverseMap();
            CreateMap<PerformansDegerlendirmeInsertVm, PerformansDegerlendirme>().ReverseMap();
            CreateMap<PerformansDegerlendirmeUpdateVm, PerformansDegerlendirme>().ReverseMap();
            CreateMap<IzinTalepInsertVm, IzinTalep>()
                .ForMember(dest => dest.BaslangicTarih, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.BaslangicTarih) ? (DateOnly?)null : DateOnly.Parse(src.BaslangicTarih)))
                .ForMember(dest => dest.BitisTarih, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.BitisTarih) ? (DateOnly?)null : DateOnly.Parse(src.BitisTarih)));

            CreateMap<UserInsertVm, User>()
            .ForMember(dest => dest.BaslamaTarihi, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.BaslamaTarihi) ? (DateOnly?)null : DateOnly.Parse(src.BaslamaTarihi)));


            CreateMap<UserUpdateVm, User>()
            .ForMember(dest => dest.BaslamaTarihi, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.BaslamaTarihi) ? (DateOnly?)null : DateOnly.Parse(src.BaslamaTarihi)));
            //.ForMember(dest => dest.HastalikIzinBakiye, opt => opt.MapFrom(src =>
            //    string.IsNullOrEmpty(src.HastalikIzinBakiye) ? (int?)null : int.Parse(src.HastalikIzinBakiye)))
            //.ForMember(dest => dest.TatilIzinBakiye, opt => opt.MapFrom(src =>
            //    string.IsNullOrEmpty(src.TatilIzinBakiye) ? (int?)null : int.Parse(src.TatilIzinBakiye)))
            //.ForMember(dest => dest.OzelIzinBakiye, opt => opt.MapFrom(src =>
            //    string.IsNullOrEmpty(src.OzelIzinBakiye) ? (int?)null : int.Parse(src.OzelIzinBakiye)));



            //.ForMember(dest => dest.BaslamaTarihi, opt => opt.MapFrom(src => DateOnly.FromDateTime(DateTime.ParseExact(src.BaslamaTarihi, "dd.MM.yyyy", CultureInfo.InvariantCulture))));
        }
    }
}
