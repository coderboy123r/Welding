using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdScene : MonoBehaviour
{
    public enum ActionMode { Delete, Deactivate }

    [Header("Objects to Switch")]
    public GameObject objectToActivate;
    public GameObject objectToDeactivate;

    [Header("Empty Groups to Handle")]
    public GameObject[] emptyGroupsToClear;  // Group1 ke jagah array
    public ActionMode groupAction = ActionMode.Delete;

    [Header("Groups to Activate")]
    public GameObject[] emptyGroupsToActivate;  // Group2

    [Header("Extra Groups to Activate")]
    public GameObject[] extraGroupsToActivate;  // 🔥 नया group (Group3)

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CubeActive")) // Tag check
        {
            // Activate / Deactivate main objects
            if (objectToActivate != null) objectToActivate.SetActive(true);
            if (objectToDeactivate != null) objectToDeactivate.SetActive(false);

            // Handle groups to clear
            foreach (GameObject group in emptyGroupsToClear)
            {
                if (group != null)
                {
                    if (groupAction == ActionMode.Delete)
                        DeleteChildren(group);
                    else if (groupAction == ActionMode.Deactivate)
                        SetChildrenActive(group, false);
                }
            }

            // Handle groups to activate
            foreach (GameObject group in emptyGroupsToActivate)
            {
                if (group != null) SetChildrenActive(group, true);
            }

            // Handle extra groups to activate
            foreach (GameObject group in extraGroupsToActivate)
            {
                if (group != null) SetChildrenActive(group, true);
            }

            // ⚡ Player (collider object) delete
            Destroy(other.gameObject);
        }
    }

    private void DeleteChildren(GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void SetChildrenActive(GameObject parent, bool state)
    {
        foreach (Transform child in parent.transform)
        {
            child.gameObject.SetActive(state);
        }
    }
}
