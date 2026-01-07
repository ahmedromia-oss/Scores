<?php

namespace Scores\Interfaces;

interface IStudentMapper
{
    public function mapToStudentSubjects(array $rows): array;
    
    public function mapAndSortScores(array $rows): array;
    
    public function mapAndSortScoresFromIterable(iterable $rows): array;
}

