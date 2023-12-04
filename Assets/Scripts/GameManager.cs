using System.Collections;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField]
    private PaddleController[] _paddles = default;
    [SerializeField]
    private BallController _ballController = default;
    [SerializeField]
    private BoundaryWall[] _boundaries = default;
    [SerializeField]
    private PowerUpsManager _powerUpsManager = default;
    [SerializeField]
    private Transform _rangeBox = default;

    [SerializeField]
    private int _amountOfPlayers = 2;

    [SerializeField]
    private GameUIManager _gameUIManager = default;

    private int[] _scores = new int[4];

    private int _lastConcedingPlayer = -1;

    private float _timeToShootBall = 3f;    

    public BallController BallController => _ballController;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
    }


    #region MENU

    public void UpdateAmountOfPlayers(int playersAmount)
    {
        _amountOfPlayers = playersAmount;
        for (int i = 0; i < _paddles.Length; i++)
            _paddles[i].gameObject.SetActive(i < playersAmount);

        for (int i = 0; i < _boundaries.Length; i++)
        {
            if (i < _amountOfPlayers)
                _boundaries[i].TurnIntoScoreLine();
            else _boundaries[i].TurnIntoBoundary();
        }
    }

    #endregion

    #region GAMEPLAY

    public void StartGame()
    {
        SetUpNewGame();
        ResumeGame();
        Invoke("ShootBall", _timeToShootBall);
    }

    private void RestartPlayersPosition()
    {
        for (int i = 0; i < _amountOfPlayers; i++)
            _paddles[i].GoToDefaultPosition();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;        
        _powerUpsManager.StopSpawning();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;        
        _powerUpsManager.ResumeSpawning();
    }

    private IEnumerator EndGame(int winner)
    {
        yield return new WaitForSecondsRealtime(2);
        StartCoroutine(_gameUIManager.ShowWinner(winner));
        Time.timeScale = 0;
        SetUpNewGame();
    }

    private void SetUpNewGame()
    {
        _powerUpsManager.Reset();
        _ballController.MoveOutOfBounds();
        _ballController.Reset();
        _ballController.Hide();

        //Scores
        for (int i = 0; i < _scores.Length; i++)
            _scores[i] = 0;


        RestartPlayersPosition();
    }

    public void ReshootBall()
    {
        _ballController.MoveOutOfBounds();
        Invoke("ShootBall", _timeToShootBall);
        ResumeGame();
    }

    #endregion

    #region IN GAME ACTIONS

    public void Score(int concedingPlayer)
    {
        _ballController.Explode();
        _lastConcedingPlayer = concedingPlayer;

        int[] lastHitPlayers = _ballController.GetLastHitPlayers();
        int scoringPlayer = -1;
        if (_amountOfPlayers == 2)
            scoringPlayer = (concedingPlayer == 0) ? 1 : 0;
        else
            scoringPlayer = (lastHitPlayers[0] != concedingPlayer) ? lastHitPlayers[0] : lastHitPlayers[1];

        if (scoringPlayer != -1)
        {
            _scores[scoringPlayer]++;
            _gameUIManager.GivePointsToPlayer(scoringPlayer, _scores[scoringPlayer]);
            if (_scores[scoringPlayer] == 3)
            {
                StartCoroutine(EndGame(scoringPlayer));
                return;
            }
        }

        Invoke("ShootBall", _timeToShootBall);
    }

    public void ShootBall()
    {
        _ballController.MoveOutOfBounds();
        _ballController.Hide();
        _ballController.Reset();
        Vector2 ballPosition;
        Vector2 ballDirection;
        int defendingPlayer = _lastConcedingPlayer;
        if (_lastConcedingPlayer == -1)
            defendingPlayer = Random.Range(0, _amountOfPlayers);

        switch (defendingPlayer)
        {
            case 0:
                ballDirection = Vector2.left;
                break;
            case 1:
                ballDirection = Vector2.right;
                break;
            case 2:
                ballDirection = Vector2.up;
                break;
            case 3:
                ballDirection = Vector2.down;
                break;
            default:
                ballDirection = Vector2.zero;
                break;
        }

        if (_amountOfPlayers == 2)
        {
            float yHalfScale = _rangeBox.localScale.y / 2;
            Vector2 range = new Vector2(_rangeBox.position.y - yHalfScale, _rangeBox.position.y + yHalfScale);
            ballPosition = Vector2.up * (Random.Range(range.x, range.y));
        }
        else ballPosition = Vector2.zero;

        ballDirection = ballDirection + (Vector2.Perpendicular(ballDirection) * Random.Range(-1.0f, 1.0f));

        _ballController.Show();
        _ballController.Spawn(ballPosition, ballDirection);
    }

    public void ActivatePowerUp(PowerUps powerUp)
    {
        int pickedUpByPlayer = _ballController.GetLastHitPlayers()[0];
        switch (powerUp)
        {
            case PowerUps.Freeze:
                for (int i = 0; i < _amountOfPlayers; i++)
                {
                    if (i != pickedUpByPlayer)
                        StartCoroutine(_paddles[i].Freeze());
                }
                break;
            case PowerUps.Turbo:
                for (int i = 0; i < _amountOfPlayers; i++)
                {
                    if (i != pickedUpByPlayer)
                        StartCoroutine(_paddles[i].Turbo());
                }
                break;
            default:
                break;
        }

    }

    #endregion  
}
