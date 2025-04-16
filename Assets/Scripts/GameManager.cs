using UnityEngine;

public class GameManager : MonoBehaviour
{
    PrefabManager prefabManager;
    LevelManager levelManager;
    UIManager uiManager;

    public bool IsGameActive { get; private set; }
    public bool isRestart = true;
    public Transform ballTransform;

    public static GameManager Instance;

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
        prefabManager = PrefabManager.Instance;
        levelManager = LevelManager.Instance;
        uiManager = UIManager.Instance;
    }

    public void StartGame()
    {
        levelManager.StartLevel();
        levelManager.CreatePizzasAndWalls();
    }

    public void OnLevelStart()
    {
        IsGameActive = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        levelManager.RestartLevel();
        uiManager.UpdateSlider();
        isRestart = true;
    }

    public void NextLevel()
    {
        levelManager.NextLevel();
        isRestart = false;
        RestartGame();
    }

    public void GameOver()
    {
        IsGameActive = false;
        uiManager.startPanel.SetActive(true);
        uiManager.SetBestScore();
    }
}
