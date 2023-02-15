namespace Unomart.Managers
{
    public class Session
    {
        public string UID { get; set; }
        public string SID { get; set; }
        public DateTime validity { get; set; }

        public Session(string UID)
        {
            this.UID = UID;
            SID = Guid.NewGuid().ToString();
            postpone();
        }

        public void postpone()
        {
            validity = DateTime.Now.AddMinutes(20.0d);
            //validity = validity.AddSeconds(5.0d);
        }
    }
    public static class SessionManager
    {
        static List<Session> active = new List<Session>();

        public static string Add(string UID)
        {
            Remove();

            Session s = new Session(UID);
            active.Add(s);
            return s.SID;
        }

        public static string validityCheck(string SID)
        {
            foreach (var s in active)
            {
                if (s.SID == SID)
                {
                    s.postpone();
                    return s.UID;
                }
            }
            return null;
        }

        public static bool isValid(string SID)
        {
            int i = 0;
            int d = -1;
            bool b = false;
            foreach (var s in active)
            {
                if (s.SID == SID)
                {
                    if (s.validity > DateTime.Now)
                    {
                        b = true;
                        s.postpone();
                    }
                    else
                    {
                        d = i;
                    }
                }
                i++;
            }
            if (d != -1)
                active.RemoveAt(d);
            return b;
        }

        private static void Remove()
        {
            foreach (var item in active)
            {
                if (item.validity < DateTime.Now)
                {
                    active.Remove(item);
                }
            }
        }
    }
}