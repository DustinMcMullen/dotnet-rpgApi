namespace dotnet_rpgApi.Services.CharacterService
{
    public interface ICharacterService
    {
         Character GetCharacterById(int id);
         List<Character> GetAllCharacters();
         List<Character> CreateCharacter(Character newCharacter);
    }
}