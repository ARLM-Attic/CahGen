using System;
using System.Collections.Generic;
using System.Linq;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace CahGen
{
    public class PageOfCards
    {
        private readonly XColor _foregroundColor;
        private readonly XColor _backgroundColor;
        private readonly IEnumerable<Card> _cards;

        private readonly XUnit CardSize = XUnit.FromInch(2.0);
        private readonly XUnit Padding = XUnit.FromInch(1.0/16.0);
        private readonly string _fontName;

        public PageOfCards(XColor foregroundColor, XColor backgroundColor, IEnumerable<Card> cards, string fontName)
        {
            _foregroundColor = foregroundColor;
            _backgroundColor = backgroundColor;
            _cards = cards;
            _fontName = fontName;
        }
        public void Render(PdfPage page)
        {
            PageHeight = page.Height;
            PageWidth = page.Width;
            using (var gfx = XGraphics.FromPdfPage(page))
            {
                DrawGrid(gfx);
                var positions = CalculateRects().Zip(_cards, (rect, card) => new { Rect = rect, Card = card });
                foreach (var position in positions)
                {
                    var card = position.Card;
                    var rect = position.Rect;
                    card.Render(rect, gfx, _fontName);

                }
            }
        }
        private IEnumerable<XRect> CalculateRects()
        {
            var lefts = Enumerable.Range(0, Columns).Select(i => XMargin + (CardSize * i) );
            var tops = Enumerable.Range(0, Rows).Select(i => YMargin + (CardSize * i) );
            return tops.SelectMany(y => lefts.Select(x => new XRect(x + Padding, y + Padding, CardSize - (Padding * 2), CardSize - (Padding * 2))));

        }

        private XUnit PageWidth { get; set; }
        private XUnit PageHeight { get; set; }
        private double YMargin { get { return ((PageHeight - (Rows * CardSize)) / 2.0); } }
        private double XMargin { get { return ((PageWidth - (Columns * CardSize)) / 2.0); } }
        private int Rows { get { return (int)Math.Floor(PageHeight / CardSize); } }
        private int Columns { get { return (int)Math.Floor(PageWidth / CardSize); } }

        private void DrawGrid(XGraphics gfx)
        {
            var foregroundPen = new XPen(_foregroundColor, 0.5);
            var backgroundPen = new XPen(_backgroundColor);

            gfx.DrawRectangle(backgroundPen, new XSolidBrush(_backgroundColor), 0, 0, PageWidth, PageHeight);

            for (int i = 0; i < (Columns + 2); i++)
            {
                var x = (XMargin + (i * CardSize));

                gfx.DrawLine(foregroundPen, x, 0, x, PageHeight);
            }
            for (int i = 0; i < (Rows + 2); i++)
            {
                var y = (YMargin + (i * CardSize));
                gfx.DrawLine(foregroundPen, 0, y, PageWidth, y);
            }
        }


    }
}