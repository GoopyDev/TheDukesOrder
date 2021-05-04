using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowParent : MonoBehaviour
{
    public Transform parentData;

    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2 (parentData.position.x, parentData.position.y + 0.375f);
        transform.rotation = parentData.rotation;
        if (parentData == null)
        {
            Debug.Log("Se murio la hormiga!");
        }
    }
}
