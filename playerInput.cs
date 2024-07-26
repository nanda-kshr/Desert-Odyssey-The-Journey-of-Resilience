using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerInput : MonoBehaviour
{
    private playerMovement charController;

    void Awake()
    {
        charController = GetComponent<playerMovement>();
    }

    private void FixedUpdate()
    {
    
    }
}
