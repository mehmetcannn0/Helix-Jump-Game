using TMPro;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody rb;
    public int bounceForce;
    public GameManager manager;
    private bool hasBounced = false;
    public int combo = 0;
    public TextMeshProUGUI comboText;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    private void FixedUpdate()
    {
        hasBounced = false;
    }
    // Update is called once per frame

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.CompareTag("slice") && !hasBounced)
        {
            hasBounced = true;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(bounceForce * Vector3.up, ForceMode.Impulse);

        }
        if (collision.transform.CompareTag("finish"))
        {
            Debug.Log("bolum býttý");
            PlayerPrefs.SetInt("CurrentLevel", manager.currentLevel + 1);

        }
        if (collision.transform.CompareTag("redSlice"))
        {
            Debug.Log("red slice");


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
            if (collision.transform.tag != "wall")
            {
                collision.gameObject.GetComponentInParent<Pizza>().DestroyPizza();
            }
            else
            {
                Destroy(collision.gameObject);
            }


        }
        combo = 0;
        comboText.text = combo.ToString();



    }
    private void OnCollisionExit(Collision collision)
    {
        //if (collision.gameObject.CompareTag("slice"))
        //{
        //    hasBounced = false;
        //}
        hasBounced = false;
    }
    private void OnTriggerEnter(Collider other)
    {



        if (other.transform.CompareTag("gap"))
        {
            combo++;
            comboText.text = combo.ToString();

            other.GetComponentInParent<Pizza>().DestroyPizza();
            manager.UpdateSlider();
            if (combo == 2)
            {
                Debug.Log("combo");
                foreach (GameObject pizza in manager.pizzas)
                {
                    foreach (Slice slice in pizza.GetComponentsInChildren<Slice>())
                    {
                        slice.ChangeColor(true);
                    }
                }

            }
        }
        hasBounced = false;

    }
}
