﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.DAL.DTOs.NotificationDTOs
{
    public class NotificationAddDto
    {
        public int UserId { get; set; }
        public string NotificationMessage { get; set; }
        public DateTime DateSent { get; set; }
    }
}