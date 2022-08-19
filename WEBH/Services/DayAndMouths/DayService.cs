using System;
using System.Collections.Generic;
using System.IO;
using WebH.Models;

namespace WebH.Services
{
    public class DayService
    {
        private readonly string pathday = "App_Data/day.txt";

        public List<Day> DayServiceReady()
        {
            string[] lines = File.ReadAllLines(pathday);

            List<Day> result = new List<Day>();
            foreach (string line in lines)
            {
                string[] items = line.Split(';');

                Day сategory = new Day();
                сategory.Id = Convert.ToInt32(items[0].Trim());
                сategory.Name = items[1].Trim();

                result.Add(сategory);
            }

            return result;
        }
    }
}
