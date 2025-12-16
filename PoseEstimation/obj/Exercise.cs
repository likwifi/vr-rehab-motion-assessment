using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity.Tutorial
{
    [Serializable]
    public class Exercise
    {
        public float start = 0f;
        public float end = 0f;
        public string name;
        public int fps;

        public List<PoseVector3[]> moves = new();
        public List<int[][]> angles = new();
        private int _frame = 0;

        public float[] CalculatePoseSimilarity(List<Vector3[]> currentMoves)
        {
            var poseSimilarity = new float[currentMoves[0].Length];
            var totalPoseSimilarity = 0.0;
            var windowSize = fps * 2;
            if (currentMoves.Count >= windowSize) // 2 second sliding window
            {
                // var j = currentMoves.Count - windowSize; // last portion of move
                // var k = 0;
                for (var i = _frame; i < currentMoves.Count; i++)
                {
                    if (moves.Count<currentMoves.Count)
                        break;
                    var currentMoveCoord = currentMoves[i];
                    var moveCoord = moves[i];
                    for (var a = 0; a < moveCoord.Length; a++)
                    {
                        poseSimilarity[a] += Vector3.Dot(currentMoveCoord[a], moveCoord[a].ToVec());
                        totalPoseSimilarity += poseSimilarity[a] / moveCoord.Length;
                    }
                }

                _frame += 1;
            }

            return poseSimilarity;
        }
    }
}
