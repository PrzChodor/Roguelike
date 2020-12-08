using System;
using System.Collections;
using System.Collections.Generic;
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

    private Rigidbody2D player;
    private Animator animator;
    private Vector2 movement;
    private Vector2 direction;
    private Vector2 dash;
    private bool dashActive = false;
    private bool falling = false;
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
                dashesLeft++;
                UpdateUI();
            }
        }

        if (!dashActive && !falling && !Physics2D.IsTouching(floor, mainCollider) && Time.frameCount != 1)
            Fall();

        if (falling && fallTime < 1)
        {
            fallTime += 0.1f;
            GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), fallTime);
            if (fallTime > 1.0f)
            {
                Die();
            }
        }

        if(holdingFire && !currentWeapon.isShooting && !dead && currentWeapon.currentAmmo != 0 && !currentWeapon.isReloading)
        {
            StartCoroutine(currentWeapon.Shoot(angle));
            UpdateUI();
        }
        else if(currentWeapon.currentAmmo == 0 && !currentWeapon.isReloading)
        {
            StartCoroutine(currentWeapon.Reload());
        }
    }

    private void Update()
    {
        direction = cursor.transform.position;
        direction = Camera.main.ScreenToWorldPoint(direction);

        if (currentControls == "Keyboard&Mouse")
        {
            var temp = (direction - player.position);

            if (Math.Abs(temp.y - 0.2f) > 0.5f || Math.Abs(temp.x) > 0.7f)
            {
                temp.Normalize();
                animator.SetFloat("Horizontal", temp.x);
                animator.SetFloat("Vertical", temp.y);

                if (temp.x < -0.1)
                {
                    leftHand.localPosition = new Vector2(-0.2f, 0.2f);
                    rightHand.localPosition = new Vector2(0.15f, 0.15f);
                    currentWeapon.transform.parent = leftHand;
                    currentWeapon.transform.localPosition = Vector3.zero;
                    currentWeapon.transform.localScale = new Vector3(1, 1, 1);
                }
                else if (temp.x > 0.1)
                {
                    leftHand.localPosition = new Vector2(-0.15f, 0.15f);
                    rightHand.localPosition = new Vector2(0.2f, 0.2f);
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
        /*else if(currentControls == "Gamepad")
        {
            animator.SetFloat("Horizontal", direction.x);
            animator.SetFloat("Vertical", direction.y);

            if (direction.x < -0.1)
            {
                hand.localPosition = new Vector2(-0.2f, 0.2f);
                currentWeapon.transform.localScale = new Vector3(1, 1, 1);
            }
            else if (direction.x > 0.1)
            {
                hand.localPosition = new Vector2(0.2f, 0.2f);
                currentWeapon.transform.localScale = new Vector3(1, -1, 1);
            }

            var gunPoint = direction - (Vector2)currentWeapon.firePoint.position;
            gunPoint.Normalize();
            var angle = Vector2.SignedAngle(Vector2.left, gunPoint);
            currentWeapon.transform.rotation = Quaternion.Euler(0, 0, angle);
        }*/
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (!dashActive)
            movement = context.ReadValue<Vector2>();

        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    public void Look(InputAction.CallbackContext context)
    {

        if (currentControls == "Gamepad")
        {
            var temp = context.ReadValue<Vector2>().normalized;
            direction = (temp != new Vector2(0, 0) ? temp : direction);
        }

        else if (currentControls == "Keyboard&Mouse")
        {
            cursor.transform.position = context.ReadValue<Vector2>();
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (movement.sqrMagnitude != 0 && !dashActive && dashesLeft > 0 && context.started && !dead)
        {
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
        health = 0;
        leftHand.gameObject.SetActive(false);
        rightHand.gameObject.SetActive(false);
        UpdateUI();
        player.constraints = RigidbodyConstraints2D.FreezeAll;
        if (!dead)
            animator.SetTrigger("Die");
        dead = true;
        OnDeath.Invoke();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        UpdateUI();
    }

    public void Fall()
    {
        falling = true;
        leftHand.gameObject.SetActive(false);
        rightHand.gameObject.SetActive(false);
        player.gravityScale = 30.0f;
        GetComponent<SortingGroup>().sortingLayerName = "Pit";
        Physics2D.IgnoreLayerCollision(1, 9);
        Physics2D.IgnoreLayerCollision(1, 0);
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
