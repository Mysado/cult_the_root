namespace DefaultNamespace.Altar
{
    using System;
    using UnityEngine;

    [Serializable]
    public class CorpseSlot : MonoBehaviour
    {
        public bool Occupied { get; set;}
        public GameObject BodyInSlot { get; set; }
    }
}