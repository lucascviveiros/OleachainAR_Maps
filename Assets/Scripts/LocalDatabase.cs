using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalDatabase : MonoBehaviour
{
    private enum TypeInfo { Oliveira = 0, Arte = 1}

    private int ID;
    private string info;

    public void SetId(int i_id)
    {
        ID = i_id;
    }

    public int GetId()
    {
        return ID;
    }
}
