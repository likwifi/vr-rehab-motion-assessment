using System;
using Google.Protobuf.Collections;
using UnityEngine;

namespace Mediapipe.Unity.Tutorial
{
    public class PoseGraph
    {
        private string[] LANDMARK_NAMES =
        {
            "nose",
            "left_eye_inner",
            "left_eye",
            "left_eye_outer",
            "right_eye_inner",
            "right_eye",
            "right_eye_outer",
            "left_ear",
            "right_ear",
            "mouth_left",
            "mouth_right",
            "left_shoulder",
            "right_shoulder",
            "left_elbow",
            "right_elbow",
            "left_wrist",
            "right_wrist",
            "left_pinky",
            "right_pinky",
            "left_index",
            "right_index",
            "left thumb",
            "right_thumb",
            "left_hip",
            "right_hip",
            "left_knee",
            "right_knee",
            "left_ankle",
            "right_ankle",
            "left_heel",
            "right_heel",
            "left_foot_index",
            "right_foot_index"
        };

        private int[][] links = new int[33][];


        public PoseGraph()
        {
            // eyes
            links[0] = new[] { 4, 1 };
            links[1] = new[] { 0, 2 };
            links[2] = new[] { 1, 3 };
            links[3] = new[] { 2, 7 };
            links[4] = new[] { 5, 0 };
            links[5] = new[] { 6, 4 };
            links[6] = new[] { 8, 5 };
            links[7] = new[] { 3 };
            links[8] = new[] { 6 };

            // mouth
            links[9] = new[] { 10 };
            links[10] = new[] { 9 };

            // shoulders 
            links[11] = new[] { 12, 23, 13 }; //right shoulder
            links[12] = new[] { 11, 14, 24 };

            // mid hand
            links[13] = new[] { 11, 15 };
            links[14] = new[] { 12, 16 };

            // palm
            links[15] = new[] { 21, 17, 19 };
            links[16] = new[] { 22, 20, 18 };
            links[17] = new[] { 19, 15 }; // right
            links[18] = new[] { 20, 16 }; // left
            links[19] = new[] { 15, 17 }; // index right 
            links[20] = new[] { 18, 16 }; // index left
            links[21] = new[] { 15 }; // thumb
            links[22] = new[] { 16 }; // thumb

            // hip
            links[23] = new[] { 11, 25 }; // right
            links[24] = new[] { 12, 26 }; // left

            // leg 
            links[25] = new[] { 23, 27 }; // right 
            links[26] = new[] { 24, 28 };
            links[27] = new[] { 29, 31 };
            // feet
            links[28] = new[] { 32, 30 };
            links[29] = new[] { 27, 31 };
            links[30] = new[] { 28, 32 };
            links[31] = new[] { 27, 29 };
            links[32] = new[] { 28, 30 };
        }
    }
}
