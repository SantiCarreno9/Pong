using UnityEngine;

public enum Control
{
    WSKeys,
    Arrows,
    Gamepad1,
    Gamepad2,
    AI
}

public enum MovementDirection
{
    Vertical,
    Horizontal
}

public class Paddle : MonoBehaviour
{
    [SerializeField]
    private Control _control;
    [SerializeField]
    private MovementDirection _movementDirection;
    [SerializeField]
    private Rigidbody2D _rigidbody = default;
    [SerializeField]
    private SpriteRenderer _spriteRenderer = default;

    [Space]
    [SerializeField]
    private int _playerNumber;
    private float _speed = 10;
    private float _paddleHeight = 0;
    private float _paddleWidth = 0;

    private Vector2 _screenBounds;
    // Start is called before the first frame update
    void Start()
    {
        _screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        _paddleHeight = _spriteRenderer.bounds.size.y / 2;
        _paddleWidth = _spriteRenderer.bounds.size.x / 2;
    }

    private void Update()
    {
        ControlMovement();
    }

    /// <summary>
    /// Adjusts the paddle position within the screen bounds
    /// </summary>
    private void LateUpdate()
    {
        Vector2 clampedPosition= transform.position;        
        if (_movementDirection == MovementDirection.Vertical)        
            clampedPosition.y= Mathf.Clamp(transform.position.y, (_screenBounds.y * -1) + _paddleHeight, _screenBounds.y - _paddleHeight);
        else
            clampedPosition.x = Mathf.Clamp(transform.position.x, (_screenBounds.x * -1) + _paddleWidth, _screenBounds.x - _paddleWidth);
        transform.position = clampedPosition;
    }

    public void SetControl(Control control) => this._control = control;

    private void ControlMovement()
    {
        int direction = 0;

        switch (_control)
        {
            case Control.WSKeys:
                if (Input.GetKey(KeyCode.W))
                    direction = 1;

                if (Input.GetKey(KeyCode.S))
                    direction = -1;
                
                break;
            case Control.Arrows:
                if (Input.GetKey(KeyCode.UpArrow))
                    direction = 1;

                if (Input.GetKey(KeyCode.DownArrow))
                    direction = -1;
                break;
            case Control.Gamepad1:
                if (Input.GetKey(KeyCode.D))
                    direction = 1;

                if (Input.GetKey(KeyCode.A))
                    direction = -1;
                break;
            case Control.Gamepad2:
                if (Input.GetKey(KeyCode.RightArrow))
                    direction = 1;

                if (Input.GetKey(KeyCode.LeftArrow))
                    direction = -1;
                break;
            case Control.AI:
                break;
            default:
                break;
        }

        Vector2 movementAxis = _movementDirection == MovementDirection.Vertical ? Vector2.up : Vector2.right;

        if (!ReferenceEquals(_rigidbody, null))
            _rigidbody.velocity = movementAxis * _speed * direction;
    }
}
