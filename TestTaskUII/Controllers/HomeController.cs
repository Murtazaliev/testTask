using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using TestTaskDomain.Abstract;
using TestTaskDomain.Models;
using TestTaskUII.Commons;
using TestTaskUII.Models;

namespace TestTaskUII.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository _repository;
        private CommonHelper _helper;
        public HomeController(ILogger<HomeController> logger, IRepository repository)
        {
            _logger = logger;
            _repository = repository;
            _helper = CommonHelper.Instance();
        }
        public IActionResult Index()
        {            
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Create(int? id)
        {
            ViewData["ParentId"] = id;
            return PartialView();
        }

        [HttpPost]
        public async Task Create([Bind("Id,Name,CompanySubdivisionId")] CompanySubdivision treeNode)
        {
            if (!ModelState.IsValid)
                return;
            try
            {
                treeNode.CompanySubdivisionId = treeNode.CompanySubdivisionId != 0 ? treeNode.CompanySubdivisionId : null;
                await Task.Run(() => _repository.Add(treeNode));
                _repository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка", ex);
            }
        }
        public IActionResult Delete(int? id)
        {
            var model = _repository.CompanySubdivisions.FirstOrDefault(x => x.Id == id);
            return PartialView(model);
        }
        [HttpPost]
        public void Delete([Bind("Id,Name,CompanySubdivisionId")] CompanySubdivision treeNode)
        {
            IEnumerable<CompanySubdivision> subdivisions = _repository.CompanySubdivisions;
            
            var result = _helper.GetChildren(subdivisions.Where(x => x.Id == treeNode.Id)).ToList();
            _repository.RemoveRange(result);
            _repository.Save();            
        }
        public IActionResult Edit(int? id)
        {
            var deleted = _repository.CompanySubdivisions.FirstOrDefault(x => x.Id == id);
            List<CompanySubdivision> subdivisions = _repository.CompanySubdivisions.ToList();
            
            var ignoreList = _helper.GetChildren(subdivisions.Where(x => x.Id == deleted?.Id), "forupdate").ToList();


            foreach (var item in ignoreList)
            {
                subdivisions.Remove(item);
            }



            ViewData["AllowList"] = new SelectList(subdivisions, "Id", "Name", deleted?.CompanySubdivisionId);
            return PartialView(deleted);
        }
        [HttpPost]
        public void Edit([Bind("Id,Name,CompanySubdivisionId")] CompanySubdivision treeNode)
        {
            var changed = _repository.CompanySubdivisions.FirstOrDefault(x => x.Id == treeNode.Id);
            if (changed != null)
            {
                changed.Name = treeNode.Name;
                changed.CompanySubdivisionId = treeNode.CompanySubdivisionId;
                _repository.Update(changed);
                _repository.Save();
            }
        }
        public IActionResult GetTree(int? id)
        {
            
            IEnumerable<CompanySubdivision> subdivisions = _repository.CompanySubdivisions.OrderBy(x => x.Id);
            var dResult = _helper.GetChildren(subdivisions.Where(x => x.CompanySubdivisionId == null)).ToList();
            ViewBag.Structure = _helper.GetTreeJsonModel(id, dResult);
            return PartialView("Tree");
        }

        public FileResult GetXmlFile()
        {
            var result = _repository.CompanySubdivisions.OrderBy(x => x.Id).ToList();
            string xml = _helper.SerializeToXml(result); 
            byte[] fileBytes = global::System.Text.Encoding.UTF8.GetBytes(xml);
            string fileName = "myfile.xml";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        
        public IActionResult UploadXmlFile()
        {
            return PartialView("Upload");
        }
        [HttpPost]
        public async Task UploadXmlFile(IFormFile file)
        {
            try
            {
                var result = new StringBuilder();
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    while (reader.Peek() >= 0)
                        result.AppendLine(await reader.ReadLineAsync());
                }
                var dResult = _helper.DeserializeXml(result.ToString());
                if (dResult != null && dResult.Count > 0)
                {
                    var subdivisions = _repository.CompanySubdivisions;



                    List<CompanySubdivision> added = new List<CompanySubdivision>();
                    foreach (var item in dResult)
                    {
                        added.Add(new CompanySubdivision { Name = item.Name });                       
                    }
                    _repository.AddRange(added);
                    _repository.Save();

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка", ex);
            }
        }



    }
}