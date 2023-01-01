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

        public CharacterService(IMapper mapper)
        {
            this._mapper = mapper;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var character = characters.FirstOrDefault(character => character.Id == id);
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            if (serviceResponse.Data == null) {
                serviceResponse.Success = false;
                serviceResponse.Message = "Character not found";
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> CreateCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var character = _mapper.Map<Character>(newCharacter);
            character.Id = characters.Max(c => c.Id) + 1;
            characters.Add( character );
            serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        } 

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var character = _mapper.Map<Character>(updatedCharacter);
            var characterToUpdateId = updatedCharacter.Id;
            var characterToUpdateIndex = characters.FindIndex(c => c.Id == characterToUpdateId);
            if (characterToUpdateIndex == -1) {
                serviceResponse.Success = false;
                serviceResponse.Message = "Character not found, no updates applied";
                return serviceResponse;
            }
            characters[characterToUpdateIndex] = character;
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(characters[characterToUpdateIndex]);
            return serviceResponse;
        } 

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var characterToDeleteIndex = characters.FindIndex(c => c.Id == id);
            if (characterToDeleteIndex == -1) {
                serviceResponse.Success = false;
                serviceResponse.Message = "No character found, no deletes executed";
                return serviceResponse;
            }
            var characterToDelete = characters[characterToDeleteIndex];
            characters.Remove(characterToDelete);
            serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }     
    }
}