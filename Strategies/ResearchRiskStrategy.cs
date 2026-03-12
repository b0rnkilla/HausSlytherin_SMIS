using HausSlytherin_SMIS.Interfaces;
using HausSlytherin_SMIS.Models;
using AppLogger = HausSlytherin_SMIS.Logger;
using LogLevel = HausSlytherin_SMIS.Enums.Level;

namespace HausSlytherin_SMIS.Strategies
{
    /// <summary> Wendet fuer Forschungsszenarien eine mildere Risikoformel an. </summary>
    public class ResearchRiskStrategy : IRiskStrategy
    {
        /// <summary> Gibt den Namen der Strategie zurueck. </summary>
        public string Name => "Forschung";

        public int CalculateRisk(Creature creature, Incident incident)
        {
            if (creature is null || incident is null)
            {
                AppLogger.LogInfo(LogLevel.Error, "Die Forschungsstrategie benoetigt eine Kreatur und einen Vorfall.", true);
                return 0;
            }

            return creature.DangerLevel * 8 + (int)incident.Severity * 4;
        }

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
}