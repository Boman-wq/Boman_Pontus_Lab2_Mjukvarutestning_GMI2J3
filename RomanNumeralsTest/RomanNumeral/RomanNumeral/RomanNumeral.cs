namespace KeesTalksTech.Utilities.Latin.Numerals;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

public class RomanNumeral
{
    #region Constants

    //0
    public const string NULLA = "NULLA";

    //values - a readonly dictionary where the numerals are the keys to values
    public static readonly IReadOnlyDictionary<string, int> VALUES = new ReadOnlyDictionary<string, int>(new Dictionary<string, int>
    {
        {"I",       1 },
        {"IV",      4 },
        {"V",       5 },
        {"IX",      9 },
        {"X",       10 },
        {"XIIX",    18 },
        {"IIXX",    18 },
        {"XL",      40 },
        {"L",       50 },
        {"XC",      90 },
        {"C",       100 },
        {"CD",      400 },
        {"D",       500 },
        {"CM",      900 },
        {"M",       1000 },

        //alternatives from Middle Ages and Renaissance
        {"O",       11 },
        {"F",       40 },
        {"P",       400 },
        {"G",       400 },
        {"Q",       500 }
    });

    //all the options that are used for parsing, in their order of value
    public static readonly string[] NUMERAL_OPTIONS =
    {
        "M", "CM", "D", "Q", "CD", "P", "G", "C", "XC", "L", "F", "XL", "IIXX", "XIIX", "O", "X", "IX", "V", "IV", "I"
    };

    //subtractive notation uses these numerals
    public static readonly string[] SUBTRACTIVE_NOTATION =
    {
        "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I"
    };

    //the addative notation uses these numerals
    public static readonly string[] ADDITIVE_NOTATION =
    {
        "M", "D", "C", "L", "X", "V", "I"
    };

    #endregion

    public const string NumberLowerThanZeroMessage = "Number out of range ( must be 1..49999)";
    public const string StringContainsNonRomanNumeralsMessage = "String can only contain RomanNumerals";
    public const string StingContainsToManyRepetativeNumerals = "String contains to many repetative numerals (I,X,C can't repeat more than 3 times. V,L,D are never repeated)";
    public const string StringInputToRomanMessage = "Input can't be of type string";

    private readonly int _number;

    public int Number => _number;

    public static string[] NonRomanNumerals = new[] { "A", "B", "E", "F", "G", "H", "J", "K", "N", "O", "P", "Q", "R", "S", "T", "U", "W", "Y", "Z" };



    public RomanNumeral(string number)
    {
        try
        {
            int num = Int32.Parse(number);
            _number = num;
        }
        catch (FormatException)
        {
            throw new ArgumentException(String.Format("Type: {0}", number), StringInputToRomanMessage);
        }

    }

    public RomanNumeral(int number)
    {
        if (number < 0)
            throw new ArgumentOutOfRangeException("number", number, NumberLowerThanZeroMessage);

        if (number > 4000)
            throw new ArgumentOutOfRangeException("number", number, NumberLowerThanZeroMessage);

        _number = number;
    }

    public override string ToString() => ToString(RomanNumeralNotation.Substractive);

    public string ToString(RomanNumeralNotation notation)
    {
        if (Number == 0)
        {
            return NULLA;
        }

        //check notation for right set of characters
        string[] numerals;
        switch (notation)
        {
            case RomanNumeralNotation.Additive:
                numerals = ADDITIVE_NOTATION;
                break;
            default:
                numerals = SUBTRACTIVE_NOTATION;
                break;
        }

        var resultRomanNumeral = "";

        //start with the M and iterate back
        var position = 0;

        //substract till the number is 0
        var value = Number;

        do
        {
            var numeral = numerals[position];
            var numeralValue = VALUES[numeral];

            //check if the value is in the number
            if (value >= numeralValue)
            {
                //substract from the value
                value -= numeralValue;

                //add the numeral to the string
                resultRomanNumeral += numeral;

                //multiple numeral? advance position because things like 'IVIV' does not exist
                bool isMultipleNumeral = numeral.Length > 1;
                if (isMultipleNumeral)
                {
                    position++;
                }
                continue;
            }
            position++;
        }
        while (value != 0);

        return resultRomanNumeral;
    }

    private static bool IsNumeral(string str)
    {
        if (String.IsNullOrEmpty(str))
        {
            return false;
        }

        return Parse(str) != null;
    }

    public static RomanNumeral Parse(string str)
    {
        if (String.IsNullOrEmpty(str))
            return new RomanNumeral(0);

        var strToRead = str.ToUpper();

        //nulla? means nothing 0 wasn't invented yet ;-)
        if (strToRead == NULLA)
            return new RomanNumeral(0);

        //if ends in J -> replace it to I (used in medicine)
        if (strToRead.EndsWith("J"))
            strToRead = strToRead.Substring(0, strToRead.Length - 1) + "I";

        //if a U is present, assume a V
        strToRead = strToRead.Replace("U", "V");

        //Check if all letters is Roman numerals
        if (NonRomanNumerals.Any(strToRead.Contains))
            throw new ArgumentException(String.Format("String {0}", strToRead), StringContainsNonRomanNumeralsMessage);

        //check simple numbers directly in dictionary
        if (VALUES.ContainsKey(str))
            return new RomanNumeral(VALUES[str]);


        //Check for to many repeated values: I,X,C can't repeat more than 3 times. V,L,D are never repeated
        var matches = Regex.Matches(strToRead, @"(.)\1+");
        for (int i = 0; i < matches.Count; i++)
        {
            if (matches[i].Value[0] == 'V' || matches[i].Value[0] == 'L' || matches[i].Value[0] == 'D')
                if (matches[i].Length > 1)
                    throw new ArgumentException(String.Format("String {0}", strToRead), StingContainsToManyRepetativeNumerals);

            if (matches[i].Length > 3)
                throw new ArgumentException(String.Format("String {0}", strToRead), StingContainsToManyRepetativeNumerals);
        }


        var resultNumber = 0;

        //start with M and iterate through the options
        var numeralOptionPointer = 0;

        //continue to read until the string is empty or the numeral options pointer has exceeded all options
        while (!String.IsNullOrEmpty(strToRead) && numeralOptionPointer < NUMERAL_OPTIONS.Length)
        {
            //select the current numeral
            var numeral = NUMERAL_OPTIONS[numeralOptionPointer];

            //read numeral -> check if the numeral is used, otherwise move on to the next one
            if (!strToRead.StartsWith(numeral))
            {
                numeralOptionPointer++;
                continue;
            }

            //add the vaue of the found numeral
            var value = VALUES[numeral];
            resultNumber += value;

            //remove the letters of the numeral from the string
            strToRead = strToRead.Substring(numeral.Length);


            //short hand like IX? -> move on to the next numeral option
            if (numeral.Length > 1)
                numeralOptionPointer++;
        }

        //whole string is read, return the numeral
        if (String.IsNullOrEmpty(strToRead))
            return new RomanNumeral(resultNumber);


        //string is invalid
        return null;
    }
    public static void Main()
    {
        Console.Clear();
        Console.WriteLine("Choose an option:");
        Console.WriteLine("1) To Roman");
        Console.WriteLine("2) From roman");
        Console.WriteLine("3) Exit");
        Console.Write("\r\nSelect an option: ");

        switch (Console.ReadLine())
        {
            case "1":
                Console.Clear();
                Console.Write("Number to convert: ");
                var toInput = Console.ReadLine();
                var l = new RomanNumeral(toInput);
                string s = l.ToString();
                Console.WriteLine($"{toInput} becomes {s} in roman numerals");
                Console.WriteLine("Press enter to return to meny");
                Console.ReadLine();
                Main();
                break;
            case "2":
                Console.Clear();
                Console.Write("Roman numeral to convert: ");
                var fromInput = Console.ReadLine();
                var roman = RomanNumeral.Parse(fromInput);
                int parsedRoman = roman.Number;
                Console.WriteLine($"{fromInput.ToUpper()} becomes {parsedRoman} in integers");
                Console.WriteLine("Press enter to return to meny");
                Console.ReadLine();
                Main();
                break;
            case "3":
                break;
            default:
                break;
        }
    }
}

