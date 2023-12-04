using UnityEngine;

public class BallSoundEffects : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource = default;
    [SerializeField]
    private AudioClip _collisionSound = default;
    [SerializeField]
    private AudioClip _powerUpSound = default;
    [SerializeField]
    private AudioClip _explosionSound = default;

    public void PlayCollisionSound()
    {
        _audioSource.clip = _collisionSound;
        _audioSource.Play();
    }

    public void PlayPowerUpSound()
    {
        _audioSource.clip = _powerUpSound;
        _audioSource.Play();
    }

    public void PlayExplosionSound()
    {
        _audioSource.clip = _explosionSound;
        _audioSource.Play();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayCollisionSound();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 0)
            PlayPowerUpSound();
    }
}
