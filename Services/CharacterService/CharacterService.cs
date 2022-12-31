namespace dotnet_rpgApi.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> characters = new List<Character> {
            new Character(),
            new Character {Id = 1, Name = "Joe"}
        };

        public Character GetCharacterById(int id)
        {
            var character = characters.FirstOrDefault(character => character.Id == id);
            if (character is not null) {
                return character;
            }

            throw new Exception("Character with specified id not found");
        }

        public List<Character> GetAllCharacters()
        {
            return characters;
        }

        public List<Character> CreateCharacter(Character newCharacter)
        {
            characters.Add(newCharacter);
            return characters;
        }        
    }
}