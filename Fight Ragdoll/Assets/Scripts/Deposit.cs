using UnityEngine;

public class Deposit : MonoBehaviour
{
    [SerializeField] PileController pileController;
    [SerializeField] DepositParent depositParent;
    [SerializeField] UIController uiController;

    public bool canDeposit = true;

    private void Start()
    {
        canDeposit = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();

        if (player != null && player.ragdollsCount > 0 && canDeposit == true)
        {
            canDeposit = false;
            player.ragdollsCount = 0;
            uiController.UpdateMaxCollectable();
            StartCoroutine(pileController.DepositRagdolls(depositParent, this));
        }
    }
}
