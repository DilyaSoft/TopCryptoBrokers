using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace TopCrypto.DataLayer.Services.Helpers
{
    public class JsonHelper
    {
        public bool IsValidJson(string strInput, out JToken token)
        {
            token = TryGetJToken(strInput);
            return token != null;
        }

        public JToken TryGetJToken(string strInput)
        {
            strInput = strInput.Trim();
            if (!((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]")))) //For array
            { return null; }

            try
            {
                return JToken.Parse(strInput);
            }
            catch (JsonReaderException)
            {
                //Exception in parsing json
                return null;
            }
            catch (Exception) //some other exception
            {
                return null;
            }
        }
    }
}
