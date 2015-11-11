using System.Collections.Generic;
using Newtonsoft.Json;
using Velov.Assets;
using Velov.DataModel;

namespace Velov.Common
{
    public static class DoWork
    {
        public static List<LyonStation> DeserializeStations()
        {
            string s = Helper.jsonEnDur();
            return JsonConvert.DeserializeObject<List<LyonStation>>(s);
        }       
    }
}
