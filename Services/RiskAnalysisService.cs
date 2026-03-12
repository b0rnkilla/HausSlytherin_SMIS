using HausSlytherin_SMIS.Interfaces;
using HausSlytherin_SMIS.Models;

namespace HausSlytherin_SMIS.Services;

public static class RiskAnalysisService
{
    public static RiskReport Analyze(Incident incident, Creature creature, IRiskStrategy strategy)
    {
        return strategy.Analyze(incident, creature);
    }
}
