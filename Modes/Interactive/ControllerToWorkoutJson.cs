#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace InteractiveModeScripts
{
    public static class ControllerToWorkoutJson
    {
        public static void CollectClipCurves(Animator animator)
        {
            // ClipCurves.Clear();

            var runtimeAnimatorController = animator.runtimeAnimatorController;
            if (runtimeAnimatorController != null)
            {
                var clips = runtimeAnimatorController.animationClips;
                var states = ((AnimatorController)animator.runtimeAnimatorController).layers[0]
                    .stateMachine.states;
                var allData = new JArray();
                var i = 0;
                string[] bonesList =
                {
                    "Right Shoulder Front-Back","Left Shoulder Front-Back",
                    
                };

                foreach (var clip in clips)
                {
                    // var curveInfos = new List<CurveInfo>();
                    // ClipCurves.Add(new ClipInfo(clip, curveInfos));
                    var clipLength = clip.length;
                    var stateName = states[i].state.name;
                    var bones = new JArray();
                    var clipObj = new JObject
                    {
                        { "clipLength", clipLength },
                        { "clipName", stateName },
                        { "bones", bones }
                    };
                    foreach (var binding in AnimationUtility.GetCurveBindings(clip))
                    {
                        var boneName = binding.propertyName;
                        if (!bonesList.Any(boneName.Contains))
                        {
                            return;
                        }

                        // var key = CurveKey(binding.path, binding.type, binding.propertyName);
                        var curve = AnimationUtility.GetEditorCurve(clip, binding);
                        var keys = curve.keys;
                        var keysArray = new JArray();
                        foreach (var keyframe in keys)
                        {
                            var obj = new JObject
                            {
                                {"time", keyframe.time},
                                {"value", keyframe.value}
                            };
                            keysArray.Add(obj);
                        }

                        // Debug.Log(curve);
                        bones.Add(new JObject
                        {
                            { "name", boneName },
                            { "keypoints", keysArray }
                        });
                        // curveInfos.Add(new CurveInfo(key, curve));
                    }
                    allData.Add(clipObj);
                    i++;
                    Debug.Log(clipObj);
                }
                Debug.Log("aa");

                System.IO.File.WriteAllText(Application.persistentDataPath + "/yoga.json", allData.ToString(Formatting.None));

            }

            // ClipCurves.Add(new ClipInfo(animator.runtimeAnimatorController.animationClips[0],  new List<CurveInfo>())); 
        }

        public static void CollectAnimatorStatesInfo(Animator m_Animator)
        {
            var stringJSON = "";
            var runtimeAnimatorController = m_Animator.runtimeAnimatorController;
            if (runtimeAnimatorController != null)
            {
                var clips = runtimeAnimatorController.animationClips;
                var states = ((UnityEditor.Animations.AnimatorController)m_Animator.runtimeAnimatorController).layers[0]
                    .stateMachine.states;
                stringJSON += "[";
                MovesJson movesJSON = new MovesJson();
                for (var i = 0; i < clips.Length; i++)
                {
                    var stateName = states[i].state.name;
                    if (stateName.IndexOf("_rev", StringComparison.OrdinalIgnoreCase) > -1 ||
                        stateName.IndexOf("rev_", StringComparison.OrdinalIgnoreCase) > -1 ||
                        stateName.IndexOf("_idle", StringComparison.OrdinalIgnoreCase) > -1 ||
                        stateName.IndexOf("idle_", StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        continue;
                    }

                    var idleIndex = findState(stateName, states, "_idle");
                    var revIndex = findState(stateName, states, "_rev");
                    var duration = clips[i].length;
                    var moveJSON = new MoveJson
                    {
                        StateName = stateName,
                        backToIdle = true,
                        Repeating = clips[i].isLooping,
                        TotalDuration = duration,
                        filePath = AssetDatabase.GetAssetPath(clips[i])
                    };
                    if (idleIndex > -1)
                    {
                        moveJSON.Repeating = clips[idleIndex].isLooping;
                        moveJSON.TotalDuration = clips[idleIndex].length;
                    }

                    if (revIndex > -1)
                    {
                        moveJSON.BackToIdleState = states[revIndex].state.name;
                        moveJSON.BackToIdleStateDuration = clips[revIndex].length;
                    }

                    /*if (stateName.IndexOf("_rev", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        stateName.IndexOf("rev_", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        stringJSON = stringJSON.Remove(stringJSON.Length - 2, 2);
                        stringJSON += ",\"BackToIdleState\":";
                        stringJSON += "\"" + stateName + "\",";
                        stringJSON += "\"BackToIdleStateDuration\":";
                        stringJSON += duration;
                        stringJSON += "},";
                        continue;
                    }

                    if (stateName.IndexOf("_idle", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        stringJSON = stringJSON.Remove(stringJSON.Length - 2, 2);
                        stringJSON += "\"IdleDuration\":";
                        stringJSON += duration;
                        stringJSON += "},";
                        // TODO add up idle duration
                        continue;
                    }*/

                    /*
                    stringJSON += "{";
                    stringJSON += "\"TotalDuration\":";
                    stringJSON += duration + ",";
                    stringJSON += "\"StateName\":";
                    stringJSON += "\"" + stateName + "\",";
                    stringJSON += "\"backToIdle\":";
                    stringJSON += "\"" + true + "\",";
                    stringJSON += "\"Repeating\":";
                    stringJSON += clips[i].isLooping.ToString().ToLower();
                    stringJSON += "},";
                    */


                    movesJSON.moves.Add(moveJSON);
                }

                // stringJSON += "]";
                var JSON = JsonUtility.ToJson(movesJSON);
                System.IO.File.WriteAllText(Application.persistentDataPath + "/kickboxing.json", JSON);

                // Debug.Log(JSON);
            }
        }

        private static int findState(string stateName, ChildAnimatorState[] states, string query)
        {
            for (var i = 0; i < states.Length; i++)
            {
                if (states[i].state.name.Equals(stateName + query, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }

            return -1;
        }

        [Serializable]
        public class MoveJson
        {
            public float TotalDuration;
            public string StateName;
            public string BackToIdleState;
            public bool backToIdle;
            public bool Repeating;
            public float BackToIdleStateDuration;
            public string filePath;
        }
    }
}
