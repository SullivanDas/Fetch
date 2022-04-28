using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour, IChangeableHealth
{
    [SerializeField] private int health = 1;
    [SerializeField] private GameObject gem;
    private bool isInteractable = true;

    public void ChangeHealth(int amount)
    {
        if (isInteractable)
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
        GetComponent<AudioSource>().Play();
        Instantiate(gem, transform.position, Quaternion.identity);
        StartCoroutine(DestroyAfterAudio());
        isInteractable = false;
        GetComponentInChildren<Renderer>().enabled = false;
    }

    private IEnumerator DestroyAfterAudio()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject.transform.parent.gameObject);
    }

}
