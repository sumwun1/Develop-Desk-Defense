using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Supply : MonoBehaviour
{
    public int id;
    bool ready;
    Desk desk;
    Manager _manager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Start0(Desk input)
    {
        desk = input;
        _manager = GameObject.Find("_manager").GetComponent<Manager>();
        //Debug.Log(_manager.GetA());

        if(id == 0)
        {
            GetComponent<Pencil>().Start0();
        }else if(id == 1)
        {
            
        }else if(id == 2)
        {
            GetComponent<Eraser>().Start0();
        }
        else if(id == 3)
        {

        }else
        {
            Debug.Log("supply ID was " + _manager.GetSupplyId());
        }
    }

    public bool Turn()
    {
        if (id == 0)
        {
            GetComponent<Pencil>().Turn();
        }
        else if (id == 1)
        {
            GetComponent<Bottle>().Turn();
        }
        else if (id == 2)
        {
            GetComponent<Eraser>().Turn();
        }
        else if (id == 3)
        {
            GetComponent<Folder>().Turn();
        }
        else
        {
            Debug.Log("supply ID was " + _manager.GetSupplyId());
        }

        return (true);
    }

    public void Sell()
    {
        //_manager = GameObject.Find("_manager").GetComponent<Manager>();

        if (_manager.GetState() == "select")
        {
            _manager.UpdateA(2);
            desk.occupied = false;
            _manager.SetTutorial(5, 6);
            Destroy(gameObject);
        }
    }

    private void OnMouseUp()
    {
        if (_manager.GetState() == "select")
        {
            _manager.Click();
            Sell();
        }
    }

    public void SetReady(bool input)
    {
        ready = input;
    }

    public bool GetReady()
    {
        return (ready);
    }

    public Manager GetManager()
    {
        return (_manager);
    }

    public Desk GetDesk()
    {
        return (desk);
    }
}
