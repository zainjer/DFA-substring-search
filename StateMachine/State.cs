using System;
using System.Collections.Generic;
using System.Text;

namespace StateMachine
{
    internal class State
    {
        public string Name { get; set; }
        public Dictionary<InputSymbol,State> Transition { get; set; }  = new Dictionary<InputSymbol,State>();
        public bool IsLastState { get; set; } = false;
    }
}
