using UnityEngine;

public class GameManager : MonoBehaviour
{
    PrefabManager prefabManager;
    LevelManager levelManager;
    UIManager uiManager;

    public Transform ballTransform;
    public bool IsGameActive { get; private set; }
    public bool isRestart = true;

    public static GameManager Instance;

    private void Awake()
    {
        // E�er ba�ka bir �rnek zaten varsa ve bu de�ilse, kendini yok et
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Singleton instance'� ayarla
        Instance = this;

        // Bu objenin sahneler aras�nda yok olmamas�n� sa�la (opsiyonel)
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
