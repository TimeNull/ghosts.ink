using SO.Variables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class YELLOW : EnemyController
{
    [SerializeField] protected float searchRadius;
    [SerializeField] protected Gun gun;
    [SerializeField] protected FloatReference rotationSpeed;
    [SerializeField] protected FloatReference rayMaxDistance;
    [SerializeField] protected FloatReference maxDistanceFromPlayer;
    [SerializeField] protected FloatReference minDistanceFromPlayer;
    [SerializeField] protected FloatReference sideWalkDistance;
    [SerializeField] protected FloatReference sideWalkCooldown;
    [SerializeField] protected LayerMask targetLayers;

    private float manualAcc = 0;
    private float currentSideWalkCd;
    private RaycastHit hitInfo;
    private Vector3 randomDirection;
    private float remainingDistance;

    private void Start()
    {
        StartCoroutine(RemainingDistance());
    }

    IEnumerator RemainingDistance()
    {
        while (true)
        {
            remainingDistance = NavMeshUtils.GetPathLength(transform, player, 1);
            yield return new WaitForSeconds(0.2f);
        }
    }

    protected override void HandleMovement()
    {
        if (Physics.Raycast(transform.position, player.position - transform.position, out hitInfo, rayMaxDistance, targetLayers) 
            && hitInfo.transform.CompareTag(player.tag))
        {
            if (remainingDistance > maxDistanceFromPlayer)
            {
                manualAcc = 0;
                agent.isStopped = false;
                agent.updateRotation = true;
                agent.SetDestination(player.position);
                currentSideWalkCd = 0;
                HandleAttack(false);
            }
            else if(remainingDistance < minDistanceFromPlayer)
            {
                if (manualAcc > 0.1f)
                    manualAcc = 0;

                agent.isStopped = false;
                agent.updateRotation = false;
                Quaternion newRotation = Quaternion.LookRotation(player.position - transform.position, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, 500);
                
                agent.ResetPath();
                manualAcc = Mathf.Lerp(manualAcc, 0.2f, Time.deltaTime);
                agent.Move(-transform.forward * manualAcc);

                HandleAttack(true);
            }
            else
            {
                agent.isStopped = true;
                agent.ResetPath();

                agent.updateRotation = false;

                if (currentSideWalkCd > 0)
                    currentSideWalkCd -= Time.deltaTime;
                else
                {
                    manualAcc = 0;
                    randomDirection = sideWalkDistance * Random.Range(-1.0f, 1.0f) * transform.right;
                    currentSideWalkCd = sideWalkCooldown;
                }

                manualAcc = Mathf.Lerp(manualAcc, 1.5f, Time.deltaTime);
                agent.Move(randomDirection * manualAcc);

                Quaternion newRotation = Quaternion.LookRotation(player.position - transform.position, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, 500);

                HandleAttack(true);
            }
        }
        else
            agent.SetDestination(player.position);
    }

    protected void HandleAttack(bool fire)
    {
        gun.HoldFire(fire);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, hitInfo.point + Vector3.up);

        Gizmos.DrawWireSphere(randomDirection, 1);
    }

}
