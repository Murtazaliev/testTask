using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace TestTaskUII.Models
{
    public class TreeModel 
    {
        public TreeModel()
        {
            Nodes = new List<TreeModel>();
        }
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }
        [JsonProperty("tags")]
        public NodeTags Tags { get; set; }
        [JsonProperty("state")]
        public State State { get; set; }

        [JsonProperty("nodes")]
        public List<TreeModel> Nodes { get; set; }


    }
    public class NodeTags
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("parentId")]
        public string ParentId { get; set; }
    }
    public class State
    {
        [JsonProperty("Checked")]
        public bool Checked { get; set; }
        [JsonProperty("selected")]
        public bool Selected { get; set; }
    }
}

