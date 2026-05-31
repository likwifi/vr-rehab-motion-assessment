using System.IO;
using obj;
using UnityEngine;
using UnityEngine.SceneManagement;
using utils;

public static class SaveManager
{
    private static readonly string SaveFile = Path.GetDirectoryName(SceneManager.GetActiveScene().path) + "/workout.json";
    public static void Save(PerformanceWorkoutObj data)
    {
        var jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(SaveFile, jsonData);
    }
        
    // public static void Save(MovesList<MoveObj> data)
    // {
    //     var jsonData = JsonUtility.ToJson(data);
    //     File.WriteAllText(SaveFile, jsonData);
    // }
    public static PerformanceWorkoutObj Load(string jsonData)
    {
        // Retrieve json data from storage of your choice
        // var jsonData = File.ReadAllText(SaveFile);
        
        // Then deserialize it back to an object
        var saveData = JsonUtility.FromJson<PerformanceWorkoutObj>(jsonData);
        return saveData;
    }
    public static MovesList<MoveObj> Load1(string jsonData)
    {
        // Retrieve json data from storage of your choice
        // var jsonData = File.ReadAllText(SaveFile);
        
        // Then deserialize it back to an object
        var saveData = JsonUtility.FromJson<MovesList<MoveObj>>(jsonData);
        return saveData;
    }
    


}