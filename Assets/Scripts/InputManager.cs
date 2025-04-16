using UnityEngine;

public class InputManager : MonoBehaviour
{
    LevelManager levelManager;
    GameManager gameManager;

    private float rotationSpeed = 100f;
    public static InputManager Instance;

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
        levelManager = LevelManager.Instance;
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        if (Input.touchCount > 0 && gameManager.IsGameActive)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                float rotationY =-touch.deltaPosition.x * rotationSpeed * Time.deltaTime;               
                levelManager.RotateTower(rotationY);
            }
        }
    }
}
