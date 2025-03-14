using UnityEngine;
using DG.Tweening;

public class CameraRot : MonoBehaviour
{
    [Tooltip("Target GameObject to rotate. If left empty, this GameObject will be rotated.")]
    public Transform targetObject;

    [Tooltip("Time (in seconds) for a complete 360° revolution.")]
    public float rotationDuration = 5f;

    void Start()
    {
        Transform target = (targetObject != null) ? targetObject : transform;

        target.DORotate(new Vector3(0, 360, 0), rotationDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear) 
            .SetLoops(-1, LoopType.Restart);
    }
}