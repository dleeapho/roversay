using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace roversay
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var strContent = "";
            SayMode sayMode = SayMode.Regular;
            foreach (var arg in args)
            {
                switch (arg)
                {
                      default:
                        strContent += $"{arg} ";
                        break;
                }
            }
            var sayRenderer = new SayRenderer(strContent.Trim(),
                    new RenderOptions {
                        SayMode = sayMode,
                        FileToDraw = "rover.txt"
                    }
                );

            sayRenderer.Render();

            foreach (var line in sayRenderer.Builder)
            {
                Console.WriteLine(line);
            }
            Console.WriteLine(" ");
        }
    }
    public enum SayMode
    {
        Regular
    }

    public class RenderOptions
    {

        public uint SpeechBalloonWidth { get; set; } = 40;

        public string FileToDraw { get; set; } = String.Empty;

        public SayMode SayMode { get; set; } = SayMode.Regular;
    }

    public class SayRenderer
    {
        private RenderOptions RenderOptions { get; set; }

        public string SpeechContent { get; private set; }

        public IList<string> Builder { get; private set; }

        public SayRenderer(string speechContent, RenderOptions renderOptions)
        {
            this.RenderOptions = renderOptions;
            SpeechContent = speechContent;
            Builder = new List<string>();
        }

        public string Render()
        {
            DrawSpeechBubble();
            DrawSayer(this.RenderOptions.FileToDraw);

            return null;
        }

        private void DrawSpeechBubble()
        {
            var lineLength = this.RenderOptions.SpeechBalloonWidth - 4;
            var output = SpeechContent.WrapText((int)lineLength).ToArray();
            var lines = output.Length;
            var wrapperLineLength = (lines == 1 ? output.First().Length : (int)this.RenderOptions.SpeechBalloonWidth - 4) + 2;

            Builder.Add($" {'_'.NChar(wrapperLineLength)}");
            if (lines == 1)
            {
                Builder.Add($"< {output.First()} >");
            }
            else
            {
                for (var i = 0; i < lines; i++)
                {
                    char lineStartChar = '|';
                    char lineEndChar = '|';

                    if (i == 0)
                    {
                        lineStartChar = '/';
                        lineEndChar = '\\';
                    }
                    else if (i == lines - 1)
                    {
                        lineStartChar = '\\';
                        lineEndChar = '/';
                    }

                    var neededPadding = (int)this.RenderOptions.SpeechBalloonWidth - 4 - output[i].Length;
                    Builder.Add($"{lineStartChar} {output[i]}{' '.NChar(neededPadding)} {lineEndChar}");
                }
            }
            Builder.Add($" {'-'.NChar(wrapperLineLength)}");
            
            var leftPad = Builder.First().Length / 4;

            Builder.Add($@"{' '.NChar(leftPad)}    \");
            Builder.Add($@"{' '.NChar(leftPad)}     \");
            Builder.Add($@"{' '.NChar(leftPad)}      \");
        }

        private void DrawSayer(string filePath)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        while (sr.Peek() >= 0)
                        {
                            Builder.Add(sr.ReadLine());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file read failed:");
                Console.WriteLine(e.Message);
            }
        }
    }

    public static class Extensions
    {
        public static List<string> WrapText(this String s, int maxWidthInChars)
        {
            string[] originalLines = s.Split(new string[] { " " }, StringSplitOptions.None);

            List<string> wrappedLines = new List<string>();

            StringBuilder currentLine = new StringBuilder();

            foreach (var word in originalLines)
            {

                if ((currentLine.Length + word.Length) > maxWidthInChars)
                {
                    wrappedLines.Add(currentLine.ToString().Trim());
                    currentLine.Clear();
                }
                currentLine.Append(word + " ");
            }

            if (currentLine.Length > 0)
                wrappedLines.Add(currentLine.ToString());

            return wrappedLines;
        }

        public static string NChar(this char @char, int count)
        {
            var charArray = new char[count];
            for (var i = 0; i < count; i++)
                charArray[i] = @char;

            return new string(charArray);
        }
    }
}
