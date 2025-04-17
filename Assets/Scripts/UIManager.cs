using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    LevelManager levelManager;
    [SerializeField] TextMeshProUGUI currentLevelText;
    [SerializeField] TextMeshProUGUI nextLevelText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Slider levelProgressBar;
    [SerializeField] TextMeshProUGUI bestScoreText;

    public TextMeshProUGUI comboText;
    public TextMeshProUGUI comboUI;
    public GameObject startPanel;
    public GameObject levelProgressUI;

    public static UIManager Instance;

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

    private void OnEnable()
    {
        GameActions.OnComboActivated += OnComboActivated;
        GameActions.OnComboDeactivated += OnComboDeactivated;
    }

    private void Start()
    {
        levelManager = LevelManager.Instance;
        SetBestScore();
    }

    private void OnDisable()
    {
        GameActions.OnComboActivated -= OnComboActivated;
        GameActions.OnComboDeactivated -= OnComboDeactivated;
    }

    public void SetUIForStart()
    {
        levelProgressUI.SetActive(true);
        startPanel.SetActive(false);
        currentLevelText.text =levelManager. currentLevel.ToString();
        nextLevelText.text = (levelManager.currentLevel + 1).ToString();
        levelProgressBar.value = levelManager.step * 2 / levelManager.levelLength;
        SetBestScore();
    }

    public void UpdateSlider()
    {
        scoreText.text = levelManager.score.ToString();
        levelProgressBar.value = levelManager.step * 2 / (levelManager.levelLength - 2.0f);

    }

    public void SetBestScore()
    {
        int bestScore = PlayerPrefs.GetInt(Utils.BEST_SCORE, 0);

        if (bestScore < levelManager.score)
        {
            bestScoreText.text = levelManager.score.ToString();
            PlayerPrefs.SetInt(Utils.BEST_SCORE, (int)levelManager.score);
        }
        else
        {
            bestScoreText.text = bestScore.ToString();
        }
    }

    private void OnComboActivated(int comboLevel)
    {
        comboText.text = comboLevel.ToString();

        comboText.gameObject.SetActive(true);
        comboUI.gameObject.SetActive(true);
    }

    private void OnComboDeactivated()
    {
        comboText.text = "0";

        comboText.gameObject.SetActive(false);
        comboUI.gameObject.SetActive(false);
    }
}
