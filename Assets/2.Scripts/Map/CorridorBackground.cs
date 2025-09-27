using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CorridorBackground : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    [SerializeField] private float speed;
    private Vector2 _currentMovement;
    
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            var input = context.ReadValue<Vector2>();
            // 좌우만 허용
            _currentMovement = new Vector2(input.x, 0f);
        }
        else if (context.canceled)
        {
            _currentMovement = Vector2.zero;
        }
    }
    private void FixedUpdate()
    {
        Vector2 delta = new Vector2(_currentMovement.x, 0f) * speed * Time.fixedDeltaTime;
        if (this.transform.position.x > 10)
        {
            this.transform.position = new Vector3(10, 0, 0);
        }
        else if (this.transform.position.x < -10)
        {
            this.transform.position = new Vector3(-10, 0, 0);            
        }
        else
        {
            _rigidbody2D.MovePosition(_rigidbody2D.position + delta);
        }
    }
    
}
