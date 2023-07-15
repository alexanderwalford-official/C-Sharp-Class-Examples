using System.ComponentModel.Design;
using System.IO;


void Main()
{
    Console.Clear();
    Console.Title = "Mass File Renamer / Fixer";
    string DESKTOP_NAME;
    string directory;

    DESKTOP_NAME = System.Environment.MachineName;

    Console.WriteLine("Mass File Renamer / Fixer");
    Console.WriteLine("By Alexander Walford 2022\n\n");
    Console.WriteLine("Computer name: " + DESKTOP_NAME + "\n\n");

    Console.WriteLine("Does the computer name match the affected files? (Y/N)");
    string inp = Console.ReadLine();
    if (inp.ToUpper() == "Y")
    {
        Console.WriteLine("Confirmed that the names match.");
    }
    else
    {
        Console.WriteLine("Enter the name present on the duplicate files (include dashses):");
        DESKTOP_NAME = Console.ReadLine();
    }

    Console.WriteLine("\nComputer name: " + DESKTOP_NAME + "\n\n");

    Console.WriteLine("Enter directory to fix > ");
    directory = Console.ReadLine();

    if (Directory.Exists(directory))
    {
        Console.WriteLine("Found directory " + directory);
        string[] dirs = Directory.GetDirectories(directory);
        int counter = 0;
        int counter2 = 0;

        // for the top level directory

        Console.WriteLine("Found " + dirs.Count() + " directories.");
        string[] files = Directory.GetFiles(directory);
        foreach (string file in files)
        {
            if (file.Contains(DESKTOP_NAME))
            {
                try
                {
                    // delete the old file and rename the new file
                    string RealFileName = file.Replace("-" + DESKTOP_NAME, "");
                    Console.WriteLine("RM " + RealFileName);
                    // delete old file
                    File.Delete(RealFileName);
                    Console.WriteLine("RN " + file + " to " + RealFileName);
                    // rename file
                    System.IO.File.Move(file, RealFileName);
                    counter++;
                }
                catch { }
            }
        }

        Console.WriteLine("Would you also like to check the recusrive directories? Y/N");
        string input = Console.ReadLine();

        if (input != null)
        {
            if (input.ToUpper().Contains("Y"))
            {
                // for recursion
                foreach (string dir in dirs)
                {
                    Console.WriteLine("Found " + dirs.Count() + " directories.");
                    string[] files2 = Directory.GetFiles(dir);
                    foreach (string file in files2)
                    {
                        if (file.Contains(DESKTOP_NAME))
                        {
                            try
                            {
                                // delete the old file and rename the new file
                                string RealFileName = file.Replace("-" + DESKTOP_NAME, "");
                                Console.WriteLine("RM " + RealFileName);
                                // delete old file
                                File.Delete(RealFileName);
                                Console.WriteLine("RN " + file + " to " + RealFileName);
                                // rename file
                                System.IO.File.Move(file, RealFileName);
                                counter++;
                            }
                            catch { }
                        }
                    }
                    counter2++;
                }
            }
            else
            {
                Console.WriteLine("Skipped recursive directory scanning.");
            }
        }

        Console.WriteLine("\n\n" + counter + " files and " + counter2 + " directories were fixed.");
        Console.Read();
        Main();
    }
    else
    {
        Console.WriteLine("The directory you entered does not exist. Please try again.");
        Console.ReadLine();
        Main();
    }
}

Main();
