using Assets_Inventory.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Assets_Inventory.Controllers
{
    public class HomeController : Controller
    {
        AssetInventoryContext db = new AssetInventoryContext();

        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<Asset> assets = db.Assets.Where(asset => asset.Active);
            ViewBag.Assets = assets;
            
            return View();
        }

        // Добавление
        [HttpGet]
        public ActionResult Create()
        {
            SelectList types = new SelectList(db.AssetTypes.Where(t => t.Active), "Id", "Name");
            ViewBag.Types = types;

            return PartialView("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Asset asset)
        {
            if (ModelState.IsValid)
            {
                db.Assets.Add(asset);

                ActionLog aLog = new ActionLog
                {
                    Action = "Create",
                    AssetId = asset.Id,
                    UserName = User.Identity.Name,
                    Notes = Request.Form["Notes"] //TODO: Проверить значение
                };

                db.ActionLogs.Add(aLog);
                db.SaveChanges();
            }
            ViewBag.Error = "Не удалось создать запись в базе данных!";
            return RedirectToAction("Index");
        }

        // Редактирование
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Asset asset = db.Assets.Find(id);            
            if (asset != null)
            {
                SelectList types = new SelectList(db.AssetTypes.Where(t => t.Active), "Id", "Name");
                ViewBag.Types = types;
                return PartialView("Edit", asset);
            }
            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Asset asset)
        {
            if (ModelState.IsValid)
            {
                db.Entry(asset).State = EntityState.Modified;
                db.SaveChanges();
            }
            ViewBag.Error = "Не удалось изменить запись в базе данных!";
            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}