﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EXE201.DAL.DTOs
{
    public class ResponeModel
    {
        public string Status { get; set; }
        public string Message { get; set; } = string.Empty;
        //public string Data { get; set; } = string.Empty;

        [IgnoreDataMember]
        [JsonIgnore]
        public Task<string>? ConfirmEmailToken { get; set; }
        public object DataObject { get; set; } = null;

        public object AdditionalData { get; set; } // Add this property

    }
}
