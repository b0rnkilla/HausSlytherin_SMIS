using HausSlytherin_SMIS.Enums;
using HausSlytherin_SMIS.Repositories;

namespace HausSlytherin_SMIS.Services
{
    public static class CreatureService
    {
        public static void AddCreature()
        {
            string name;
            while (true)
            {
                Console.Write("Bitte Kreaturname eingeben: ");
                name = (Console.ReadLine() ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Ungültige Eingabe für Kreaturname: Leere oder nur Leerzeichen.");
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
                Console.WriteLine("Bitte Kreaturtyp auswaehlen:");
                CreatureType[] values = Enum.GetValues<CreatureType>();
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
                    Console.WriteLine("Ungültige Eingabe für Spezies: Leere oder nur Leerzeichen.");
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
                    Console.WriteLine("Ungültige Eingabe für Habitat: Leere oder nur Leerzeichen.");
                    continue;
                }

                if (!habitat.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
                {
                    Console.WriteLine("Fehler: Das Habitat darf nur Buchstaben und Leerzeichen enthalten.");
                    continue;
                }

                break;
            }

            //CreatureFactory.Create(name, creatureType, species, habitat);
            // TODO: Wenn Factory fertig, o.g. Aufruf anpassen.
        }

        public static void GetAllCreatures()
        {
            var allCreatures = CreatureRepository.GetAll();

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

        public static void GetMostDangerousCreature()
        {
            var creatures = CreatureRepository.GetAll();

            if (creatures.Count == 0)
            {
                Console.WriteLine("Keine Kreaturen vorhanden, um den gefährlichsten zu bestimmen.");
                return;
            }

            var mostDangerous = creatures.OrderByDescending(c => c.DangerLevel).FirstOrDefault();

            Console.WriteLine($"Die gefährlichste Kreatur ist: {mostDangerous.Name} mit einem Gefahrenlevel von {mostDangerous.DangerLevel}");
        }

        public static void GetAverageDangerLevel()
        {
            var creatures = CreatureRepository.GetAll();

            if (creatures.Count == 0)
            {
                Console.WriteLine("Keine Kreaturen vorhanden, um den Durchschnitt zu berechnen.");
                return;
            }

            double averageDangerLevel = creatures.Average(c => c.DangerLevel);

            Console.WriteLine($"Der durchschnittliche Gefahrenlevel aller Kreaturen beträgt: {averageDangerLevel:F2}");
        }

        public static void GetRestrictedCreatures()
        {
            var creatures = CreatureRepository.GetAll();

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
