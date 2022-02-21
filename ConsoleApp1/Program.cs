using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SpeedRead
{
    /// <summary>
    /// Author: Steven Blanco
    /// Date: 12 / 29 / 21
    /// Version 1.0
    /// </summary>
    class Program
    {
        private const string MainMenu = @"
░██████╗██████╗░███████╗███████╗██████╗░██████╗░███████╗░█████╗░██████╗░
██╔════╝██╔══██╗██╔════╝██╔════╝██╔══██╗██╔══██╗██╔════╝██╔══██╗██╔══██╗
╚█████╗░██████╔╝█████╗░░█████╗░░██║░░██║██████╔╝█████╗░░███████║██║░░██║
░╚═══██╗██╔═══╝░██╔══╝░░██╔══╝░░██║░░██║██╔══██╗██╔══╝░░██╔══██║██║░░██║
██████╔╝██║░░░░░███████╗███████╗██████╔╝██║░░██║███████╗██║░░██║██████╔╝
╚═════╝░╚═╝░░░░░╚══════╝╚══════╝╚═════╝░╚═╝░░╚═╝╚══════╝╚═╝░░╚═╝╚═════╝░
                                                Created by Steven Blanco

    0) Exit
    1) Read (Choose where to start)
    2) Read (Begin where you left off)
    3) Settings
";

        private const string CollectionMenu = @"
█▀█ █ █▀▀ █▄▀   ▄▀█   █▀▀ █▀█ █░░ █░░ █▀▀ █▀▀ ▀█▀ █ █▀█ █▄░█
█▀▀ █ █▄▄ █░█   █▀█   █▄▄ █▄█ █▄▄ █▄▄ ██▄ █▄▄ ░█░ █ █▄█ █░▀█

    0) Go back
    1) Book of Mormon
    2) Bible Old Testament
    3) Bible New Testament
";

        private const string BookofMormonMenu = @"
█▄▄ █▀█ █▀█ █▄▀   █▀█ █▀▀   █▀▄▀█ █▀█ █▀█ █▀▄▀█ █▀█ █▄░█
█▄█ █▄█ █▄█ █░█   █▄█ █▀░   █░▀░█ █▄█ █▀▄ █░▀░█ █▄█ █░▀█

    0)  back
    1)  THE FIRST BOOK OF NEPHI HIS REIGN AND MINISTRY
    2)  THE SECOND BOOK OF NEPHI
    3)  THE BOOK OF JACOB
    4)  THE BOOK OF ENOS
    5)  THE BOOK OF JAROM
    6)  THE BOOK OF OMNI
    7)  THE WORDS OF MORMON
    8)  THE BOOK OF MOSIAH
    9)  THE BOOK OF ALMA
    10) THE BOOK OF HELAMAN
    11) THIRD BOOK OF NEPHI
    12) FOURTH NEPHI
    13) THE BOOK OF MORMON
    14) THE BOOK OF ETHER
    15) THE BOOK OF MORONI
";

        private const string ReadMenu = @"
█▀█ █▀▀ ▄▀█ █▀▄ █                       w to speed up || s to slow down 
█▀▄ ██▄ █▀█ █▄▀ ▄                       p to pause and unpause
                                        q to exit to main menu
";

        private const string OldTestamentMenu = @"
█▀█ █░░ █▀▄   ▀█▀ █▀▀ █▀ ▀█▀ ▄▀█ █▀▄▀█ █▀▀ █▄░█ ▀█▀
█▄█ █▄▄ █▄▀   ░█░ ██▄ ▄█ ░█░ █▀█ █░▀░█ ██▄ █░▀█ ░█░

    0)  Go back
    1)  The First Book of Moses:  Called Genesis                          
    2)  The Second Book of Moses:  Called Exodus                          
    3)  The Third Book of Moses:  Called Leviticus                        
    4)  The Fourth Book of Moses:  Called Numbers                         
    5)  The Fifth Book of Moses:  Called Deuteronomy                      
    6)  The Book of Joshua                                               
    7)  The Book of Judges                                               
    8)  The Book of Ruth                                                 
    9)  The First Book of Samuel                                         
    10) The Second Book of Samuel                                        
    11) The First Book of the Kings                                      
    12) The Second Book of the Kings                                     
    13) The First Book of the Chronicles                                 
    14) The Second Book of the Chronicles                                
    15) Ezra                                                             
    16) The Book of Nehemiah                                             
    17) The Book of Esther                                               
    18) The Book of Job                                                  
    19) The Book of Psalms                                               
    20) The Proverbs
    21) Ecclesiastes
    22) The Song of Solomon
    23) The Book of the Prophet Isaiah
    24) The Book of the Prophet Jeremiah
    25) The Lamentations of Jeremiah
    26) The Book of the Prophet Ezekiel
    27) The Book of Daniel
    28) Hosea
    29) Joel
    30) Amos
    31) Obadiah
    32) Jonah
    33) Micah
    34) Nahum
    35) Habakkuk
    36) Zephaniah
    37) Haggai
    38) Zechariah
    39) Malachi
";

        private const string NewTestamentMenu = @"
█▄░█ █▀▀ █░█░█   ▀█▀ █▀▀ █▀ ▀█▀ ▄▀█ █▀▄▀█ █▀▀ █▄░█ ▀█▀
█░▀█ ██▄ ▀▄▀▄▀   ░█░ ██▄ ▄█ ░█░ █▀█ █░▀░█ ██▄ █░▀█ ░█░

    0)  back
    1)  The New Testament of the King James Bible                        
    2)  The Gospel According to Saint Matthew                            
    3)  The Gospel According to Saint Mark                               
    4)  The Gospel According to Saint Luke                               
    5)  The Gospel According to Saint John                               
    6)  The Acts of the Apostles                                         
    7)  The Epistle of Paul the Apostle to the Romans                    
    8)  The First Epistle of Paul the Apostle to the Corinthians         
    9)  The Second Epistle of Paul the Apostle to the Corinthians
    10) The Epistle of Paul the Apostle to the Galatians
    11) The Epistle of Paul the Apostle to the Ephesians
    12) The Epistle of Paul the Apostle to the Philippians
    13) The Epistle of Paul the Apostle to the Colossians
    14) The First Epistle of Paul the Apostle to the Thessalonians
    15) The Second Epistle of Paul the Apostle to the Thessalonians
    16) The First Epistle of Paul the Apostle to Timothy
    17) The Second Epistle of Paul the Apostle to Timothy
    18) The Epistle of Paul the Apostle to Titus
    19) The Epistle of Paul the Apostle to Philemon
    20) The Epistle of Paul the Apostle to the Hebrews
    21) The General Epistle of James
    22) The First Epistle General of Peter
    23) The Second General Epistle of Peter
    24) The First Epistle General of John
    25) The Second Epistle General of John
    26) The Third Epistle General of John
    27) The General Epistle of Jude
    28) The Revelation of Saint John the Divine
";

        private static Settings settings;
        private static bool pause;
        private const string collectionPath = "../../../";
        private const int readMenuHeight = 6;
        private static System.Threading.Thread t;

        static void Main(string[] args)
        {
            t = new System.Threading.Thread(initialize);
            t.Start();
        }

        private static void initialize()
        {
            LoadSettings();
            DisplayMainMenu();
        }

        private static string GrabOptionName(string option, string text)
        {
            using (StringReader reader = new StringReader(text))
            {
                int lineNum = GetLineNum(option, text, "\r\n");

                // error checking
                if (lineNum == -1)
                    return "";

                // keep only the alphabetical and space characters
                string[] lines = text.Split("\r\n");
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (char letter in lines[lineNum])
                {
                    string s = letter.ToString();
                    if (Regex.IsMatch(s, "[A-Za-z]|:| "))
                    {
                        sb.Append(letter);
                    }
                }

                return sb.ToString().Trim();
            }
        }

        private static int GetLineNum(string subString, string s, string delimeter)
        {
            string[] lines;

            if (delimeter == "")
                lines = s.Split("\n");
            else
                lines = s.Split(delimeter);

            for (int i = 0; i < lines.Length - 1; i++)
            {
                if (i == 5)
                    Console.WriteLine("");

                if (lines[i].Contains(subString))
                {
                    return i;
                }
            }
            // failed to find the line
            return -1;
        }

        private static void LoadSettings()
        {
            try
            {
                string settingsAsJson = File.ReadAllText(Settings.settingsPath);
                settings = JsonConvert.DeserializeObject<Settings>(settingsAsJson);
            }
            catch
            {
                settings = new Settings();
                Error("Cannot Read save file");
            }
        }

        private static void DisplayMainMenu()
        {
            Console.Clear();
            Console.WriteLine(MainMenu);
            string choice = GetInput("[0-3]", "");

            switch (choice)
            {
                case "0":
                    return;
                case "1":
                    settings.SaveState();
                    DisplayCollectionMenu();
                    break;
                case "2":
                    settings.SaveState();
                    if (settings.HasValidPath())
                        ReadBook(settings.bookFilePath);
                    else
                    {
                        Error("You do not have a valid save file");
                    }
                    break;
                case "3":
                    settings.ChangeSettings();
                    initialize();
                    break;
            }
        }

        private static void Error(string message)
        {
            Console.Clear();
            Console.WriteLine(message);
            Console.WriteLine("redirecting to the main menu...");
            System.Threading.Thread.Sleep(2000);
            initialize();
        }

        private static void DisplayCollectionMenu()
        {
            Console.Clear();
            Console.WriteLine(CollectionMenu);

            string choice = GetInput("[0-3]", "");

            switch (choice)
            {
                case "0":
                    initialize();
                    break;
                case "1":
                    DisplayCollection(BookofMormonMenu, "BookofMormon.txt", "^[1][0-5]$|^[0-9]{1}$");
                    break;
                case "2":
                    DisplayCollection(OldTestamentMenu, "BibleOldTestament.txt", "^[0-9]{1}$|^[1-3][0-9]");
                    break;
                case "3":
                    DisplayCollection(NewTestamentMenu, "BibleNewTestament.txt", "^[0-9]{1}$|^[1][0-9]$|^[2][0-8]$");
                    break;
            }
        }

        private static void DisplayCollection(string menu, string dirName, string choices)
        {
            Console.Clear();
            Console.WriteLine(menu);
            Console.SetCursorPosition(0, 0);

            string path = collectionPath + dirName;
            string choice = GetInput(choices, "");

            if (choice == "0")
            {
                DisplayCollectionMenu();
                return;
            }

            // given a choice of a book, grab the line number of that book within the BOM collection
            System.Text.StringBuilder book = new System.Text.StringBuilder();
            foreach (string s in System.IO.File.ReadLines(path))
            {
                book.Append(s);
                book.Append("\n");
            }

            string option = GrabOptionName(choice, menu);
            if (option == "")
            {
                Error("Something went wrong, please contact steven");
            }

            settings.lineNumber = GetLineNum(option, book.ToString(), "");
            ReadBook(path);
        }

        private static string GetInput(string pattern, string errorMessage)
        {
            string choice;
            if (errorMessage == "")
                errorMessage = "Invalid input";

            while (true)
            {
                choice = Console.ReadLine();
                if (Regex.IsMatch(choice, "^" + pattern + "$"))
                    break;
                else
                    Console.WriteLine(errorMessage);
            }

            return choice;
        }

        /// READ part of the program below ///
        public static void ClearConsoleRead(int start)
        {
            for (int i = 0; i < Console.WindowHeight - start; i++)
            {
                Console.SetCursorPosition(0, start + i);
                Console.Write(new string(' ', Console.WindowWidth));
            }
            Console.SetCursorPosition(0, 0);
            Console.SetCursorPosition(0, start);
        }

        private static void ChangeSpeed(double factor)
        {
            double speed = settings.speed;
            speed = speed *= factor;
            settings.speed = (int)speed;

            // display the new speed
            Console.SetCursorPosition(0, readMenuHeight - 1);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, readMenuHeight - 1);
            Console.Write("Milliseconds per line " + settings.speed);
            Console.SetCursorPosition(0, readMenuHeight + settings.pageSize);
        }

        private static void GetInputRead()
        {
            // loop grabing input and changing the state of settings
            bool abort = false;
            while (!abort)
            {
                System.String input = Console.ReadLine();
                switch (input)
                {
                    case "q":
                        abort = true;
                        break;
                    case "w":
                        ChangeSpeed(.95);
                        break;
                    case "s":
                        ChangeSpeed(1.05);
                        break;
                    case "p":
                        pause = true;
                        while (pause)
                            System.Threading.Thread.Sleep(100);
                        break;
                }
                // clear line
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write(new string(' ', Console.WindowWidth));
            }
            Console.WriteLine("Quitting...");
        }

        private static void DisplaySaveSettings()
        {
            Console.Clear();
            Console.WriteLine("Save where you left off / save settings? (y, n)");
            string input = GetInput("[yn]", "Enter y or n");
            if (input == "y")
            {
                settings.Save();
                Console.WriteLine("Settings Saved!");
            }
            else if (input == "n")
            {
                Console.WriteLine("Settings not saved");
                string oldState = settings.GetOldState();
                settings = JsonConvert.DeserializeObject<Settings>(oldState);
            }
            Console.WriteLine("Redirecting to main menu...");
            System.Threading.Thread.Sleep(2000);
            initialize();
        }

        private static void ReadBook(string bookFilePath)
        {
            System.Threading.Thread acceptingInput = new System.Threading.Thread(GetInputRead);
            acceptingInput.Start();

            settings.bookFilePath = bookFilePath;

            Console.Clear();
            Console.WriteLine(ReadMenu);
            Console.WriteLine("Milliseconds per line " + settings.speed);

            if (Console.WindowHeight < settings.pageSize + 12)
                settings.MaxPageSize();

            string[] lines = System.IO.File.ReadLines(bookFilePath).ToArray();
            bool quitEarly = false;
            int displayBuffer = 0; // corrects the vertical spacing for sentences in buffer
            int currentWindowSize = Console.WindowHeight;
            for (int i = settings.lineNumber; i < lines.Length - 1; i++)
            {
                settings.lineNumber++;
                if (lines[i] == "") continue;

                if (currentWindowSize != Console.WindowHeight)
                {
                    settings.MaxPageSize();
                    currentWindowSize = Console.WindowHeight;
                }

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                int buffer = (displayBuffer++ % settings.pageSize);
                for (int j = 0; j < settings.pageSize; j++)
                {
                    if (j == buffer)
                        sb.Append(lines[i]);

                    sb.Append("\n");
                }

                System.Console.WriteLine(sb);
                System.Threading.Thread.Sleep((int)settings.speed);
                ClearConsoleRead(readMenuHeight);


                if (!acceptingInput.IsAlive)
                {
                    pause = false;
                    quitEarly = true;
                    break;
                }
                if (pause)
                {
                    Console.WriteLine("paused");
                    GetInput("[p]", "Press P to unPause");
                    pause = false;
                    ClearConsoleRead(settings.pageSize + readMenuHeight);
                }

                ClearConsoleRead(readMenuHeight);
            }
            if (!quitEarly)
            {
                ClearConsoleRead(readMenuHeight);
                Console.WriteLine("You finished the book!");
                settings.lineNumber = 0;
                System.Threading.Thread.Sleep(2000);
            }
            DisplaySaveSettings();
        }

    }
}
