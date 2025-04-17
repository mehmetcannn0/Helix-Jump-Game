using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    GameManager gameManager;
    UIManager uiManager;
    PrefabManager prefabManager;

    [SerializeField] Transform pizzasParent; 
    [SerializeField] Pizza finishPizza;


    public GameObject cylinder;
    public List<GameObject> pizzasInLevel;
    public List<GameObject> wallsInLevel;

    public int currentLevel { get; private set; }
    public float levelLength { get; private set; }
    public float score { get; private set; }
    public int step { get; private set; }

    public static LevelManager Instance;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        gameManager = GameManager.Instance;
        uiManager = UIManager.Instance;
        prefabManager = PrefabManager.Instance;
        currentLevel = 1;
    }


    public void StartLevel()
    {
        step = 0;
        gameManager.OnLevelStart();
        cylinder.transform.localScale = new Vector3(1.5f, currentLevel * 20, 1.5f);
        cylinder.transform.localPosition = new Vector3(0, -currentLevel * 20, 0);
        levelLength = cylinder.transform.localScale.y * 2;
        finishPizza.enabled = true;
        finishPizza .SetFinishPizza();
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
            newPizza.transform.SetParent(pizzasParent);  
            newPizza.transform.localPosition= new Vector3(0, i * 2, 0);

            if ((int)randomYRotation % 2 == 0 && i != 0)
            {
                GameObject newWall = Instantiate(prefabManager.wallPrefab, new Vector3(0, i * 2, 0), Quaternion.Euler(0f, randomYRotation, 0f));
                newWall.transform.SetParent(pizzasParent);
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
            uiManager.SetUIForStart();
        }

    }

    public void RotateTower(float rotationY)
    {
        Vector3 rotationVector = new Vector3(0, rotationY, 0);
        pizzasParent.Rotate(rotationVector);
    }
    public void IncreaseStep()
    {
        step++;
    }
    public void IncreaseScore(int value)
    {
        score += value;

    }

}
