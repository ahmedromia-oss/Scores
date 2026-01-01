# Scores Application

A .NET console application for processing student scores from CSV files and generating JSON output with sorted results.

## Prerequisites
- .NET 10.0 SDK or later

## Running the Application
```bash
cd Scores
dotnet run
```

---

## Configuration (appsettings.json)

The application reads its configuration from `Scores/appsettings.json`:

```json
{
  "AppSettings": {
    "DefaultInputPath": "D:\\scores.csv",
    "DefaultOutputPath": "D:\\output.json",
    "ChunkSize": 100,
    "EnableAutoSave": false
  }
}
```

### **DefaultInputPath**
- **This is the file path where the program will read the CSV/Excel file from**
- Default: `D:\\scores.csv`
- Change this to point to your CSV file location
- Example: `"DefaultInputPath": "C:\\Users\\YourName\\Documents\\scores.csv"`
- **Important:** Use double backslashes (`\\`) in Windows paths

### **DefaultOutputPath**
- **This is where the program will save the JSON output file**
- Default: `D:\\output.json`
- Only saves if `EnableAutoSave` is set to `true`
- Example: `"DefaultOutputPath": "C:\\Results\\output.json"`

---

## How to Change File Paths

1. Open `Scores/appsettings.json` in any text editor
2. Update the paths:
   ```json
   {
     "AppSettings": {
       "DefaultInputPath": "C:\\MyFolder\\my-data.csv",
       "DefaultOutputPath": "C:\\MyFolder\\results.json",
       "EnableAutoSave": true
     }
   }
   ```
3. Save the file
4. Run the program: `dotnet run`
