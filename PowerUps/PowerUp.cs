using System.Collections;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    protected float _duration = 5.0f;
    public abstract void PowerUpWasPickedAgain();
    protected abstract IEnumerator PowerUpDurationLoop();
}
