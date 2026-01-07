<?php

namespace Scores\Models;

class StudentSubject
{
    public function __construct(
        public int $studentId = 0,
        public string $studentName = '',
        public string $subject = '',
        public array $scores = []
    ) {}
    
    public function toArray(): array
    {
        return [
            'student_id' => $this->studentId,
            'student_name' => $this->studentName,
            'subject' => $this->subject,
            'scores' => array_map(fn($score) => $score->toArray(), $this->scores)
        ];
    }
}

