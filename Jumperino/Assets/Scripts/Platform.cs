using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Spawner;

public class Platform : MonoBehaviour
{
    public bool first;
    public bool activated;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        UpdatePlatformsList();
    }
}
