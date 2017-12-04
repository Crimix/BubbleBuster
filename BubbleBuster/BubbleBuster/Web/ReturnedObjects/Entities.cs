﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster.Web.ReturnedObjects
{
    /// <summary>
    /// Class that can contain JSON information from Twitter. Deserilized using Newtonsoft.Json
    /// </summary>
    public class Entities
    {
        [JsonProperty("urls")]
        public List<Url> Urls { get; set; }
    }
}
