using HausSlytherin_SMIS.Enums;
using HausSlytherin_SMIS.Models;

namespace HausSlytherin_SMIS.Factories
{
    public class ResearcherFactory
    {
        private int _idCounter = 1;

        public void UpdateIdCounter(int lastId)
        {
            _idCounter = lastId + 1;
        }

        public Researcher Create(string name, House house, string spec, int level)
        {
            return new Researcher 
            {
                Id = _idCounter++,
                Name = name,
                House = house,
                Specialization = spec,
                ExperienceLevel = level
            }; 
        }
    }
}
