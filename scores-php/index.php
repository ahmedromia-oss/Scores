<?php

require_once __DIR__ . '/vendor/autoload.php';

use Scores\Container;
use Scores\Interfaces\ICSVHelper;
use Scores\Interfaces\IJsonHelper;
use Scores\Interfaces\IStudentMapper;
use Scores\Models\AppSettings;

try {
    $config = require __DIR__ . '/config/config.php';
    
    // Create DI container
    $container = new Container($config);
    
    // Get services from container
    $logger = $container->get('logger');
    $csvHelper = $container->get('csvHelper');
    $jsonHelper = $container->get('jsonHelper');
    $studentMapper = $container->get('studentMapper');
    $appSettings = $container->get('appSettings');
    
    $logger->info("Application starting...");
    
    // Parse command line arguments
    $inputFilePath = $argv[1] ?? $appSettings->defaultInputPath;
    $outputFilePath = $argv[2] ?? ($appSettings->enableAutoSave ? $appSettings->defaultOutputPath : null);
    
    $logger->info("Starting CSV processing from: {$inputFilePath}");
    
   
    $rowsGenerator = $csvHelper->readCSVInChunks($inputFilePath, $appSettings->chunkSize);
    
    $studentSubjects = $studentMapper->mapAndSortScoresFromIterable($rowsGenerator);
    
    // Convert to JSON
    $json = $jsonHelper->convertToJson($studentSubjects);
    
    // Display JSON
    $jsonHelper->displayJson($json);
    
    // Save to file if output path is provided
    if (!empty($outputFilePath)) {
        $jsonHelper->saveToFile($json, $outputFilePath);
    }
    
    $logger->info("Processing completed successfully!");
    echo "\n Processing completed successfully!\n";
    
    exit(0);
    
} catch (\Exception $e) {
    echo "\nError: " . $e->getMessage() . "\n";
    
    if (isset($logger)) {
        $logger->error("Application terminated unexpectedly", [
            'exception' => $e->getMessage(),
            'trace' => $e->getTraceAsString()
        ]);
    }
    
    if (in_array('--verbose', $argv ?? [])) {
        echo "\nStack Trace:\n" . $e->getTraceAsString() . "\n";
    }
    
    exit(1);
}

