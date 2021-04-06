using UnityEngine;

public class PlayerColliderWalls : MonoBehaviour
{
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            Destroy(GetComponent<SphereCollider>());
            _gameManager.Death(other.gameObject);
        }
    }

}
