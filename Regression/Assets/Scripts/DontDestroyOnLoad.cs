using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyOnLoad : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> allObjs = new List<GameObject>();
    private void Awake()
    {
        

        GameObject[] instances = GameObject.FindGameObjectsWithTag("MusicHandler");
        if (instances.Length > 1)
        {
            Destroy(this.gameObject);
        }
        for (int i = 0; i < instances.Length; i++)
        {
            allObjs.Add(instances[i]);
        }
        DontDestroyOnLoad(this.gameObject);
        
    }
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            Destroy(this.gameObject);
        }
    }
}
