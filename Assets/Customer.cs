using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
public class Customer : MonoBehaviour
{
    public CustomerInfo customerInfo;
    public int currentWaypointIndex = 0;
    public Transform meshParent;

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
        float animationSpeed = 0;
        DOTween.To(() => animationSpeed, x => animationSpeed = x, 1, 0.2f)
            .OnUpdate(() => {
                customerInfo.customerAnimator.SetFloat("Speed", animationSpeed);
            });
        float dist = Vector2.Distance(transform.position, waypoint.position);
        transform.DOLookAt(waypoint.position, 0.4f);
        transform.DOMove(waypoint.position, dist / customerInfo.movementSpeed).OnComplete(()=> {


            if (endPointSet)
            {
                welcomeDeskManager.SetCustomerInfoOnUi(this);
                print(Time.time + "Name: " + customerInfo.name);
            }
            float _animationSpeed = 0;
            DOTween.To(() => _animationSpeed, x => _animationSpeed = x, 0, 0.4f)
                .OnUpdate(() => {
                    customerInfo.customerAnimator.SetFloat("Speed", _animationSpeed);
                   
                });

        });
    }


}
