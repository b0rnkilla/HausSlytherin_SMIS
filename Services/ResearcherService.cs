using HausSlytherin_SMIS.Models;
using HausSlytherin_SMIS.Enums;
using HausSlytherin_SMIS.Repositories;
using HausSlytherin_SMIS.Factories;
using AppLogger = HausSlytherin_SMIS.Logger;
using LogLevel = HausSlytherin_SMIS.Enums.Level;

namespace HausSlytherin_SMIS.Services
{
    public class ResearcherService
    {
        private readonly ResearcherRepository _researcherRepository;
        private readonly ResearcherFactory _researcherFactory; 
        public ResearcherService(ResearcherRepository researcherRepository, ResearcherFactory researcherFactory)
        {
            _researcherRepository = researcherRepository;
            _researcherFactory = researcherFactory;
        }

        /// <summary>
        /// Prueft und speichert einen neuen Forscher.
        /// </summary>
        /// <param name="researcher">Der hinzuzufuegende Forscher.</param>
        public void AddResearcher()
        {
            string name;
            while (true)
            {
                Console.Write("Bitte Forschername eingeben: ");
                name = (Console.ReadLine() ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Fehler: Die Eingabe darf nicht leer sein.");
                    continue;
                }

                if (!name.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
                {
                    Console.WriteLine("Fehler: Der Name darf nur Buchstaben und Leerzeichen enthalten.");
                    continue;
                }

                break;
            }

            House house;
            while (true)
            {
                Console.WriteLine("Bitte Haus auswählen:");
                House[] values = System.Enum.GetValues<House>();

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

                if (!int.TryParse(input, out int houseIndex))
                {
                    Console.WriteLine("Fehler: Bitte eine gültige ganze Zahl eingeben.");
                    continue;
                }

                if (houseIndex < 1 || houseIndex > values.Length)
                {
                    Console.WriteLine($"Fehler: Bitte eine Zahl zwischen 1 und {values.Length} eingeben.");
                    continue;
                }

                house = values[houseIndex - 1];
                break;
            }

            string specialization;
            while (true)
            {
                Console.Write("Bitte Spezialisierung eingeben: ");
                specialization = (Console.ReadLine() ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(specialization))
                {
                    Console.WriteLine("Fehler: Die Eingabe darf nicht leer sein.");
                    continue;
                }

                if (!specialization.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
                {
                    Console.WriteLine("Fehler: Die Spezialisierung darf nur Buchstaben und Leerzeichen enthalten.");
                    continue;
                }

                break;
            }

            int experienceLevel;
            while (true)
            {
                Console.Write("Bitte Erfahrungslevel eingeben: ");
                string input = (Console.ReadLine() ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Fehler: Die Eingabe darf nicht leer sein.");
                    continue;
                }

                if (!int.TryParse(input, out experienceLevel))
                {
                    Console.WriteLine("Fehler: Bitte eine gültige ganze Zahl eingeben.");
                    continue;
                }

                if (experienceLevel < 0)
                {
                    Console.WriteLine("Fehler: Das Erfahrungslevel darf nicht negativ sein.");
                    continue;
                }

                break;
            }

            var researcher = _researcherFactory.Create(name, house, specialization, experienceLevel);
            _researcherRepository.Add(researcher);
            Console.WriteLine("Forscher erfolgreich hinzugefügt.");
        }

        /// <summary> Gibt alle registrierten Forscher zurueck. </summary>
        /// <returns>Eine Liste aller Forscher.</returns>
        public void GetAllResearchers()
        {
            var researchers = _researcherRepository.GetAll()
                .OrderBy(researcher => researcher.Name)
                .ThenByDescending(researcher => researcher.ExperienceLevel)
                .ToList();

            if (researchers.Count == 0)
            {
                Console.WriteLine("Keine Forscher vorhanden.");
                return;
            }

            Console.WriteLine("Alle Forscher:");
            foreach (var researcher in researchers)
            {
                Console.WriteLine(
                    $"- {researcher.Name} (Haus: {researcher.House}, Spezialisierung: {researcher.Specialization}, Erfahrungslevel: {researcher.ExperienceLevel})");
            }
        }

        /// <summary> Berechnet und gibt das durchschnittliche Erfahrungslevel aller Forscher zurueck. </summary>
        public void GetAverageExperienceLevel()
        {
            var researchers = _researcherRepository.GetAll();

            if (researchers.Count == 0)
            {
                Console.WriteLine("Keine Forscher vorhanden, um den Durchschnitt zu berechnen.");
                return;
            }

            double averageExperienceLevel = researchers.Average(researcher => researcher.ExperienceLevel);
            Console.WriteLine($"Das durchschnittliche Erfahrungslevel aller Forscher betraegt: {averageExperienceLevel:F2}");
        }

        /// <summary> Gibt alle Forscher eines bestimmten Hauses zurueck. </summary>
        public void GetResearchersByHouse()
        {
            var researchers = _researcherRepository.GetAll();

            if (researchers.Count == 0)
            {
                Console.WriteLine("Keine Forscher vorhanden, um die Haeuser auszuwerten.");
                return;
            }

            var researchersByHouse = researchers
                .GroupBy(researcher => researcher.House)
                .OrderBy(group => group.Key);

            Console.WriteLine("Forscher pro Haus:");
            foreach (var houseGroup in researchersByHouse)
            {
                Console.WriteLine($"- {houseGroup.Key}: {houseGroup.Count()}");
            }
        }
    }
}