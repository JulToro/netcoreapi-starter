using netcore.api.Models;
using netcore.infrastructure.Entities;

namespace netcore.api.Factories
{
    public static class UserFactory
    {
        public static User ToUser<T>(this T @this) where T : class, new()
        {
            var @object = @this as UserDto;
            return new User()
            {
                Id = @object.Id,
                BirthDay = @object.BirthDay,
                Email = @object.Email,
                LastName = @object.LastName,
                Name = @object.Name,
                Nit = @object.Nit
            };
        }
        public static UserDto ToUserDto<T>(this T @this) where T : class, new()
        {
            var @object = @this as User;
            return new UserDto()
            {
                Id = @object.Id,
                BirthDay = @object.BirthDay,
                Email = @object.Email,
                LastName = @object.LastName,
                Name = @object.Name,
                Nit = @object.Nit
            };
        }
    }
}
