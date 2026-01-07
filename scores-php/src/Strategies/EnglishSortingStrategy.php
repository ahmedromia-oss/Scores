<?php

namespace Scores\Strategies;

use Scores\Enums\EnglishScore;

class EnglishSortingStrategy extends EnumSortingStrategy
{
    protected string $enumClass = EnglishScore::class;
}

