using System.Collections;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(ScoreCoroutine());
    }

    private IEnumerator ScoreCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
        GameManager.Instance.Score(_playerNumber);
    }
}
