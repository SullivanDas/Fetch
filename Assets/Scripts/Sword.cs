using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IInteractable
{
    public void Interact(GameObject interactor)
    {
        if (IsCurrentlyInteractable(interactor))
        {
            PlayerController player = interactor.GetComponent<PlayerController>();
            player.AddEquipment(PlayerController.Equipment.Sword);
            Destroy(gameObject);
        }
    }

    public bool IsCurrentlyInteractable(GameObject interactor)
    {
        PlayerController player = interactor.GetComponent<PlayerController>();
        if(player != null)
        {
            return !player.HasEquipment[(int)PlayerController.Equipment.Sword];
        }
        else
        {
            return false;
        }
    }

}
