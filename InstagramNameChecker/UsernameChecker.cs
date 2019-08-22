using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

namespace InstagramNameChecker
{
    class UsernameChecker
    {
        static void Main()
        {
            // usernames from file
            List<string> usernames = new List<string>();
            List<string> freeUsernames = new List<string>();
            // usernames paired with http responses indicating wheather or not the account exists
            Dictionary<string, HttpStatusCode> responses = new Dictionary<string, HttpStatusCode>();

            ReadUsernamesFromFile(usernames);

            Console.WriteLine("======================= STARTING ACCOUNT EXISTENCE CHECKS =======================");
            Thread.Sleep(1000);
            CheckNamesGetResponses(usernames, freeUsernames, responses);
            Console.WriteLine("======================= FINISHED ACCOUNT EXISTENCE CHECKS =======================");
            Thread.Sleep(1000);

            Console.WriteLine("=============================== PRINTING RESULTS ================================");
            Thread.Sleep(1000);
            PrintResult(responses);
            Console.WriteLine("======================= FINISHED PRINTING RESULTS ===============================");
            Thread.Sleep(2000);

            Console.WriteLine("======================= STARTING ACCOUNT CREATION ===============================");
            Thread.Sleep(1000);
            SeleniumHelper.CreateInstagramProfiles(freeUsernames);
            Console.WriteLine("======================= FINISHED ACCOUNT CREATION ===============================");
        }      

        private static void ReadUsernamesFromFile(List<string> usernamesList)
        {
            string line;
            try
            {
                using (StreamReader file = new StreamReader(Utils.FilePath))
                {
                    while ((line = file.ReadLine()) != null)
                    {
                        usernamesList.Add(line);
                    }
                }

                if (usernamesList.Count == 0)
                {
                    throw new Exception("Empty file?");
                }
            }
            catch (Exception)
            {
                Console.WriteLine($"ERROR: {Utils.FilePath} must be in the same directory as the executable!");
                Console.WriteLine("The file must not be empty!");
                Console.WriteLine("The usernames must be each on a new line!");
                Console.WriteLine("Aborting...");
                Thread.Sleep(6000);
                Environment.Exit(1);
            }
        }

        private static void PrintResult(Dictionary<string, HttpStatusCode> responses)
        {
            foreach (KeyValuePair<string, HttpStatusCode> resp in responses)
            {
                switch (resp.Value)
                {
                    case HttpStatusCode.OK:
                        //Console.WriteLine($"{resp.Key} - taken");
                        break;
                    case HttpStatusCode.NotFound:
                        Console.WriteLine($"Username {resp.Key} is free!");
                        break;
                    default:
                        Console.WriteLine($"Unexpected status code! Status code: {(int)resp.Value} - {resp.Value}");
                        break;
                }
            }
        }

        private static void CheckNamesGetResponses(List<string> usernames, List<string> freeUsernames, Dictionary<string, HttpStatusCode> responses)
        {
            HttpWebRequest webRequest = null;
            HttpWebResponse response = null;

            foreach (var username in usernames)
            {
                string url = $@"https://www.instagram.com/{username}/";
                try
                {
                    Console.WriteLine($"Checking {username}...");

                    webRequest = (HttpWebRequest)WebRequest.Create(url);
                    response = (HttpWebResponse)webRequest.GetResponse();

                    responses[username] = response.StatusCode;
                }
                catch (WebException we) //404
                {
                    response = we.Response as HttpWebResponse;

                    if (response == null)
                    {
                        Console.WriteLine("Response is null!");
                        throw;
                    }

                    responses[username] = response.StatusCode;
                    freeUsernames.Add(username);
                }
                finally
                {
                    response.Close();
                }
            }
        }
    }
}
