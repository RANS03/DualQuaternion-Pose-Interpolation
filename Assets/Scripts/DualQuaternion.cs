using UnityEngine;

[System.Serializable]
public struct DualQuaternion
{
    public Quaternion r;   // Real part = rotation
    public Quaternion d;   // Dual part = translation

    public DualQuaternion(Quaternion real, Quaternion dual)
    {
        r = real;
        d = dual;
    }

    // --------------------------------------------------
    // FROM POSE
    // --------------------------------------------------
    public static DualQuaternion FromPose(
        Quaternion rotation,
        Vector3 translation)
    {
        rotation = rotation.normalized;

        // Translation as a pure quaternion
        Quaternion t = new Quaternion(
            translation.x,
            translation.y,
            translation.z,
            0f
        );

        // qd = 1/2 * t * qr
        Quaternion dual = Multiply(t, rotation);
        dual = Scale(dual, 0.5f);

        return new DualQuaternion(rotation, dual);
    }

    // --------------------------------------------------
    // TO POSE
    // --------------------------------------------------
    public void ToPose(
        out Quaternion rotation,
        out Vector3 translation)
    {
        rotation = r.normalized;

        // t = 2 * qd * inverse(qr)
        Quaternion t = Multiply(
            d,
            Quaternion.Inverse(rotation)
        );

        t = Scale(t, 2f);

        translation = new Vector3(
            t.x,
            t.y,
            t.z
        );
    }

    // --------------------------------------------------
    // MULTIPLY TWO DUAL QUATERNIONS
    // --------------------------------------------------
    public static DualQuaternion Multiply(
        DualQuaternion a,
        DualQuaternion b)
    {
        Quaternion real = Multiply(a.r, b.r);

        Quaternion dual1 = Multiply(a.r, b.d);
        Quaternion dual2 = Multiply(a.d, b.r);

        Quaternion dual = Add(dual1, dual2);

        return new DualQuaternion(real, dual);
    }

    // --------------------------------------------------
    // NORMALIZE
    // --------------------------------------------------
    public DualQuaternion Normalized()
    {
        float magnitude = Mathf.Sqrt(
            r.x * r.x +
            r.y * r.y +
            r.z * r.z +
            r.w * r.w
        );

        if (magnitude < 1e-6f)
        {
            return new DualQuaternion(
                Quaternion.identity,
                new Quaternion(0, 0, 0, 0)
            );
        }

        Quaternion real = Scale(r, 1f / magnitude);

        float dot = Quaternion.Dot(real, d);

        Quaternion dual =
            Scale(
                Subtract(d, Scale(real, dot)),
                1f / magnitude
            );

        return new DualQuaternion(real, dual);
    }

    // --------------------------------------------------
    // SLERP
    // --------------------------------------------------
    public static DualQuaternion Slerp(
        DualQuaternion a,
        DualQuaternion b,
        float u)
    {
        u = Mathf.Clamp01(u);

        // Shortest rotation path
        if (Quaternion.Dot(a.r, b.r) < 0f)
        {
            a.r = Negate(a.r);
            a.d = Negate(a.d);
        }

        // Interpolate rotation
        Quaternion real =
            Quaternion.Slerp(a.r, b.r, u);

        // Interpolate dual part
        Quaternion dual =
            Quaternion.Lerp(a.d, b.d, u);

        DualQuaternion result =
            new DualQuaternion(real, dual);

        return result.Normalized();
    }

    // ==================================================
    // HELPER FUNCTIONS
    // ==================================================

    // Quaternion multiplication
    private static Quaternion Multiply(
        Quaternion a,
        Quaternion b)
    {
        return new Quaternion(
            a.w * b.x + a.x * b.w + a.y * b.z - a.z * b.y,
            a.w * b.y - a.x * b.z + a.y * b.w + a.z * b.x,
            a.w * b.z + a.x * b.y - a.y * b.x + a.z * b.w,
            a.w * b.w - a.x * b.x - a.y * b.y - a.z * b.z
        );
    }

    // Quaternion × scalar
    private static Quaternion Scale(
        Quaternion q,
        float s)
    {
        return new Quaternion(
            q.x * s,
            q.y * s,
            q.z * s,
            q.w * s
        );
    }

    // Quaternion addition
    private static Quaternion Add(
        Quaternion a,
        Quaternion b)
    {
        return new Quaternion(
            a.x + b.x,
            a.y + b.y,
            a.z + b.z,
            a.w + b.w
        );
    }

    // Quaternion subtraction
    private static Quaternion Subtract(
        Quaternion a,
        Quaternion b)
    {
        return new Quaternion(
            a.x - b.x,
            a.y - b.y,
            a.z - b.z,
            a.w - b.w
        );
    }

    // Quaternion negation
    private static Quaternion Negate(
        Quaternion q)
    {
        return new Quaternion(
            -q.x,
            -q.y,
            -q.z,
            -q.w
        );
    }
}