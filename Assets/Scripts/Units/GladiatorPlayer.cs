using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GladiatorPlayer : Gladiator
{
    [Header("Player References")]
    [SerializeField] private Rigidbody rigidbody;

    // Update is called once per frame
    protected override void Update() {
        base.Update();
        
        if(IsAlive) {
            if(Input.GetButton("Fire1") && canAttack) {
                Attack();
            }
        }
    }

    private void FixedUpdate() {
        if(IsAlive && CanMove) {
            var moveForward = transform.forward * Input.GetAxis("Vertical");
            rigidbody.MovePosition(transform.position + (moveForward * 0.06f) + (moveForward * 0.06f) * (0.06f * (Data.Speed + Data.Weapon.Speed + Data.Armor.Speed)));

            transform.Rotate(new Vector3(0f, Input.GetAxis("Horizontal") * 7f, 0f));

            animator.SetFloat("Move", Input.GetAxis("Vertical"));
            animator.SetBool("Standing", Mathf.Abs(Input.GetAxis("Vertical")) < 0.05f);
        }
    }
}
