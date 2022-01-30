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
        public enum Category
        {
            CodeMonkey = 1,
            Hub = 2,
            Short = 3,
            TeamBuilding = 4
        }
        public enum Type
        {
            Live = 1,
            InPerson = 2
        }
        public string? name { get; set; }
        public string? responsiblePerson { get; set; }
        public string? description { get; set; }
        public string? category { get; set; } = null;
        public string? type { get; set; } = null;
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public List<String>? attendees { get; set; } = new List<String>();

        public static Meeting CreateMeeting()
        {
            Meeting meeting = new Meeting();

            Console.WriteLine("Enter meeting name.");
            meeting.name = Console.ReadLine();

            Console.WriteLine("Enter responsible person.");
            meeting.responsiblePerson = Console.ReadLine();

            Console.WriteLine("Enter description.");
            meeting.description = Console.ReadLine();

            Console.WriteLine("Select category.");
            while (meeting.category == null)
            {
                int index = 1;
                int input;
                foreach(var cat in Enum.GetValues(typeof(Category)))
                {
                    Console.WriteLine($"{index}. {cat}");
                    index++;
                }
                input = int.Parse(Console.ReadLine());
                if (Enum.IsDefined((Category)input))
                {
                    meeting.category = ((Category)input).ToString();
                }
                else
                {
                    Console.WriteLine("Index out of bounds.");
                }
            }

            Console.WriteLine("Select type.");
            while (meeting.type == null)
            {
                int index = 1;
                int input;
                foreach (var meetType in Enum.GetValues(typeof(Type)))
                {
                    Console.WriteLine($"{index}. {meetType}");
                    index++;
                }
                input = int.Parse(Console.ReadLine());
                if (Enum.IsDefined((Type)input))
                {
                    meeting.type = ((Type)input).ToString();
                }
                else
                {
                    Console.WriteLine("Index out of bounds.");
                }
            }

            Console.WriteLine("Enter start date.");
            string line = Console.ReadLine();
            DateTime dt;
            while (!DateTime.TryParseExact(line, "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None, out dt))
            {
                Console.WriteLine("Invalid date, please retry");
                line = Console.ReadLine();
            }
            meeting.startDate = dt;

            Console.WriteLine("Enter end date.");
            string line2 = Console.ReadLine();
            DateTime dt2;
            while (!DateTime.TryParseExact(line2, "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None, out dt2))
            {
                Console.WriteLine("Invalid date, please retry");
                line2 = Console.ReadLine();
            }
            meeting.endDate = dt2;

            //string json = Newtonsoft.Json.JsonConvert.SerializeObject(meeting);
            //File.WriteAllText($"{Environment.CurrentDirectory}/meetingList/meetingList.Json", json);

            return meeting;
        }
    }
}
