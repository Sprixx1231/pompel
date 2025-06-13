using System;
using UnityEngine;

namespace _Scripts
{
    public class Throwable : MonoBehaviour
    {
        
        public Action OnThrowableDeflect;
        public Action<Transform> OnHitTarget;
        
        private bool _isFlying = true;
        private PlayerController _playerController;
        

        public void Init(PlayerController controller)
        {
            _playerController = controller;
            _isFlying = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Target")) 
            {
                if (_playerController != null)
                {
                    _isFlying = false;
                    _playerController.OnThrowableHitTarget(other.transform);
                    
                    ChangeColliderSafeGuard();
                    OnHitTarget?.Invoke(other.transform); //action callback
                }
            }
            
            // Messer trifft anderes Messer
            if (other.GetComponent<Throwable>() != null)
            {
                var otherKnife = other.GetComponent<Throwable>();
                
                if (_isFlying && !otherKnife._isFlying)
                {
                    Debug.Log("Messer hat anderes Messer getroffen!");
                    
                    _isFlying = false;

                    DeflectAnimation();
                    
                    
                    // ❗ Collider deaktivieren, damit es keine anderen Messer stört
                    GetComponent<Collider2D>().enabled = false;
                    Destroy(gameObject, 3f);
                    OnThrowableDeflect?.Invoke(); //callback zum player controller
                }
            }
        }

        private void ChangeColliderSafeGuard()
        {
            var collider = GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                collider.size = new Vector2(collider.size.x * 0.6f, collider.size.y * 0.6f);
                collider.offset = new Vector2(0, -0.1f); 
            }
        }

        private void DeflectAnimation()
        {
            // Rigidbody aktivieren
            // TODO: besser machen
            var rb = GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 1.5f;
            rb.linearVelocity = new Vector2(UnityEngine.Random.Range(-1f, 1f), -2f); 
            rb.angularVelocity = UnityEngine.Random.Range(-200f, 200f);
        }
    }
}