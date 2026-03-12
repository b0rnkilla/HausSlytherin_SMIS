using System.Linq;
using HausSlytherin_SMIS.Models;
using HausSlytherin_SMIS.Repositories;

namespace HausSlytherin_SMIS.Services;

public static class StatisticsService
{
    public static void ShowStatistics()
    {
        var creatureRepo = RepositoryContext.Creatures;
        var researcherRepo = RepositoryContext.Researchers;
        var incidentRepo = RepositoryContext.Incidents;
        var reports = ReportService.GetAllReports();

        var creatures = creatureRepo.GetAll();
        var researchers = researcherRepo.GetAll();
        var incidents = incidentRepo.GetAll();

        Console.WriteLine("=== Statistiken ===");
        Console.WriteLine($"Anzahl Kreaturen:   {creatures.Count}");
        Console.WriteLine($"Anzahl Forscher:    {researchers.Count}");
        Console.WriteLine($"Anzahl Vorfälle:    {incidents.Count}");
        Console.WriteLine($"Anzahl Reports:     {reports.Count}");
        Console.WriteLine();

        if (creatures.Count > 0)
        {
            var avgDanger = creatures.Average(c => c.DangerLevel);
            Console.WriteLine($"Ø Gefahrenlevel aller Kreaturen: {avgDanger:F2}");
        }

        if (incidents.Count > 0)
        {
            Console.WriteLine();
            Console.WriteLine("Verteilung der Vorfälle nach Schweregrad:");
            var bySeverity = incidents
                .GroupBy(i => i.Severity)
                .OrderBy(g => g.Key)
                .ToList();

            foreach (var group in bySeverity)
            {
                var percentage = group.Count() * 100.0 / incidents.Count;
                Console.WriteLine($"- {group.Key}: {group.Count()} ({percentage:F1}%)");
            }
        }

        if (reports.Count > 0)
        {
            Console.WriteLine();
            Console.WriteLine("Risikoreports:");
            var avgScore = reports.Average(r => r.RiskScore);
            var maxScore = reports.Max(r => r.RiskScore);
            Console.WriteLine($"Ø Risk Score: {avgScore:F2}");
            Console.WriteLine($"Maximaler Risk Score: {maxScore}");
        }
    }
}

