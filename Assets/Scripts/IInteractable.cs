using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable 
{
    bool IsCurrentlyInteractable(GameObject interactor);

    void Interact(GameObject interactor);
}
