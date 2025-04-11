using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject pizzaPrefab;
    public GameObject wallPrefab;
    public GameObject pizzasParent;
    public GameObject cylinder;
    public GameObject finishPizza;
    public TextMeshProUGUI currentLevelText;
    public TextMeshProUGUI nextLevelText;
    public TextMeshProUGUI scoreText;
    public Slider levelProgressBar;
    public List<GameObject> pizzas;
    public List<GameObject> walls;
    public GameObject startPanel;
    public TextMeshProUGUI bestScoreText;
    public Transform ballTransform;
    public GameObject levelProgressUI;
    public int rotationSpeed = 100;
    public float levelLength;
    public int currentLevel = 1;
    public float score = 0f;
    public int step = 0;
    public bool isGameActive = false;
    private bool isRestart = true;



    public void StartGame()
    {

        step = 0;
        ballTransform.gameObject.SetActive(true);
        isGameActive = true;
        levelProgressUI.SetActive(true);
        startPanel.SetActive(false);

        cylinder.transform.localScale = new Vector3(1.5f, currentLevel * 20, 1.5f);
        cylinder.transform.localPosition = new Vector3(0, -currentLevel * 20, 0);

        levelLength = cylinder.transform.localScale.y * 2;

        finishPizza.transform.localPosition = new Vector3(0, -levelLength, 0);
        currentLevelText.text = currentLevel.ToString();
        nextLevelText.text = (currentLevel + 1).ToString();
        levelProgressBar.value = score * 2 / levelLength;

        CreatePizzasAndWalls();
    }
    public void CreatePizzasAndWalls()
    {
        float length = levelLength;
        for (int i = 0; i > -length / 2; i--)
        {
            float randomYRotation = Random.Range(0f, 360f);
            GameObject newPizza = Instantiate(pizzaPrefab, new Vector3(0, i * 2, 0), Quaternion.identity);
            newPizza.transform.SetParent(pizzasParent.transform);

            if ((int)randomYRotation % 2 == 0    )
            {
                GameObject newWall = Instantiate(wallPrefab, new Vector3(0, i * 2, 0), Quaternion.Euler(0f, randomYRotation, 0f));
                newWall.transform.SetParent(pizzasParent.transform);
                newPizza.GetComponent<Pizza>().wall = newWall;
                walls.Add(newWall);

            }
            pizzas.Add(newPizza);

        }
    }
    public void GameOver()
    {
        isGameActive = false;
        startPanel.SetActive(true);
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        if (bestScore < score)
        {
            bestScoreText.text = score.ToString();
        }
        else
        {
            bestScoreText.text = bestScore.ToString();
        }

    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        foreach (var pizza in pizzas)
        {
            Destroy(pizza);
        }
        foreach (var wall in walls)
        {
            Destroy(wall);
        }

        pizzas.Clear();
        ballTransform.position = new Vector3(0, 0.3f, -1.7f);
        StartGame();

        if (isRestart)
        {
            score = 0;

        }
        UpdateSlider();

    }
    public void NextLevel()
    {
        currentLevel++;
        isRestart = false;
        RestartGame();
        isRestart = true;
    }

    public void UpdateSlider()
    {
        scoreText.text = score.ToString();
        levelProgressBar.value = step * 2 / (levelLength - 1.0f);

    }
    private void Update()
    {
        if (Input.touchCount > 0 && isGameActive)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                pizzasParent.transform.Rotate(new Vector3(pizzasParent.transform.rotation.x, -touch.deltaPosition.x * rotationSpeed * Time.deltaTime, pizzasParent.transform.rotation.z));

            }


        }

    }


}
