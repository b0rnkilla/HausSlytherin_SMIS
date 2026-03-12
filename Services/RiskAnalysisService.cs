using HausSlytherin_SMIS.Interfaces;
using HausSlytherin_SMIS.Models;
using HausSlytherin_SMIS.Repositories;
using HausSlytherin_SMIS.Services;
using AppLogger= HausSlytherin_SMIS.Logger;
using LogLevel = HausSlytherin_SMIS.Enum.Level;

namespace HausSlytherin_SMIS.Factories;

/// <summary> Koordiniert die Risikoanalyse ueber Kreaturen, Vorfaelle und Strategien hinweg </summary>
public class RiskAnalysisService
{

    private readonly ICreatureRepository _creatureRepository;
    private readonly IncidentRepository _incidentRepository;
    private readonly ReportService _reportService;

    /// <summary>
    /// Erstellt den Service mit dem benoetigten Report-Service.
    /// </summary>
    /// <param name="creatureRepository"></param>
    /// <param name="incidentRepository"></param>
    /// <param name="reportService"></param>
    public RiskAnalysisService(
        ICreatureRepository creatureRepository,
        IncidentRepository incidentRepository,
        ReportService reportService
    )
    {
        _creatureRepository = creatureRepository;
        _incidentRepository = incidentRepository;
        _reportService = reportService;
    }

    /// <summary>
    /// Erstellt mit der ausgewaehlten Strategie einen Risikobericht fuer eine Kreatur und einen ihrer Vorfaelle.
    /// </summary>
    /// <param name="creatureId">Die ID der Kreatur.</param>
    /// <param name="incidentId">Die ID des Vorfalls.</param>
    /// <param name="strategy">Die Risikostrategie fuer die Berechnung.</param>
    public RiskReport? CreateRiskReport(int creatureId, int incidentId, IRiskStrategy? strategy)
    {
        if(strategy is null)
        {
            AppLogger.LogInfo(LogLevel.Error, "Eine Risikostrategie ist erforderlich", true);
            return null;
        }

        var creature = _creatureRepository.GetById(creatureId);
        if (creature is null){
            AppLogger.LogInfo(LogLevel.Error, $"Die Kreatur mit der ID {creatureId} wurde nicht gefunden.", console: true);
            return null;
        }

        var incident = _incidentRepository.GetById(incidentId);
        if(incident is null){
            AppLogger.LogInfo(LogLevel.Error, $"Der Vorfall mit der ID {incidentId} wurde nicht gefunden.", console: true);
            return null;
        }

        if(incident.CreatureId != creatureId)
        {
            AppLogger.LogInfo(LogLevel.Error, "Der ausgewaehlte Vorfall gehoert nicht zur ausgewaehlten Kreatur.", console: true);
            return null;
        }

        var report = _reportService.Generate(creature, incident, strategy);
        if (report is null)
        {
            AppLogger.LogInfo(LogLevel.Error, "Der Risikobericht konnte nicht erstellt werden.", console: true);
            return null;
        }

        AppLogger.LogInfo(
            LogLevel.Info,
            $"Risikobericht erstellt: Kreatur {report.CreatureName}, Vorfall {report.IncidentTitle}, Score {report.RiskScore}, Strategie {report.StrategyName}");

        return report;
    }
     
}