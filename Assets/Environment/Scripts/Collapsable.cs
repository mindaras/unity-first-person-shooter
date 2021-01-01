using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collapsable : MonoBehaviour
{
    public GameObject collapsableObject;

    public void Collapse()
    {
        Instantiate(collapsableObject);
        Destroy(gameObject);
    }
}
