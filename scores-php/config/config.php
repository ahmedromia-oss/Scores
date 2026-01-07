<?php

return [
    'app' => [
        'default_input_path' => 'D:\\scores.csv',
        'default_output_path' => 'D:\\output.json',
        'chunk_size' => 11,
        'enable_auto_save' => false,
    ],
    
    'logging' => [
        'path' => __DIR__ . '/../logs/scores.log',
        'level' => 'INFO', // DEBUG, INFO, WARNING, ERROR
        'format' => '[%datetime%] %level_name%: %message% %context%' . PHP_EOL,
    ],
];

