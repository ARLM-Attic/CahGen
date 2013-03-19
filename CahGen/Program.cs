using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace CahGen
{
    class Program
    {
        static void Main(string[] args)
        {
            var cardsFileName = args.Length > 0? args[0] : "cards.json";
            var outputFileName = args.Length > 1 ? args[1] : string.Format("cah{0:hhmmss}.pdf",System.DateTime.Now);
            var data = JsonConvert.DeserializeObject<CardsData>(File.ReadAllText(cardsFileName));
            data.Render(outputFileName);
            System.Diagnostics.Process.Start(outputFileName);
        }
    }
}
