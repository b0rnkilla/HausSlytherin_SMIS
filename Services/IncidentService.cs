using HausSlytherin_SMIS.Exceptions;
using HausSlytherin_SMIS.Models;   
using HausSlytherin_SMIS.Interfaces;
using HausSlytherin_SMIS.Repositories;
using HausSlytherin_SMIS.Enums;
using HausSlytherin_SMIS.Factories;
using System.Globalization;
using AppLogger = HausSlytherin_SMIS.Logger;
using LogLevel = HausSlytherin_SMIS.Enums.Level;

namespace HausSlytherin_SMIS.Services
{
    /// <summary> Enthaelt die Geschaeftslogik fuer die Verwaltung von Vorfaellen. </summary>
    public class IncidentService
    {
        private readonly IncidentRepository _incidentRepository;
        private readonly ICreatureRepository _creatureRepository;
        private readonly IncidentFactory _incidentFactory;
        /// <summary>
        /// Erstellt den Service mit Vorfall- und Kreaturen-Repository.
        /// </summary>
        public IncidentService(IncidentRepository incidentRepository, ICreatureRepository creatureRepository, IncidentFactory incidentFactory)
        {
            _incidentRepository = incidentRepository;
            _creatureRepository = creatureRepository;
            _incidentFactory = incidentFactory;
        }

        /// <summary>
        /// Prueft und speichert einen neuen Vorfall fuer eine vorhandene Kreatur.
        /// </summary>
        /// <param name="incident">Der hinzuzufuegende Vorfall.</param>
        public void AddIncident()
        {
            var creatures = _creatureRepository.GetAll();

            if (creatures.Count == 0)
            {
                Console.WriteLine("Keine Kreaturen vorhanden. Bitte zuerst eine Kreatur anlegen.");
                return;
            }

            string title;
            while (true)
            {
                Console.Write("Bitte Vorfalltitel eingeben: ");
                title = (Console.ReadLine() ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(title))
                {
                    Console.WriteLine("Fehler: Die Eingabe darf nicht leer sein.");
                    continue;
                }

                break;
            }

            string desc;
            while (true)
            {
                Console.Write("Bitte Vorfallbeschreibung eingeben: ");
                desc = (Console.ReadLine() ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(desc))
                {
                    Console.WriteLine("Fehler: Die Eingabe darf nicht leer sein.");
                    continue;
                }

                break;
            }

            IncidentSeverity severity;
            while (true)
            {
                Console.WriteLine("Bitte Schweregrad auswählen:");
                IncidentSeverity[] values = System.Enum.GetValues<IncidentSeverity>();

                for (int i = 0; i < values.Length; i++)
                {
                    Console.WriteLine($"[{i + 1}] {values[i]}");
                }

                Console.Write("Auswahl: ");
                string input = (Console.ReadLine() ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Fehler: Die Eingabe darf nicht leer sein.");
                    continue;
                }

                if (!int.TryParse(input, out int severityIndex))
                {
                    Console.WriteLine("Fehler: Bitte eine gültige ganze Zahl eingeben.");
                    continue;
                }

                if (severityIndex < 1 || severityIndex > values.Length)
                {
                    Console.WriteLine($"Fehler: Bitte eine Zahl zwischen 1 und {values.Length} eingeben.");
                    continue;
                }

                severity = values[severityIndex - 1];
                break;
            }

            DateTime date;
            while (true)
            {
                Console.Write("Bitte Datum eingeben (TT.MM.JJJJ) oder leer fuer heute: ");
                string input = (Console.ReadLine() ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(input))
                {
                    date = DateTime.Now;
                    break;
                }

                if (!DateTime.TryParseExact(
                    input,
                    "dd.MM.yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out date))
                {
                    Console.WriteLine("Fehler: Bitte ein gueltiges Datum im Format TT.MM.JJJJ eingeben.");
                    continue;
                }

                if (date > DateTime.Now)
                {
                    Console.WriteLine("Fehler: Das Datum darf nicht in der Zukunft liegen.");
                    continue;
                }

                break;
            }

            int creatureId;
            while (true)
            {
                Console.WriteLine("Bitte Kreatur auswählen:");
                for (int i = 0; i < creatures.Count; i++)
                {
                    var creature = creatures[i];
                    Console.WriteLine($"[{i + 1}] ID {creature.Id} - {creature.Name} (Typ: {creature.CreatureType})");
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
                    Console.WriteLine("Fehler: Bitte eine gültige ganze Zahl eingeben.");
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

            Incident incident = _incidentFactory.Create(title, desc, severity, date, creatureId);
            _incidentRepository.Add(incident);
            Console.WriteLine($"Vorfall erfolgreich erfasst.");
        }
        
        public void GetAllIncidents()
        {
            var incidents = _incidentRepository.GetAll()
                .OrderByDescending(incident => incident.Date)
                .ThenByDescending(incident => incident.Severity)
                .ToList();

            if (incidents.Count == 0)
            {
                Console.WriteLine("Keine Vorfaelle vorhanden.");
                return;
            }

            Console.WriteLine("Alle Vorfaelle:");
            foreach (var incident in incidents)
            {
                var creature = _creatureRepository.GetById(incident.CreatureId);
                string creatureName = creature?.Name ?? $"Kreatur-ID {incident.CreatureId}";

                Console.WriteLine(
                    $"- {incident.Title} (Kreatur: {creatureName}, Schweregrad: {incident.Severity}, Datum: {incident.Date:dd.MM.yyyy})");
            }
        }

        /// <summary>
        /// Gibt alle Vorfaelle einer bestimmten Kreatur zurueck.
        /// </summary>
        /// <param name="creatureId">Die ID der Kreatur.</param>
        public List<Incident> GetIncidentsForCreature(int creatureId)
        {
            return _incidentRepository.GetByCreatureId(creatureId);
        }

        /// <summary>
        /// Gibt Vorfaelle mit mindestens hohem Schweregrad zurueck.
        /// </summary>
        public void GetCriticalIncidents()
        {
            var criticalIncidents = _incidentRepository.GetByMinimumSeverity(IncidentSeverity.High);

            if (criticalIncidents.Count == 0)
            {
                Console.WriteLine("Keine kritischen Vorfaelle vorhanden.");
                return;
            }

            Console.WriteLine("Kritische Vorfaelle:");
            foreach (var incident in criticalIncidents)
            {
                var creature = _creatureRepository.GetById(incident.CreatureId);
                string creatureName = creature?.Name ?? $"Kreatur-ID {incident.CreatureId}";

                Console.WriteLine(
                    $"- {incident.Title} (Kreatur: {creatureName}, Schweregrad: {incident.Severity}, Datum: {incident.Date:dd.MM.yyyy})");
            }
        }

        public void GetAverageSeverity()
        {
            var incidents = _incidentRepository.GetAll();

            if (incidents.Count == 0)
            {
                Console.WriteLine("Keine Vorfaelle vorhanden, um den Durchschnitt zu berechnen.");
                return;
            }

            double averageSeverity = incidents.Average(incident => (int)incident.Severity);
            Console.WriteLine($"Der durchschnittliche Schweregrad aller Vorfaelle betraegt: {averageSeverity:F2}");
        }


        /// <summary>
        /// Zaehlt, wie viele Vorfaelle fuer jede Kreatur existieren.
        /// </summary>
        public List<(Creature Creature, int Count)> GetIncidentCountByCreature()
        {
            return [.. _incidentRepository.GetAll()
            .GroupBy(incident => incident.CreatureId)
            .Join(
                _creatureRepository.GetAll(),
                groupedIncidents => groupedIncidents.Key,
                creature => creature.Id,
                (groupedIncidents, creature) => (Creature: creature, Count: groupedIncidents.Count()))
            .OrderByDescending(result => result.Count)
            .ThenByDescending(result => result.Creature.DangerLevel)
            .ThenBy(result => result.Creature.Name)];
        }

        /// <summary>
        /// Gibt die Kreatur mit der hoechsten Anzahl erfasster Vorfaelle zurueck.
        /// </summary>
        /// <returns>Die Kreatur mit ihrer Vorfallanzahl oder <see langword="null"/>, wenn keine Vorfaelle vorhanden sind.</returns>
        public void GetCreatureWithMostIncidents()
        {
            var incidentCounts = GetIncidentCountByCreatureData();

            if (incidentCounts.Count == 0)
            {
                Console.WriteLine("Keine Vorfaelle vorhanden, um die Kreatur mit den meisten Vorfaellen zu bestimmen.");
                return;
            }

            var mostIncidents = incidentCounts[0];
            Console.WriteLine(
                $"Die Kreatur mit den meisten Vorfaellen ist: {mostIncidents.Creature.Name} mit {mostIncidents.Count} Vorfall/Vorfaellen.");
        }

        private List<(Creature Creature, int Count)> GetIncidentCountByCreatureData()
        {
            return [.. _incidentRepository.GetAll()
                .GroupBy(incident => incident.CreatureId)
                .Join(
                    _creatureRepository.GetAll(),
                    groupedIncidents => groupedIncidents.Key,
                    creature => creature.Id,
                    (groupedIncidents, creature) => (Creature: creature, Count: groupedIncidents.Count()))
                .OrderByDescending(result => result.Count)
                .ThenByDescending(result => result.Creature.DangerLevel)
                .ThenBy(result => result.Creature.Name)];
        }
    }
}