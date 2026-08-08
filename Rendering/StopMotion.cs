using System.Collections.Generic;
using UnityEngine;
using System.Collections;


public class StopMotion : MonoBehaviour
{
    public Transform RootBone;
    public int StoppedFrameCount = 5;

    int recordedFrame = -1;

    List<Transform> transforms = null;
    List<STransform> actualPositions = null;
    List<STransform> renderedPositions = null;

    IEnumerator endOfFrameCoroutine;

    void OnEnable()
    {
        transforms = null;
        actualPositions = null;
        renderedPositions = null;

        endOfFrameCoroutine = EndOfFrameCoroutine();

        StartCoroutine(endOfFrameCoroutine);
    }

    void OnDisable()
    {
        StopCoroutine(endOfFrameCoroutine);
    }

    void LateUpdate()
    {
        if (transforms == null)
        {
            transforms = new List<Transform>(RootBone.GetComponentsInChildren<Transform>());
        }

        RecordTransform(ref actualPositions);

        if (renderedPositions == null || Time.frameCount - recordedFrame >= StoppedFrameCount)
        {
            recordedFrame = Time.frameCount;

            RecordTransform(ref renderedPositions);
        }
        else
        {
            RestoreRecord(renderedPositions);
        }
    }

    IEnumerator EndOfFrameCoroutine()
    {
        var endOfFrame = new WaitForEndOfFrame();

        while (true)
        {
            yield return endOfFrame;

            RestoreRecord(actualPositions);
        }
    }

    void RecordTransform(ref List<STransform> record)
    {
        if (record == null)
        {
            record = new List<STransform>(transforms.Count);

            foreach (var t in transforms)
            {
                record.Add(STransform.FromTransform(t));
            }
        }
    }
}
