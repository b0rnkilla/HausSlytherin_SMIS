using HausSlytherin_SMIS.Exceptions;
using HausSlytherin_SMIS.Models;   
using HausSlytherin_SMIS.Interfaces;
using HausSlytherin_SMIS.Repositories;
using HausSlytherin_SMIS.Enums;
using AppLogger = HausSlytherin_SMIS.Logger;
using LogLevel = HausSlytherin_SMIS.Enum.Level;
namespace HausSlytherin_SMIS.Services;

/// <summary> Enthaelt die Geschaeftslogik fuer die Verwaltung von Vorfaellen. </summary>
public class IncidentService
{   
    private readonly IncidentRepository _incidentRepository;
    private readonly ICreatureRepository _creatureRepository;

    /// <summary>
    /// Erstellt den Service mit Vorfall- und Kreaturen-Repository.
    /// </summary>
    public IncidentService(IncidentRepository incidentRepository, ICreatureRepository creatureRepository)
    {
        _incidentRepository = incidentRepository;
        _creatureRepository = creatureRepository;
    }

    /// <summary>
    /// Prueft und speichert einen neuen Vorfall fuer eine vorhandene Kreatur.
    /// </summary>
    /// <param name="incident">Der hinzuzufuegende Vorfall.</param>
    public bool AddIncident(Incident? incident)
    {
        
        if (incident is null)
        {
            AppLogger.LogInfo(LogLevel.Error, "Der Vorfall ist erforderlich.", console: true);
            return false;
        }

        if (string.IsNullOrWhiteSpace(incident.Title))
        {
            AppLogger.LogInfo(LogLevel.Error, "Der Titel des Vorfalls ist erforderlich.", console: true);
            return false;
        }

        if (string.IsNullOrWhiteSpace(incident.Description))
        {
            AppLogger.LogInfo(LogLevel.Error, "Die Beschreibung des Vorfalls ist erforderlich.", console: true);
            return false;
        }

        if (!System.Enum.IsDefined(typeof(IncidentSeverity), incident.Severity))
        {
            AppLogger.LogInfo(LogLevel.Error, "Der Schweregrad des Vorfalls ist ungueltig.", console: true);
            return false;
        }

        if (_creatureRepository.GetById(incident.CreatureId) is null)
        {
            AppLogger.LogInfo(
                LogLevel.Error,
                $"Die Kreatur mit der ID {incident.CreatureId} existiert nicht.",
                console: true);
            return false;
        }

        if (incident.Date == default)
        {
            incident.Date = DateTime.Now;
        }

        _incidentRepository.Add(incident);
        AppLogger.LogInfo(
            LogLevel.Info,
            $"Vorfall erstellt: {incident.Title} (ID {incident.Id}, Kreatur {incident.CreatureId})");
        return true;
    }
    public List<Incident> GetAllIncidents()
    {
        return _incidentRepository.GetAll();
    }

    /// <summary>
    /// Gibt alle Vorfaelle einer bestimmten Kreatur zurueck.
    /// </summary>
    /// <param name="creatureId">Die ID der Kreatur.</param>
    public List<Incident> GetIncidentsForCreature(int creatureId)
    {
        return _incidentRepository.GetByCreatureId(creatureId);
    }

    /// <summary>
    /// Gibt Vorfaelle mit mindestens hohem Schweregrad zurueck.
    /// </summary>
    public List<Incident> GetCriticalIncidents()
    {
        return _incidentRepository.GetByMinimumSeverity(IncidentSeverity.High);
    }

    public double GetAverageSeverity()
    {
        var incidents = _incidentRepository.GetAll();
        return incidents.Count == 0 ? 0 : incidents.Average(incident => (int)incident.Severity);
    }

    /// <summary>
    /// Zaehlt, wie viele Vorfaelle fuer jede Kreatur existieren.
    /// </summary>
    public List<(Creature Creature, int Count)> GetIncidentCountByCreature()
    {
        return [.. _incidentRepository.GetAll()
            .GroupBy(incident => incident.CreatureId)
            .Join(
                _creatureRepository.GetAll(),
                groupedIncidents => groupedIncidents.Key,
                creature => creature.Id,
                (groupedIncidents, creature) => (Creature: creature, Count: groupedIncidents.Count()))
            .OrderByDescending(result => result.Count)
            .ThenByDescending(result => result.Creature.DangerLevel)
            .ThenBy(result => result.Creature.Name)];
    }

    /// <summary>
    /// Gibt die Kreatur mit der hoechsten Anzahl erfasster Vorfaelle zurueck.
    /// </summary>
    /// <returns>Die Kreatur mit ihrer Vorfallanzahl oder <see langword="null"/>, wenn keine Vorfaelle vorhanden sind.</returns>
    public (Creature Creature, int Count)? GetCreatureWithMostIncidents()
    {
        var incidentCounts = GetIncidentCountByCreature();
        return incidentCounts.Count == 0 ? null : incidentCounts[0];
    }
}
