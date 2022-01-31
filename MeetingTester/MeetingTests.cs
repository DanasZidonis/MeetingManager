using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using MeetingManager;
using System.Text;

namespace MeetingTester
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            if (!File.Exists($"{Environment.CurrentDirectory}/MeetingList/meetingList.Json"))
            {
                Directory.CreateDirectory($"{Environment.CurrentDirectory}/meetingList");
                using (FileStream fs = File.Create($"{Environment.CurrentDirectory}/MeetingList/meetingList.Json"))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(new List<Meeting>()));
                    fs.Write(info, 0, info.Length);
                }
            }

            var consoleWrapper = new MockConsoleWrapper();
            string[] inputs = { "testMeeting1", "testPerson1", "meeting used for test.", "2", "1", "2022-01-26:13:30", "2022-01-26:14:30",
            "testMeeting2", "testPerson2", "meeting used for test.", "2", "1", "2022-01-26:12:00", "2022-01-26:12:30",
            "testMeeting3", "testPerson2", "meeting used for test.", "2", "1", "2022-01-26:12:00", "2022-01-26:12:30",
            "testMeeting4", "testPerson2", "meeting used for test2.", "1", "2", "2022-01-26:12:00", "2022-01-26:12:30",
            "testMeeting5", "testPerson3", "2meeting used for test.", "1", "2", "2022-01-26:12:00", "2022-01-26:12:30",
            "testMeeting6", "testPerson3", "meeting used2 for test.", "1", "2", "2022-01-26:12:00", "2022-01-26:12:30",
            "testMeeting7", "testPerson4", "meeting used for test.", "3", "1", "2022-01-26:12:00", "2022-01-26:12:30",
            "testMeeting8", "testPerson5", "meeting used for test.", "3", "1", "2022-01-26:12:00", "2022-01-26:12:30",
            "testMeeting9", "testPerson6", "meeting used for test.", "4", "1", "2022-01-26:12:00", "2022-01-26:12:30",
            "testMeeting10", "testPerson7", "meeting used for test.", "4", "1", "2022-01-26:13:30", "2022-01-26:14:00"};
            consoleWrapper.LinesToRead = new List<string>(inputs);
            List<Meeting> meetingList = new();
            while (consoleWrapper.LinesToRead.Count != 0)
            {
                Meeting meeting = new(consoleWrapper);
                meeting.CreateMeeting();
                meetingList.Add(meeting);
            }
            meetingList[6].Attendees = new List<string> { "TestAttendee1", "TestAttendee2", "TestAttendee3", "AddedAttendee3" };
            meetingList[7].Attendees = new List<string> { "TestAttendee1", "TestAttendee2", "TestAttendee3" };
            meetingList[8].Attendees = new List<string> { "TestAttendee1", "TestAttendee2", "TestAttendee3" };
            meetingList[9].Attendees = new List<string> { "TestAttendee1", "TestAttendee2", "TestAttendee3" };

            string jsonWrite = Newtonsoft.Json.JsonConvert.SerializeObject(meetingList);
            File.WriteAllText($"{Environment.CurrentDirectory}/meetingList/meetingList.Json", jsonWrite);
        }

        [Test]
        public void TestFilterByDescription()
        {
            var consoleWrapper = new MockConsoleWrapper();
            List<Meeting> expectedList = new List<Meeting> {
            new Meeting(consoleWrapper) {Name = "testMeeting4",ResponsiblePerson = "testPerson2", Description = "meeting used for test2.",Category = "CodeMonkey",Type = "InPerson",StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None),EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting5", ResponsiblePerson = "testPerson3", Description = "2meeting used for test.", Category = "CodeMonkey", Type = "InPerson", StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting6", ResponsiblePerson = "testPerson3", Description = "meeting used2 for test.", Category = "CodeMonkey", Type = "InPerson", StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)}
            };

            string[] inputs = { "1", "2", "2" };
            consoleWrapper.LinesToRead = new List<string>(inputs);
            CommandManager commandManager = new(consoleWrapper);
            string jsonRead = File.ReadAllText($"{Environment.CurrentDirectory}/MeetingList/meetingList.Json");
            List<Meeting> meetingList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Meeting>>(jsonRead);
            var filteredList = commandManager.FilterMeetings(meetingList);

            StringAssert.AreEqualIgnoringCase(Newtonsoft.Json.JsonConvert.SerializeObject(expectedList), filteredList);
        }

        [Test]
        public void TestFilterByResponsiblePerson()
        {
            var consoleWrapper = new MockConsoleWrapper();
            List<Meeting> expectedList = new List<Meeting> {
            new Meeting(consoleWrapper) {Name = "testMeeting2",ResponsiblePerson = "testPerson2", Description = "meeting used for test.",Category = "Hub",Type = "Live",StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None),EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting3", ResponsiblePerson = "testPerson2", Description = "meeting used for test.", Category = "Hub", Type = "Live", StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting4", ResponsiblePerson = "testPerson2", Description = "meeting used for test2.", Category = "CodeMonkey", Type = "InPerson", StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)}
            };

            string[] inputs = { "2", "testPerson2", "2" };
            consoleWrapper.LinesToRead = new List<string>(inputs);
            CommandManager commandManager = new(consoleWrapper);
            string jsonRead = File.ReadAllText($"{Environment.CurrentDirectory}/MeetingList/meetingList.Json");
            List<Meeting> meetingList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Meeting>>(jsonRead);
            var filteredList = commandManager.FilterMeetings(meetingList);

            StringAssert.AreEqualIgnoringCase(Newtonsoft.Json.JsonConvert.SerializeObject(expectedList), filteredList);
        }

        [Test]
        public void TestFilterByCategory()
        {
            var consoleWrapper = new MockConsoleWrapper();
            List<Meeting> expectedList = new List<Meeting> {
            new Meeting(consoleWrapper) {Name = "testMeeting4",ResponsiblePerson = "testPerson2", Description = "meeting used for test2.",Category = "CodeMonkey",Type = "InPerson",StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None),EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting5", ResponsiblePerson = "testPerson3", Description = "2meeting used for test.", Category = "CodeMonkey", Type = "InPerson", StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting6", ResponsiblePerson = "testPerson3", Description = "meeting used2 for test.", Category = "CodeMonkey", Type = "InPerson", StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)}
            };

            string[] inputs = { "3", "CodeMonkey", "2" };
            consoleWrapper.LinesToRead = new List<string>(inputs);
            CommandManager commandManager = new(consoleWrapper);
            string jsonRead = File.ReadAllText($"{Environment.CurrentDirectory}/MeetingList/meetingList.Json");
            List<Meeting> meetingList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Meeting>>(jsonRead);
            var filteredList = commandManager.FilterMeetings(meetingList);

            StringAssert.AreEqualIgnoringCase(Newtonsoft.Json.JsonConvert.SerializeObject(expectedList), filteredList);
        }

        [Test]
        public void TestFilterByType()
        {
            var consoleWrapper = new MockConsoleWrapper();
            List<Meeting> expectedList = new List<Meeting> {
            new Meeting(consoleWrapper) {Name = "testMeeting4",ResponsiblePerson = "testPerson2", Description = "meeting used for test2.",Category = "CodeMonkey",Type = "InPerson",StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None),EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting5", ResponsiblePerson = "testPerson3", Description = "2meeting used for test.", Category = "CodeMonkey", Type = "InPerson", StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting6", ResponsiblePerson = "testPerson3", Description = "meeting used2 for test.", Category = "CodeMonkey", Type = "InPerson", StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)}
            };

            string[] inputs = { "4", "InPerson", "2" };
            consoleWrapper.LinesToRead = new List<string>(inputs);
            CommandManager commandManager = new(consoleWrapper);
            string jsonRead = File.ReadAllText($"{Environment.CurrentDirectory}/MeetingList/meetingList.Json");
            List<Meeting> meetingList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Meeting>>(jsonRead);
            var filteredList = commandManager.FilterMeetings(meetingList);

            StringAssert.AreEqualIgnoringCase(Newtonsoft.Json.JsonConvert.SerializeObject(expectedList), filteredList);
        }

        [Test]
        public void TestFilterByStartDate()
        {
            var consoleWrapper = new MockConsoleWrapper();
            List<Meeting> expectedList = new List<Meeting> {
            new Meeting(consoleWrapper) {Name = "testMeeting1",ResponsiblePerson = "testPerson1", Description = "meeting used for test.",Category = "Hub",Type = "Live",StartDate = DateTime.ParseExact("2022-01-26:13:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None),EndDate = DateTime.ParseExact("2022-01-26:14:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting10", ResponsiblePerson = "testPerson7", Description = "meeting used for test.", Category = "TeamBuilding", Type = "Live", StartDate = DateTime.ParseExact("2022-01-26:13:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:14:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)}
            };
            expectedList[1].Attendees = new List<string> { "TestAttendee1", "TestAttendee2", "TestAttendee3" };

            string[] inputs = { "5", "2022-01-26:13:30", "2" };
            consoleWrapper.LinesToRead = new List<string>(inputs);
            CommandManager commandManager = new(consoleWrapper);
            string jsonRead = File.ReadAllText($"{Environment.CurrentDirectory}/MeetingList/meetingList.Json");
            List<Meeting> meetingList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Meeting>>(jsonRead);
            var filteredList = commandManager.FilterMeetings(meetingList);

            StringAssert.AreEqualIgnoringCase(Newtonsoft.Json.JsonConvert.SerializeObject(expectedList), filteredList);
        }

        [Test]
        public void TestFilterByAttendeeNumber()
        {
            var consoleWrapper = new MockConsoleWrapper();
            List<Meeting> expectedList = new List<Meeting> {
            new Meeting(consoleWrapper) {Name = "testMeeting8", ResponsiblePerson = "testPerson5", Description = "meeting used for test.", Category = "Short", Type = "Live", StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting9", ResponsiblePerson = "testPerson6", Description = "meeting used for test.", Category = "TeamBuilding", Type = "Live", StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting10", ResponsiblePerson = "testPerson7", Description = "meeting used for test.", Category = "TeamBuilding", Type = "Live", StartDate = DateTime.ParseExact("2022-01-26:13:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:14:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)}
            };
            expectedList[0].Attendees = new List<string> { "TestAttendee1", "TestAttendee2", "TestAttendee3" };
            expectedList[1].Attendees = new List<string> { "TestAttendee1", "TestAttendee2", "TestAttendee3" };
            expectedList[2].Attendees = new List<string> { "TestAttendee1", "TestAttendee2", "TestAttendee3" };

            string[] inputs = { "6", "3", "2" };
            consoleWrapper.LinesToRead = new List<string>(inputs);
            CommandManager commandManager = new(consoleWrapper);
            string jsonRead = File.ReadAllText($"{Environment.CurrentDirectory}/MeetingList/meetingList.Json");
            List<Meeting> meetingList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Meeting>>(jsonRead);
            var filteredList = commandManager.FilterMeetings(meetingList);
            Console.WriteLine(filteredList);

            StringAssert.AreEqualIgnoringCase(Newtonsoft.Json.JsonConvert.SerializeObject(expectedList), filteredList);
        }

        [Test]
        public void TestDeleteMeetingWithWrongUser()
        {
            var consoleWrapper = new MockConsoleWrapper();
            List<Meeting> expectedList = new List<Meeting> {
            new Meeting(consoleWrapper) {Name = "testMeeting1",ResponsiblePerson = "testPerson1", Description = "meeting used for test.",Category = "Hub",Type = "Live",StartDate = DateTime.ParseExact("2022-01-26:13:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None),EndDate = DateTime.ParseExact("2022-01-26:14:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting2",ResponsiblePerson = "testPerson2", Description = "meeting used for test.",Category = "Hub",Type = "Live",StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None),EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting3", ResponsiblePerson = "testPerson2", Description = "meeting used for test.", Category = "Hub", Type = "Live", StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting4", ResponsiblePerson = "testPerson2", Description = "meeting used for test2.", Category = "CodeMonkey", Type = "InPerson", StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting5", ResponsiblePerson = "testPerson3", Description = "2meeting used for test.", Category = "CodeMonkey", Type = "InPerson", StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting6", ResponsiblePerson = "testPerson3", Description = "meeting used2 for test.", Category = "CodeMonkey", Type = "InPerson", StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting7",ResponsiblePerson = "testPerson4", Description = "meeting used for test.",Category = "Short",Type = "Live",StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None),EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting8", ResponsiblePerson = "testPerson5", Description = "meeting used for test.", Category = "Short", Type = "Live", StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting9", ResponsiblePerson = "testPerson6", Description = "meeting used for test.", Category = "TeamBuilding", Type = "Live", StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting10", ResponsiblePerson = "testPerson7", Description = "meeting used for test.", Category = "TeamBuilding", Type = "Live", StartDate = DateTime.ParseExact("2022-01-26:13:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:14:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)}
            };
            expectedList[6].Attendees = new List<string> { "TestAttendee1", "TestAttendee2", "TestAttendee3", "AddedAttendee3" };
            expectedList[7].Attendees = new List<string> { "TestAttendee1", "TestAttendee2", "TestAttendee3" };
            expectedList[8].Attendees = new List<string> { "TestAttendee1", "TestAttendee2", "TestAttendee3" };
            expectedList[9].Attendees = new List<string> { "TestAttendee1", "TestAttendee2", "TestAttendee3" };

            string[] inputs = { "7" };
            consoleWrapper.LinesToRead = new List<string>(inputs);
            CommandManager commandManager = new(consoleWrapper);
            string jsonRead = File.ReadAllText($"{Environment.CurrentDirectory}/MeetingList/meetingList.Json");
            List<Meeting> meetingList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Meeting>>(jsonRead);
            User user = new();
            user.Name("testPerson5");
            var filteredList = commandManager.DeleteMeeting(meetingList, user);
            Console.WriteLine(filteredList);

            StringAssert.AreEqualIgnoringCase(Newtonsoft.Json.JsonConvert.SerializeObject(expectedList), filteredList);
        }

        [Test]
        public void TestDeleteMeetingWithRightUser()
        {
            var consoleWrapper = new MockConsoleWrapper();
            List<Meeting> expectedList = new List<Meeting> {
            new Meeting(consoleWrapper) {Name = "testMeeting1",ResponsiblePerson = "testPerson1", Description = "meeting used for test.",Category = "Hub",Type = "Live",StartDate = DateTime.ParseExact("2022-01-26:13:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None),EndDate = DateTime.ParseExact("2022-01-26:14:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting2",ResponsiblePerson = "testPerson2", Description = "meeting used for test.",Category = "Hub",Type = "Live",StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None),EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting3", ResponsiblePerson = "testPerson2", Description = "meeting used for test.", Category = "Hub", Type = "Live", StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting4", ResponsiblePerson = "testPerson2", Description = "meeting used for test2.", Category = "CodeMonkey", Type = "InPerson", StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting5", ResponsiblePerson = "testPerson3", Description = "2meeting used for test.", Category = "CodeMonkey", Type = "InPerson", StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting6", ResponsiblePerson = "testPerson3", Description = "meeting used2 for test.", Category = "CodeMonkey", Type = "InPerson", StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting8", ResponsiblePerson = "testPerson5", Description = "meeting used for test.", Category = "Short", Type = "Live", StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting9", ResponsiblePerson = "testPerson6", Description = "meeting used for test.", Category = "TeamBuilding", Type = "Live", StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting10", ResponsiblePerson = "testPerson7", Description = "meeting used for test.", Category = "TeamBuilding", Type = "Live", StartDate = DateTime.ParseExact("2022-01-26:13:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:14:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)}
            };
            expectedList[6].Attendees = new List<string> { "TestAttendee1", "TestAttendee2", "TestAttendee3" };
            expectedList[7].Attendees = new List<string> { "TestAttendee1", "TestAttendee2", "TestAttendee3" };
            expectedList[8].Attendees = new List<string> { "TestAttendee1", "TestAttendee2", "TestAttendee3" };

            string[] inputs = { "7" };
            consoleWrapper.LinesToRead = new List<string>(inputs);
            CommandManager commandManager = new(consoleWrapper);
            string jsonRead = File.ReadAllText($"{Environment.CurrentDirectory}/MeetingList/meetingList.Json");
            List<Meeting> meetingList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Meeting>>(jsonRead);
            User user = new();
            user.Name("testPerson4");
            var filteredList = commandManager.DeleteMeeting(meetingList, user);
            Console.WriteLine(filteredList);

            StringAssert.AreEqualIgnoringCase(Newtonsoft.Json.JsonConvert.SerializeObject(expectedList), filteredList);
        }

        [Test]
        public void TestAddPerson()
        {
            var consoleWrapper = new MockConsoleWrapper();
            List<Meeting> expectedList = new List<Meeting> {
            new Meeting(consoleWrapper) {Name = "testMeeting1",ResponsiblePerson = "testPerson1", Description = "meeting used for test.",Category = "Hub",Type = "Live",StartDate = DateTime.ParseExact("2022-01-26:13:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None),EndDate = DateTime.ParseExact("2022-01-26:14:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting2",ResponsiblePerson = "testPerson2", Description = "meeting used for test.",Category = "Hub",Type = "Live",StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None),EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting3", ResponsiblePerson = "testPerson2", Description = "meeting used for test.", Category = "Hub", Type = "Live", StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting4", ResponsiblePerson = "testPerson2", Description = "meeting used for test2.", Category = "CodeMonkey", Type = "InPerson", StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting5", ResponsiblePerson = "testPerson3", Description = "2meeting used for test.", Category = "CodeMonkey", Type = "InPerson", StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting6", ResponsiblePerson = "testPerson3", Description = "meeting used2 for test.", Category = "CodeMonkey", Type = "InPerson", StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting7",ResponsiblePerson = "testPerson4", Description = "meeting used for test.",Category = "Short",Type = "Live",StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None),EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting8", ResponsiblePerson = "testPerson5", Description = "meeting used for test.", Category = "Short", Type = "Live", StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting9", ResponsiblePerson = "testPerson6", Description = "meeting used for test.", Category = "TeamBuilding", Type = "Live", StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting10", ResponsiblePerson = "testPerson7", Description = "meeting used for test.", Category = "TeamBuilding", Type = "Live", StartDate = DateTime.ParseExact("2022-01-26:13:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:14:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)}
            };
            expectedList[1].Attendees = new List<string> { "AddedAttendee1", "AddedAttendee2", "AddedAttendee3" };
            expectedList[6].Attendees = new List<string> { "TestAttendee1", "TestAttendee2", "TestAttendee3", "AddedAttendee3" };
            expectedList[7].Attendees = new List<string> { "TestAttendee1", "TestAttendee2", "TestAttendee3" };
            expectedList[8].Attendees = new List<string> { "TestAttendee1", "TestAttendee2", "TestAttendee3" };
            expectedList[9].Attendees = new List<string> { "TestAttendee1", "TestAttendee2", "TestAttendee3" };

            string[] inputs = { "2" , "AddedAttendee1", "AddedAttendee1", "AddedAttendee2", "AddedAttendee3" , "Y" , "Exit" };
            consoleWrapper.LinesToRead = new List<string>(inputs);
            CommandManager commandManager = new(consoleWrapper);
            string jsonRead = File.ReadAllText($"{Environment.CurrentDirectory}/MeetingList/meetingList.Json");
            List<Meeting> meetingList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Meeting>>(jsonRead);
            var filteredList = commandManager.AddPerson(meetingList);
            Console.WriteLine(filteredList);

            StringAssert.AreEqualIgnoringCase(Newtonsoft.Json.JsonConvert.SerializeObject(expectedList), filteredList);
        }

        [Test]
        public void TestRemovePerson()
        {
            var consoleWrapper = new MockConsoleWrapper();
            List<Meeting> expectedList = new List<Meeting> {
            new Meeting(consoleWrapper) {Name = "testMeeting1",ResponsiblePerson = "testPerson1", Description = "meeting used for test.",Category = "Hub",Type = "Live",StartDate = DateTime.ParseExact("2022-01-26:13:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None),EndDate = DateTime.ParseExact("2022-01-26:14:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting2",ResponsiblePerson = "testPerson2", Description = "meeting used for test.",Category = "Hub",Type = "Live",StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None),EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting3", ResponsiblePerson = "testPerson2", Description = "meeting used for test.", Category = "Hub", Type = "Live", StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting4", ResponsiblePerson = "testPerson2", Description = "meeting used for test2.", Category = "CodeMonkey", Type = "InPerson", StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting5", ResponsiblePerson = "testPerson3", Description = "2meeting used for test.", Category = "CodeMonkey", Type = "InPerson", StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting6", ResponsiblePerson = "testPerson3", Description = "meeting used2 for test.", Category = "CodeMonkey", Type = "InPerson", StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting7",ResponsiblePerson = "testPerson4", Description = "meeting used for test.",Category = "Short",Type = "Live",StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None),EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting8", ResponsiblePerson = "testPerson5", Description = "meeting used for test.", Category = "Short", Type = "Live", StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting9", ResponsiblePerson = "testPerson6", Description = "meeting used for test.", Category = "TeamBuilding", Type = "Live", StartDate = DateTime.ParseExact("2022-01-26:12:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:12:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)},
            new Meeting(consoleWrapper) {Name = "testMeeting10", ResponsiblePerson = "testPerson7", Description = "meeting used for test.", Category = "TeamBuilding", Type = "Live", StartDate = DateTime.ParseExact("2022-01-26:13:30", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None), EndDate = DateTime.ParseExact("2022-01-26:14:00", "yyyy-MM-dd:HH:m", null, System.Globalization.DateTimeStyles.None)}
            };
            expectedList[6].Attendees = new List<string> { "TestAttendee1", "TestAttendee2", "TestAttendee3", "AddedAttendee3" };
            expectedList[7].Attendees = new List<string> { "TestAttendee1", "TestAttendee3" };
            expectedList[8].Attendees = new List<string> { "TestAttendee1", "TestAttendee2", "TestAttendee3" };
            expectedList[9].Attendees = new List<string> { "TestAttendee1", "TestAttendee2", "TestAttendee3" };

            string[] inputs = { "8", "2" , "Exit"};
            consoleWrapper.LinesToRead = new List<string>(inputs);
            CommandManager commandManager = new(consoleWrapper);
            string jsonRead = File.ReadAllText($"{Environment.CurrentDirectory}/MeetingList/meetingList.Json");
            List<Meeting> meetingList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Meeting>>(jsonRead);
            var filteredList = commandManager.RemovePerson(meetingList);
            Console.WriteLine(filteredList);

            StringAssert.AreEqualIgnoringCase(Newtonsoft.Json.JsonConvert.SerializeObject(expectedList), filteredList);
        }
    }
}