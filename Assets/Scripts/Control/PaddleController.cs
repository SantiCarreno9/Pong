using System;
using System.Collections;
using UnityEngine;

public enum Control
{
    AI,
    WSKeys,
    Arrows,
    Gamepad1,
    Gamepad2
}

public enum MovementDirection
{
    Vertical,
    Horizontal
}

public class PaddleController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Control _control;
    [SerializeField]
    private MovementDirection _movementDirection;
    [SerializeField]
    private Rigidbody2D _rigidbody = default;
    [SerializeField]
    private SpriteRenderer _spriteRenderer = default;
    [SerializeField]
    private Transform _rangeBox = default;
    [SerializeField]
    private GameObject _aiTriggerZone = default;
    [SerializeField]
    private SpaceshipVisualController _spaceshipVisualController = default;

    public bool EnableAIMovement { get; set; } = false;

    [Space]
    [SerializeField]
    private int _playerNumber;

    private Control _originalControl;
    private float _speed = 10;
    private float _paddleHeight = 0;
    private float _paddleWidth = 0;

    private float _aiMovementFrequency = 0;
    private float _aiTime = 0;

    private Vector2 _movementBounds;

    private float _input = 0;
    private float _initialSpeed;
    private Vector2 _movementAxis = Vector2.zero;
    private Vector2 _defaultPosition = Vector2.zero;

    public int PlayerNumber => _playerNumber;
    public MovementDirection Direction => _movementDirection;

    // Start is called before the first frame update
    void Start()
    {
        _originalControl = _control;
        _defaultPosition = transform.position;
        _initialSpeed = _speed;

        if (_movementDirection == MovementDirection.Vertical)
        {
            float yHalfScale = _rangeBox.localScale.y / 2;
            _movementBounds = new Vector2(_rangeBox.position.y - yHalfScale, _rangeBox.position.y + yHalfScale);
        }
        else
        {
            float xHalfScale = _rangeBox.localScale.x / 2;
            _movementBounds = new Vector2(_rangeBox.position.x - xHalfScale, _rangeBox.position.x + xHalfScale);
        }
        _paddleHeight = transform.localScale.y / 2;
        _paddleWidth = transform.localScale.x / 2;

        _movementAxis = _movementDirection == MovementDirection.Vertical ? Vector2.up : Vector2.right;
    }

    private void OnDisable()
    {
        _aiTriggerZone.SetActive(false);
    }

    private void Update()
    {
        if (_control == Control.AI && EnableAIMovement)
        {
            GetAIInput();
            return;
        }

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
            clampedPosition.y = Mathf.Clamp(transform.position.y, _movementBounds.x + _paddleHeight, _movementBounds.y - _paddleHeight);
        else
            clampedPosition.x = Mathf.Clamp(transform.position.x, _movementBounds.x + _paddleWidth, _movementBounds.y - _paddleWidth);
        transform.position = clampedPosition;
    }

    #region CONTROL

    public void SetControl(int control)
    {
        if (control == 1)
            _control = Control.AI;
        else _control = _originalControl;

        _aiTriggerZone.SetActive(_control == Control.AI);
    }

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
                float inputJoy1 = 0;
                if (_movementDirection == MovementDirection.Horizontal)
                    inputJoy1 = Input.GetAxis("HorizontalJoy1");
                //else 
                if (inputJoy1 > 0)
                    _input = 1;

                if (inputJoy1 < 0)
                    _input = -1;
                break;
            case Control.Gamepad2:
                float horizontalJoy2 = Input.GetAxis("HorizontalJoy2");
                if (horizontalJoy2 > 0)
                    _input = 1;

                if (horizontalJoy2 < 0)
                    _input = -1;
                break;
        }
    }

    private void GetAIInput()
    {
        if (_aiMovementFrequency == 0)
            _aiMovementFrequency = UnityEngine.Random.Range(0f, 0.4f);
        _aiTime += Time.deltaTime;
        if (_aiTime < _aiMovementFrequency)
            return;

        float distance = 0;
        _input = 0;
        if (_movementDirection == MovementDirection.Vertical)
        {
            float ballYPosition = GameManager.Instance.BallController.transform.position.y;
            distance = ballYPosition - transform.position.y;
        }
        else
        {
            float ballxPosition = GameManager.Instance.BallController.transform.position.x;
            distance = ballxPosition - transform.position.x;
        }

        if (distance > _paddleHeight) _input = 1;
        if (distance < -_paddleHeight) _input = -1;

        _aiTime = 0;
        _aiMovementFrequency = 0;
    }

    private void MovePaddle()
    {
        _rigidbody.velocity = _movementAxis * _speed * _input;
    }

    public void GoToDefaultPosition()
    {
        transform.position = _defaultPosition;
    }

    #endregion

    #region POWER-UPS

    public IEnumerator Freeze()
    {
        _speed = 0;
        _spaceshipVisualController.Freeze();
        yield return new WaitForSeconds(2);
        _spaceshipVisualController.SetNormalState();
        _speed = _initialSpeed;
    }

    public IEnumerator Turbo()
    {        
        _speed *= 2;
        _spaceshipVisualController.SetTurbo();
        yield return new WaitForSeconds(5);
        _spaceshipVisualController.SetNormalSpeed();
        _speed = _initialSpeed;
    }

    #endregion

}
