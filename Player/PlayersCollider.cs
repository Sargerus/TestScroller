using System;
using UnityEngine;

public class PlayersCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUpHolder"))
        {
            Type type = other.GetComponent<PowerUp>().GetType();
            Component powerUp = GetComponent(type);

            if (powerUp == null)
                gameObject.AddComponent(type);
            else ((PowerUp)powerUp).PowerUpWasPickedAgain();

            Destroy(other.gameObject);
        }
    }
}
