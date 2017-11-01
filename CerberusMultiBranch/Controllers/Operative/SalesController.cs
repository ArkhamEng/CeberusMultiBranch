﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CerberusMultiBranch.Models;
using CerberusMultiBranch.Models.Entities.Operative;
using Microsoft.AspNet.Identity;
using CerberusMultiBranch.Support;
using Microsoft.AspNet.Identity.Owin;
using CerberusMultiBranch.Models.ViewModels.Operative;
using CerberusMultiBranch.Models.Entities.Config;
using System.Net.Http;

namespace CerberusMultiBranch.Controllers.Operative
{
    [Authorize]
    public class SalesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Supervisor")]
        public ActionResult History()
        {
            //obtengo las sucursales configuradas para el empleado
            var branches = User.Identity.GetBranches();

            TransactionViewModel model = new TransactionViewModel();
            model.Branches = branches.ToSelectList();
            model.Sales = LookFor(null, DateTime.Today, null, null, null, null, null, null);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Supervisor")]
        public JsonResult Cancel(int transactionId, string comment)
        {
            try
            {
                //busco la venta a cancelar
                var sale = db.Sales.Include(s => s.TransactionDetails).
                    FirstOrDefault(s => s.TransactionId == transactionId);

                //regreso los productos al stock
                foreach (var detail in sale.TransactionDetails)
                {
                    var bp = db.BranchProducts.Find(sale.BranchId, detail.ProductId);
                    bp.LastStock = bp.Stock;
                    bp.Stock += detail.Quantity;

                    db.Entry(bp).State = EntityState.Modified;
                }

                //desactivo la venta y registo usuario, comentario y fecha de cancelación
                sale.LastStatus = sale.Status;
                sale.Status = TranStatus.Canceled;
                sale.UpdUser = User.Identity.Name;
                sale.UpdDate = DateTime.Now;
                sale.Comment = comment;

                db.Entry(sale).State = EntityState.Modified;

                db.SaveChanges();

                return Json(new { Result = "OK", Message = "Venta Cancelada" });
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "Supervisor")]
        public ActionResult Search(int? branchId, DateTime? beginDate, DateTime? endDate,
            string folio, string client, string user, TranStatus? status)
        {
            var model = LookFor(branchId, beginDate, endDate, folio, client, user, status, null);

            return PartialView("_SaleList", model);
        }


        [Authorize(Roles = "Supervisor")]
        public ActionResult Detail(int id)
        {
            var brancheIds = User.Identity.GetBranches().Select(b => b.BranchId);

            var sale = db.Sales.Include(s => s.TransactionDetails).Include(s => s.User).
                Include(s => s.TransactionDetails.Select(td => td.Product.Images)).
                FirstOrDefault(s => s.TransactionId == id && brancheIds.Contains(s.BranchId));

            if (sale == null)
                return RedirectToAction("History");

            return View(sale);
        }

        [Authorize(Roles = "Vendedor")]
        public ActionResult MyDetail(int id)
        {
            var brancheId = User.Identity.GetBranchId();

            var sale = db.Sales.Include(s => s.TransactionDetails).Include(s => s.User).
                Include(s => s.TransactionDetails.Select(td => td.Product.Images)).
                FirstOrDefault(s => s.TransactionId == id && s.BranchId == brancheId);

            if (sale == null)
                return RedirectToAction("MyHistory");

            return View(sale);
        }

        [Authorize(Roles = "Vendedor")]
        public ActionResult MySales()
        {
            var bId = User.Identity.GetBranchId();

            TransactionViewModel model = new TransactionViewModel();
            model.Sales = LookFor(bId, DateTime.Today, null, null, null, null, null, User.Identity.GetUserId());

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Vendedor")]
        public ActionResult SearchPerUser(DateTime? beginDate, DateTime? endDate, string folio, string client)
        {
            var bId = User.Identity.GetBranchId();
            var model = LookFor(bId, beginDate, endDate, folio, client, null, null, User.Identity.GetUserId());

            return PartialView("_MySaleList", model);
        }


        private List<Sale> LookFor(int? branchId, DateTime? beginDate, DateTime? endDate, string folio, string client,
            string user, TranStatus? status, string userId)
        {

            var brancheIds = User.Identity.GetBranches().Select(b => b.BranchId);

            var sales = (from p in db.Sales.Include(p => p.User).Include(p => p.TransactionDetails)
                         where
                            (branchId == null && brancheIds.Contains(p.BranchId) || p.BranchId == branchId)
                         && (beginDate == null || p.TransactionDate >= beginDate)
                         && (endDate == null || p.TransactionDate <= endDate)
                         && (folio == null || folio == string.Empty || p.Folio.Contains(folio))
                         && (client == null || client == string.Empty || p.Client.Name.Contains(client))
                         && (user == null || user == string.Empty || p.User.UserName.Contains(user))
                         && (userId == null || userId == string.Empty || p.UserId == userId)
                         && (status == null || p.Status == status)
                         select p).OrderByDescending(p => p.TransactionDate).ToList();

            return sales;
        }

        [HttpPost]
        public JsonResult CheckCart()
        {
            var userId = User.Identity.GetUserId();
            var branchId = User.Identity.GetBranchId();


            var sale = db.Sales.Include(s => s.TransactionDetails).
                FirstOrDefault(s => s.BranchId == branchId && s.UserId == userId && s.Status == TranStatus.InProcess);

            if (sale != null)
            {
                var list = sale.TransactionDetails;
                return Json(new { Result = "OK", Message = (list.Sum(td => td.Quantity)).ToString() });
            }

            else
                return Json(new { Result = "OK", Message = Cons.Zero.ToString() });
        }

        [HttpPost]
        [Authorize(Roles = "Vendedor")]
        public JsonResult SetClient(int clientId, int transactionId)
        {
            try
            {
                var sale = db.Sales.Find(transactionId);

                if (sale != null)
                    sale.ClientId = clientId;

                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error Al asignar el cliente", Data = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Vendedor")]
        public JsonResult SetNewPrice(int productId, int transactionId, double price)
        {
            try
            {
                var det = db.TransactionDetails.Include(td => td.Product).Include(td => td.Product.Images)
             .FirstOrDefault(td => td.TransactionId == transactionId && td.ProductId == productId);

                det.Price = price;
                det.Amount = det.Price * det.Quantity;

                db.Entry(det).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error al cambiar el costo!", Message = "Ocurrio un error al actualizar el precio detail " + ex.Message });
            }
        }

        [Authorize(Roles = "Vendedor")]
        public ActionResult ShopingCart()
        {
            SaleViewModel model;
            var branchId = User.Identity.GetBranchId();
            string userId = User.Identity.GetUserId();

            var sale = db.Sales.Include(s => s.TransactionDetails).Include(s => s.Client).
                Include(s => s.TransactionDetails.Select(td => td.Product)).
                Include(s => s.TransactionDetails.Select(td => td.Product.Images)).
                Include(s => s.TransactionDetails.Select(td => td.Product.BranchProducts)).

                FirstOrDefault(s => (userId == null || s.UserId == userId)
                               && (s.Status == TranStatus.InProcess)
                               && s.BranchId == branchId);

            if (sale != null)
            {
                foreach (var detail in sale.TransactionDetails)
                {
                    var brp = detail.Product.BranchProducts.FirstOrDefault(bp => bp.BranchId == branchId);
                    detail.Product.Quantity = brp != null ? brp.Stock : Cons.Zero;
                }
                model = new SaleViewModel(sale);
            }

            else
            {
                model          = new SaleViewModel();
                model.UpdDate  = DateTime.Now;
                model.UserId   = userId;
                model.BranchId = branchId;
                model.TransactionDetails = new List<TransactionDetail>();
            }

            model.Categories = db.Categories.ToSelectList();
            model.CarMakes   = db.CarMakes.ToSelectList();

            model.UpdUser = User.Identity.Name;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Vendedor")]
        public JsonResult RemoveFromCart(int transactionId, int productId)
        {
            var detail = db.TransactionDetails.Find(transactionId, productId);

            if (detail != null)
            {
                try
                {
                    db.TransactionDetails.Remove(detail);
                    db.SaveChanges();

                    return Json("OK");
                }
                catch (Exception ex)
                {
                    return Json("Ocurrio un error al eliminar el registro det:" + ex.Message);
                }
            }
            else
            {
                return Json("No se encontro el registro");
            }
        }


        [HttpPost]
        [Authorize(Roles = "Vendedor")]
        public JsonResult AddToCart(int productId, double quantity, double price)
        {
            var userId = User.Identity.GetUserId();
            var branchId = User.Identity.GetBranchId();


            var bp = db.BranchProducts.FirstOrDefault(brp => brp.BranchId == branchId && brp.ProductId == productId);
            var existance = bp != null ? bp.Stock : Cons.Zero;

            if (existance < quantity)
            {
                var j = new
                {
                    Result = "Sin producto en almacen",
                    Message = "Estas intentando vender mas productos de los disponibles, revisa existencia en sucursal"
                };
                return Json(j);
            }

            var sale = db.Sales.FirstOrDefault(s => s.UserId == userId && s.BranchId == branchId && s.Status == TranStatus.InProcess);

            var amount = (quantity * price);
            //if there is no transaction, create a new one
            if (sale == null)
            {
                sale = new Sale
                {
                    UserId = userId,
                    BranchId = branchId,
                    TransactionDate = DateTime.Now,
                    UpdDate = DateTime.Now,
                    UpdUser = User.Identity.Name,
                    Folio = Cons.CodeMask,
                    LastStatus = TranStatus.InProcess,
                    Status = TranStatus.InProcess
                };

                db.Sales.Add(sale);
                db.SaveChanges();
            }

            if (sale.TransactionId > Cons.Zero)
            {
                //check if the product is already added to the transaction
                var detail = db.TransactionDetails.
                    FirstOrDefault(td => td.ProductId == productId && td.TransactionId == sale.TransactionId);

                //if is sum the new quantity
                if (detail != null)
                {
                    detail.Amount += amount;
                    detail.Quantity += quantity;

                    if (existance < detail.Quantity)
                    {
                        var j = new
                        {
                            Result = "Cantidad insuficiente",
                            Message = "Estas intentando vender mas productos de los disponibles, revisa el carrito de venta"
                        };

                        return Json(j);
                    }

                    db.Entry(detail).State = EntityState.Modified;
                }
                else
                {
                    detail = new TransactionDetail
                    {
                        ProductId = productId,
                        TransactionId = sale.TransactionId,
                        Price = price,
                        Quantity = quantity,
                        Amount = amount
                    };

                    db.TransactionDetails.Add(detail);
                }
                sale.TotalAmount += amount;

                try
                {
                    db.Entry(sale).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return Json(new { Result = "Error", Message = ex.Message });
                }

            }

            var js = new { Result = "OK" };

            return Json(js);
        }

        [HttpPost]
        [Authorize(Roles = "Vendedor")]
        public ActionResult ShopingCart(Sale sale)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var detail in sale.TransactionDetails)
                    {
                        //busco stock en sucursal
                        var pb = db.BranchProducts.Find(sale.BranchId, detail.ProductId);

                        // si no hay producto suficiente la operación concluye
                        if (pb == null || pb.Stock < detail.Quantity)
                            throw new Exception("Un producto execede la cantidad en inventario");

                        //guardo dato del detalle
                        detail.Amount = detail.Price * detail.Quantity;
                        detail.Quantity = detail.Quantity;
                        db.Entry(detail).State = EntityState.Modified;

                        //actualizo stock de sucursal
                        pb.LastStock = pb.Stock;
                        pb.Stock -= detail.Quantity;

                        db.Entry(pb).State = EntityState.Modified;
                    }

                    //ajusto el monto total y agrego el folio
                    sale.TotalAmount = sale.TransactionDetails.Sum(td => td.Amount);
                    sale.Folio = sale.TransactionId.ToString(Cons.CodeMask);

                    //coloco el porcentaje de comision del empleado
                    sale.ComPer = User.Identity.GetSalePercentage();

                    //si tiene comision por venta, coloco la cantidad de esta
                    if (sale.ComPer > Cons.Zero)
                        sale.ComAmount = Math.Round(sale.TotalAmount * (sale.ComPer / Cons.OneHundred), Cons.Two);

                    //ajuto la hora y fecha de venta a la actual y concluyo
                    sale.TransactionDate = DateTime.Now;
                    sale.Status = TranStatus.Reserved;
                    sale.LastStatus = TranStatus.InProcess;
                    sale.UpdUser = User.Identity.Name;
                    sale.UpdDate = DateTime.Now;

                    db.Entry(sale).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("MySales");
                }
                catch (Exception ex)
                {
                    db.Dispose();
                    db = null;

                    var model = GetVM(sale.TransactionId);
                    ViewBag.Header = "Error al cerrar la venta!";
                    ViewBag.Message = ex.Message;
                    return View("ShopingCart", model);
                }
            }
            else
            {
                db.Dispose();
                db = null;

                var model = GetVM(sale.TransactionId);
                ViewBag.Header = "Datos inválidos!";
                ViewBag.Message = "No se pudo concretar venta debido un error en los datos de ingreso";
                return View("ShopingCart", model);
            }
        }



        private SaleViewModel GetVM(int id)
        {
            if (db == null)
                db = new ApplicationDbContext();


            var sale = db.Sales.Include(s => s.TransactionDetails).Include(s => s.Client).
                                     Include(s => s.TransactionDetails.Select(td => td.Product)).
                                     Include(s => s.TransactionDetails.Select(td => td.Product.Images)).
                                     Include(s => s.TransactionDetails.Select(td => td.Product.BranchProducts)).
                                     FirstOrDefault(s => s.TransactionId == id);

            SaleViewModel model = new SaleViewModel(sale);
            model.Categories = db.Categories.ToSelectList();
            model.CarMakes = db.CarMakes.ToSelectList();

            return model;
        }




        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}