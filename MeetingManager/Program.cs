using System;
using MeetingManager;

namespace MeetingManager
{
    class Program
    {
        public static void Main()
        {
            User user = new User();
            Controller controller = new Controller();
            string answer = "";

            //string jsonRead = File.ReadAllText($"{Environment.CurrentDirectory}/meetingList/meetingList.Json");
            //List<Meeting> meetingList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Meeting>>(jsonRead);

            Console.WriteLine("Enter your name.");
            user.Name(Console.ReadLine());

            do
            {
                Console.Clear();
                answer = controller.AwaitCommand(user);
                Console.WriteLine(answer);
            }while (answer != "Exit");

            //Meeting meeting = Meeting.CreateMeeting();
            //meetingList.Add(meeting);
            //string jsonWrite = Newtonsoft.Json.JsonConvert.SerializeObject(meetingList);
            //File.WriteAllText($"{Environment.CurrentDirectory}/meetingList/meetingList.Json", jsonWrite);
            //Console.WriteLine(Environment.CurrentDirectory);

        }
    }
}


