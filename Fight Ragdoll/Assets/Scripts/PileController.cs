using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PileController : MonoBehaviour
{
    public List<Transform> slots = new List<Transform>();
    public int maxNumberOfSlots = 10;
    [SerializeField] float forceX;
    [SerializeField] float forceZ;
    [SerializeField] GameObject slotPrefab;
    [SerializeField] float slotHight = 1f;
    [SerializeField] float rateRange = 0.4f;

    private Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();


        slots.Add(player.transform);
    }

    void LateUpdate()
    {
        if (slots.Count <= 1) return;

        for (int i = 1; i < slots.Count; ++i)
        {
            //Quanto mais alto, mais lenta a variação
            float rate = rateRange * ((float)i / (float)slots.Count);

            //Posição
            slots[i].localPosition = Vector3.Slerp(slots[i].localPosition, slots[i - 1].localPosition + (slots[i - 1].up * slotHight), rate);

            //Rotação
            slots[i].eulerAngles = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(player.input.y * slotHight * forceX, player.transform.localEulerAngles.y, player.input.x * slotHight * forceZ) + slots[i - 1].eulerAngles, rate / 4);
        }

    }

    public IEnumerator DepositRagdolls(DepositParent depositParent, Deposit deposit)
    {
        var _slots = GetComponentsInChildren<Slot>();
        var enemies = GetComponentsInChildren<Enemy>();
        slots.Clear();

        slots.Add(player.transform);

        foreach (Slot slot in _slots)
        {
            depositParent.slots.Add(slot.transform);
            slot.transform.SetParent(depositParent.transform);

            slot.transform.LeanMoveLocal(new Vector3(depositParent.slots[0].transform.position.x, slot.transform.position.y, depositParent.slots[0].transform.position.z), .25f);
        }

        enemies[enemies.Length - 1].isLastEnemy = true;

        var index = 0;

        foreach (Enemy enemy in enemies)
        {
            enemy.rb.constraints = RigidbodyConstraints.FreezeRotation;
            enemy.rb.constraints = RigidbodyConstraints.FreezePositionZ;
            enemy.rb.constraints = RigidbodyConstraints.FreezePositionX;
        }

        while (index < enemies.Length)
        {
            yield return StartCoroutine(DepositRoutine(enemies[index]));
            index++;
        }

        if (index == enemies.Length)
        {
            deposit.canDeposit = true;
        }
    }


    private IEnumerator DepositRoutine(Enemy enemy)
    {
        yield return new WaitForSeconds(.55f);
        enemy.m_Enemy.transform.SetParent(null);
        enemy.canCollect = true;
        enemy.rb.isKinematic = false;
    }


    public Transform AddSlot()
    {
        var newSlot = Instantiate(slotPrefab, transform).transform;
        slots.Add(newSlot);
        newSlot.localPosition = new Vector3(0, slots.Count + 1, 0);

        return newSlot;
    }
}
