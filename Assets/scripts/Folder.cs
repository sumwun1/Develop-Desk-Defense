﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Folder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Turn()
    {
        if (GetComponent<Supply>().GetReady() && (GetComponent<Supply>().GetDesk().GetHomework() != null))
        {
            GetComponent<Supply>().GetManager().TriggerFolder();
            GetComponent<Supply>().SetReady(false);
        }
    }
}
