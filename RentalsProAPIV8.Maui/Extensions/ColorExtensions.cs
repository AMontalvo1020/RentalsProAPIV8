using Microsoft.Maui.Graphics;

namespace RentalsProAPIV8.Maui.Extensions
{
    public static class ColorExtensions
    {
        public static Color GetContrastingTextColor(this Color color)
        {
            // Return black or white text color based on luminance
            return (0.299 * color.Red + 0.587 * color.Green + 0.114 * color.Blue) > 0.5 ? Colors.Black : Colors.White;
        }
    }
}
