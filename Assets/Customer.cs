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
    private Tween animationTween;
    public ShootingRange shootingRange;
    public Weapon currentWeapon;

    public GameObject headphones;
    //public int sofaId;
    public MovingTo moving;
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

    public void MoveToTargetPosition(Transform destination, MovingTo movingTo)
    {
        StopAllCoroutines();
        if (animationTween.IsActive()) animationTween.Kill();
        AnimationControll(1);
        customerInfo.agent.SetDestination(destination.position);

        moving = movingTo;
        if (movingTo == MovingTo.CustomerLine)
        {
            StartCoroutine(PostArivalAction(destination.position, () => {

                Vector3 lookAtPos = customerManager.welcomeDeskManager.transform.position;
                lookAtPos.y = transform.position.y;
                transform.DOLookAt(lookAtPos, 0.1f);
            }));
        }
        else if (movingTo == MovingTo.WelcomeDesk)
        {
            StartCoroutine(PostArivalAction(destination.position, () =>
            {
                customerManager.welcomeDeskManager.SetCustomerInfoOnUi(this);
                Vector3 lookAtPos = customerManager.welcomeDeskManager.transform.position;
                lookAtPos.y = transform.position.y;
                transform.DOLookAt(lookAtPos, 0.1f);
            }));
        }
        else if (movingTo == MovingTo.ExitFromWelcomeDesk)
        {
            StartCoroutine(PostArivalAction(destination.position, () =>
            {
                customerManager.ReturnCustomerPooledObject(this);

            }));
        }
        else if (movingTo == MovingTo.Sofa)
        {
            StartCoroutine(PostArivalAction(destination.position, () => {

                //Sit

                customerManager.customersInSofa.Add(this);
            }));
        }
        else if (movingTo == MovingTo.ShootingRange)
        {
            print("Shoot");
            //shootingRange = customerManager.shootingAreaManager.GetFreeShootinRange();
            StartCoroutine(PostArivalAction(destination.position, () =>
            {
                print("Shoot");
                Shoot();
            }));
        }
   
        else if (movingTo == MovingTo.ExitFromShootingRange)
        {

           shootingRange.isOccupied = false;
           shootingRange.outOfAmmoSign.SetActive(true);
            customerInfo.isInsideShootingRange = false;
            customerInfo.hasWeapon = false;
            currentWeapon = null;
            shootingRange = null;
            StartCoroutine(PostArivalAction(destination.position, () => {

                customerInfo.shootingIsOver = false;
                customerManager.ReturnCustomerPooledObject(this);
              
            }));
        }
 
    }

    public void Shoot()
    {
        print("shoot");
        customerInfo.isInsideShootingRange = true;
        // transform.DOLocalRotate(Vector3.back * 90, 0.3f);
        customerInfo.agent.enabled = false;
        if (!customerInfo.hasWeapon)
        {
            if (!shootingRange.weaponManager.HasLoadedGun()) return;
            Weapon weapon = shootingRange.weaponManager.weapon;

            headphones.SetActive(true);
            customerInfo.hasWeapon = true;
            currentWeapon = weapon;
            weapon.isInUse = true;

            Vector3 lookAtPos = Vector3.zero;
            lookAtPos.y = shootingRange.yRotationWhileShooting;
            //lookAtPos.y = transform.position.y;
            transform.DORotate(lookAtPos, 0.2f).OnComplete(() => {

                weapon.gameObject.transform.DOJump(customerInfo.weaponPos.position, 2, 1, 0.2f).OnComplete(() => {

                    weapon.gameObject.transform.parent = customerInfo.weaponPos;
                    weapon.gameObject.transform.localRotation = Quaternion.identity;
                    weapon.gameObject.transform.localPosition = Vector3.zero;
                    weapon.gameObject.transform.localScale = Vector3.one;
                    if (!customerInfo.isShooting && !customerInfo.shootingIsOver) StartShooting();
                });

            });
        }
    }

    private void StartShooting()
    {
        customerInfo.isShooting = true;
        customerInfo.customerAnimator.SetBool("IsShooting", true);
        customerInfo.customerAnimator.Play("Shoot");
    }
    public void CheckWeaponAndShootingRound()
    {
        if (customerInfo.shootingIsOver) return;
        EconomyManager.UpdateEconomy(1);
        currentWeapon.weaponInfo.currentBulletCount --;
        if (currentWeapon.weaponInfo.currentBulletCount == 0)
        {

            shootingRange.isOccupied = false;
            headphones.SetActive(false);
            customerInfo.agent.enabled = true;
            customerInfo.shootingIsOver = true;
            customerInfo.isShooting = false;
            currentWeapon.WeaponEmptyAction();
            customerInfo.customerAnimator.SetBool("IsShooting", false);
            MoveToTargetPosition(customerManager.firstWaypoint, MovingTo.ExitFromShootingRange);
            customerManager.SendNextCustomerToShoot();
            shootingRange = null;
        }
    }
 /*   private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 12)
        {
            customerInfo.isInsideShootingRange = false;
            customerInfo.hasWeapon = false;
            currentWeapon = null;
            shootingRange = null;

        }
    }*/

    public void AnimationControll(float speed)
    {
        float _speed = 0;
        animationTween = DOTween.To(() => _speed, x => _speed = x, speed, 0.4f)
            .OnUpdate(() => {
                customerInfo.customerAnimator.SetFloat("Speed", _speed);
            });
    }
    private IEnumerator PostArivalAction(Vector3 destination, UnityAction postArivalAction
    )
    {
        yield return new WaitUntil(() => Vector3.Distance(transform.position, destination) <= 0.1f);
        AnimationControll(0);
        yield return new WaitUntil(() => Vector3.Distance(transform.position, destination) <= 0.5f);
        postArivalAction.Invoke();
       
    }
    public void RejectionSequence(Transform finalPointAfterExit)
    {
        exitPointFromDesk = finalPointAfterExit;       
        customerInfo.customerAnimator.Play("Angry");//This animation calls MoveAlongWaypoints
    }
    public void StartRejectionSequence() {
        MoveToTargetPosition(exitPointFromDesk, MovingTo.ExitFromWelcomeDesk);
    }
}
