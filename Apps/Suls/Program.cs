using MySUS.MvcFramework;
using System.Threading.Tasks;

namespace Suls
{
    public class Program
    {
        public static async Task  Main(string[] args)
        {
            await Host.CreateHostAsync(new StartUp());
        }
    }
}
