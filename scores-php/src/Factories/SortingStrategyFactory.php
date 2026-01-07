<?php

namespace Scores\Factories;

use Scores\Interfaces\IScoreSortingStrategy;
use Scores\Interfaces\ISortingStrategyFactory;
use Scores\Strategies\DefaultSortingStrategy;
use Psr\Log\LoggerInterface;

class SortingStrategyFactory implements ISortingStrategyFactory
{
    private array $strategies = [];
    
    public function __construct(
        private LoggerInterface $logger
    ) {}
    
    public function getStrategy(string $subject): IScoreSortingStrategy
    {
        $key = strtolower($subject);
        
        if (isset($this->strategies[$key])) {
            $this->logger->debug("Found strategy for subject: {$subject}");
            return $this->strategies[$key];
        }
        
        $this->logger->warning("No strategy found for subject: {$subject}, using default strategy");
        return new DefaultSortingStrategy();
    }
    
    public function registerStrategy(string $subject, IScoreSortingStrategy $strategy): void
    {
        if (empty($subject)) {
            throw new \InvalidArgumentException('Subject cannot be empty');
        }
        
        $key = strtolower($subject);
        $this->strategies[$key] = $strategy;
        $this->logger->info("Registered strategy for subject: {$subject}");
    }
}

