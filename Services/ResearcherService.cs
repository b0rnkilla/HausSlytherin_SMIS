using HausSlytherin_SMIS.Models;
using HausSlytherin_SMIS.Enums;
using HausSlytherin_SMIS.Repositories;
using AppLogger = HausSlytherin_SMIS.Logger;
using LogLevel = HausSlytherin_SMIS.Enum.Level;

namespace HausSlytherin_SMIS.Services
{
    
        private readonly ResearcherRepository _researcherRepository;

    /// <summary>
    /// Erstellt den Service mit dem benoetigten Forscher-Repository.
    /// </summary>
    public ResearcherService(ResearcherRepository researcherRepository)
    {
        _researcherRepository = researcherRepository;
    }

    /// <summary>
    /// Prueft und speichert einen neuen Forscher.
    /// </summary>
    /// <param name="researcher">Der hinzuzufuegende Forscher.</param>
    public bool AddResearcher(Researcher? researcher)
    {
        if (researcher is null)
        {
            AppLogger.LogInfo(LogLevel.Error,"Der Forscher ist erforderlich.", true);
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(researcher.Name))
        {
            AppLogger.LogInfo(LogLevel.Error, "Der Name des Forschers ist erforderlich.", true);
            return false;
        }

        if (string.IsNullOrWhiteSpace(researcher.Specialization))
        {
            AppLogger.LogInfo(LogLevel.Error, "Die Spezialisierung des Forschers ist erforderlich.", true);
            return false;
        }

        if (!System.Enum.IsDefined(typeof(House), researcher.House))
        {
            AppLogger.LogInfo(LogLevel.Error, "Das Haus des Forschers ist ungueltig.", true);
            return false;
        }

        if(researcher.ExperienceLevel < 0)
        {
            AppLogger.LogInfo(LogLevel.Error, "Das Erfahrungslevel darf nicht negativ sein", true);
            return false;
        }

        _researcherRepository.Add(researcher);
        AppLogger.LogInfo(LogLevel.Info, $"Forscher erstellt: {researcher.Name} (ID {researcher.Id})");
        return true;
    }   
    
    /// <summary> Gibt alle registrierten Forscher zurueck. </summary>
    /// <returns>Eine Liste aller Forscher.</returns>
    public List<Researcher> GetAllResearchers()
    {
        return _researcherRepository.GetAll();
    }

}