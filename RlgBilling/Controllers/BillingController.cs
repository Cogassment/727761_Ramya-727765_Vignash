using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Data.SqlClient;
using RlgBilling.Models;
using System.Configuration;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;

namespace RlgBilling.Controllers
{
    public class BillingController : Controller
    {
        // GET: Billing
        string constr = ConfigurationManager.ConnectionStrings["Constring"].ConnectionString;
        public ActionResult Index()
        {
            return View(GetBillingDetails());
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(BillingModel billingModel)
        {
            if (ModelState.IsValid)
            {
                if (AssociateIDExists(billingModel.AssociateID) != 1)
                {
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        string query = "Insert into BillingTable(ProjectID,ProjectName,ManagerName,AssociateID,AssociateName,AllocationPercent,OnOff,RateCard,BillableDays,LeaveDays,Amount,Comments)VALUES(@ProjectID,@ProjectName,@ManagerName,@AssociateID,@AssociateName,@AllocationPercent,@OnOff,@RateCard,@BillableDays,@LeaveDays,@Amount,@Comments)";

                        using (SqlCommand cmd = new SqlCommand(query))
                        {
                            cmd.Connection = con;
                            con.Open();
                            cmd.Parameters.AddWithValue("@ProjectID", billingModel.ProjectID);
                            cmd.Parameters.AddWithValue("@ProjectName", billingModel.ProjectName);
                            cmd.Parameters.AddWithValue("@ManagerName", billingModel.ManagerName);
                            cmd.Parameters.AddWithValue("@AssociateID", billingModel.AssociateID);
                            cmd.Parameters.AddWithValue("@AssociateName", billingModel.AssociateName);
                            cmd.Parameters.AddWithValue("@AllocationPercent", billingModel.AllocationPercent);
                            cmd.Parameters.AddWithValue("@OnOff", billingModel.OnOff);
                            cmd.Parameters.AddWithValue("@RateCard", billingModel.RateCard);
                            cmd.Parameters.AddWithValue("@BillableDays", billingModel.BillableDays);
                            cmd.Parameters.AddWithValue("@LeaveDays", billingModel.LeaveDays);
                            cmd.Parameters.AddWithValue("@Amount", billingModel.Amount);
                            cmd.Parameters.AddWithValue("@Comments", billingModel.Comments);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    ViewBag.message = "User Saved successfully";
                    ModelState.Clear();
                }
                else
                {
                   ViewBag.message = "Id already exist";
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult Save(int BillableDays, int LeaveDays, int Amount, int AssociateID, string Comments)
        {
            string query = "update BillingTable set BillableDays = '" + BillableDays + "', LeaveDays = " + LeaveDays + " , Amount = '" + Amount + "',Comments = '" + Comments + "' where AssociateID =" + AssociateID;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Connection = con;

                    cmd.ExecuteNonQuery();

                }
                con.Close();
            }

            return Json("Details saved successfully.");
        }
        private IList<BillingModel> GetBillingDetails()
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("Select * from BillingTable", con);
                cmd.ExecuteNonQuery();
                List<BillingModel> list = new List<BillingModel>();
                BillingModel billingModel;
                int rate = 0;
                using (SqlDataReader read = cmd.ExecuteReader())
                {
                    while (read.Read())
                    {
                        billingModel = new BillingModel();
                        billingModel.ProjectID = int.Parse(read["ProjectID"].ToString());
                        billingModel.ProjectName = read["ProjectName"].ToString();
                        billingModel.ManagerName = read["ManagerName"].ToString();
                        billingModel.AssociateID = int.Parse(read["AssociateID"].ToString());
                        billingModel.AssociateName = read["AssociateName"].ToString();
                        billingModel.AllocationPercent = int.Parse(read["AllocationPercent"].ToString());
                        billingModel.OnOff = read["OnOff"].ToString();
                        billingModel.RateCard = int.Parse(read["RateCard"].ToString());
                        billingModel.BillableDays = int.Parse(read["BillableDays"].ToString());
                        rate = billingModel.RateCard * billingModel.BillableDays;
                        billingModel.LeaveDays = int.Parse(read["LeaveDays"].ToString());
                        billingModel.Amount = Convert.ToInt32(rate);
                        billingModel.Amount = rate;
                        billingModel.Comments = read["Comments"].ToString();
                        list.Add(billingModel);
                    }
                    con.Close();
                }

                return list;
            }
        }
        public ActionResult ExportToExcel()
        {
            var billingdata = GetBillingDetails();
            var gridview = new GridView();
            gridview.DataSource = this.GetBillingDetails();
            gridview.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=BillingDetails.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gridview.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return Content("Success");
        }
        private int AssociateIDExists(int associateId)
        {
            int isUserExists = 0;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string userExistQuery = "SELECT COUNT(*) FROM dbo.BillingTable WHERE AssociateID=" + associateId;
                using (SqlCommand cmd = new SqlCommand(userExistQuery))
                {
                    cmd.Connection = con;
                    con.Open();
                    isUserExists = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                }
            }
            return isUserExists;
        }

    }
}



