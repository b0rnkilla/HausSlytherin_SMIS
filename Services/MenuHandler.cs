using HausSlytherin_SMIS.Enums;

namespace HausSlytherin_SMIS.Services
{
    public class MenuHandler
    {
        public static void ShowMenu()
        {
            Console.WriteLine();
            Console.WriteLine("╔═══════════════════════════════════════════╗");
            Console.WriteLine("║   SLYTHERIN MAGICAL INTELLIGENCE SYSTEM   ║");
            Console.WriteLine("╠═══════════════════════════════════════════╣");
            Console.WriteLine("║   [1] Kreatur hinzufügen                  ║");
            Console.WriteLine("║   [2] Zeige Kreaturen                     ║");
            Console.WriteLine("║   [3] Forscher hinzufügen                 ║");
            Console.WriteLine("║   [5] Zeige Vorfälle                      ║");
            Console.WriteLine("║   [6] Generiere Risikobericht             ║");
            Console.WriteLine("║   [7] Zeige Risikoberichte                ║");
            Console.WriteLine("║   [8] Zeige Statistiken                   ║");
            Console.WriteLine("║   [0] Beenden                             ║");
            Console.WriteLine("╚═══════════════════════════════════════════╝");
            Console.WriteLine();
        }
    
        public static void Run(AppContainer appContainer)
        {
            while (true)
            {
                MenuHandler.ShowMenu();

                Console.Write("Auswahl: ");
                string input = Console.ReadLine() ?? string.Empty;
                Console.WriteLine();

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
                        // Geändert auf GetAllCreatures (passend zu deinem Service)
                        appContainer.CreatureService.GetAllCreatures();
                        break;

                    case MenuOptions.AddResearcher:
                        appContainer.ResearcherService.AddResearcher();
                        break;

                    case MenuOptions.AddIncident:
                        appContainer.IncidentService.AddIncident();
                        break;

                    case MenuOptions.ShowIncidents:
                        // Geändert auf GetAllIncidents (passend zu deinem Service)
                        appContainer.IncidentService.GetAllIncidents();
                        break;

                    case MenuOptions.Exit:
                        Console.WriteLine("Programm beendet.");
                        return;

                    default:
                        Console.WriteLine("Diese Funktion ist noch nicht implementiert.");
                        break;
                }

                Console.WriteLine();
                Console.WriteLine("Drücke ENTER, um zum Menü zurückzukehren.");
                Console.ReadLine();
                Console.Clear();
            }
        }
    }
}