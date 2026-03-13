using System.Text.Json;
using System.IO;
using HausSlytherin_SMIS.Interfaces;
using HausSlytherin_SMIS.Models;
using HausSlytherin_SMIS.Enums;

namespace HausSlytherin_SMIS.Repositories
{
    public class IncidentRepository : IRepository<Incident>
    {
        private readonly List<Incident> _incidents;
        private readonly string _filePath = "incidents.json";

        public IncidentRepository()
        {
            // Laden beim Start
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                _incidents = JsonSerializer.Deserialize<List<Incident>>(json) ?? new();
            }
            else
            {
                _incidents = new List<Incident>();
            }
        }

        private void Save()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(_filePath, JsonSerializer.Serialize(_incidents, options));
        }

        public void Add(Incident item)
        {
            _incidents.Add(item);
            Save();
        }

        public void Remove(int id)
        {
            var incident = GetById(id);
            if (incident != null)
            {
                _incidents.Remove(incident);
                Save();
            }
        }

        public List<Incident> GetAll() => _incidents;

        public Incident? GetById(int id) => _incidents.FirstOrDefault(i => i.Id == id);

        public void Update(Incident item)
        {
            var existing = GetById(item.Id);
            if (existing != null)
            {
                existing.Title = item.Title;
                existing.Description = item.Description;
                existing.Severity = item.Severity;
                existing.Date = item.Date;
                existing.CreatureId = item.CreatureId;
                Save();
            }
        }        
        
        public List<Incident> GetByCreatureId(int creatureId)
        {
            return _incidents.Where(i => i.CreatureId == creatureId)
                             .OrderByDescending(i => i.Date).ToList();
        }

        public List<Incident> GetByMinimumSeverity(IncidentSeverity severity)
        {
            return _incidents.Where(i => i.Severity >= severity)
                             .OrderByDescending(i => i.Severity)
                             .ThenByDescending(i => i.Date).ToList();
        }
    }
}

 