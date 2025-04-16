using UnityEngine;

public class Ball : MonoBehaviour
{
    private bool hasBounced;
    private Vector3 ballStartPosition = new Vector3(0, 2f, -1.7f);
    private Rigidbody rb;
    private int combo;

    [SerializeField] GameManager gameManager;
    [SerializeField] LevelManager levelManager;
    [SerializeField] UIManager uiManager;
    [SerializeField] int bounceForce;

    readonly int MIN_COMBO_COUNT = 2;


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

        if (other.TryGetComponent(out IDestroyable idestroyable))
        {

            if (idestroyable.objectType == ObjectType.Gap && !hasBounced)
            {
                combo++;
                uiManager.comboText.text = combo.ToString();

                //other.gameObject.GetComponentInParent<Pizza>().DestroyPizza();
                idestroyable.DestroyObject();

                levelManager.IncreaseScore(combo > 1 ? combo : 0 + levelManager.currentLevel);
                levelManager.IncreaseStep();
                uiManager.UpdateSlider();

                if (combo == MIN_COMBO_COUNT)
                {
                    ToggleComboUI();
                    uiManager.MakeGreenSlice(true);
                }

                hasBounced = false;
            }

        }

    }

    private void OnCollisionEnter(Collision collision)
    {


        if (collision.gameObject.TryGetComponent(out IDestroyable idestroyable))
        {
            if (idestroyable.objectType == ObjectType.FinishSlice)
            {
                levelManager.IncreaseScore(combo > 1 ? combo : 0 + levelManager.currentLevel);
                levelManager.IncreaseStep();
                gameManager.NextLevel();
                if (combo >= MIN_COMBO_COUNT)
                {
                    Invoke(nameof(ToggleComboUI), 1f);
                    combo = 0;
                }
            }
            else if (combo >= MIN_COMBO_COUNT)
            {
                levelManager.IncreaseStep();
                hasBounced = true;
                rb.velocity = Vector3.zero;
                rb.AddForce(bounceForce * Vector3.up, ForceMode.Impulse);
                uiManager.MakeGreenSlice(false);
                idestroyable.DestroyObject();
                Invoke(nameof(ToggleComboUI), 1f);
            }
            else if (idestroyable.objectType == ObjectType.RedSlice || idestroyable.objectType == ObjectType.Wall)
            {
                Time.timeScale = 0;

                gameManager.GameOver();
            }
            else if (idestroyable.objectType == ObjectType.Slice && !hasBounced)
            {

                hasBounced = true;
                rb.velocity = Vector3.zero;
                rb.AddForce(bounceForce * Vector3.up, ForceMode.Impulse);
            }

            combo = 0;
        }


    }



    //??
    public void ToggleComboUI()
    {
        uiManager.comboText.gameObject.SetActive(combo >= MIN_COMBO_COUNT);
        uiManager.comboUI.gameObject.SetActive(combo >= MIN_COMBO_COUNT);
        uiManager.comboText.text = combo.ToString();


    }




}
