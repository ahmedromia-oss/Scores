<?php

namespace Scores;

use Monolog\Logger;
use Monolog\Handler\StreamHandler;
use Monolog\Formatter\LineFormatter;
use Scores\Factories\SortingStrategyFactory;
use Scores\Interfaces\ICSVHelper;
use Scores\Interfaces\IJsonHelper;
use Scores\Interfaces\IStudentMapper;
use Scores\Interfaces\ISortingStrategyFactory;
use Scores\Models\AppSettings;
use Scores\Services\CSVHelper;
use Scores\Services\JsonHelper;
use Scores\Services\StudentMapper;
use Scores\Strategies\EnglishSortingStrategy;
use Scores\Strategies\MathsSortingStrategy;
use Scores\Strategies\ScienceSortingStrategy;

class Container
{
    private array $services = [];
    private array $config;
    
    public function __construct(array $config)
    {
        $this->config = $config;
        $this->registerServices();
    }
    
    private function registerServices(): void
    {
        // Register Logger
        $this->services['logger'] = function() {
            $logger = new Logger('scores');
            
            $logPath = $this->config['logging']['path'];
            $logLevel = constant(Logger::class . '::' . $this->config['logging']['level']);
            
            $handler = new StreamHandler($logPath, $logLevel);
            
            $formatter = new LineFormatter(
                $this->config['logging']['format'],
                'Y-m-d H:i:s',
                true,
                true
            );
            $handler->setFormatter($formatter);
            
            $logger->pushHandler($handler);
            
            // Also log to console
            $consoleHandler = new StreamHandler('php://stdout', $logLevel);
            $consoleHandler->setFormatter($formatter);
            $logger->pushHandler($consoleHandler);
            
            return $logger;
        };
        
        // Register AppSettings
        $this->services['appSettings'] = function() {
            return AppSettings::fromArray($this->config['app']);
        };
        
        // Register SortingStrategyFactory
        $this->services['sortingStrategyFactory'] = function() {
            $factory = new SortingStrategyFactory($this->get('logger'));
            
            // Register strategies
            $factory->registerStrategy('English', new EnglishSortingStrategy());
            $factory->registerStrategy('Maths', new MathsSortingStrategy());
            $factory->registerStrategy('Science', new ScienceSortingStrategy());
            
            return $factory;
        };
        
        // Register Services
        $this->services['csvHelper'] = function() {
            return new CSVHelper($this->get('logger'));
        };
        
        $this->services['jsonHelper'] = function() {
            return new JsonHelper($this->get('logger'));
        };
        
        $this->services['studentMapper'] = function() {
            return new StudentMapper(
                $this->get('sortingStrategyFactory'),
                $this->get('logger')
            );
        };
    }
    
    public function get(string $serviceName)
    {
        if (!isset($this->services[$serviceName])) {
            throw new \RuntimeException("Service not found: {$serviceName}");
        }
        
        $service = $this->services[$serviceName];
        
        if ($service instanceof \Closure) {
            $this->services[$serviceName] = $service();
        }
        
        return $this->services[$serviceName];
    }
}

