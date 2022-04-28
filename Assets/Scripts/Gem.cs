using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : Pickup
{
    protected override void OnPickup(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller)
        {
            controller.Money++;
            GetComponent<AudioSource>().Play();
            StartCoroutine(WaitForAudio());
            GetComponent<Renderer>().enabled = false;

        }

    }

    private IEnumerator WaitForAudio()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject); 
    }
}
