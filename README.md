# VR Rehab Motion Assessment

Unity C# research code for a virtual-reality rehabilitation application. The code supports pose-based exercise assessment, repetition counting, interactive workout activities, avatar control, and communication with a Flutter host application.

This repository contains the application scripts used for research and paper reproducibility. It is a code-only extract rather than a complete, directly runnable Unity project.

## Main capabilities

- Pose landmark processing and pose similarity analysis
- Exercise repetition detection and counting
- Dynamic Time Warping (DTW) experiments for action matching
- Interactive and performance workout modes
- Workout JSON loading and state management
- Unity-to-Flutter messaging
- Avatar, scene, camera, audio, and AssetBundle utilities

## Research figures

<table>
  <tr>
    <td width="50%" align="center" valign="top">
      <img src="docs/images/vr-smartphone-setup.png" width="280" alt="Trainee wearing an Oculus Quest 2 while a smartphone captures the exercise in portrait mode">
      <br>
      <sub><strong>Capture setup</strong><br>VR headset and smartphone placement for full-body exercise capture.</sub>
    </td>
    <td width="50%" align="center" valign="top">
      <img src="docs/images/virtual-trainer-environment.jpg" width="190" alt="Virtual personal trainer displayed in the immersive workout environment">
      <br>
      <sub><strong>Immersive trainer</strong><br>Virtual personal trainer in the workout environment.</sub>
    </td>
  </tr>
  <tr>
    <td colspan="2" align="center" valign="top">
      <img src="docs/images/squat-straight-back-measurement.png" width="600" alt="Chart comparing trainee and instructor back angles during squat repetitions">
      <br>
      <sub><strong>Movement assessment</strong><br>Straight-back angle comparison between the trainee and instructor during squat repetitions.</sub>
    </td>
  </tr>
</table>

Figures are reproduced from the accompanying [IEEE EDUCON 2024 paper](https://ieeexplore.ieee.org/document/10578849).

## Repository structure

```text
.
├── Avatar/                 # Avatar selection, naming, and idle animation
├── Character/              # CharacterControllerBase, shared by every workout mode
├── Loader/                 # Scene loading, AssetBundles, backend API, Flutter bridge
├── Modes/
│   ├── Interactive/        # Interactive workout mode (targets, scoring, lesson authoring)
│   ├── Performance/        # Timed follow-along performance mode
│   └── Kids/               # Simplified mode for younger users
├── PoseEstimation/         # Pose analysis, recording, compliance, repetition detection
│   ├── graphs/             # MediaPipe pose-tracking graph configs (CPU / GPU)
│   └── obj/                # Exercise, landmark, and similarity data models
├── Rendering/              # Planar reflection, skybox rotation, stop motion
│   └── Lightmapping/       # Runtime lightmap and light-probe helpers for bundled prefabs
├── Speech/                 # Wake word, text-to-speech playback, coach voice queue
├── UI/                     # Button and touch input handling
├── Utils/                  # Shared data models and Flutter messaging utilities
├── ThirdParty/
│   └── DynamicBone/        # Third-party secondary-motion asset (see Licence note)
└── docs/                   # Paper figures and authoring notes
```

Unity `.meta` files are not tracked in this repository. It is a code-only extract
intended for reading and review, so the GUID metadata carries no value here; the
scripts must be re-imported into a Unity project and rewired as described below.

## Requirements

The scripts reference the following Unity packages or integrations:

- Unity and the Universal Render Pipeline (URP)
- MediaPipe Unity integration and Google Protobuf
- Newtonsoft.Json for Unity
- TextMesh Pro
- Cinemachine
- Unity Recorder
- Flutter Unity Integration

The exact Unity and package versions used for the paper should be recorded here before publication.

## Using the code

1. Create or open a compatible Unity project.
2. Install the dependencies listed above.
3. Copy this repository into a folder under the Unity project's `Assets` directory.
4. Restore the scenes, prefabs, animator controllers, workout JSON files, and other assets referenced by the scripts.
5. Attach the relevant `MonoBehaviour` components to the corresponding GameObjects and configure their Inspector fields.

Because project assets and configuration are not included, additional wiring is required before the code will run.

## Research reproducibility

For a reproducible paper release, consider adding:

- The paper title, authors, and publication link
- The tested Unity and package versions
- Hardware and VR headset specifications
- Dataset or exercise-template preparation steps
- Experiment configuration and evaluation protocol
- An anonymized example workout and expected output
- A tagged release matching the submitted or published paper

## Security and privacy

Some scripts currently contain hard-coded service endpoints and Firebase download URLs with access tokens. Review, revoke, or replace these values before publishing the repository. Do not commit participant data, recordings, credentials, private endpoints, or other sensitive rehabilitation data.

## Citation

This code accompanies the following paper:

> M. Aslanyan, "Development of Intelligent Workout Environment for VR Devices," *2024 IEEE Global Engineering Education Conference (EDUCON)*, 2024. [IEEE Xplore](https://ieeexplore.ieee.org/document/10578849)

If you use this code in academic work, please cite:

```bibtex
@inproceedings{aslanyan2024intelligent,
  author    = {Minas Aslanyan},
  title     = {Development of Intelligent Workout Environment for VR Devices},
  booktitle = {2024 IEEE Global Engineering Education Conference (EDUCON)},
  year      = {2024},
  publisher = {IEEE},
  url       = {https://ieeexplore.ieee.org/document/10578849}
}
```

## Disclaimer

This research software is provided for experimental and academic use. It is not a medical device and must not be used as a substitute for professional clinical judgment.

## License

No license has been selected yet. Add an appropriate license before public distribution and verify that any included third-party code can be redistributed under that license.
