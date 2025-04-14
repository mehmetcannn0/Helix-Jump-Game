using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] PrefabManager prefabManager;
    [SerializeField] LevelManager levelManager;
    [SerializeField] UIManager uiManager;

    public Transform ballTransform;

    public int rotationSpeed = 100;
    public bool isGameActive = false;
    public bool isRestart = true;
     

    
    public void StartGame()
    {
        levelManager.StartLevel();
        levelManager.CreatePizzasAndWalls();
    }

    public void GameOver()
    { 
        isGameActive = false;
        uiManager.startPanel.SetActive(true);
        uiManager.SetBestScore(); 
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

    private void Update()
    {
        if (Input.touchCount > 0 && isGameActive)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                levelManager.pizzasParent.transform.Rotate(new Vector3(levelManager.pizzasParent.transform.rotation.x, -touch.deltaPosition.x * rotationSpeed * Time.deltaTime, levelManager.pizzasParent.transform.rotation.z));
            } 
        }
         
    }

}
