using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    #region Singleton
    public static Inventory instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("More than one instance of Inventory found!");
            return;
        }

        instance = this;
    }
    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int space = 20;
    public List<Item> items = new();

    public GameObject ItemPrefab;

    public Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public bool Add(Item item)
    {
        if (items.Count >= space)
        {
            Debug.Log("Not enough room.");
            return false;
        }

        items.Add(item);


        if(onItemChangedCallback != null)
        {
            Debug.Log("onItemChangedCallback is not null");
        }
        else
        {
            Debug.Log("onItemChangedCallback is null");
        }

        onItemChangedCallback?.Invoke();

        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);

        Instantiate(ItemPrefab, player.transform.position + Camera.main.transform.forward + Vector3.up, Quaternion.identity);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }
}
