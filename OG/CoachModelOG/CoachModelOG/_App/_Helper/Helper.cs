using System;
using System.Collections.Generic;
using System.Text;

namespace CoachModel._App._Helper
{
    class Helper
    {
    }

    public static class InventoryItems
    {
        public const string _Physical = "Physical";
        public const string _Social = "Social";
        public const string _Mental = "Mental";
        public const string _Emotional = "Emotional";
        public const string _Financial = "Financial";

        static InventoryItems()
        {
            InventoryItemList = new List<string>();

            InventoryItemList.Add("Root (Ignore)");
            InventoryItemList.Add(_Physical);
            InventoryItemList.Add(_Social);
            InventoryItemList.Add(_Mental);
            InventoryItemList.Add(_Emotional);
            InventoryItemList.Add(_Financial);
        }

        public static List<string> InventoryItemList { get; set; }

        public static int GetID(string inventoryItem)
        {
            return InventoryItemList.IndexOf(inventoryItem);
        }

        public static string GetInventoryItem(int idInventoryItem)
        {
            return InventoryItemList[idInventoryItem];
        }
    }

    public static class Types
    {
        public const string _spotlight = "Spotlight";
        public const string _primary = "Primary";
        public const string _secondary = "Secondary";
        public const string _tertiary = "Tertiary";
        public const string _tally = "Tally";
        public const string _inverse = "Inverse";
        public const string _errand = "Errand";
        public const string _routine = "Routine";
        public const string _lifestyle = "Lifestyle";
        public const string _maintainance = "Maintainance";
        public const string _housekeeping = "Housekeeping";
        public const string _past = "Past";
        public const string _present = "Present";
        public const string _future = "Future";
        public const string _recommended = "Recommended";
        public const string _consumed = "Consumed";
        public const string _saved = "Saved";
        public const string _slider = "Slider";
        public const string _numberpad = "Numberpad";
        public const string _counter = "Counter";
        public const string _textarea = "Textarea";

        static Types()
        {
            TypeList = new List<string>();

            TypeList.Add("Root (Ignore)");
            TypeList.Add(_spotlight);
            TypeList.Add(_primary);
            TypeList.Add(_secondary);
            TypeList.Add(_tertiary);
            TypeList.Add(_tally);
            TypeList.Add(_inverse);
            TypeList.Add(_errand);
            TypeList.Add(_routine);
            TypeList.Add(_lifestyle);
            TypeList.Add(_maintainance);
            TypeList.Add(_housekeeping);
            TypeList.Add(_past);
            TypeList.Add(_present);
            TypeList.Add(_future);
            TypeList.Add(_recommended);
            TypeList.Add(_consumed);
            TypeList.Add(_saved);
            TypeList.Add(_slider);
            TypeList.Add(_numberpad);
            TypeList.Add(_counter);
            TypeList.Add(_textarea);
        }

        public static List<string> TypeList { get; set; }

        public static int GetID(string type)
        {
            return TypeList.IndexOf(type);
        }

        public static string GetType(int idType)
        {
            return TypeList[idType];
        }
    }

    public static class Timeframes
    {
        public const string _millisecond = "Millisecond";
        public const string _second = "Second";
        public const string _minute = "Minute";
        public const string _hour = "Hour";
        public const string _day = "Day";
        public const string _workday = "Workday";
        public const string _weekday = "Weekday";
        public const string _weekenday = "Weekend Day";
        public const string _weekend = "Weekend";
        public const string _week = "Week";
        public const string _month = "Month";
        public const string _season = "Season";
        public const string _quarter = "Quarter";
        public const string _trimester = "Trimester";
        public const string _semester = "Semester";
        public const string _year = "Year";
        public const string _milestone = "Milestone";
        public const string _life = "Life";

        static Timeframes()
        {
            TimeframeList = new List<string>();

            TimeframeList.Add("Root (Ignore)");
            TimeframeList.Add(_millisecond);
            TimeframeList.Add(_second);
            TimeframeList.Add(_minute);
            TimeframeList.Add(_hour);
            TimeframeList.Add(_day);
            TimeframeList.Add(_workday);
            TimeframeList.Add(_weekday);
            TimeframeList.Add(_weekenday);
            TimeframeList.Add(_weekend);
            TimeframeList.Add(_week);
            TimeframeList.Add(_month);
            TimeframeList.Add(_season);
            TimeframeList.Add(_quarter);
            TimeframeList.Add(_trimester);
            TimeframeList.Add(_semester);
            TimeframeList.Add(_year);
            TimeframeList.Add(_milestone);
            TimeframeList.Add(_life);
        }

        public static List<string> TimeframeList { get; set; }

        public static int GetID(string timeframe)
        {
            return TimeframeList.IndexOf(timeframe);
        }

        public static string GetTimeframe(int idTimeframe)
        {
            return TimeframeList[idTimeframe];
        }
    }

    public static class Repetitions
    {
        public const string _millisecondly = "Millisecondly";
        public const string _secondly = "Secondly";
        public const string _minutley = "Minutley";
        public const string _hourly = "Hourly";
        public const string _daily = "Daily";
        public const string _workdaily = "Workdaily";
        public const string _weekdaily = "Weekdaily";
        public const string _weekendaily = "Weekendaily";
        public const string _weekendly = "Weekendly";
        public const string _weekly = "Weekly";
        public const string _monthly = "Monthly";
        public const string _seasonally = "Seasonaly";
        public const string _quarterly = "Quarterly";
        public const string _triAnnually = "Tri-Annualy";
        public const string _biAnnually = "Bi-Annually";
        public const string _annually = "Annually";
        public const string _milestonly = "Milestonly";
        public const string _lifely = "Lifely";

        static Repetitions()
        {
            RepetitionList = new List<string>();

            RepetitionList.Add("Root (Ignore)");
            RepetitionList.Add(_millisecondly);
            RepetitionList.Add(_secondly);
            RepetitionList.Add(_minutley);
            RepetitionList.Add(_hourly);
            RepetitionList.Add(_daily);
            RepetitionList.Add(_workdaily);
            RepetitionList.Add(_weekdaily);
            RepetitionList.Add(_weekendaily);
            RepetitionList.Add(_weekendly);
            RepetitionList.Add(_weekly);
            RepetitionList.Add(_monthly);
            RepetitionList.Add(_seasonally);
            RepetitionList.Add(_quarterly);
            RepetitionList.Add(_triAnnually);
            RepetitionList.Add(_biAnnually);
            RepetitionList.Add(_annually);
            RepetitionList.Add(_milestonly);
            RepetitionList.Add(_lifely);
        }

        public static List<string> RepetitionList { get; set; }

        public static int GetID(string interval)
        {
            return RepetitionList.IndexOf(interval);
        }

        public static string GetRepetition(int idRepetition)
        {
            return RepetitionList[idRepetition];
        }
    }

    public static class Mediums
    {
        public const string _mobile = "Mobile";
        public const string _computer = "Computer";
        public const string _travel = "Travel";
        public const string _brickAndMortar = "Brick 'n' Mortar";
        public const string _home = "Home";
        public const string _any = "Any";

        static Mediums()
        {
            MediumList = new List<string>();

            MediumList.Add("Root (Ignore)");
            MediumList.Add(_mobile);
            MediumList.Add(_computer);
            MediumList.Add(_travel);
            MediumList.Add(_brickAndMortar);
            MediumList.Add(_home);
            MediumList.Add(_any);
        }

        public static List<string> MediumList { get; set; }

        public static int GetID(string medium)
        {
            return MediumList.IndexOf(medium);
        }

        public static string GetMedium(int idMedium)
        {
            return MediumList[idMedium];
        }
    }

    public static class Deadlines
    {
        public const string _mobile = "Mobile";
        public const string _computer = "Computer";
        public const string _travel = "Travel";
        public const string _brickAndMortar = "Brick 'n' Mortar";
        public const string _home = "Home";
        public const string _any = "Any";

        static Deadlines()
        {
            DeadlineList = new List<string>();

            DeadlineList.Add("Root (Ignore)");
            DeadlineList.Add(_mobile);
            DeadlineList.Add(_computer);
            DeadlineList.Add(_travel);
            DeadlineList.Add(_brickAndMortar);
            DeadlineList.Add(_home);
            DeadlineList.Add(_any);
        }

        public static List<string> DeadlineList { get; set; }

        public static int GetID(string deadline)
        {
            return DeadlineList.IndexOf(deadline);
        }

        public static string GetDeadline(int idDeadline)
        {
            return DeadlineList[idDeadline];
        }
    }

    public static class Nutrients
    {
        #region Constrants
        public const string _calcium = "Calcium";
        public const string _carbs = "Carbohydrates";
        public const string _cholesterol = "Cholesterol";
        public const string _monounsaturated = "Monounsaturated Fat";
        public const string _polyunsaturated = "Polyunsaturated Fat";
        public const string _saturated = "Saturated Fat";
        public const string _fat = "Fat";
        public const string _trans = "Trans Fat";
        public const string _iron = "Iron";
        public const string _fiber = "Fiber";
        public const string _folateEquivalent = "Folate Equivalent";
        public const string _potassium = "Potassium";
        public const string _magnesium = "Magnesium";
        public const string _sodium = "Sodium";
        public const string _energy = "Energy";
        public const string _niacinB3 = "Niacin B3";
        public const string _phosphorus = "Phosphorus";
        public const string _protein = "Protein";
        public const string _riboflavinB2 = "Riboflavin B2";
        public const string _sugars = "Sugars";
        public const string _thiaminB1 = "Thiamin B1";
        public const string _vitaminE = "Vitamin E";
        public const string _vitaminA = "Vitamin A";
        public const string _vitaminB12 = "Vitamin B12";
        public const string _vitaminB6 = "Vitamin B6";
        public const string _vitaminC = "Vitamin C";
        public const string _vitaminD = "Vitamin D";
        public const string _vitaminK = "Vitamin K";
        #endregion

        static Nutrients()
        {
            NutrientList = new List<string>();

            NutrientList.Add("Root (Ignore)");
            NutrientList.Add(_calcium);
            NutrientList.Add(_carbs);
            NutrientList.Add(_cholesterol);
            NutrientList.Add(_monounsaturated);
            NutrientList.Add(_polyunsaturated);
            NutrientList.Add(_saturated);
            NutrientList.Add(_fat);
            NutrientList.Add(_trans);
            NutrientList.Add(_iron);
            NutrientList.Add(_fiber);
            NutrientList.Add(_folateEquivalent);
            NutrientList.Add(_potassium);
            NutrientList.Add(_magnesium);
            NutrientList.Add(_sodium);
            NutrientList.Add(_energy);
            NutrientList.Add(_niacinB3);
            NutrientList.Add(_phosphorus);
            NutrientList.Add(_protein);
            NutrientList.Add(_riboflavinB2);
            NutrientList.Add(_sugars);
            NutrientList.Add(_thiaminB1);
            NutrientList.Add(_vitaminE);
            NutrientList.Add(_vitaminA);
            NutrientList.Add(_vitaminB12);
            NutrientList.Add(_vitaminB6);
            NutrientList.Add(_vitaminC);
            NutrientList.Add(_vitaminD);
            NutrientList.Add(_vitaminK);
        }

        public static List<string> NutrientList { get; set; }

        public static int GetID(string nutrient)
        {
            return NutrientList.IndexOf(nutrient);
        }

        public static string GetNutrient(int idNutrient)
        {
            return NutrientList[idNutrient];
        }
    }
}
