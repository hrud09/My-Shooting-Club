using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.AI;
using System.Collections;

public class Customer : MonoBehaviour
{
    public CustomerInfo customerInfo;
    public int currentWaypointIndex = 0;
    public Transform meshParent;
    public CustomerManager customerManager;
    Transform exitPointFromDesk;
    public void InitializeCustomer(CustomerInfo info)
    {
        for (int i = 0; i < meshParent.childCount; i++)
        {
            meshParent.GetChild(i).gameObject.SetActive(false);
        }
        meshParent.GetChild(Random.Range(0, meshParent.childCount)).gameObject.SetActive(true);
        customerInfo.name = info.name;
        customerInfo.designation = info.designation;
    }

    public void MoveToTargetPosition(Transform destination, bool finalDestination, MovingTo movingTo)
    {
        StopAllCoroutines();
        customerInfo.customerAnimator.SetFloat("Speed", 1);
        customerInfo.agent.SetDestination(destination.position);
        if (finalDestination)
        {
            if (movingTo == MovingTo.WelcomeDesk)
            {
                StartCoroutine(PostArivalAction(destination.position, () =>
                {
                    customerManager.welcomeDeskManager.SetCustomerInfoOnUi(this);
                    customerInfo.customerAnimator.SetFloat("Speed", 0);
                    //Play talking animation
                }));
            }
            else if (movingTo == MovingTo.ExitFromWelcomeDesk)
            {
                StartCoroutine(PostArivalAction(destination.position, () => { 
                    customerManager.ReturnCustomerPooledObject(this);
                    customerInfo.customerAnimator.SetFloat("Speed", 0);
                }));
            }
            else if (movingTo == MovingTo.ShootingRange)
            {
                StartCoroutine(PostArivalAction(destination.position, () => {
                    // transform.forward = Vector3.forward;
                    customerInfo.customerAnimator.SetFloat("Speed", 0);
                }));
            }
        }


    }

    private IEnumerator PostArivalAction(Vector3 destination, UnityAction postArivalAction)
    {
        yield return new WaitUntil(() => Vector2.Distance(transform.position, destination) <= 0.1f);
        postArivalAction.Invoke();
    }
    public void RejectionSequence(Transform finalPointAfterExit)
    {
        exitPointFromDesk = finalPointAfterExit;       
        customerInfo.customerAnimator.Play("Angry");//This animation calls MoveAlongWaypoints
    }
    public void StartRejectionSequence() {
        MoveToTargetPosition(exitPointFromDesk, true, MovingTo.ExitFromWelcomeDesk);
    }
}
