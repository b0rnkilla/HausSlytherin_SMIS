using HausSlytherin_SMIS.Interfaces;
using HausSlytherin_SMIS.Models;
using HausSlytherin_SMIS.Enums;

namespace HausSlytherin_SMIS.Repositories;

public class IncidentRepository : IRepository<Incident>
{
    private readonly List<Incident> _incidents;

    public IncidentRepository()
    {
        _incidents = new List<Incident>();
    }

    public void Add(Incident item)
    {
        _incidents.Add(item);
    }

    public void Remove(int id)
    {
        var incident = GetById(id);
        if (incident != null)
        {
            _incidents.Remove(incident);
        }
    }

    public List<Incident> GetAll()
    {
        return _incidents;
    }

    public Incident GetById(int id)
    {
        return _incidents.FirstOrDefault(i => i.Id == id);
    }

    public void Update(Incident item)
    {
        var existingIncident = GetById(item.Id);
        if (existingIncident != null)
        {
            existingIncident.Title = item.Title;
            existingIncident.Description = item.Description;
            existingIncident.Severity = item.Severity;
            existingIncident.Date = item.Date;
            existingIncident.CreatureId = item.CreatureId;
        }
    }

    public List<Incident> GetByCreatureId(int creatureId)
    {
        return [.. _incidents
            .Where(incident => incident.CreatureId == creatureId)
            .OrderByDescending(incident => incident.Date)];
    }

    public List<Incident> GetByMinimumSeverity(IncidentSeverity severity)
    {
        return [.. _incidents
            .Where(incident => incident.Severity >= severity)
            .OrderByDescending(incident => incident.Severity)
            .ThenByDescending(incident => incident.Date)];
    }
}
