using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Collectable : MonoBehaviour, IInteractable
{
    public enum CollectableType { Mushroom, Flower, Berry, Wood, Fish, Bug, Bird, Lilypad }

    [SerializeField] private CollectableType _type;
    [SerializeField] private float respawnTime = 15f;
    [SerializeField] private bool isCatchable;
    [SerializeField] private bool isRespawnable = true;

    private bool isHidden;
    private float currentRespawnTime = 0f;
    public CollectableType Type { get { return _type; } private set { _type = value; } }

    private void Update()
    {
        if (isHidden && isRespawnable)
        {
            currentRespawnTime += Time.deltaTime;
            if (currentRespawnTime > respawnTime)
            {
                
                Show();
                currentRespawnTime = 0;
            }
        }   
    }

    public bool IsCurrentlyInteractable(GameObject interactor)
    {
        if (isHidden || isCatchable)
        {
            return false;
        }
        PlayerController player = interactor.GetComponent<PlayerController>();
        if (!player)
        {
            return false;
        }
        return player.CurrentInventoryAmount + 1 <= player.InventorySize;
    }

    public void Interact(GameObject interactor)
    {
        PlayerController player = interactor.GetComponent<PlayerController>();
        if (player)
        {
            player.AddInventoryItem(this);
            GetComponent<AudioSource>().Play();
            Hide();
            
        }
    }

    private void Hide()
    {
        isHidden = true;
        GetComponentInChildren<Renderer>().enabled = false;       
    }

    private void Show()
    {
        isHidden = false;
        GetComponentInChildren<Renderer>().enabled = true;
    }
}