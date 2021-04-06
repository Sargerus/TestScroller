using UnityEngine;

public class DeadLine : MonoBehaviour
{
    private Pool _pool;
    private GameManager _gameManager;

    private void Awake()
    {
        _pool = Pool.GetInstance();

        var go = GameObject.FindGameObjectWithTag("GameManager");
        if (go)
            _gameManager = go.GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Wall"))
        {
            Wall component = other.gameObject.GetComponent<Wall>();
            if (component != null)
            {
                _gameManager.CreateNewLine();
                Destroy(component);
            }

            _pool.AddToPool(other.gameObject);
        }

        if (other.CompareTag("PowerUpHolder"))
        {
            Destroy(other.gameObject);
        }

        if(other.CompareTag("ParticleSystem"))
        {
            Destroy(other.gameObject, 1);
        }
    }


}
