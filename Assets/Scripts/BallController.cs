using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rigidbody;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private float _speed = 2.5f;

    private bool _isMoving = false;
    private int[] _lastHitPlayers = new int[2];

    public int[] GetLastHitPlayers()
    {
        return _lastHitPlayers;
    }

    private void Start()
    {
        Hide();
        for (int i = 0; i < _lastHitPlayers.Length; i++)
            _lastHitPlayers[i] = -1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            UpdateLastHitPlayer(collision.gameObject.GetComponent<PaddleController>().PlayerNumber);
        }
    }

    private void UpdateLastHitPlayer(int playerNumber)
    {
        if (_lastHitPlayers[0] == -1)
        {
            _lastHitPlayers[0] = playerNumber;
            return;
        }

        if (_lastHitPlayers[0] != playerNumber)
        {
            _lastHitPlayers[1] = _lastHitPlayers[0];
            _lastHitPlayers[0] = playerNumber;
        }
    }

    private void FixedUpdate()
    {
        if (!_isMoving)
            return;

        Vector2 normalized = _rigidbody.velocity.normalized;
        _rigidbody.velocity = normalized * _speed;
    }

    public void Show()
    {
        _spriteRenderer.enabled = true;
    }

    public void Hide()
    {
        _spriteRenderer.enabled = false;
    }

    /// <summary>
    /// Spawns the ball in a random position along the axi
    /// </summary>
    /// <param name="range"></param>
    /// <param name="direction"></param>
    public void Spawn(Vector2 position, Vector2 direction)
    {
        _rigidbody.velocity = Vector2.zero;
        transform.position = position;
        _rigidbody.AddForce(direction * _speed, ForceMode2D.Impulse);
        _isMoving = true;
    }

    public void Reset()
    {
        transform.position = Vector2.zero;
        _isMoving = false;
        for (int i = 0; i < _lastHitPlayers.Length; i++)
            _lastHitPlayers[i] = -1;
    }

    public void Explode()
    {
        _rigidbody.velocity = Vector2.zero;
        _animator.Play("Explosion");
    }

    public void MoveOutOfBounds()
    {
        transform.position = new Vector2(100, 100);
    }


}
