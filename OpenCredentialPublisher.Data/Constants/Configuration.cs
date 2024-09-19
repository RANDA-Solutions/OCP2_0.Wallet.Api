using System;

namespace OpenCredentialPublisher.Data.Constants
{
    public static class Configuration
    {
        public const string Key = "ApplicationSettings:";

        public struct ConsoleColors
        {
            public const ConsoleColor Name = ConsoleColor.White;
            public const ConsoleColor Milestone = ConsoleColor.Cyan;
            public const ConsoleColor Success = ConsoleColor.Green;
            public const ConsoleColor Error = ConsoleColor.Red;
            public const ConsoleColor Warning = ConsoleColor.Yellow;
            public const ConsoleColor SPROC = ConsoleColor.Magenta;
            public const ConsoleColor Default = ConsoleColor.Gray;
            public const ConsoleColor InProgress = ConsoleColor.Yellow;
            public const ConsoleColor Strong = ConsoleColor.White;
        }
    }
}