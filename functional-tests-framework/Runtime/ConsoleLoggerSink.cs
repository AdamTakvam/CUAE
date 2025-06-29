using System;
using System.Diagnostics;
using System.Runtime.InteropServices; // DllImport()

using Metreos.LoggingFramework;

namespace Metreos.Samoa.FunctionalTestRuntime
{
    public sealed class ConsoleLoggerSink : LoggerSinkBase
    {
        [DllImport("kernel32.dll")]
        public static extern bool SetConsoleTextAttribute(IntPtr hConsoleOutput,
            int wAttributes);
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetStdHandle(uint nStdHandle);

        public ConsoleLoggerSink() : base()
        {
            RefreshConfiguration();
        }

        private const string HighlightIndicator = "!";

        public override void LoggerWriteCallback(DateTime timeStamp, TraceLevel errorLevel, string message)
        {
            const uint STD_OUTPUT_HANDLE = 0xfffffff5;
            const string LOG_TIMESTAMP_FORMAT = "HH:mm:ss.fff";

            // Determine color of line based on trace level.
            int color = (int)Background.Black;	// Start with a black background.
            switch (errorLevel)
            {
                case TraceLevel.Error:
                    color |= (int)Foreground.BrightRed;
                    break;
                case TraceLevel.Warning:
                    color |= (int)Foreground.Yellow;
                    break;
                case TraceLevel.Verbose:
                    color |= (int)Foreground.Grey;
                    break;
                case TraceLevel.Info:
                default:
                    color |= (int)Foreground.White;
                    break;
            }

            IntPtr hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
            SetConsoleTextAttribute(hConsole, color);

            // If message contains the highlight indicator, highlight the
            // message by inverting the colors, e.g., red on black ->
            // black on red.
            if (message.IndexOf(HighlightIndicator) != -1)
            {
                // Swap nibbles, e.g., 0x0c -> 0xc0.
                SetConsoleTextAttribute(hConsole, color << 4 | color >> 4);
            }

            Console.WriteLine(timeStamp.ToString(LOG_TIMESTAMP_FORMAT) +
                " " + errorLevel.ToString()[0] + " " + message);

            // Reset color back to white on black
            SetConsoleTextAttribute(hConsole, (int)Foreground.White | (int)Background.Black);
        }

        public override void RefreshConfiguration()
        {
            this.SubscribeToLoggerEvents(TraceLevel.Verbose);
        }

        public override void Dispose() { }

        // Character attributes for Windows console screen buffers.
        private enum CharacterAttributes
        {
            FOREGROUND_BLUE = 0x0001,
            FOREGROUND_GREEN = 0x0002,
            FOREGROUND_RED = 0x0004,
            FOREGROUND_INTENSITY = 0x0008,
            BACKGROUND_BLUE = 0x0010,
            BACKGROUND_GREEN = 0x0020,
            BACKGROUND_RED = 0x0040,
            BACKGROUND_INTENSITY = 0x0080,
            COMMON_LVB_LEADING_BYTE = 0x0100,
            COMMON_LVB_TRAILING_BYTE = 0x0200,
            COMMON_LVB_GRID_HORIZONTAL = 0x0400,
            COMMON_LVB_GRID_LVERTICAL = 0x0800,
            COMMON_LVB_GRID_RVERTICAL = 0x1000,
            COMMON_LVB_REVERSE_VIDEO = 0x4000,
            COMMON_LVB_UNDERSCORE = 0x8000,
        }

        // Windows character attributes OR together for combined foreground colors.
        private enum Foreground
        {
            Black = 0,
            Blue = CharacterAttributes.FOREGROUND_BLUE,
            Green = CharacterAttributes.FOREGROUND_GREEN,
            Cyan = Blue | Green,
            Red = CharacterAttributes.FOREGROUND_RED,
            Purple = Blue | Red,
            Brown = Green | Red,
            White = Blue | Green | Red,
            Grey = CharacterAttributes.FOREGROUND_INTENSITY,
            BrightBlue = Blue | CharacterAttributes.FOREGROUND_INTENSITY,
            BrightGreen = Green | CharacterAttributes.FOREGROUND_INTENSITY,
            BrightCyan = Cyan | CharacterAttributes.FOREGROUND_INTENSITY,
            BrightRed = Red | CharacterAttributes.FOREGROUND_INTENSITY,
            Pink = Purple | CharacterAttributes.FOREGROUND_INTENSITY,
            Yellow = Brown | CharacterAttributes.FOREGROUND_INTENSITY,
            BrightWhite = White | CharacterAttributes.FOREGROUND_INTENSITY
        }

        // Windows character attributes OR together for combined background colors.
        private enum Background
        {
            Black = Foreground.Black << 4,
            Blue = Foreground.Blue << 4,
            Green = Foreground.Green << 4,
            Cyan = Foreground.Cyan << 4,
            Red = Foreground.Red << 4,
            Purple = Foreground.Purple << 4,
            Brown = Foreground.Brown << 4,
            White = Foreground.White << 4,
            Grey = Foreground.Grey << 4,
            BrightBlue = Foreground.BrightBlue << 4,
            BrightGreen = Foreground.BrightGreen << 4,
            BrightCyan = Foreground.BrightCyan << 4,
            BrightRed = Foreground.BrightRed << 4,
            Pink = Foreground.Pink << 4,
            Yellow = Foreground.Yellow << 4,
            BrightWhite = Foreground.BrightWhite << 4
        }
    }
}
