using AutoMapper;

namespace LocalNuget.Models
{
    public class MapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<StoragePackage, PackageInfoModel>()
                .ForMember(model => model.VisualStudioProject, opt => opt.MapFrom(package => package.CsProjectFile))
                .ForMember(model => model.Id, opt => opt.MapFrom(package => package.Name))
                .ForMember(model => model.NuspecInProject, opt => opt.MapFrom(package => package.NuspecProjectFile));
        }
    }

}
