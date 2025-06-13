using System;
using _Scripts;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject throwablePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float speed = 10f;

    private GameObject _throwableObject;
    private bool _isFlying = false;
    
    private void Start()
    {
        SpawnThrowable();
    }

    private void Update()
    {
        //wenn es fliegt
        if (_throwableObject != null && _isFlying)
        {
            _throwableObject.transform.Translate(Vector3.up * (speed * Time.deltaTime));
        }

        //wenn es nicht fliegt
        if (!_isFlying && PlayerDidTapOrPress() && !GameManager.Instance.IsGameOver())
        {
            
            _isFlying = true;
        }
    }

    private void SpawnThrowable()
    {
        _throwableObject = Instantiate(throwablePrefab, spawnPoint.position, Quaternion.identity);
        _throwableObject.transform.parent = transform;
        
        
        var throwable = _throwableObject.GetComponent<Throwable>();
        if (throwable == null)
        {
            throwable = _throwableObject.AddComponent<Throwable>();
        }
        throwable.Init(this);
        
        // CALLBACK: Wenn Messer Ziel trifft
        throwable.OnHitTarget = target =>
        {
            GameManager.Instance.RegisterHit(); //registriere spielstatus dem game manager
            OnThrowableHitTarget(target);
        };
        
        // CALLBACK: Wenn Messer Deflektiert Wird
        throwable.OnThrowableDeflect = () =>
        {
            GameManager.Instance.RegisterMiss(); //registriere spielstatus dem game manager
            _isFlying = false;
            _throwableObject = null;
            
            Invoke(nameof(SpawnThrowable), 0.3f);
        };
    }
    
    
    private bool PlayerDidTapOrPress()
    {
        // PC: Leertaste oder linke Maustaste
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Vector2 clickPosition = Input.mousePosition;

            // Check if click is below xy% of the screen height sonst schießt man wenn man andere ui knöpfe drückt
            if (clickPosition.y < Screen.height * 0.45f)
            {
                return true;
            }
        }

        // Mobile: Touch-Eingabe (Finger hat gerade den Bildschirm berührt)
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 touchPosition = Input.GetTouch(0).position;

            if (touchPosition.y < Screen.height * 0.45)
            {
                return true;
            }
        }

        return false;
    }
    
    public void OnThrowableHitTarget(Transform target)
    {
        _isFlying = false;

        if (_throwableObject != null)
        {
            _throwableObject.transform.parent = target;
            _throwableObject = null;
            
            Invoke(nameof(SpawnThrowable),0f);
        }
    }
}
