using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerController : Character
{
    public float speed = 3.0f;
    [Header("Dash")]
    public float dashSpeed = 10.0f;
    public float dashDuration = 0.1f;
    public float dashCooldown = 1.0f;
    public int dashNumber = 2;
    [Space]
    public GameObject gameMaster;
    public Weapon currentWeapon;
    [Space]
    public Image cursor;
    [Space]
    public AudioClip dashClip;
    public AudioClip dashRecharge;
    public AudioClip heal;

    [HideInInspector]
    public bool falling = false;
    [HideInInspector]
    public bool firstFrame = true;
    [HideInInspector]
    public bool dashActive = false;

    private Rigidbody2D player;
    private Animator animator;
    private Vector2 movement;
    private Vector2 movDuringDash;
    private bool movChangedDuringDash;
    private Vector2 direction;
    private Vector2 dash;
    private float dashTime;
    private float dashCooldownTime;
    private int dashesLeft;
    private UIManager uiManager;
    private LevelMaster levelMaster;
    private string currentControls = "Keyboard&Mouse";
    private float fallTime = 0;
    private CompositeCollider2D floor;
    private CapsuleCollider2D mainCollider;
    private Transform leftHand;
    private Transform rightHand;
    private UnityEvent OnDeath;
    private TrailRenderer trail;
    private float angle;
    private bool holdingFire = false;


    public override void Awake()
    {
        base.Awake();
        leftHand = transform.GetChild(0);
        rightHand = transform.GetChild(1);
        mainCollider = GetComponent<CapsuleCollider2D>();
        player = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        uiManager = gameMaster.GetComponent<UIManager>();
        levelMaster = gameMaster.GetComponent<LevelMaster>();
        OnDeath = new UnityEvent();
        OnDeath.AddListener(levelMaster.DeactivateCurrent);
        OnDeath.AddListener(uiManager.ShowDeathScreen);
        dashTime = dashDuration;
        dashCooldownTime = 0;
        dashesLeft = dashNumber;
        trail = GetComponent<TrailRenderer>();
    }

    private void Start()
    {
        UpdateUI();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        currentWeapon.onReloaded.AddListener(UpdateUI);
    }

    void FixedUpdate()
    {
        if (dashActive)
        {
            Physics2D.IgnoreLayerCollision(0, 11);
            Physics2D.IgnoreLayerCollision(0, 12);
            player.MovePosition(player.position + movement * speed * Time.deltaTime + dash * Time.deltaTime);
            dashTime -= Time.deltaTime;
            trail.emitting = true;
            if (dashTime < 0)
            {
                Physics2D.IgnoreLayerCollision(0, 11, false);
                Physics2D.IgnoreLayerCollision(0, 12, false);
                dashActive = false;
                dashTime = dashDuration;

                if (movChangedDuringDash)
                    movement = movDuringDash;
                animator.SetFloat("Speed", movement.sqrMagnitude);
                movChangedDuringDash = false;
            }
        }
        else
        {
            player.MovePosition(player.position + movement * speed * Time.deltaTime);
            trail.emitting = false;
        }

        if (dashesLeft < dashNumber)
        {
            if (dashCooldownTime < dashCooldown)
            {
                UpdateUI();
                dashCooldownTime += Time.deltaTime;
            }
            else
            {
                dashCooldownTime = 0;
                GetComponent<AudioSource>().PlayOneShot(dashRecharge);
                dashesLeft++;
                UpdateUI();
            }
        }

        if (!dashActive && !falling && !Physics2D.IsTouching(floor, mainCollider) && !firstFrame)
            Fall();

        if (falling && fallTime < 0.5)
        {
            fallTime += Time.deltaTime;
            GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), 10 * fallTime);
            dead = true;
            if (fallTime > 0.5)
            {
                Die();
            }
        }

        if (holdingFire && !currentWeapon.isShooting && !dead && currentWeapon.currentAmmo != 0 && !currentWeapon.isReloading)
        {
            StartCoroutine(currentWeapon.Shoot(angle));
            UpdateUI();
        }
        else if (currentWeapon.currentAmmo == 0 && !currentWeapon.isReloading && !currentWeapon.isShooting)
        {
            StartCoroutine(currentWeapon.Reload());
        }

        firstFrame = false;
    }

    private void Update()
    {
        direction = cursor.transform.position;
        direction = Camera.main.ScreenToWorldPoint(direction);

        var collisions = new List<Collider2D>();
        player.GetContacts(collisions);

        if (collisions.Any(c => c.CompareTag("Door")) && !falling)
        {
            var door = (collisions.Find(c => c.CompareTag("Door"))).GetComponent<Door>();
            if (door.opened)
                uiManager.ShowInteraction();
        }
        else
        {
            uiManager.HideInteraction();
        }

        if (currentControls == "Keyboard&Mouse" && Time.timeScale != 0)
        {
            var temp = (direction - player.position);

            if (Math.Abs(temp.y - 0.2f) > 0.5f || Math.Abs(temp.x) > 0.7f)
            {
                temp.Normalize();
                animator.SetFloat("Horizontal", temp.x);
                animator.SetFloat("Vertical", temp.y);

                if (temp.x < -0.1)
                {
                    leftHand.localPosition = new Vector2(-0.18f, 0.15f);
                    rightHand.localPosition = new Vector2(0.15f, 0.12f);
                    currentWeapon.transform.parent = leftHand;
                    currentWeapon.transform.localPosition = Vector3.zero;
                    currentWeapon.transform.localScale = new Vector3(1, 1, 1);
                }
                else if (temp.x > 0.1)
                {
                    leftHand.localPosition = new Vector2(-0.15f, 0.12f);
                    rightHand.localPosition = new Vector2(0.18f, 0.15f);
                    currentWeapon.transform.parent = rightHand;
                    currentWeapon.transform.localPosition = Vector3.zero;
                    currentWeapon.transform.localScale = new Vector3(1, -1, 1);
                }

                var gunPoint = direction - (Vector2)currentWeapon.firePoint.position;
                gunPoint.Normalize();
                angle = Vector2.SignedAngle(Vector2.left, gunPoint);
                currentWeapon.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (!dashActive)
            movement = context.ReadValue<Vector2>();
        else
        {
            movChangedDuringDash = true;
            movDuringDash = context.ReadValue<Vector2>();
        }

        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    public void Look(InputAction.CallbackContext context)
    {

        if (currentControls == "Gamepad")
        {
            var temp = context.ReadValue<Vector2>().normalized;
            direction = (temp != new Vector2(0, 0) ? temp : direction);
        }

        else if (currentControls == "Keyboard&Mouse" && cursor != null)
        {
            cursor.transform.position = context.ReadValue<Vector2>();
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.started && !falling)
        {
            var collisions = new List<Collider2D>();
            player.GetContacts(collisions);

            if (collisions.Any(c => c.CompareTag("Door")))
            {
                var door = (collisions.Find(c => c.CompareTag("Door"))).GetComponent<Door>();
                door.Enter();
            }
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (movement.sqrMagnitude != 0 && !dashActive && dashesLeft > 0 && context.started && !dead)
        {
            GetComponent<AudioSource>().PlayOneShot(dashClip);
            dash = movement * dashSpeed;
            dashActive = true;
            dashesLeft--;
            UpdateUI();
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.started)
            holdingFire = true;
        else if (context.canceled)
            holdingFire = false;
    }

    public void Reload(InputAction.CallbackContext context)
    {
        if (context.started && !currentWeapon.isReloading && currentWeapon.currentAmmo != currentWeapon.maxAmmo)
            StartCoroutine(currentWeapon.Reload());
    }

    public void ControlsChanged(PlayerInput input)
    {
        currentControls = input.currentControlScheme;
        direction = new Vector2(1, 0);
    }

    public override void Die()
    {
        MusicPlayer.instance.OnDeath();
        health = 0;
        leftHand.gameObject.SetActive(false);
        rightHand.gameObject.SetActive(false);
        UpdateUI();
        player.constraints = RigidbodyConstraints2D.FreezeAll;
        animator.SetTrigger("Die");
        dead = true;
        OnDeath.Invoke();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        UpdateUI();
    }

    public void Heal(int amount)
    {
        health = Mathf.Clamp(health + amount, 0, maxHealth);
        GetComponent<AudioSource>().PlayOneShot(heal);
        UpdateUI();
    }

    public void Fall()
    {
        falling = true;
        leftHand.gameObject.SetActive(false);
        rightHand.gameObject.SetActive(false);
        player.gravityScale = 30.0f;
        GetComponent<SortingGroup>().sortingOrder = -2;
    }

    public void LevelChange(Level level)
    {
        floor = level.floor;
    }

    public void UpdateUI()
    {
        uiManager.UpdateAmmoUI(currentWeapon.currentAmmo, currentWeapon.maxAmmo);
        uiManager.UpdateDashUI(dashNumber, dashesLeft, dashCooldownTime, dashCooldown);
        uiManager.UpdateHealthUI(maxHealth, health);
    }
}
