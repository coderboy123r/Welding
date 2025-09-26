using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CableRope : MonoBehaviour
{
    public Transform startPoint;   // Fixed end
    public Transform endPoint;     // Torch end
    public int segmentCount = 20;
    public float segmentLength = 0.1f;
    public float ropeWidth = 0.02f;
    public int solverIterations = 50;
    public float damping = 0.1f; // Reduce shaking
    public float stiffness = 1f; // 0 = very soft, 1 = very stiff

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

        for (int i = 0; i < segmentCount; i++)
        {
            float t = (float)i / (segmentCount - 1);
            segmentPos[i] = Vector3.Lerp(startPoint.position, endPoint.position, t);
            segmentOldPos[i] = segmentPos[i];
        }
    }

    void Update()
    {
        Simulate(Time.deltaTime);
        DrawCable();
    }

    void Simulate(float deltaTime)
    {
        // Verlet Integration
        for (int i = 1; i < segmentCount - 1; i++)
        {
            Vector3 velocity = (segmentPos[i] - segmentOldPos[i]) * (1f - damping);
            segmentOldPos[i] = segmentPos[i];
            segmentPos[i] += velocity + Physics.gravity * deltaTime * deltaTime;
        }

        // Lock ends
        segmentPos[0] = startPoint.position;
        segmentPos[segmentCount - 1] = endPoint.position;

        // Apply constraints multiple times
        for (int iter = 0; iter < solverIterations; iter++)
        {
            for (int i = 0; i < segmentCount - 1; i++)
            {
                Vector3 delta = segmentPos[i + 1] - segmentPos[i];
                float dist = delta.magnitude;
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
            line.SetPosition(i, segmentPos[i]);
    }
}
