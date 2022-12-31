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
        public ActionResult<Character> GetOneCharacter(int id)
        {
            return Ok(_characterService.GetCharacterById(id));
        }

        [HttpGet("GetAll")]
        public ActionResult<List<Character>> GetCharacters()
        {
            return Ok(_characterService.GetAllCharacters());
        }

        [HttpPost("Create")]
        public ActionResult<List<Character>> CreateCharacter(Character newCharacter)
        {
            return Ok(_characterService.CreateCharacter(newCharacter));
        }
    }
}