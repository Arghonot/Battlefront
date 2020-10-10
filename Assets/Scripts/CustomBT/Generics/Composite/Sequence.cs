using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Graph;

namespace BT.Composite
{
    public class Sequence : Branch<int>
    {
        [Input(
            ShowBackingValue.Always,
            ConnectionType.Override,
            TypeConstraint.Strict,
            dynamicPortList = true)]
        public List<int> Actions = new List<int>();

        public override object Run()
        {
            for (int i = 0; i < Actions.Count; i++)
            {
                if (GetInputValue<int>(
                        "Actions " + i.ToString()) == 0)
                {
                    return 0;
                }
            }

            return 1;
        }
    }
}