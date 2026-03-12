using HausSlytherin_SMIS.Enums;
using HausSlytherin_SMIS.Interfaces;
using HausSlytherin_SMIS.Models;

namespace HausSlytherin_SMIS.Strategies;

public class StandardRiskStrategy : IRiskStrategy
{
    public string Name => "Standard";

    public RiskReport Analyze(Incident incident, Creature creature)
    {
        var severityFactor = (int)incident.Severity;
        var score = severityFactor * 2 + creature.DangerLevel;

        var recommendation = score switch
        {
            >= 20 => "Sofortige Eindämmung und Alarmierung des Ministeriums.",
            >= 12 => "Verstärkte Überwachung und zusätzliche Schutzmaßnahmen.",
            >= 6 => "Regelmäßige Beobachtung, Standardprotokolle einhalten.",
            _ => "Geringes Risiko, Routineüberwachung ausreichend."
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
