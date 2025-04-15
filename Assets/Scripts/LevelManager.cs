using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] UIManager uiManager;
    [SerializeField] PrefabManager prefabManager;

    public GameObject pizzasParent;
    public GameObject cylinder;
    public GameObject finishPizza;

    public List<GameObject> pizzasInLevel;
    public List<GameObject> wallsInLevel;

    public float levelLength;
    public int currentLevel = 1;
    public float score = 0f;
    public int step = 0;


    public void StartLevel()
    {
        step = 0;
        gameManager.isGameActive = true;
        cylinder.transform.localScale = new Vector3(1.5f, currentLevel * 20, 1.5f);
        cylinder.transform.localPosition = new Vector3(0, -currentLevel * 20, 0);
        levelLength = cylinder.transform.localScale.y * 2;
        finishPizza.transform.localPosition = new Vector3(0, -levelLength, 0);

        uiManager.SetUIForStart();

        gameManager.ballTransform.gameObject.SetActive(true);

    }
    public void NextLevel()
    { 
        currentLevel++;
    }
    public void CreatePizzasAndWalls()
    { 
        float length = levelLength;
        for (int i = 0; i > -length / 2; i--)
        {
            float randomYRotation = Random.Range(0f, 360f);
            GameObject newPizza = Instantiate(prefabManager.pizzaPrefab, new Vector3(0, i * 2, 0), Quaternion.identity);
            newPizza.transform.SetParent(pizzasParent.transform);

            if ((int)randomYRotation % 2 == 0 && i !=0)
            {
                GameObject newWall = Instantiate(prefabManager.wallPrefab, new Vector3(0, i * 2, 0), Quaternion.Euler(0f, randomYRotation, 0f));
                newWall.transform.SetParent(pizzasParent.transform);
                newPizza.GetComponent<Pizza>().wall = newWall;
                newWall.GetComponentInChildren<Wall>().pizza = newPizza.GetComponent<Pizza>();
                wallsInLevel.Add(newWall);

            }
            pizzasInLevel.Add(newPizza);

        }
    }
    public void RestartLevel()
    {
        foreach (var pizza in pizzasInLevel)
        {
            Destroy(pizza);
        }
        foreach (var wall in wallsInLevel)
        {
            Destroy(wall);
        }

        pizzasInLevel.Clear();
        wallsInLevel.Clear();
        gameManager.ballTransform.GetComponent<Ball>().RegenerateBall();
        gameManager.StartGame();

        if (gameManager.isRestart)
        {
            score = 0;
            currentLevel = 1; 
        }

    }
}
