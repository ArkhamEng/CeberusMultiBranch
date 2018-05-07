using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Operative;
using CerberusMultiBranch.Models.ViewModels.Operative;
using CerberusMultiBranch.Support;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CerberusMultiBranch.Controllers.Operative
{
    public partial class SalesController
    {
        [Authorize(Roles = "Vendedor")]
        [HttpPost]
        public ActionResult OpenCart()
        {
            SaleViewModel model;
            var branchId = User.Identity.GetBranchId();
            string userId = User.Identity.GetUserId();

            //busco una venta de contrado en proceso
            var sale = db.Sales.Include(s => s.SaleDetails).Include(s => s.Client).
                Include(s => s.SaleDetails.Select(td => td.Product)).
                Include(s => s.SaleDetails.Select(td => td.Product.Images)).
                Include(s => s.SaleDetails.Select(td => td.Product.BranchProducts)).

                FirstOrDefault(s => (s.UserId == userId)
                               && (s.Status == TranStatus.InProcess && s.TransactionType == TransactionType.Contado)
                               && s.BranchId == branchId);

            if (sale != null)
            {
                foreach (var detail in sale.SaleDetails)
                {
                    var brp = detail.Product.BranchProducts.FirstOrDefault(bp => bp.BranchId == branchId);
                    detail.Product.Quantity = brp != null ? brp.Stock : Cons.Zero;
                    detail.Product.StorePrice = brp != null ? brp.StorePrice : Cons.Zero;
                    detail.Product.DealerPrice = brp != null ? brp.DealerPrice : Cons.Zero;
                    detail.Product.WholesalerPrice = brp != null ? brp.WholesalerPrice : Cons.Zero;
                }
                model = new SaleViewModel(sale);
            }

            else
            {
                model = new SaleViewModel();
                model.UpdDate = DateTime.Now.ToLocal();
                model.UserId = userId;
                model.BranchId = branchId;
                model.SaleDetails = new List<SaleDetail>();
            }

            model.UpdUser = User.Identity.Name;
            return PartialView("_Cart", model);
        }

        [HttpPost]
        [Authorize(Roles = "Vendedor")]
        public ActionResult RemoveFromCart(int transactionId, int productId)
        {
            var sale = db.Sales.Include(s => s.SaleDetails.Select(d=> d.Product.Images)).FirstOrDefault(s=> s.SaleId == transactionId);

            var detail = sale.SaleDetails.FirstOrDefault(d => d.ProductId == productId);

            if (detail != null)
            {
                try
                {
                    sale.SaleDetails.Remove(detail);

                    if (sale != null)
                    {
                        sale.TotalAmount        = sale.SaleDetails.Sum(d => d.Amount).RoundMoney();
                        sale.TotalTaxedAmount   = sale.SaleDetails.Sum(d => d.TaxedAmount).RoundMoney();
                        sale.TotalTaxAmount     = (sale.TotalTaxedAmount - sale.TotalAmount).RoundMoney();
                        sale.FinalAmount        = sale.TotalTaxedAmount;

                        sale.UpdDate = DateTime.Now.ToLocal();
                        sale.UpdUser = User.Identity.GetUserName();

                        db.Entry(sale).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    var model = sale.SaleDetails;
                    return PartialView("_CartDetails", model);
                }
                catch (Exception ex)
                {
                    return Json(new { Result = "Ocurrio un error al eliminar el registro", Message = ex.Message });
                }
            }
            else
            {
                return Json(new { Result = "Dato no encontrado", Message = "No se encontro el registo a eliminar, intenta recargar la pagina usando F5" });
            }
        }


        [HttpPost]
        [Authorize(Roles = "Vendedor")]
        public JsonResult AddToCart(int productId, double quantity)
        {
            var userId = User.Identity.GetUserId();
            var branchId = User.Identity.GetBranchId();

            //obtengo el IVA configurado en base de datos
            var iva = Convert.ToDouble(db.Variables.FirstOrDefault(v => v.Name == Cons.VariableIVA).Value);


            //Obtengo el inventario en la sucursal
            var bp = db.BranchProducts.Include(brp => brp.Product).
                        FirstOrDefault(brp => brp.BranchId == branchId && brp.ProductId == productId);

            //consulto si el usuario tiene una venta de contado activa
            var sale = db.Sales.Include(s => s.Client).Include(s=> s.SaleDetails).FirstOrDefault(s => s.UserId == userId &&
                         s.BranchId == branchId && s.Status == TranStatus.InProcess && s.TransactionType == TransactionType.Contado);

            var existance = bp != null ? bp.Stock : Cons.Zero;

            //valido la configuración de precios
            if (bp.StorePrice <= Cons.Zero)
            {
                var j = new
                {
                    Result = "Error en precio",
                    Message = "El precio de mostrador debe ser mayor a $0, revisa la configuración"
                };
                return Json(j);
            }
            else if (bp.DealerPrice <= Cons.Zero)
            {
                var j = new
                {
                    Result = "Error en precio",
                    Message = "El precio de distribuidor debe ser mayor a $0, revisa la configuración"
                };
                return Json(j);
            }
            else if (bp.WholesalerPrice <= Cons.Zero)
            {
                var j = new
                {
                    Result = "Error en precio",
                    Message = "El precio de mayorista debe ser mayor a $0, revisa la configuración"
                };
                return Json(j);
            }

            //Si no hay un registro de venta activo, creo uno nuevo con cliente por defecto
            if (sale == null)
            {
                sale = new Sale
                {
                    UserId          = userId,
                    BranchId        = branchId,
                    TransactionDate = DateTime.Now.ToLocal(),
                    UpdDate         = DateTime.Now.ToLocal(),
                    UpdUser         = User.Identity.Name,
                    Folio           = User.Identity.GetFolio(Cons.Zero),
                    LastStatus      = TranStatus.InProcess,
                    Status          = TranStatus.InProcess,
                    TransactionType = TransactionType.Contado
                };

                db.Sales.Add(sale);
                db.SaveChanges();
            }


            var amount = 0.0;
            var price = 0.0;


            //se asigna el precio en base a la configuración del cliente
            //si no hay un cliente asignado se asigna precio mostrador
            if (sale.Client == null || sale.Client.Type == ClientType.Store)
            {
                amount = (quantity * bp.StorePrice);
                price = bp.StorePrice;
            }
            else
            {
                switch (sale.Client.Type)
                {
                    case ClientType.Dealer:
                        amount = (quantity * bp.DealerPrice);
                        price = bp.DealerPrice;
                        break;
                    case ClientType.Wholesaler:
                        amount = (quantity * bp.WholesalerPrice);
                        price = bp.WholesalerPrice;
                        break;
                }
            }


            SaleDetail detail = null;
            //checo si el producto ya esta en la venta
            if(sale.SaleDetails != null || sale.SaleDetails.Count > Cons.Zero)
                detail = sale.SaleDetails.FirstOrDefault(td => td.ProductId == productId);

            if (detail != null)
            {
                detail.Quantity     += quantity;
                detail.TaxedAmount  = (detail.TaxedPrice * detail.Quantity);
                detail.Price        = (detail.TaxedPrice / (Cons.One + (iva / Cons.OneHundred))).RoundMoney();
                detail.Amount       = (detail.Price * detail.Quantity).RoundMoney();
                detail.TaxAmount    = ((detail.TaxedPrice - detail.Price) * detail.Quantity).RoundMoney();
            }
            else
            {
                detail = new SaleDetail
                {
                    ProductId = productId,
                    SaleId = sale.SaleId
                };

                detail.TaxPercentage    = iva;
                detail.Quantity         = quantity;
                detail.TaxedAmount      = amount.RoundMoney();
                detail.TaxedPrice       = price.RoundMoney();
                detail.Price            = (detail.TaxedPrice / (Cons.One + (iva / Cons.OneHundred))).RoundMoney();
                detail.Amount           = (detail.Price * detail.Quantity).RoundMoney();
                detail.TaxAmount        = ((detail.TaxedPrice - detail.Price) * detail.Quantity).RoundMoney();

                sale.SaleDetails.Add(detail);
            }
            sale.TotalAmount        = sale.SaleDetails.Sum(d => d.Amount).RoundMoney();
            sale.TotalTaxedAmount   = sale.SaleDetails.Sum(d => d.TaxedAmount).RoundMoney();
            sale.TotalTaxAmount     = (sale.TotalTaxedAmount - sale.TotalAmount).RoundMoney();
            sale.FinalAmount        = sale.TotalTaxedAmount;


            //verifico el stock y valido si es posible agregar mas producto a la venta
            if (existance < detail.Quantity)
            {
                var j = new
                {
                    Result = "Cantidad insuficiente",
                    Message = "Estas intentando vender mas productos de los disponibles, revisa el carrito de venta, solo puedes vender productos sin existencia "+
                    "Iniciando una preventa desde el modulo de ventas"
                };

                return Json(j);
            }

            try
            {
                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }

            var js = new { Result = "OK" };
            return Json(js);
        }


        public ActionResult DailyBill()
        {
            DailyBillViewModel model = new DailyBillViewModel();
            model.Branches = User.Identity.GetBranches().ToSelectList();
           
            return View(model);
        }

        [HttpPost]
        public ActionResult SearchSoldItems(DailyBillViewModel model)
        {
            
            var SoldItems = (from sd in db.SaleDetails.Include(sd => sd.Product.Images).Include(sd=> sd.Product.Category)
                         where (sd.Sale.TransactionDate >= model.Date) &&
                               (sd.Sale.TransactionDate < model.EndDate) &&
                               (sd.Sale.TransactionType == model.TransType) &&
                               (sd.Sale.Status == TranStatus.Compleated) && //solo las ventas pagadas en su totalidad
                               (sd.Sale.BranchId == model.BranchId) &&
                               (model.Client == null || sd.Sale.Client.Name == model.Client) &&
                               (model.Folio == null || sd.Sale.Folio == model.Folio) 
                         select sd).ToList();

            
            return PartialView("_SoldItemsDetails", SoldItems);
        }

    }
}