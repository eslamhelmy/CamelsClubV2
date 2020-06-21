using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace CamelsClub.API.Helpers
{
    public class CultureHelper
    {
        public static int CurrentCulture
        {
            set
            {

                if (value == 1)
                {
                    CultureInfo culture = new CultureInfo("ar-EG");
                    Thread.CurrentThread.CurrentUICulture = culture;
                }
                else if (value == 2)
                {
                    CultureInfo culture = new CultureInfo("en");                  
                    Thread.CurrentThread.CurrentUICulture = culture;              
                }

                else
                {
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
                }
            

            }
        }
        
    }
}