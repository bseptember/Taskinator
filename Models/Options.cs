﻿namespace Taskinator.Models
{
    public static class Options
    {
        public static readonly List<string> FilterOptions = new List<string>()
        {
            "",
            "Recent",
            "Upcoming",
            "Priority",
            "Urgency",
            "Suggested"
        };

        public static readonly List<string> TodoCollectionOptions = new List<string>()
        {
            "Incomplete",
            "Completed"
        };

        public static readonly Dictionary<int, Color> PriorityColors = new Dictionary<int, Color>()
        {
            { 0, Colors.LightSkyBlue},
            { 1, Colors.Cyan},
            { 2, Colors.Aquamarine},
            { 3, Colors.Green},
            { 4, Colors.YellowGreen},
            { 5, Colors.Yellow},
            { 6, Colors.Gold},
            { 7, Colors.Orange},
            { 8, Colors.OrangeRed},
            { 9, Colors.Red},
            { 10, Colors.DarkRed}
        };
    }
}