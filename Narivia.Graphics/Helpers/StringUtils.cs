using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Narivia.Graphics.Helpers
{
    static class StringUtils
    {
        /// <summary>
        /// Wraps the text on multiple lines.
        /// </summary>
        /// <returns>The text.</returns>
        /// <param name="font">Font.</param>
        /// <param name="text">Text.</param>
        /// <param name="maxLineWidth">Maximum line width.</param>
        public static string WrapText(SpriteFont font, string text, float maxLineWidth)
        {
            if (font.MeasureString(text).X <= maxLineWidth)
            {
                return text;
            }

            string[] words = text.Split(' ');
            StringBuilder sb = new StringBuilder();
            float lineWidth = 0f;
            float spaceWidth = font.MeasureString(" ").X;

            foreach (string word in words)
            {
                Vector2 size = font.MeasureString(word);

                if (word.Contains("\r"))
                {
                    lineWidth = 0f;
                    sb.Append("\r \r");
                }

                if (lineWidth + size.X < maxLineWidth)
                {
                    sb.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }

                else
                {
                    if (size.X > maxLineWidth)
                    {
                        if (sb.ToString() == " ")
                        {
                            sb.Append(WrapText(font, word.Insert(word.Length / 2, " ") + " ", maxLineWidth));
                        }
                        else
                        {
                            sb.Append("\n" + WrapText(font, word.Insert(word.Length / 2, " ") + " ", maxLineWidth));
                        }
                    }
                    else
                    {
                        sb.Append("\n" + word + " ");
                        lineWidth = size.X + spaceWidth;
                    }
                }
            }

            return sb.ToString();
        }
    }
}
