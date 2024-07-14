using DG.Tweening;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject m_Enemy;
    public Collider collectableCollider;
    Animator animator;
    private bool isAlive;
    public Rigidbody rb;
    [SerializeField] float force;
    public bool canCollect;
    public Vector3 initialTransformOnPile = new Vector3();
    public EnemySpawner spawner;
    public bool isLastEnemy;

    private void Start()
    {
        canCollect = true;
        isAlive = true;
        isLastEnemy = false;
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isAlive) return;

        var player = other.GetComponentInParent<Player>() as Player;

        if (player != null && player.isAttacking)
        {
            GetHit(player);
        }
    }

    private void GetHit(Player player)
    {
        Instantiate(player.hit_VFX, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);

        animator.enabled = false;
        isAlive = false;

        rb.constraints = RigidbodyConstraints.None;
        rb.AddForce(player.transform.forward * force, ForceMode.Impulse);
        rb.AddForce(player.transform.up * force / 150, ForceMode.Impulse);

        player.CameraShake();
        player.PunchSound();

        float count = 0f;
        var valueToTween = 0f;
        DOTween.To(() => valueToTween, x => valueToTween = x, 1, .25f)
            .OnUpdate(() => count = valueToTween).OnComplete(() => collectableCollider.enabled = true);
    }

    public void Collect(PileController pileController)
    {
        canCollect = false;
        m_Enemy.transform.SetParent(pileController.AddSlot());
        m_Enemy.transform.localPosition = new Vector3(0, 0, 0);
        transform.localPosition = new Vector3(0, 0, 0);
        initialTransformOnPile = transform.localPosition;
        transform.localEulerAngles = new Vector3(0, 0, 90);
        rb.isKinematic = true;
    }
}
