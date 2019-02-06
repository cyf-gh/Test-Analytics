using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Test_Analytics.Web {
    public static class Utils {
        public static T GetObjectFromJsonInRequest<T>( HttpRequest request ) {
            MemoryStream stream = new MemoryStream();
            request.Body.CopyTo( stream );
            stream.Position = 0;
            using( StreamReader reader = new StreamReader( stream ) ) {
                string requestBody = reader.ReadToEnd();
                if( requestBody.Length > 0 ) {
                    return JsonConvert.DeserializeObject<T>( requestBody );
                }
            }
            throw new InvalidDataException("In Utils.GetObjectFromJsonInRequest<>. Json Data Is invalid");
        }
    }
}
