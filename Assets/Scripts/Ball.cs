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
        CheckBelowSides();
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
        
        hasBounced = false;
    }

     

    void CheckBelowSides()
    {
        Vector3 leftOrigin = transform.position + new Vector3(-0.2f, 0f, 0f); // topun solundan
        Vector3 rightOrigin = transform.position + new Vector3(0.2f, 0f, 0f); // topun saðýndan

        RaycastHit leftHit;
        RaycastHit rightHit;

        bool leftRay = Physics.Raycast(leftOrigin, Vector3.down, out leftHit, 0.2f);
        bool rightRay = Physics.Raycast(rightOrigin, Vector3.down, out rightHit, 0.2f);

         
            Debug.DrawRay(leftOrigin, Vector3.down * 0.2f, Color.red);

         
            Debug.DrawRay(rightOrigin, Vector3.down * 0.2f, Color.blue);

        if (leftRay && leftHit.collider.CompareTag("gap") && rightRay && rightHit.collider.CompareTag("gap"))
        {
            Debug.Log("Gap altýnda, düþ!");
            // düþme veya zýplamama davranýþý
             
                combo++;
                comboText.text = combo.ToString();

                leftHit.collider.GetComponentInParent<Pizza>().DestroyPizza();
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
           
            hasBounced = false;
        }
        else if (leftRay || rightRay)
        {
            Debug.Log("Slice altýnda, zýpla!");
            // zýplama davranýþý
        }
    }
}
