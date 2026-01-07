# Scores Application (PHP Version)

A PHP 8.0+ application for processing student scores from CSV files and generating JSON output with sorted results.

This is a PHP port of the .NET C# version, maintaining the same architecture patterns:
- **Strategy Pattern** for different scoring systems
- **Factory Pattern** for strategy selection
- **Dependency Injection** for loose coupling
- **Clean Architecture** with interfaces and separation of concerns

## Prerequisites
- PHP 8.0 or later
- Composer (PHP dependency manager)

## Installation

```bash
cd scores-php
composer install
```

## Running the Application

```bash
# Use default settings from config.php
php index.php

# Specify custom input file
php index.php /path/to/your/scores.csv

# Specify both input and output files
php index.php /path/to/your/scores.csv /path/to/output.json

# With verbose error output
php index.php --verbose
```

---

## Configuration (config/config.php)

The application reads its configuration from `config/config.php`:

```php
return [
    'app' => [
        'default_input_path' => 'D:\\scores.csv',
        'default_output_path' => 'D:\\output.json',
        'chunk_size' => 100,
        'enable_auto_save' => false,
    ],
    
    'logging' => [
        'path' => __DIR__ . '/../logs/scores.log',
        'level' => 'INFO', // DEBUG, INFO, WARNING, ERROR
    ],
];
```

### **default_input_path**
- **This is the file path where the program will read the CSV file from**
- Default: `D:\\scores.csv`
- Change this to point to your CSV file location

### **default_output_path**
- **This is where the program will save the JSON output file**
- Default: `D:\\output.json`
- Only saves if `enable_auto_save` is set to `true`

### **chunk_size**
- Controls how many rows are processed at once in streaming mode
- Default: `100`
- The application uses memory-efficient streaming processing that reads rows one at a time
- This setting affects logging frequency (logs progress every N rows)
- Works efficiently even with very large CSV files

### **enable_auto_save**
- Controls whether output is automatically saved
- Default: `false`
- Set to `true` to automatically save JSON files

---

## Input CSV Format

Your CSV file should have these columns:

```csv
Student ID,Name,Learning Objective,Score,Subject
1,John Smith,Algebra,A,Maths
1,John Smith,Grammar,Seven,English
1,John Smith,Chemistry,Excellent,Science
```

---

## Scoring Systems

### English Scores
- **Values:** One, Two, Three, Four, Five, Six, Seven, Eight
- **Sorting:** Best to worst (Eight → One)

### Maths Scores
- **Values:** A, B, C, D, E, F
- **Sorting:** Best to worst (A → F)

### Science Scores
- **Values:** Excellent, Good, Average, Poor, VeryPoor
- **Sorting:** Best to worst (Excellent → VeryPoor)

---

## Adding New Subjects

To add a new subject with different grades:

### 1. Create Enum in `src/Enums/YourSubjectScore.php`

```php
<?php
namespace Scores\Enums;

enum HistoryScore: int
{
    case Outstanding = 4;
    case VeryGood = 3;
    case Satisfactory = 2;
    case NeedsImprovement = 1;
}
```

### 2. Create Strategy in `src/Strategies/YourSubjectSortingStrategy.php`

```php
<?php
namespace Scores\Strategies;

use Scores\Enums\HistoryScore;

class HistorySortingStrategy extends EnumSortingStrategy
{
    protected string $enumClass = HistoryScore::class;
}
```

### 3. Register Strategy in `src/Container.php`

In the `registerServices()` method, add:

```php
$factory->registerStrategy('History', new HistorySortingStrategy());
```

---

## Project Structure

```
scores-php/
├── composer.json           # PHP dependencies
├── index.php              # Main entry point
├── config/
│   └── config.php         # Configuration file
├── src/
│   ├── Container.php      # Dependency Injection container
│   ├── Enums/            # Grade enumerations
│   ├── Interfaces/       # Service interfaces
│   ├── Models/           # Data models
│   ├── Services/         # Business logic services
│   ├── Strategies/       # Sorting strategies
│   └── Factories/        # Factory classes
├── logs/                 # Application logs
└── vendor/               # Composer dependencies
```

---

## Architecture Patterns

### Strategy Pattern
Different subjects have different scoring systems, implemented as separate strategy classes.

### Factory Pattern
`SortingStrategyFactory` selects the appropriate strategy based on subject name.

### Dependency Injection
All dependencies are injected through constructors, managed by the `Container` class.

### Interface Segregation
Clean interfaces separate concerns: `ICSVHelper`, `IJsonHelper`, `IStudentMapper`, etc.

### Memory-Efficient Streaming
Uses PHP generators to process CSV files row-by-row without loading the entire file into memory. This allows processing of very large CSV files (100MB+) with minimal memory footprint.

---

## Logging

Logs are written to:
- Console output (real-time)
- `logs/scores.log` file

Log levels: DEBUG, INFO, WARNING, ERROR

---

## Output Format

JSON output grouped by student and subject:

```json
[
  {
    "student_id": 1,
    "student_name": "John Smith",
    "subject": "Maths",
    "scores": [
      {
        "learning_objective": "Algebra",
        "score": "A"
      }
    ]
  }
]
```

---





