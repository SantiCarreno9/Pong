using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rigidbody;
    [SerializeField]
    private GameManager _gameManager;

    [SerializeField]
    private float _speed = 1.0f;

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
    }
}
