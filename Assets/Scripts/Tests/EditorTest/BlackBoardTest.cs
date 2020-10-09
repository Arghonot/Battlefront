using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public enum VariableType
{
    Int,
    Bool,
    String
}

[System.Serializable]
public class BlackboardInt : BlackboardElement
{
    public int Intvalue;
    public bool Boolvalue;
    public string Stringvalue;
    public VariableType type;

    public override string ToString()
    {
        switch (type)
        {
            case VariableType.Int:
                return Intvalue.ToString();
                break;
            case VariableType.Bool:
                return Boolvalue.ToString();
                break;
            case VariableType.String:
                return Stringvalue;
                break;
        }

        return Intvalue.ToString();
    }
}

[System.Serializable]
public class BlackboardElement
{
    public string name;

    public int Intvalue;
    public bool Boolvalue;
    public string Stringvalue;
    public VariableType type;

    public override string ToString()
    {
        switch (type)
        {
            case VariableType.Int:
                return Intvalue.ToString();
                break;
            case VariableType.Bool:
                return Boolvalue.ToString();
                break;
            case VariableType.String:
                return Stringvalue;
                break;
        }

        return Intvalue.ToString();
    }
}

public class BlackBoardTest : MonoBehaviour
{
    public string test;

    public BlackboardElement[] elems = new BlackboardElement[15];
}
