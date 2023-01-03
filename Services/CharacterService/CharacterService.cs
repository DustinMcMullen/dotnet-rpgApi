using System.Security.Claims;
using AutoMapper;

namespace dotnet_rpgApi.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> characters = new List<Character> {
            new Character(),
            new Character {Id = 1, Name = "Joe"}
        };
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var dbcharacter = await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbcharacter);
            if (serviceResponse.Data == null) {
                serviceResponse.Success = false;
                serviceResponse.Message = "Character not found";
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            int userId = GetUserId();
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters = await _context.Characters
                .Where(c => c.User!.Id == userId)
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .ToListAsync();
            serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> CreateCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var character = _mapper.Map<Character>(newCharacter);
            character.User = await _context.Users.FirstOrDefaultAsync(user => user.Id == GetUserId());
            _context.Characters.Add( character );
            await _context.SaveChangesAsync();
            serviceResponse.Data = await _context.Characters.Where(c=> c.User!.Id == GetUserId()).Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            return serviceResponse;
        } 

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var dbcharacterToUpdate = await _context.Characters.Where(c => c.User!.Id == GetUserId()).FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);
            if (dbcharacterToUpdate is null) {
                serviceResponse.Success = false;
                serviceResponse.Message = "Character not found, no updates applied";
                return serviceResponse;
            }

            dbcharacterToUpdate.Name = updatedCharacter.Name;
            dbcharacterToUpdate.HitPoints = updatedCharacter.HitPoints;
            dbcharacterToUpdate.Strength = updatedCharacter.Strength;
            dbcharacterToUpdate.Defense = updatedCharacter.Defense;
            dbcharacterToUpdate.Intelligence = updatedCharacter.Intelligence;
            dbcharacterToUpdate.Class = updatedCharacter.Class;

            await _context.SaveChangesAsync();
            
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbcharacterToUpdate);
            return serviceResponse;
        } 

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacterToDelete = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());
            if (dbCharacterToDelete is null) {
                serviceResponse.Success = false;
                serviceResponse.Message = "No character found, no deletes executed";
                return serviceResponse;
            }
            _context.Characters.Remove(dbCharacterToDelete);
            await _context.SaveChangesAsync();

            serviceResponse.Data = await _context.Characters.Where(c => c.User!.Id == GetUserId()).Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
        {
            var response = new ServiceResponse<GetCharacterDto>();
            try
            {
                var character = await _context.Characters
                    .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(c => c.Id == newCharacterSkill.CharacterId && c.User!.Id == GetUserId());
                if (character is null) {
                    response.Success = false;
                    response.Message = "Character not found, no skill added";
                    return response;
                }

                var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillId);
                if (skill is null) {
                    response.Success = false;
                    response.Message = "Skill not found, no skill added";
                    return response;
                }

                character.Skills!.Add(skill);
                await _context.SaveChangesAsync();
                response.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }

            return response;
        }
    }
}