using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.DebugLeaves
{
    public class SimpleLog : AILeaf
    {
        public string StringToLog;

        public int StateToReturn;

        public override object Run()
        {
            Debug.Log(StringToLog);

            return StateToReturn;
        }
    }
}