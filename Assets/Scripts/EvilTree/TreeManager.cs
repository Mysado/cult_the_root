using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    [SerializeField] private GameObject treeParticle;
    [SerializeField] private List<GameObject> treeMeshes;
    [SerializeField] private int maxLevel = 4;
    public int TreeLevel = 0;
    public event Action OnMaxLevelReached;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (var mesh in treeMeshes)
        {
            mesh.SetActive(false);
        }
        treeMeshes[TreeLevel].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LevelUpTree()
    {
        TreeLevel++;
        foreach (var mesh in treeMeshes)
        {
            mesh.SetActive(false);
        }

        treeMeshes[TreeLevel].SetActive(true);
        var particle = Instantiate(treeParticle, treeMeshes[TreeLevel].transform);
        Destroy(particle,5);
        if(TreeLevel >= maxLevel)
            OnMaxLevelReached?.Invoke();
    }

    public void DelayedLevel()
    {
        var y = 0;
        DOTween.To(()=> y , x=> y  = x, 52, 5f).onComplete = LevelUpTree;
    }
}
