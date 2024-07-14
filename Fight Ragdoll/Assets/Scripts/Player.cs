using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PileController pileController;
    public Transform targetFollow;
    public GameObject hit_VFX;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float attackAnimationDuration;

    [SerializeField] Material powerUpMaterial;
    [SerializeField] SkinnedMeshRenderer playerMesh;

    private float inputMagnitude;
    private float attackTime;
    private float yVelocity;

    private Vector3 movementDirection = new Vector3();
    private Vector3 velocity = new Vector3();

    private bool canMove = false;

    private PlayerInputActions actions;
    private Animator animator;
    private CharacterController characterController;
    private Camera cam;
    private AudioSource audioSource;

    [HideInInspector] public int ragdollsCount = 0;
    [HideInInspector] public int money = 0;
    [HideInInspector] public bool isAttacking = false;
    [HideInInspector] public Vector2 input = new Vector2();

    [Header("Audio Controller")]
    [SerializeField] AudioClip[] punchAudios;
    [SerializeField] AudioClip collectClip;
    [SerializeField] AudioClip powerUpSound;
    [SerializeField] AudioClip depositSound;
    [SerializeField] AudioClip errorSound;
    [SerializeField] UIController uiController;

    #region Awake and Start
    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        cam = Camera.main;

        actions = new PlayerInputActions();
        actions.Enable();

        ragdollsCount = 0;
    }

    private void Start()
    {
        canMove = true;
    }
    #endregion

    #region Update
    private void Update()
    {
        GetInput();
        ApplyGravity();
        ApplyRotation();

        if (isAttacking)
        {
            if (Time.time >= attackTime + attackAnimationDuration / 2)
            {
                isAttacking = false;
                canMove = true;
            }
        }
    }
    #endregion

    #region MOVIMENTO
    public void GetInput()
    {
        if (!canMove)
        {
            input = new Vector2(0, 0);
            inputMagnitude = 0;
            return;
        }

        input = actions.Player.Move.ReadValue<Vector2>();
        movementDirection = new Vector3(input.x, 0, input.y);
        inputMagnitude = Mathf.Clamp01(input.magnitude);
    }

    public void ApplyMovement()
    {
        characterController.Move(velocity);
    }

    public void ApplyRotation()
    {
        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnAnimatorMove()
    {
        velocity = animator.deltaPosition;
        velocity.y = yVelocity;
        SetInputMagnitudeOnAnimator();
        ApplyMovement();
    }

    public void ApplyGravity()
    {
        if (characterController.isGrounded && inputMagnitude < 0.0f)
            yVelocity = -1;

        else
            yVelocity += -9.8f * Time.deltaTime;
    }

    public void SetInputMagnitudeOnAnimator() => animator.SetFloat("InputMagnitude", inputMagnitude, 0.05f, Time.deltaTime);
    #endregion

    #region BOTÕES

    public void OnAttackButton()
    {
        if (!isAttacking)
        {
            canMove = false;
            isAttacking = true;
            animator.Play("Punch");
            attackTime = Time.time;
        }
    }

    public void OnBuySlotsButton()
    {
        if (money >= 100)
        {
            money -= 100;
            uiController.UpdateMoney(money);
            pileController.maxNumberOfSlots += 5;
            uiController.UpdateMaxCollectable();
            PowerUpSound();
            playerMesh.material = powerUpMaterial;
        }

        else
        {
            ErrorSound();
        }
    }
    #endregion

    #region COLISÃO CHECK
    private void OnTriggerEnter(Collider other)
    {
        var collectable = other.GetComponentInParent<Enemy>();

        if (collectable != null && collectable.canCollect == true && ragdollsCount < pileController.maxNumberOfSlots)
        {
            CollectSound();
            ragdollsCount++;
            collectable.Collect(pileController);
            uiController.UpdateMaxCollectable();
        }
    }
    #endregion

    #region OPCIONAIS
    public void CameraShake()
    {
        var pos = cam.transform.position;
        pos.z += Random.Range(2, 5.1f);
        cam.transform.DOShakeRotation(1, Random.Range(.75f, 1.25f), 10, 90, true);
    }

    public void PunchSound()
    {
        audioSource.PlayOneShot(punchAudios[(int)Random.Range(0, 2)]);

    }

    public void CollectSound()
    {
        audioSource.PlayOneShot(collectClip);
    }

    public void DepositSound()
    {
        audioSource.PlayOneShot(depositSound);
    }

    public void PowerUpSound()
    {
        audioSource.PlayOneShot(powerUpSound);
    }

    public void ErrorSound()
    {
        audioSource.PlayOneShot(errorSound);
    }
    #endregion
}
