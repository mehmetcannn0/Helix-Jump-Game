using System.Collections.Generic;
using UnityEngine;

public class Pizza : MonoBehaviour
{

    public List<Transform> slices = new List<Transform>(); 
    public GameObject wall;
    GameManager manager;
    void Start()
    {
        manager = FindAnyObjectByType<GameManager>();
        foreach (Transform slice in transform)
        {
            slices.Add(slice);

        }
        int randomInt = Random.Range(0, slices.Count);
        int redCount;

        if (randomInt % 2 == 0)
        {
            redCount = 2;
        }
        else if (randomInt % 3 == 0)
        {
            redCount = 2;
        }
        else
        {
            redCount = 1;
        }

        while (redCount > 0)
        {
            int tempRandomInt = Random.Range(0, slices.Count);
            slices[tempRandomInt].gameObject.GetComponent<Slice>().SetRedSlice();
            redCount--;
        }

        slices[randomInt].gameObject.GetComponent<Slice>().SetGap();

    }
    public void DestroyPizza()
    {
        if (wall != null)
        {
            wall.GetComponentInChildren<Wall>().DestroyWall();
        }
        foreach (Transform slice in slices)
        {
            slice.GetComponent<Slice>().DestroySlice();
        }
        Invoke(nameof(DestroyObjects), 2);

    }
    public void DestroyObjects()
    {
        manager.pizzas.Remove(gameObject);
        Destroy(gameObject);
        if (wall != null)
        {
            Destroy(wall);
            manager.walls.Remove(wall);   
        }


    }

}
