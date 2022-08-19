using WebH.Models;

namespace WebH
{
    public static class MappingExtensions
    {
        public static LoginServiceModel ToServiceModel(this LoginBindingModel source)
        {
            return new LoginServiceModel()
            {
                Email = source.Login,
                Password = source.Password
            };
        }
    }

}
