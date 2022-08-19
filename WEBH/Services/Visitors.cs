using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WebH.Services
{
    public class Visitors
    {
        static Dictionary<string, bool> list = new Dictionary<string, bool>();

        public int Count { get { return list.Count; } }

        public void Check(string SessionId)
        {
            bool check = false;
            foreach (var item in list)
                if (SessionId == item.Key)
                    check = true;

            if (!check)
                AddSession(SessionId);
            else
            {
                if (list[SessionId] == false)
                    list[SessionId] = true;
            }
        }


        public void AddSession(string SessionId)
        {
            list.Add(SessionId, true);
            Task.Factory.StartNew(LifeSession, (SessionId));

        }

        void LifeSession(object SessionId)
        {
            //установка жизни сессии
            Thread.Sleep(120000);//600000

            list[(string)SessionId] = false;

            //проверка активна ли сессия
            Thread.Sleep(10000);//600000

            if (list[(string)SessionId] == false)
                Deleter((string)SessionId);
            else
                LifeSession(SessionId);
        }

        void Deleter(string SessionId)
        {
            list.Remove(SessionId);
        }
    }
}
