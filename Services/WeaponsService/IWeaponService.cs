using dotnet_rpgApi.Dtos.Weapon;

namespace dotnet_rpgApi.Services.WeaponService
{
    public interface IWeaponService
    {
         Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon);
    }
}