using System.Collections.Generic;
using UnityEngine;

public class DepositParent : MonoBehaviour
{
    public List<Transform> slots = new List<Transform>();
    public GameObject slotPrefab;

    public void ResetDepositParent()
    {
        var obs = GetComponentsInChildren<Transform>();

        for (int i = 1; i < obs.Length; ++i)
        {
            Destroy(obs[i].gameObject);
        }

        slots.Clear(); ;
        slots.Add(Instantiate(slotPrefab, transform).transform);
    }
}
