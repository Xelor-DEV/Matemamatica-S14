using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;
    [SerializeField] private float brakeForce;
    [SerializeField] private float enlarge;
    [SerializeField] private Vector3 shrink;
    [SerializeField] private float duration;
    [SerializeField] private Ease ease;
    private Vector2 movement = Vector2.zero;
    private Rigidbody _compRigidbody;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        _compRigidbody = GetComponent<Rigidbody>();
    }
    public void SetDirection(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed == true)
        {
            _compRigidbody.velocity = new Vector3(_compRigidbody.velocity.x, 0, _compRigidbody.velocity.z);
            _compRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    public void Enlarge(InputAction.CallbackContext context)
    {
        if (context.performed == true)
        {
            this.gameObject.transform.DOScale(Vector3.one * enlarge, duration).SetEase(ease);
        }
    }
    public void Shrink(InputAction.CallbackContext context)
    {
        if (context.performed == true)
        {
            this.gameObject.transform.DOScale(shrink, duration).SetEase(ease);
        }
    }
    public void Default(InputAction.CallbackContext context)
    {
        if (context.performed == true)
        {
            this.gameObject.transform.DOScale(Vector3.one, duration).SetEase(ease);
        }
    }
    private void FixedUpdate()
    {
        _compRigidbody.AddForce(Vector3.down * gravity);
        _compRigidbody.AddForce(new Vector3(movement.x * speed, 0, movement.y * speed));
        if (movement.magnitude < 0.2f)
        {
            Vector3 brakeForce = -_compRigidbody.velocity.normalized * _compRigidbody.velocity.magnitude * this.brakeForce;
            _compRigidbody.AddForce(brakeForce, ForceMode.Force);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "LOSE")
        {
            GameManagerController.Instance.LoadScene("GameOver");
        }
    }
}
