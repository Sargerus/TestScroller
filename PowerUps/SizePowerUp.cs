using System.Collections;
using UnityEngine;

public class SizePowerUp : PowerUp
{
    private Vector3 _originalScale;

    private float _timePassed = 0;

    private void Start()
    {
        _originalScale = transform.lossyScale;

        transform.parent.localScale *= 0.5f;

        StartCoroutine(PowerUpDurationLoop());
    }

    public override void PowerUpWasPickedAgain()
    {
        _duration += 5.0f;
    }

    protected override IEnumerator PowerUpDurationLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            _timePassed += 0.1f;

            if (_timePassed >= _duration)
                break;
        }

        transform.parent.localScale = _originalScale;
        Destroy(this, 1.0f);
    }
}
