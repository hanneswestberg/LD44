using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gladiator : MonoBehaviour
{
    public UnitData Data { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetUnitData(UnitData data) {
        Data = data;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
