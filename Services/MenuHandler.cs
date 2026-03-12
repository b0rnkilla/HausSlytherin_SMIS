using HausSlytherin_SMIS.Enums;

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

        public static void Run(AppContainer appContainer)
        {
            while (true)
            {
                MenuHandler.ShowMenu();

                Console.Write("Auswahl: ");
                string input = Console.ReadLine() ?? string.Empty;
                Console.WriteLine(); // Leerzeile

                if (!int.TryParse(input, out int number) || !System.Enum.IsDefined(typeof(MenuOptions), number))
                {
                    Console.WriteLine("Ungültige Auswahl.\nBitte eine Zahl aus dem Menü eingeben.");
                    continue;
                }

                MenuOptions option = (MenuOptions)number;

                switch (option)
                {
                    case MenuOptions.AddCreature:
                        appContainer.CreatureService.AddCreature();
                        break;

                    case MenuOptions.ShowCreatures:
                        appContainer.CreatureService.GetAllCreatures();
                        break;

                    case MenuOptions.AddResearcher:
                        //appContainer.ResearcherService.AddResearcher();
                        break;

                    case MenuOptions.AddIncident:
                        //appContainer.IncidentService.AddIncident();
                        break;

                    case MenuOptions.ShowIndicents:
                        //appContainer.IncidentService.GetAllIncidents();
                        break;

                    case MenuOptions.GenerateRiskReport:
                        //appContainer.ReportService.GenerateRiskReport();
                        break;

                    case MenuOptions.ShowReports:
                        //appContainer.ReportService.GetAllReports();
                        break;

                    case MenuOptions.ShowStatistics:
                        //StatisticsService.ShowStatistics();
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
    }
}
