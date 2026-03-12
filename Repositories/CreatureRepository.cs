using HausSlytherin_SMIS.Interfaces;
using HausSlytherin_SMIS.Models;

namespace HausSlytherin_SMIS.Repositories;

public class CreatureRepository : ICreatureRepository
{
    private readonly List<Creature> _creatures = new();

    public void Add(Creature item)
    {
        _creatures.Add(item);
    }

    public List<Creature> GetAll()
    {
        return _creatures;
    }

    public Creature? GetById(int id)
    {
        return _creatures.FirstOrDefault(c => c.Id == id);
    }

    public void Remove(int id)
    {
        var creature = GetById(id);
        if (creature != null)
        {
            _creatures.Remove(creature);
        }
    }
}
