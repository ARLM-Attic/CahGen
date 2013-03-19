using System;
using System.Collections.Generic;
using System.Linq;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace CahGen
{
    public class CardsData
    {
        public string FontName { get; set; }
        public List<Card> Cards { get; set; }
        public bool IncludeBackPages { get; set; }

        public void Render(string outputFileName)
        {
            var rng = new Random();
            var pagesOfAnswers = Cards.Where(c => c.Type == "White")
                .Select((x, i) => new {Card = x, Index = i})
                .GroupBy(x => x.Index/24)
                .Select(g => new PageOfCards(XColors.Black, XColors.White, g.Select(x => x.Card), FontName))
                .OrderBy(x=>rng.Next());

            var pagesOfQuestions = Cards.Where(x => x.Type == "Black")
                .Select((x, i) => new {Card = x, Index = i})
                .GroupBy(x => x.Index/24)
                .Select(g => new PageOfCards(XColors.White, XColors.Black, g.Select(x => x.Card), FontName))
                .OrderBy(x => rng.Next());
            var allPages = pagesOfAnswers.Concat(pagesOfQuestions);
            var document = new PdfDocument();
            foreach (PageOfCards page in allPages)
            {
                page.Render(document.AddPage());
            }
            document.Save(outputFileName);
        }
    }
}