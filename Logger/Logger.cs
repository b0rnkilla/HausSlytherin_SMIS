namespace HausSlytherin_SMIS;

using HausSlytherin_SMIS.Enum;

/// <summary> Einfacher Projekt-Logger mit taeglicher Textdatei im Dokumente-Ordner. </summary>
public static class Logger
{
    private static readonly object SyncRoot = new();
    private static readonly string FixedLogDirectory =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Logs");

    /// <summary> Initialisiert den Logger im Standardordner "Dokumente\Logs". </summary>
    static Logger()
    {
        Directory.CreateDirectory(FixedLogDirectory);
    }

    /// <summary> Gibt den Pfad der aktuellen Tages-Logdatei zurueck. </summary>
    public static string LogFilePath => BuildLogFilePath(DateTime.Now);

    /// <summary> Schreibt eine allgemeine Information in die Tages-Logdatei. </summary>
    /// <param name="message">Die zu protokollierende Nachricht.</param>
    public static void LogInfo(string message)
    {
        Write("INFO", message);
    }

    /// <summary> Schreibt eine Nachricht mit einem spezifischen Log-Level in die Tages-Logdatei. </summary>
    /// <param name="message">Die zu protokollierende Nachricht.</param>
    public static void LogInfo(Level level, string message)
    {
        Write(level.ToString(), message);
    }

    /// <summary> Schreibt einen formatierten Logeintrag in die aktuelle Tagesdatei. </summary>
    /// <param name="level">Das Log-Level des Eintrags.</param>
    /// <param name="message">Die zu schreibende Nachricht.</param>
    private static void Write(string level, string message)
    {
        var filePath = BuildLogFilePath(DateTime.Now);
        var line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}{Environment.NewLine}";

        lock (SyncRoot)
        {
            File.AppendAllText(filePath, line);
        }
    }

    private static string BuildLogFilePath(DateTime date)
    {
        return Path.Combine(FixedLogDirectory, $"Logs-{date:yyyy-MM-dd}.txt");
    }
}
