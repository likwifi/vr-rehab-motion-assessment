using System;

namespace obj
{
    [Serializable]
    public class MoveObj
    {
        public float TransitionDuration;

        public int TotalRepeats;
        // public float RepeatDuration;

        public string AnnounceText;
        public float BeforeAnnounceAnimationTime;
        public string BeforeAnnounceAnimationName;

        public float AfterAnnounceAnimationTime;
        public string AfterAnnounceAnimationName;


        // TODO add speaker voice
        public MoveObj(float transitionDuration, int totalRepeats, string announceText, float beforeAnnounceAnimationTime,
            string beforeAnnounceAnimationName, float afterAnnounceAnimationTime, string afterAnnounceAnimationName)
        {
            TransitionDuration = transitionDuration;
            TotalRepeats = totalRepeats;
            // RepeatDuration = repeatDuration;
            AnnounceText = announceText;
            BeforeAnnounceAnimationTime = beforeAnnounceAnimationTime;
            BeforeAnnounceAnimationName = beforeAnnounceAnimationName;
            AfterAnnounceAnimationTime = afterAnnounceAnimationTime;
            AfterAnnounceAnimationName = afterAnnounceAnimationName;
        }
    }
}