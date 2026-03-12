using HausSlytherin_SMIS.Enums;
using HausSlytherin_SMIS.Models;
using HausSlytherin_SMIS.Repositories;

namespace HausSlytherin_SMIS.Factories
{
    public class IncidentFactory
    {
        private int _idCounter = 1;
        private readonly IncidentRepository _repo;

        public IncidentFactory(IncidentRepository repo) => _repo = repo;

        public bool Create(string title, string desc, IncidentSeverity severity, DateTime date, int creatureId)
        {
            var obj = new Incident
            {
                Id = _idCounter++,
                Title = title,
                Description = desc,
                Severity = severity,
                Date = date,
                CreatureId = creatureId
            };
            _repo.Add(obj);
            return true;
        }
    }
}