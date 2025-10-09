using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MIG_Torch : MonoBehaviour
{
    public GameObject lens;

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("torch"))
        {
            lens.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("torch"))
        {
            lens.SetActive(false);
        }
    }
}
