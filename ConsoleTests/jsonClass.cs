using DAL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleTests
{
    [Serializable]
    public class Rootobject
    {
        public Allgroup[] allGroups { get; set; }
        public Allpriem[] allPriems { get; set; }
    }
    [Serializable]
    public class Allgroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    [Serializable]
    public class Allpriem
    {
        public string Name { get; set; }
        public int GroupId { get; set; }
        public int Id { get; set; }
    }

}
