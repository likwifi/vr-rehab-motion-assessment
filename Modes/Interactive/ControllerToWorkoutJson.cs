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
    }
}
