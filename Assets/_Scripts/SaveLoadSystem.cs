using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace _Scripts
{
    public class SaveLoadSystem
    {
        private readonly JsonSerializerSettings _jsonSettings = new();

        public void Initialize()
        {
            _jsonSettings.Formatting = Formatting.Indented;
        }

        public void Save<T>(T objectToSave, string fileName)
        {
            string jsonString = JsonConvert.SerializeObject(objectToSave, _jsonSettings);
            File.WriteAllText(fileName, jsonString);
            Debug.Log("Data: " + jsonString + "\nPath: " + fileName);
        }

        public T Load<T>(string fileName)
        {
            string jsonString = File.ReadAllText(fileName);
            var objectToLoad = JsonConvert.DeserializeObject<T>(jsonString);
            return objectToLoad;
        }
    }
}