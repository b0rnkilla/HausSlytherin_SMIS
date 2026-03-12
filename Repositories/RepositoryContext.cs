using HausSlytherin_SMIS.Models;

namespace HausSlytherin_SMIS.Repositories;

public static class RepositoryContext
{
    public static readonly CreatureRepository Creatures = new();
    public static readonly ResearcherRepository Researchers = new();
    public static readonly IncidentRepository Incidents = new();
}

