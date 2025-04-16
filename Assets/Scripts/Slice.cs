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


    public float defaultRotationY;

    [SerializeField] Rigidbody rb;



    public ObjectType objectType { get; set; } = ObjectType.Slice;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        //meshCollider = gameObject.GetComponent<MeshCollider>();
        boxCollider = rb.GetComponent<BoxCollider>();
        objectRenderer = gameObject.GetComponent<Renderer>();

    }
    private void Start()
    {
        defaultRotationY = transform.rotation.eulerAngles.y;
    }

    public void SetRedSlice()
    {
        objectType = ObjectType.RedSlice;
        objectRenderer.material = redMaterial;
        //gameObject.tag = Utils.RED_SLICE_TAG;
    }
    public void SetGap()
    {
        objectType = ObjectType.Gap;
        //meshCollider.convex = true;
        //meshCollider.isTrigger = true;
        boxCollider.isTrigger = true;
        //gameObject.tag = Utils.GAP_SLICE_TAG;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        //gameObject.GetComponent<MeshRenderer>().material = greenMaterial;   
        gameObject.transform.localPosition = gapPosition;
    }
    public void SetFinishSlice()
    {
        objectType = ObjectType.FinishSlice;
    }

    public void ChangeColor(bool combo)
    {
        if (combo)
        {
            objectRenderer.material = greenMaterial;

        }
        else
        {
            if (objectType == ObjectType.RedSlice)
            {
                objectRenderer.material = redMaterial;
            }
            else
            {

                objectRenderer.material = whiteMaterial;
            }

        }

    }

    public void DestroyObject()
    {
        gameObject.GetComponentInParent<Pizza>().DestroyPizza();
    }
    public void DestroySlice()
    {
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f)).normalized;
        float randomForce = Random.Range(5, 10);
        //meshCollider.enabled = false;
        boxCollider.enabled = false;
        rb.isKinematic = false;
        rb.AddForce(randomDirection * randomForce, ForceMode.Impulse);
        //  Invoke(nameof(SetDefaultValues), 2f);
    }
    private void SetDefaultValues()
    {
        objectType = ObjectType.Slice;
        rb.isKinematic = true;
        boxCollider.enabled = true;
        boxCollider.isTrigger = false;
        objectRenderer.material = whiteMaterial;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        rb.velocity = Vector3.zero;
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(0, defaultRotationY, 0));

    }


}
