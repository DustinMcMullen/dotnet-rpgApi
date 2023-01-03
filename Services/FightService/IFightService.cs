using dotnet_rpgApi.Dtos.Fight;

namespace dotnet_rpgApi.Services.FightService
{
    public interface IFightService
    {
         Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto attack);
         Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto attack);
    }
}