using KeesTalksTech.Utilities.Latin.Numerals;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RomanNumeralTest
{
    [TestClass]
    public class RomanKnownValues
    {

        [DataRow(1110, "MCX")]
        [DataRow(1993, "MCMXCIII")]
        [DataRow(8, "VIII")]
        [DataTestMethod]
        public void To_Roman_Known_values_ReturnRomanString(int input, string expected)
        {
            var romanToString = new RomanNumeral(input);

            var actual = romanToString.ToString();

            Assert.AreEqual(expected, actual);
        }

        [DataRow("MCX", 1110)]
        [DataRow("MCMXCIII", 1993)]
        [DataRow("VIII", 8)]
        [DataTestMethod]
        public void From_Roman_Known_Values_ReturnInt(string input, int expected)
        {
            var romanToInt = RomanNumeral.Parse(input);

            var actual = romanToInt.Number;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void From_Roman_End_With_J_ReturnRomanString()
        {
            var testString = "CMJ"; // Should be same as CMI
            var expected = 901;
            var romanToInt = RomanNumeral.Parse(testString);

            var actual = romanToInt.Number;

            Assert.AreEqual(expected, actual);
        }
    }

    [TestClass]
    public class ToRomanBadInput
    {
        [TestMethod]
        public void To_Roman_Negative_value_ShouldThrowArgumentOutOfRangeException()
        {
            var value = -100;

            try
            {
                var toRomanString = new RomanNumeral(value);
            }
            catch (System.ArgumentOutOfRangeException e)
            {
                StringAssert.Contains(e.Message, RomanNumeral.NumberLowerThanZeroMessage);
                return;
            }
            Assert.Fail("The expected exception was not thrown.");
        }


        [TestMethod]
        public void To_Roman_Large_Input_ShouldThrowArgumentOutOfRangeException()
        {
            var value = 5000;

            try
            {
                var toRomanString = new RomanNumeral(value);
            }
            catch (System.ArgumentOutOfRangeException e)
            {
                StringAssert.Contains(e.Message, RomanNumeral.NumberLowerThanZeroMessage);
                return;
            }
            Assert.Fail("The expected exception was not thrown.");
        }

        [TestMethod]
        public void To_Roman_String_Input_ShouldThrowArgumentOutOfRangeException()
        {
            string inputValue = "one";

            try
            {
                var toRomanString = new RomanNumeral(inputValue);
            }
            catch (System.ArgumentException e)
            {
                StringAssert.Contains(e.Message, RomanNumeral.StringInputToRomanMessage);
                return;
            }
            Assert.Fail("The expected exception was not thrown.");
        }
    }


    [TestClass]
    public class FromRomanBadInput
    {
        [TestMethod]
        public void From_Roman_Empty_String_ReturnZero()
        {
            var testString = string.Empty;
            var romanToInt = RomanNumeral.Parse(testString);

            var actual = romanToInt.Number;

            Assert.AreEqual(0, actual);
        }

        [TestMethod]
        public void From_Roman_Null_ReturnZero()
        {
            string testString = null;
            var romanToInt = RomanNumeral.Parse(testString);

            var actual = romanToInt.Number;

            Assert.AreEqual(0, actual);
        }

        [TestMethod]
        public void From_Roman_Non_Roman_Numerals_ShouldThrowArgumentException()
        {
            string testString = "NCJS";

            try
            {
                var romanToInt = RomanNumeral.Parse(testString);
                var actual = romanToInt.Number;
            }
            catch (System.ArgumentException e)
            {
                StringAssert.Contains(e.Message, RomanNumeral.StringContainsNonRomanNumeralsMessage);
                return;
            }
            Assert.Fail("The expected exception was not thrown.");
        }

        [DataRow("IIII")]
        [DataRow("DDD")]
        [DataRow("CCCC")]
        [DataRow("MMMM")]
        [DataTestMethod]
        public void From_Roman_To_Many_repted_Numerals_ShouldThrowArgumentException(string input)
        {
            string testString = input;

            try
            {
                var romanToInt = RomanNumeral.Parse(testString);
                var actual = romanToInt.Number;
            }
            catch (System.ArgumentException e)
            {
                StringAssert.Contains(e.Message, RomanNumeral.StingContainsToManyRepetativeNumerals);
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

    }
    [TestClass]
    public class RoundTripCheck
    {
        [TestMethod]
        public void Round_Trip_Check_ReturnSameValue()
        {
            for (var i = 1; i < 4000; i++)
            {
                var numeral = new RomanNumeral(i);
                var result = RomanNumeral.Parse(numeral.ToString()).Number;

                Assert.AreEqual(result, i);
            }
        }
    }
}