<?php

namespace Scores\Strategies;

use Scores\Interfaces\IScoreSortingStrategy;

class DefaultSortingStrategy implements IScoreSortingStrategy
{
    public function sortScores(array $scores): array
    {
        return $scores;
    }
}

