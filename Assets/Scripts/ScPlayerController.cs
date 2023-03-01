    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScPlayerController : MonoBehaviour
{
    //GameObjects
    private GameObject WeaponObj;
    private Rigidbody2D playerRB;

    //Player Control Variables
    [SerializeField] private float movementSpeed = 7.0f;
    [SerializeField] private float weaponAwayRadius = 0.4f;
    private float weaponAngle;

    // Input Variables
    private float h_input;
    private float v_input;

    //Equip Weapon
    private bool isWeaponEquipped;

    //Player States
    private bool isIdle;
    private bool isMovingX;
    private bool isMovingY;
    private bool isMoving;

    //Animator
    Animator p_Animator;

    void Awake()
    {
        playerRB = this.GetComponent<Rigidbody2D>();

        p_Animator = this.GetComponent<Animator>();
        
        isWeaponEquipped = false;

        isIdle = true;
        isMovingX = false;
        isMovingY = false;
        isMoving = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (isWeaponEquipped == false && col.CompareTag("Weapon"))
        {
            WeaponObj = col.gameObject;

            col.gameObject.transform.position = new Vector2(this.gameObject.transform.position.x+ weaponAwayRadius, this.gameObject.transform.position.y);
            col.gameObject.transform.parent = this.gameObject.transform;
            isWeaponEquipped=true;
        }
    }

    void CheckInput()
    {
        h_input = Input.GetAxis("Horizontal");
        v_input = Input.GetAxis("Vertical");

        if (h_input < -0.01f || h_input > 0.01)
        {
            isIdle = false;
            isMovingX = true;
            isMoving = true;
            p_Animator.SetBool("Bool_IsIdle", isIdle);
            p_Animator.SetBool("Bool_IsWalking", isMoving);

            p_Animator.SetFloat("MovX", h_input);
        }
        if (v_input < -0.01f || v_input > 0.01)
        {
            isIdle = false;
            isMovingY = true;
            p_Animator.SetBool("Bool_IsIdle", isIdle);
            p_Animator.SetBool("Bool_IsWalking", isMoving);

            p_Animator.SetFloat("MovY", v_input);
        }
        if (v_input >= -0.01f && v_input <= 0.01f)
        {
            isMovingY = false;
            //p_Animator.SetBool("Bool_MovY", isMovingY);
        }
        if (h_input >= -0.01f && h_input <= 0.01f)
        {
            isMovingX = false;
            //p_Animator.SetBool("Bool_MovX", isMovingX);
        }
        if (isIdle==false && isMovingX==false && isMovingY==false)
        {
            isIdle=true;
            isMoving = false;
            p_Animator.SetBool("Bool_IsIdle", isIdle);
            p_Animator.SetBool("Bool_IsWalking", isMoving);

            StartCoroutine(WaitForIdleAnimation());
        }

        if (WeaponObj != null && isWeaponEquipped == true)
        {

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePos - this.transform.position;
            float angle = Vector2.SignedAngle(Vector2.right, direction);
            float angleRad = angle * Mathf.PI / 180;
            weaponAngle = angle;

            WeaponObj.transform.eulerAngles = new Vector3(0, 0, angle);
            WeaponObj.transform.localPosition = new Vector2(weaponAwayRadius * Mathf.Cos(angleRad),
                weaponAwayRadius * Mathf.Sin(angleRad));
        }

        /*if (isWeaponEquipped==true && Input.GetButtonDown("Fire1"))
        {
            if(WeaponObj != null)
            {
                WeaponObj.GetComponent<ScHandGun>().Shoot(weaponAngle);
            }
            else { Debug.LogError("Error: WeaponObj is " + WeaponObj);}
        }*/

        if (isWeaponEquipped == true && Input.GetButtonDown("Fire1") && WeaponObj != null)
        {
            WeaponObj.GetComponent<ScHandGun>().Shoot(weaponAngle);
        }
        else if (WeaponObj == null)
        {
            Debug.LogError("Error: WeaponObj is null");
        }
    }

    void FixedUpdate()
    {
        playerRB.velocity = new Vector2(h_input*movementSpeed, v_input*movementSpeed);
    }

    //Coroutine for changing Idle Animation
    IEnumerator WaitForIdleAnimation()
    {
        yield return new WaitForSeconds(0.1f);

        p_Animator.SetBool("Bool_IsIdle", false);
    }
}
