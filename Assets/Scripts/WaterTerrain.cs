using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTerrain : Terrain
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void ApplyTerrainEffect(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player)
        {
            player.Speed = player.MaxSpeed / player.WaterModifier;
            GetComponent<AudioSource>().Play();
        }
    }

    protected override void RemoveTerrainEffect(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player)
        {
            player.Speed = player.MaxSpeed;
        }
    }
}
