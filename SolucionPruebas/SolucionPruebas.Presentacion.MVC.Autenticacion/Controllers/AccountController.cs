using System;
using System.Data;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Reflection;
using SolucionPruebas.Presentacion.MVC.Autenticacion.Models;

namespace SolucionPruebas.Presentacion.MVC.Autenticacion.Controllers
{

    [HandleError]
    public class AccountController : Controller
    {

        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }


        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }

        // **************************************
        // URL: /Account/LogOn
        // **************************************

        public ActionResult LogOn()
        {
            DataTable dtEmpresas = new DataTable();
            dtEmpresas.Columns.Add("nIdEmpresa", typeof(int));
            dtEmpresas.Columns.Add("sEmpresa", typeof(string));

            LogOnModel cLogOn = new LogOnModel();

            List<Empresa> slEmpresas = new List<Empresa>();

            Empresa cEmpresa = new Empresa();
            DataRow drEmpresa = dtEmpresas.NewRow();
            
            cEmpresa.sEmpresa = "NAMEX";
            cEmpresa.nIdEmpresa = 1;
            slEmpresas.Add(cEmpresa);

            drEmpresa["nIdEmpresa"] = 1;
            drEmpresa["sEmpresa"] = "NAMEX";
            dtEmpresas.Rows.Add(drEmpresa);

            cEmpresa = new Empresa();

            cEmpresa.sEmpresa = "PAX Facturación";
            cEmpresa.nIdEmpresa = 2;
            slEmpresas.Add(cEmpresa);

            drEmpresa = dtEmpresas.NewRow();
            drEmpresa["nIdEmpresa"] = 2;
            drEmpresa["sEmpresa"] = "PAX Facturación";
            dtEmpresas.Rows.Add(drEmpresa);

            //cLogOn.Empresas = slEmpresas.ToList();
            cLogOn.Empresas = fnObtenerEntidad<Empresa>(dtEmpresas);
            cLogOn.IdEmpresa = 0;

            return View(cLogOn);
            //return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ValidateUser(model.UserName, model.Password))
                {
                    FormsService.SignIn(model.UserName, model.RememberMe);
                    if (!String.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            List<Empresa> slEmpresas = new List<Empresa>();

            Empresa cEmpresa = new Empresa();
            cEmpresa.sEmpresa = "NAMEX";
            cEmpresa.nIdEmpresa = 1;
            slEmpresas.Add(cEmpresa);

            cEmpresa = new Empresa();
            cEmpresa.sEmpresa = "PAX Facturación";
            cEmpresa.nIdEmpresa = 2;
            slEmpresas.Add(cEmpresa);

            model.Empresas = slEmpresas.ToList();
            model.IdEmpresa = 0;

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // **************************************
        // URL: /Account/LogOff
        // **************************************

        public ActionResult LogOff()
        {
            FormsService.SignOut();

            return RedirectToAction("Index", "Home");
        }

        // **************************************
        // URL: /Account/Register
        // **************************************

        public ActionResult Register()
        {
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus = MembershipService.CreateUser(model.UserName, model.Password, model.Email);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsService.SignIn(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePassword
        // **************************************

        [Authorize]
        public ActionResult ChangePassword()
        {
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePasswordSuccess
        // **************************************

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        public ActionResult RecoverPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RecoverPassword(RecoverPasswordModel cRecoverPasswordModel, string returnUrl)
        {
            bool bResultado = false;
            if (ModelState.IsValid)
            {
                //// Attempt to register the user
                bResultado = MembershipService.fnRecuperarPasswordPorCorreo(cRecoverPasswordModel.Email);

                if (bResultado)
                {
                    return RedirectToAction("LogOn", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Intente má tarde");
                }
            }

            // If we got this far, something failed, redisplay form

            return View(cRecoverPasswordModel);
        }

        public IEnumerable<T> fnObtenerEntidad<T>(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }

            List<T> returnValue = new List<T>();
            List<string> typeProperties = new List<string>();

            T typeInstance = Activator.CreateInstance<T>();

            foreach (DataColumn column in dt.Columns)
            {
                var prop = typeInstance.GetType().GetProperty(column.ColumnName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                if (prop != null)
                {
                    typeProperties.Add(column.ColumnName);
                }
            }

            foreach (DataRow row in dt.Rows)
            {
                T entity = Activator.CreateInstance<T>();

                foreach (var propertyName in typeProperties)
                {

                    if (row[propertyName] != DBNull.Value)
                    {
                        string str = row[propertyName].GetType().FullName;

                        if (entity.GetType().GetProperty(propertyName).PropertyType == typeof(System.String))
                        {
                            object Val = row[propertyName].ToString();
                            entity.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).SetValue(entity, Val, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public, null, null, null);
                        }
                        else if (entity.GetType().GetProperty(propertyName).PropertyType == typeof(System.Guid))
                        {
                            object Val = Guid.Parse(row[propertyName].ToString());
                            entity.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).SetValue(entity, Val, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public, null, null, null);
                        }
                        else
                        {
                            entity.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).SetValue(entity, row[propertyName], BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public, null, null, null);
                        }
                    }
                    else
                    {
                        entity.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).SetValue(entity, null, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public, null, null, null);
                    }
                }
                returnValue.Add(entity);
            }
            return returnValue.AsEnumerable();
        }       
    }
}
