using System.Collections.Generic;
using UnityEngine;

public class Pizza : MonoBehaviour
{

    public List<Transform> slices = new List<Transform>();
    
    public Material redMaterial;
    public GameObject wall;
    GameManager manager;
    void Start()
    {
        manager = FindAnyObjectByType<GameManager>();
        foreach (Transform slice in transform)
        { 
            slices.Add(slice);
        }
        int randomInt = Random.Range(0, slices.Count);// enum slice type 
 
        if (randomInt % 2 == 0)
        { 
            for (int i = 0; i < 2; i++)
            {
                int tempRandomInt = Random.Range(0, slices.Count);
                slices[tempRandomInt].gameObject.GetComponent<Renderer>().material = redMaterial;
                slices[tempRandomInt].gameObject.tag = "redSlice";
            } 
        }
        else if (randomInt % 3 == 0)
        {
            for (int i = 0; i < 3; i++)
            {
                int tempRandomInt = Random.Range(0, slices.Count);
                slices[tempRandomInt].gameObject.GetComponent<Renderer>().material = redMaterial;
                slices[tempRandomInt].gameObject.tag = "redSlice";
            } 
        }
        else
        {
            int tempRandomInt = Random.Range(0, slices.Count);
            slices[tempRandomInt].gameObject.GetComponent<Renderer>().material = redMaterial;
            slices[tempRandomInt].gameObject.tag = "redSlice";
        }

        slices[randomInt].gameObject.GetComponent<MeshCollider>().convex = true;
        slices[randomInt].gameObject.GetComponent<MeshCollider>().isTrigger = true;
        slices[randomInt].gameObject.tag = "gap";
        slices[randomInt].gameObject.GetComponent<MeshRenderer>().enabled = false;
         
    }
    public void DestroyPizza()
    {

        foreach (Transform slice in slices)
        {
            Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f)).normalized;
            float randomForce = Random.Range(5, 10); 
            slice.GetComponent<MeshCollider>().enabled = false;
            Rigidbody sliceRB = slice.GetComponent<Rigidbody>();
            sliceRB.isKinematic = false;
            sliceRB.AddForce(randomDirection * randomForce, ForceMode.Impulse);
        }
        Invoke(nameof(DestroySlices), 5);
         
    }
    public void DestroySlices()
    {
        manager.pizzas.Remove(gameObject);
        
        Destroy(gameObject);
        if (wall != null)
        { 
            Destroy(wall);
        }

    }

}
