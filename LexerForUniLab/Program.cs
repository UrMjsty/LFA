using System;
using System.Collections.Generic;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter a regular expression:");
        string regexString = Console.ReadLine();

        List<string> examples = GenerateExamples(regexString, 5);

        Console.WriteLine("Examples of strings that match the regular expression:");
        foreach (string example in examples)
        {
            Console.WriteLine(example);
        }
    }

    static List<string> GenerateExamples(string regexString, int count)
    {
        List<string> examples = new List<string>();

        Random random = new Random();
        for (int i = 0; i < count; i++)
        {
            StringBuilder exampleBuilder = new StringBuilder();
            int j = 0;
            while (j < regexString.Length)
            {
                char c = regexString[j];
                switch (c)
                {
                    case '\\':
                        // Skip escaped character
                        j++;
                        if (j < regexString.Length)
                            exampleBuilder.Append(regexString[j]);
                        break;
                    case '(':
                        // Process group
                        int closingIndexGroup = FindClosingBracketIndex(regexString, j);
                        if (closingIndexGroup != -1)
                        {
                            string subRegex = regexString.Substring(j + 1, closingIndexGroup - j - 1);
                            List<string> subExamples = GenerateExamples(subRegex, 1);
                            exampleBuilder.Append(subExamples[0]);
                            j = closingIndexGroup;
                        }
                        break;
                    case '+':
                        // Repeat previous character at least once
                        if (exampleBuilder.Length > 0)
                        {
                            char prevChar = exampleBuilder[exampleBuilder.Length - 1];
                            exampleBuilder.Append(prevChar);
                        }
                        break;
                    case '*':
                        // Repeat previous character zero or more times
                        if (exampleBuilder.Length > 0)
                        {
                            char prevChar = exampleBuilder[exampleBuilder.Length - 1];
                            if (random.Next(2) == 1)
                                exampleBuilder.Append(prevChar);
                        }
                        break;
                    case '?':
                        // Make previous character optional
                        if (exampleBuilder.Length > 0)
                        {
                            char prevChar = exampleBuilder[exampleBuilder.Length - 1];
                            if (random.Next(2) == 1)
                                exampleBuilder.Append(prevChar);
                        }
                        break;
                    case '|':
                        // OR operator, choose randomly between the options
                        string leftOption = "";
                        string rightOption = "";
                        for (int k = j - 1; k >= 0; k--)
                        {
                            if (regexString[k] == '(')
                            {
                                leftOption = regexString.Substring(k + 1, j - k - 1);
                                break;
                            }
                        }
                        int closingIndexOr = FindClosingBracketIndex(regexString, j);
                        if (closingIndexOr != -1)
                        {
                            rightOption = regexString.Substring(j + 1, closingIndexOr - j - 1);
                           // string chosenOption = random.Next(2) == 0 ? GetRandomChar(leftOption, random) : GetRandomChar(rightOption, random);
                           // exampleBuilder.Append(chosenOption);
                            j = closingIndexOr;
                        }
                        else
                        {
                            // Handle cases where '|' is at the beginning or end of the regex or followed by another '|'
                            exampleBuilder.Append('|');
                        }
                        break;
                    default:
                        exampleBuilder.Append(c);
                        break;
                }
                j++;
            }
            examples.Add(exampleBuilder.ToString());
        }

        return examples;
    }

    static int FindClosingBracketIndex(string regexString, int startIndex)
    {
        int level = 0;
        for (int i = startIndex; i < regexString.Length; i++)
        {
            char c = regexString[i];
            if (c == '(')
                level++;
            else if (c == ')')
                level--;

            if (level == 0)
                return i;
        }
        return -1;
    }

    static char GetRandomChar(string options, Random random)
    {
        int index = random.Next(options.Length);
        return options[index];
    }
}
    