using HausSlytherin_SMIS.Enums;
using HausSlytherin_SMIS.Models;

namespace HausSlytherin_SMIS.Factories
{
    public class CreatureFactory
    {
        private int _idCounter = 1;

        public void UpdateIdCounter(int lastId)
        {
            _idCounter = lastId + 1;
        }
        
        public Creature Create(string name, string species, int danger, string habitat, bool restricted, CreatureType type)
        {
            return new Creature
            {
                Id = _idCounter++,
                Name = name,
                Species = species,
                DangerLevel = danger,
                Habitat = habitat,
                IsRestricted = restricted,
                CreatureType = type
            };
        }
    }
}
    


