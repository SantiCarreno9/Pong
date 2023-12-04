using UnityEngine;

public class SpaceshipSoundEffects : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource = default;

    public void SetNormalMovementSound()
    {
        _audioSource.volume = 0.7f;
    }

    public void SetTurboSound()
    {
        _audioSource.volume = 0.9f;
    }

    public void PlayMovementSound()
    {
        if (!_audioSource.isPlaying)
            _audioSource.Play();
    }

    public void StopMovementSound()
    {
        _audioSource.Stop();
    }

}
