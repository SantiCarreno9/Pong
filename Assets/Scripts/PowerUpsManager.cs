using UnityEngine;

public class PowerUpsManager : MonoBehaviour
{
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
    private bool _spawn=false;

    public void ResumeSpawning() => _spawn = true;
    public void StopSpawning() => _spawn = false;

    void Start()
    {
        float xScale = _spawningSpace.transform.localScale.x;
        float yScale = _spawningSpace.transform.localScale.y;
        _xAxisRange = new Vector2(-xScale / 2, xScale / 2);
        _yAxisRange = new Vector2(-yScale / 2, yScale / 2);
        Reset();
    }

    public void SpawnRandomPowerUp()
    {
        if (!_spawn)
            return;

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

    public void HideAllPowerUps()
    {
        for (int i = 0; i < _freezePowerUps.Length; i++)
            _freezePowerUps[i].gameObject.SetActive(false);

        for (int i = 0; i < _turboPowerUps.Length; i++)
            _turboPowerUps[i].gameObject.SetActive(false);
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
        GameManager.Instance.ActivatePowerUp(powerUp.Power);

        powerUp.gameObject.SetActive(false);
        _activatedPowerUps--;
    }

    public void Reset()
    {
        _activatedPowerUps = 0;
        CancelInvoke();
        HideAllPowerUps();
        InvokeRepeating("SpawnRandomPowerUp", 10, 10);
    }
}
