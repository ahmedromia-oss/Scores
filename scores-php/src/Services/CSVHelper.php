<?php

namespace Scores\Services;

use Scores\Interfaces\ICSVHelper;
use Scores\Models\StudentScoreRow;
use Psr\Log\LoggerInterface;

class CSVHelper implements ICSVHelper
{
    public function __construct(
        private LoggerInterface $logger
    ) {}
    
    public function readCSVInChunks(string $filePath, int $chunkSize = 11): \Generator
    {
        $this->validateFilePath($filePath);
        
        if (!file_exists($filePath)) {
            $this->logger->error("CSV file not found: {$filePath}");
            throw new \RuntimeException("CSV file not found: {$filePath}");
        }
        
        $this->logger->info("Starting CSV processing from {$filePath} with chunk size {$chunkSize}");
        
        $handle = fopen($filePath, 'r');
        if ($handle === false) {
            throw new \RuntimeException("Could not open file: {$filePath}");
        }
        
        try {
            $processedCount = 0;
            $errorCount = 0;
            $headers = null;
            $chunk = [];
            
            while (($data = fgetcsv($handle)) !== false) {
                if ($headers === null) {
                    $headers = $data;
                    continue;
                }
                
                if (empty(array_filter($data))) {
                    continue;
                }
                
                $rowData = array_combine($headers, $data);
                
                if ($rowData === false) {
                    $errorCount++;
                    $this->logger->warning("Skipped invalid row - column count mismatch");
                    continue;
                }
                
                $row = StudentScoreRow::fromArray($rowData);
                
                if ($row->isValid()) {
                    $chunk[] = $row;
                    $processedCount++;
                    
                    if (count($chunk) >= $chunkSize) {
                        $this->logger->debug("Yielding chunk with {$chunkSize} records (total processed: {$processedCount})");
                        yield $chunk;
                        $chunk = [];
                    }
                } else {
                    $errorCount++;
                    $this->logger->warning("Skipped invalid record");
                }
            }
            
            if (!empty($chunk)) {
                $this->logger->debug("Yielding final chunk with " . count($chunk) . " records");
                yield $chunk;
            }
            
            $this->logger->info("CSV processing completed. Total rows processed: {$processedCount}, Errors: {$errorCount}");
        } finally {
            fclose($handle);
        }
    }
    
    private function validateFilePath(string $filePath): void
    {
        if (empty($filePath)) {
            throw new \InvalidArgumentException('File path cannot be empty');
        }
        
        $realPath = realpath($filePath);
        if ($realPath === false && !file_exists($filePath)) {
            $this->logger->warning("Potential path traversal attempt detected: {$filePath}");
        }
    }
}

