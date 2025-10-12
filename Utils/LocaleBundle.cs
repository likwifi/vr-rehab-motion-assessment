using System.Collections.Generic;

namespace utils
{
    public static class LocaleBundle
    {
        // TODO change multiple jsons localization to type and locale bundle
        static Dictionary<string, string> us = new()
        {
            {"cant_see", "Hello {0}, I Can't see your full body." +
                         "Please stand back," +
                         "or press skip button, to proceed without body movement analysis."},
            {"resuming", "Resuming workout"},
            {"can_see","I can see you now, {0}, lets continue."},
            {"positive_word", "continue"},
            {"hiitW", "high intensity workout"},
            {"hiit", "high intensity"},
            {"yoga", "yoga"},
            {"strength", "strength"},
            {"core", "core"},
            {"muscle", "muscle"},
            {"muscles", "muscles"},
            {"martial", "martial"},
            {"kids", "kids"},
            {"kid", "kid"},
            {"boxing", "boxing"},
            {"kick", "kick"},
            {"gettingReady", "Excellent, getting your workout ready!"},
            {"workoutFound", "Awesome, loading {0} workout for you, meanwhile stand back and get ready."},
            {"workoutTypeQuestion", "Hey {0}, Jamie is here. What type of workout would you like to have today?"},
            {"jamieHelperPrompt", "I can load High intensity, martial, yoga, strength, and kids workouts, which one would you like?"}
        };
        static Dictionary<string, string> zh = new()
        {
            {"cant_see", "您好 {0}，我看不到您的全身。请退后一步，或者说“继续”，以便在不进行身体运动分析的情况下继续"},
            {"resuming", "恢复锻炼"},
            {"can_see","我现在可以见到你了，{0}，我们继续吧。"},
            {"positive_word", "继续"}
        };

        public static Dictionary<string, string> get()
        {
            var dict = us;
            /*if (WorkoutStarter.localeCode == "zh")
            {
                dict = zh;
            }*/

            return dict;
        }
    }
}