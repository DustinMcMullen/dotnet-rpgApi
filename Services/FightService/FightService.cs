using AutoMapper;
using dotnet_rpgApi.Dtos.Fight;

namespace dotnet_rpgApi.Services.FightService
{
    public class FightService : IFightService
    {
        private readonly DataContext _context;

        public FightService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request)
        {
            var response = new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker = await _context.Characters.Include(c => c.Skills).FirstOrDefaultAsync(c => c.Id == request.AttackerId);
                var opponent = await _context.Characters.FirstOrDefaultAsync(c => c.Id == request.OpponentId);
                if (attacker is null || opponent is null || attacker.Skills is null) {
                    response.Message = "Attack failed, something weired happened.";
                    return response;
                }

                var skill = attacker.Skills.FirstOrDefault(s => s.Id == request.SkillId);
                if (skill is null) {
                    response.Message = $"{attacker.Name} does not have that skill.";
                    return response;
                }

                int damage = skill.Damage + (new Random().Next(attacker.Intelligence));
                damage -= new Random().Next(opponent.Defense);
                opponent.HitPoints = opponent.HitPoints - damage;
                if (opponent.HitPoints <= 0) {
                    response.Message = $"{opponent.Name} has been defeated!";
                }

                await _context.SaveChangesAsync();

                response.Data = new AttackResultDto
                {
                    Attacker = attacker.Name,
                    Opponent = opponent.Name,
                    AttackerHP = attacker.HitPoints,
                    OpponentHP = opponent.HitPoints,
                    Damage = damage
                };
                
            }
            catch (Exception ex)
            {
                
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request)
        {
            var response = new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker = await _context.Characters.Include(c => c.Weapon).FirstOrDefaultAsync(c => c.Id == request.AttackerId);
                var opponent = await _context.Characters.FirstOrDefaultAsync(c => c.Id == request.OpponentId);

                if (attacker is null || opponent is null || attacker.Weapon is null) {
                    response.Message = "Attack failed, something weired happened.";
                    return response;
                }
                int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));
                damage -= new Random().Next(opponent.Defense);
                opponent.HitPoints = opponent.HitPoints - damage;

                if (opponent.HitPoints <= 0) {
                    response.Message = $"{opponent.Name} has been defeated!";
                }

                await _context.SaveChangesAsync();

                response.Data = new AttackResultDto
                {
                    Attacker = attacker.Name,
                    Opponent = opponent.Name,
                    AttackerHP = attacker.HitPoints,
                    OpponentHP = opponent.HitPoints,
                    Damage = damage
                };
                
            }
            catch (Exception ex)
            {
                
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}