using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;

namespace CahGen
{
    public class Card
    {

        public virtual double FontSize { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }

        public virtual void Render(XRect rect, XGraphics gfx, string fontName)
        {
            //gfx.DrawRectangle(XBrushes.Red, rect);
            var font = new XFont(fontName, FontSize, XFontStyle.Regular);
            var brush = new XSolidBrush(ForegroundColor);
            var textFormatter = new XTextFormatter(gfx);
            textFormatter.Alignment = XParagraphAlignment.Left;

            textFormatter.DrawString(Text, font, brush, rect);

        }
        public virtual XColor ForegroundColor { get { return Type == "White" ? XColors.Black : XColors.White; } }
        public virtual XColor BackgroundColor { get { return Type == "White" ? XColors.White : XColors.Black; } }


    }
}