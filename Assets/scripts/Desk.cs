﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desk : MonoBehaviour
{
    public bool occupied;
	public int x;
	public int y;
	public int index;
    public Manager _manager;
    //public Desks desks;
	Homework homework;

    private void OnMouseUp()
    {
        //Debug.Log("desk called click");

        if (!occupied && _manager.GetState() == "place" && _manager.GetSupplyId() >= 0)
        {
            _manager.Click();
            //Debug.Log(_manager.GetSupplyId());
            Instantiate(_manager.supplyTypes[_manager.GetSupplyId()], transform.position, transform.rotation).GetComponent<Supply>().Start0(this);
            _manager.UpdateA(-2);
            _manager.SetTutorial(4, 5);
            occupied = true;

            if(_manager.GetA() < 2 || transform.parent.GetComponent<Desks>().Occupied(true))
            {
                _manager.TogglePlace();
            }
            /*if(_manager.GetSupplyId() == 0)
            {
                supply.
            }
            else if(_manager.GetSupplyId() == 1)
            {
                //supply.GetComponent<Pencil>().Start0(this);
            }
            else if(_manager.GetSupplyId() == 2)
            {
                //supply.GetComponent<Pencil>().Start0(this);
            }
            else if(_manager.GetSupplyId() == 3)
            {
                //supply.GetComponent<Pencil>().Start0(this);
            }
            else
            {
                
            }*/
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //_manager = GameObject.Find("_manager").GetComponent<Manager>();
    }
	
	/*public int GetIndex(){
		return(index);
	}
	
	public void SetIndex(int input){
		index = input;
	}*/

    public Homework GetHomework()
	{
		return(homework);
	}
	
	public void SetHomework(Homework input)
	{
		homework = input;
	}
}
