<?php

namespace Scores\Interfaces;

interface IJsonHelper
{
    public function convertToJson(array $data): string;
    
    public function displayJson(string $json): void;
    
    public function saveToFile(string $json, string $filePath): void;
}

