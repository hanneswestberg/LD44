using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

[RequireComponent(typeof(NavMeshAgent))]
public class GladiatorAI : Gladiator
{
    [Header("AI Settings:")]
    [SerializeField, Range(0f, 5f)] private float attackDistance;
    [SerializeField, Range(0f, 5f)] private float attackCooldown;

    [Header("Navigation:")]
    [SerializeField] private NavMeshAgent navMeshAgent;

    // Internal variables
    private Gladiator currentTarget;
    private float currentAttackCooldown;

    public override void SetUnitData(UnitData data) {
        base.SetUnitData(data);

        // Set speed
        navMeshAgent.speed = 3f + (Data.Speed * 0.1f);
    }

    // Update is called once per frame
    void Update() {

        if(IsAlive) {
            // First we find a target if we don't have any
            if(GameManager.instance != null
                && GameManager.instance.LivingGladiators != null
                && GameManager.instance.LivingGladiators.Any()) {
                currentTarget = GameManager.instance.LivingGladiators.Where(x => x != this).OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).FirstOrDefault();
            }

            if(currentTarget != null) {
                // If we are close enough to attack
                if(Vector3.Distance(transform.position, currentTarget.transform.position) < attackDistance && currentAttackCooldown < 0) {
                    Attack(currentTarget);
                    currentAttackCooldown = attackCooldown;
                }
                else if(currentAttackCooldown > 0) {
                    navMeshAgent.SetDestination(transform.position + (transform.position - currentTarget.transform.position).normalized);
                }
                else {
                    navMeshAgent.SetDestination(currentTarget.transform.position);
                }
            }
        }
        else {
            navMeshAgent.isStopped = true;
        }

        // Update timers
        currentAttackCooldown -= Time.deltaTime;
    }

    private void OnDrawGizmos() {
        if(currentTarget != null) {
            Gizmos.DrawLine(transform.position, currentTarget.transform.position);
        }
    }
}
