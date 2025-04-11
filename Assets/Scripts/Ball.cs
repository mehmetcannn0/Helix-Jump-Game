using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody rb;
    public int bounceForce;
    public GameManager manager;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI comboUI;   
    private bool hasBounced = false; 
    private bool inGap = false;
    public int combo = 0;


    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }
    private void Update()
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

        if (other.transform.CompareTag("gap") && !hasBounced)
        {
            inGap = true;
             
            StartCoroutine(EnableSliceCollisionAfterDelay(0.2f));

            combo++;
            comboText.text = combo.ToString();

            other.gameObject.GetComponentInParent<Pizza>().DestroyPizza(); 

            manager.score += combo>1?combo:0 + manager.currentLevel;
            manager.step ++;
            manager.UpdateSlider();

            if (combo == 2)
            { 
                ToggleComboUI();
                foreach (GameObject pizza in manager.pizzas)
                {
                    foreach (Slice slice in pizza.GetComponentsInChildren<Slice>())
                    {
                        slice.ChangeColor(true);
                    }
                }

            }

            hasBounced = false;
        }
    }
    IEnumerator EnableSliceCollisionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        
        inGap = false;


    }

    


    private void OnCollisionEnter(Collision collision)
    { 
        if (collision.transform.CompareTag("slice") && !hasBounced && !inGap)
        {
            hasBounced = true;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(bounceForce * Vector3.up, ForceMode.Impulse);

        } 
        if (collision.transform.CompareTag("finish"))
        {
            Debug.Log("bolum býttý");
            manager.NextLevel();


        }
       

        if (combo >= 2)
        {

            foreach (GameObject pizza in manager.pizzas)
            {
                foreach (Slice slice in pizza.GetComponentsInChildren<Slice>())
                {
                    slice.ChangeColor(false);
                }
            }

            if (collision.transform.CompareTag("slice") || collision.transform.CompareTag("redSlice"))
            {
                collision.gameObject.GetComponentInParent<Pizza>().DestroyPizza();
            }
            else if (collision.transform.CompareTag("wall"))
            {

                collision.gameObject.GetComponent<Wall>().DestroyWall();
                collision.gameObject.GetComponent<Wall>().MakeDisabled();
            } 
          

            hasBounced = true;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(bounceForce * Vector3.up, ForceMode.Impulse);

        }
        combo = 0;
        Invoke(nameof(ToggleComboUI),1f);
        if (collision.transform.CompareTag("redSlice"))
        {
            Time.timeScale = 0; 
            Debug.Log("Game Over");
            manager.GameOver();

        }

    }

    public void ToggleComboUI()
    {
        comboText.gameObject.SetActive(combo >=2);
        comboUI.gameObject.SetActive(combo >=2);
        comboText.text = combo.ToString();


    }


}
