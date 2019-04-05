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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cogniac;

namespace CogniacUploadTool
{
    class Program
    {
        private static Connection _con = null;
        private static Auth _auth = null;
        private static bool _recursive = false;
        private static bool _isRecursiveError = false;
        private static string _forceSet = "";
        private static string _license = "";
        private static string _authorProfileUrl = "";
        private static string _domainUnit = "";
        private static string _urlPrefix = "https://api.cogniac.io/1";
        private static string _localGatewayUrl = "";
        private static bool _forceOverwrite = true;
        private static bool _forceFeedback = false;
        private static bool _isPublic = false;
        private static List<string> _metaTags = new List<string>();
        private static string _subjectUid = "";
        private static string _token = "";

        static void Main(string[] args)
        {
            string fileName = "";
            string dirName = "";
            string userName = "";
            string password = "";
            string tenantId = "";
            long mediaTimestamp = 0;
            string externalMediaId = "";
            string originalUrl = "";
            string originalLandingUrl = "";
            string title = "";
            string sourceUrl = "";
            string previewUrl = "";
            for (int index = 0; index < args.Length; index++)
            {
                switch (args[index].ToLower())
                {
                    case "-fs":
                    case "-forceset":
                        _forceSet = args.ElementAtOrDefault(++index);
                        if (!_forceSet.Equals("training") && !_forceSet.Equals("validation"))
                        {
                            _forceSet = null;
                        }
                        CheckParam(userName, nameof(userName));
                        break;

                    case "-f":
                    case "-filename":
                        fileName = args.ElementAtOrDefault(++index).Replace('+', ' ');
                        if (!File.Exists(fileName))
                        {
                            Kill($"FileName: '{fileName}' does not exist.", -1);
                        }
                        break;

                    case "-d":
                    case "-dirname":
                        dirName = args.ElementAtOrDefault(++index).Replace('+', ' ');
                        if (!Directory.Exists(dirName))
                        {
                            Kill($"DirName: '{dirName}' does not exist.", -1);
                        }
                        break;

                    case "-u":
                    case "-username":
                        userName = args.ElementAtOrDefault(++index);
                        CheckParam(userName, nameof(userName));
                        break;

                    case "-p":
                    case "-password":
                        password = args.ElementAtOrDefault(++index);
                        CheckParam(password, nameof(password));
                        break;

                    case "-tid":
                    case "-tenantid":
                        tenantId = args.ElementAtOrDefault(++index);
                        CheckParam(tenantId, nameof(tenantId));
                        break;

                    case "-tk":
                    case "-token":
                        _token = args.ElementAtOrDefault(++index);
                        CheckParam(_token, nameof(_token));
                        break;

                    case "-up":
                    case "-urlprefix":
                        _urlPrefix = args.ElementAtOrDefault(++index);
                        CheckParam(_urlPrefix, nameof(_urlPrefix));
                        break;

                    case "-lgu":
                    case "-localgatewayurl":
                        _localGatewayUrl = args.ElementAtOrDefault(++index);
                        CheckParam(_localGatewayUrl, nameof(_localGatewayUrl));
                        break;

                    case "-mt":
                    case "-mediatimestamp":
                        try
                        {
                            mediaTimestamp = Convert.ToInt64(args.ElementAtOrDefault(++index));
                        }
                        catch (Exception ex)
                        {
                            Kill($"'MediaTimestamp' is invalid: {ex.Message}", -1);
                        }
                        break;

                    case "-fow":
                    case "-forceoverwrite":
                        try
                        {
                            _forceOverwrite = Convert.ToBoolean(args.ElementAtOrDefault(++index));
                        }
                        catch (Exception ex)
                        {
                            Kill($"'ForceOverwrite' is invalid: {ex.Message}", -1);
                        }
                        break;

                    case "-ff":
                    case "-forcefeedback":
                        try
                        {
                            _forceOverwrite = Convert.ToBoolean(args.ElementAtOrDefault(++index));
                        }
                        catch (Exception ex)
                        {
                            Kill($"'ForceFeedback' is invalid: {ex.Message}", -1);
                        }
                        break;

                    case "-mtg":
                    case "-metatags":
                        try
                        {
                            index++;
                            while (index < args.Length - 1)
                            {
                                if (args[index].StartsWith("-"))
                                {
                                    index--;
                                    break;
                                }
                                else
                                {
                                    _metaTags.Add(args[index].Replace('+', ' '));
                                }
                                index++;
                            }
                            if (_metaTags.Count == 0)
                            {
                                Kill($"'MetaTags' is invalid: {args[index]}", -1);
                            }
                        }
                        catch (Exception ex)
                        {
                            Kill($"'MetaTags' is invalid: {ex.Message}", -1);
                        }
                        break;

                    case "-isp":
                    case "-ispublic":
                        try
                        {
                            _isPublic = Convert.ToBoolean(args.ElementAtOrDefault(++index));
                        }
                        catch (Exception ex)
                        {
                            Kill($"'IsPublic' is invalid: {ex.Message}", -1);
                        }
                        break;

                    case "-emid":
                    case "-externalmediaid":
                        externalMediaId = args.ElementAtOrDefault(++index);
                        CheckParam(externalMediaId, nameof(externalMediaId));
                        break;

                    case "-ou":
                    case "-originalurl":
                        originalUrl = args.ElementAtOrDefault(++index);
                        CheckParam(originalUrl, nameof(originalUrl));
                        break;

                    case "-olu":
                    case "-originallandingurl":
                        originalLandingUrl = args.ElementAtOrDefault(++index);
                        CheckParam(originalLandingUrl, nameof(originalLandingUrl));
                        break;

                    case "-l":
                    case "-license":
                        _license = args.ElementAtOrDefault(++index).Replace('+', ' ');
                        CheckParam(_license, nameof(_license));
                        break;

                    case "-apu":
                    case "-authorprofileurl":
                        _authorProfileUrl = args.ElementAtOrDefault(++index);
                        CheckParam(_authorProfileUrl, nameof(_authorProfileUrl));
                        break;

                    case "-t":
                    case "-title":
                        title = args.ElementAtOrDefault(++index).Replace('+', ' ');
                        CheckParam(title, nameof(title));
                        break;

                    case "-su":
                    case "-sourceurl":
                        sourceUrl = args.ElementAtOrDefault(++index);
                        CheckParam(sourceUrl, nameof(sourceUrl));
                        break;

                    case "-pu":
                    case "-previewurl":
                        previewUrl = args.ElementAtOrDefault(++index);
                        CheckParam(previewUrl, nameof(previewUrl));
                        break;

                    case "-suid":
                    case "-subjectuid":
                        _subjectUid = args.ElementAtOrDefault(++index);
                        CheckParam(_subjectUid, nameof(_subjectUid));
                        break;

                    case "-du":
                    case "-domainunit":
                        _domainUnit = args.ElementAtOrDefault(++index);
                        CheckParam(_domainUnit, nameof(_domainUnit));
                        break;

                    case "-r":
                    case "-recursive":
                        try
                        {
                            _recursive = Convert.ToBoolean(args.ElementAtOrDefault(++index));
                        }
                        catch (Exception ex)
                        {
                            Kill($"'Recursive' is invalid: {ex.Message}", -1);
                        }
                        break;

                    case "-h":
                    case "-help":
                        DisplayUsage();
                        Kill(null, 0);
                        break;

                    default:
                        Kill($"Error parsing argument: '{args[index]}'. Terminating...\r\nUse option '-h' or '-help' for the correct syntax.", -1);
                        break;
                }
            }
            // Input parsed, do the uploading.
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                // Some validation
                if (string.IsNullOrEmpty(fileName) && string.IsNullOrEmpty(dirName))
                {
                    Kill($"'FileName' or 'DirName' not provided. Terminating...", -1);
                }
                if (string.IsNullOrEmpty(_subjectUid))
                {
                    Kill($"'SubjectUid' is not provided. Terminating...", -1);
                }
                if (!string.IsNullOrEmpty(_token))
                {
                    Console.WriteLine($"Connecting to the Cogniac public API with provided token...");
                    _con = new Connection(token: _token, urlPrefix: _urlPrefix);
                }
                else
                {
                    Console.WriteLine($"Connecting to the Cogniac public API as: '{userName}'...");
                    _con = new Connection(userName, password, tenantId, urlPrefix: _urlPrefix);
                }
                if (_con != null)
                {
                    Console.WriteLine("Connected to Cogniac.");
                    Console.WriteLine($"Current tenant ID: '{tenantId}'");
                    _auth = _con.GetAuthObject();
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        Media m = _con.UploadMedia(_forceSet, fileName, mediaTimestamp,
                        _forceOverwrite, _metaTags.ToArray(), _isPublic, externalMediaId,
                        originalUrl, originalLandingUrl, _license, _authorProfileUrl,
                        title, sourceUrl, previewUrl, _domainUnit, _localGatewayUrl);
                        if (m != null)
                        {
                            Console.WriteLine($"'{fileName}' uploaded.");
                            Console.WriteLine($"MediaId: '{m.MediaId}'");
                            Console.WriteLine($"Associating media to subject: '{_subjectUid}'");
                            CaptureId ci = _con.AssociateMediaToSubject(m.MediaId, _subjectUid, _forceFeedback);
                            if (ci != null)
                            {
                                Console.WriteLine($"Association successful. CaptureId: '{ci.Id}'");
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(dirName))
                    {
                        DirectoryInfo di = new DirectoryInfo(dirName);
                        WalkDirectoryTree(di);
                        if (_isRecursiveError)
                        {
                            Console.WriteLine($"Processed: '{dirName}' but with errors, please see above log.");
                        }
                        else
                        {
                            Console.WriteLine($"Processed: '{dirName}' fully.");
                        }
                        if (_recursive)
                        {
                            Console.WriteLine("Processing was done recursively to all sub-directories.");
                        }
                    }
                }
                sw.Stop();
                Console.WriteLine($"Execution time: '{sw.Elapsed}'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void WalkDirectoryTree(System.IO.DirectoryInfo root)
        {
            FileInfo[] files = null;
            DirectoryInfo[] subDirs = null;

            // First, process all the files directly under this folder
            try
            {
                files = root.GetFiles("*.*");
            }
            // This is thrown if even one of the files requires permissions greater than the application provides
            catch (UnauthorizedAccessException e)
            {
                // This code just writes out the message and continues to recurse
                Console.WriteLine($"Error: '{e.Message}'");
                _isRecursiveError = true;
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine($"Error: '{e.Message}'");
                _isRecursiveError = true;
            }

            if (files != null)
            {
                // Perform upload asyncronously
                var tasks = files.Select(fi => Task<bool>.Factory.StartNew(() => ProcessFile(fi.FullName))).ToArray();
                Task.WaitAll(tasks);
                if (tasks.Select(task => task.Result).ToList().Contains(false))
                {
                    _isRecursiveError = true;
                }

                // Now find all the subdirectories under this directory
                if (_recursive)
                {
                    subDirs = root.GetDirectories();
                    foreach (DirectoryInfo dirInfo in subDirs)
                    {
                        // Resursive call for each subdirectory
                        WalkDirectoryTree(dirInfo);
                    }
                }
            }
        }

        private static bool ProcessFile(string fileName)
        {
            Connection c = null;
            if (_auth != null)
            {
                c = new Connection(token: _auth.AccessToken);
            }
            else
            {
                c = new Connection(token: _token);
            }
            if (c != null)
            {
                Media m = c.UploadMedia(forceSet: _forceSet, fileName: fileName,
                    forceOverwrite: _forceOverwrite, metaTags: _metaTags.ToArray(),
                    isPublic: _isPublic, license: _license, authorProfileUrl: _authorProfileUrl,
                    domainUnit: _domainUnit, localGatewayUrl: _localGatewayUrl);
                if (m != null)
                {
                    Console.WriteLine($"'{fileName}' uploaded.");
                    Console.WriteLine($"MediaId: '{m.MediaId}'");
                    Console.WriteLine($"Associating media to subject: '{_subjectUid}'");
                    CaptureId ci = _con.AssociateMediaToSubject(m.MediaId, _subjectUid, _forceFeedback);
                    if (ci != null)
                    {
                        Console.WriteLine($"Association successful. CaptureId: '{ci.Id}'");
                    }
                    return true;
                }
            }
            return false;
        }

        private static void DisplayUsage()
        {
            string text =
                "\r\n" +
                "Usage: CogUpload [-OPTION1 [ARG1]] [-OPTION2 [ARG1] [ARG2] [ARG3] ...] [-OPTION3 [ARG1]] ... \r\n" +
                "\r\n" +
                "   * All options starts with '-' followed by a space after the option. \r\n" +
                "   * Arguments to each option follow the option directly. \r\n" +
                "   * If an option takes an array, the members are provided as space separated arguments. \r\n" +
                "   * All options take a single string argument unless specified. \r\n" +
                "\r\n" +
                "List of available options: \r\n" +
                "\r\n" +
                "-fs   | -ForceSet           Either 'training' or 'validation', don't provide it otherwise. \r\n" +
                "-f    | -FileName           Full path and file name of meida. \r\n" +
                "-d    | -Dirname            Directory of media files to process. \r\n" +
                "-u    | -Username           Cogniac issued username. \r\n" +
                "-p    | -Password           Cogniac issued password. \r\n" +
                "-tid  | -TenantId           Valid Cogniac tenant ID. \r\n" +
                "-tk   | -Token              Valid Cogniac access token. \r\n" +
                "-up   | -UrlPrefix          URL prefix of the Cogniac API. \r\n" +
                "-lgu  | -LocalGatewayUrl    Local gateway URL. \r\n" +
                "-mt   | -MediaTimestamp     Time stamp of the media. \r\n" +
                "-ff   | -ForceFeedback      ['True' or 'False' (default)] Force feedback after upload. \r\n" +
                "-fow  | -ForceOverwrite     ['True' (default) or 'False'] Force overwrite of media. \r\n" +
                "-mtg  | -MetaTags           [Array] List of meta tags of the media. \r\n" +
                "-isp  | -IsPublic           ['True' or 'False' (default)] Set media to public. \r\n" +
                "-emid | -ExternalMediaId    External media ID. \r\n" +
                "-ou   | -OriginalUrl        Original media URL. \r\n" +
                "-olu  | -OriginalLandingUrl Original landing URL. \r\n" +
                "-l    | -License            License link or text of the media. \r\n" +
                "-apu  | -AuthorProfileUrl   Author profile URL. \r\n" +
                "-t    | -Title              Title of the media. \r\n" +
                "-su   | -SourceUrl          Source URL of the media. \r\n" +
                "-pu   | -PreviewUrl         Preview URL of the media. \r\n" +
                "-suid | -SubjectUid         The Cogniac subject to associate the media with. \r\n" +
                "-du   | -DomainUnit         (E.G. serial number) for set assignment grouping. \r\n" +
                "-r    | -Recursive          ['True' or 'False' (default)] Recursively upload files in 'DirName'. \r\n" +
                "-h    | -Help               Displays this help message. \r\n" +
                "\r\n" +
                "   Note 1: 'TenantId' must always be provided unless 'Token' is used. \r\n" +
                "   Note 2: Either use 'Token' or 'Username' and 'Password' but not both. If both are provided 'Token' \r\n" +
                "           will be used and 'Username' and 'Password' will be ignored. \r\n" +
                "   Note 3: 'SubjectUid' must always be provided. It applies to all 'FileName' and/or 'DirName' uploads.\r\n" +
                "   Note 4: 'if 'DirName' is provided, all media within the directory will be uploaded recursively. \r\n" +
                "   Note 5: 'MediaTimestamp', 'ExternalMediaId', 'SourceUrl', 'OriginalUrl', 'OriginalLandingUrl', \r\n" +
                "           'Title', 'SourceUrl' and 'PreviewUrl' are single media file options only and cannot be \r\n" +
                "           applied to an entire directory. The rest of the options apply to every media file within \r\n" +
                "           the provided directory to process. \r\n" +
                "   Note 6: If both 'FileName and 'DirName' are provided, the single file will process first \r\n" +
                "           then the entire directory will process second. \r\n" +
                "   Note 7: Spaces in any string feild are NOT permitted, please use '+' instead. \r\n" +
                "           For example: '-f my image.png' is invalid, use '-f my+image.png' instead. \r\n" +
                "   Note 8: Options are NOT case-sensitive. \r\n" +
                "\r\n" +
                "   Example 1:  CogUpload -f C:\\Path\\To\\Image.png -u MYUSER -p MYPASSWORD -tid ABC123 -suid DEF456 \r\n" +
                "   Example 2:  CogUpload -dirName C:\\Path\\To\\Images -u MYUSER -p MYPASSWORD -tid ABC123 -suid DEF456 \r\n" +
                "   Example 3:  CogUpload -fileName C:\\Path\\To\\Image.png -token ABCDEF123456 \r\n" +
                "               -mtg John+Doe BlackBerry Android+7.1.1 -IsPublic True -ff True -suid DEF456";
            Console.WriteLine(text);
        }
        
        private static void Kill(string message = "", int errorCode = 0)
        {
            if (!string.IsNullOrEmpty(message))
            {
                Console.WriteLine(message);
            }
            Environment.ExitCode = errorCode;
            Environment.Exit(errorCode);
        }

        private static void CheckParam(object paramValue, string paramName)
        {
            try
            {
                string paramValueString = "";
                Type t = paramValue.GetType();
                if (t.Equals(typeof(string)))
                {
                    paramValueString = (string)paramValue;
                    if (string.IsNullOrEmpty(paramValueString))
                    {
                        Kill($"{FirstCharToUpper(paramName)}: {paramValueString} is null or empty.", -1);
                    }
                    if (paramValueString.StartsWith("-"))
                    {
                        Kill($"{FirstCharToUpper(paramName)}: {paramValueString} is invalid, it cannot start with '-'", -1);
                    }
                }
            }
            catch (Exception ex)
            {
                Kill($"Error: {ex.Message}", -1);
            }
        }

        private static string FirstCharToUpper(string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }
    }
}
