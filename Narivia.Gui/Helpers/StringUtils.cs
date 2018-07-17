using System;

namespace Narivia.Gui.Helpers
{
    public static class StringUtils
    {
        public static string NumberToHumanReadable(int value)
        {
            string text = null;

            if (Math.Abs(value) < 10000) // 1K
            {
                text = value.ToString();
            }
            else if (Math.Abs(value) < 10000) // 10K
            {
                double div = value / 1000.0f;
                text = $"{div:0.00}K";
            }
            else if (Math.Abs(value) < 1000000) // 1M
            {
                double div = value / 1000.0f;
                text = $"{div:0}K";
            }
            else if (Math.Abs(value) < 10000000) // 10M
            {
                double div = value / 1000000.0f;
                text = $"{div:0.00}M";
            }
            else if (Math.Abs(value) < 100000000) // 100M
            {
                double div = value / 1000000.0f;
                text = $"{div:0.0}M";
            }
            else if (Math.Abs(value) < 1000000000) // 1B
            {
                double div = value / 1000000.0f;
                text = $"{div:0}M";
            }
            else
            {
                double div = value / 1000000000.0f;
                text = $"{div:0.0}B";
            }

            return text;
        }
    }
}
