using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    public WeaponInfo weaponInfo;
    public bool isInUse;
    public TMP_Text bulletCount;
    public void WeaponEmptyAction()
    {
        transform.DOJump(weaponInfo.weaponPositionTransform.position, 2, 1, 0.5f).OnComplete(()=> {

            isInUse = false;
          
            transform.parent = weaponInfo.weaponPositionTransform;
            transform.localScale = Vector3.one;
            transform.DOLocalRotate(Vector3.zero, 0.2f);
        });
    }

    public void UpdateBulletCount()
    {
        bulletCount.text = weaponInfo.currentBulletCount.ToString();
    }
}
