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
    }
}
