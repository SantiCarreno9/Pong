using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    [Header("Menu")]
    [SerializeField]
    private GameObject _menuScreen = default;
    [SerializeField]
    private TMP_Dropdown _playersDropdown = default;    


    [Space]
    [SerializeField]
    private GameObject _inGameScreen = default;
    [SerializeField]
    private GameObject _pauseScreen = default;

    [Header("End Screen")]
    [SerializeField]
    private GameObject _endScreen = default;
    [SerializeField]
    private TMP_Text _endScreenText = default;

    [Space]
    [Header("In Game Components")]
    [SerializeField]
    private GameObject _gameParent = default;
    [SerializeField]
    private Paddle[] _paddles = default;
    [SerializeField]
    private BallController _ballController = default;
    [SerializeField]
    private TMP_Text[] _scoreTexts = default;


    private Vector2 _screenBounds;

    [SerializeField]
    private int _amountOfPlayers = 2;

    private int[] _scores;

    private int _lastConcedingPlayer = -1;

    private bool _gamePaused = false;
    // Start is called before the first frame update
    void Start()
    {
        _screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        if (!ReferenceEquals(_gameParent, null))
            _gameParent.SetActive(false);

        _scores = new int[_amountOfPlayers];
        for (int i = 0; i < _amountOfPlayers; i++)
            _scores[i] = 0;

        SetGameLayout();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (!_gamePaused) PauseGame();
            else ResumeGame();
        }

    }

    #region GAMEPLAY

    public void StartGame()
    {
        HideMainMenu();
        ShowInGameScreen();
        StartCoroutine(ShootBall());
    }

    private void RestartPlayersPosition()
    {
        for (int i = 0; i < _amountOfPlayers; i++)
            _paddles[i].GoToDefaultPosition();
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        _gamePaused = true;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
        _gamePaused = true;
    }

    private IEnumerator EndGame(int winner)
    {
        SetUpNewGame();
        HideInGameScreen();
        ShowEndGameScreen(winner);
        yield return new WaitForSeconds(3);
        HideEndGameScreen();
        ShowMainMenu();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void SetUpNewGame()
    {
        _ballController.Reset();
        _ballController.Hide();
        for (int i = 0; i < _scoreTexts.Length; i++)
            _scoreTexts[i].text = "0";
        for (int i = 0; i < _scores.Length; i++)
            _scores[i] = 0;
        RestartPlayersPosition();
    }

    #endregion

    #region ACTIONS

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

        if (_scores[playerScored] == 3)
        {
            StartCoroutine(EndGame(playerScored + 1));
            return;
        }

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
                ballPositionRange = Vector2.zero;
                break;
        }

        _ballController.Show();
        _ballController.Spawn(ballPositionRange, ballDirection);
    }

    public void ActivatePowerUp(PowerUps powerUp)
    {
        Debug.Log("Player " + _ballController.GetLastHitPlayer() + " picked " + powerUp.ToString() + " power-up");
        int pickedUpByPlayer=_ballController.GetLastHitPlayer();
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

    #region SCREEN

    private void ShowMainMenu()
    {
        if (!ReferenceEquals(_menuScreen, null))
            _menuScreen.SetActive(true);
    }

    private void HideMainMenu()
    {
        if (!ReferenceEquals(_menuScreen, null))
            _menuScreen.SetActive(false);
    }

    private void ShowInGameScreen()
    {
        if (!ReferenceEquals(_inGameScreen, null))
            _inGameScreen.SetActive(true);
        if (!ReferenceEquals(_gameParent, null))
            _gameParent.SetActive(true);
    }

    private void HideInGameScreen()
    {
        if (!ReferenceEquals(_inGameScreen, null))
            _inGameScreen.SetActive(false);
        if (!ReferenceEquals(_gameParent, null))
            _gameParent.SetActive(false);
    }

    private void OpenPauseMenu()
    {
        if (!ReferenceEquals(_pauseScreen, null))
            _pauseScreen.SetActive(true);
    }

    private void ClosePauseMenu()
    {
        if (!ReferenceEquals(_pauseScreen, null))
            _pauseScreen.SetActive(false);
    }

    private void ShowEndGameScreen(int winner)
    {
        if (!ReferenceEquals(_endScreen, null))
            _endScreen.SetActive(true);
        if (!ReferenceEquals(_endScreenText, null))
            _endScreenText.text = "Player " + winner + " wins!";
    }

    private void HideEndGameScreen()
    {
        if (!ReferenceEquals(_endScreen, null))
            _endScreen.SetActive(false);
    }

    #endregion


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
