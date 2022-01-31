using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingManager
{
    public class CommandManager
    {
        readonly IConsole consoleWrapper;
        public CommandManager(IConsole changedWrapper)
        {
            consoleWrapper = changedWrapper;
        }
        public string AwaitCommand(User user)
        {
            if (!File.Exists($"{Environment.CurrentDirectory}/meetingList/meetingList.Json"))
            {
                Directory.CreateDirectory($"{Environment.CurrentDirectory}/meetingList");
                using (FileStream fs = File.Create($"{Environment.CurrentDirectory}/meetingList/meetingList.Json"))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(new List<Meeting>()));
                    fs.Write(info, 0, info.Length);
                }
            }
            string jsonRead = File.ReadAllText($"{Environment.CurrentDirectory}/meetingList/meetingList.Json");
            List<Meeting> meetingList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Meeting>>(jsonRead);

            consoleWrapper.WriteLine("Command list:");
            consoleWrapper.WriteLine("\t1 - Create a meeting.");
            consoleWrapper.WriteLine("\t2 - Delete a meeting.");
            consoleWrapper.WriteLine("\t3 - Add a person to a meeting.");
            consoleWrapper.WriteLine("\t4 - Remove a person from a meeting.");
            consoleWrapper.WriteLine("\t5 - List all meetings.");
            consoleWrapper.WriteLine("\t9 - Close program.");

            switch (consoleWrapper.ReadLine())
            {
                case "1":
                    Meeting meeting = new(consoleWrapper);
                    meeting.CreateMeeting();
                    meetingList.Add(meeting);
                    string jsonWrite = Newtonsoft.Json.JsonConvert.SerializeObject(meetingList);
                    File.WriteAllText($"{Environment.CurrentDirectory}/meetingList/meetingList.Json", jsonWrite);
                    consoleWrapper.WriteLine(meeting.ToString());
                    return meeting.ToString();
                case "2":
                    DeleteMeeting(meetingList, user);
                    return meetingList.ToString();
                case "3":
                    AddPerson(meetingList);
                    return "Added";
                case "4":
                    RemovePerson(meetingList);
                    return "Removed";
                case "5":
                    foreach (var oneMeeting in meetingList)
                    {
                        consoleWrapper.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(oneMeeting));
                    }
                    FilterMeetings(meetingList);
                    return meetingList.ToString();
                case "9":
                    return "Exit";
                default:
                    return "Incorrect command.";
            }
        }

        public string FilterMeetings(List<Meeting> meetingList)
        {
            consoleWrapper.WriteLine("Command list:");
            consoleWrapper.WriteLine("\t1 - Filter by description.");
            consoleWrapper.WriteLine("\t2 - Filter by responsible person.");
            consoleWrapper.WriteLine("\t3 - Filter by category.");
            consoleWrapper.WriteLine("\t4 - Filter by type.");
            consoleWrapper.WriteLine("\t5 - Filter by date.");
            consoleWrapper.WriteLine("\t6 - Filter by number of attendees.");
            string searchKey;
            List<Meeting> filteredList = new();

            switch (consoleWrapper.ReadLine())
            {
                case "1":
                    consoleWrapper.WriteLine("Enter search term for description.");
                    searchKey = consoleWrapper.ReadLine();
                    filteredList = meetingList.FindAll(o => o.Description.Contains(searchKey));
                    foreach (var oneMeeting in filteredList)
                    {
                        consoleWrapper.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(oneMeeting));
                    }
                    break;
                case "2":
                    consoleWrapper.WriteLine("Enter responsible person.");
                    searchKey = consoleWrapper.ReadLine();
                    filteredList = meetingList.FindAll(o => o.ResponsiblePerson.Contains(searchKey));
                    foreach (var oneMeeting in filteredList)
                    {
                        consoleWrapper.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(oneMeeting));
                    }
                    break;
                case "3":
                    consoleWrapper.WriteLine("Select category.");
                    searchKey = consoleWrapper.ReadLine();
                    filteredList = meetingList.FindAll(o => o.Category.Contains(searchKey));
                    foreach (var oneMeeting in filteredList)
                    {
                        consoleWrapper.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(oneMeeting));
                    }
                    break;
                case "4":
                    consoleWrapper.WriteLine("Select type.");
                    searchKey = consoleWrapper.ReadLine();
                    filteredList = meetingList.FindAll(o => o.Type.Contains(searchKey));
                    foreach (var oneMeeting in filteredList)
                    {
                        consoleWrapper.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(oneMeeting));
                    }
                    break;
                case "5":
                    consoleWrapper.WriteLine("Enter date.");
                    searchKey = consoleWrapper.ReadLine();
                    filteredList = meetingList.FindAll(o => o.StartDate == DateTime.ParseExact(searchKey, "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None));
                    foreach (var oneMeeting in filteredList)
                    {
                        consoleWrapper.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(oneMeeting));
                    }
                    break;
                case "6":
                    consoleWrapper.WriteLine("Enter number of attendees.");
                    searchKey = consoleWrapper.ReadLine();
                    filteredList = meetingList.FindAll(o => o.Attendees.Count == int.Parse(searchKey));
                    foreach (var oneMeeting in filteredList)
                    {
                        consoleWrapper.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(oneMeeting));
                    }
                    break;
                default:
                break;

            }
            consoleWrapper.WriteLine("Press any key to close.");
            consoleWrapper.ReadLine();
            return Newtonsoft.Json.JsonConvert.SerializeObject(filteredList);
        }

        public string DeleteMeeting(List<Meeting> meetingList, User user)
        {
            int index = 1;
            foreach (var oneMeeting in meetingList)
            {
                consoleWrapper.WriteLine($"{index}.\n");
                consoleWrapper.WriteLine($"{Newtonsoft.Json.JsonConvert.SerializeObject(oneMeeting)}\n");
                index++;
            }
            consoleWrapper.WriteLine("Select number of meeting to delete.");
            int toDelete = int.Parse(consoleWrapper.ReadLine());
            if(toDelete > meetingList.Count)
            {
                consoleWrapper.WriteLine("Meeting with this number does not exist.\n");
                DeleteMeeting(meetingList, user);
            }
            else
            {
                if (!meetingList[toDelete - 1].ResponsiblePerson.Equals(user.Name()))
                {
                    consoleWrapper.WriteLine("You are not the responsible person for this meeting.");
                }
                else
                {
                    meetingList.RemoveAt(toDelete - 1);
                    string jsonWrite = Newtonsoft.Json.JsonConvert.SerializeObject(meetingList);
                    File.WriteAllText($"{Environment.CurrentDirectory}/meetingList/meetingList.Json", jsonWrite);
                }
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(meetingList);
        }

        public string AddPerson(List<Meeting> meetingList)
        {
            int index = 1;
            string answer;

            foreach (var oneMeeting in meetingList)
            {
                consoleWrapper.WriteLine($"{index}.\n");
                consoleWrapper.WriteLine($"{Newtonsoft.Json.JsonConvert.SerializeObject(oneMeeting)}\n");
                index++;
            }
            consoleWrapper.WriteLine("Select number of meeting to add people.");
            int toAdd = int.Parse(consoleWrapper.ReadLine());
            consoleWrapper.WriteLine("Write names of people to add one by one or write exit to finish adding people.\n");

            while ((answer = consoleWrapper.ReadLine()).ToLower() != "Exit".ToLower())
            {
                bool addToMeeting = false;
                List<Meeting> filteredList = new();
                filteredList = meetingList.FindAll(o => o.Attendees.Contains(answer))
                    .FindAll(o => (o.StartDate <= meetingList[toAdd - 1].StartDate && o.EndDate > meetingList[toAdd - 1].StartDate) ||
                    (o.StartDate < meetingList[toAdd - 1].EndDate && o.EndDate >= meetingList[toAdd - 1].EndDate) ||
                    (o.StartDate >= meetingList[toAdd - 1].StartDate && o.EndDate <= meetingList[toAdd - 1].EndDate));
                if (meetingList[toAdd - 1].Attendees.Contains(answer))
                {
                    consoleWrapper.WriteLine("This person is already in the meeting.");
                }
                else if(filteredList.Count != 0){
                    consoleWrapper.WriteLine("This person is in another meeting with an intersecting time do you want to continue? Y/N");
                    string input = consoleWrapper.ReadLine();
                    if(input.ToLower() == "Y".ToLower())
                    {
                        meetingList[toAdd - 1].Attendees.Add(answer);
                        string jsonWrite = Newtonsoft.Json.JsonConvert.SerializeObject(meetingList);
                        File.WriteAllText($"{Environment.CurrentDirectory}/meetingList/meetingList.Json", jsonWrite);
                    }
                    else if(input.ToLower() == "N".ToLower())
                    {
                        break;
                    }
                    else
                    {
                        consoleWrapper.WriteLine("Wrong command.");
                        consoleWrapper.WriteLine("Write name of person to add.");
                    }
                }
                else
                {
                    meetingList[toAdd - 1].Attendees.Add(answer);
                    string jsonWrite = Newtonsoft.Json.JsonConvert.SerializeObject(meetingList);
                    File.WriteAllText($"{Environment.CurrentDirectory}/meetingList/meetingList.Json", jsonWrite);
                }
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(meetingList);
        }

        public string RemovePerson(List<Meeting> meetingList)
        {
            int index = 1;
            int attendeeIndex = 1;
            string answer;

            foreach (var oneMeeting in meetingList)
            {
                consoleWrapper.WriteLine($"{index}.\n");
                consoleWrapper.WriteLine($"{Newtonsoft.Json.JsonConvert.SerializeObject(oneMeeting)}\n");
                index++;
            }
            consoleWrapper.WriteLine("Select number of meeting to remove people.");
            int toRemove = int.Parse(consoleWrapper.ReadLine());

            foreach (string attendee in meetingList[toRemove-1].Attendees)
            {
                consoleWrapper.WriteLine($"{attendeeIndex}.\n");
                consoleWrapper.WriteLine($"{attendee}\n");
                attendeeIndex++;
            }
            consoleWrapper.WriteLine("Write the number of person to remove or exit to stop removing.\n");

            while ((answer = consoleWrapper.ReadLine()).ToLower() != "Exit".ToLower())
            {
                if (int.Parse(answer) > attendeeIndex-1)
                {
                    consoleWrapper.WriteLine("Index out of bounds.");
                }
                else
                {
                    meetingList[toRemove - 1].Attendees.RemoveAt(int.Parse(answer)-1);
                    string jsonWrite = Newtonsoft.Json.JsonConvert.SerializeObject(meetingList);
                    File.WriteAllText($"{Environment.CurrentDirectory}/meetingList/meetingList.Json", jsonWrite);
                }
                attendeeIndex = 1;
                foreach (string attendee in meetingList[toRemove - 1].Attendees)
                {
                    consoleWrapper.WriteLine($"{attendeeIndex}.\n");
                    consoleWrapper.WriteLine($"{attendee}\n");
                    attendeeIndex++;
                }
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(meetingList);
        }
    }
}
