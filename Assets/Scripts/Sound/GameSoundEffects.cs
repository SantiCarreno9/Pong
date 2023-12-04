using UnityEngine;

public class GameSoundEffects : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource = default;

    [Header("UI")]
    [SerializeField]
    private AudioClip _buttonSound = default;

    [Space]
    [Header("Game sound")]
    [SerializeField]
    private AudioClip _scoreSound = default;
    [SerializeField]
    private AudioClip _winnerSound = default;

    public void PlayButtonSound()
    {
        _audioSource.clip = _buttonSound;        
        _audioSource.Play();
    }

    public void PlayScoreSound()
    {
        _audioSource.clip = _scoreSound;        
        _audioSource.Play();
    }

    public void PlayWinnerSound()
    {
        _audioSource.clip = _winnerSound;        
        _audioSource.Play();
    }
    
}
