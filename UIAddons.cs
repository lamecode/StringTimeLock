using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace StringTimeLock
{
    internal class UIAddons
    {
        /// <summary>
        /// Draws colored header as intro.
        /// </summary>
        /// <param name="subjectName">Name of your company</param>
        public static void DrawHeader(string subjectName)
        {
            StripedLine(subjectName.Length);
            Console.WriteLine(subjectName);
            StripedLine(subjectName.Length);
        }
        /// <summary>
        /// Creates a striped line.
        /// </summary>
        /// <param name="amount">Amount of dashes on single line</param>
        private static void StripedLine(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                if (i % 2 == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("-");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("-");
                }
            }
            Console.WriteLine();
            Console.ResetColor();
        }
        /// <summary>
        /// Animation that says "Press any key to continue" and waits for user before clearing the console. Console Clear not included!
        /// </summary>
        /// <param name="character">Takes any character that will be animated. Leave empty for ">"</param>
        public static void ConfirmContinue(string message, char character = '>')
        {
            Console.CursorVisible = false;
            Console.Write(message);

            while (!Console.KeyAvailable)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(character + "  ");
                Console.Write(new string('\b', 3));
                Thread.Sleep(200);
                Console.Write(" " + character + " ");
                Console.Write(new string('\b', 3));
                Thread.Sleep(200);
                Console.Write("  " + character);
                Console.Write(new string('\b', 3));
                Thread.Sleep(200);
                Console.Write("   ");
                Console.Write(new string('\b', 3));
                Thread.Sleep(400);
                Console.ResetColor();
            }

            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
        }

        public static void PasswordDecryptedMessageAndConfirmation()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Decrypted password: ");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write("  Hidden  ");
            Console.WriteLine();
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Are you ready to unhide the string? ");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Y ");
            Console.ResetColor();
            Console.Write("/");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(" N");
            Console.WriteLine();
            Console.ResetColor();
        }
        public static void DecryptedPasswordLabel()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Decrypted password: ");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Red;
        }
        /// <summary>
        /// Draws Reason for terminating the app
        /// </summary>
        /// <param name="choice">Fills |CHOICE| and |REASON| in this message: You chose |CHOICE|, ~the app will now be terminated for following reason: |REASON|.</param>
        public static void TerminationMessage(string choice, string reason)
        {
            Console.Write("You chose ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(choice);
            Console.ResetColor();
            Console.WriteLine(", the application will now be terminated for the following reason:");
            Console.ForegroundColor= ConsoleColor.Magenta;
            Console.WriteLine(reason);
            UIAddons.ConfirmContinue("Press any key to continue ");
        }
        /// <summary>
        /// Writes the message in yellow
        /// </summary>
        /// <param name="text">text for the yellow message</param>
        public static void MessageColorDarkYellow(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        /// <summary>
        /// Writes the message in red
        /// </summary>
        /// <param name="text">text for the red message</param>
        public static void MessageColorBrightRed(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}
