using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;
    [SerializeField] TextMeshProUGUI currentLevelText;
    [SerializeField] TextMeshProUGUI nextLevelText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Slider levelProgressBar;
    [SerializeField] TextMeshProUGUI bestScoreText;


    public TextMeshProUGUI comboText;
    public TextMeshProUGUI comboUI;
    public GameObject startPanel;
    public GameObject levelProgressUI;
    private void Start()
    {
        SetBestScore();
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
}
