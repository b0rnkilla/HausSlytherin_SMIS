using HausSlytherin_SMIS.Models;

namespace HausSlytherin_SMIS.Interfaces
{

    /// <summary>
    /// Definiert eine Komponente, die Risikoberichte erzeugen kann.
    /// </summary>
    public interface IReportGenerator
    {
        
        /// <summary>
        /// Erzeugt einen Bericht fuer die Kombination aus Kreatur, Vorfall und Strategie.
        /// </summary>
        /// <param name="creature">Die bewertete Kreatur.</param>
        /// <param name="incident">Der bewertete Vorfall.</param>
        /// <param name="strategy">Die ausgewaehlte Risikostrategie.</param>
        /// <returns>Der erzeugte Risikobericht.</returns>
        RiskReport? Generate(Creature? creature, Incident? incident, IRiskStrategy? strategy);

    }

}
