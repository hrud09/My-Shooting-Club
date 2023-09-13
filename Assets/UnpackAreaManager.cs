using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UnpackAreaManager : MonoBehaviour
{
    public List<GameObject> disposedPackages;
    public int maxPackageCapacity;
    public bool isDisposing;
    public Transform dispossedParent;
    public Transform[] disposPoses;
    public float spawnedItemGap;
    public float unstackTime;
    void Start()
    {
        //DisposeAPackage();
        //StartCoroutine(UnPack());
    }

    public void DisposeAPackage(GameObject _package) {

        isDisposing = true;
        MovePackages(_package);


        //StartCoroutine(UnPack());
    }

    public IEnumerator UnPack()
    {

        yield return new WaitUntil(() => disposedPackages.Count > 0 && !isDisposing);
        GameObject _selectedPackage = disposedPackages[disposedPackages.Count - 1];
        disposedPackages.Remove(_selectedPackage);
        _selectedPackage.transform.parent = dispossedParent;
        Reposition();
    }

    private void MovePackages(GameObject package)
    {
        int index = disposedPackages.Count;
        float stackHeight = Mathf.Floor(index / disposPoses.Length);
        Vector3 newPosition = disposPoses[index % disposPoses.Length].position + Vector3.up * spawnedItemGap * stackHeight;
      
        package.transform.DOJump(newPosition, 3, 1, unstackTime).OnComplete(()=>{

            isDisposing = false;
            disposedPackages.Add(package);
            package.transform.parent = dispossedParent;
            package.transform.DOLocalRotate(Vector3.zero, unstackTime);
            Reposition();
        });
    }
    void Reposition()
    {
        if (disposedPackages.Count > 0)
        {
            for (int i = 0; i < disposedPackages.Count; i++)
            {
                float stackHeight = Mathf.Floor(i / disposPoses.Length);
                Vector3 newPosition = disposPoses[i % disposPoses.Length].position + Vector3.up * spawnedItemGap * stackHeight;
                disposedPackages[i].transform.position = Vector3.Lerp(disposedPackages[i].transform.position, newPosition, Time.deltaTime * 10);
            }
        }
    }
    public bool IsInDisposableCondition()
    {
        if (disposedPackages.Count >= maxPackageCapacity || isDisposing)
        {
            return false;
        }
        return true;
    }
}
