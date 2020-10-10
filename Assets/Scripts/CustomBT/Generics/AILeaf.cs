using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Graph;

namespace BT
{
    /// <summary>
    /// Mainly a shortcut for the graph context, without it we have to write 
    /// ((DefaultGraph)graph).gd everytime
    /// </summary>
    public class AILeaf : Leaf<int>
    {
        protected GenericDicionnary Gd
        {
            get
            {
                return ((DefaultGraph)graph).gd;
            }
        }
    }
}