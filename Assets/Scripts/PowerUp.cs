using UnityEngine;

public enum PowerUps
{
    Freeze,
    Turbo
}

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private PowerUps _powerUp = PowerUps.Freeze;

    [Space]
    [SerializeField]
    private PowerUpsManager _powerUpsManager;

    public PowerUps Power => _powerUp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Collision with Obstacle");
            _powerUpsManager.Respawn(this);
        }

        if (!collision.gameObject.CompareTag("Ball"))
            return;

        _powerUpsManager.ActivatePowerUp(this);
    }

}
