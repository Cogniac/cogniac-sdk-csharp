/*
    Copyright 2019 Cogniac Corporation.

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/

using System;
using Cogniac;

namespace CogniacEdgeFlowCLI
{
    class Program
    {
        private static string _urlPrefix = "https://api.cogniac.io/1";
        private static string _subjectUid = "";
        
        private static Gateway _gw = null;

        static void Main(string[] args)
        {
            string fileName = "C:\\Path\\To\\My\\File.jpg";

            _urlPrefix = "http://edgeflow-hostname-or-ip:8000/1";
            _subjectUid = "MySubjectUid";

            Console.WriteLine($"Connecting to EdgeFlow...");
            Console.WriteLine($"Uploading Media: '{fileName}'...");
            _gw = new Gateway(_urlPrefix);
            MediaDetections pm = _gw.ProcessMedia(_subjectUid, fileName: fileName);
            if (pm != null)
            {
                Console.WriteLine($"Media '{fileName}' processed.");
                Console.WriteLine($"MediaDetections: '{pm.ToJson()}'");
            }
            return;
        }
    }
}
