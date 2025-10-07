using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class LensFlareTrigger : MonoBehaviour
{
    [Header("Object to Enable/Disable")]
    public GameObject objectToToggle;   // Drag your visual GameObject (light, VFX, etc.) here

    private void Start()
    {
        // Make sure it starts OFF
        if (objectToToggle != null)
            objectToToggle.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lens"))
        {
            if (objectToToggle != null)
                objectToToggle.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Lens"))
        {
            if (objectToToggle != null)
                objectToToggle.SetActive(false);
        }
    }
}