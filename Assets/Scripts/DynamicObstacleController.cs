using UnityEngine;

public class DynamicObstacleController : MonoBehaviour
{
    [SerializeField]
    private Vector2 _movementRange = Vector2.zero;
    [SerializeField]
    private MovementDirection _axis = MovementDirection.Vertical;
    [SerializeField]
    private float _speed = 2f;

    [SerializeField]
    private Vector2 _direction = Vector2.zero;

    private Rigidbody2D _rigidbody;
    

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();    
    }

    private void FixedUpdate()
    {
        if (_axis == MovementDirection.Vertical)
        {            
            if (transform.localPosition.y <= _movementRange.x)
                _direction = Vector2.up;

            if (transform.localPosition.y >= _movementRange.y)
                _direction = Vector2.down;
        }
        else
        {
            if (transform.localPosition.x <= _movementRange.x)
                _direction = Vector2.right;

            if (transform.localPosition.x >= _movementRange.y)
                _direction = Vector2.left;
        }

        _rigidbody.velocity = _direction * _speed;
    }
}
