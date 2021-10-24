using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace StateMachine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("*******************************************");
            Console.WriteLine("::: Problem Statement - write a program to search a substring within a text by using DFA.\n::: [Hint] You must first create a state machine dynamically based on given substring-\n::: and then traverse through the transition matrix to obtain the results");
            Console.WriteLine("\nWelcome to Deterministic State Machine");
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("Paste/Write the Text to perform search on, and press enter:");
            var searchText = Console.ReadLine();
            Console.WriteLine("Write a search string (case sensitive):");
            var searchString = Console.ReadLine();

            var tuple = GenerateStateMachine(searchString.Trim());
            var tableLines = GetTransitionGraph(tuple.states);

            Console.WriteLine("\n---------- Transition Table -----------");
            tableLines.ForEach(Console.WriteLine);

            var transitionMatrix = GetTransitionMatrix(tuple);

            var strExist = RunStateMachine(searchText,transitionMatrix,tuple);

            Console.WriteLine($"The search string exists in given text: {strExist.ToString().ToUpper()}");

        }


        private static (List<State> states, List<InputSymbol> inputSymbols) GenerateStateMachine(string searchString)
        {
            var inputChars = searchString.ToCharArray().ToList();
            var inputSymbols = new List<InputSymbol>();

            inputChars.ForEach(@char => inputSymbols.Add(new InputSymbol(@char)));

            var states = new List<State>();
            for (int i = 0; i <= inputSymbols.Count; i++)
            {
                var state = new State();
                state.Name = "S" + i;
                states.Add(state);
            }

            for (int i = 0; i < states.Count; i++)
            {
                for (int j = 0; j < inputSymbols.Count; j++)
                {
                    if (j == i)
                    {
                        states[i].Transition.Add(inputSymbols[j], states[j + 1]);
                    }
                    else
                    {
                        states[i].Transition.Add(inputSymbols[j], states.First());
                    }
                }
            }

            states.Last().IsLastState = true;

            return (states, inputSymbols);
        }
        //Dictionary<(State state,InputSymbol input),State>
        private static List<string> GetTransitionGraph(List<State> states)
        {
            var tableLines = new List<string>();
            foreach (var state in states)
            {
                var inputs = state.Transition.Keys;

                string transitionMap = "[" + state.Name + "] - ";

                foreach (var input in inputs)
                {
                    var transitionState = state.Transition[input];
                    transitionMap += $"  {input.Value} -> [{transitionState.Name}]";
                }
                tableLines.Add(transitionMap);
            }
            return tableLines;
        }

        private static Dictionary<(State state, char input), (State state,InputSymbol inputSymbol)>
            GetTransitionMatrix(
            (List<State> states,
            List<InputSymbol> inputSymbols) tuple)
        {

            var matrix = new Dictionary<(State state, char input), (State state, InputSymbol inputSymbol)>();

            for (int i = 0; i < tuple.inputSymbols.Count; i++)
            {
                var exists = tuple.states[i].Transition.TryGetValue(tuple.inputSymbols[i], out var nextState);

                if (exists)
                {
                    matrix.Add((tuple.states[i], tuple.inputSymbols[i].Value), (nextState,tuple.inputSymbols[i]));
                }
                else
                {
                    matrix.Add((tuple.states[i], tuple.inputSymbols[i].Value), (tuple.states.First(),null));
                }
            }

            return matrix;
        }

        private static bool RunStateMachine
            (
            string searchText,
            Dictionary<(State state, char input), (State state, InputSymbol inputSymbol)> matrix,
            (List<State> states, List<InputSymbol> inputSymbols) tuple
            )
        {
            var currentState = tuple.states.FirstOrDefault();
            var strExists = false;

          
            foreach (var ch in searchText)
            {
                var keyExists = matrix.ContainsKey((currentState,ch));


                if (keyExists)
                {
                    var inp = matrix[(currentState, ch)].inputSymbol;
                    var ext = currentState.Transition.TryGetValue(inp, out State nextState);

                    if (ext is true)
                    {
                        currentState = nextState;
                    }
                    if (currentState.IsLastState)
                    {
                        strExists = true;
                    }
                }
                else //Reset function
                {
                    currentState = tuple.states.FirstOrDefault();
                }
            }

            return strExists;
        }
    }
}