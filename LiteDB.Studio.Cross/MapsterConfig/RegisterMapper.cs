using LiteDB.Studio.Cross.Contracts.DTO;
using LiteDB.Studio.Cross.ViewModels;
using Mapster;

namespace LiteDB.Studio.Cross.MapsterConfig {
    public class RegisterMapper : IRegister{
        public void Register(TypeAdapterConfig config) {
            config.NewConfig<ConnectionParametersDto, ConnectionParametersViewModel>()
                .RequireDestinationMemberSource(true);
        }
    }
}