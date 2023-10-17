using System.Collections;
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

    private float _input = 0;
    private float _initialSpeed;
    private Vector2 _movementAxis = Vector2.zero;
    private Vector2 _defaultPosition = Vector2.zero;

    public int PlayerNumber => _playerNumber;
    // Start is called before the first frame update
    void Start()
    {
        _defaultPosition = transform.position;
        _initialSpeed = _speed;

        _screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        _paddleHeight = _spriteRenderer.bounds.size.y / 2;
        _paddleWidth = _spriteRenderer.bounds.size.x / 2;

        _movementAxis = _movementDirection == MovementDirection.Vertical ? Vector2.up : Vector2.right;
    }

    private void Update()
    {
        GetUserInput();
    }

    private void FixedUpdate()
    {
        MovePaddle();
    }

    /// <summary>
    /// Adjusts the paddle position within the screen bounds
    /// </summary>
    private void LateUpdate()
    {
        Vector2 clampedPosition = transform.position;
        if (_movementDirection == MovementDirection.Vertical)
            clampedPosition.y = Mathf.Clamp(transform.position.y, (_screenBounds.y * -1) + _paddleHeight, _screenBounds.y - _paddleHeight);
        else
            clampedPosition.x = Mathf.Clamp(transform.position.x, (_screenBounds.x * -1) + _paddleWidth, _screenBounds.x - _paddleWidth);
        transform.position = clampedPosition;
    }

    public void SetControl(Control control) => this._control = control;

    private void GetUserInput()
    {
        _input = 0;
        switch (_control)
        {
            case Control.WSKeys:
                if (Input.GetKey(KeyCode.W))
                    _input = 1;

                if (Input.GetKey(KeyCode.S))
                    _input = -1;

                break;
            case Control.Arrows:
                if (Input.GetKey(KeyCode.UpArrow))
                    _input = 1;

                if (Input.GetKey(KeyCode.DownArrow))
                    _input = -1;
                break;
            case Control.Gamepad1:
                if (Input.GetKey(KeyCode.D))
                    _input = 1;

                if (Input.GetKey(KeyCode.A))
                    _input = -1;
                break;
            case Control.Gamepad2:
                if (Input.GetKey(KeyCode.RightArrow))
                    _input = 1;

                if (Input.GetKey(KeyCode.LeftArrow))
                    _input = -1;
                break;
            case Control.AI:
                break;
            default:
                break;
        }
    }

    private void MovePaddle()
    {
        if (!ReferenceEquals(_rigidbody, null))
            _rigidbody.velocity = _movementAxis * _speed * _input;
    }

    public void GoToDefaultPosition()
    {
        transform.position = _defaultPosition;
    }

    #region POWER-UPS

    public IEnumerator Freeze()
    {
        _speed = 0;
        yield return new WaitForSeconds(2);
        _speed = _initialSpeed;
    }

    public IEnumerator Turbo()
    {
        _speed *= 2;
        yield return new WaitForSeconds(5);
        _speed = _initialSpeed;
    }

    #endregion
}
