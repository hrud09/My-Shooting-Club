using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BulletTray : MonoBehaviour
{

    public bool trayFilled;
    public int bulletCapacity = 20;
    public int currentBulletCount;
    public List<GameObject> bulletsStored;
    public GameObject trayCover;
    public GameObject maxSign;
    public Transform bulletParent;
    BulletTrayManager trayManager;

    private void Start()
    {
        trayManager = GetComponentInParent<BulletTrayManager>();
    }
    public void StoreBullet(GameObject bullet)
    {
        if (!trayFilled)
        {
            if (maxSign) maxSign.SetActive(false);
            if (currentBulletCount < bulletCapacity)
            {
                bulletsStored.Add(bullet);
                currentBulletCount++;
                StartCoroutine(DeactivateRigidbodyOfBullet(bullet));
                if (currentBulletCount >= bulletCapacity)
                {
                    StartCoroutine(TrayFilledAction());
               
                }
            }
        }
    }
    private IEnumerator DeactivateRigidbodyOfBullet(GameObject bullet)
    {
        yield return new WaitForSeconds(0.7f);
        //bullet.GetComponent<Rigidbody>().useGravity = false;
        bullet.GetComponent<Rigidbody>().isKinematic = true;
    }
    private IEnumerator TrayFilledAction()
    {

        trayFilled = true;
        if (maxSign) maxSign.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        Vector3 initScale = trayCover.transform.localScale;
        trayCover.transform.localScale = Vector3.zero;
        trayCover.SetActive(true);
        trayCover.transform.DOScale(initScale, 0.2f);
        trayManager.TrackFreeBulletTrays(this);


    }
}
