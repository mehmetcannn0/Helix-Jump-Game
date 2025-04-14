using System.Collections.Generic;
using UnityEngine;

public class Pizza : MonoBehaviour
{

    [SerializeField] LevelManager levelManager;
    public List<Transform> slices = new List<Transform>();
    public GameObject wall;
    private bool firstPizza = false;
    private void Awake()
    {
        levelManager = FindAnyObjectByType<LevelManager>();
        firstPizza = levelManager.pizzasInLevel.Count == 0; 
    }

    void Start()
    {
        foreach (Transform slice in transform)
        {
            slices.Add(slice);

        }
        int randomInt = Random.Range(0, slices.Count);
        //int randomInt = Random.Range(0, 2);
        if (!firstPizza)
        {
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
        levelManager.pizzasInLevel.Remove(gameObject);
        Destroy(gameObject);
        if (wall != null)
        {
            Destroy(wall);
            levelManager.wallsInLevel.Remove(wall);
        }


    }

}
