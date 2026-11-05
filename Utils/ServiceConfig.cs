// Service endpoints used by the research application.
//
// The deployment URLs and storage download tokens that were previously hard-coded
// in the scripts have been removed from this public repository. Replace the
// placeholder values below with your own services before running the code.
public static class ServiceConfig
{
    // Base URL of the speech service that returns synthesised audio for coaching
    // sentences and background music (paths "/speech/requestSpeech" and
    // "/speech/requestMusicByType" are appended by the callers).
    public const string SpeechServiceBaseUrl = "https://YOUR-SPEECH-SERVICE";

    // Optional self-hosted speech-synthesis endpoint used by SpeechUtils.
    public const string SynthAudioUrl = "http://YOUR-SYNTH-SERVER/synth-audio";

    // Download URLs for Unity AssetBundles and workout JSON files
    // (for example, files in a cloud storage bucket).
    public const string EditorTestBundleUrl = "https://YOUR-STORAGE/bundles/hiit/trainer-IOS";
    public const string EditorTestWorkoutJsonUrl = "https://YOUR-STORAGE/jsons/hiit/workout.json";
    public const string InteractiveLessonJsonUrl = "https://YOUR-STORAGE/classes/lesson1.json";
    public const string DemoLessonBundleUrl = "https://YOUR-STORAGE/classes/lesson_1-IOS";
    public const string SalsaLessonBundleUrl = "https://YOUR-STORAGE/classes/salsa_lesson_1-IOS";
    public const string ForestLessonBundleUrl = "https://YOUR-STORAGE/classes/forest_lesson_1-IOS";
}
