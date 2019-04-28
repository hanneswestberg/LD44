﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GladiatorPlayer : Gladiator
{
    [Header("Player References")]
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private PlayerTargets playerTargets;

    // Update is called once per frame
    protected override void Update() {
        base.Update();

        if(IsAlive) {
            if(Input.GetButton("Fire1") && canAttack) {
                foreach(var target in playerTargets.ValidTargets) {
                    Attack(target);
                }
            }
        }

    }

    private void FixedUpdate() {
        if(IsAlive) {
            var moveForward = transform.forward * Input.GetAxis("Vertical");
            rigidbody.MovePosition(transform.position + (moveForward * 0.07f) + (moveForward * 0.07f) * (0.1f * Data.Speed));

            transform.Rotate(new Vector3(0f, Input.GetAxis("Horizontal") * 4f, 0f));

            animator.SetFloat("Move", Input.GetAxis("Vertical"));
            animator.SetBool("Standing", Mathf.Abs(Input.GetAxis("Vertical")) < 0.05f);
        }
    }
}
