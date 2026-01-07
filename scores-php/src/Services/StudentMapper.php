<?php

namespace Scores\Services;

use Scores\Interfaces\IStudentMapper;
use Scores\Interfaces\ISortingStrategyFactory;
use Scores\Models\StudentScore;
use Scores\Models\StudentSubject;
use Psr\Log\LoggerInterface;

class StudentMapper implements IStudentMapper
{
    public function __construct(
        private ISortingStrategyFactory $strategyFactory,
        private LoggerInterface $logger
    ) {}
    
    public function mapToStudentSubjects(array $rows): array
    {
        $this->logger->info("Mapping " . count($rows) . " rows to student subjects");
        
        $grouped = [];
        
        foreach ($rows as $row) {
            $key = "{$row->studentId}_{$row->subject}";
            
            if (!isset($grouped[$key])) {
                $grouped[$key] = new StudentSubject(
                    studentId: $row->studentId,
                    studentName: $row->name,
                    subject: $row->subject,
                    scores: []
                );
            }
            
            $grouped[$key]->scores[] = new StudentScore(
                learningObjective: $row->learningObjective,
                score: $row->score
            );
        }
        
        $studentSubjects = array_values($grouped);
        
        $this->logger->info("Mapped to " . count($studentSubjects) . " student subjects");
        
        return $studentSubjects;
    }
    
    public function mapAndSortScores(array $rows): array
    {
        $this->logger->info("Starting mapping and sorting process for " . count($rows) . " rows");
        
        $studentSubjects = $this->mapToStudentSubjects($rows);
        
        foreach ($studentSubjects as $student) {
            $this->logger->debug("Sorting scores for student {$student->studentId} - {$student->subject}");
            $strategy = $this->strategyFactory->getStrategy($student->subject);
            $student->scores = $strategy->sortScores($student->scores);
        }
        
        $this->logger->info("Mapping and sorting completed successfully");
        
        return $studentSubjects;
    }
    
    public function mapAndSortScoresFromIterable(iterable $rows): array
    {
        $this->logger->info("Starting streaming mapping and sorting process");
        
        $grouped = [];
        $processedCount = 0;
        
        foreach ($rows as $chunk) {
            foreach ($chunk as $row) {
                $key = "{$row->studentId}_{$row->subject}";
                
                if (!isset($grouped[$key])) {
                    $grouped[$key] = new StudentSubject(
                        studentId: $row->studentId,
                        studentName: $row->name,
                        subject: $row->subject,
                        scores: []
                    );
                }
                
                $grouped[$key]->scores[] = new StudentScore(
                    learningObjective: $row->learningObjective,
                    score: $row->score
                );
                
                $processedCount++;
            }
            
            $this->logger->debug("Processed chunk: {$processedCount} total rows processed so far");
            
            unset($chunk);
        }
        
        $this->logger->info("Mapped {$processedCount} rows to " . count($grouped) . " student subjects");
        
        // Sort scores for each student subject
        $studentSubjects = array_values($grouped);
        
        foreach ($studentSubjects as $student) {
            $this->logger->debug("Sorting scores for student {$student->studentId} - {$student->subject}");
            $strategy = $this->strategyFactory->getStrategy($student->subject);
            $student->scores = $strategy->sortScores($student->scores);
        }
        
        $this->logger->info("Streaming mapping and sorting completed successfully");
        
        return $studentSubjects;
    }
}

