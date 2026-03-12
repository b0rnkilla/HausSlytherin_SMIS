namespace HausSlytherin_SMIS.Exceptions;

/// <summary>
/// Wird ausgelöst, wenn eine Kreatur einen ungültigen Danger Level verwendet.
/// </summary>
public class InvalidDangerLevelException : Exception
{
    /// <summary>
    /// Erstellt die Exception für einen ungültigen numerischen Danger Level.
    /// </summary>
    /// <param name="dangerLevel">Der ungültige numerische Danger Level.</param>
    public InvalidDangerLevelException(int dangerLevel)        : base($"Der Dangerlevel '{dangerLevel}' ist ungültig. Er muss zwischen 1 und 10 liegen.")
    {
        
    }

    /// <summary>
    /// Erstellt die Exception mit einer benutzerdefinierten Fehlermeldung.
    /// </summary>
    /// <param name="message">Die benutzerdefinierte Fehlermeldung.</param>
    public InvalidDangerLevelException(string message) : base(message)
    {
        
    }
}
