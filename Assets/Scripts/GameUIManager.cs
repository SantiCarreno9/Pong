using System.Collections;
using TMPro;
using UnityEngine;

public enum GameScreen
{
    None,
    MainMenu,
    InGame,
    Pause,
    End
}

public class GameUIManager : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField]
    private GameObject _mainMenuScreen = default;
    [SerializeField]
    private TMP_Dropdown _playersDropdown = default;
    [SerializeField]
    private GameObject[] _playerSelectionItems = default;

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
    private TMP_Text[] _scoreTexts = default;

    [Space]
    [SerializeField]
    private GameSoundEffects _soundEffects = default;

    private bool _isGamePaused = false;

    private void Start()
    {
        UpdateAmountOfPlayers(_playersDropdown.value);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (!_isGamePaused) PauseGame();
            else ResumeGame();
        }
    }

    #region SCREEN

    private void ShowScreen(GameScreen screen)
    {
        _mainMenuScreen.SetActive(screen == GameScreen.MainMenu);

        _inGameScreen.SetActive(screen == GameScreen.InGame);

        _pauseScreen.SetActive(screen == GameScreen.Pause);
        _endScreen.SetActive(screen == GameScreen.End);
    }

    public IEnumerator ShowWinner(int winnerNumber)
    {
        _soundEffects.PlayWinnerSound();
        ShowScreen(GameScreen.End);
        ResetScores();
        _endScreenText.text = "Player " + (winnerNumber + 1) + " has won!";
        yield return new WaitForSecondsRealtime(3);
        GoToMainMenu();
    }

    #endregion

    #region MENU

    public void UpdateAmountOfPlayers(int amountOfPlayers)
    {
        amountOfPlayers += 2;
        for (int i = 0; i < _playerSelectionItems.Length; i++)
            _playerSelectionItems[i].SetActive(i < amountOfPlayers);

        for (int i = 0; i < _scoreTexts.Length; i++)
            _scoreTexts[i].gameObject.SetActive(i < amountOfPlayers);

        GameManager.Instance.UpdateAmountOfPlayers(amountOfPlayers);
    }

    public void StartGame()
    {        
        ResumeGame();
        ResetScores();
        GameManager.Instance.StartGame();
    }

    public void ResumeGame()
    {
        ShowScreen(GameScreen.InGame);
        GameManager.Instance.ResumeGame();
        _isGamePaused = false;
    }

    public void PauseGame()
    {
        ShowScreen(GameScreen.Pause);
        GameManager.Instance.PauseGame();
        _isGamePaused = true;
    }

    public void GoToMainMenu()
    {
        ShowScreen(GameScreen.MainMenu);
    }

    public void ReshootBall()
    {
        ResumeGame();
        GameManager.Instance.ReshootBall();
    }

    public void RestartGame()
    {
        ShowScreen(GameScreen.InGame);
        ResetScores();
        GameManager.Instance.StartGame();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    #endregion

    #region SCORE

    public void GivePointsToPlayer(int playerNumber, int points)
    {
        _scoreTexts[playerNumber].text = points.ToString();
        StartCoroutine(HighlightScore(playerNumber));
        _soundEffects.PlayScoreSound();
    }

    private IEnumerator HighlightScore(int scoreIndex)
    {
        _scoreTexts[scoreIndex].alpha = 1;
        yield return new WaitForSeconds(1);
        _scoreTexts[scoreIndex].alpha = 0.23f;
    }

    public void ResetScores()
    {
        for (int i = 0; i < _scoreTexts.Length; i++)
            _scoreTexts[i].text = "0";
    }

    #endregion


}
