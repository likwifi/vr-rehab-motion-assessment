using System;
using utils;

namespace obj
{
    [Serializable]
    public class PerformanceWorkoutObj
    {
        public string speaker;
        public string style;
        public MovesList<MoveObj> moves;

        public PerformanceWorkoutObj(string speaker, string style, MovesList<MoveObj> moves)
        {
            this.speaker = speaker;
            this.style = style;
            this.moves = moves;
        }
    }
}
