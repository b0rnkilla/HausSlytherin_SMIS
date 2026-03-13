namespace HausSlytherin_SMIS.Services
{
    public class StatisticsService
    {
        private readonly CreatureService _creatureService;
        private readonly IncidentService _incidentService;
        private readonly ResearcherService _researcherService;
        private readonly ReportService _reportService;

        public StatisticsService(
            CreatureService creatureService,
            IncidentService incidentService,
            ResearcherService researcherService,
            ReportService reportService)
        {
            _creatureService = creatureService;
            _incidentService = incidentService;
            _researcherService = researcherService;
            _reportService = reportService;
        }

        public void ShowStatistics()
        {
            Console.WriteLine("Kreaturstatistiken:");
            _creatureService.GetMostDangerousCreature();
            _creatureService.GetAverageDangerLevel();
            _creatureService.GetRestrictedCreatures();

            Console.WriteLine();
            Console.WriteLine("Vorfallstatistiken:");
            _incidentService.GetAverageSeverity();
            _incidentService.GetCreatureWithMostIncidents();

            Console.WriteLine();
            Console.WriteLine("Forscherstatistiken:");
            _researcherService.GetAverageExperienceLevel();
            _researcherService.GetResearchersByHouse();

            Console.WriteLine();
            Console.WriteLine("Risikoberichte:");
            _reportService.GetAverageRiskScore();
            _reportService.GetHighRiskReports();
        }
    }
}
