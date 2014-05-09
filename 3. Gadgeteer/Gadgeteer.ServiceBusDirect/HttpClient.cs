using System;
using Microsoft.SPOT;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;
using System.Security.Cryptography.X509Certificates;

namespace Gadgeteer.ServiceBusDirect
{
   static class HttpClient
    {
        static internal string ReadAndDeleteFromQueue(Uri Url, X509Certificate[] Certs)
        {
            string page = null;

            // Create an HTTP Web request.
            HttpWebRequest request =
                HttpWebRequest.Create(Url) as HttpWebRequest;

            // Assign the certificates. The value must not be null if the
            // connection is HTTPS.
            request.HttpsAuthentCerts = Certs;

            // Set request.KeepAlive to use a persistent connection. 
            request.KeepAlive = true;

            request.Method = "DELETE";
            var token = "[ACS TOKEN]";
            request.Headers.Add("Date", DateTime.UtcNow.ToString("R"));
            request.Headers.Add("Authorization", "WRAP access_token=\"" + token + "\"");

            // Get a response from the server.
            WebResponse resp = null;

            try
            {
                resp = request.GetResponse();
            }
            catch (Exception e)
            {
                Debug.Print("Exception in HttpWebRequest.GetResponse(): " +
                    e.ToString());
            }

            // Get the network response stream to read the page data.
            if (resp != null)
            {
                Stream respStream = resp.GetResponseStream();
                byte[] byteData = new byte[4096];
                char[] charData = new char[4096];
                int bytesRead = 0;
                Decoder UTF8decoder = System.Text.Encoding.UTF8.GetDecoder();
                int totalBytes = 0;

                // allow 5 seconds for reading the stream
                respStream.ReadTimeout = 5000;

                // If we know the content length, read exactly that amount of 
                // data; otherwise, read until there is nothing left to read.
                if (resp.ContentLength != -1)
                {
                    for (int dataRem = (int)resp.ContentLength; dataRem > 0; )
                    {
                        Thread.Sleep(500);
                        bytesRead =
                            respStream.Read(byteData, 0, byteData.Length);
                        if (bytesRead == 0)
                        {
                            Debug.Print("Error: Received " +
                                (resp.ContentLength - dataRem) + " Out of " +
                                resp.ContentLength);
                            break;
                        }
                        dataRem -= bytesRead;

                        // Convert from bytes to chars, and add to the page 
                        // string.
                        int byteUsed, charUsed;
                        bool completed = false;
                        totalBytes += bytesRead;
                        UTF8decoder.Convert(byteData, 0, bytesRead, charData, 0,
                            bytesRead, true, out byteUsed, out charUsed,
                            out completed);
                        page = page + new String(charData, 0, charUsed);

                        // Display the page download status.
                        Debug.Print("Bytes Read Now: " + bytesRead +
                            " Total: " + totalBytes);
                    }

                    page = new String(
                        System.Text.Encoding.UTF8.GetChars(byteData));
                }
                else
                {
                    // Read until the end of the data is reached.
                    while (true)
                    {
                        // If the Read method times out, it throws an exception, 
                        // which is expected for Keep-Alive streams because the 
                        // connection isn't terminated.
                        try
                        {
                            Thread.Sleep(500);
                            bytesRead =
                                respStream.Read(byteData, 0, byteData.Length);
                        }
                        catch (Exception)
                        {
                            bytesRead = 0;
                        }

                        // Zero bytes indicates the connection has been closed 
                        // by the server.
                        if (bytesRead == 0)
                        {
                            break;
                        }

                        int byteUsed, charUsed;
                        bool completed = false;
                        totalBytes += bytesRead;
                        UTF8decoder.Convert(byteData, 0, bytesRead, charData, 0,
                            bytesRead, true, out byteUsed, out charUsed,
                            out completed);
                        page = page + new String(charData, 0, charUsed);

                        // Display page download status.
                        Debug.Print("Bytes Read Now: " + bytesRead +
                            " Total: " + totalBytes);
                    }

                    Debug.Print("Total bytes downloaded in message body : "
                        + totalBytes);
                }


            }

            // Close the response stream.  For Keep-Alive streams, the 
            // stream will remain open and will be pushed into the unused 
            // stream list.
            resp.Close();

            return page;
        }
    }


}
