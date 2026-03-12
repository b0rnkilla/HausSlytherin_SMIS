using HausSlytherin_SMIS.Enums;
using HausSlytherin_SMIS.Interfaces;
using HausSlytherin_SMIS.Models;
using HausSlytherin_SMIS.Repositories;

namespace HausSlytherin_SMIS.Strategies;

public class ResearchRiskStrategy : IRiskStrategy
{
    public string Name => "Research";

    public RiskReport Analyze(Incident incident, Creature creature)
    {
        // Diese Strategie bewertet Risiken etwas niedriger,
        // da von einer kontrollierten Forschungssituation ausgegangen wird.
        var baseScore = (int)incident.Severity * 2 + creature.DangerLevel;
        var adjustedScore = Math.Max(1, baseScore - 2);

        var recommendation = adjustedScore switch
        {
            >= 18 => "Forschung sofort pausieren, Sicherheitskonzept überarbeiten.",
            >= 10 => "Forschungsprotokolle überprüfen, zusätzliche Schutzmaßnahmen einführen.",
            >= 5 => "Fortsetzung unter erhöhter Beobachtung, Protokolle dokumentieren.",
            _ => "Forschung kann regulär fortgesetzt werden, Ergebnisse dokumentieren."
        };

        return new RiskReport
        {
            CreatureName = creature.Name,
            IncidentTitle = incident.Title,
            StrategyName = Name,
            RiskScore = adjustedScore,
            Recommendation = recommendation
        };
    }
}
