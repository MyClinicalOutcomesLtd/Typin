﻿namespace Typin.Tests.Commands.CustomTypes.NonInitializable
{
    using System;

    public struct NonInitializableStructType
    {
        public int Value { get; set; }
        public DayOfWeek Day { get; set; }
    }
}