using UnityEngine;
using UnityEngine.UI;

public class DualQuaternionPoseDemo : MonoBehaviour
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

        // Read the slider value
        if (interpolationSlider != null)
        {
            u = interpolationSlider.value;
        }

        // Convert Pose A to Dual Quaternion
        DualQuaternion dqA =
            DualQuaternion.FromPose(
                poseA.rotation,
                poseA.position
            );

        // Convert Pose B to Dual Quaternion
        DualQuaternion dqB =
            DualQuaternion.FromPose(
                poseB.rotation,
                poseB.position
            );

        // Interpolate
        DualQuaternion dqInterpolated =
            DualQuaternion.Slerp(
                dqA,
                dqB,
                u
            );

        // Convert back to normal Unity pose
        dqInterpolated.ToPose(
            out Quaternion rotation,
            out Vector3 position
        );

        // Apply pose
        transform.position = position;
        transform.rotation = rotation;
    }
}