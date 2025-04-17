using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private bool hasBounced;
    private Vector3 ballStartPosition = new Vector3(0, 2f, -1.7f);
    private Rigidbody rb;
    private int combo;

    GameManager gameManager;
    LevelManager levelManager;
    UIManager uiManager;
    [SerializeField] int bounceForce;

    readonly int MIN_COMBO_COUNT = 2;
    readonly float COMBO_DELAY_TIME = 1f;

    void Start()
    {
        gameManager = GameManager.Instance;
        levelManager = LevelManager.Instance;
        uiManager = UIManager.Instance;
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
        if (other.TryGetComponent(out IDestroyable destroyable))
        {
            if (destroyable.objectType == DestroyableType.Gap && !hasBounced)
            {
                OnGapInteracted(destroyable);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IDestroyable destroyableObject))
        {
            if (destroyableObject.objectType == DestroyableType.FinishSlice)
            {
                OnFinishSliceInteracted();
                return;
            }

            if (combo >= MIN_COMBO_COUNT)
            {
                OnComboInteraction(destroyableObject);
                return;
            }

            if (destroyableObject.objectType == DestroyableType.RedSlice || destroyableObject.objectType == DestroyableType.Wall)
            {
                OnObstacleInteracted();
            }
            else if (destroyableObject.objectType == DestroyableType.DefaultSlice && !hasBounced)
            {
                OnDefaultSliceInteracted();
            }

            //destroyableObject.OnInteracted(this);

            OnComboDeactivated();
        }
    }

    private void OnGapInteracted(IDestroyable destroyable)
    {
        combo++;
        uiManager.comboText.text = combo.ToString();
        destroyable.DestroyObject();
        levelManager.IncreaseScore(combo > 0 ? combo + levelManager.currentLevel : levelManager.currentLevel);
        levelManager.IncreaseStep();
        uiManager.UpdateSlider();

        CheckComboActivation();

        hasBounced = false;
    }

    private void CheckComboActivation()
    {
        if (combo == MIN_COMBO_COUNT)
            GameActions.OnComboActivated?.Invoke(combo);
    }

    private void OnFinishSliceInteracted()
    {
        levelManager.IncreaseScore(combo > 0 ? combo + levelManager.currentLevel : levelManager.currentLevel);
        levelManager.IncreaseStep();
        gameManager.NextLevel();

        //if (combo >= MIN_COMBO_COUNT)
        //{
        //    //Invoke(nameof(ToggleComboUI), COMBO_DELAY_TIME);
        //}
        OnComboDeactivated();
    }

    private void OnComboInteraction(IDestroyable destroyable)
    {
        levelManager.IncreaseStep();
        hasBounced = true;
        rb.velocity = Vector3.zero;
        rb.AddForce(bounceForce * Vector3.up, ForceMode.Impulse);
        destroyable.DestroyObject();
        OnComboDeactivated();
        //Invoke(nameof(ToggleComboUI),COMBO_DELAY_TIME);
    }

    private void OnObstacleInteracted()
    {
        Time.timeScale = 0;
        gameManager.GameOver();
    }

    public void OnDefaultSliceInteracted()
    {
        hasBounced = true;
        rb.velocity = Vector3.zero;
        rb.AddForce(bounceForce * Vector3.up, ForceMode.Impulse);
    }



    private void OnComboDeactivated()
    {
        combo = 0;
        GameActions.OnComboDeactivated?.Invoke();
    }
}

public static partial class GameActions
{
    public static Action<int> OnComboActivated;
    public static Action OnComboDeactivated;
}
