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
            //ViewBag.Error = "Не удалось создать запись в базе данных!";
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
        public ActionResult Edit(int id)
        {
            Asset asset = db.Assets.Find(id);
            if (ModelState.IsValid)
            {
                db.Entry(asset).State = EntityState.Modified;

                ActionLog aLog = new ActionLog
                {
                    Action = "Edit",
                    AssetId = asset.Id,
                    UserName = User.Identity.Name,
                    Notes = Request.Form["Notes"] //TODO: Проверить значение
                };

                db.ActionLogs.Add(aLog);
                db.SaveChanges();
            }
            
            //ViewBag.Error = "Не удалось изменить запись в базе данных!";
            return RedirectToAction("Index");
        }

        // Удаление. При вызове этого действия происходит изменение флага 'Active' в таблице 'Assets' на false 
        // и соответствующая запись в журнале 'ActionLog'
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Asset asset = db.Assets.Find(id);
            if (asset != null)
            {
                return PartialView("Delete", asset);
            }
            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Asset asset = db.Assets.Find(id);
            if (ModelState.IsValid)
            {
                asset.Active = false;
                asset.Location = null;
                asset.Connection = null;
                asset.Netw_name = null;

                db.Entry(asset).State = EntityState.Modified;

                ActionLog aLog = new ActionLog
                {
                    Action = "Delete",
                    AssetId = asset.Id,
                    UserName = User.Identity.Name,
                    Notes = Request.Form["Notes"] //TODO: Проверить значение
                };

                db.ActionLogs.Add(aLog);
                db.SaveChanges();
            }
            //ViewBag.Error = "Не удалось изменить запись " + asset.Id.ToString() + " в базе данных!";
            return RedirectToAction("Index");
        }

        // Расположение
        [HttpGet]
        public ActionResult Location(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Asset asset = db.Assets.Find(id);
            if (asset != null)
            {
                return PartialView("Location", asset);
            }

            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Location(int id)
        {
            Asset asset = db.Assets.Find(id);
            if (ModelState.IsValid)
            {
                db.Entry(asset).State = EntityState.Modified;
                asset.Location = Request.Form["Location"];

                LocationLog locLog = new LocationLog
                {
                    Location = asset.Location,
                    AssetId = asset.Id,
                    UserName = User.Identity.Name,
                    Notes = Request.Form["Notes"] //TODO: Проверить значение
                };

                db.LocationLogs.Add(locLog);
                db.SaveChanges();
            }            
            return RedirectToAction("Index");
        }

        // Подключения
        [HttpGet]
        public ActionResult Connect(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Asset asset = db.Assets.Find(id);
            if (asset != null && asset.AssetTypeId != 1) // AssetTypeId = 1 это компьютер. Компьютер нкчему не подключаем.
            {
                SelectList comps = new SelectList(db.Assets.Where(t => t.AssetTypeId == 1).Where(a => a.Active), "Inv_Id", "Inv_Id");
                ViewBag.Comps = comps;

                return PartialView("Connect", asset);
            }
            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Connect(int id)
        {
            Asset asset = db.Assets.Find(id);
            if (ModelState.IsValid)
            {
                asset.Connection = Request.Form["Connection"]; //TODO: Проверить значение
                db.Entry(asset).State = EntityState.Modified;

                ConnectionLog cLog = new ConnectionLog
                {
                    AssetId = asset.Id,
                    ConnectTo = asset.Connection,
                    UserName = User.Identity.Name,
                    Notes = Request.Form["Notes"] //TODO: Проверить значение
                };

                db.ConnectionLogs.Add(cLog);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Asset asset = db.Assets.Find(id);
            if (asset == null)
            {
                HttpNotFound();
            }

            string allConnections = "";

            //История перемещений
            IEnumerable<LocationLog> lLog = db.LocationLogs
                .Where(a => a.AssetId == asset.Id)
                .OrderByDescending(t => t.Timestamp);
            ViewBag.HystoryLocations = lLog;

            //История Подключений
            IEnumerable<ConnectionLog> cLog = db.ConnectionLogs
                .Where(a => a.AssetId == asset.Id)
                .OrderByDescending(t => t.Timestamp);
            ViewBag.HystoryConnections = cLog;

            //История операций
            IEnumerable<ActionLog> aLog = db.ActionLogs
                .Where(a => a.AssetId == asset.Id)
                .OrderByDescending(t => t.Timestamp);
            ViewBag.HystoryActions = aLog;

            //Подключения
            //Получаем коллекцию записей журнала подключений, где указано подключение к текущему объекту 
            IEnumerable<ConnectionLog> conections = db.ConnectionLogs.Where(a => a.ConnectTo == asset.Inv_id);
            ViewBag.ConnectionsTmp = conections;

            //Берем из коллекции инвентарные номера только тех объектов, которые подключены к текущему в данный момент
            foreach (ConnectionLog cl in conections)
            {
                if (cl.Asset.Connection == cl.ConnectTo)
                {
                    allConnections += cl.Asset.Inv_id + ", ";
                }
            }
            ViewBag.Connections = allConnections.TrimEnd(' ').TrimEnd(',');
            IEnumerable<ConnectionLog> conLog = asset.ConnectionLog;
            return View(asset);
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