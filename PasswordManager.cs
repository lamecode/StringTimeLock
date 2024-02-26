using StringTimeLock;
using System.Security.Cryptography;
using System.Text;
class PasswordManager
{
    private const string generalEncryptionKey = "0xFF_demonstrationEncryptionKey";
    private const string dataFolder = "data";
    private const string descFolder = "data/desc";
    public void StorePassword()
    {
        UIAddons.MessageColorDarkYellow("Enter the string (or password) you want to store:");
        string inputString = Console.ReadLine();

        UIAddons.MessageColorDarkYellow("Enter a description for the password/string:");
        string description = Console.ReadLine();

        // Encrypt the string using AES encryption
        string encryptedString = EncryptString(inputString, generalEncryptionKey);

        // Generate a shorter ID for the stored string
        string id = GenerateShortID();

        // Create the data folder if it doesn't exist
        Directory.CreateDirectory(dataFolder);

        // Store the encrypted string in a file inside the data folder
        File.WriteAllText(Path.Combine(dataFolder, $"{id}_data"), encryptedString);

        // Create the desc folder if it doesn't exist
        Directory.CreateDirectory(descFolder);

        // Store the description in a file inside the desc folder
        File.WriteAllText(Path.Combine(descFolder, $"{id}_desc"), description);

        UIAddons.MessageColorDarkYellow("String stored successfully.");
    }

    public void RetrievePassword(string masterPassword)
    {
        UIAddons.MessageColorDarkYellow("Enter the master password:");
        string enteredPassword = Console.ReadLine();

        if (enteredPassword == masterPassword)
        {
            UIAddons.MessageColorDarkYellow("Master password accepted. Retrieving stored strings:");

            // List all stored password IDs and descriptions
            string[] files = Directory.GetFiles(descFolder, "*_desc");
            foreach (string file in files)
            {
                string id = Path.GetFileNameWithoutExtension(file).Replace("_desc", "");
                string description = File.ReadAllText(file);
                Console.WriteLine($"ID: {id}\t String/Password description: {description}");
            }

            while (true)
            {
                UIAddons.MessageColorDarkYellow("Enter the ID of the string/password you want to retrieve:");
                string selectedId = Console.ReadLine();

                // Check if the selected ID exists
                if (File.Exists(Path.Combine(dataFolder, $"{selectedId}_data")))
                {
                    // Read the encrypted password
                    string encryptedString = File.ReadAllText(Path.Combine(dataFolder, $"{selectedId}_data"));

                    // Decrypt and print the password with a delay
                    DecryptAndPrint(encryptedString);
                    break; // Exit the loop if the password is retrieved successfully
                }
                else
                {
                    UIAddons.MessageColorBrightRed($"Password with ID '{selectedId}' does not exist. Please enter a valid ID.");
                    continue; // Repeat the loop to prompt for a valid ID
                }
            }
        }
        else
        {
            UIAddons.MessageColorBrightRed("Incorrect master password. Decryption failed.");
            UIAddons.TerminationMessage("wrong master password", "[Security reason] Countering bruteforce.");
            Environment.Exit(0);
        }
    }


    public string ReadMasterPassword()
    {
        try
        {

            // Read the encrypted master password from the file
            string encryptedMasterPassword = File.ReadAllText("master.key");

            // Decrypt the master password
            return DecryptString(encryptedMasterPassword, generalEncryptionKey);
        }
        catch (FileNotFoundException)
        {
            return null;
        }
    }

    public void SaveMasterPassword(string password)
    {

        // Encrypt the master password
        string encryptedMasterPassword = EncryptString(password, generalEncryptionKey);

        // Save the encrypted master password to the file
        File.WriteAllText("master.key", encryptedMasterPassword);
    }

    private string GenerateShortID()
    {
        // Generate a short alphanumeric ID
        const string chars = "abcdefghijklmnopqrstuvwxyz123456789";
        Random random = new Random();
        return new string(Enumerable.Repeat(chars, 5)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private string EncryptString(string input, string key)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.IV = new byte[16]; // Use zero initialization vector for simplicity

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(input);
                    }
                }
                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }

    private string DecryptString(string input, string key)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.IV = new byte[16];

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(input)))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }

    private void DecryptAndPrint(string encryptedString)
    {
        string decryptedString = DecryptString(encryptedString, generalEncryptionKey);

        Console.WriteLine("Retrieving string:");

        // Print the password with a set delay
        int totalSeconds = 10; // Set the total number of seconds

        for (int i = totalSeconds; i > 0; i--)
        {
            int hours = i / 3600;
            int minutes = (i % 3600) / 60;
            int seconds = i % 60;

            Console.Clear();
            UIAddons.DrawHeader(">>>>>  StringTimeLock: Delaying the decode.  <<<<<");
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"Decryption delayed: {hours:D2}h {minutes:D2}m {seconds:D2}s remaining.");
            Thread.Sleep(1000);
            Console.ResetColor();
        }

        Console.Clear();
        UIAddons.DrawHeader(">>>>>  StringTimeLock: Decoded string ready  <<<<<");
        UIAddons.ConfirmContinue("String or password is ready, press any key to continue.\n");

        // Prompt for master password again
        Console.Clear();
        UIAddons.DrawHeader(">>>>>  StringTimeLock: Confirm master password  <<<<<");
        UIAddons.MessageColorDarkYellow("Enter the master password again to view the decrypted string:");
        string enteredPassword = Console.ReadLine();

        Console.Clear();
        UIAddons.DrawHeader(">>>>>  StringTimeLock: Showing decoded string  <<<<<");
        if (enteredPassword == ReadMasterPassword())
        {
            UIAddons.PasswordDecryptedMessageAndConfirmation();

            ConsoleKeyInfo confirmation = Console.ReadKey(true);

            while (true)
            {
                switch (confirmation.KeyChar)
                {
                    case 'y':
                    case 'Y':
                        Console.Clear();
                        UIAddons.DrawHeader(">>>>>  StringTimeLock: Showing decrypted password!  <<<<<");
                        UIAddons.DecryptedPasswordLabel();
                        Console.WriteLine(decryptedString);
                        Console.ResetColor();
                        break;
                    case 'n':
                    case 'N':
                        Console.Clear();
                        UIAddons.DrawHeader(">>>>>  StringTimeLock: Operation cancelled!  <<<<<");
                        UIAddons.TerminationMessage("'Not Ready'", "You were not ready. Operation cancelled by user.");
                        UIAddons.ConfirmContinue("Press any key to terminate the app.", '×');
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        continue; // Restart the loop
                }
                break;
            }

            UIAddons.MessageColorDarkYellow("Copy the password. When you are ready, press 'Y' on your keyboard to close the application.");

            while (true)
            {
                ConsoleKeyInfo terminationKey = Console.ReadKey(true);

                switch (terminationKey.KeyChar)
                {
                    case 'y':
                    case 'Y':
                        UIAddons.TerminationMessage("'Y > Close the App'", "Program terminated by user.");
                        Environment.Exit(0);
                        return;
                    default:
                        UIAddons.MessageColorBrightRed("Invalid choice. Please press 'Y' to close the application.");
                        continue; // Repeat the loop
                }
            }


        }
        else
        {
            UIAddons.MessageColorBrightRed("Incorrect master password. Decryption failed.");
            UIAddons.TerminationMessage("wrong master password", "[Security reason] Countering bruteforce.");
            Environment.Exit(0);
        }
    }
}