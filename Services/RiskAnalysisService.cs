using HausSlytherin_SMIS.Interfaces;
using HausSlytherin_SMIS.Models;
using HausSlytherin_SMIS.Repositories;
using HausSlytherin_SMIS.Strategies;
using HausSlytherin_SMIS.Factories;
using HausSlytherin_SMIS.Services;
using AppLogger = HausSlytherin_SMIS.Logger;
using LogLevel = HausSlytherin_SMIS.Enums.Level;

namespace HausSlytherin_SMIS.Factories
{
    /// <summary> Koordiniert die Risikoanalyse ueber Kreaturen, Vorfaelle und Strategien hinweg </summary>
    public class RiskAnalysisService
    {

        private readonly ICreatureRepository _creatureRepository;
        private readonly IncidentRepository _incidentRepository;
        private readonly ReportService _reportService;
        private readonly IRiskStrategy[] _riskStrategies;

        /// <summary>
        /// Erstellt den Service mit dem benoetigten Report-Service.
        /// </summary>
        /// <param name="creatureRepository"></param>
        /// <param name="incidentRepository"></param>
        /// <param name="reportService"></param>
        public RiskAnalysisService(
            ICreatureRepository creatureRepository,
            IncidentRepository incidentRepository,
            ReportService reportService)
        {
            _creatureRepository = creatureRepository;
            _incidentRepository = incidentRepository;
            _reportService = reportService;
            _riskStrategies =
            [
                new StandardRiskStrategy(),
                new StrictRiskStrategy(),
                new ResearchRiskStrategy()
            ];
        }

        public void GenerateRiskReport()
        {
            var creatures = _creatureRepository.GetAll();
            if (creatures.Count == 0)
            {
                Console.WriteLine("Keine Kreaturen vorhanden. Bitte zuerst eine Kreatur anlegen.");
                return;
            }

            if (_incidentRepository.GetAll().Count == 0)
            {
                Console.WriteLine("Keine Vorfaelle vorhanden. Bitte zuerst einen Vorfall anlegen.");
                return;
            }

            int creatureId;
            while (true)
            {
                Console.WriteLine("Bitte Kreatur auswählen:");
                for (int i = 0; i < creatures.Count; i++)
                {
                    var creature = creatures[i];
                    Console.WriteLine($"[{i + 1}] {creature.Name} (Typ: {creature.CreatureType}, Gefahrenlevel: {creature.DangerLevel})");
                }

                Console.Write("Auswahl: ");
                string input = (Console.ReadLine() ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Fehler: Die Eingabe darf nicht leer sein.");
                    continue;
                }

                if (!int.TryParse(input, out int creatureIndex))
                {
                    Console.WriteLine("Fehler: Bitte eine gueltige ganze Zahl eingeben.");
                    continue;
                }

                if (creatureIndex < 1 || creatureIndex > creatures.Count)
                {
                    Console.WriteLine($"Fehler: Bitte eine Zahl zwischen 1 und {creatures.Count} eingeben.");
                    continue;
                }

                creatureId = creatures[creatureIndex - 1].Id;
                break;
            }

            var incidents = _incidentRepository.GetByCreatureId(creatureId);
            if (incidents.Count == 0)
            {
                var creature = _creatureRepository.GetById(creatureId);
                Console.WriteLine($"Fuer die Kreatur {creature?.Name ?? $"mit der ID {creatureId}"} sind keine Vorfaelle vorhanden.");
                return;
            }

            int incidentId;
            while (true)
            {
                Console.WriteLine("Bitte Vorfall auswählen:");
                for (int i = 0; i < incidents.Count; i++)
                {
                    var incident = incidents[i];
                    Console.WriteLine($"[{i + 1}] {incident.Title} (Schweregrad: {incident.Severity}, Datum: {incident.Date:dd.MM.yyyy})");
                }

                Console.Write("Auswahl: ");
                string input = (Console.ReadLine() ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Fehler: Die Eingabe darf nicht leer sein.");
                    continue;
                }

                if (!int.TryParse(input, out int incidentIndex))
                {
                    Console.WriteLine("Fehler: Bitte eine gueltige ganze Zahl eingeben.");
                    continue;
                }

                if (incidentIndex < 1 || incidentIndex > incidents.Count)
                {
                    Console.WriteLine($"Fehler: Bitte eine Zahl zwischen 1 und {incidents.Count} eingeben.");
                    continue;
                }

                incidentId = incidents[incidentIndex - 1].Id;
                break;
            }

            IRiskStrategy strategy;
            while (true)
            {
                Console.WriteLine("Bitte Risikostrategie auswählen:");
                for (int i = 0; i < _riskStrategies.Length; i++)
                {
                    Console.WriteLine($"[{i + 1}] {_riskStrategies[i].Name}");
                }

                Console.Write("Auswahl: ");
                string input = (Console.ReadLine() ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Fehler: Die Eingabe darf nicht leer sein.");
                    continue;
                }

                if (!int.TryParse(input, out int strategyIndex))
                {
                    Console.WriteLine("Fehler: Bitte eine gueltige ganze Zahl eingeben.");
                    continue;
                }

                if (strategyIndex < 1 || strategyIndex > _riskStrategies.Length)
                {
                    Console.WriteLine($"Fehler: Bitte eine Zahl zwischen 1 und {_riskStrategies.Length} eingeben.");
                    continue;
                }

                strategy = _riskStrategies[strategyIndex - 1];
                break;
            }

            var report = CreateRiskReport(creatureId, incidentId, strategy);

            if (report is null)
            {
                return;
            }

            Console.WriteLine("Risikobericht erstellt:");
            Console.WriteLine(
                $"- Kreatur: {report.CreatureName}, Vorfall: {report.IncidentTitle}, Strategie: {report.StrategyName}, Risiko: {report.RiskScore}, Empfehlung: {report.Recommendation}");
        }

        /// <summary>
        /// Erstellt mit der ausgewaehlten Strategie einen Risikobericht fuer eine Kreatur und einen ihrer Vorfaelle.
        /// </summary>
        /// <param name="creatureId">Die ID der Kreatur.</param>
        /// <param name="incidentId">Die ID des Vorfalls.</param>
        /// <param name="strategy">Die Risikostrategie fuer die Berechnung.</param>
        public RiskReport? CreateRiskReport(int creatureId, int incidentId, IRiskStrategy? strategy)
        {
            if (strategy is null)
            {
                Console.WriteLine("Eine Risikostrategie ist erforderlich.");
                return null;
            }

            var creature = _creatureRepository.GetById(creatureId);
            if (creature is null)
            {
                Console.WriteLine($"Die Kreatur mit der ID {creatureId} wurde nicht gefunden.");
                return null;
            }

            var incident = _incidentRepository.GetById(incidentId);
            if (incident is null)
            {
                Console.WriteLine($"Der Vorfall mit der ID {incidentId} wurde nicht gefunden.");
                return null;
            }

            if (incident.CreatureId != creatureId)
            {
                Console.WriteLine("Der ausgewaehlte Vorfall gehoert nicht zur ausgewaehlten Kreatur.");
                return null;
            }

            var report = _reportService.Generate(creature, incident, strategy);
            if (report is null)
            {
                Console.WriteLine("Der Risikobericht konnte nicht erstellt werden.");
                return null;
            }

            return report;
        }
    }
}