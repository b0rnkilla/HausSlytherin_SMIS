using HausSlytherin_SMIS.Models;

namespace HausSlytherin_SMIS.Interfaces;

public interface IReportGenerator
{
    RiskReport Generate(Incident incident, Creature creature, IRiskStrategy strategy);
}
