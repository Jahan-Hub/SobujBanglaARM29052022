using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using Telerik.Web.UI;

namespace SobujBanglaARM.Forms.ReportForms
{
    public partial class rptDailyTotalTransaction : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand Cmd;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dpStartDate.SelectedDate = DateTime.Now;
            }
        }
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                if (dpStartDate.SelectedDate == null)
                {
                    lblMessage.Text = "Select Date.";
                    return;
                }
                //if (cmCompany.SelectedValue == "")
                //{
                //    lblMessage.Text = "Select Company.";
                //    return;
                //}
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
                con.Open();

                if (AppEnv.Current.p_rptSource != null)
                {
                    AppEnv.Current.p_rptSource.Close();
                    AppEnv.Current.p_rptSource.Dispose();
                }
                AppEnv.Current.p_rptSource = new ReportDocument();
                SqlCommand Cmd;
                Cmd = new SqlCommand("Sp_ReportManager", con);
                Cmd.CommandType = CommandType.StoredProcedure;
                if (cmCompany.SelectedValue != "")
                {
                    Cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                    Cmd.Parameters.Add("@mode", SqlDbType.VarChar).Value = 25;
                }
                else
                    Cmd.Parameters.Add("@mode", SqlDbType.VarChar).Value = 26;

                if (dpStartDate.SelectedDate != null)
                    Cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = dpStartDate.SelectedDate;
                SqlDataAdapter adpt = new SqlDataAdapter(Cmd);
                DataSet ds = new DataSet();
                adpt.Fill(ds);
                Cmd.Dispose();
                ReportDocument subPurchase;
                ReportDocument subSales;
                ReportDocument subIncome;
                ReportDocument subExpense;
                ReportDocument subPayment;
                ReportDocument subMoneyReceived;
                ReportDocument subBankTransaction;
                ReportDocument subLendBorrow;

                DataTable dtRptPurchase = ds.Tables[0];
                DataTable dtRptSales = ds.Tables[1];
                DataTable dtRptIncome = ds.Tables[2];
                DataTable dtRptExpense = ds.Tables[3];
                DataTable dtRptPayment = ds.Tables[4];
                DataTable dtRptMoneyReceived = ds.Tables[5];
                DataTable dtRptBankTransaction = ds.Tables[6];
                DataTable dtRptLendBorrow = ds.Tables[7];

                Cmd.Dispose();

                string tempPath = "";
                string reportName = "";

                AppEnv.Current.p_rptObject = "~/Reports/DailyAllTransactions.rpt";
                tempPath = @System.IO.Path.GetTempPath() + "DailyAllTransactions";
                reportName = "DailyAllTransactions";

                AppEnv.Current.p_rptSource.Load(Server.MapPath(AppEnv.Current.p_rptObject.ToString()));

                subPurchase = AppEnv.Current.p_rptSource.OpenSubreport("rptSubSales.rpt");
                subPurchase.SetDataSource(dtRptSales);
                subSales = AppEnv.Current.p_rptSource.OpenSubreport("rptSubSales.rpt");
                subSales.SetDataSource(dtRptSales);
                subIncome = AppEnv.Current.p_rptSource.OpenSubreport("rptSubIncome.rpt");
                subIncome.SetDataSource(dtRptIncome);
                subExpense = AppEnv.Current.p_rptSource.OpenSubreport("rptSubExpense.rpt");
                subExpense.SetDataSource(dtRptExpense);
                subPayment = AppEnv.Current.p_rptSource.OpenSubreport("rptSubPayment.rpt");
                subPayment.SetDataSource(dtRptPayment);
                subMoneyReceived = AppEnv.Current.p_rptSource.OpenSubreport("rptSubMoneyReceived.rpt");
                subMoneyReceived.SetDataSource(dtRptMoneyReceived);
                subBankTransaction = AppEnv.Current.p_rptSource.OpenSubreport("rptSubBankTransaction.rpt");
                subBankTransaction.SetDataSource(dtRptBankTransaction);
                subLendBorrow = AppEnv.Current.p_rptSource.OpenSubreport("rptSubLendBorrow.rpt");
                subLendBorrow.SetDataSource(dtRptLendBorrow);

                AppEnv.Current.p_rptSource.SetDataSource(dtRptPurchase);


                con.Close();

                ((TextObject)AppEnv.Current.p_rptSource.ReportDefinition.Sections["Section1"].ReportObjects["txtCompanyName"]).Text = Session["Name"].ToString();
                ((TextObject)AppEnv.Current.p_rptSource.ReportDefinition.Sections["Section1"].ReportObjects["txtAddress"]).Text = Session["Address"].ToString();
                ((TextObject)AppEnv.Current.p_rptSource.ReportDefinition.Sections["Section1"].ReportObjects["txtContact"]).Text = Session["Contact1"].ToString();
                ((TextObject)AppEnv.Current.p_rptSource.ReportDefinition.Sections["Section1"].ReportObjects["txtContact"]).Text = Session["Contact2"].ToString();
                ((TextObject)AppEnv.Current.p_rptSource.ReportDefinition.Sections["Section5"].ReportObjects["txtUserName"]).Text = AppEnv.Current.p_UserName;
                ((TextObject)AppEnv.Current.p_rptSource.ReportDefinition.Sections["Section1"].ReportObjects["txtDatePeriod"]).Text = "Date: " + Convert.ToDateTime(dpStartDate.SelectedDate).ToString("dd-MMM-yyyy");

                if (rbtnPdf.Checked == true)
                {
                    ViewState["preview"] = "pdf";
                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    ExportFormatType format = ExportFormatType.PortableDocFormat;
                    try
                    {
                        AppEnv.Current.p_rptSource.ExportToHttpResponse(format, Response, true, reportName);
                    }
                    finally
                    {
                        AppEnv.Current.p_rptSource.Close();
                        AppEnv.Current.p_rptSource.Dispose();
                        GC.Collect();
                    }
                }
                if (rbtnWord.Checked == true)
                {
                    ViewState["preview"] = "word";
                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    ExportFormatType format = ExportFormatType.WordForWindows;
                    try
                    {
                        AppEnv.Current.p_rptSource.ExportToHttpResponse(format, Response, true, reportName);
                    }
                    finally
                    {
                        AppEnv.Current.p_rptSource.Close();
                        AppEnv.Current.p_rptSource.Dispose();
                        GC.Collect();
                    }
                }
                if (rbtnExcel.Checked == true)
                {
                    ViewState["preview"] = "word";
                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    ExportFormatType format = ExportFormatType.Excel;
                    try
                    {
                        AppEnv.Current.p_rptSource.ExportToHttpResponse(format, Response, true, reportName);
                    }
                    finally
                    {
                        AppEnv.Current.p_rptSource.Close();
                        AppEnv.Current.p_rptSource.Dispose();
                        GC.Collect();
                    }
                }

                if (rbtnCrystal.Checked == true)
                {
                    string URL = "~/CRpreview.aspx";
                    URL = Page.ResolveClientUrl(URL);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick", "javascript:window.open('" + URL + "','_blank','height=700,width=1200,toolbar=no,location=no, directories=no,status=no,menubar=no,scrollbars=no,resizable=no');", true);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        protected void cmCompany_ItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
                con.Open();
                Cmd = new SqlCommand("select * from tblCompany", con);
                Cmd.CommandType = CommandType.Text;
                SqlDataAdapter adapter = new SqlDataAdapter(Cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                foreach (DataRow dataRow in dt.Rows)
                {
                    RadComboBoxItem item = new RadComboBoxItem();
                    item.Text = (string)dataRow["Name"].ToString();
                    item.Value = dataRow["ComId"].ToString();

                    cmCompany.Items.Add(item);
                    item.DataBind();
                }
                con.Close();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
    }
}