using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CamprayPortal.Web.Models.Topics;

namespace CamprayPortal.Web.Models.Solutions
{
    public class BenchmarkViewModel
    {
        public TopicModel Overview { get; set; }
        public TopicModel Mobile { get; set; }
        public TopicModel SpeedTime { get; set; }  
    } 
}