using HausSlytherin_SMIS.Enums;
using HausSlytherin_SMIS.Factories;
using HausSlytherin_SMIS.Interfaces;
using HausSlytherin_SMIS.Repositories;

namespace HausSlytherin_SMIS.Services
{
    public class CreatureService
    {
        private readonly ICreatureRepository _creatureRepository;
        private readonly CreatureFactory _creatureFactory;

        public CreatureService(ICreatureRepository creatureRepository, CreatureFactory creatureFactory)
        {
            _creatureRepository = creatureRepository;
            _creatureFactory = creatureFactory;
        }


        public void AddCreature()
        {
            string name;
            while (true)
            {
                Console.Write("Bitte Kreaturname eingeben: ");
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

            CreatureType creatureType;
            while (true)
            {
                Console.WriteLine("Bitte Kreaturtyp auswählen:");
                CreatureType[] values = System.Enum.GetValues<CreatureType>();

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

                if (!int.TryParse(input, out int typeIndex))
                {
                    Console.WriteLine("Fehler: Bitte eine gültige ganze Zahl eingeben.");
                    continue;
                }

                if (typeIndex < 1 || typeIndex > values.Length)
                {
                    Console.WriteLine($"Fehler: Bitte eine Zahl zwischen 1 und {values.Length} eingeben.");
                    continue;
                }

                creatureType = values[typeIndex - 1];
                break;
            }

            string species;
            while (true)
            {
                Console.Write("Bitte Spezies eingeben: ");
                species = (Console.ReadLine() ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(species))
                {
                    Console.WriteLine("Fehler: Die Eingabe darf nicht leer sein.");
                    continue;
                }

                if (!species.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
                {
                    Console.WriteLine("Fehler: Die Spezies darf nur Buchstaben und Leerzeichen enthalten.");
                    continue;
                }

                break;
            }

            string habitat;
            while (true)
            {
                Console.Write("Bitte Habitat eingeben: ");
                habitat = (Console.ReadLine() ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(habitat))
                {
                    Console.WriteLine("Fehler: Die Eingabe darf nicht leer sein.");
                    continue;
                }

                if (!habitat.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
                {
                    Console.WriteLine("Fehler: Das Habitat darf nur Buchstaben und Leerzeichen enthalten.");
                    continue;
                }

                break;
            }

            int danger;
            while (true)
            {
                Console.Write("Bitte Gefahrenlevel (1-10) eingeben: ");
                string input = (Console.ReadLine() ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Fehler: Die Eingabe darf nicht leer sein.");
                }

                if (!int.TryParse(input, out danger))
                {
                    Console.WriteLine("Fehler: Bitte eine gültige ganze Zahl eingeben.");
                    continue;
                }

                if (danger < 1 || danger > 10)
                {
                    Console.WriteLine("Fehler: Bitte eine Zahl zwischen 1 und 10 eingeben.");
                    continue;
                }

                break;
            }

            bool restricted;
            while (true)
            {
                Console.Write("Ist die Kreatur eingeschränkt? (ja/nein): ");
                string input = (Console.ReadLine() ?? string.Empty).Trim().ToLower();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Fehler: Die Eingabe darf nicht leer sein.");
                    continue;
                }

                switch (input)
                {
                    case "ja":
                    case "j":
                        restricted = true;
                        break;
                    case "nein":
                    case "n":
                        restricted = false;
                        break;
                    default:
                        Console.WriteLine("Fehler: Bitte 'ja' oder 'nein' eingeben.");
                        continue;
                }

                break;
            }

            var creature = _creatureFactory.Create(name, species, danger, habitat, restricted, creatureType);
            _creatureRepository.Add(creature);
        }

        public void GetAllCreatures()
        {
            var allCreatures = _creatureRepository.GetAll();
            
            if (allCreatures.Count == 0)
            {
                Console.WriteLine("Keine Kreaturen vorhanden.");
                return;
            }

            Console.WriteLine("Alle Kreaturen:");
            foreach (var creature in allCreatures)
            {
                Console.WriteLine($"- {creature.Name} (Typ: {creature.CreatureType}, Spezies: {creature.Species}, Habitat: {creature.Habitat}, Gefahrenlevel: {creature.DangerLevel}, Eingeschränkt: {(creature.IsRestricted ? "Ja" : "Nein")})");
            }
        }

        public void GetMostDangerousCreature()
        {
            var creatures = _creatureRepository.GetAll();

            if (creatures.Count == 0)
            {
                Console.WriteLine("Keine Kreaturen vorhanden, um den gefährlichsten zu bestimmen.");
                return;
            }

            var mostDangerous = creatures.OrderByDescending(c => c.DangerLevel).FirstOrDefault();

            Console.WriteLine($"Die gefährlichste Kreatur ist: {mostDangerous.Name} mit einem Gefahrenlevel von {mostDangerous.DangerLevel}");
        }

        public void GetAverageDangerLevel()
        {
            var creatures = _creatureRepository.GetAll();

            if (creatures.Count == 0)
            {
                Console.WriteLine("Keine Kreaturen vorhanden, um den Durchschnitt zu berechnen.");
                return;
            }

            double averageDangerLevel = creatures.Average(c => c.DangerLevel);

            Console.WriteLine($"Der durchschnittliche Gefahrenlevel aller Kreaturen beträgt: {averageDangerLevel:F2}");
        }

        public void GetRestrictedCreatures()
        {
            var creatures = _creatureRepository.GetAll();

            if (creatures.Count == 0)
            {
                Console.WriteLine("Keine Kreaturen vorhanden, um die eingeschränkten Kreaturen zu bestimmen.");
                return;
            }

            var restrictedCreatures = creatures.Where(c => c.IsRestricted == true).ToList();

            Console.WriteLine("Eingeschränkte Kreaturen:"); 
            foreach (var creature in restrictedCreatures)
            {
                Console.WriteLine($"- {creature.Name} (Gefahrenlevel: {creature.DangerLevel})");
            }
        }
    }
}