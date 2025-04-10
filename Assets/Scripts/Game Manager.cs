using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject pizzaPrefab;
    public GameObject pizzasParent;
    public int rotationSpeed = 100;
    public GameObject cylinder;
    public float levelLength;
    public GameObject finishPizza;
    public int currentLevel ;
    public TextMeshProUGUI currentLevelText;
    public TextMeshProUGUI nextLevelText;
    public Slider levelProgressBar;
    public float score = 0f;
    public TextMeshProUGUI scoreText;

    public GameObject wallPrefab;
    public List<GameObject> pizzas;



    private void Start()
    {
        currentLevel = PlayerPrefs.GetInt("CurrentLevel",1);

        levelLength = cylinder.transform.localScale.y * 2;
        float length = levelLength;
        finishPizza.transform.localPosition = new Vector3(0, -levelLength, 0);
        currentLevelText.text = currentLevel.ToString();
        nextLevelText.text = (currentLevel + 1).ToString();
        levelProgressBar.value = score * 2 / levelLength;


        for (int i = 0; i > -length / 2; i--)
        {
            float randomYRotation = Random.Range(0f, 360f);
            GameObject newPizza = Instantiate(pizzaPrefab, new Vector3(0, i * 2, 0), Quaternion.identity);
            newPizza.transform.SetParent(pizzasParent.transform);

            if ((int)randomYRotation % 2 == 0)
            {
                GameObject newWall = Instantiate(wallPrefab, new Vector3(0, i * 2, 0), Quaternion.Euler(0f, randomYRotation, 0f));
                newWall.transform.SetParent(pizzasParent.transform);
                newPizza.GetComponent<Pizza>().wall = newWall;

            }
            pizzas.Add(newPizza);
        }
         
    }
    public void UpdateSlider()
    {
        score++;
        scoreText.text = score.ToString();
        levelProgressBar.value = score * 2 / levelLength;

    }
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                //Debug.Log("x " + touch.deltaPosition.x  + " y "+ touch.deltaPosition.y +"  "+touch.deltaPosition  );
                pizzasParent.transform.Rotate(new Vector3(pizzasParent.transform.rotation.x, -touch.deltaPosition.x * rotationSpeed * Time.deltaTime, pizzasParent.transform.rotation.z));

            }


        }

    }


}
