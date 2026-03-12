using HausSlytherin_SMIS.Interfaces;
using HausSlytherin_SMIS.Models;
using AppLogger= HausSlytherin_SMIS.Logger;
using LogLevel = HausSlytherin_SMIS.Enum.Level;

namespace HausSlytherin_SMIS.Strategies;

/// <summary> Wendet fuer gefaerhrliche Situationen eine Strengere Risikoformel an. </summary>
public class StrictRiskStrategy : IRiskStrategy
{
    
    /// <summary> Gibt den Namen der Strategie zurueck </summary>
    public string Name => "Streng";

    /// <summary> Berechent das Risiko mit strengerer Gewichtung und einem Zusatz fuer eingeschraenkte Kreaturen </summary>
    public int CalculateRisk(Creature creature, Incident incident)
    {
        if (creature is null || incident is null)
        {
            AppLogger.LogInfo(LogLevel.Error, "Die StrictRiskStrategy benoetigt eine Kreatur und einen Vorfall.", true);
            return 0;
        }

        var score = creature.DangerLevel * 15 + (int)incident.Severity * 15;

        if (creature.IsRestricted)
        {
            score += 20;
        }

        return score;
    }

    /// <summary> Uebersetzt einen numerischen Risikowert in eine Handlungsempfehlung.</summary>
    public string GetRecommendation(int riskScore)
    {
        return riskScore switch
        {
            <= 30 => "Beobachten",
            <= 60 => "Vorsicht",
            <= 100 => "Eingeschraenkter Zugang",
            _ => "Sofortiges Eingreifen"
        };
    }
}
