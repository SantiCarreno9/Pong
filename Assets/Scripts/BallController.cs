using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rigidbody;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private GameManager _gameManager;

    [SerializeField]
    private float _speed = 2.5f;

    private bool _isMoving = false;
    private int _lastHitPlayer = -1;

    public int GetLastHitPlayer()
    {
        return _lastHitPlayer;
    }

    private void Start()
    {
        Hide();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _lastHitPlayer = collision.gameObject.GetComponent<Paddle>().PlayerNumber;
        }
    }

    private void FixedUpdate()
    {
        if (!_isMoving)
            return;

        Vector2 normalized = _rigidbody.velocity.normalized;
        _rigidbody.velocity = normalized * _speed;
    }

    /// <summary>
    /// Spawns the ball in a random position along the axi
    /// </summary>
    /// <param name="range"></param>
    /// <param name="direction"></param>
    public void Spawn(Vector2 range, Vector2 direction)
    {
        if (ReferenceEquals(_rigidbody, null))
            return;

        _rigidbody.velocity = Vector2.zero;

        Debug.Log(Vector2.Perpendicular(direction));

        float randomPosition = Random.Range(range.x, range.y);
        transform.position = Vector2.Perpendicular(direction) * randomPosition;

        Vector2 forceVector = direction + (Vector2.Perpendicular(direction) * Random.Range(-1.0f, 1.0f));
        _rigidbody.AddForce(forceVector * _speed, ForceMode2D.Impulse);
        _isMoving = true;
    }

    public void Reset()
    {
        transform.position = Vector2.zero;
        _isMoving = false;
        _lastHitPlayer = -1;
    }    

    public void Hide()
    {
        if (!ReferenceEquals(_spriteRenderer, null))
            _spriteRenderer.enabled = false;
    }

    public void Show()
    {
        if (!ReferenceEquals(_spriteRenderer, null))
            _spriteRenderer.enabled = true;
    }
}
