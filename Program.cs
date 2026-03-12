using HausSlytherin_SMIS.Services;

namespace HausSlytherin_SMIS
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AppContainer appContainer = new AppContainer();

            MenuHandler.Run(appContainer);
        }
    }
}
