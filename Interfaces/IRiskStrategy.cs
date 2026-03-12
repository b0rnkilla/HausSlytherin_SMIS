using HausSlytherin_SMIS.Models;

namespace HausSlytherin_SMIS.Interfaces
{
    public interface IRiskStrategy
    {
        /// <summary> Gibt den Anzeigenamen der Strategie zurueck. </summary>
        string Name { get; }

        /// <summary> Berechnet den Risikowert fuer eine Kreatur und einen Vorfall. </summary>
        /// <param name="creature">Die zu bewertende Kreatur.</param>
        /// <param name="incident">Der zu bewertende Vorfall.</param>
        /// <returns>Der berechnete Risikowert.</returns>
        int CalculateRisk(Creature creature, Incident incident);

        /// <summary> Wandelt einen Risikowert in eine textuelle Empfehlung um. </summary>
        /// <param name="riskScore">Der berechnete Risikowert.</param>
        /// <returns>Eine Empfehlung zum Umgang mit dem Risiko.</returns>
        string GetRecommendation(int riskScore);
    }
}