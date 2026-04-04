using InteractiveModeScripts.obj;
using UnityEngine.SceneManagement;

namespace InteractiveModeScripts
{
    using System.IO;
    using UnityEngine;


    public static class InteractionSaveManger
    {
        private static readonly string SaveFile = Path.GetDirectoryName(SceneManager.GetActiveScene().path);

        public static void Save(InteractionWorkoutObj workout)
        {
            var jsonData = JsonUtility.ToJson(workout);
            File.WriteAllText(SaveFile +"/"+ workout.lessonName+".json", jsonData);
        }

        public static InteractionWorkoutObj Load(string jsonData)
        {
            // Retrieve json data from storage of your choice
            // var jsonData = File.ReadAllText(SaveFile);

            // Then deserialize it back to an object
            var saveData = JsonUtility.FromJson<InteractionWorkoutObj>(jsonData);
            return saveData;
        }
    }
}