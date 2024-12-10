using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeItem : MonoBehaviour
{
    [SerializeField] private List<GameObject> matObjects = new List<GameObject>();
    public List<GameObject> getMatObjects()
    {
        return matObjects;
    }
}
