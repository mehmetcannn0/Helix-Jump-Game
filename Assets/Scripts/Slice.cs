using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slice : MonoBehaviour
{
    public Material greenMaterial;
    public Material ownMaterial; 

    private void Start()
    {
        ownMaterial = GetComponent<Renderer>().material; 

    }
    public void ChangeColor(bool combo)
    {
        if (combo)
        {
            GetComponent<Renderer>().material = greenMaterial;

        }
        else
        {
            GetComponent<Renderer>().material = ownMaterial;
        }

    }
}
