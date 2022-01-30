using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingManager
{
    public class Controller
    {
        public string AwaitCommand(User user)
        {
            string jsonRead = File.ReadAllText($"{Environment.CurrentDirectory}/meetingList/meetingList.Json");
            List<Meeting> meetingList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Meeting>>(jsonRead);

            Console.WriteLine("Command list:");
            Console.WriteLine("\t1 - Create a meeting.");
            Console.WriteLine("\t2 - Delete a meeting.");
            Console.WriteLine("\t3 - Add a person to a meeting.");
            Console.WriteLine("\t4 - Remove a person from a meeting.");
            Console.WriteLine("\t5 - List all meetings.");
            Console.WriteLine("\t9 - Close program.");
            switch (Console.ReadLine())
            {
                case "1":
                    Meeting meeting = Meeting.CreateMeeting();
                    meetingList.Add(meeting);
                    string jsonWrite = Newtonsoft.Json.JsonConvert.SerializeObject(meetingList);
                    File.WriteAllText($"{Environment.CurrentDirectory}/meetingList/meetingList.Json", jsonWrite);
                    Console.WriteLine(meeting.ToString());
                    return meeting.ToString();
                case "2":
                    DeleteMeeting(meetingList, user);
                    return meetingList.ToString();
                case "3":
                    AddPerson(meetingList);
                    return "Added";
                case "4":
                    RemovePerson(meetingList);
                    return "Remove";
                case "5":
                    Console.WriteLine("List all");
                    foreach (var oneMeeting in meetingList)
                    {
                        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(oneMeeting));
                    }
                    FilterMeetings(meetingList);
                    return meetingList.ToString();
                case "9":
                    return "Exit";
                default:
                    return "Incorrect command.";
            }
        }

        public void FilterMeetings(List<Meeting> meetingList)
        {
            Console.WriteLine("Command list:");
            Console.WriteLine("\t1 - Filter by description.");
            Console.WriteLine("\t2 - Filter by responsible person.");
            Console.WriteLine("\t3 - Filter by category.");
            Console.WriteLine("\t4 - Filter by type.");
            Console.WriteLine("\t5 - Filter by date.");
            Console.WriteLine("\t6 - Filter by number of attendees.");
            string searchKey;
            List<Meeting> filteredList = new List<Meeting>();
            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine("Enter search term for description.");
                    searchKey = Console.ReadLine();
                    filteredList = meetingList.FindAll(o => o.description.Contains(searchKey));
                    foreach (var oneMeeting in filteredList)
                    {
                        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(oneMeeting));
                    }
                    break;
                case "2":
                    Console.WriteLine("Enter responsible person.");
                    searchKey = Console.ReadLine();
                    filteredList = meetingList.FindAll(o => o.responsiblePerson.Contains(searchKey));
                    foreach (var oneMeeting in filteredList)
                    {
                        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(oneMeeting));
                    }
                    break;
                case "3":
                break;
                    Console.WriteLine("Select category.");
                    searchKey = Console.ReadLine();
                    filteredList = meetingList.FindAll(o => o.category.Contains(searchKey));
                    foreach (var oneMeeting in filteredList)
                    {
                        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(oneMeeting));
                    }
                case "4":
                    Console.WriteLine("Select type.");
                    searchKey = Console.ReadLine();
                    filteredList = meetingList.FindAll(o => o.type.Contains(searchKey));
                    foreach (var oneMeeting in filteredList)
                    {
                        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(oneMeeting));
                    }
                    break;
                case "5":
                    Console.WriteLine("Enter date.");
                    searchKey = Console.ReadLine();
                    filteredList = meetingList.FindAll(o => o.startDate == DateTime.ParseExact(searchKey, "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None));
                    foreach (var oneMeeting in filteredList)
                    {
                        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(oneMeeting));
                    }
                    break;
                case "6":
                    Console.WriteLine("Enter number of attendees.");
                    searchKey = Console.ReadLine();
                    filteredList = meetingList.FindAll(o => o.attendees.Count == int.Parse(searchKey));
                    foreach (var oneMeeting in filteredList)
                    {
                        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(oneMeeting));
                    }
                    break;
                default:
                break;

            }
        }

        public void DeleteMeeting(List<Meeting> meetingList, User user)
        {
            int index = 1;
            foreach (var oneMeeting in meetingList)
            {
                Console.WriteLine($"{index}.\n");
                Console.WriteLine($"{Newtonsoft.Json.JsonConvert.SerializeObject(oneMeeting)}\n");
                index++;
            }
            Console.WriteLine("Select number of meeting to delete.");
            int toDelete = int.Parse(Console.ReadLine());
            if(toDelete > meetingList.Count)
            {
                Console.WriteLine("Meeting with this number does not exist.\n");
                DeleteMeeting(meetingList, user);
            }
            else
            {
                if (!meetingList[toDelete - 1].responsiblePerson.Equals(user.Name()))
                {
                    Console.WriteLine("You are not the responsible person for this meeting.");
                }
                else
                {
                    meetingList.RemoveAt(toDelete - 1);
                    string jsonWrite = Newtonsoft.Json.JsonConvert.SerializeObject(meetingList);
                    File.WriteAllText($"{Environment.CurrentDirectory}/meetingList/meetingList.Json", jsonWrite);
                }
            }
        }

        public void AddPerson(List<Meeting> meetingList)
        {
            int index = 1;
            string answer;
            foreach (var oneMeeting in meetingList)
            {
                Console.WriteLine($"{index}.\n");
                Console.WriteLine($"{Newtonsoft.Json.JsonConvert.SerializeObject(oneMeeting)}\n");
                index++;
            }
            Console.WriteLine("Select number of meeting to add people.");
            int toAdd = int.Parse(Console.ReadLine());
            Console.WriteLine("Write names of people to add one by one or write exit to finish adding people.\n");
            while ((answer = Console.ReadLine()).ToLower() != "Exit".ToLower())
            {
                bool addToMeeting = false;
                List<Meeting> filteredList = new List<Meeting>();
                filteredList = meetingList.FindAll(o => o.attendees.Contains(answer))
                    .FindAll(o => (o.startDate <= meetingList[toAdd - 1].startDate && o.endDate > meetingList[toAdd - 1].startDate) ||
                    (o.startDate < meetingList[toAdd - 1].endDate && o.endDate >= meetingList[toAdd - 1].endDate) ||
                    (o.startDate >= meetingList[toAdd - 1].startDate && o.endDate <= meetingList[toAdd - 1].endDate));
                if (meetingList[toAdd - 1].attendees.Contains(answer))
                {
                    Console.WriteLine("This person is already in the meeting.");
                }
                else if(filteredList.Count != 0){
                    Console.WriteLine("This person is in another meeting with an intersecting time do you want to continue? Y/N");
                    string input = Console.ReadLine();
                    if(input.ToLower() == "Y".ToLower())
                    {
                        meetingList[toAdd - 1].attendees.Add(answer);
                        string jsonWrite = Newtonsoft.Json.JsonConvert.SerializeObject(meetingList);
                        File.WriteAllText($"{Environment.CurrentDirectory}/meetingList/meetingList.Json", jsonWrite);
                    }
                    else if(input.ToLower() == "N".ToLower())
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Wrong command.");
                        Console.WriteLine("Write name of person to add.");
                    }
                }
                else
                {
                    meetingList[toAdd - 1].attendees.Add(answer);
                    string jsonWrite = Newtonsoft.Json.JsonConvert.SerializeObject(meetingList);
                    File.WriteAllText($"{Environment.CurrentDirectory}/meetingList/meetingList.Json", jsonWrite);
                }
            }
        }

        public void RemovePerson(List<Meeting> meetingList)
        {
            int index = 1;
            int attendeeIndex = 1;
            string answer;
            foreach (var oneMeeting in meetingList)
            {
                Console.WriteLine($"{index}.\n");
                Console.WriteLine($"{Newtonsoft.Json.JsonConvert.SerializeObject(oneMeeting)}\n");
                index++;
            }
            Console.WriteLine("Select number of meeting to remove people.");
            int toRemove = int.Parse(Console.ReadLine());
            foreach (string attendee in meetingList[toRemove-1].attendees)
            {
                Console.WriteLine($"{attendeeIndex}.\n");
                Console.WriteLine($"{attendee}\n");
                attendeeIndex++;
            }
            Console.WriteLine("Write the number of person to remove or exit to stop removing.\n");
            while ((answer = Console.ReadLine()).ToLower() != "Exit".ToLower())
            {
                if (int.Parse(answer) > attendeeIndex-1)
                {
                    Console.WriteLine("Index out of bounds.");
                }
                else
                {
                    meetingList[toRemove - 1].attendees.RemoveAt(int.Parse(answer)-1);
                    string jsonWrite = Newtonsoft.Json.JsonConvert.SerializeObject(meetingList);
                    File.WriteAllText($"{Environment.CurrentDirectory}/meetingList/meetingList.Json", jsonWrite);
                }
                attendeeIndex = 1;
                foreach (string attendee in meetingList[toRemove - 1].attendees)
                {
                    Console.WriteLine($"{attendeeIndex}.\n");
                    Console.WriteLine($"{attendee}\n");
                    attendeeIndex++;
                }
            }
        }
    }
}
