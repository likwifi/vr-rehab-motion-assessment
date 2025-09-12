using System;
using FlutterUnityIntegration;
using UnityEngine;

namespace utils
{
    [Serializable]
    public class FlutterMessage
    {
        public string type;
        public  string message;

        public FlutterMessage(string t, string message)
        {
            this.type = t;
            this.message = message;
        }
    }
}
