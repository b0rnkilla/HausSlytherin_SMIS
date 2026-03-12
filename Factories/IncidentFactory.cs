using HausSlytherin_SMIS.Enums;
using HausSlytherin_SMIS.Models;
namespace HausSlytherin_SMIS.Factories
{
    public class IncidentFactory
    {
        private int _idCounter = 1;
        public Incident Create(string title, string desc, IncidentSeverity severity, DateTime date, int creatureId)
        {
            return new Incident
            {
                Id = _idCounter++,
                Title = title,
                Description = desc,
                Severity= severity,
                Date = date,
                CreatureId = creatureId
            };
        }
    }
}