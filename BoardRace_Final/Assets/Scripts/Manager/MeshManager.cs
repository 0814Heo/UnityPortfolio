using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshManager : MonoBehaviour
{
   void Update()
    {
        if (this.GetComponent<MeshRenderer>().enabled == false)
            this.GetComponent<MeshRenderer>().enabled = true;
    }
}
