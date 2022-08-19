using System;
using System.Collections.Generic;
using System.IO;
using WebH.Models;

namespace WebH.Services
{
    public class MonthsService
    {
        private readonly string pathmonths = "App_Data/months.txt";

        public List<Months> MonthsServiceRead()
        {
            string[] lines = File.ReadAllLines(pathmonths);

            List<Months> result = new List<Months>();
            foreach (string line in lines)
            {
                string[] items = line.Split(';');

                Months product = new Months();
                product.Id = Convert.ToInt32(items[0].Trim());
                product.Name = items[1].Trim();

                result.Add(product);
            }

            return result;
        }

    }
}
