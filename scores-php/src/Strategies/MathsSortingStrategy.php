<?php

namespace Scores\Strategies;

use Scores\Enums\MathsScore;

class MathsSortingStrategy extends EnumSortingStrategy
{
    protected string $enumClass = MathsScore::class;
}

