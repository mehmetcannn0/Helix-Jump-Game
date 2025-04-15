using UnityEngine;

public class Ball : MonoBehaviour
{
    private bool hasBounced ;
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

        if (other.TryGetComponent(out Slice slice))
        {

            if (slice.sliceType == SliceType.Gap && !hasBounced)
            {
                combo++;
                uiManager.comboText.text = combo.ToString();

                other.gameObject.GetComponentInParent<Pizza>().DestroyPizza();

                levelManager.IncreaseScore( combo > 1 ? combo : 0 + levelManager.currentLevel);
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

        if (combo >= MIN_COMBO_COUNT)
        {
            levelManager.IncreaseStep();

            hasBounced = true;

            rb.velocity = Vector3.zero;
            rb.AddForce(bounceForce * Vector3.up, ForceMode.Impulse);

            uiManager.MakeGreenSlice(false);
            /*
              if (collision.gameObject.TryGetComponent(out Slice slice) || collision.gameObject.TryGetComponent(out Wall wall))
            {
                if (slice.sliceType == SliceType.Slice || slice.sliceType == SliceType.Redslice)
                {
                    collision.gameObject.GetComponentInParent<Pizza>().DestroyPizza();
                }
                else if (wall != null)
                {
                    wall.DestroyWall();
                    wall.MakeDisabled();
                }

            }
             */


            if (collision.gameObject.TryGetComponent(out Slice slice))
            {
                if (slice.sliceType == SliceType.Slice || slice.sliceType == SliceType.Redslice)
                {
                    collision.gameObject.GetComponentInParent<Pizza>().DestroyPizza();
                }

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

            gameManager.GameOver();
        }
        else if (collision.transform.CompareTag(Utils.SLICE_TAG) && !hasBounced)
        {

            hasBounced = true;
            rb.velocity = Vector3.zero;
            rb.AddForce(bounceForce * Vector3.up, ForceMode.Impulse);
        }
        else if (collision.transform.CompareTag(Utils.FINISH_SLICE_TAG))
        {
            gameManager.NextLevel();
        }
        combo = 0;
    }
    //??
    public void ToggleComboUI()
    {
        uiManager.comboText.gameObject.SetActive(combo >= MIN_COMBO_COUNT);
        uiManager.comboUI.gameObject.SetActive(combo >= MIN_COMBO_COUNT);
        uiManager.comboText.text = combo.ToString();


    }




}
