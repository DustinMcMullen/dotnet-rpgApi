using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpgApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [HttpGet("GetOne/{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetOneCharacter(int id)
        {
            var response = await _characterService.GetCharacterById(id);
            if (response.Success == false) {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> GetCharacters()
        {
            return Ok(await _characterService.GetAllCharacters());
        }

        [HttpPost("Create")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> CreateCharacter(AddCharacterDto newCharacter)
        {
            return Ok(await _characterService.CreateCharacter(newCharacter));
        }

        [HttpPut("Update")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var response = await _characterService.UpdateCharacter(updatedCharacter);
            if (response.Success == false) {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> DeleteCharacter(int id)
        {
            var response = await _characterService.DeleteCharacter(id);
            if (response.Success == false) {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}