using System.Text.Json;
using System.IO;
using HausSlytherin_SMIS.Interfaces;
using HausSlytherin_SMIS.Models;

namespace HausSlytherin_SMIS.Repositories
{
    public class ResearcherRepository : IRepository<Researcher>
    {
        private readonly List<Researcher> _researchers;
        private readonly string _filePath = "researchers.json";

        public ResearcherRepository()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                _researchers = JsonSerializer.Deserialize<List<Researcher>>(json) ?? new();
            }
            else
            {
                _researchers = new List<Researcher>();
            }
        }

        private void Save()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(_filePath, JsonSerializer.Serialize(_researchers, options));
        }

        public void Add(Researcher entity)
        {
            _researchers.Add(entity);
            Save();
        }

        public void Remove(int id)
        {
            var researcher = GetById(id);
            if (researcher != null)
            {
                _researchers.Remove(researcher);
                Save();
            }
        }

        public List<Researcher> GetAll() => _researchers;

        public Researcher? GetById(int id) => _researchers.FirstOrDefault(r => r.Id == id);
    }
}
