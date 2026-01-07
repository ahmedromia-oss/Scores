<?php

namespace Scores\Models;

class StudentScoreRow
{
    public function __construct(
        public int $studentId = 0,
        public string $name = '',
        public string $learningObjective = '',
        public string $score = '',
        public string $subject = ''
    ) {}
    
    public static function fromArray(array $data): self
    {
        return new self(
            (int)($data['Student ID'] ?? $data['student_id'] ?? 0),
            $data['Name'] ?? $data['name'] ?? '',
            $data['Learning Objective'] ?? $data['learning_objective'] ?? '',
            $data['Score'] ?? $data['score'] ?? '',
            $data['Subject'] ?? $data['subject'] ?? ''
        );
    }
    
    public function isValid(): bool
    {
        return $this->studentId > 0
            && !empty($this->name)
            && !empty($this->learningObjective)
            && !empty($this->score)
            && !empty($this->subject);
    }
}

