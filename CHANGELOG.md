# Changelog

## v1.0.1

Maintenance release. No change to pose estimation or repetition detection behaviour.

- Renamed product-specific identifiers to neutral names (`LoadTrainerAssistant`,
  `trainerHelperPrompt`, `WakeWordListener`) and neutral greeting text.
- `PoseCompliance`: keyframe compliance time now uses a millisecond tick count
  (`Environment.TickCount`). It previously used `DateTime.Now.Millisecond`, the
  0–999 ms component of the current time, so durations between keyframes were
  not meaningful. This affects only the reported hold time.
- `RepetitionDetector.cs`: removed four commented-out earlier versions; the active
  detector (30-frame window, 0.1 threshold) is unchanged.
- `LICENSE`: standard MIT text so the license is detected automatically; the
  third-party exception for `ThirdParty/DynamicBone/` is stated in the README.

## v1.0.0

Code snapshot accompanying the VIRTA journal manuscript submission and the IEEE
EDUCON 2024 paper. Service endpoints moved to placeholders in
`Utils/ServiceConfig.cs`; README scope section and MIT license added.
