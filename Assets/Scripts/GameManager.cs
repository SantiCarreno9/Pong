using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private GameObject _menuScreen = default;
    [SerializeField]
    private GameObject _startScreen = default;
    [SerializeField]
    private GameObject _inGameScreen = default;
    [SerializeField]
    private GameObject _endScreen = default;

    [Space]
    [Header("In Game Components")]
    [SerializeField]
    private GameObject _gameParent = default;
    [SerializeField]
    private BallController _ballController = default;
    [SerializeField]
    private TMP_Text[] _scoreTexts = default;

    private Vector2 _screenBounds;

    [SerializeField]
    private int _amountOfPlayers = 2;

    private int[] _scores;

    private int _lastConcedingPlayer = -1;
    // Start is called before the first frame update
    void Start()
    {
        _screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        _scores = new int[_amountOfPlayers];
        for (int i = 0; i < _amountOfPlayers; i++)
            _scores[i] = 0;

        SetGameLayout();

        StartCoroutine(ShootBall());
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Score(int concedingPlayer)
    {
        _lastConcedingPlayer = concedingPlayer;
        if (ReferenceEquals(_scoreTexts, null) || _scoreTexts.Length < 2)
            return;

        int playerScored = 0;
        if (_amountOfPlayers == 2)
            playerScored = (concedingPlayer == 0) ? 1 : 0;

        _scores[playerScored]++;
        _scoreTexts[playerScored].text = _scores[playerScored].ToString();

        StartCoroutine(ShootBall());
    }

    private IEnumerator ShootBall()
    {
        yield return new WaitForSeconds(2);
        Vector2 ballPositionRange;
        Vector2 ballDirection;
        int defendingPlayer = _lastConcedingPlayer;
        if (_lastConcedingPlayer == -1)
            defendingPlayer = Random.Range(0, _amountOfPlayers);

        if (_amountOfPlayers == 2)
            ballPositionRange = new Vector2(_screenBounds.y * -1 * 0.8f, _screenBounds.y * 0.8f);
        else ballPositionRange = Vector2.zero;

        switch (defendingPlayer)
        {
            case 0:
                ballDirection = Vector2.left;                
                //ballPositionRange = new Vector2(_screenBounds.y * -1 * 0.8f, _screenBounds.y * 0.8f);                
                break;
            case 1:
                ballDirection = Vector2.right;
                //ballPositionRange = new Vector2(_screenBounds.y * -1 * 0.8f, _screenBounds.y * 0.8f);
                break;
            case 2:
                ballDirection = Vector2.up;
                //ballPositionRange = new Vector2(_screenBounds.x * -1 * 0.8f, _screenBounds.x * 0.8f);
                break;
            case 3:
                ballDirection = Vector2.down;
                //ballPositionRange = new Vector2(_screenBounds.x * -1 * 0.8f, _screenBounds.x * 0.8f);
                break;
            default:
                ballDirection = Vector2.zero;
                ballPositionRange = Vector2.zero;
                break;
        }

        _ballController.Spawn(ballPositionRange, ballDirection);
    }

    private void SetGameLayout()
    {
        //if (_paddles.Length < 2)
        //    return;

        ////Left Paddle
        //int leftPaddleDirection=0;

        //if (Input.GetKey(KeyCode.W))
        //    leftPaddleDirection = 1;

        //if (Input.GetKey(KeyCode.S))
        //    leftPaddleDirection = -1;

        //if (!ReferenceEquals(_paddles[0], null))
        //    _paddles[0].Move(leftPaddleDirection);

        ////Right Paddle
        //int rightPaddleDirection = 0;

        //if (Input.GetKey(KeyCode.UpArrow))
        //    rightPaddleDirection = 1;

        //if (Input.GetKey(KeyCode.DownArrow))
        //    rightPaddleDirection = -1;


        //if (!ReferenceEquals(_paddles[1], null))
        //    _paddles[1].Move(rightPaddleDirection);
    }
}
