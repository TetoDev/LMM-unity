using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] public float lifetime;
    private float direction;
    private bool hit;

    private Animator anim;
    private BoxCollider2D boxcollider;  

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxcollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed,0,0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        boxcollider.enabled = false;
        anim.SetTrigger("explode");
    }

    public void SetDirection(float _direction)
    {
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxcollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if(Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
