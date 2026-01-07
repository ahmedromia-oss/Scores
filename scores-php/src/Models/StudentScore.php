<?php

namespace Scores\Models;

class StudentScore
{
    public function __construct(
        public string $learningObjective = '',
        public string $score = ''
    ) {}
    
    public function toArray(): array
    {
        return [
            'learning_objective' => $this->learningObjective,
            'score' => $this->score
        ];
    }
}

