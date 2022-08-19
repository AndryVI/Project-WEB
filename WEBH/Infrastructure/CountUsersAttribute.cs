using Microsoft.AspNetCore.Mvc.Filters;
using System.IO;
using System;
using System.Collections.Generic;
using WebH.Models;
using WebH.Services;

namespace WebH.Infrastructure
{
    public class CountUsersAttribute : ActionFilterAttribute
    {
        private const string cookieKey = ".MyAppV.Session";
        private const string pathlogUniqueUserslist  = "App_Data/logUniqueUsersList.txt";
        private static List<CountUsersModel> listCountUsersModels;
        private WriterAndReadService wArSeirvice = new WriterAndReadService();

        public int CountUniqueUsers { get { return listCountUsersModels.Count; } }

        public CountUsersAttribute()
        {
            string[] lines =  File.ReadAllLines(pathlogUniqueUserslist);

            listCountUsersModels = new List<CountUsersModel>();

            foreach (string line in lines)
            {
                string[] items = line.Split(';');

                CountUsersModel countUsers = new CountUsersModel();
                countUsers.IdUser = Convert.ToInt32(items[0].Trim());
                countUsers.CookieId = items[1].Trim(); 
                countUsers.DateAndTime = Convert.ToDateTime(items[2].Trim());

                listCountUsersModels.Add(countUsers);
            }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            bool check = false;
            var cookies = context.HttpContext.Request.Cookies[cookieKey];
            var datetime = DateTime.Now;


            foreach (var item in listCountUsersModels)
                if (item.CookieId == cookies)
                    check = true;

            if (!check)
            {
                if (cookies is not null)
                {
                    wArSeirvice.FileWriter($"{CountUniqueUsers + 1}; {cookies} ; {datetime}", getfile: @"\logUniqueUsersList.txt", getdirectory: @"App_Data");
                    listCountUsersModels.Add(new CountUsersModel() { IdUser = CountUniqueUsers + 1, CookieId = cookies, DateAndTime = datetime });
                }
            }
        }

        //private void ReadLogUniqueUsers(){}

    }
}
