using System;
using HouseSlytherin_SMIS.Models;
using HouseSlytherin_SMIS.Interfaces;
using HouseSlytherin_SMIS.Repositories;

namespace HausSyltherin_SMIS.Factories
{
    public class ResearcherFactory
    {

        private int _idCounter = 1;
        private readonly ReseacherRepository _repo;

        public ResearcherFactory(ResearcherRepository repo) => _repo = repo;

        public bool Create(string name, House house, string spec, int level)
        {
            var obj = new Researcher 
            {
                Id = _idCounter++,
                Name = name,
                House = house,
                Specialization = spec,
                ExperienceLevel = level

            };
            _repo.Add(obj);
            return true;
        }

    }
}