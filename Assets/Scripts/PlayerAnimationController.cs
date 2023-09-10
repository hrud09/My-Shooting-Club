using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator playerAnim;
    public float movementSpeed;

    private void Update()
    {
        playerAnim.SetFloat("Speed", movementSpeed);
    }
}
