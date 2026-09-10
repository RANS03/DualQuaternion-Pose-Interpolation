using UnityEngine;
using UnityEngine.UI;

public class ConventionalPoseDemo : MonoBehaviour
{
    public Transform poseA;
    public Transform poseB;

    public Slider interpolationSlider;

    [Range(0f, 1f)]
    public float u = 0.5f;

    void Update()
    {
        if (poseA == null || poseB == null)
            return;

        // Read slider value
        if (interpolationSlider != null)
        {
            u = interpolationSlider.value;
        }

        // Normal position interpolation
        Vector3 position = Vector3.Lerp(
            poseA.position,
            poseB.position,
            u
        );

        // Normal rotation interpolation
        Quaternion rotation = Quaternion.Slerp(
            poseA.rotation,
            poseB.rotation,
            u
        );

        // Apply the interpolated pose
        transform.position = position;
        transform.rotation = rotation;
    }
}