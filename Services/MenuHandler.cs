using HausSlytherin_SMIS.Enums;
using HausSlytherin_SMIS.Models;

namespace HausSlytherin_SMIS.Services
{
    public class MenuHandler
    {
        /// <summary> Gibt das Menü in der Konsole aus </summary>
        public static void ShowMenu()
        {
            Console.WriteLine();
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║  SLYTHERIN MAGICAL INTELLIGENCE SYSTEM ║");
            Console.WriteLine("╠════════════════════════════════════════╣");
            Console.WriteLine("║  [1] Kreatur hinzufügen                ║");
            Console.WriteLine("║  [2] Zeige Kreaturen                   ║");
            Console.WriteLine("║  [3] Forscher hinzufügen               ║");
            Console.WriteLine("║  [4] Vorfall hinzufügen                ║");
            Console.WriteLine("║  [5] Zeige Vorfälle                    ║");
            Console.WriteLine("║  [6] Generiere Risikobericht           ║");
            Console.WriteLine("║  [7] Zeige Risikoberichte              ║");
            Console.WriteLine("║  [8] Zeige Statistiken                 ║");
            Console.WriteLine("║  [0] Beenden                           ║");
            Console.WriteLine("╚════════════════════════════════════════╝");
            Console.WriteLine();
        }

        public static void Run()
        {
            while (true)
            {
                MenuHandler.ShowMenu();

                Console.Write("Auswahl: ");
                string input = Console.ReadLine() ?? string.Empty;
                Console.WriteLine(); // Leerzeile

                if (!int.TryParse(input, out int number) || !Enum.IsDefined(typeof(MenuOptions), number))
                {
                    Console.WriteLine("Ungültige Auswahl.\nBitte eine Zahl aus dem Menü eingeben.");
                    continue;
                }

                MenuOptions option = (MenuOptions)number;

                switch (option)
                {
                    case MenuOptions.AddCreature:

                        break;

                    case MenuOptions.ShowCreatures:

                        break;

                    case MenuOptions.AddResearcher:

                        break;

                    case MenuOptions.AddIncident:

                        break;

                    case MenuOptions.ShowIndicents:

                        break;

                    case MenuOptions.GenerateRiskReport:

                        break;

                    case MenuOptions.ShowReports:

                        break;

                    case MenuOptions.ShowStatistics:

                        break;

                    case MenuOptions.Exit:
                        Console.WriteLine("Programm beendet.");
                        return;

                    default:
                        Console.WriteLine("Ungültige Option.");
                        break;
                }

                Console.WriteLine(); // Leerzeile
                Console.WriteLine("Drücke ENTER, um zum Menü zurückzukehren.");
                Console.ReadLine();
                Console.Clear();
            }
        }

        public static void AddCreature(List<Creature> creatures)
        {

        }

        public static void ShowCreatures(List<Creature> creatures)
        {

        }

        public static void AddResearcher(List<Researcher> researchers)
        {

        }

        public static void AddIncident(List<Incident> incidents)
        {

        }

        public static void ShowIncidents(List<Incident> incidents)
        {

        }

        public static void GenerateRiskReport(List<Creature> creatures, List<Incident> incidents)
        {

        }

        public static void ShowRiskReports(List<RiskReport> reports)
        {

        }

        public static void ShowStatistics(List<Creature> creatures, List<Incident> incidents)
        {

        }
    }
}
