using HausSlytherin_SMIS.Interfaces;
using HausSlytherin_SMIS.Models;
using AppLogger = HausSlytherin_SMIS.Logger;
using LogLevel = HausSlytherin_SMIS.Enums.Level;

namespace HausSlytherin_SMIS.Services
{

    public class ReportService : IReportGenerator
    {
        private readonly List<RiskReport> _reports = [];

        /// <summary>
        /// Erstellt einen leeren Report-Service.
        /// </summary>
        public ReportService()
        {
        }

        /// <summary>
        /// Erstellt einen Report-Service mit bestehenden Berichten.
        /// </summary>
        /// <param name="seedReports">Die Anfangsberichte.</param>
        public ReportService(IEnumerable<RiskReport> seedReports)
        {
            _reports.AddRange(seedReports);
        }

        /// <summary>
        /// Erzeugt einen Risikobericht aus Kreatur, Vorfall und ausgewaehlter Strategie.
        /// </summary>
        /// <param name="creature">Die bewertete Kreatur.</param>
        /// <param name="incident">Der bewertete Vorfall.</param>
        /// <param name="strategy">Die Strategie fuer die Risikoberechnung.</param>
        public RiskReport? Generate(Creature? creature, Incident? incident, IRiskStrategy? strategy)
        {
            if (creature is null || incident is null || strategy is null)
            {
                AppLogger.LogInfo(LogLevel.Error, "Fuer die Erstellung eines Risikoberichts werden Kreatur, Vorfall und Strategie benoetigt.", console: true);
                return null;
            }

            var riskScore = strategy.CalculateRisk(creature, incident);
            var report = new RiskReport
            {
                CreatureName = creature.Name,
                IncidentTitle = incident.Title,
                StrategyName = strategy.Name,
                RiskScore = riskScore,
                Recommendation = strategy.GetRecommendation(riskScore)
            };

            _reports.Add(report);
            return report;
        }

        /// <summary>
        /// Gibt alle erzeugten Berichte zurueck.
        /// </summary>
        /// <returns>Eine Liste aller Berichte.</returns>
        public void GetAllReports()
        {
            var reports = _reports
                .OrderByDescending(report => report.RiskScore)
                .ThenBy(report => report.CreatureName)
                .ThenBy(report => report.IncidentTitle)
                .ToList();

            if (reports.Count == 0)
            {
                Console.WriteLine("Keine Risikoberichte vorhanden.");
                return;
            }

            Console.WriteLine("Alle Risikoberichte:");
            foreach (var report in reports)
            {
                PrintReport(report);
            }
        }

        /// <summary>
        /// Gibt den zuletzt erzeugten Bericht zurueck.
        /// </summary>
        /// <returns>Der letzte Bericht oder <see langword="null"/>, wenn noch keiner existiert.</returns>
        public void GetLatestReport()
        {
            var latestReport = _reports.LastOrDefault();

            if (latestReport is null)
            {
                Console.WriteLine("Kein Risikobericht vorhanden.");
                return;
            }

            Console.WriteLine("Letzter Risikobericht:");
            PrintReport(latestReport);
        }

        /// <summary>
        /// Gibt alle Berichte nach absteigendem Risikowert sortiert zurueck.
        /// </summary>
        public List<RiskReport> GetReportsOrderedByRisk()
        {
            return [.. _reports
                .OrderByDescending(report => report.RiskScore)
                .ThenBy(report => report.CreatureName)
                .ThenBy(report => report.IncidentTitle)];
        }

        /// <summary>
        /// Gibt nur Berichte zurueck, deren Risikowert mindestens dem angegebenen Grenzwert entspricht.
        /// </summary>
        /// <param name="minimumRiskScore">Der minimale Risikowert fuer die Rueckgabe.</param>
        public void GetHighRiskReports(int minimumRiskScore = 61)
        {
            var reports = _reports
                .Where(report => report.RiskScore >= minimumRiskScore)
                .OrderByDescending(report => report.RiskScore)
                .ThenBy(report => report.CreatureName)
                .ThenBy(report => report.IncidentTitle)
                .ToList();

            if (reports.Count == 0)
            {
                Console.WriteLine($"Keine Risikoberichte mit einem Risikowert von mindestens {minimumRiskScore} vorhanden.");
                return;
            }

            Console.WriteLine($"Risikoberichte ab Score {minimumRiskScore}:");
            foreach (var report in reports)
            {
                PrintReport(report);
            }
        }

        /// <summary>
        /// Berechnet den durchschnittlichen Risikowert aller erzeugten Berichte.
        /// </summary>
        public double GetAverageRiskScore()
        {
            return _reports.Count == 0 ? 0 : _reports.Average(report => report.RiskScore);
        }

        /// <summary>
        /// Hilfsmethode zur Ausgabe eines einzelnen Berichts in der Konsole.
        /// </summary>
        /// <param name="report"></param>
        private static void PrintReport(RiskReport report)
        {
            Console.WriteLine(
                $"- Kreatur: {report.CreatureName}, Vorfall: {report.IncidentTitle}, Strategie: {report.StrategyName}, Risiko: {report.RiskScore}, Empfehlung: {report.Recommendation}");
        }
    }
}
