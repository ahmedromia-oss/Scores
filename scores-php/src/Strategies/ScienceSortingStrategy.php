<?php

namespace Scores\Strategies;

use Scores\Enums\ScienceScore;

class ScienceSortingStrategy extends EnumSortingStrategy
{
    protected string $enumClass = ScienceScore::class;
}

