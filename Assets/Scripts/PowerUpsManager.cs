using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsManager : MonoBehaviour
{
    [SerializeField]
    private GameManager _gameManager = default;
    [SerializeField]
    private PowerUp[] _freezePowerUps;
    [SerializeField]
    private PowerUp[] _turboPowerUps;

    [Space]
    [SerializeField]
    private GameObject _spawningSpace = default;

    private Vector2 _xAxisRange = Vector2.zero;
    private Vector2 _yAxisRange = Vector2.zero;

    private byte _activatedPowerUps = 0;

    // Start is called before the first frame update
    void Start()
    {
        float xScale = _spawningSpace.transform.localScale.x;
        float yScale = _spawningSpace.transform.localScale.y;
        _xAxisRange = new Vector2(-xScale / 2, xScale / 2);
        _yAxisRange = new Vector2(-yScale / 2, yScale / 2);
        InvokeRepeating("SpawnRandomPowerUp", 10, 10);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnRandomPowerUp()
    {
        if (_activatedPowerUps == 2)
            return;

        int randomPower = Random.Range(0, 2);
        GameObject powerUpItem = null;

        switch ((PowerUps)randomPower)
        {
            case PowerUps.Freeze:
                for (int i = 0; i < _freezePowerUps.Length; i++)
                {
                    if (!_freezePowerUps[i].gameObject.activeSelf)
                    {
                        powerUpItem = _freezePowerUps[i].gameObject;
                        break;
                    }
                }
                break;
            case PowerUps.Turbo:
                for (int i = 0; i < _turboPowerUps.Length; i++)
                {
                    if (!_turboPowerUps[i].gameObject.activeSelf)
                    {
                        powerUpItem = _turboPowerUps[i].gameObject;
                        break;
                    }
                }
                break;
            default:
                break;
        }

        if (ReferenceEquals(powerUpItem, null))
            return;

        powerUpItem.gameObject.SetActive(true);
        powerUpItem.transform.position = GetRandomPosition();
        _activatedPowerUps++;
    }

    public void Respawn(PowerUp powerUp)
    {
        powerUp.transform.position = GetRandomPosition();
    }

    private Vector2 GetRandomPosition()
    {
        float randomX = Random.Range(_xAxisRange.x, _xAxisRange.y);
        float randomY = Random.Range(_yAxisRange.x, _yAxisRange.y);
        return new Vector2(randomX, randomY);
    }

    public void ActivatePowerUp(PowerUp powerUp)
    {
        if (!ReferenceEquals(_gameManager, null))
            _gameManager.ActivatePowerUp(powerUp.Power);

        powerUp.gameObject.SetActive(false);
        _activatedPowerUps--;
    }

    //public void 
}
