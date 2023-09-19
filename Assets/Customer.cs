using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using System.Collections.Generic;

public class Customer : MonoBehaviour
{
    public CustomerInfo customerInfo;
    public int currentWaypointIndex = 0;
    public Transform meshParent;
    public CustomerManager customerManager;
    private List<Tween> tweens = new List<Tween>();
    public void InitializeCustomer(CustomerInfo info)
    {
        for (int i = 0; i < meshParent.childCount; i++)
        {
            meshParent.GetChild(i).gameObject.SetActive(false);
        }
        meshParent.GetChild(Random.Range(0, meshParent.childCount)).gameObject.SetActive(true);
        customerInfo.name = info.name;
        customerInfo.movementSpeed = info.movementSpeed;
        customerInfo.designation = info.designation;
    }
    public void MoveToNextTargetPoint(Transform waypoint, WelcomeDeskManager welcomeDeskManager, bool endPointSet = false)
    {
        // Kill any existing tweens
        foreach (var tween in tweens)
        {
            tween.Kill();
        }
        tweens.Clear(); // Clear the list

        float animationSpeed = 0;
        Tween speedTween = DOTween.To(() => animationSpeed, x => animationSpeed = x, 1, 0.2f)
            .OnUpdate(() => {
                customerInfo.customerAnimator.SetFloat("Speed", animationSpeed);
            });
        tweens.Add(speedTween);

        float dist = Vector2.Distance(transform.position, waypoint.position);
        Tween lookAtTween = transform.DOLookAt(waypoint.position, 0.3f);
        tweens.Add(lookAtTween);

        Tween moveTween = transform.DOMove(waypoint.position, dist / customerInfo.movementSpeed).OnComplete(() => {
            if (endPointSet)
            {
                welcomeDeskManager.SetCustomerInfoOnUi(this);  
            }
            float _animationSpeed = 1;
            Tween resetSpeedTween = DOTween.To(() => _animationSpeed, x => _animationSpeed = x, 0, 0.5f)
                .OnUpdate(() => {
                    customerInfo.customerAnimator.SetFloat("Speed", _animationSpeed);
                });
            tweens.Add(resetSpeedTween);
        });
        tweens.Add(moveTween);
    }

    public void MoveAlongWaypoints(Transform[] waypoints)
    {
        if (waypoints.Length == 0)
        {
            Debug.LogWarning("No waypoints provided.");
            return;
        }

        customerInfo.customerAnimator.SetFloat("Speed", 1);
        Sequence moveSequence = DOTween.Sequence();
        customerManager.RemoveCustomerFromLine(this);
        for (int i = 0; i < waypoints.Length; i++)
        {
            float dist = Vector3.Distance(transform.position, waypoints[i].position);

            // Calculate the direction to the waypoint for rotation
            Vector3 lookDirection = (waypoints[i].position - transform.position).normalized;
            
            moveSequence.Append(transform.DOLookAt(waypoints[i].position, 0.4f));

            moveSequence.Append(transform.DOMove(waypoints[i].position, dist / customerInfo.movementSpeed).SetEase(Ease.Linear).OnComplete(() =>
            {
               

                if (i == waypoints.Length - 1)
                {
                    Debug.Log(Time.time + "Name: " + customerInfo.name);
                }
            }));

            moveSequence.AppendCallback(() =>
            {
                // Rotate towards the waypoint direction
                transform.forward = lookDirection;
            });
        }
    }


}
