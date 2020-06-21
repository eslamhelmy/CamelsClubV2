using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Services.Helpers
{

    public class StringConvertor
    {
        public static int[] ConvertStringsToArrayOfInt(string text)
        {
            string[] parts = text.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
            int count = parts.Length;
            int[] arr = new int[count];
            for (int i = 0; i < count; i++)
            {
                arr[i] = int.Parse(parts[i]);
            }
            return arr;
        }
    }
}