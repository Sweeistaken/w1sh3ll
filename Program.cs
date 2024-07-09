// Start the session by spitting out the version then defining variables
using System.Data;
using System.Runtime.InteropServices;

Console.WriteLine("W1SH3ll version 0.1a1");
void resetConsole()
{
    Console.ForegroundColor = ConsoleColor.Gray;
}
string prompt = Environment.UserName+"@"+System.Net.Dns.GetHostName();
var command = "";
string pwd = "";
string home = "";
void prepareConsole()
{
    command = "";
    pwd = Environment.CurrentDirectory;
    home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write(prompt);
    Console.ForegroundColor = ConsoleColor.Blue;
    if (pwd == home) { Console.Write(" ~ "); } else { Console.Write(" " + pwd.Replace(home, "~\\") + " "); }
    resetConsole();
    Console.Write("$ ");
}
while (true) {
    prepareConsole();
    command = Console.ReadLine();
    if (command != null)
    {
        string[] commands = command.Split(" ");
        if (commands[0] == "exit")
        {
            break;
        }
        if (commands[0] == "cd" | commands[0] == "chdir") {
            if (commands.Length > 1) {
                try
                {
                    var temp = commands.ToList();
                    temp.RemoveAt(0);
                    if (Path.Exists(string.Join(" ", temp))) {
                        Directory.SetCurrentDirectory(string.Join(" ", temp));
                    } else {
                        Directory.SetCurrentDirectory(pwd + "\\" + string.Join(" ", temp));
                    }
                    
                } catch(Exception ex)
                {
                    Console.WriteLine("chdir error: " + ex.ToString());
                }
            } else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("chdir error: Directory name expected.");
            }
        }
        if (commands[0] == "ls" | commands[0] == "dir")
        {
            List<string> tmp = Directory.GetFileSystemEntries(pwd, "*", SearchOption.TopDirectoryOnly).ToList();
            foreach (string i in tmp)
            {
                if (File.Exists(i))
                {
                    Console.WriteLine(Path.GetFileName(i));
                } else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(Path.GetFileName(i));
                    resetConsole();
                }
            }
        }
    }
}
