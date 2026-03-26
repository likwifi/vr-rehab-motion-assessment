using System;
using UnityEngine.Serialization;
using utils;

namespace InteractiveModeScripts.obj
{
    [Serializable]
    public class InteractionWorkoutObj
    {
        public string localeCode;
        public string speaker;
        public string speakerStyle;
        public MovesList<InteractionObject> moves;
        public string lessonName;
        public string timerType;
        public InteractionWorkoutObj(string lessonName, string speaker, string speakerStyle,string localCode,
            MovesList<InteractionObject> moves, string timerType="")
        {
            this.lessonName = lessonName;
            this.speaker = speaker;
            this.speakerStyle = speakerStyle;
            this.moves = moves;
            this.timerType = timerType;
        }
    }
}