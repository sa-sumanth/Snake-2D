using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SnakeScript : MonoBehaviour
{
    private SnakeScript next;
    private String hitAction;

    public void SetNext(SnakeScript newSnake)
    {
        next = newSnake;
    }

    public SnakeScript GetNext()
    {
        return next;
    }

    public void Remove()
    {
        Destroy(this.gameObject);
    }

    public String GetHitStatus()
    {
        if(hitAction == "Food")
        {
            hitAction = "";
            return "Food";
        }
        if(hitAction == "Wall")
        {
            hitAction = "";
            return "Wall";
        }
        return "";
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Food"))
        {
            hitAction = "Food";
            Destroy(col.gameObject);
        }
        if (col.CompareTag("Wall"))
        {
            hitAction = "Wall";
        }
        if (col.CompareTag("Snake"))
        {
            hitAction = "Snake";
        }
    }
}