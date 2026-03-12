using HausSlytherin_SMIS.Models;

namespace HausSlytherin_SMIS.Interfaces;

public interface IRiskStrategy
{
    string Name { get; }

    RiskReport Analyze(Incident incident, Creature creature);
}
