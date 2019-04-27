using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Linq;

[RequireComponent(typeof(NavMeshAgent))]
public class GladiatorAI : Gladiator
{
    [Header("AI References:")]
    [SerializeField] private NavMeshAgent navMeshAgent;

    // Internal variables
    private Gladiator currentTarget;

    public override void SetUnitData(UnitData data) {
        base.SetUnitData(data);

        // Set speed
        navMeshAgent.speed = 3f + (Data.Speed * 0.1f);
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();

        if(IsAlive) {
            // First we find a target if we don't have any
            if(GameManager.instance != null
                && GameManager.instance.LivingGladiators != null
                && GameManager.instance.LivingGladiators.Any()) {
                currentTarget = GameManager.instance.LivingGladiators.Where(x => x != this).OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).FirstOrDefault();
            }

            if(currentTarget != null) {
                // If we are close enough to attack
                if(Vector3.Distance(transform.position, currentTarget.transform.position) < attackDistance && canAttack) {
                    Attack(currentTarget);
                }
                else if(!canAttack) {
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
    }

    private void OnDrawGizmos() {
        if(currentTarget != null) {
            Gizmos.DrawLine(transform.position, currentTarget.transform.position);
        }
    }
}
