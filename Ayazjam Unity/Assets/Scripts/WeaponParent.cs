using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    public Vector2 PointerPosition { get; set; }

    private void Update()
    {
        transform.position = (PointerPosition -(Vector2)transform.position).normalized
            ;
    }
}
