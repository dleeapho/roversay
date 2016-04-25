﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Console.WriteLine();
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
        private RenderOptions _renderOptions { get; set; }

        public string SpeechContent { get; private set; }

        public IList<string> Builder { get; private set; }

        public SayRenderer(string speechContent, RenderOptions renderOptions)
        {
            _renderOptions = renderOptions;
            SpeechContent = speechContent;
            Builder = new List<string>();
        }

        public string Render()
        {
            DrawSpeechBubble();
            DrawRover();

            return null;
        }

        private void DrawSpeechBubble()
        {
            var lineLength = _renderOptions.SpeechBalloonWidth - 4;
            var output = SpeechContent.WrapText((int)lineLength).ToArray();
            var lines = output.Length;
            var wrapperLineLength = (lines == 1 ? output.First().Length : (int)_renderOptions.SpeechBalloonWidth - 4) + 2;

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

                    var neededPadding = (int)_renderOptions.SpeechBalloonWidth - 4 - output[i].Length;
                    Builder.Add($"{lineStartChar} {output[i]}{' '.NChar(neededPadding)} {lineEndChar}");
                }
            }
            Builder.Add($" {'-'.NChar(wrapperLineLength)}");
        }

         private void DrawRover()
        {
            var leftPad = Builder.First().Length / 4;

            Builder.Add($@"{' '.NChar(leftPad)}    \");
            Builder.Add($@"{' '.NChar(leftPad)}     \");
            Builder.Add($@"{' '.NChar(leftPad)}      \");

            DrawFromFile(Builder, "rover.txt");
        }
        private void DrawFromFile(IList<string> builder, string filePath)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        while (sr.Peek() >= 0)
                        {
                            builder.Add(sr.ReadLine());
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

            StringBuilder actualLine = new StringBuilder();
            int actualWidth = 0;

            foreach (var item in originalLines)
            {
                //Size szText = TextRenderer.MeasureText(item, font);

                actualWidth += item.Length + 1;

                if (actualWidth > maxWidthInChars)
                {
                    wrappedLines.Add(actualLine.ToString());
                    actualLine.Clear();
                    actualLine.Append(item + " ");
                    actualWidth = actualLine.Length;
                }
                else
                {
                    actualLine.Append(item + " ");
                }
            }

            if (actualLine.Length > 0)
                wrappedLines.Add(actualLine.ToString());

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