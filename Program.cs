// Start the session
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;
resetConsole();
Console.WriteLine("W1SH3ll version 0.1a1");
void resetConsole()
{
    Console.ForegroundColor = ConsoleColor.Gray;
}
string prompt = Environment.UserName+"@"+System.Net.Dns.GetHostName();
var command = "";
string pwd = "";
int exitcode = 0;
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
    if (exitcode != 0) {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(exitcode.ToString() + " ");
        resetConsole();
    }
    Console.Write("$ ");
}
while (true) {
    prepareConsole();
    exitcode = 0;
    command = Console.ReadLine();
    if (command != null)
    {
        command = command.Replace("~", home);
        string[] commands = command.Split(" ");
        if (commands[0] == "exit")
        {
            break;
        }
        else if (commands[0] == "cd" | commands[0] == "chdir") {
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

                } catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("chdir error: " + ex.Message);
                    exitcode++;
                }
            } else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("chdir error: Directory name expected.");
                exitcode++;
            }
        }
        else if (commands[0] == "ls" | commands[0] == "dir")
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
        else if (commands[0] == "clear")
        {
            Console.Clear();
        }
        else
        {
            var temp = commands.ToList();
            temp.RemoveAt(0);
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = commands[0],
                    Arguments = string.Join(" ", temp)
                }
            };
            try
            {
                process.Start();
                process.WaitForExit();
            }
            catch (FileNotFoundException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Command not found.");
            }
            catch (InvalidOperationException)
            {
                continue;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
            }
            try {
                exitcode = process.ExitCode;
            } catch
            {
                exitcode = 127;
            }
            
        }
    }
}
