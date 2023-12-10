using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    // Start is called before the first frame update

    public PlayerInput playerInputs;

    public InputAction input_move;
    public InputAction input_look;
    public InputAction input_run;
    public InputAction input_interact;
    public InputAction input_throw;
    public InputAction input_changeView;
    public Vector2 movement = Vector2.zero;
    public  Vector2 looking = Vector2.zero;
    private bool running = false;
    private RaycastHit raycastHit;
    public Transform guide;
    private bool isTaken;
    private bool readyToThrow;
    private GameObject takenObject;
    public GameObject _cameraThird;
    public GameObject _cameraFirst;
    public GameObject _folowTarget;
    public Rigidbody rigidBody;
    public float speed;
    private Animator _animator;
    private float grabDistance = 3f;
    private float sensitivy_firstPerson = 7f;
    private float sensitivy_thirdPerson = 1f;
    private bool firstLook = true;
    private GameObject currentCam;
    [SerializeField] private LayerMask raycastMask;
    private void Awake()
    {
        playerInputs = new PlayerInput();
        _animator = gameObject.GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        _cameraThird.SetActive(false);
        _cameraFirst.SetActive(true);
        currentCam = _cameraFirst;
    }

    private void OnEnable()
    {
        input_move = playerInputs.Player.Move;
        input_look = playerInputs.Player.Look;
        input_run = playerInputs.Player.Run;
        input_interact = playerInputs.Player.Interact;
        input_throw = playerInputs.Player.Throw;
        input_changeView = playerInputs.Player.View;
        input_move.Enable();
        input_look.Enable();
        input_run.Enable();
        input_interact.Enable();
        input_throw.Enable();
        input_changeView.Enable();
        input_run.performed += IncreaseSpeed;
        input_interact.performed += Interact;
        input_throw.performed += ThrowHeldObject;
        input_changeView.performed += ChangeView;
    }

    private void OnDisable()
    {
        input_move.Disable();
        input_look.Disable();
        input_run.Disable();
        input_interact.Disable();
        input_throw.Disable();
        input_changeView.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        movement = input_move.ReadValue<Vector2>();
        Look();
        // _animator.ResetTrigger("Pickup");
    }

    private void FixedUpdate()
    {
        if (movement.magnitude > 0.01f)
        {
            rigidBody.velocity = transform.forward * movement.y * speed + transform.right * movement.x * speed;
        }
        _animator.SetFloat("Speed", rigidBody.velocity.magnitude);
    }

    private void lookAtCursor()
    {
        rigidBody.transform.rotation = Quaternion.Euler(0, _folowTarget.transform.rotation.eulerAngles.y, 0);
        var angle = _folowTarget.transform.localEulerAngles.x;
        _folowTarget.transform.localEulerAngles = new Vector3(angle, 0, 0);
    }


    private void Look()
    {
        looking = input_look.ReadValue<Vector2>();
        var horizontalLook = looking.x * Vector3.up * Time.deltaTime;
        var verticalLook = looking.y * Vector3.left * Time.deltaTime;
        
        if (firstLook)
        {
            rigidBody.transform.localRotation *= Quaternion.Euler(horizontalLook * sensitivy_firstPerson);
            transform.localRotation *= Quaternion.Euler(horizontalLook * sensitivy_firstPerson);
            
            var newQ = currentCam.transform.localRotation * Quaternion.Euler(verticalLook * sensitivy_firstPerson);
            currentCam.transform.localRotation = RotationTools.ClampRotationAroundXAxis(newQ, -60, 60);    
        }
        else
        {
            _folowTarget.transform.rotation *= Quaternion.AngleAxis(looking.x * sensitivy_thirdPerson, Vector3.up);
            _folowTarget.transform.rotation *= Quaternion.AngleAxis(looking.y * sensitivy_thirdPerson, Vector3.left);

            var angles = _folowTarget.transform.localEulerAngles;
            angles.z = 0;

            var angle = _folowTarget.transform.localEulerAngles.x;

            if (angle > 180 && angle < 340)
            {
                angles.x = 340;
            }
            else if (angle < 180 && angle > 40)
            {
                angles.x = 40;
            }

            _folowTarget.transform.localEulerAngles = angles;
            
            if(movement.magnitude > 0.01 || isTaken)
                lookAtCursor();
        }
    }
    private void IncreaseSpeed(InputAction.CallbackContext context)
    {
        if (!running)
        {
            speed = 7f;
            running = true;
            _animator.SetBool("Running", true);
        }
        else
         {
             speed = 5f;
             running = false;
            _animator.SetBool("Running", false);
         }
    }

    private void Interact(InputAction.CallbackContext context)
    {
        Ray ray = new Ray();
        if (firstLook)
        {
            ray.origin = currentCam.transform.position;
            ray.direction = currentCam.transform.forward;
        }
        else
        {
            Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Debug.Log(Camera.main);
            ray = Camera.main.ScreenPointToRay(screenCenter);
        }
            
        if (isTaken) 
            DropObject();
        else if ((Physics.Raycast(ray, out raycastHit, Mathf.Infinity, raycastMask)) &&
                 ((raycastHit.transform.position - transform.position).magnitude <= grabDistance))
        {
            if (raycastHit.collider.tag == "Object")
            {
                TakeObject();
                return;
            }

            IInteractable interactible = raycastHit.collider.GetComponent<IInteractable>();
            if (interactible != null)
            {
                lookAtCursor();
                interactible.Interact();
            }
        }
    }

    public void DropObject()
    {
        isTaken = false;
        takenObject.transform.parent = null;
        takenObject.transform.position = new Vector3(takenObject.transform.position.x , takenObject.transform.position.y , takenObject.transform.position.z);
        takenObject.GetComponent<Rigidbody>().useGravity = true;
        takenObject.GetComponent<Rigidbody>().isKinematic = false;
        _animator.SetBool("Holding", false);
    }
    
    private void TakeObject()
    {
        lookAtCursor();
        _animator.SetTrigger("Pickup");
        raycastHit.collider.gameObject.transform.position = guide.transform.position + guide.forward * 1.5f + guide.up * 1f;
        raycastHit.collider.gameObject.transform.parent = this.transform;
        isTaken = true; 
        takenObject = raycastHit.collider.gameObject; 
        takenObject.GetComponent<Rigidbody>().useGravity = false; 
        takenObject.GetComponent<Rigidbody>().isKinematic = true;
        _animator.SetBool("Holding", true);
    }


    private void ThrowHeldObject(InputAction.CallbackContext context)
    {
        if (isTaken)
        {
            takenObject.transform.parent = null;
            takenObject.GetComponent<Rigidbody>().useGravity = true;
            takenObject.GetComponent<Rigidbody>().isKinematic = false;
            takenObject.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward * 500f);
            isTaken = false;
            _animator.SetBool("Holding", false);
        }
    }
    private void ChangeView(InputAction.CallbackContext context)
    {
        if (firstLook)
        {
            _cameraFirst.SetActive(false);
            _cameraThird.SetActive(true);
            currentCam = _cameraThird;
            firstLook = false;
        }

        else
        {
            _cameraThird.SetActive(false);
            _cameraFirst.SetActive(true);
            currentCam = _cameraFirst;
            firstLook = true;
        }
    }
    
    public GameObject GetTakenObject()
    {
        return takenObject;
    }
}
