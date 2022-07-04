using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using TestTaskDomain.Abstract;
using TestTaskDomain.Models;
using TestTaskUII.Models;

namespace TestTaskUII.Commons
{
    public class CommonHelper
    {

        private CommonHelper()
        {
           
        }
        private static CommonHelper _instance = null;
        public static CommonHelper Instance()
        {
            if (_instance == null)
            {
                _instance = new CommonHelper();                
            }
            return _instance;
        }
        public IEnumerable<CompanySubdivision> GetChildren(IEnumerable<CompanySubdivision> nodes, string? type = "forall")
        {
            if (nodes?.Count() > 0)
            {
                int k = 1;
                foreach (CompanySubdivision node in nodes)
                {
                    node.Name = type == "forall" ? $"{k++}. {node.Name}" : node.Name;
                    yield return node;
                    foreach (var child in GetChildren(node.ChildSubdivisions))
                    {
                        yield return child;
                    }
                }
            }
        }
        public string GetTreeJsonModel(int? id, IEnumerable<CompanySubdivision> subdivisions)
        {
            
            IEnumerable<TreeModel> model = subdivisions.Select(s => new TreeModel
            {
                Href = s.Id.ToString(),
                Tags = new NodeTags { Id = s.Id.ToString(), ParentId = s.CompanySubdivisionId.ToString() },
                State = new State { Checked = id == s.Id, Selected = id == s.Id },
                Text = s.Name
            });
            
            Dictionary<string, TreeModel> dict = model.ToDictionary(loc => loc.Href);
            foreach (TreeModel struc in dict.Values)
            {

                if (struc.Tags.ParentId != struc.Href && !string.IsNullOrEmpty(struc.Tags.ParentId))
                {
                    TreeModel parent = dict[struc.Tags.ParentId];
                    parent.Nodes.Add(struc);
                }
            }
            var root = dict.Values.Where(x => string.IsNullOrEmpty(x.Tags.ParentId));

            string json = JsonSerialiser(root);
            return json.Replace("\"nodes\": []", "");
        }
        /// <summary>
        /// Сериализация модели в xml
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string JsonSerialiser<T>(T entity) 
        {
            try
            {
                string jsonIgnoreNullValues = JsonConvert.SerializeObject(entity, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
                return jsonIgnoreNullValues;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при сериализации в json", ex);
            }
        }
        /// <summary>
        /// Десериализация модели в xml
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<CompanySubdivision> DeserializeXml(string value)
        {
            if (value == null)
            {
                return null;
            }

            try
            {
                var serializer = new XmlSerializer(typeof(List<CompanySubdivision>));
                List<CompanySubdivision> result;

                using (TextReader reader = new StringReader(value))
                {
                    result = (List<CompanySubdivision>)serializer.Deserialize(reader);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при десериализации xml, проверте файл", ex);
            }
        }
        /// <summary>
        /// Сериализация модели в xml
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string SerializeToXml<T>(T value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            try
            {
                var xmlserializer = new XmlSerializer(typeof(T));
                var stringWriter = new StringWriter();
                using (var writer = XmlWriter.Create(stringWriter))
                {
                    xmlserializer.Serialize(writer, value);
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(stringWriter.ToString());
                    // var declaration = doc.CreateXmlDeclaration("1.0", "utf-8", null);
                    // doc.AppendChild(declaration);
                    return Beautify(doc);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при сериализации в xml", ex);
            }
        }
        public string Beautify(XmlDocument doc)
        {
            string xmlString = null;
            using (MemoryStream ms = new MemoryStream())
            {
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Encoding = new UTF8Encoding(false),
                    Indent = true,
                    IndentChars = "  ",
                    NewLineChars = "\r\n",
                    NewLineHandling = NewLineHandling.Replace
                };
                using (XmlWriter writer = XmlWriter.Create(ms, settings))
                {
                    doc.Save(writer);
                }
                xmlString = Encoding.UTF8.GetString(ms.ToArray());
            }
            return xmlString;
        }

        /// <summary>
        /// Метод для каскадного сбора вложенных отделов
        /// </summary>
        /// <param name="selectedNode"></param>
        /// <returns></returns>
        public IEnumerable<CompanySubdivision> CollectChildNodes(IEnumerable<CompanySubdivision> selectedNode)
        {
            if (selectedNode?.Count() > 0)
            {
                foreach (CompanySubdivision node in selectedNode)
                {
                    yield return node;

                    foreach (var child in CollectChildNodes(node.ChildSubdivisions))
                    {
                        yield return child;
                    }
                }
            }
        }
    }
}
