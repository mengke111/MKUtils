using System.Collections.Generic;

namespace MK
{

        public class DataClass
        {
            public class ParaPack
            {
                public List<string> slist { get;  set; }
                public string DownLoadPath { get;  set; }
            }

            public class data1Info
            {
                public string name { get;  set; }
                public string value { get;  set; }
            }
            public class User
            {
                public string name { get;  set; }
                public string passwd { get;  set; }
                public string email { get;  set; }
                public string salt { get;  set; }
            }

            public class LoginInfo
            {
                public User mUser { get;  set; }
                public string bu { get;  set; }
                public string ProjectText { get;  set; }
            }

        public class DataItem
        {
            public string ItemName { get; set; }
            public string MD5 { get; set; }

        }

        public class MailBody
        {
            public string ProjectName { get; set; }
            public string UserName { get; set; }
            public string Status { get; set; }
            public string Reason { get; set; }
        }

    }
    }