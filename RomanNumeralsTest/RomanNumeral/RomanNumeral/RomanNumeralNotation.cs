namespace KeesTalksTech.Utilities.Latin.Numerals
{
    public enum RomanNumeralNotation
    {
        Substractive = 0,
        Additive = 1
    }
}

//public static int operator +(int r1, RomanNumeral r2)
//{
//    var r = new RomanNumeral(r1) + r2;
//    return r.Number;
//}

//public static string operator +(string r1, RomanNumeral r2)
//{
//    var r = RomanNumeral.Parse(r1) + r2;
//    return r.ToString();
//}

//public static RomanNumeral operator +(RomanNumeral r1, string r2)
//{
//    return r1 + RomanNumeral.Parse(r2);
//}

//public static RomanNumeral operator +(RomanNumeral r1, int r2)
//{
//    var n = r1.Number + r2;
//    return new RomanNumeral(n);
//}

//public static RomanNumeral operator +(RomanNumeral r1, RomanNumeral r2)
//{
//    var n = r1.Number + r2.Number;
//    return new RomanNumeral(n);
//}


//public static int operator -(int r1, RomanNumeral r2)
//{
//    var r = new RomanNumeral(r1) - r2;
//    return r.Number;
//}

//public static string operator -(string r1, RomanNumeral r2)
//{
//    var r = RomanNumeral.Parse(r1) - r2;
//    return r.ToString();
//}

//public static RomanNumeral operator -(RomanNumeral r1, RomanNumeral r2)
//{
//    var n = r1.Number - r2.Number;

//    if (n < 0)
//    {
//        n = 0;
//    }

//    return new RomanNumeral(n);
//}

//public static implicit operator int(RomanNumeral r)
//{
//    return (r?.Number).GetValueOrDefault();
//}

//public static implicit operator string(RomanNumeral r)
//{
//    return r?.ToString();
//}

//public static implicit operator RomanNumeral(int r)
//{
//    return new RomanNumeral(r);
//}

//public static implicit operator RomanNumeral(string r)
//{
//    return Parse(r);
//}