using UnityEngine;

public class Slice : MonoBehaviour, IDestroyable, IColorChangeable
{
    private Vector3 gapPosition = new Vector3(0, -0.3f, 0);
    [SerializeField] Material greenMaterial;
    [SerializeField] Material redMaterial;
    [SerializeField] Material whiteMaterial;
    [SerializeField] MeshCollider meshCollider;
    [SerializeField] BoxCollider boxCollider;
    [SerializeField] Renderer objectRenderer;
    [SerializeField] Rigidbody rb;

    public float defaultRotationY;

    public DestroyableType objectType { get; set; } = DestroyableType.DefaultSlice;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>(); 
        boxCollider = rb.GetComponent<BoxCollider>();
        objectRenderer = gameObject.GetComponent<Renderer>();

    }
    private void Start()
    {
        defaultRotationY = transform.rotation.eulerAngles.y;
    }

    public void SetRedSlice()
    {
        objectType = DestroyableType.RedSlice;
        objectRenderer.material = redMaterial; 
    }
    public void SetGap()
    { 
        objectType = DestroyableType.Gap; 
        boxCollider.isTrigger = true; 
        gameObject.GetComponent<MeshRenderer>().enabled = false;   
        gameObject.transform.localPosition = gapPosition;
    }
    public void SetFinishSlice()
    {
        objectType = DestroyableType.FinishSlice;
    }

    public void ChangeColor(bool combo)
    {
        if (combo)
        {
            objectRenderer.material = greenMaterial;
            return;
        }
        objectRenderer.material = objectType == DestroyableType.RedSlice ? redMaterial : whiteMaterial;
    }

    public void DestroyObject()
    {
        gameObject.GetComponentInParent<Pizza>().DestroyPizza();
    }
    public void DestroySlice()
    {
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f)).normalized;
        float randomForce = Random.Range(5, 10);
        boxCollider.enabled = false;
        rb.isKinematic = false;
        rb.AddForce(randomDirection * randomForce, ForceMode.Impulse);
        //Invoke(nameof(SetDefaultValues), 2f);
    }
    private void SetDefaultValues()
    {
        objectType = DestroyableType.DefaultSlice;
        rb.isKinematic = true;
        boxCollider.enabled = true;
        boxCollider.isTrigger = false;
        objectRenderer.material = whiteMaterial;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        rb.velocity = Vector3.zero;
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(0, defaultRotationY, 0));

    }

    //public void OnInteracted(Ball ball)
    //{
    //    switch (objectType)
    //    {
    //        case DestroyableType.Slice:
    //            ball.OnDefaultSliceInteracted();
    //            break;
    //        case DestroyableType.Gap:
    //            ball.On
    //            break;
    //        case DestroyableType.RedSlice:
    //            break;
    //        case DestroyableType.FinishSlice:
    //            break;
    //        case DestroyableType.Wall:
    //            break;
    //        default:
    //            break;
    //    }
    //}
}
