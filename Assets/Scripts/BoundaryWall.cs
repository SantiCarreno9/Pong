using UnityEngine;

public class BoundaryWall : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D _collider = default;
    [SerializeField]
    private GameManager _gameManager = default;
    [SerializeField]
    private int _playerNumber = 0;

    public void TurnIntoBoundary()
    {
        if (!ReferenceEquals(_collider, null))
            _collider.isTrigger = false;
    }

    public void TurnIntoScoreLine() 
    {
        if (!ReferenceEquals(_collider, null))
            _collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!ReferenceEquals(_gameManager, null))
            _gameManager.Score(_playerNumber);
    }
}
