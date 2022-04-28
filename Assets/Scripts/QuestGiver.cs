using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class QuestGiver : MonoBehaviour, IInteractable
{
    public enum QuestType { Tier1, Tier2, Tier3}

    [SerializeField] private QuestType questTier;
    [SerializeField] private int minItemAmount = 1;
    [SerializeField] private int maxItemAmount = 4;
    public (int amount, Collectable item) CurrentQuest { get; set; }

    [SerializeField] private List<Collectable> possibleItems = new List<Collectable>();

    [SerializeField] private TextMeshProUGUI questText;

    private bool hasQuest;

    private (int amount, Collectable item) GenerateQuest()
    {
        int a = UnityEngine.Random.Range(minItemAmount,maxItemAmount + 1);
        Collectable i = possibleItems[Random.Range(0, possibleItems.Count)];

        return (a, i);
    }

    public void Interact(GameObject interactor)
    {
        if (IsCurrentlyInteractable(interactor))
        {
            if(!hasQuest)
            {
                CurrentQuest = GenerateQuest();
                hasQuest = true;
                questText.text = "Current Tier 1: " + CurrentQuest.amount + " " + CurrentQuest.item.Type + "s";
            }
            else
            {
                PlayerController player = interactor.GetComponent<PlayerController>();
                int count = 0;
                List<Collectable> invToRemove = new List<Collectable>();
                foreach(Collectable c in player.Inventory)
                {
                    if(CurrentQuest.item.Type == c.Type && count < CurrentQuest.amount)
                    {
                        count++;
                        invToRemove.Add(c);
                    }
                }

                if(count >= CurrentQuest.amount)
                {
                    player.XP += 1 + (int)questTier * 2;
                    player.Money += 1 + (int)questTier * 2;
                    hasQuest = false;
                    GetComponent<AudioSource>().Play();

                    questText.text = "Current Tier " + ((int)questTier + 1) + ": ";
                    foreach (Collectable c in invToRemove)
                    {
                        player.RemoveInventoryItem(c);
                    }
                    Interact(interactor);
                    
                }
            }
            
            
        }
    }

    public bool IsCurrentlyInteractable(GameObject interactor)
    {
        PlayerController player = interactor.GetComponent<PlayerController>();
        return player != null;
    }
}
