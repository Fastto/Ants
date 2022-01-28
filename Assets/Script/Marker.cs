using UnityEngine;

public class Marker : MonoBehaviour
{
    public float Intensivity = 1;

    [SerializeField] public float BurningRatePerSec = .9f;

    [SerializeField] public float IntensivityLimit = .05f;

    private Collider2D _collider;

    public Vector3 position
    {
        get { return transform.position; }
    }

    private float LifeCounter = 0f;

    private SpriteRenderer _spriteRenderer;

    private bool _initialised = false;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _collider = GetComponentInChildren<Collider2D>();
    }

    private void Update()
    {
        LifeCounter += Time.deltaTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (LifeCounter > 1f)
        {
            LifeCounter = 0;
            Refresh();
        }

        if (Intensivity < IntensivityLimit)
        {
            Destroy(this.gameObject);
        }
    }

    public void Refresh()
    {
        Intensivity *= BurningRatePerSec;
        Color old = _spriteRenderer.color;
        _spriteRenderer.color = new Color(old.r, old.g, old.b, Intensivity);

        if (Intensivity < 1 && !_initialised)
        {
            _collider.enabled = true;
            _initialised = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 7)
        {
            if (CompareTag(other.tag))
            {
                if (other.GetComponentInChildren<Marker>().Intensivity > Intensivity)
                {
                    _collider.enabled = false;
                    BurningRatePerSec = .8f;
                    //Destroy(gameObject);
                }
            }
        }
    }
}