namespace HausSlytherin_SMIS.Models
{
    public class RiskReport
    {
        public string CreatureName { get; set; } = string.Empty;
        public string IncidentTitle { get; set; } = string.Empty;
        public string StrategyName { get; set; } = string.Empty;
        public int RiskScore { get; set; }
        public string Recommendation { get; set; } = string.Empty;

        public void PrintReport() => Console.WriteLine($"Risk Report for {CreatureName}:\n" +
                                                       $"Incident: {IncidentTitle}\n" +
                                                       $"Strategy: {StrategyName}\n" +
                                                       $"Risk Score: {RiskScore}\n" +
                                                       $"Recommendation: {Recommendation}");
    }
}