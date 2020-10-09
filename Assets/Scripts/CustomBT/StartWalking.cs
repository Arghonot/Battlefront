﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using XNode;
using Graph;

namespace BT.CustomLeaves
{
    public class StartWalking : Leaf<int>
    {
        // TODO remove ?
        Graph.GenericDicionnary AIcontext
        {
            get
            {
                return ((DefaultGraph)graph).gd;
            }
        }

        public override object Run()
        {
            var agent = AIcontext.Get<NavMeshAgent>("agent");

            if (agent == null)
            {
                Debug.Log("Couldn't find any agent");
                return 0;
            }
            
            agent.isStopped = false;

            return 1;
        }
    }
}