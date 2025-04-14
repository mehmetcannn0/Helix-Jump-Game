using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private bool hasBounced = false; 
    private Vector3 ballStartPosition = new Vector3(0, 2f, -1.7f);

    [SerializeField] LevelManager levelManager;
    [SerializeField] UIManager uiManager;
    [SerializeField] int bounceForce;
    
    public Rigidbody rb;
    public GameManager manager;
    public int combo = 0;


    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }
    public void RegenerateBall()
    {
        hasBounced = false; 
        transform.position = ballStartPosition;
    }
    private void FixedUpdate()
    {
        IsBallFalling();
    }
    public void IsBallFalling()
    {
        if (rb.velocity.y < 0 && hasBounced)
        {
            hasBounced = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    { 
        if (other.TryGetComponent(out Slice slice))
        {

            if (slice.sliceType == SliceType.Gap && !hasBounced)
            { 

                combo++;
                uiManager.comboText.text = combo.ToString();

                other.gameObject.GetComponentInParent<Pizza>().DestroyPizza();

                levelManager.score += combo > 1 ? combo : 0 + levelManager.currentLevel;
                levelManager.step++;
                uiManager.UpdateSlider();

                if (combo == 2)
                {
                    ToggleComboUI();
                    foreach (GameObject pizza in levelManager.pizzasInLevel)
                    {
                        foreach (Slice sliceInPizza in pizza.GetComponentsInChildren<Slice>())
                        {
                            sliceInPizza.ChangeColor(true);
                        }
                    }
                }

                hasBounced = false;
            }

        }
 
    }

  

    private void OnCollisionEnter(Collision collision)
    {

        if (combo >= 2)
        {
            levelManager.step++;

            hasBounced = true;
             
            rb.velocity = Vector3.zero;
            rb.AddForce(bounceForce * Vector3.up, ForceMode.Impulse);

            foreach (GameObject pizza in levelManager.pizzasInLevel)
            {
                foreach (Slice slice in pizza.GetComponentsInChildren<Slice>())
                {
                    slice.ChangeColor(false);
                }
            }


            if (collision.transform.CompareTag(Utils.SLICE_TAG) || collision.transform.CompareTag(Utils.RED_SLICE_TAG))
            {

                collision.gameObject.GetComponentInParent<Pizza>().DestroyPizza();
                
                 
            }
            else if (collision.transform.TryGetComponent(out Wall wall))
            {    
                wall.DestroyWall();
                wall.MakeDisabled();
            }
            Invoke(nameof(ToggleComboUI), 1f);
        }
        else if (collision.transform.CompareTag(Utils.RED_SLICE_TAG) || collision.transform.CompareTag(Utils.WALL_TAG))
        {
            Time.timeScale = 0; 
            manager.GameOver();
        }
        else if (collision.transform.CompareTag(Utils.SLICE_TAG) && !hasBounced )
        {

            hasBounced = true; 
            rb.velocity = Vector3.zero;
            rb.AddForce(bounceForce * Vector3.up, ForceMode.Impulse); 
        }
        else if (collision.transform.CompareTag(Utils.FINISH_SLICE_TAG))
        { 
            manager.NextLevel();
        }
        combo = 0;
    }
 
    public void ToggleComboUI()
    {
        uiManager.comboText.gameObject.SetActive(combo >= 2);
        uiManager.comboUI.gameObject.SetActive(combo >= 2);
        uiManager.comboText.text = combo.ToString();


    }


}
