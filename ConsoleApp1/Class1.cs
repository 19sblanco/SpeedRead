using Newtonsoft.Json;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace SpeedRead

{
    /// <summary>
    /// class for creating settings
    /// will be serialized into a file using json
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class Settings
    {

        public const string settingsPath = "../../../Settings.txt";

        private const string s = @"
█▀ █▀▀ ▀█▀ ▀█▀ █ █▄░█ █▀▀ █▀
▄█ ██▄ ░█░ ░█░ █ █░▀█ █▄█ ▄█

    0) Go back                                  
    1) Speed     {0}                            
    2) Page size {1}
    3) Revert to Default settings

    i) More info about settings
";

        public int speed = 5000;
        public int lineNumber = 0;
        public string bookFilePath = "";
        public int pageSize = 10;
        public bool updated = false;
        public bool exit = false;

        private string temperaryState;

        public Settings()
        {

        }
        public void ChangeSettings()
        {
            try
            {
                Console.Clear();
                string newS = string.Format(s, speed, pageSize);
                Console.WriteLine(newS);
                string choice = GetInput("[0-3]|i", "");

                switch (choice)
                {
                    case "0":
                        exit = true;
                        break;
                    case "1":
                        ChangeSpeed();
                        break;
                    case "2":
                        ChangePageSize();
                        break;
                    case "3":
                        Revert();
                        break;
                    case "i":
                        ExplainSettings();
                        break;
                }
                if (!exit)
                    ChangeSettings();
                exit = false;
            }
            catch
            {
                Console.WriteLine("Error Occurred");
                Thread.Sleep(2000);
            }
        }

        public void ExplainSettings()
        {
            Console.Clear();
            Console.WriteLine(@"
Speed:
    The number of milliseconds that a line will display for 

    ***
    1000 milliseconds are in  1 second, meaning...
    500  milliseconds is  1/2 a second, and
    5000 milliseconds is  5   seconds
    
Page size:  
    This determines how many vertical lines of space in which text will print down (page size)

    ***
    Page Size = 1: display each line of text on the same line.
    Page Size = 10: starting at the top of the window, display the
    first line of text. Next frame display the second line of 
    text a line below the first. Simularly the third will print under the
    second and so on, continuing to 10. After the 10th, the next line will print
    at the top of the page repeating the cycle.

Revert to default:  
    Reverts the settings to their default values and removes bookmark

    ***
    speed = 5000
    Page size = 10

");
            String message = "Enter i to go back to settings";
            Console.WriteLine(message);
            while (GetInput("i", message) != "i") ;
        }

        public void ChangeSpeed()
        {
            Console.WriteLine("How many milliseconds per line would you like? [1 to 10,000]");
            speed = Int32.Parse(GetInput("^([1-9][0-9]{0,3}|10000)$", ""));
            Save();
        }

        public void ChangePageSize()
        {
            int useAbleHeight = Console.WindowHeight - 12;
            string s = "What would you like the page size to be? [1 to {0}]";
            Console.WriteLine(string.Format(s, useAbleHeight));
            int choice;
            while (true)
            {
                choice = Int32.Parse(GetInput("[0-9]+", ""));

                if (choice <= useAbleHeight)
                    break;

                Console.WriteLine(string.Format("Page size must be between 1 and {0}", useAbleHeight));
            }
            pageSize = choice;
            Save();
        }

        public void MaxPageSize()
        {
            if (Console.WindowHeight > 12)
                pageSize = Console.WindowHeight - 12;
            else
                pageSize = 1;
        }

        private static string GetInput(string pattern, string message)
        {
            string choice;

            while (true)
            {
                choice = Console.ReadLine();
                if (Regex.IsMatch(choice, pattern))
                    break;
                else
                {
                    if (message == "")
                        Console.WriteLine("Invalid input");
                    else
                        Console.WriteLine(message);
                }

            }

            return choice;
        }
        private void Revert()
        {
            speed = 5000;
            lineNumber = 0;
            bookFilePath = "";
            pageSize = 10;
            updated = false;
            exit = false;
            Save();
        }

        public void Save()
        {
            string saveFile = JsonConvert.SerializeObject(this);

            File.WriteAllText(settingsPath, System.String.Empty);
            File.WriteAllText(settingsPath, saveFile);
        }

        public void SaveState()
        {
            temperaryState = JsonConvert.SerializeObject(this);
        }

        public string GetOldState()
        {
            return temperaryState;
        }

        public bool HasValidPath()
        {
            try
            {
                System.IO.File.ReadLines(bookFilePath);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
