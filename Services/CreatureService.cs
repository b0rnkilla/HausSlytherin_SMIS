using System.Linq;
using HausSlytherin_SMIS;
using HausSlytherin_SMIS.Enum;
using HausSlytherin_SMIS.Enums;
using HausSlytherin_SMIS.Exceptions;
using HausSlytherin_SMIS.Factories;
using HausSlytherin_SMIS.Models;
using HausSlytherin_SMIS.Repositories;

namespace HausSlytherin_SMIS.Services
{
    public static class CreatureService
    {
        private static readonly CreatureRepository _repository = RepositoryContext.Creatures;

        public static void AddCreature()
        {
            string name;
            while (true)
            {
                Console.Write("Bitte Kreaturname eingeben: ");
                name = (Console.ReadLine() ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Ungùltige Eingabe fùr Kreaturname: Leere oder nur Leerzeichen.");
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
                Console.WriteLine("Bitte Kreaturtyp auswùhlen:");
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
                    Console.WriteLine("Fehler: Bitte eine gùltige ganze Zahl eingeben.");
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
                    Console.WriteLine("Ungùltige Eingabe fùr Spezies: Leere oder nur Leerzeichen.");
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
                    Console.WriteLine("Ung¸ltige Eingabe f¸r Habitat: Leere oder nur Leerzeichen.");
                    continue;
                }

                if (!habitat.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
                {
                    Console.WriteLine("Fehler: Das Habitat darf nur Buchstaben und Leerzeichen enthalten.");
                    continue;
                }

                break;
            }

            int dangerLevel;
            while (true)
            {
                Console.Write("Bitte Gefahrenlevel (1-10) eingeben: ");
                var input = (Console.ReadLine() ?? string.Empty).Trim();

                if (!int.TryParse(input, out dangerLevel))
                {
                    Console.WriteLine("Fehler: Bitte eine g¸ltige ganze Zahl eingeben.");
                    continue;
                }

                if (dangerLevel is < 1 or > 10)
                {
                    Console.WriteLine("Fehler: Gefahrenlevel muss zwischen 1 und 10 liegen.");
                    continue;
                }

                break;
            }

            bool isRestricted;
            while (true)
            {
                Console.Write("Ist die Kreatur eingeschr‰nkt? (j/n): ");
                var input = (Console.ReadLine() ?? string.Empty).Trim().ToLowerInvariant();

                if (input is "j" or "ja")
                {
                    isRestricted = true;
                    break;
                }

                if (input is "n" or "nein")
                {
                    isRestricted = false;
                    break;
                }

                Console.WriteLine("Fehler: Bitte 'j' oder 'n' eingeben.");
            }

            try
            {
                var nextId = _repository.GetAll().Count + 1;
                var creature = CreatureFactory.Create(
                    nextId,
                    name,
                    creatureType,
                    species,
                    habitat,
                    dangerLevel,
                    isRestricted);

                _repository.Add(creature);

                Logger.LogInfo(Level.Info, $"Kreatur '{creature.Name}' erstellt (Typ: {creature.CreatureType}, Gefahrenlevel: {creature.DangerLevel}).", console: true);
            }
            catch (InvalidDangerLevelException ex)
            {
                Logger.LogInfo(Level.Error, ex.Message, console: true);
            }
        }

        public static void GetAllCreatures()
        {
            var allCreatures = _repository.GetAll().OrderBy(c => c.Name).ToList();
            
            if (allCreatures.Count == 0)
            {
                Console.WriteLine("Keine Kreaturen vorhanden.");
                return;
            }

            Console.WriteLine("Alle Kreaturen:");
            foreach (var creature in allCreatures)
            {
                Console.WriteLine($"- {creature.Name} (Typ: {creature.CreatureType}, Spezies: {creature.Species}, Habitat: {creature.Habitat}, Gefahrenlevel: {creature.DangerLevel}, Eingeschrùnkt: {(creature.IsRestricted ? "Ja" : "Nein")})");
            }
        }

        public static void GetMostDangerousCreature()
        {
            var creatures = _repository.GetAll();

            if (creatures.Count == 0)
            {
                Console.WriteLine("Keine Kreaturen vorhanden, um den gefùhrlichsten zu bestimmen.");
                return;
            }

            var mostDangerous = creatures.OrderByDescending(c => c.DangerLevel).FirstOrDefault();

            Console.WriteLine($"Die gefùhrlichste Kreatur ist: {mostDangerous.Name} mit einem Gefahrenlevel von {mostDangerous.DangerLevel}");
        }

        public static void GetAverageDangerLevel()
        {
            var creatures = _repository.GetAll();

            if (creatures.Count == 0)
            {
                Console.WriteLine("Keine Kreaturen vorhanden, um den Durchschnitt zu berechnen.");
                return;
            }

            double averageDangerLevel = creatures.Average(c => c.DangerLevel);

            Console.WriteLine($"Der durchschnittliche Gefahrenlevel aller Kreaturen betrùgt: {averageDangerLevel:F2}");
        }

        public static void GetRestrictedCreatures()
        {
            var creatures = _repository.GetAll();

            if (creatures.Count == 0)
            {
                Console.WriteLine("Keine Kreaturen vorhanden, um die eingeschrùnkten Kreaturen zu bestimmen.");
                return;
            }

            var restrictedCreatures = creatures.Where(c => c.IsRestricted == true).ToList();

            Console.WriteLine("Eingeschrùnkte Kreaturen:"); 
            foreach (var creature in restrictedCreatures)
            {
                Console.WriteLine($"- {creature.Name} (Gefahrenlevel: {creature.DangerLevel})");
            }
        }
    }
}