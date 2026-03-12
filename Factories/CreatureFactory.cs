namespace HausSlytherin_SMIS.Factories;
using HausSlytherin_SMIS.Models;
using HausSlytherin_SMIS.Enums;

public static class CreatureFactory
{
    /// <summary>
    /// Erstellt eine vordefinierte Drachen-Kreatur.
    /// </summary>
    /// <param name="name">Der individuelle Name der Kreatur.</param>
    public static Creature CreateDragon(string name)
    {
        return new Creature
        {
            Name = name,
            Species = "Drache",
            DangerLevel = 9,
            Habitat = "Gebirgshoehlen",
            IsRestricted = true,
            CreatureType = CreatureType.Dragon
        };
    }

    /// <summary>
    /// Erstellt eine vordefinierte Basilisken-Kreatur.
    /// </summary>
    /// <param name="name">Der individuelle Name der Kreatur.</param>
    public static Creature CreateBasilisk(string name)
    {
        return new Creature
        {
            Name = name,
            Species = "Griffin",
            DangerLevel = 10,
            Habitat = "Uralte Kammern",
            IsRestricted = true,
            CreatureType = CreatureType.Griffin
        };
    }

    /// <summary>
    /// Erstellt eine vordefinierte Hippogreif-Kreatur.
    /// </summary>
    /// <param name="name">Der individuelle Name der Kreatur.</param>
    public static Creature CreateHippogriff(string name)
    {
        return new Creature
        {
            Name = name,
            Species = "Phoenix",
            DangerLevel = 6,
            Habitat = "Offene Waldlichtungen",
            IsRestricted = false,
            CreatureType = CreatureType.Phoenix
        };
    }

    /// <summary>
    /// Erstellt eine vordefinierte Acromantula-Kreatur.
    /// </summary>
    /// <param name="name">Der individuelle Name der Kreatur.</param>
    public static Creature CreateAcromantula(string name)
    {
        return new Creature
        {
            Name = name,
            Species = "Werewolf",
            DangerLevel = 8,
            Habitat = "Dichter verbotener Wald",
            IsRestricted = true,
            CreatureType = CreatureType.Werewolf
        };
    }
}

