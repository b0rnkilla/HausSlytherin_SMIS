using HausSlytherin_SMIS.Interfaces;
using HausSlytherin_SMIS.Models;
using AppLogger= HausSlytherin_SMIS.Logger;
using LogLevel = HausSlytherin_SMIS.Enum.Level;
namespace HausSlytherin_SMIS.Strategies;

/// <summary> Wendet die Standardformel fuer Risiken an. </summary>
public class StandardRiskStrategy : IRiskStrategy
{
    
    /// <summary> Gibt den Namen der Strategie zurueck </summary>
    public string Name => "Standard";

    /// <summary> Berechnet den Risiko mit dem Standard-Gewichtungsmodell. </summary>
    public int CalculateRisk(Creature creature, Incident incident)
    {
        if(creature is null || incident is null)
        {
           AppLogger.LogInfo(LogLevel.Error, "Die StandardRiskStrategy benoetigt eine Kreatur und einen Vorfall.", true);
           return 0;
        }

        return creature.DangerLevel * 10 + (int)incident.Severity * 5;
    }

    /// <summary> Uebersetzt einen numerischen Risikowert in ein Handlungsempfehlung </summary>
    public string GetRecommendation(int riskScore)
    {
        return riskScore switch
        {
            <= 30 => "Low Risk: Monitor",
            <= 60 => "Medium Risk: Caution",
            <= 100 => "High Risk: Restricted Access",
            _ => "Critical Risk: No Access"
        };
    }
}
