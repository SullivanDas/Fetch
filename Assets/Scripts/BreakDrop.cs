using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakDrop : MonoBehaviour, IChangeableHealth
{
    [SerializeField] private float maxHealth;
    [SerializeField] private GameObject droppedItem;
    [SerializeField] private float respawnTime = 15f;

    private float health;

    private void Start()
    {
        health = maxHealth;
    }
    private bool isHidden;

    public void ChangeHealth(int amount)
    {
        if (!isHidden)
        {
            health += amount;
            if (health <= 0)
            {
                Die();
            }
        }

    }

    private void Die()
    {
        if (droppedItem)
        {
            Instantiate(droppedItem, transform.position, Quaternion.identity);

            isHidden = true;
            GetComponentInChildren<Renderer>().enabled = false;
            GetComponent<AudioSource>().Play();
            StartCoroutine(RespawnTimer());
        }
    }

    private void Show()
    {
        isHidden = false;
        GetComponentInChildren<Renderer>().enabled = true;
        health = maxHealth;
    }

    private IEnumerator RespawnTimer()
    {
        yield return new WaitForSeconds(respawnTime);
        Show();
    }

}
