<?php

namespace Scores\Interfaces;

interface ICSVHelper
{
    public function readCSVInChunks(string $filePath, int $chunkSize = 100): \Generator;
}

