using UnityEngine;

public class BoundaryWall : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField]
    private GameObject _asteroids = default;
    [SerializeField]
    private GameObject _laserBeam = default;

    [SerializeField]
    private int _playerNumber = 0;

    public void TurnIntoBoundary()
    {
        _asteroids.SetActive(true);
        _laserBeam.SetActive(false);
    }

    public void TurnIntoScoreLine()
    {
        _asteroids.SetActive(false);
        _laserBeam.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameManager.Instance.Score(_playerNumber);
    }
}
