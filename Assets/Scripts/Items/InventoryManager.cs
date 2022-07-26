using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public bool isOpen;
    public event Action OnPause;
    public event Action OnEndPause;
    [SerializeField] private GameObject inventoryPanel;

    public static InventoryManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            if (GameManager.instance.state != GameState.Dialog)
                OpenIt();
        }
    }

    public void OpenIt()
    {
        isOpen = !isOpen;
        if (isOpen)
        {
            inventoryPanel.SetActive(true);
            OnPause?.Invoke();
        }
        else
        {
            inventoryPanel.SetActive(false);
            OnEndPause?.Invoke();
        }
    }
}
