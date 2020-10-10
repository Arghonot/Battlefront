using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Graph;

namespace BT.Decorator
{
    public class Invert : Branch<int>
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public int Input;

        public override object Run()
        {
            return GetInputValue<int>("Input") == 0 ? 1 : 0;
        }
    }
}