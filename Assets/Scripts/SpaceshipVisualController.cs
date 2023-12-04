using UnityEngine;

public class SpaceshipVisualController : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField]
    private Transform _spaceshipTransform = default;
    [SerializeField]
    private GameObject _normalPropellant = default;
    [SerializeField]
    private GameObject _turboPropellant = default;
    [SerializeField]
    private GameObject _freezeSprite = default;
    [SerializeField]
    private GameObject _turboSprite = default;


    [Header("Paddle Components")]
    [SerializeField]
    private Rigidbody2D _rigidbody = default;
    [SerializeField]
    private PaddleController _paddleController = default;
    [SerializeField]
    private SpaceshipSoundEffects _spaceshipSoundEffects = default;

    private bool _turbo = false;


    void Update()
    {
        UpdateShipVisuals();
    }

    private void UpdateShipVisuals()
    {
        float velocity = _rigidbody.velocity.magnitude;
        if (velocity != 0)
        {
            _spaceshipSoundEffects.PlayMovementSound();
            if (_paddleController.Direction == MovementDirection.Vertical)
            {
                if (_rigidbody.velocity.y < 0) _spaceshipTransform.transform.rotation = Quaternion.Euler(0f, 0f, 180);
                else _spaceshipTransform.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            if (_paddleController.Direction == MovementDirection.Horizontal)
            {
                if (_rigidbody.velocity.x > 0) _spaceshipTransform.transform.rotation = Quaternion.Euler(0f, 0f, -90);
                else _spaceshipTransform.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
        }
        else _spaceshipSoundEffects.StopMovementSound();

        _normalPropellant.SetActive(velocity != 0 && !_turbo);
        _turboPropellant.SetActive(velocity != 0 && _turbo);
    }

    public void SetNormalSpeed()
    {
        _turbo = false;
        _spaceshipSoundEffects.SetNormalMovementSound();
        _turboSprite.SetActive(false);
    }

    public void SetTurbo()
    {
        _turbo = true;
        _spaceshipSoundEffects.SetTurboSound();
        _turboSprite.SetActive(true);
    }

    public void Freeze()
    {
        _freezeSprite.SetActive(true);
    }

    public void SetNormalState()
    {
        _freezeSprite.SetActive(false);
    }

}
