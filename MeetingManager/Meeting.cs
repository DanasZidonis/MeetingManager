using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingManager
{
    public class Meeting
    {
        private readonly IConsole consoleWrapper;
        public Meeting(IConsole changedWrapper)
        {
            consoleWrapper = changedWrapper;
        }
        public enum CategoryEnum
        {
            CodeMonkey = 1,
            Hub = 2,
            Short = 3,
            TeamBuilding = 4
        }
        public enum TypeEnum
        {
            Live = 1,
            InPerson = 2
        }
        public string? Name { get; set; }
        public string? ResponsiblePerson { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; } = null;
        public string? Type { get; set; } = null;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<String>? Attendees { get; set; } = new List<String>();

        public Meeting CreateMeeting()
        {
            consoleWrapper.WriteLine("Enter meeting name.");
            Name = consoleWrapper.ReadLine();

            consoleWrapper.WriteLine("Enter responsible person.");
            ResponsiblePerson = consoleWrapper.ReadLine();

            consoleWrapper.WriteLine("Enter description.");
            Description = consoleWrapper.ReadLine();

            consoleWrapper.WriteLine("Select category.");
            while (Category == null)
            {
                int index = 1;
                int input;
                foreach(var cat in Enum.GetValues(typeof(CategoryEnum)))
                {
                    consoleWrapper.WriteLine($"{index}. {cat}");
                    index++;
                }
                input = int.Parse(consoleWrapper.ReadLine());
                if (Enum.IsDefined((CategoryEnum)input))
                {
                    Category = ((CategoryEnum)input).ToString();
                }
                else
                {
                    consoleWrapper.WriteLine("Index out of bounds.");
                }
            }

            consoleWrapper.WriteLine("Select type.");
            while (Type == null)
            {
                int index = 1;
                int input;
                foreach (var meetType in Enum.GetValues(typeof(TypeEnum)))
                {
                    consoleWrapper.WriteLine($"{index}. {meetType}");
                    index++;
                }
                input = int.Parse(consoleWrapper.ReadLine());
                if (Enum.IsDefined((TypeEnum)input))
                {
                    Type = ((TypeEnum)input).ToString();
                }
                else
                {
                    consoleWrapper.WriteLine("Index out of bounds.");
                }
            }

            consoleWrapper.WriteLine("Enter start date.");
            string line = consoleWrapper.ReadLine();
            DateTime dt;
            while (!DateTime.TryParseExact(line, "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None, out dt))
            {
                consoleWrapper.WriteLine("Invalid date, please retry");
                line = consoleWrapper.ReadLine();
            }
            StartDate = dt;

            consoleWrapper.WriteLine("Enter end date.");
            string line2 = consoleWrapper.ReadLine();
            DateTime dt2;
            while (!DateTime.TryParseExact(line2, "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None, out dt2))
            {
                consoleWrapper.WriteLine("Invalid date, please retry");
                line2 = consoleWrapper.ReadLine();
            }
            EndDate = dt2;

            return this;
        }
    }
}
