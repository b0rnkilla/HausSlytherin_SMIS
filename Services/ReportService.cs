using HausSlytherin_SMIS.Interfaces;
using HausSlytherin_SMIS.Models;
using AppLogger = HausSlytherin_SMIS.Logger;
using LogLevel = HausSlytherin_SMIS.Enum.Level;


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
        public List<RiskReport> GetAllReports()
        {
            return [.. _reports];
        }

        /// <summary>
        /// Gibt den zuletzt erzeugten Bericht zurueck.
        /// </summary>
        /// <returns>Der letzte Bericht oder <see langword="null"/>, wenn noch keiner existiert.</returns>
        public RiskReport? GetLatestReport()
        {
            return _reports.LastOrDefault();
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
        public List<RiskReport> GetHighRiskReports(int minimumRiskScore = 61)
        {
            return [.. _reports
                .Where(report => report.RiskScore >= minimumRiskScore)
                .OrderByDescending(report => report.RiskScore)];
        }

        /// <summary>
        /// Berechnet den durchschnittlichen Risikowert aller erzeugten Berichte.
        /// </summary>
        public double GetAverageRiskScore()
        {
            return _reports.Count == 0 ? 0 : _reports.Average(report => report.RiskScore);
        }
    }
}
