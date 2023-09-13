using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UnpackAreaManager : MonoBehaviour
{
    public List<GameObject> disposedPackages;
    public int maxPackageCapacity;
    public bool isDisposing;
    public bool isUnpacking;
    public Transform dispossedParent;
    public Transform[] disposPoses;
    public float spawnedItemGap;
    public float unstackTime;
    public float unPackTime;

    public Transform[] movementPositions;
    public void DisposeAPackage(GameObject _package) {

        isDisposing = true;
        MovetoUnpackingStack(_package);
        //StartCoroutine(UnPack());
    }
    private void MovetoUnpackingStack(GameObject package)
    {
        int index = disposedPackages.Count;
        float stackHeight = Mathf.Floor(index / disposPoses.Length);
        Vector3 newPosition = disposPoses[index % disposPoses.Length].position + Vector3.up * spawnedItemGap * stackHeight;

        package.transform.DOJump(newPosition, 2, 1, unstackTime).OnComplete(() => {

            isDisposing = false;
            disposedPackages.Add(package);
            package.transform.parent = dispossedParent;
            package.transform.DOLocalRotate(Vector3.zero, unstackTime).OnComplete(()=> {
            CheckForUnpacking();

        });
            Reposition();
        });
    }



    private void CheckForUnpacking() {

        if (disposedPackages.Count > 0 && !isUnpacking) {

            isUnpacking = true;
            UnPack();
        
        }
    }
    public void UnPack()
    {

        GameObject _selectedPackage = disposedPackages[disposedPackages.Count - 1];
        disposedPackages.Remove(_selectedPackage);
        _selectedPackage.transform.DOJump(movementPositions[0].position, 2, 1, unPackTime * 0.3f).OnComplete(() => {

            _selectedPackage.transform.DOMove(movementPositions[1].position, unPackTime * 0.6f).OnComplete(() => {

                _selectedPackage.GetComponent<Crate>().Break();
                isUnpacking = false;
                Reposition();
                CheckForUnpacking();
            });
        });
    }

    public bool IsInDisposableCondition()
    {
        if (disposedPackages.Count >= maxPackageCapacity || isDisposing)
        {
            return false;
        }
        return true;
    }
    void Reposition()
    {
        if (disposedPackages.Count > 0)
        {
            for (int i = 0; i < disposedPackages.Count; i++)
            {
                float stackHeight = Mathf.Floor(i / disposPoses.Length);
                Vector3 newPosition = disposPoses[i % disposPoses.Length].position + Vector3.up * spawnedItemGap * stackHeight;
                disposedPackages[i].transform.position = newPosition;
            }
        }
    }
}
