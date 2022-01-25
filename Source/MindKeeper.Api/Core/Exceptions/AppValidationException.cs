﻿using System;
using System.Collections.Generic;

namespace MindKeeper.Api.Core.Exceptions
{
    public class AppValidationException : Exception
    {
        public AppValidationException() : base("One or more validation failures have occurred.")
        {
            Errors = new List<string>();
        }
        public List<string> Errors { get; }
        public AppValidationException(params string[] failures)
            : this()
        {
            foreach (var failure in failures)
            {
                Errors.Add(failure);
            }
        }

    }
}
