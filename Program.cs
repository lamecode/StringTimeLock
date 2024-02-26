using StringTimeLock;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

class Program
{
    static void Main()
    {
        PasswordManager passwordManager = new PasswordManager();

        // Check if the master password is already set
        string masterPassword = passwordManager.ReadMasterPassword();
        if (string.IsNullOrEmpty(masterPassword))
        {
            // If not set, prompt the user to set the master password
            Console.WriteLine("Set the master password:");
            masterPassword = Console.ReadLine();
            passwordManager.SaveMasterPassword(masterPassword);
            Console.WriteLine("Master password set successfully.");
        }

        while (true)
        {
            UIAddons.DrawHeader(">>>>>  StringTimeLock: Welcome  <<<<<");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Press a number on your keyboard to start an action:");
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("[ 1 ]");
            Console.ResetColor();
            Console.WriteLine("\tStore new string/password");

            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("[ 2 ]");
            Console.ResetColor();
            Console.WriteLine("\tRetrieve a string/password");

            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("[ 3 ]");
            Console.ResetColor();
            Console.WriteLine("\tTerminate the application");
            Console.ResetColor();

            do
            {
                ConsoleKeyInfo kbKey = Console.ReadKey(true);

                switch (kbKey.KeyChar)
                {
                    case '1':
                        passwordManager.StorePassword();
                        break;
                    case '2':
                        passwordManager.RetrievePassword(masterPassword);
                        break;
                    case '3':
                        return; // Terminate the application
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        continue; // Restart the loop
                }

                // Exit the loop if a valid option (1 or 2) is selected
                break;
            }
            while (true);
        }
    }
}