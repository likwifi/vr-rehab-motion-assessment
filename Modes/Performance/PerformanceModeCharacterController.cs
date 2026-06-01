using System.Collections;
using System.Collections.Generic;
using obj;
using UnityEngine;
using utils;

public class PerformanceModeCharacterController : CharacterControllerBase
{
    public TextAsset movesJSON;
    private int moveIndex = 0;
    private MovesList<MoveObj> moves;

    private void Start()
    {
        // var movements = new SerializableList1<MoveObj>();
        // movements.Add(new MoveObj(0.2f, 0, " Hello, how are you doing today?",
        //     2f, "Waving", 0f, null));
        // movements.Add(new MoveObj(1f, 20, " Lets begin out workout, 20 Air Squats in 3. 2. 1. Start!",
        //     2f, "Happy Idle", 1.83333f, "Air Squat"));
        // movements.Add(new MoveObj(1f, 15, " 15 Cross Jump. in 3. 2. 1. Start!",
        //     2f, "Happy Idle", 2.042f, "Cross Jump"));
        // movements.Add(new MoveObj(1f, 10, "Rest for 10 seconds",
        //     1, "Happy Idle", 1, "Happy Idle"));
        // movements.Add(new MoveObj(1f, 15, " 15 Jumping Jacks. in 3. 2. 1. Start!",
        //     2f, "Happy Idle", 1.083f, "Jumping Jack"));
        // movements.Add(new MoveObj(1f, 10, " 10 Sit ups. in 3. 2. 1. Start!",
        //     2f, "Happy Idle", 2.250f, "Situps"));
        // movements.Add(new MoveObj(1f, 0, "Congratulations! You successfully completed your workout!",
        //     2f, "Dance", 0, null));
        //
        // SaveManager.Save(movements);
        PerformanceWorkoutObj movements = SaveManager.Load(movesJSON.ToString());
        moves = movements.moves;
        // SaveManager.Save(workout);
        StartCoroutine(nameof(WorkoutRunner), moves[0]);
    }
}
