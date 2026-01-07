<?php

namespace Scores\Interfaces;

interface IScoreSortingStrategy
{
    public function sortScores(array $scores): array;
}

