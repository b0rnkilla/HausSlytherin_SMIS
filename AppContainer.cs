using HausSlytherin_SMIS.Factories;
using HausSlytherin_SMIS.Interfaces;
using HausSlytherin_SMIS.Repositories;
using HausSlytherin_SMIS.Services;

namespace HausSlytherin_SMIS
{
    public class AppContainer
    {
        public ICreatureRepository CreatureRepository { get; }
        public IncidentRepository IncidentRepository { get; }
        public ResearcherRepository ResearcherRepository { get; }

        public CreatureFactory CreatureFactory { get; }
        public IncidentFactory IncidentFactory { get; }
        public ResearcherFactory ResearcherFactory { get; }
        
        public CreatureService CreatureService { get; }
        public IncidentService IncidentService { get; }
        public ResearcherService ResearcherService { get; }
        public RiskAnalysisService RiskAnalysisService { get; }

        public AppContainer()
        {
            CreatureRepository = new CreatureRepository();
            IncidentRepository = new IncidentRepository();
            ResearcherRepository = new ResearcherRepository();
            CreatureFactory = new CreatureFactory();
            IncidentFactory = new IncidentFactory();
            ResearcherFactory = new ResearcherFactory();
            CreatureService = new CreatureService(CreatureRepository, CreatureFactory);
            //IncidentService = new IncidentService(IncidentRepository, IncidentFactory);
            //ResearcherService = new ResearcherService(ResearcherRepository, ResearcherFactory);
            //RiskAnalysisService = new RiskAnalysisService(CreatureRepository, IncidentRepository);

            //TODO: o.g. Services müssen grundlegend überarbeitet werden, damit sie mit den Factories und Repositories zusammenarbeiten. Daher erstmal auskommentiert, damit die AppContainer-Klasse kompilierbar bleibt.
        }
    }
}
