﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CV_Console
{
    internal class Program
    {
        private const string DataUrl = @"https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_global.csv";

        private static async Task<Stream> GetDataStream()
        {
            var client = new HttpClient();
            var result = await client.GetAsync(DataUrl, HttpCompletionOption.ResponseHeadersRead);
            return await result.Content.ReadAsStreamAsync();
        }

        private static IEnumerable<string> GetDataLines()
        {
            using var data_stream = GetDataStream().Result;
            using var data_reader = new StreamReader(data_stream);

            while (!data_reader.EndOfStream)
            {
                var line = data_reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;
                yield return line;
            }
        }

        private static IEnumerable<DateTime> GetDates() => GetDataLines()
            .First()
            .Split(',')
            .Skip(4)
            .Select(s => DateTime.Parse(s, CultureInfo.InvariantCulture))
            .ToArray();

        private static IEnumerable<(string Country, string Province, int[] Counts)> GetData()
        {
            Regex regx = new (',' + "(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            var lines = GetDataLines()
                .Skip(1)
                .Select(line => regx.Split(line));

            foreach(var line in lines)
            {
                var province = line[0].Trim();
                var country = line[1].Trim(' ', '"');
                var counts = line.Skip(4).Select(int.Parse).ToArray();

                yield return (country, province, counts);
            }
        }

        private static void Main()
        {
            //foreach (var data_line in GetDataLines())
            //    Console.WriteLine(data_line);

            //Console.WriteLine(string.Join("\r\n", GetDates()));

            var russia_data = GetData()
                .First(v => v.Country.Equals("Russia", StringComparison.OrdinalIgnoreCase));
            Console.WriteLine(string.Join("\r\n", GetDates().Zip(russia_data.Counts, (date, count) => $"{date:dd:MM} - {count}")));

        }
    }
}