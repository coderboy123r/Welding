using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CableRope : MonoBehaviour
{
    [Header("Rope Ends")]
    public Transform startPoint;   // Fixed end
    public Transform endPoint;     // Torch end (movable)

    [Header("Rope Settings")]
    public int segmentCount = 20;
    public float segmentLength = 0.1f;
    public float ropeWidth = 0.02f;
    public int solverIterations = 50;
    public float damping = 0.1f;   // Reduce shaking
    public float stiffness = 1f;   // 0 = soft, 1 = stiff

    private LineRenderer line;
    private Vector3[] segmentPos;
    private Vector3[] segmentOldPos;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = segmentCount;
        line.startWidth = ropeWidth;
        line.endWidth = ropeWidth;

        segmentPos = new Vector3[segmentCount];
        segmentOldPos = new Vector3[segmentCount];

        if (startPoint == null || endPoint == null)
        {
            Debug.LogWarning("CableRope missing startPoint or endPoint. Disabling.");
            enabled = false;
            return;
        }

        for (int i = 0; i < segmentCount; i++)
        {
            float t = (float)i / (segmentCount - 1);
            segmentPos[i] = Vector3.Lerp(startPoint.position, endPoint.position, t);
            segmentOldPos[i] = segmentPos[i];
        }
    }

    void Update()
    {
        // If either transform is destroyed, stop the rope
        if (startPoint == null || endPoint == null)
        {
            Debug.LogWarning("CableRope endpoint destroyed. Disabling rope.");
            enabled = false;
            return;
        }

        Simulate(Time.deltaTime);
        DrawCable();
    }

    void Simulate(float deltaTime)
    {
        // Verlet integration
        for (int i = 1; i < segmentCount - 1; i++)
        {
            Vector3 velocity = (segmentPos[i] - segmentOldPos[i]) * (1f - damping);
            segmentOldPos[i] = segmentPos[i];
            segmentPos[i] += velocity + Physics.gravity * deltaTime * deltaTime;
        }

        // Lock rope ends (safe because we checked null in Update)
        segmentPos[0] = startPoint.position;
        segmentPos[segmentCount - 1] = endPoint.position;

        // Constraint solver
        for (int iter = 0; iter < solverIterations; iter++)
        {
            for (int i = 0; i < segmentCount - 1; i++)
            {
                Vector3 delta = segmentPos[i + 1] - segmentPos[i];
                float dist = delta.magnitude;
                if (dist == 0) continue;

                float diff = (dist - segmentLength) / dist;
                Vector3 offset = delta * 0.5f * diff * stiffness;

                if (i != 0) segmentPos[i] += offset;
                if (i != segmentCount - 1) segmentPos[i + 1] -= offset;
            }
        }
    }

    void DrawCable()
    {
        for (int i = 0; i < segmentCount; i++)
        {
            line.SetPosition(i, segmentPos[i]);
        }
    }
}
