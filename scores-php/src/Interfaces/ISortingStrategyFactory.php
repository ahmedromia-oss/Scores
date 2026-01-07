<?php

namespace Scores\Interfaces;

interface ISortingStrategyFactory
{
    public function getStrategy(string $subject): IScoreSortingStrategy;
    
    public function registerStrategy(string $subject, IScoreSortingStrategy $strategy): void;
}

