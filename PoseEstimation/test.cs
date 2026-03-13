// using System;
// using System.Collections;
// using System.Diagnostics;
// using Mediapipe.Unity.PoseTracking;
// using UnityEngine;
// using Debug = UnityEngine.Debug;
//
// namespace Mediapipe.Unity.Tutorial
// {
//     public class test
//     {
//         private WebCamTexture _webCamTexture;
//         private CalculatorGraph _graph;
//         private const int Width = 640;
//         private const int Height = 480;
//         private const int Fps = 24;
//         private TextAsset _configAssetGPU;
//         
//         private IEnumerator Start()
//         {
//             _graph = new CalculatorGraph(_configAssetGPU.text);
//             _graph.SetGpuResources(GpuManager.GpuResources).AssertOk();
//             
//             var webCamDevice = WebCamTexture.devices[1]; // index for front camera input
//             _webCamTexture = new WebCamTexture(webCamDevice.name, Width, Height, Fps);
//             _webCamTexture.Play();
//             
//             var sidePacket = new SidePacket();
//             sidePacket.Emplace("model_complexity", new IntPacket((int)PoseTrackingGraph.ModelComplexity.Lite));
//             _graph.StartRun(sidePacket).AssertOk();
//             var _inputTexture = new Texture2D(Width, Height, TextureFormat.RGBA32, false);
//             var _inputPixelData = new Color32[Width * Height];
//             var stopwatch = new Stopwatch();
//             stopwatch.Start();
//             
//             var poseLandmarksStream =
//                 new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(_graph, "pose_landmarks");
//             poseLandmarksStream.StartPolling().AssertOk();
//
//             while (true)
//             {
//                 try
//                 {
//                     _inputTexture.SetPixels32(_webCamTexture.GetPixels32(_inputPixelData));
//                     
//                     var imageFrame = new ImageFrame(ImageFormat.Types.Format.Srgba, Width, Height, _width * 4,
//                         _inputTexture.GetRawTextureData<byte>());
//                     var currentTimestamp = stopwatch.ElapsedTicks / (TimeSpan.TicksPerMillisecond / 1000);
//
//                     _graph.AddPacketToInputStream("input_video",
//                             new ImageFramePacket(imageFrame, new Timestamp(currentTimestamp)))
//                         .AssertOk();
//                 } catch (Exception e) {
//                     Debug.LogError(e);
//                 }
//             }
//         }
//     }
// }