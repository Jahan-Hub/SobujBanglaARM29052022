﻿using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SobujBanglaARM
{
    public partial class Home : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand Cmd;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //lblWelcome.Text = "Welcome: Shamsul Alam";// +Session["UserName"].ToString();
                AppEnv.Current.p_UserName = "";//Session["UserName"].ToString();
                DateTime date = DateTime.Today;
                DateTime vdate = Convert.ToDateTime("2025-01-01 17:16:00");
                if (date > vdate)
                {
                    Response.Redirect("~/Validation.aspx");
                }
                if (Session["comId"].ToString() != "")
                {
                    con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
                    con.Open();
                    Cmd = new SqlCommand("select * from tblCompany where ComId=" + Session["comId"], con);
                    Cmd.CommandType = CommandType.Text;
                    SqlDataReader Dr;
                    Dr = Cmd.ExecuteReader();
                    while (Dr.Read())
                    {
                        lblCompanyName.Text = Dr["Name"].ToString();
                        Session["Name"] = Dr["Name"].ToString();
                        Session["Address"] = Dr["Address"].ToString();
                        Session["Contact1"] = Dr["Contact1"].ToString();
                        Session["Contact2"] = Dr["Contact2"].ToString();
                        Session["Fax"] = Dr["Fax"].ToString();
                        Session["CompanyLogo"] = Dr["CompanyLogo"].ToString();
                        Session["CompanyMoto"] = Dr["CompanyMoto"].ToString();
                        Session["Email"] = Dr["Email"].ToString();
                        Session["Website"] = Dr["Website"].ToString();
                        Page.Title = Dr["Name"].ToString();
                    }
                    con.Close();
                }
            }
            DateTime dt = DateTime.Now;
            string Day = dt.Day.ToString();
            string Month = dt.ToString("MMMM");
            string Year = dt.ToString("yyyy");

            string Hour = dt.Hour.ToString();
            string Minute = dt.ToString("mm");
            string Seconds = dt.ToString("ss");
            lblDateTime.Text = Day + " " + Month + "   " + Year + ",   " + Hour + ":" + Minute + ":" + Seconds;
            lblDayName.Text = System.DateTime.Now.DayOfWeek.ToString();
            LoadIFrame("Forms/Dashboard.aspx");
        }

        private void LoadIFrame(string src)
        {
            if (RadPane2.FindControl("frame") != null)
            {
                ((HtmlGenericControl)RadPane2.FindControl("frame")).Attributes.Remove("src");
                ((HtmlGenericControl)RadPane2.FindControl("frame")).Attributes.Add("src", src);
            }
            else
            {
                HtmlGenericControl iFrame = new HtmlGenericControl("iframe");
                iFrame.ID = "frame";
                iFrame.Attributes.Add("frameborder", "0");
                iFrame.Attributes.Add("class", "module");
                iFrame.Attributes.Add("style", "width:100%;height:100%;padding:0;");
                iFrame.Attributes.Add("src", src);

                RadPane2.Controls.Add(iFrame);
            }
        }

        protected void RadButton1_Click(object sender, EventArgs e)
        {
            // Label1.Text = Session["admn"].ToString();
        }
        protected void RadButton2_Click(object sender, EventArgs e)
        {
            LoadIFrame("HR/element/newemp.aspx");
            //Response.Redirect("HR/element/newemp.aspx");
        }
        private void LoadUserControl(Telerik.Web.UI.RadPanelItem item)
        {
            Label userControl1 = new Label();
            userControl1.ID = "userControl1";
            userControl1.Text = DateTime.Now.ToString();
            item.Items[1].Controls.Add(userControl1);
        }
        protected void MainMenu_ItemClick1(object sender, Telerik.Web.UI.RadPanelBarEventArgs e)
        {
            //this.LoadUserControl(e.Item);
            RadPanelItem ItemClicked = e.Item; //Response.Write("Server event raised -- you clicked: " + ItemClicked.Text);
            string s = ItemClicked.Text;

            if (s == "Employee Information")
            {
                LoadIFrame("Forms/EmployeeInfo.aspx");
            }

            if (s == "Company Info")
            {
                LoadIFrame("Forms/CompanyInfo.aspx");
            }
            if (s == "Master Data")
            {
                LoadIFrame("Forms/MasterData.aspx");
            }
            if (s == "DB Backup")
            {
                LoadIFrame("Forms/DBBackup.aspx");
            }
            if (s == "Dashboard")
            {
                LoadIFrame("Forms/Dashboard.aspx");
            }
            
            if (s == "Items")
            {
                LoadIFrame("Forms/ItemInfo.aspx");
            }
            if (s == "Customers")
            {
                LoadIFrame("Forms/CustomerInfo.aspx");
            }
            if (s == "Suppliers")
            {
                LoadIFrame("Forms/SupplierInfo.aspx");
            }
            if (s == "Staffs")
            {
                LoadIFrame("Forms/Staff.aspx");
            }
            if (s == "Purchases")
            {
                LoadIFrame("Forms/trnItemPurchase.aspx");
            }
            if (s == "Sales")
            {
                LoadIFrame("Forms/trnItemSales.aspx");
            }
            if (s == "Payment")
            {
                LoadIFrame("Forms/trnPayment.aspx");
            }
            if (s == "Money Receive")
            {
                LoadIFrame("Forms/trnMoneyReceive.aspx");
            }
            if (s == "Expenses Entry")
            {
                LoadIFrame("Forms/Expense.aspx");
            }
            if (s == "Income Entry")
            {
                LoadIFrame("Forms/Income.aspx");
            }
            if (s == "Bank Transactions")
            {
                LoadIFrame("Forms/BankTransaction.aspx");
            }
            if (s == "Stock Entry")
            {
                LoadIFrame("Forms/StockEntry.aspx");
            }
            if (s == "Emailing")
            {
                LoadIFrame("Forms/Mailing.aspx");
            }
            if (s == "Opening Balance(Cust)")
            {
                LoadIFrame("Forms/OpeningBalanceCust.aspx");
            }
            if (s == "Opening Balance(Sup)")
            {
                LoadIFrame("Forms/OpeningBalanceSup.aspx");
            }
            if (s == "Lot Wise Production")
            {
                LoadIFrame("Forms/RatioCalculation.aspx");
            }
            if (s == "Lend Borrow")
            {
                LoadIFrame("Forms/LendBorrow.aspx");
            }
            if (s == "Inquiry")
            {
                LoadIFrame("Forms/Inquiry.aspx");
            }
            

            ///////////////Reports////////////////
            if (s == "Item")
            {
                LoadIFrame("Forms/ReportForms/rptItem.aspx");
            }
            if (s == "Customer")
            {
                LoadIFrame("Forms/ReportForms/rptCustomer.aspx");
            }
            if (s == "Supplier")
            {
                LoadIFrame("Forms/ReportForms/rptSuppliers.aspx");
            }
            if (s == "Staff")
            {
                LoadIFrame("Forms/ReportForms/rptStaff.aspx");
            }
            if (s == "Purchase")
            {
                LoadIFrame("Forms/ReportForms/rptPurchase.aspx");
            }
            if (s == "Sale")
            {
                LoadIFrame("Forms/ReportForms/rptSales.aspx");
            }
            if (s == "Money Paid")
            {
                LoadIFrame("Forms/ReportForms/rptPayment.aspx");
            }
            if (s == "Money Received")
            {
                LoadIFrame("Forms/ReportForms/rptMoneyReceive.aspx");
            }
            if (s == "Income")
            {
                LoadIFrame("Forms/ReportForms/rptIncome.aspx");
            }
            if (s == "Expense")
            {
                LoadIFrame("Forms/ReportForms/rptExpense.aspx");
            }
            if (s == "Salary")
            {
                LoadIFrame("Forms/ReportForms/rptSalary.aspx");
            }
            if (s == "All Master Data")
            {
                LoadIFrame("Forms/ReportForms/rptMasterData.aspx");
            }
            if (s == "Report Manager")
            {
                LoadIFrame("Forms/ReportManager.aspx");
            }
            if (s == "Stock")
            {
                LoadIFrame("Forms/ReportForms/rptStocks.aspx");
            }
            if (s == "Stock Details")
            {
                LoadIFrame("Forms/ReportForms/rptStockDetails.aspx");
            }
            if (s == "Bank Transaction")
            {
                LoadIFrame("Forms/ReportForms/rptBankTransactions.aspx");
            }
            if (s == "Daily Transactions")
            {
                LoadIFrame("Forms/ReportForms/rptDailyTotalTransaction.aspx");
            }
            if (s == "Due List")
            {
                LoadIFrame("Forms/ReportForms/rptDueList.aspx");
            }
            if (s == "Lend & Borrow")
            {
                LoadIFrame("Forms/ReportForms/rptLendBorrow.aspx");
            }

        }
        protected void RadContextMenu1_ItemClick(object sender, RadMenuEventArgs e)
        {

        }
    }
}