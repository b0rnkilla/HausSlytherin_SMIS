using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using HausSlytherin_SMIS.Interfaces;
using HausSlytherin_SMIS.Models;

namespace HausSlytherin_SMIS.Repositories
{
    public class CreatureRepository : ICreatureRepository
    {
        private readonly List<Creature> _creatures = new();
        private readonly string _filePath = "creatures.json";

        public CreatureRepository()
        {
            _creatures = LoadFromFile();
        }

        private List<Creature> LoadFromFile()
        {
            if (!File.Exists(_filePath)) return new List<Creature>();

            try
            {
                string json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<Creature>>(json) ?? new List<Creature>();
            }
            catch (Exception)
            {
                return new List<Creature>();
            }
        }

        private void SaveToFile()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(_creatures, options);
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Speichern der Datei: {ex.Message}");
            }
        }

        public void Add(Creature item)
        {
            _creatures.Add(item);
            SaveToFile();
        }

        public void Remove(int id)
        {
            var creature = GetById(id);
            if (creature != null)
            {
                _creatures.Remove(creature);
                SaveToFile();
            }
        }

        public List<Creature> GetAll() => _creatures;

        public Creature? GetById(int id) => _creatures.FirstOrDefault(c => c.Id == id);

        public Creature? GetByName(string name) => 
            _creatures.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        public List<Creature> GetRestrictedCreatures() => 
            _creatures.Where(c => c.IsRestricted).ToList();
    }
}