using System.ComponentModel.Design;
using System.Text;

namespace LFALab;

public class Lab2
{

    static void Main()
    {
        // Define a context-free grammar
        Grammar grammar = new Grammar(new List<char>{'a','b'}, new List<char>{'S', 'A', 'B', 'C'} );
        grammar.AddRule("S", "aA");
        grammar.AddRule("A", "bA");
        grammar.AddRule("A", "aB");
        grammar.AddRule("B", "bB");
        grammar.AddRule("B", "bC");
        grammar.AddRule("C", "Aa");
        grammar.AddRule("C", "b");
     /*   Grammar grammar3 = new Grammar(new List<char>{'a', 'b'}, new List<char>{'S', 'X', 'Y'} );
        grammar3.AddRule("S", "");
        grammar3.AddRule("S", "X");
        grammar3.AddRule("X", "a");
        grammar3.AddRule("X", "aY");
        grammar3.AddRule("Y", "b");
        Grammar grammar2 = new Grammar(new List<char>{'a', 'b', 'c'}, new List<char>{'S', 'X'} );
        grammar2.AddRule("S", "Xa");
        grammar2.AddRule("X", "aX");
        grammar2.AddRule("X", "a");
        grammar2.AddRule("X", "abc");
        grammar2.AddRule("X", "");
        Grammar grammar1 = new Grammar(new List<char>{'a', 'b', 'c'}, new List<char>{'S', 'A', 'B'} );
        grammar1.AddRule("S", "AB");
        grammar1.AddRule("AB", "aX");
        grammar1.AddRule("X", "a");
        grammar1.AddRule("X", "abc");
        grammar1.AddRule("X", "");*/
      

        var nfa = new NFA();
        // States
        nfa.AddState(0, isAccepting: false); // Initial state
        nfa.AddState(1, isAccepting: false); // Accepting state
        nfa.AddState(2, isAccepting: false); // Accepting state
        nfa.AddState(3, isAccepting: false); // Accepting state
        nfa.AddState(4, isAccepting: true); // Accepting state

        // Transitions 
        nfa.AddTransition(0, 'a', 1);
        nfa.AddTransition(1, 'b', 1);
        nfa.AddTransition(1, 'a', 2);
        nfa.AddTransition(2, 'b', 2);
        nfa.AddTransition(2, 'b', 3);
        nfa.AddTransition(3, 'b', 4);
        nfa.AddTransition(3, 'a', 1);
        
       
        var dfa = new NFA();
        // States
            dfa.AddState(0, isAccepting: false); // Initial state
            dfa.AddState(1, isAccepting: false); // Accepting state
            dfa.AddState(2, isAccepting: false); // Accepting state
            dfa.AddState(3, isAccepting: false); // Accepting state
            dfa.AddState(4, isAccepting: true); // Accepting state
            //dfa.AddState(5, isAccepting: true); // Accepting state

            // Transitions 
            dfa.AddTransition(0, 'a', 1);
            dfa.AddTransition(1, 'b', 1);
            dfa.AddTransition(1, 'a', 2);
            dfa.AddTransition(2, 'b', 3);
            dfa.AddTransition(3, 'b', 4);
            dfa.AddTransition(3, 'a', 1);
            dfa.AddTransition(4, 'a', 1);
            dfa.AddTransition(4, 'b', 4);

         Grammar gram = nfa.CreateGrammar();
       /*  Grammar gram0 = nfa.CreateGrammar();
         Grammar gram1 = nfa.CreateGrammar();
         Grammar gram2 = nfa.CreateGrammar();
         Grammar gram3 = nfa.CreateGrammar();
        // Check if a string is accepted by the NFA
        */
        for (int i = 0; i < 5; i++)
        {
            string test = gram.GenerateString();
            Console.WriteLine(test);
            bool isAccepted = dfa.Accepts(test);
            Console.WriteLine($"String '{test}' is {(isAccepted ? "accepted" : "rejected")} by the NFA.");
        }
        
        Console.WriteLine(grammar.GetChomskyType());
      /*  Console.WriteLine(grammar1.GetChomskyType());
        Console.WriteLine(grammar2.GetChomskyType());
        Console.WriteLine(grammar3.GetChomskyType());*/
        Console.WriteLine(dfa.IsDeterministic());
        
    }

    public class Grammar
    {
        private Dictionary<string, HashSet<string>> productionRules;
        private Random random;
        private List<Char> terminals;
        private List<Char> nonterminals;

        public Grammar(List<Char> term, List<Char> nonterm)
        {
            this.terminals = term;
            this.nonterminals = nonterm;
            productionRules = new Dictionary<string, HashSet<string>>();
            random = new Random();
        }


        public void AddRule(string nonTerminal, string production)
        {
            if (!productionRules.ContainsKey(nonTerminal))
            {
                productionRules[nonTerminal] = new HashSet<string>();
            }

            productionRules[nonTerminal].Add(production);
        }

        public string GenerateString()
        {
            return GenerateString("S");
        }

        private string GenerateString(string symbol)
        {
            StringBuilder result = new StringBuilder();

            if (productionRules.ContainsKey(symbol))
            {
                var possibleProductions = new List<string>(productionRules[symbol]);
                int selectedProductionIndex = random.Next(possibleProductions.Count);
                string selectedProduction = possibleProductions[selectedProductionIndex];

                foreach (char c in selectedProduction)
                {
                    if (char.IsUpper(c)) // Non-terminal symbol
                    {
                        result.Append(GenerateString(c.ToString()));
                    }
                    else
                    {
                        result.Append(c);
                    }
                }
            }

            return result.ToString();
        }

        public int GetChomskyType()
        {
            var list = new List<Char>();
            foreach (var hash in productionRules.Values)
            {
                foreach (var VARIABLE in hash)
                {
                    list.Add(IsThird(VARIABLE));
                }
            }
            var left = new List<Char>() { 'l', 'b' };
            var right = new List<Char>() { 'r', 'b' };
          //  if (productionRules.Values.All(set => (set.All(s => left.Contains(IsThird(s))) || set.All(s => right.Contains(IsThird(s))))))
          if(!(list.Contains('l') && list.Contains('r')))
                    return 3;
            if (productionRules.Keys.All(key => nonterminals.Contains(key[0])))
                return 2;
            if (productionRules.All(kv => kv.Value.All(str => str.Length < kv.Key.Length)))
                return 1;
            if (productionRules.Keys.All(key => !string.IsNullOrEmpty(key)))
                return 0;
            return -1;
        }

        private char IsThird(string s)
        {
            if (s.Length == 0)
                return 'n';
            // Check if str contains strictly either one char from terminals
            if (s.Length == 1 && terminals.Contains(s[0]))
            {
                return 'b';
            }

            // Check if str contains one char from terminals in the left spot and one char from nonterminals in the right spot
            if (s.Length == 2)
                if (terminals.Contains(s[0]) && nonterminals.Contains(s[1]))
                    return 'l';
                else if (terminals.Contains(s[1]) && nonterminals.Contains(s[0]))
                    return 'r';
            
            return 'n';
        }
    }

    class NFATransition
    {
        public char Symbol { get; set; }
        public int TargetState { get; set; }
    }

    class NFAState
    {
        public int StateId { get; set; }
        public List<NFATransition> Transitions { get; } = new List<NFATransition>();
        public bool IsAccepting { get; set; }
    }

    class NFA
    {
        private List<NFAState> states = new List<NFAState>(10);

        public void AddState(int stateId, bool isAccepting = false)
        {
            states.Add(new NFAState { StateId = stateId, IsAccepting = isAccepting });
        }

        public void AddTransition(int sourceState, char symbol, int targetState)
        {
            var transition = new NFATransition { Symbol = symbol, TargetState = targetState };
            states[sourceState].Transitions.Add(transition);
        }

        public bool Accepts(string input)
        {
            var currentStates = new HashSet<int> { 0 }; // Start with the initial state
            foreach (char symbol in input)
            {
                var nextStates = new HashSet<int>();
                foreach (var state in currentStates)
                {
                    foreach (var transition in states[state].Transitions)
                    {
                        if (transition.Symbol == symbol)
                        {
                            nextStates.Add(transition.TargetState);
                        }
                    }
                }

                currentStates = nextStates;
            }

            // Check if any of the current states is an accepting state
            return currentStates.Any(state => states[state].IsAccepting);
        }

        public bool IsDeterministic()
        {
           // return states.All(state => state.Transitions.Count == 1);
           return states.All((state => state.Transitions.GroupBy(t => t.Symbol).All(g => g.Count() == 1)));
        }

        public NFA CreateDfa()
        {
            var dfa = new NFA();
            dfa.AddState(0, states[0].IsAccepting);
            var currentState = dfa.states[0];
            List<NFAState> newStates = new List<NFAState>();
            var istrue = true;
            var index = 0;
            while (istrue)
            {
                istrue = false;

                if (currentState.Transitions.Count == 1)
                {
                 //   newStates = new List<NFAState>() { states[currentState.Transitions[0].TargetState] };
                    dfa.AddState(index, false);
                    dfa.AddTransition(index, currentState.Transitions[0].Symbol, currentState.Transitions[0].TargetState);
                    istrue = true;
                }
                else
                {
                    foreach (var tr in states[0].Transitions)
                    {

                    }
                }
                currentState = newStates[0];
            }

            return dfa;
        }

        public Grammar CreateGrammar()
        {
            var terms = states.SelectMany(s => s.Transitions.Select(t => t.Symbol).Select(symbol =>symbol)).Distinct().ToList();

            List<char> nonterms = new List<char>(){'S'};
            for (int i = 0; i < states.Count() - 1; i++)
            {
                nonterms.Add((char)((char)65+i));
            }
            Grammar grammar = new Grammar(terms, nonterms);
            foreach (var state in states)
            {
              //  if(!state.Transitions.Any())
                 //   grammar.AddRule(nonterms[state.StateId].ToString(), "");
                foreach (var trans in state.Transitions)
                {
                    grammar.AddRule(nonterms[state.StateId].ToString(), trans.Symbol.ToString() + nonterms[trans.TargetState].ToString());
                }
            } 
            return grammar;
        }
}
}