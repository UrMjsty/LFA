using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class GrammarToNFAConverter
{


    static void Main()
    {
        // Define a context-free grammar
        Grammar grammar = new Grammar();
        grammar.AddRule("S", "aS");
        grammar.AddRule("S", "bD");
        grammar.AddRule("S", "fR");
        grammar.AddRule("D", "cD");
        grammar.AddRule("D", "dR");
        grammar.AddRule("R", "bR");
        grammar.AddRule("R", "f");
        grammar.AddRule("D", "d");

        var nfa = new NFA();
        // States
        nfa.AddState(0, isAccepting: false); // Initial state
        nfa.AddState(1, isAccepting: false); // Accepting state
        nfa.AddState(2, isAccepting: false); // Accepting state
        nfa.AddState(3, isAccepting: true); // Accepting state

        // Transitions 
        nfa.AddTransition(0, 'a', 0);
        nfa.AddTransition(0, 'b', 1);
        nfa.AddTransition(0, 'f', 2);
        nfa.AddTransition(1, 'c', 1);
        nfa.AddTransition(1, 'd', 2);
        nfa.AddTransition(1, 'd', 3);
        nfa.AddTransition(2, 'b', 2);
        nfa.AddTransition(2, 'f', 3);


        // Check if a string is accepted by the NFA
        for (int i = 0; i < 5; i++)
        {
            string test = grammar.GenerateString();
            Console.WriteLine(test);
            bool isAccepted = nfa.Accepts(test);
            Console.WriteLine($"String '{test}' is {(isAccepted ? "accepted" : "rejected")} by the NFA.");
        }
    }

}

  

class GrammarCringe
{
    private Dictionary<char, List<string>> rules = new Dictionary<char, List<string>>();
    private char startSymbol;

    public void AddRule(char nonTerminal, string production)
    {
        if (!rules.ContainsKey(nonTerminal))
        {
            rules[nonTerminal] = new List<string>();
        }
        rules[nonTerminal].Add(production);
    }

    public void SetStartSymbol(char startSymbol)
    {
        this.startSymbol = startSymbol;
    }

    public char GetStartSymbol()
    {
        return startSymbol;
    }

    public Dictionary<char, List<string>> GetRules()
    {
        return rules;
    }

    public IEnumerable<char> GetNonTerminals()
    {
        return rules.Keys;
    }

    public IEnumerable<char> GetTerminals()
    {
        return rules.Values.SelectMany(p => p).SelectMany(p => p).Where(c => !char.IsUpper(c)).Distinct();
    }
}
class Grammar
{
    private Dictionary<string, HashSet<string>> productionRules;
    private Random random;

    public Grammar()
    {
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
}


