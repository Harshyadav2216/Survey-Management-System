# Survey Management System

A simple console-based application in C# that allows users to participate in and manage surveys. The system can record survey responses, store them in a file, and display them when needed.

## 💡 Features

- Create and conduct surveys via command-line interface
- Store survey data persistently using file handling
- View and manage collected responses
- Basic error handling and user prompts

## 🛠️ Tech Stack

- **Language**: C#
- **Framework**: .NET Framework (Console App)
- **File Handling**: Text files (`survey_data.txt`)
- **IDE**: Visual Studio

## 📁 Project Structure
survey_system/
│
├── survey_data.txt # Stores survey responses
├── Program.cs # Main logic for the app
├── survey_system.csproj # C# project file
├── App.config # Application config (if used)
├── bin/ and obj/ # Build outputs
└── .vs/ # Visual Studio settings


## 🚀 How to Run

1. Open the project in **Visual Studio**.
2. Build the solution (`Ctrl + Shift + B`).
3. Run the program (`Ctrl + F5` or Run without Debugging).

Alternatively, run the compiled `.exe` file found in:


## 📝 Usage

On running the application, follow on-screen instructions to:

- Fill out a new survey
- View stored survey results
- Exit the application

## 📦 Data Storage

All survey responses are stored in a simple text file: `survey_data.txt`. Each record typically includes respondent's input in a structured format.

## 🙌 Contribution

Pull requests and suggestions are welcome! Feel free to fork the repository and enhance the project.

## 📄 License

This project is open-source and available under the [MIT License](LICENSE).
