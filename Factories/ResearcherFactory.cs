using HausSlytherin_SMIS.Enums;
using HausSlytherin_SMIS.Models;
using HausSlytherin_SMIS.Repositories;

namespace HausSlytherin_SMIS.Factories
{
    public class ResearcherFactory
    {
        private int _idCounter = 1;
        private readonly ResearcherRepository _repo;

        public ResearcherFactory(ResearcherRepository repo) => _repo = repo;

        public bool Create(string name, House house, string spec, int level)
        {
            var obj = new Researcher 
            {
                Id = _idCounter++,
                Name = name,
                House = house,
                Specialization = spec,
                ExperienceLevel = level

            };
            _repo.Add(obj);
            return true;
        }
    }
}