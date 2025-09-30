using UnityEngine;

public class FlexibleWire : MonoBehaviour
{
    public Transform machineAnchor;    // where the wire starts
    public Transform torchAnchor;      // where the wire ends (torch)
    public int segmentCount = 20;      // more segments = smoother cable, more cost
    public float segmentLength = 0.1f; // length of each segment
    public float stiffness = 1000f;    // how stiff the joints are
    public float damping = 5f;         // how much damping/sag

    private Rigidbody[] segments;
    private GameObject[] segmentObjects;

    void Start()
    {
        // 1. Create segments
        segments = new Rigidbody[segmentCount];
        segmentObjects = new GameObject[segmentCount];
        Transform prevTransform = machineAnchor;

        for (int i = 0; i < segmentCount; i++)
        {
            GameObject seg = new GameObject("WireSegment_" + i);
            seg.transform.parent = this.transform;

            seg.transform.position = prevTransform.position + Vector3.down * segmentLength;
            Rigidbody rb = seg.AddComponent<Rigidbody>();
            rb.mass = 0.1f;
            // maybe add Collider if needed to avoid wire penetrating objects

            // Add joint connecting to the previous segment
            var joint = seg.AddComponent<ConfigurableJoint>();
            joint.connectedBody = prevTransform.GetComponent<Rigidbody>(); // for i = 0: machineAnchor might not have RB; handle separately
            joint.axis = Vector3.forward;
            // set up limits
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Limited;
            joint.zMotion = ConfigurableJointMotion.Limited;
            JointDrive drive = new JointDrive();
            drive.positionSpring = stiffness;
            drive.positionDamper = damping;
            joint.slerpDrive = drive;

            segments[i] = rb;
            segmentObjects[i] = seg;

            prevTransform = seg.transform;
        }

        // 2. Last segment connect to torchAnchor
        // maybe via fixed joint or configurable joint
    }

    void Update()
    {
        // If needed, update positions or apply additional forces
        // For performance, consider doing most physics in FixedUpdate()

        // For example: ensure machineAnchor fixed; torchAnchor drives last segment’s position
        Rigidbody lastRB = segments[segmentCount - 1];
        Vector3 targetPos = torchAnchor.position;
        // Option 1: Use a joint already linking last segment to torch anchor
        // Option 2: Apply force to lastRB to pull toward torchAnchor
        Vector3 diff = targetPos - lastRB.position;
        lastRB.AddForce(diff * stiffness * Time.deltaTime);
    }
}
