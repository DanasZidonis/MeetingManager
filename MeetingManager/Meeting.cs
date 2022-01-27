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
            CodeMonkey,
            Hub,
            Short,
            TeamBuilding
        }
        public enum Type
        {
            Live,
            InPerson
        }
        public string? name { get; set; }
        public string? responsiblePerson { get; set; }
        public string? description { get; set; }
        public string? category { get; set; }
        public string? type { get; set; }
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

            Console.WriteLine("Enter category.");
            meeting.category = Console.ReadLine();

            Console.WriteLine("Enter type.");
            meeting.type = Console.ReadLine();

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
