using UnityEngine;
using System;

public class CollectibleItem : MonoBehaviour
{
    [SerializeField] private string itemID;

    void Awake()
    {
        // Garante que cada item tenha um ID único persistente
        if (string.IsNullOrEmpty(itemID))
            itemID = Guid.NewGuid().ToString();
    }

    public string GetID()
    {
        return itemID;
    }
}
