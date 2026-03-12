using HausSlytherin_SMIS.Enums;
using HausSlytherin_SMIS.Interfaces;
using HausSlytherin_SMIS.Models;

namespace HausSlytherin_SMIS.Strategies;

public class StrictRiskStrategy : IRiskStrategy
{
    public string Name => "Strict";

    public RiskReport Analyze(Incident incident, Creature creature)
    {
        var severityFactor = (int)incident.Severity;
        var score = severityFactor * 3 + creature.DangerLevel * 2;

        var recommendation = score switch
        {
            >= 25 => "Vollständige Evakuierung, sofortige Versiegelung des Bereichs, Auroren alarmieren.",
            >= 16 => "Strenge Quarantäne der Kreatur, Zugang nur für erfahrene Forscher.",
            >= 8 => "Erhöhte Sicherheitsmaßnahmen, Zugangsbeschränkungen prüfen.",
            _ => "Trotz geringem Risiko Vorsicht walten lassen."
        };

        return new RiskReport
        {
            CreatureName = creature.Name,
            IncidentTitle = incident.Title,
            StrategyName = Name,
            RiskScore = score,
            Recommendation = recommendation
        };
    }
}
