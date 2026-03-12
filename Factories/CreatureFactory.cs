using HausSlytherin_SMIS.Enums;
using HausSlytherin_SMIS.Models;
using HausSlytherin_SMIS.Repositories;

namespace HausSyltherin_SMIS.Factories
{
    public class CreatureFactory
    {
        private int _idCounter = 1;
        private readonly CreatureRepository _repo;

        public CreatureFactory(CreatureRepository repo) => _repo = repo;

        public bool Create(string name, string species, int danger, string habitat, bool restricted, CreatureType type)
        {
            var obj = new Creature
            {
                Id = _idCounter++,
                Name = name,
                Species = species,
                DangerLevel = danger,
                Habitat = habitat,
                IsRestricted = restricted,
                CreatureType = type
            };
            _repo.Add(obj);
            return true;
        }
    }
}
    


