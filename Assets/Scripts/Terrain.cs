using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ApplyTerrainEffect(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        RemoveTerrainEffect(collision);
    }

    protected virtual void ApplyTerrainEffect(Collider2D collision)
    {

    }

    protected virtual void RemoveTerrainEffect(Collider2D collision)
    {

    }
}
