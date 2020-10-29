using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {

        GameObject[] instances = GameObject.FindGameObjectsWithTag("Handlers");
        if (instances.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        
    }
}
