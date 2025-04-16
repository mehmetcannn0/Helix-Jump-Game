using System.Collections.Generic;
using UnityEngine;

public class Pizza : MonoBehaviour , IColorChangeable
{
    private List<Transform> slices = new List<Transform>();

    [SerializeField] LevelManager levelManager;
    public GameObject wall;
    private bool firstPizza;
    private bool finishPizza;
    private void Awake()
    {
        levelManager = FindAnyObjectByType<LevelManager>();
        firstPizza = levelManager.pizzasInLevel.Count == 0;
        if (gameObject.CompareTag(Utils.FINISH_SLICE_TAG))
        {
            finishPizza = true;
        }
    }

    void Start()
    {
        foreach (Transform slice in transform)
        {
            slices.Add(slice);

        }
      int randomInt = Random.Range(0, slices.Count);
       //   int randomInt = Random.Range(0, 2);
        if (!firstPizza && !finishPizza)
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
        if (!finishPizza)
        {
            slices[randomInt].gameObject.GetComponent<Slice>().SetGap();

        }

    }
    public void SetFinishPizza()
    { 

        transform.localPosition = new Vector3(0, -levelManager.levelLength, 0);

        foreach (Transform item in slices)
        {
            item.GetComponent<Slice>().SetFinishSlice();
        }
    }


    public void ChangeColor(bool combo)
    {
        foreach (Transform slice in slices)
        {
            slice.GetComponent<IColorChangeable>().ChangeColor(combo);
            
        }
        if (wall != null)
        {
            wall.GetComponentInChildren<IColorChangeable>().ChangeColor(combo);
        }

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
           // Destroy(wall);
            levelManager.wallsInLevel.Remove(wall);
        }


    }

}
