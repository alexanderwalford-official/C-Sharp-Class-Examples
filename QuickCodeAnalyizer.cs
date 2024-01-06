

void main()
{
    int files_c = 0;
    int folders_c = 0;
    int lines_c = 0;

    Console.Title = "Quick Code Analyizer";

    Console.WriteLine("Quick Code Analyizer\nScan how many files, directories and how many lines of code your project has.\n\n");
    Console.Write("Enter the directory you wish to scan: ");
    string directory = Console.ReadLine();
    Console.WriteLine("\nThe application will now scan the directory: " + directory);
    Console.Write("\nContinue? (y/n): ");
    string response = Console.ReadLine().ToLower();
    if (response == "y")
    {
        Console.WriteLine("\n\nCONFIRMED, starting scan..");
        if (Directory.Exists(directory))
        {
            Console.WriteLine("\nEnter the file extensions you wish to scan:");
            Console.Write("EXT > ");
            string extension = Console.ReadLine();

            // impement any folder names the user wishes to ignore
            Console.WriteLine("Are there any folders you wish to ignore?\nIf so, please enter them as a list seperated by commas below. Otherwise just hit enter without typing anything. ");
            Console.Write("Folder names to ignore > ");

            string folder_in = Console.ReadLine();
            string[] folders_to_ignore = folder_in.Split(",");


            if (extension != "")
            {
                Console.WriteLine("\nFound the directory, getting files & subdirectories..");

                string[] dirs = Directory.GetDirectories(directory);

                // realistically this is not recursive enough, folders indside of folders will not be counted

                foreach (string dir in dirs)
                {
                    bool ignore_folder = false;
                    foreach (string ignores in folders_to_ignore)
                    {
                        if (dir == ignores)
                        {
                            ignore_folder = true;
                        }
                    }
                    if (!ignore_folder)
                    {
                        Console.WriteLine(dir);
                        folders_c++;
                        string[] files = Directory.GetFiles(dir);
                        foreach (string file in files)
                        {
                            if (file.Contains(extension))
                            {
                                Console.WriteLine(file);
                                files_c++;

                                string[] lines = File.ReadAllLines(file);
                                int lines_count_local = 0;
                                foreach (string line in lines)
                                {
                                    if (line != "")
                                    {
                                        lines_count_local++;
                                        lines_c++;
                                    }
                                }
                                Console.WriteLine(lines_count_local);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("IGNORE: " + dir);
                    }
                }
            }
            Console.WriteLine("\n\nDONE! Found " + folders_c.ToString() + " folders, " + files_c.ToString() + " files with a total of " + lines_c.ToString() + " lines of code in total.");
            Console.Read();
            Console.Clear();
            main();
        }
        else
        {
            Console.WriteLine("ERROR: Could not locate the directory you entered. Please try again..");
            Console.Read();
            Console.Clear();
            main();
        }
    }
    else
    {
        Console.Clear();
        main();
    }
}


main();