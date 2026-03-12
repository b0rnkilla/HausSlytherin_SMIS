using HausSlytherin_SMIS.Interfaces;
using HausSlytherin_SMIS.Models;

namespace HausSlytherin_SMIS.Repositories
{
    public class ResearcherRepository : IRepository<Researcher>
    {
        private readonly List<Researcher> _researchers;

        public ResearcherRepository()
        {
            _researchers = new List<Researcher>();
        }

        public void Add(Researcher entity)
        {
            _researchers.Add(entity);
        }

        public void Remove(int id)
        {
            var researcher = GetById(id);
            if (researcher != null)
            {
                _researchers.Remove(researcher);
            }
        }

        public List<Researcher> GetAll()
        {
            return _researchers;
        }

        public Researcher GetById(int id)
        {
            return _researchers.FirstOrDefault(r => r.Id == id);
        }
    }
}