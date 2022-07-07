﻿using System;

namespace password_manager_wf.Models
{
    public class Password
    {
        //modelo para realizar insert  o update
        public int id { get; set; }
        public int userId { get; set; }
        public string title { get; set; }
        public string websiteName { get; set; }
        public string usernameOrEmail { get; set; }
        public string passwordToSave { get; set; }
    }
}
