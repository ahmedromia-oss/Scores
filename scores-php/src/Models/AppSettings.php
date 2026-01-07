<?php

namespace Scores\Models;

class AppSettings
{
    public function __construct(
        public string $defaultInputPath = 'scores.csv',
        public string $defaultOutputPath = 'output.json',
        public int $chunkSize = 100,
        public bool $enableAutoSave = false
    ) {}
    
    public static function fromArray(array $config): self
    {
        return new self(
            $config['default_input_path'] ?? 'scores.csv',
            $config['default_output_path'] ?? 'output.json',
            $config['chunk_size'] ?? 100,
            $config['enable_auto_save'] ?? false
        );
    }
}

