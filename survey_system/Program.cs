using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;

class Program
{
    private const string FILE_PATH = "survey_data.txt";
    private const string RECORD_SEPARATOR = "\n----------------------------------------\n";

    // Function to add a new survey response
    static void AddSurveyResponse()
    {
        try
        {
            // Collect user input
            Console.Write("Enter User ID: ");
            string userId = Console.ReadLine()?.Trim() ?? "";
            if (string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("\nUser ID cannot be empty!");
                return;
            }

            Console.Write("Enter Name: ");
            string name = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Enter Age: ");
            string age = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Enter Father's Name: ");
            string fatherName = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Enter Marital Status (Married/Unmarried): ");
            string maritalStatus = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Enter Occupation: ");
            string occupation = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Enter Salary (Per Month): ");
            string salary = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Enter Gender (Male/Female/Other): ");
            string gender = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Enter Number of Family Members: ");
            string familyMembers = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Enter Home/Building No.: ");
            string homeNo = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Enter Village: ");
            string village = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Enter Post Office: ");
            string postOffice = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Enter Police Station: ");
            string policeStation = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Enter District: ");
            string district = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Enter State: ");
            string state = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Enter Pincode: ");
            string pincode = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Enter Mobile Number: ");
            string mobileNumber = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Enter Vehicles and their Number (e.g., Car: XYZ123, Bike: ABC456): ");
            string vehicles = Console.ReadLine()?.Trim() ?? "";

            // Prepare the record as a single string
            string record = $"User ID: {userId}\n" +
                            $"Name: {name}\n" +
                            $"Age: {age}\n" +
                            $"Father's Name: {fatherName}\n" +
                            $"Marital Status: {maritalStatus}\n" +
                            $"Occupation: {occupation}\n" +
                            $"Salary: {salary}\n" +
                            $"Gender: {gender}\n" +
                            $"Family Members: {familyMembers}\n" +
                            $"Home/Building No.: {homeNo}\n" +
                            $"Village: {village}\n" +
                            $"Post Office: {postOffice}\n" +
                            $"Police Station: {policeStation}\n" +
                            $"District: {district}\n" +
                            $"State: {state}\n" +
                            $"Pincode: {pincode}\n" +
                            $"Mobile Number: {mobileNumber}\n" +
                            $"Vehicles: {vehicles}";

            // Append the record to the file
            using (StreamWriter file = File.AppendText(FILE_PATH))
            {
                // If the file is not empty, add a separator before the new record
                if (new FileInfo(FILE_PATH).Length > 0)
                {
                    file.Write(RECORD_SEPARATOR);
                }
                file.Write(record);
            }

            Console.WriteLine("\nSurvey Response Added Successfully!\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError adding survey response: {ex.Message}\n");
        }
    }

    // Function to check if file exists and is not empty
    static bool FileExists()
    {
        return File.Exists(FILE_PATH) && new FileInfo(FILE_PATH).Length > 0;
    }

    // Function to read records one at a time (stream-based)
    static IEnumerable<string> ReadRecords()
    {
        if (!FileExists())
        {
            yield break;
        }

        using (StreamReader reader = new StreamReader(FILE_PATH))
        {
            StringBuilder recordBuilder = new StringBuilder();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Trim() == "----------------------------------------" && recordBuilder.Length > 0)
                {
                    yield return recordBuilder.ToString().Trim();
                    recordBuilder.Clear();
                }
                else
                {
                    recordBuilder.AppendLine(line);
                }
            }

            // Handle the last record if it exists
            if (recordBuilder.Length > 0)
            {
                yield return recordBuilder.ToString().Trim();
            }
        }
    }

    // Function to write records back to the file
    static void WriteRecords(IEnumerable<string> records)
    {
        using (StreamWriter writer = new StreamWriter(FILE_PATH, false))
        {
            bool firstRecord = true;
            foreach (string record in records)
            {
                if (!firstRecord)
                {
                    writer.Write(RECORD_SEPARATOR);
                }
                writer.Write(record);
                firstRecord = false;
            }
        }
    }

    // Function to show all survey responses
    static void ShowAllResponses()
    {
        try
        {
            if (!FileExists())
            {
                Console.WriteLine("\nNo survey responses found!");
                return;
            }

            List<(string userId, string name, string record)> userList = new List<(string, string, string)>();
            foreach (string record in ReadRecords())
            {
                string[] lines = record.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                string userId = "";
                string name = "";
                foreach (string line in lines)
                {
                    if (!string.IsNullOrEmpty(line) && line.Contains(": "))
                    {
                        string[] parts = line.Split(new[] { ": " }, StringSplitOptions.None);
                        if (parts.Length >= 2)
                        {
                            if (line.StartsWith("User ID:"))
                                userId = parts[1].Trim();
                            else if (line.StartsWith("Name:"))
                                name = parts[1].Trim();
                        }
                    }
                }
                if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(name))
                {
                    userList.Add((userId, name, record));
                }
            }

            if (!userList.Any())
            {
                Console.WriteLine("\nNo survey responses found!");
                return;
            }

            Console.WriteLine("\nAvailable Survey Responses:");
            foreach (var (userId, name, _) in userList)
            {
                Console.WriteLine($"{userId}: {name}"); // Modified to show both User ID and Name
            }

            Console.Write("\nEnter User ID to view full details (or press Enter to go back): ");
            string searchQuery = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(searchQuery))
                return;

            bool found = false;
            foreach (var (userId, _, record) in userList)
            {
                if (userId == searchQuery)
                {
                    Console.WriteLine("\nFull Details:\n" + record);
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Console.WriteLine("\nNo matching User ID found!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError showing survey responses: {ex.Message}\n");
        }
    }

    // Function to search response by user ID or name
    static void SearchResponse()
    {
        try
        {
            if (!FileExists())
            {
                Console.WriteLine("\nNo survey responses found!");
                return;
            }

            Console.Write("Enter User ID or Name to search: ");
            string searchQuery = Console.ReadLine()?.Trim().ToLower();
            if (string.IsNullOrEmpty(searchQuery))
            {
                Console.WriteLine("\nPlease enter a valid search query!");
                return;
            }

            bool found = false;
            foreach (string record in ReadRecords())
            {
                if (record.ToLower().Contains(searchQuery))
                {
                    Console.WriteLine("\nMatch Found!\n" + record);
                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine("\nNo matching record found!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError searching survey responses: {ex.Message}\n");
        }
    }

    // Function to update a specific field in a survey response
    static void UpdateResponse()
    {
        try
        {
            if (!FileExists())
            {
                Console.WriteLine("\nNo survey responses found!");
                return;
            }

            Console.Write("Enter User ID to update: ");
            string searchQuery = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(searchQuery))
            {
                Console.WriteLine("\nPlease enter a valid User ID!");
                return;
            }

            List<string> updatedRecords = new List<string>();
            bool found = false;

            foreach (string record in ReadRecords())
            {
                string[] lines = record.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                string userId = "";
                foreach (string line in lines)
                {
                    if (line.StartsWith("User ID:"))
                    {
                        string[] parts = line.Split(new[] { ": " }, StringSplitOptions.None);
                        if (parts.Length >= 2)
                        {
                            userId = parts[1].Trim();
                        }
                        break;
                    }
                }

                if (userId == searchQuery)
                {
                    Console.WriteLine("\nMatch Found! Current Details:\n" + record);

                    string[] fields = new[] {
                        "Name",
                        "Age", 
                        "Father's Name",
                        "Marital Status", 
                        "Occupation",
                        "Salary",
                        "Gender",
                        "Family Members",
                        "Home/Building No.",
                        "Village",
                        "Post Office",
                        "Police Station", 
                        "District",
                        "State", 
                        "Pincode",
                        "Mobile Number", 
                        "Vehicles"
                    };

                    Console.WriteLine("\nSelect fields to update (comma separated numbers):");
                    for (int i = 0; i < fields.Length; i++)
                    {
                        Console.WriteLine($"{i + 1}. {fields[i]}");
                    }

                    Console.Write("\nEnter numbers (e.g., 1, 3, 5): ");
                    string[] choices = Console.ReadLine()?.Trim().Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                    string[] updatedRecord = record.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string choice in choices)
                    {
                        if (int.TryParse(choice.Trim(), out int index) && index > 0 && index <= fields.Length)
                        {
                            string fieldToUpdate = fields[index - 1];
                            Console.Write($"Enter new value for {fieldToUpdate}: ");
                            string newValue = Console.ReadLine()?.Trim() ?? "";

                            for (int i = 0; i < updatedRecord.Length; i++)
                            {
                                if (updatedRecord[i].StartsWith(fieldToUpdate + ":"))
                                {
                                    updatedRecord[i] = $"{fieldToUpdate}: {newValue}";
                                }
                            }
                        }
                    }

                    updatedRecords.Add(string.Join("\n", updatedRecord));
                    found = true;
                }
                else
                {
                    updatedRecords.Add(record);
                }
            }

            if (found)
            {
                WriteRecords(updatedRecords);
                Console.WriteLine("\nSurvey Response Updated Successfully!");
            }
            else
            {
                Console.WriteLine("\nNo matching record found!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError updating survey response: {ex.Message}\n");
        }
    }

    // Function to delete a survey response
    static void DeleteResponse()
    {
        try
        {
            if (!FileExists())
            {
                Console.WriteLine("\nNo survey responses found!");
                return;
            }

            Console.Write("Enter User ID or Name to delete: ");
            string searchQuery = Console.ReadLine()?.Trim().ToLower();
            if (string.IsNullOrEmpty(searchQuery))
            {
                Console.WriteLine("\nPlease enter a valid search query!");
                return;
            }

            List<string> updatedRecords = new List<string>();
            bool found = false;

            foreach (string record in ReadRecords())
            {
                if (record.ToLower().Contains(searchQuery))
                {
                    Console.WriteLine("\nSurvey Response Deleted Successfully!");
                    found = true;
                }
                else
                {
                    updatedRecords.Add(record);
                }
            }

            if (found)
            {
                WriteRecords(updatedRecords);
            }
            else
            {
                Console.WriteLine("\nNo matching record found!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError deleting survey response: {ex.Message}\n");
        }
    }

    // Function to show statistics
    static void ShowStatistics()
    {
        try
        {
            if (!FileExists())
            {
                Console.WriteLine("\nNo survey responses found!");
                return;
            }

            int count = 0;
            foreach (string record in ReadRecords())
            {
                if (!string.IsNullOrWhiteSpace(record) && record.Contains("User ID:"))
                {
                    count++;
                }
            }
            Console.WriteLine($"\nTotal Number of Surveys Conducted: {count}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError showing statistics: {ex.Message}\n");
        }
    }

    // Main Menu
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n==== Survey System Menu ====");
            Console.WriteLine("1. Add Survey Response");
            Console.WriteLine("2. Show All Survey Responses");
            Console.WriteLine("3. Search Survey Response");
            Console.WriteLine("4. Update Survey Response");
            Console.WriteLine("5. Delete Survey Response");
            Console.WriteLine("6. Show Survey Statistics");
            Console.WriteLine("7. Exit");
            Console.Write("\nEnter your choice: ");
            string choice = Console.ReadLine()?.Trim();

            switch (choice)
            {
                case "1":
                    AddSurveyResponse();
                    break;
                case "2":
                    ShowAllResponses();
                    break;
                case "3":
                    SearchResponse();
                    break;
                case "4":
                    UpdateResponse();
                    break;
                case "5":
                    DeleteResponse();
                    break;
                case "6":
                    ShowStatistics();
                    break;
                case "7":
                    Console.WriteLine("\nExiting Survey System. Thanks For Your Valuable Time!\n");
                    return;
                default:
                    Console.WriteLine("\nInvalid choice! Please enter a valid option.");
                    break;
            }
        }
    }
}