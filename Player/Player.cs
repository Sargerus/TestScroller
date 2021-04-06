using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    private Vector3 _cameraTopLeft;
    private Vector3 _cameraBottomRight;

    private Vector3 _moveTarget;
    private Vector3 _touchPoint;

    private bool _movingAllowed;

    private void Awake()
    {
        _cameraTopLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 10f));
        _cameraBottomRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 10f));

        _touchPoint.z = 10f;
    }

    private void Start()
    {
        _movingAllowed = true;
    }

    private void FixedUpdate()
    {
        if (Input.touchCount > 0 && _movingAllowed)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Moved)
            {
                _touchPoint.x = touch.position.x;
                _touchPoint.y = touch.position.y;

                _moveTarget = Camera.main.ScreenToWorldPoint(_touchPoint);

                _moveTarget.x = Mathf.Clamp(_moveTarget.x, _cameraTopLeft.x, _cameraBottomRight.x);

                transform.DOMoveX(_moveTarget.x, 0.05f, false);
            }
        }
    }

    public void RestrictMoving() 
    {
        _movingAllowed = false;
    } 
}
