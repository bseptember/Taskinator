using Microsoft.Recognizers.Text.DateTime;
using Taskinator.Models;

namespace Taskinator.Services
{
    public class EventParser : IEventParser
    {
        public EventParser()
        {

        }

        public EventModel ParseEventFromText(string text)
        {
            var culture = "en-US"; // Set culture if necessary
            var results = DateTimeRecognizer.RecognizeDateTime(text, culture);

            DateTimeOffset start = DateTimeOffset.Now;
            DateTimeOffset end = DateTimeOffset.Now.AddHours(1);
            string description = text;

            // Adjust the current time to the current date without the time (midnight)
            var currentDate = DateTimeOffset.Now.Date;

            if (results.Any())
            {
                var dateTimeEntities = results.First().Resolution;
                if (dateTimeEntities.TryGetValue("values", out var values))
                {
                    var valuesList = values as List<Dictionary<string, string>>;
                    if (valuesList != null && valuesList.Count > 0)
                    {
                        var firstEntity = valuesList.First();

                        if (firstEntity.TryGetValue("type", out var type))
                        {
                            if (type == "timerange")
                            {
                                if (firstEntity.TryGetValue("start", out var startTimeStr) && DateTimeOffset.TryParse(startTimeStr, out var startTime))
                                {
                                    // Adjust start time to match the current date if only time is provided
                                    if (startTime.Date == DateTimeOffset.MinValue.Date)
                                    {
                                        startTime = currentDate.Add(startTime.TimeOfDay);
                                    }
                                    start = startTime;
                                }

                                if (firstEntity.TryGetValue("end", out var endTimeStr) && DateTimeOffset.TryParse(endTimeStr, out var endTime))
                                {
                                    end = endTime;
                                }
                                else if (firstEntity.TryGetValue("timex", out var timex) && TimeSpan.TryParse(timex.Replace("PT", "").Replace("H", ":00:00"), out var duration))
                                {
                                    end = start.Add(duration);
                                }
                            }
                            else
                            {
                                if (firstEntity.TryGetValue("value", out var startTimeStr) && DateTimeOffset.TryParse(startTimeStr, out var startTime))
                                {
                                    // Adjust start time to match the current date if only time is provided
                                    if (startTime.Date == DateTimeOffset.MinValue.Date)
                                    {
                                        startTime = currentDate.Add(startTime.TimeOfDay);
                                    }
                                    start = startTime;
                                    end = startTime.AddHours(1);
                                }
                            }
                        }
                    }
                }

                // Extract the description (everything before the recognized datetime)
                description = text.Substring(0, results.First().Start).Trim();
            }

            // Handling specific text patterns for duration and specific days
            var lowerText = text.ToLower();
            var durationMatch = System.Text.RegularExpressions.Regex.Match(lowerText, @"(\d+)\s*hours?");
            var dayMatch = System.Text.RegularExpressions.Regex.Match(lowerText, @"(monday|tuesday|wednesday|thursday|friday|saturday|sunday|tomorrow)");

            if (durationMatch.Success)
            {
                int hours = int.Parse(durationMatch.Groups[1].Value);
                if (dayMatch.Success)
                {
                    string dayStr = dayMatch.Groups[1].Value;
                    int daysToAdd = (int)(Enum.Parse<DayOfWeek>(dayStr, true) - DateTimeOffset.Now.DayOfWeek + 7) % 7;
                    if (dayStr == "tomorrow") daysToAdd = 1;
                    start = DateTimeOffset.Now.Date.AddDays(daysToAdd).Add(start.TimeOfDay);
                    end = start.AddHours(hours);
                }
                else
                {
                    end = start.AddHours(hours);
                }
            }

            // Assume the first word is the event name and the rest is the description
            var splitText = description.Split(' ', 2);
            var name = char.ToUpper(splitText[0][0]) + splitText[0].Substring(1);
            if (splitText.Length > 1)
            {
                var words = splitText[1].Trim().Split(' ');
                var lastWord = words[words.Length - 1].ToLower();

                // Check if the last word is a recognized preposition
                if (lastWord == "at" ||
                    lastWord == "on" ||
                    lastWord == "before" ||
                    lastWord == "after" ||
                    lastWord == "during" ||
                    lastWord == "between")
                {
                    description = $"{splitText[0]} {string.Join(" ", words.Take(words.Length - 1))}".Trim();
                }
                else
                {
                    // If no recognized preposition as the last word, keep both parts
                    description = $"{splitText[0]} {splitText[1]}".Trim();
                }
            }
            else
            {
                description = splitText[0].Trim();
            }

            return new EventModel
            {
                Name = name,
                Description = description,
                Starting = start,
                Ending = end,
                LastModifiedDate = DateTimeOffset.Now
            };
        }
    }
}