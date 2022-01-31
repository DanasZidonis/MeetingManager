using System;
using MeetingManager;

namespace MeetingManager
{
    class Program
    {
        public static void Main()
        {
            User user = new();
            ConsoleWrapper consoleWrapper = new();
            CommandManager controller = new(consoleWrapper);
            string answer;

            consoleWrapper.WriteLine("Enter your name.");
            user.Name(consoleWrapper.ReadLine());

            do
            {
                Console.Clear();
                answer = controller.AwaitCommand(user);
                consoleWrapper.WriteLine(answer);
            }while (answer != "Exit");

        }
    }
}


