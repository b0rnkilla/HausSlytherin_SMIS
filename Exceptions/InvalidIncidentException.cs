namespace HausSlytherin_SMIS.Exceptions;

/// <summary>  Wird ausgeloest, wennVorfalldaten ungueltig oder inkonsistent sind. </summary>
public class InvalidIncidentException : Exception
{
    
    /// <summary> Erstellt die Exception mit einer benutzerdefinierten Fehlermeldung. </summary>
    /// <param name="message">Die benutzerdefinierte Fehlermeldung.</param>
    public InvalidIncidentException(string message) : base(message)
    {
        
    }

    /// <summary> Erstellt die Exception mit einer benutzerdefinierten Meldung und InnerException. </summary>
    /// <param name="message">Die benutzerdefinierte Fehlermeldung.</param>
    /// <param name="innerException">Die ursprüngliche Exception.</param>
    public InvalidIncidentException(string message, Exception innerException) : base(message, innerException)
    {
        
    }
}
