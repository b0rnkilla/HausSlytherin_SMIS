namespace HausSlytherin_SMIS.Enum
{
    /// <summary> Enum für die verschiedenen Log-Level. </summary>
    public enum Level
    {   
        /// <summary> Debug: Detaillierte Informationen, hauptsächlich für Entwickler zur Fehlersuche. Normalerweise in der Produktion nicht aktiviert. </summary>
        Debug, 
        /// <summary> Info: Allgemeine Informationen über den normalen Betrieb der Anwendung. </summary>
        Info,
        /// <summary> Warning: Hinweise auf potenzielle Probleme oder ungewöhnliche Situationen, die aber nicht unbedingt Fehler sind. </summary>
        Warning,
        /// <summary> Error: Fehler, die aufgetreten sind, aber die Anwendung weiterhin funktionsfähig ist. </summary>
        Error,
        /// <summary> Critical: Schwere Fehler, die wahrscheinlich zum Absturz der Anwendung führen oder sofortige Aufmerksamkeit erfordern. </summary>
        Critical
    }
}