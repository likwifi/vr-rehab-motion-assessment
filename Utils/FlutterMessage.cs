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

        public static void SendMessage(string message, string type="message")
        {
# if UNITY_IOS
            var flutterMessage = new FlutterMessage(type, message);
            UnityMessageManager.Instance.SendMessageToFlutter(JsonUtility.ToJson(flutterMessage));
            // var m1 = new Message {data = JsonUtility.ToJson(flutterMessage)};
            // Messages.Send(m1);
#endif
        }
    }
}