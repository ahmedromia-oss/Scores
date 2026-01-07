<?php

namespace Scores\Services;

use Scores\Interfaces\IJsonHelper;
use Psr\Log\LoggerInterface;

class JsonHelper implements IJsonHelper
{
    public function __construct(
        private LoggerInterface $logger
    ) {}
    
    public function convertToJson(array $data): string
    {
        $this->logger->debug("Converting " . count($data) . " records to JSON");
        
        // Convert StudentSubject objects to arrays
        $arrayData = array_map(fn($item) => $item->toArray(), $data);
        
        $json = json_encode($arrayData, JSON_PRETTY_PRINT | JSON_UNESCAPED_SLASHES);
        
        if ($json === false) {
            throw new \RuntimeException('Failed to encode JSON: ' . json_last_error_msg());
        }
        
        return $json;
    }
    
    public function displayJson(string $json): void
    {
        echo "\n" . str_repeat('=', 60) . "\n";
        echo "JSON Output:\n";
        echo str_repeat('=', 60) . "\n";
        echo $json . "\n";
        echo str_repeat('=', 60) . "\n";
    }
    
    public function saveToFile(string $json, string $filePath): void
    {
        $this->logger->info("Saving JSON to file: {$filePath}");
        
        $directory = dirname($filePath);
        if (!is_dir($directory)) {
            mkdir($directory, 0777, true);
        }
        
        $result = file_put_contents($filePath, $json);
        
        if ($result === false) {
            $this->logger->error("Failed to save JSON to file: {$filePath}");
            throw new \RuntimeException("Failed to save JSON to file: {$filePath}");
        }
        
        $this->logger->info("Successfully saved JSON to: {$filePath}");
        echo "\n Output saved to: {$filePath}\n";
    }
}

