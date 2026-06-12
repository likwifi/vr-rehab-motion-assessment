using System.Collections.Generic;
using System.IO;
using obj;
using UnityEngine;
using utils;


namespace Lessons.Hatsune
{
    public static class SaveManager
    {
        private static readonly string SaveFile = Application.persistentDataPath + "/movements.json";
        public static void Save(MovesList<MoveObj> data)
        {
            var jsonData = JsonUtility.ToJson(data);
            File.WriteAllText(SaveFile, jsonData);
        }

        public static MovesList<MoveObj> Load(string jsonData)
        {
            // Retrieve json data from storage of your choice
            // var jsonData = File.ReadAllText(SaveFile);
        
            // Then deserialize it back to an object
            var saveData = JsonUtility.FromJson<MovesList<MoveObj>>(jsonData);
            return saveData;
        }

    }
}