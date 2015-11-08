using AutoMapper;

namespace LocalNuget.Settings
{
    public class MapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<NugetSettings.SettingsDefaultsModel, DefaultSettingsDefaults>();
        }
    }
}
