using AutoMapper;

namespace LocalNuget.Models
{
    public static class AutoMapperModels
    {

        public static void CreateMap()
        {
            Mapper.CreateMap<StoragePackage, PackageInfoModel>()
                .ForMember(model => model.VisualStudioProject, opt => opt.MapFrom(package => package.CsProjectFile))
                .ForMember(model => model.Id, opt => opt.MapFrom(package => package.Name));

        }

    }
}
