<?php

namespace Scores\Strategies;

use Scores\Interfaces\IScoreSortingStrategy;
use Scores\Models\StudentScore;

abstract class EnumSortingStrategy implements IScoreSortingStrategy
{
    protected string $enumClass;
    
    public function sortScores(array $scores): array
    {
        usort($scores, function(StudentScore $a, StudentScore $b) {
            $valueA = $this->getEnumValue($a->score);
            $valueB = $this->getEnumValue($b->score);
            
            return $valueB <=> $valueA;
        });
        
        return $scores;
    }
    
    protected function getEnumValue(string $score): int
    {
        $enumClass = $this->enumClass;

        foreach ($enumClass::cases() as $case) {
            if ($case->name === $score) {
                return $case->value;
            }
        }

        foreach ($enumClass::cases() as $case) {
            if (strcasecmp($case->name, $score) === 0) {
                return $case->value;
            }
        }

        if (is_numeric($score)) {
            try {
                $enum = $enumClass::from((int)$score);
                return $enum->value;
            } catch (\ValueError $e) {
            }
        }

        return 0;
    }
}

