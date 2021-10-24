using System;
using System.Collections.Generic;
using System.Text;

namespace StateMachine
{
    internal class InputSymbol
    {
        public InputSymbol(char value)
        {
            Value = value;
        }
        public char Value { get; }
    }
}
