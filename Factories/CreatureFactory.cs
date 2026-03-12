using HausSlytherin_SMIS.Enums;
using HausSlytherin_SMIS.Exceptions;
using HausSlytherin_SMIS.Models;

namespace HausSlytherin_SMIS.Factories;

public static class CreatureFactory
{
    public static Creature Create(
        int id,
        string name,
        CreatureType type,
        string species,
        string habitat,
        int dangerLevel,
        bool isRestricted)
    {
        if (dangerLevel is < 1 or > 10)
        {
            throw new InvalidDangerLevelException(dangerLevel);
        }

        return new Creature
        {
            Id = id,
            Name = name,
            CreatureType = type,
            Species = species,
            Habitat = habitat,
            DangerLevel = dangerLevel,
            IsRestricted = isRestricted
        };
    }
}

