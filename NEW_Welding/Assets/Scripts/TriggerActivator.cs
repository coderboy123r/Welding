using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TriggerActivator : MonoBehaviour
{
    [Header("Lens Flare Component to Enable/Disable")]
    public LensFlareComponentSRP lensFlare;  // Drag your Lens Flare (SRP) here

    private void Start()
    {
        if (lensFlare != null)
            lensFlare.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("lens"))
        {
            Debug.Log("Player entered trigger - Lens Flare ON");
            if (lensFlare != null)
                lensFlare.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("lens"))
        {
            Debug.Log("Player exited trigger - Lens Flare OFF");
            if (lensFlare != null)
                lensFlare.enabled = false;
        }
    }
}
