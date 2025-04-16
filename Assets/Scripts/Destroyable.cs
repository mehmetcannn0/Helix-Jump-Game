using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDestroyable 
{
    DestroyableType objectType { get; set; }

    void DestroyObject();
    //void OnInteracted(Ball ball);
}
