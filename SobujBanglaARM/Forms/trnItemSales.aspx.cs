using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using Telerik.Web.UI;

namespace SobujBanglaARM.Forms
{
    public partial class trnItemSales : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        public DataTable dtItemDetailSales
        {
            get
            {
                object obj = this.Session["dtItemDetailSales"];
                if (obj != null)
                {
                    return (DataTable)obj;
                }
                DataTable dt1 = new DataTable();

                dt1.Columns.Add("rowid", typeof(Int16));
                dt1.Columns.Add("ItemCode", typeof(string));
                dt1.Columns.Add("ItemName", typeof(string));
                dt1.Columns.Add("ComId", typeof(string));
                dt1.Columns.Add("ShortName", typeof(string));
                dt1.Columns.Add("Qty", typeof(decimal));
                dt1.Columns.Add("UnitPrice", typeof(decimal));
                dt1.Columns.Add("Amount", typeof(decimal));

                dt1.PrimaryKey = new DataColumn[] { dt1.Columns["ItemCode"], dt1.Columns["ComId"] };

                this.Session["dtItemDetailSales"] = dt1;
                return dtItemDetailSales;
            }
        }
        public DataTable dtSales
        {
            get
            {
                object obj = this.Session["dtSales"];
                if (obj != null)
                {
                    return (DataTable)obj;
                }
                DataTable dt1 = new DataTable();
                dt1.Columns.Add("rowid", typeof(Int32));
                dt1.Columns.Add("SalesId", typeof(string));
                dt1.Columns.Add("SalesDate", typeof(DateTime));
                dt1.Columns.Add("CustId", typeof(string));
                dt1.Columns.Add("CustomerName", typeof(string));
                dt1.Columns.Add("NetAmount", typeof(decimal));
                dt1.Columns.Add("Balance", typeof(decimal));
                dt1.PrimaryKey = new DataColumn[] { dt1.Columns["rowid"] };
                this.Session["dtSales"] = dt1;
                return dtSales;
            }
        }
        public string GetAutoNumber(string fieldName, string tableName)
        {
            // concat date with max to create new invoice number
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
                string ss = "Select convert(int,Max(" + fieldName + ")) from " + tableName;
                SqlCommand cmd = new SqlCommand(ss, con);

                con.Open();
                int x = (int)cmd.ExecuteScalar() + 1;
                return x.ToString();
            }
            catch (Exception)
            {
                return "10001";
            }
            finally
            {
                con.Close();
            }
        }
        public void EnableControl(bool ec)
        {
            txtQty.Enabled = ec;
            txtRemarks.Enabled = ec;
            txtUnitPrice.Enabled = ec;
            txtAmount.Enabled = ec;
            txtPacketCost.Enabled = ec;
            txtDiscount.Enabled = ec;
            txtLabourCost.Enabled = ec;
            txtAgentCost.Enabled = ec;
            txtOtherCost.Enabled = ec;
            txtPaidAmount.Enabled = ec;
            cmItemName.Enabled = ec;
            cmCompany.Enabled = ec;
            cmCustomerName.Enabled = ec;
            cmReference.Enabled = ec;
            cmShipTo.Enabled = ec;
            dpSalesDt.Enabled = ec;
        }
        public void EnableControlPartial(bool ec)
        {
            txtChequeNo.Enabled = ec;
            dpCheckDate.Enabled = ec;
            txtBankName.Enabled = ec;
            txtChequeDetails.Enabled = ec;
        }
        public void ClearControl()
        {
            txtAmount.Text = "";
            txtQty.Text = "";
            txtRemarks.Text = "";
            txtUnitPrice.Text = "";
            cmItemName.Text = "";
            cmItemName.SelectedValue = "";
            cmCompany.Text = "";
            cmCompany.SelectedValue = "";
            cmCustomerName.Text = "";
            cmCustomerName.SelectedValue = "";
            cmReference.Text = "";
            cmReference.SelectedValue = "";
            cmShipTo.Text = "";
            cmShipTo.SelectedValue = "";
            dpSalesDt.SelectedDate = null;
            txtPaidAmount.Text = "0";
            txtPacketCost.Text = "0";
            txtDiscount.Text = "0";
            txtLabourCost.Text = "0";
            txtAgentCost.Text = "0";
            txtOtherCost.Text = "0";
            lblPreviousDue.Text = "0.00";
            lblDueAmount.Text = "0.00";
            lblNetAmount.Text = "0.00";
        }
        public void ClearControlPartial()
        {
            cmItemName.SelectedValue = "";
            cmItemName.Text = "";
            cmCompany.SelectedValue = "";
            cmCompany.Text = "";
            txtQty.Text = "";
            txtUnitPrice.Text = "";
            txtAmount.Text = "";
        }
        public void ButtonControl(string bc)
        {
            //// Which button you click control according by button
            if (bc == "L")
            {
                btnNew.Enabled = true;
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
                btnCancel.Enabled = false;
                lblOperationMode.Text = "";
                lblMessage.Text = "";
            }
            else if (bc == "N")
            {
                btnNew.Enabled = false;
                btnSave.Enabled = true;
                btnDelete.Enabled = false;
                btnCancel.Enabled = true;
                lblOperationMode.Text = "Save Mode";
                lblMessage.Text = "";
            }
            else if (bc == "F")
            {
                btnNew.Enabled = false;
                btnSave.Enabled = false;
                btnDelete.Enabled = true;
                btnCancel.Enabled = true;
                lblOperationMode.Text = "";
                lblMessage.Text = "";
            }
            else if (bc == "E")
            {
                btnNew.Enabled = false;
                btnSave.Enabled = true;
                btnDelete.Enabled = true;
                btnCancel.Enabled = true;
                lblOperationMode.Text = "Edit Mode";
                lblMessage.Text = "";
            }
        }
        private void ReloadMainGrid()
        {
            try
            {
                dtSales.Clear();
                rgMain.DataSource = null;
                rgMain.Rebind();

                con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
                con.Open();
                cmd = new SqlCommand("Sp_Sales", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 4;
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                adpt.Fill(ds);
                dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow newRow = this.dtSales.NewRow();
                    newRow["rowid"] = this.dtSales.Rows.Count + 1;
                    newRow["SalesId"] = dt.Rows[i]["SalesId"].ToString();
                    newRow["SalesDate"] = dt.Rows[i]["SalesDate"].ToString();
                    newRow["CustId"] = dt.Rows[i]["CustId"].ToString();
                    newRow["CustomerName"] = dt.Rows[i]["CustomerName"].ToString();
                    newRow["NetAmount"] = dt.Rows[i]["NetAmount"].ToString();
                    newRow["Balance"] = dt.Rows[i]["Balance"].ToString();
                    this.dtSales.Rows.Add(newRow);
                    this.dtSales.AcceptChanges();
                }
                rgMain.Rebind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        private void DataRefill()
        {
            try
            {
                dtItemDetailSales.Clear();
                RadGrid1.DataSource = null;
                RadGrid1.Rebind();

                GridDataItem selectedItem = (GridDataItem)rgMain.SelectedItems[0];
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
                con.Open();
                cmd = new SqlCommand("Sp_Sales", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 6;
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                cmd.Parameters.Add("@SalesId", SqlDbType.VarChar).Value = selectedItem["SalesId"].Text;
                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                adpt.Fill(ds);
                dt1 = ds.Tables[0];
                dt2 = ds.Tables[1];

                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    DataRow newRow = this.dtItemDetailSales.NewRow();
                    newRow["rowid"] = this.dtItemDetailSales.Rows.Count + 1;
                    newRow["ItemCode"] = dt2.Rows[i]["ItemCode"].ToString();
                    newRow["ItemName"] = dt2.Rows[i]["ItemName"].ToString();
                    newRow["ComId"] = dt2.Rows[i]["ComId"].ToString();
                    newRow["ShortName"] = dt2.Rows[i]["ShortName"].ToString();
                    newRow["Qty"] = dt2.Rows[i]["Qty"].ToString();
                    newRow["UnitPrice"] = dt2.Rows[i]["UnitPrice"].ToString();
                    newRow["Amount"] = dt2.Rows[i]["Amount"].ToString();
                    this.dtItemDetailSales.Rows.Add(newRow);
                    this.dtItemDetailSales.AcceptChanges();
                }
                RadGrid1.Rebind();

                //cmPurchaseFor.SelectedValue = dt1.Rows[0]["Projcode"].ToString();
                txtSalesID.Text = dt1.Rows[0]["SalesId"].ToString();
                dpSalesDt.SelectedDate = Convert.ToDateTime(dt1.Rows[0]["SalesDate"].ToString());
                cmCustomerName.SelectedValue = dt1.Rows[0]["CustId"].ToString();
                cmCustomerName.Text = dt1.Rows[0]["CustName"].ToString();
                txtRemarks.Text = dt1.Rows[0]["Remarks"].ToString();
                cmReference.SelectedValue = dt1.Rows[0]["Reference"].ToString();
                //= dt1.Rows[0]["TotalAmount"].ToString();
                txtLabourCost.Text = dt1.Rows[0]["LabourCost"].ToString();
                txtAgentCost.Text = dt1.Rows[0]["AgentCost"].ToString();
                txtOtherCost.Text = dt1.Rows[0]["OtherCost"].ToString();
                txtPaidAmount.Text = dt1.Rows[0]["Paid"].ToString();
                txtPacketCost.Text = dt1.Rows[0]["PacketCost"].ToString();
                txtDiscount.Text = dt1.Rows[0]["Discount"].ToString();
                lblNetAmount.Text = dt1.Rows[0]["NetAmount"].ToString();
                lblPreviousDue.Text = dt1.Rows[0]["PreviousDue"].ToString();
                lblDueAmount.Text = dt1.Rows[0]["Due"].ToString();
                hfOpBalanceYN.Value = dt1.Rows[0]["OpBalance"].ToString();

                cmPayMode.SelectedValue = dt1.Rows[0]["PayMode"].ToString();
                txtChequeNo.Text = dt1.Rows[0]["ChequeNo"].ToString();
                txtChequeDetails.Text = dt1.Rows[0]["ChequeDetails"].ToString();
                //dpCheckDate.SelectedDate = Convert.ToDateTime(dt1.Rows[0]["ChequeDt"]);
                //txtDepositBank.Text = dt1.Rows[0]["DepositBank"].ToString();
                txtBankName.Text = dt1.Rows[0]["BankName"].ToString();

                txtPaidAmount.Text = dt1.Rows[0]["Paid"].ToString();

                 ViewState["PreviousDue"] = Convert.ToDecimal(dt1.Rows[0]["PreviousDue"].ToString());
                ViewState["TotalAmount"] = (Convert.ToDecimal(lblPreviousDue.Text) + Convert.ToDecimal(lblNetAmount.Text));
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        private void SaveData()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
            con.Open();
            SqlTransaction myTran;
            SqlCommand cmd = con.CreateCommand();
            myTran = con.BeginTransaction();
            cmd.Connection = con;
            cmd.Transaction = myTran;
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                cmd = new SqlCommand("Sp_Sales", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 1;
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                cmd.Parameters.Add("@SalesId", SqlDbType.Int).Value = txtSalesID.Text;
                cmd.Parameters.Add("@CustId", SqlDbType.Int).Value = cmCustomerName.SelectedValue;
                cmd.Parameters.Add("@Paid", SqlDbType.Decimal).Value = Convert.ToDecimal(txtPaidAmount.Text);
                cmd.Parameters.Add("@Due", SqlDbType.Decimal).Value = Convert.ToDecimal(lblDueAmount.Text);

                cmd.Parameters.Add("@SalesDate", SqlDbType.DateTime).Value = dpSalesDt.SelectedDate;
                cmd.Parameters.Add("@Reference", SqlDbType.NVarChar).Value = cmReference.SelectedValue;
                cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = txtRemarks.Text;

                cmd.Parameters.Add("@PacketCost", SqlDbType.Decimal).Value = Convert.ToDecimal(txtPacketCost.Text);
                cmd.Parameters.Add("@LabourCost", SqlDbType.Decimal).Value = Convert.ToDecimal(txtLabourCost.Text);
                cmd.Parameters.Add("@AgentCost", SqlDbType.Decimal).Value = Convert.ToDecimal(txtAgentCost.Text);
                cmd.Parameters.Add("@OtherCost", SqlDbType.Decimal).Value = Convert.ToDecimal(txtOtherCost.Text);
                cmd.Parameters.Add("@Discount", SqlDbType.Decimal).Value = Convert.ToDecimal(txtDiscount.Text);
                cmd.Parameters.Add("@NetAmount", SqlDbType.Decimal).Value = lblNetAmount.Text;

                cmd.Parameters.Add("@PayMode", SqlDbType.VarChar).Value = cmPayMode.SelectedValue;
                cmd.Parameters.Add("@ChequeNo", SqlDbType.VarChar).Value = txtChequeNo.Text;
                if (dpCheckDate.SelectedDate != null)
                    cmd.Parameters.Add("@ChequeDt", SqlDbType.DateTime).Value = Convert.ToDateTime(dpCheckDate.SelectedDate);
                cmd.Parameters.Add("@BankName", SqlDbType.VarChar).Value = txtBankName.Text;
                cmd.Parameters.Add("@ChequeDetails", SqlDbType.VarChar).Value = txtChequeDetails.Text;
                cmd.Transaction = myTran;
                cmd.ExecuteNonQuery();

                ///////////////////////update or opening Money Receive Table and insert it for the first time (check)
                cmd = new SqlCommand("Sp_Sales", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 7;
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                cmd.Parameters.Add("@SalesId", SqlDbType.Int).Value = txtSalesID.Text;
                cmd.Parameters.Add("@CustId", SqlDbType.NVarChar).Value = cmCustomerName.SelectedValue;
                cmd.Parameters.Add("@ReceivedDate", SqlDbType.DateTime).Value = dpSalesDt.SelectedDate;
                cmd.Parameters.Add("@Paid", SqlDbType.Decimal).Value = Convert.ToDecimal(txtPaidAmount.Text);
                cmd.Parameters.Add("@PayMode", SqlDbType.NVarChar).Value = cmPayMode.SelectedValue;
                cmd.Parameters.Add("@ChequeNo", SqlDbType.NVarChar).Value = txtChequeNo.Text;
                cmd.Parameters.Add("@ChequeDetails", SqlDbType.NVarChar).Value = txtChequeDetails.Text;
                if (dpCheckDate.SelectedDate != null)
                    cmd.Parameters.Add("@ChequeDt", SqlDbType.DateTime).Value = Convert.ToDateTime(dpCheckDate.SelectedDate);
                cmd.Parameters.Add("@BankName", SqlDbType.NVarChar).Value = txtBankName.Text;
                cmd.Parameters.Add("@UserId", SqlDbType.NVarChar).Value = AppEnv.Current.p_UserName.ToString();
                cmd.Transaction = myTran;
                cmd.ExecuteNonQuery();

                if (dtItemDetailSales.Rows.Count > 0)
                {
                    for (int i = 0; i < dtItemDetailSales.Rows.Count; i++)
                    {
                        cmd = new SqlCommand("Sp_Sales", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 11;
                        cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                        cmd.Parameters.Add("@SalesId", SqlDbType.Int).Value = Convert.ToInt32(txtSalesID.Text);
                        cmd.Parameters.Add("@CustId", SqlDbType.VarChar).Value = cmCustomerName.SelectedValue;
                        cmd.Parameters.Add("@Itemcode", SqlDbType.Int).Value = dtItemDetailSales.Rows[i]["ItemCode"].ToString();
                        cmd.Parameters.Add("@ComId", SqlDbType.VarChar).Value = dtItemDetailSales.Rows[i]["ComId"].ToString();
                        cmd.Parameters.Add("@UnitPrice", SqlDbType.Decimal).Value = dtItemDetailSales.Rows[i]["UnitPrice"].ToString();
                        cmd.Parameters.Add("@Qty", SqlDbType.Decimal).Value = dtItemDetailSales.Rows[i]["Qty"].ToString();
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = dtItemDetailSales.Rows[i]["Amount"].ToString();
                        cmd.Parameters.Add("@UserId", SqlDbType.NVarChar).Value = AppEnv.Current.p_UserName.ToString();
                        cmd.Transaction = myTran;
                        cmd.ExecuteNonQuery();

                        cmd = new SqlCommand("Sp_Sales", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 111;
                        cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                        cmd.Parameters.Add("@Qty", SqlDbType.Decimal).Value = dtItemDetailSales.Rows[i]["Qty"].ToString();
                        cmd.Parameters.Add("@Itemcode", SqlDbType.Int).Value = dtItemDetailSales.Rows[i]["ItemCode"].ToString();
                        cmd.Transaction = myTran;
                        cmd.ExecuteNonQuery();
                    }
                }
                myTran.Commit();
                con.Close();

                ButtonControl("L");
                ClearControl();
                EnableControl(false);
                EnableControlPartial(false);
                dtItemDetailSales.Clear();
                dtItemDetailSales.AcceptChanges();
                RadGrid1.Rebind();
                lblMessage.Text = "Sales Successfull.";
                ReloadMainGrid();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message.ToString();
                try
                {
                    myTran.Rollback();
                }
                catch (Exception ex1)
                {
                    lblMessage.Text = ex1.Message.ToString();
                    return;
                }
            }
        }
        private void EidtData()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
            con.Open();
            SqlTransaction myTran;
            SqlCommand cmd = con.CreateCommand();
            myTran = con.BeginTransaction();
            cmd.Connection = con;
            cmd.Transaction = myTran;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd = new SqlCommand("Sp_Sales", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 3;
            cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
            cmd.Parameters.Add("@CustId", SqlDbType.NVarChar).Value = cmCustomerName.SelectedValue;
            cmd.Parameters.Add("@SalesId", SqlDbType.NVarChar).Value = txtSalesID.Text;
            cmd.Transaction = myTran;
            cmd.ExecuteNonQuery();

            try
            {
                cmd = new SqlCommand("Sp_Sales", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 1;
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                cmd.Parameters.Add("@SalesId", SqlDbType.Int).Value = txtSalesID.Text;
                cmd.Parameters.Add("@CustId", SqlDbType.Int).Value = cmCustomerName.SelectedValue;
                cmd.Parameters.Add("@Paid", SqlDbType.Decimal).Value = Convert.ToDecimal(txtPaidAmount.Text);
                cmd.Parameters.Add("@Due", SqlDbType.Decimal).Value = Convert.ToDecimal(lblDueAmount.Text);

                cmd.Parameters.Add("@SalesDate", SqlDbType.DateTime).Value = dpSalesDt.SelectedDate;
                cmd.Parameters.Add("@Reference", SqlDbType.NVarChar).Value = cmReference.SelectedValue;
                cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = txtRemarks.Text;

                cmd.Parameters.Add("@PacketCost", SqlDbType.Decimal).Value = Convert.ToDecimal(txtPacketCost.Text);
                cmd.Parameters.Add("@LabourCost", SqlDbType.Decimal).Value = Convert.ToDecimal(txtLabourCost.Text);
                cmd.Parameters.Add("@AgentCost", SqlDbType.Decimal).Value = Convert.ToDecimal(txtAgentCost.Text);
                cmd.Parameters.Add("@OtherCost", SqlDbType.Decimal).Value = Convert.ToDecimal(txtOtherCost.Text);
                cmd.Parameters.Add("@Discount", SqlDbType.Decimal).Value = Convert.ToDecimal(txtDiscount.Text);
                cmd.Parameters.Add("@NetAmount", SqlDbType.Decimal).Value = lblNetAmount.Text;

                cmd.Parameters.Add("@PayMode", SqlDbType.VarChar).Value = cmPayMode.SelectedValue;
                cmd.Parameters.Add("@ChequeNo", SqlDbType.VarChar).Value = txtChequeNo.Text;
                if (dpCheckDate.SelectedDate != null)
                    cmd.Parameters.Add("@ChequeDt", SqlDbType.DateTime).Value = Convert.ToDateTime(dpCheckDate.SelectedDate);
                cmd.Parameters.Add("@BankName", SqlDbType.VarChar).Value = txtBankName.Text;
                cmd.Parameters.Add("@ChequeDetails", SqlDbType.VarChar).Value = txtChequeDetails.Text;
                cmd.Transaction = myTran;
                cmd.ExecuteNonQuery();

                ///////////////////////update or opening Money Receive Table and insert it for the first time (check)
                cmd = new SqlCommand("Sp_Sales", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 7;
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                cmd.Parameters.Add("@SalesId", SqlDbType.Int).Value = txtSalesID.Text;
                cmd.Parameters.Add("@CustId", SqlDbType.NVarChar).Value = cmCustomerName.SelectedValue;
                cmd.Parameters.Add("@ReceivedDate", SqlDbType.DateTime).Value = dpSalesDt.SelectedDate;
                cmd.Parameters.Add("@Paid", SqlDbType.Decimal).Value = Convert.ToDecimal(txtPaidAmount.Text);
                cmd.Parameters.Add("@PayMode", SqlDbType.NVarChar).Value = cmPayMode.SelectedValue;
                cmd.Parameters.Add("@ChequeNo", SqlDbType.NVarChar).Value = txtChequeNo.Text;
                cmd.Parameters.Add("@ChequeDetails", SqlDbType.NVarChar).Value = txtChequeDetails.Text;
                if (dpCheckDate.SelectedDate != null)
                    cmd.Parameters.Add("@ChequeDt", SqlDbType.DateTime).Value = Convert.ToDateTime(dpCheckDate.SelectedDate);
                cmd.Parameters.Add("@BankName", SqlDbType.NVarChar).Value = txtBankName.Text;
                cmd.Parameters.Add("@UserId", SqlDbType.NVarChar).Value = AppEnv.Current.p_UserName.ToString();
                cmd.Transaction = myTran;
                cmd.ExecuteNonQuery();

                if (dtItemDetailSales.Rows.Count > 0)
                {
                    for (int i = 0; i < dtItemDetailSales.Rows.Count; i++)
                    {
                        cmd = new SqlCommand("Sp_Sales", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 11;
                        cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                        cmd.Parameters.Add("@SalesId", SqlDbType.Int).Value = Convert.ToInt32(txtSalesID.Text);
                        cmd.Parameters.Add("@CustId", SqlDbType.NVarChar).Value = cmCustomerName.SelectedValue;
                        cmd.Parameters.Add("@Itemcode", SqlDbType.Int).Value = dtItemDetailSales.Rows[i]["ItemCode"].ToString();
                        cmd.Parameters.Add("@ComId", SqlDbType.Int).Value = dtItemDetailSales.Rows[i]["ComId"].ToString();
                        cmd.Parameters.Add("@UnitPrice", SqlDbType.Decimal).Value = dtItemDetailSales.Rows[i]["UnitPrice"].ToString();
                        cmd.Parameters.Add("@Qty", SqlDbType.Decimal).Value = dtItemDetailSales.Rows[i]["Qty"].ToString();
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = dtItemDetailSales.Rows[i]["Amount"].ToString();
                        cmd.Parameters.Add("@UserId", SqlDbType.NVarChar).Value = AppEnv.Current.p_UserName.ToString();
                        cmd.Transaction = myTran;
                        cmd.ExecuteNonQuery();

                        cmd = new SqlCommand("Sp_Sales", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 111;
                        cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                        cmd.Parameters.Add("@Qty", SqlDbType.Decimal).Value = dtItemDetailSales.Rows[i]["Qty"].ToString();
                        cmd.Parameters.Add("@Itemcode", SqlDbType.Int).Value = dtItemDetailSales.Rows[i]["ItemCode"].ToString();
                        cmd.Transaction = myTran;
                        cmd.ExecuteNonQuery();
                    }
                }
                myTran.Commit();
                con.Close();

                ButtonControl("L");
                ClearControl();
                EnableControl(false);
                EnableControlPartial(false);
                dtItemDetailSales.Clear();
                dtItemDetailSales.AcceptChanges();
                RadGrid1.Rebind();
                lblMessage.Text = "Sales Successfull.";
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message.ToString();
                try
                {
                    myTran.Rollback();
                }
                catch (Exception ex1)
                {
                    lblMessage.Text = ex1.Message.ToString();
                    return;
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                EnableControl(false);
                EnableControlPartial(false);
                ClearControl();
                ButtonControl("L");
                this.dtItemDetailSales.Clear();
                ReloadMainGrid();
                ViewState["PreviousDue"] = "0";
                ViewState["TotalAmount"] = "0";
            }
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                EnableControl(true);
                ClearControl();
                ButtonControl("N");
                dpSalesDt.SelectedDate = System.DateTime.Now;
                int max = Convert.ToInt32(GetAutoNumber("SalesId", "tblSalesDTL"));
                txtSalesID.Text = max.ToString();

                int maxChallan = Convert.ToInt32(GetAutoNumber("VoucherNo", "tblSalesDTL"));
                int maxBill = Convert.ToInt32(GetAutoNumber("BillNo", "tblSalesDTL"));
                lblOperationMode.Text = "Save Mode";
                lblMessage.Text = "";
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (cmPayMode.SelectedValue == "")
            {
                lblMessage.Text = "Select Pay Mode.";
            }
            else if (cmCustomerName.SelectedValue == "")
            {
                lblMessage.Text = "Select Customer.";
            }
            else if (dpSalesDt.SelectedDate == null)
            {
                lblMessage.Text = "Select Invoice Date.";
            }
            else if (dtItemDetailSales.Rows.Count <= 0)
            {
                lblMessage.Text = "Add Item.";
            }
            else if (Convert.ToDecimal(txtPaidAmount.Text) > 0 && cmPayMode.SelectedValue == "")
            {
                lblMessage.Text = "Select Receive Mode.";
            }
            //else if (Convert.ToDecimal(lblDueAmount.Text) < 0)
            //{
            //    lblMessage.Text = "Due Amount can not be Negetive.";
            //}
            else
            {
                if (lblOperationMode.Text == "Save Mode")
                {
                    ViewState["PreviousDue"] = lblPreviousDue.Text;
                    ViewState["TotalAmount"] = Convert.ToDecimal(lblNetAmount.Text) + Convert.ToDecimal(lblPreviousDue.Text);
                    try
                    {
                        //============Validation Check==============
                        con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
                        con.Open();
                        if (dtItemDetailSales.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtItemDetailSales.Rows.Count; i++)
                            {
                                cmd = new SqlCommand("Sp_ValidationAll", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 1;
                                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                                cmd.Parameters.Add("@Itemcode", SqlDbType.Int).Value = dtItemDetailSales.Rows[i]["ItemCode"].ToString();
                                SqlDataReader dr = cmd.ExecuteReader();
                                if (dr.Read())
                                {
                                    if (Convert.ToDecimal(dr["StockQty"].ToString()) < Convert.ToDecimal(dtItemDetailSales.Rows[i]["Qty"].ToString()))
                                    {
                                        lblMessage.Text = "Check Stock Quantity for " + dtItemDetailSales.Rows[i]["ItemCode"].ToString() + "";
                                        return;
                                    }
                                }
                                dr.Close();
                            }
                        }
                        con.Close();
                        //=============End Validation Check=============

                        SaveData();
                        PrintInvoice();
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = ex.Message;
                    }
                }
                //else if (lblOperationMode.Text == "Edit Mode")
                //{
                //    try
                //    {
                //        EidtData();
                //    }
                //    catch (Exception ex)
                //    {
                //        lblMessage.Text = ex.Message;
                //    }
                //}
                ReloadMainGrid();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSalesID.Text == "")
            {
                lblMessage.Text = "Sales ID can not be Blank!";
            }
            else
            {

            }
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ButtonControl("L");
            EnableControl(false);
            ClearControl();
            dtItemDetailSales.Clear();
            RadGrid1.Rebind();
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (cmItemName.Text == "")
            {
                lblMessage.Text = "Select Item.";
            }
            else if (txtQty.Text == "" || Convert.ToDecimal(txtQty.Text) <= 0)
            {
                lblMessage.Text = "Qty can not be Blank.";
            }
            else if (txtUnitPrice.Text == "")
            {
                lblMessage.Text = "Unit Price can not be Blank.";
            }
            else if (txtAmount.Text == "")
            {
                lblMessage.Text = "Amount can not be Blank.";
            }
            else
            {
                try
                {
                    if (cmItemName.SelectedValue != "")
                    {
                        DataRow newRow = this.dtItemDetailSales.NewRow();
                        newRow["rowid"] = this.dtItemDetailSales.Rows.Count + 1;
                        newRow["ItemCode"] = cmItemName.SelectedValue;
                        newRow["ItemName"] = cmItemName.Text;
                        newRow["ComId"] = cmCompany.SelectedValue;
                        newRow["ShortName"] = cmCompany.Text;
                        newRow["Qty"] = Convert.ToDecimal(txtQty.Text);
                        newRow["UnitPrice"] = Convert.ToDecimal(txtUnitPrice.Text);
                        newRow["Amount"] = Convert.ToDecimal(txtAmount.Text);
                        this.dtItemDetailSales.Rows.Add(newRow);
                        this.dtItemDetailSales.AcceptChanges();
                        RadGrid1.Rebind();
                        ClearControlPartial();
                        lblMessage.Text = "";

                        txtPaidAmount.Text = "0";
                        txtPacketCost.Text = "0";
                        txtDiscount.Text = "0";
                        txtLabourCost.Text = "0";
                        txtAgentCost.Text = "0";
                        txtOtherCost.Text = "0";
                        if (txtPacketCost.Text != "" && txtDiscount.Text != "" && lblDueAmount.Text != "" && lblPreviousDue.Text != "" && txtLabourCost.Text != "" && txtAgentCost.Text != "" && txtPaidAmount.Text != "")
                        {
                            decimal total = Convert.ToDecimal(ViewState["sumAmount"]);
                            decimal labourCost = Convert.ToDecimal(txtLabourCost.Text);
                            decimal agentCost = Convert.ToDecimal(txtAgentCost.Text);
                            decimal otherCost = Convert.ToDecimal(txtOtherCost.Text);
                            decimal PacketCost = Convert.ToDecimal(txtPacketCost.Text);
                            decimal discount = Convert.ToDecimal(txtDiscount.Text);
                            decimal balance = Convert.ToDecimal(lblPreviousDue.Text);
                            decimal netamount = (total + labourCost + agentCost + otherCost + PacketCost) - discount;
                            lblNetAmount.Text = netamount.ToString();
                            decimal paid = Convert.ToDecimal(txtPaidAmount.Text);
                            decimal due = ((total + labourCost + agentCost + otherCost + PacketCost + balance) - discount) - paid;
                            lblDueAmount.Text = due.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = ex.Message;
                }
            }
        }
        protected void cmItemName_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                txtQty.Text = "";
                txtUnitPrice.Text = "";
                txtAmount.Text = "";

                con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
                con.Open();
                cmd = new SqlCommand("select a.SalesRate,b.Name PackName from tblItems a left join tblPacking b on a.Pack=b.Id where ItemCode='" + cmItemName.SelectedValue + "'", con);
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    txtUnitPrice.Text = dr["SalesRate"].ToString();
                    lblPacking.Text = dr["PackName"].ToString();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        protected void txtUnitPrice_TextChanged(object sender, EventArgs e)
        {
            if (txtQty.Text != "" && txtUnitPrice.Text != "")
            {
                decimal q = Convert.ToDecimal(txtQty.Text);
                decimal p = Convert.ToDecimal(txtUnitPrice.Text);
                decimal r = q * p;
                txtAmount.Text = r.ToString();
            }
        }
        decimal sumQty = 0;
        decimal sumAmount = 0;
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = this.dtItemDetailSales;
        }
        protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    sumQty += Convert.ToDecimal(dataItem["Qty"].Text);
                    sumAmount += Convert.ToDecimal(dataItem["Amount"].Text);
                    ViewState["sumAmount"] = sumAmount;
                    lblDueAmount.Text = sumAmount.ToString();
                }
                else if (e.Item is GridFooterItem)
                {
                    GridFooterItem footerItem = e.Item as GridFooterItem;
                    footerItem["Qty"].Text = sumQty.ToString();
                    footerItem["Amount"].Text = sumAmount.ToString();
                    footerItem["ItemName"].Text = "Total : ";
                    footerItem.BackColor = System.Drawing.Color.LightGray;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "GridDelete")
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    int RID = Convert.ToInt16(item["rowid"].Text);
                    DataRow[] Drow = this.dtItemDetailSales.Select("rowid='" + RID + "'");
                    if (Drow.Length > 0)
                    {
                        Drow[0].Delete();
                        this.dtItemDetailSales.AcceptChanges();
                        RadGrid1.Rebind();
                    }
                    txtPaidAmount.Text = "0";
                    txtPacketCost.Text = "0";
                    txtDiscount.Text = "0";
                    txtLabourCost.Text = "0";
                    txtAgentCost.Text = "0";
                    txtOtherCost.Text = "0";
                    if (txtPacketCost.Text != "" && txtDiscount.Text != "" && lblDueAmount.Text != "" && txtLabourCost.Text != "" && txtAgentCost.Text != "" && txtPaidAmount.Text != "")
                    {
                        decimal total = Convert.ToDecimal(ViewState["sumAmount"]);
                        decimal labourCost = Convert.ToDecimal(txtLabourCost.Text);
                        decimal agentCost = Convert.ToDecimal(txtAgentCost.Text);
                        decimal otherCost = Convert.ToDecimal(txtOtherCost.Text);
                        decimal PacketCost = Convert.ToDecimal(txtPacketCost.Text);
                        decimal discount = Convert.ToDecimal(txtDiscount.Text);
                        decimal netamount = (total + labourCost + agentCost + otherCost + PacketCost) - discount;
                        lblNetAmount.Text = netamount.ToString();
                        decimal paid = Convert.ToDecimal(txtPaidAmount.Text);
                        decimal due = ((total + labourCost + agentCost + otherCost + PacketCost) - discount) - paid;
                        lblDueAmount.Text = due.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        protected void rbtDeleteGrid_Click(object sender, ImageClickEventArgs e)
        {

        }
        protected void txtPaidAmount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtPacketCost.Text != "" && txtDiscount.Text != "" && lblDueAmount.Text != "" && lblPreviousDue.Text != "" && txtLabourCost.Text != "" && txtAgentCost.Text != "" && txtPaidAmount.Text != "")
                {
                    decimal total = Convert.ToDecimal(ViewState["sumAmount"]);
                    decimal labourCost = Convert.ToDecimal(txtLabourCost.Text);
                    decimal agentCost = Convert.ToDecimal(txtAgentCost.Text);
                    decimal otherCost = Convert.ToDecimal(txtOtherCost.Text);
                    decimal PacketCost = Convert.ToDecimal(txtPacketCost.Text);
                    decimal discount = Convert.ToDecimal(txtDiscount.Text);
                    decimal balance = Convert.ToDecimal(lblPreviousDue.Text);
                    decimal netamount = (total + labourCost + agentCost + otherCost + PacketCost) - discount;
                    lblNetAmount.Text = netamount.ToString();
                    decimal paid = Convert.ToDecimal(txtPaidAmount.Text);
                    decimal due = ((total + labourCost + agentCost + otherCost + PacketCost + balance) - discount) - paid;
                    lblDueAmount.Text = due.ToString();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        protected void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtPacketCost.Text != "" && txtDiscount.Text != "" && lblDueAmount.Text != "" && txtLabourCost.Text != "" && txtAgentCost.Text != "" && txtPaidAmount.Text != "")
                {
                    decimal total = Convert.ToDecimal(ViewState["sumAmount"]);
                    decimal labourCost = Convert.ToDecimal(txtLabourCost.Text);
                    decimal agentCost = Convert.ToDecimal(txtAgentCost.Text);
                    decimal otherCost = Convert.ToDecimal(txtOtherCost.Text);
                    decimal PacketCost = Convert.ToDecimal(txtPacketCost.Text);
                    decimal discount = Convert.ToDecimal(txtDiscount.Text);
                    decimal netamount = (total + labourCost + agentCost + otherCost + PacketCost) - discount;
                    lblNetAmount.Text = netamount.ToString();
                    decimal paid = Convert.ToDecimal(txtPaidAmount.Text);
                    decimal due = ((total + labourCost + agentCost + otherCost + PacketCost) - discount) - paid;
                    lblDueAmount.Text = due.ToString();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        protected void txtLabourCost_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtPacketCost.Text != "" && txtDiscount.Text != "" && lblDueAmount.Text != "" && txtLabourCost.Text != "" && txtAgentCost.Text != "" && txtPaidAmount.Text != "")
                {
                    decimal total = Convert.ToDecimal(ViewState["sumAmount"]);
                    decimal labourCost = Convert.ToDecimal(txtLabourCost.Text);
                    decimal agentCost = Convert.ToDecimal(txtAgentCost.Text);
                    decimal otherCost = Convert.ToDecimal(txtOtherCost.Text);
                    decimal PacketCost = Convert.ToDecimal(txtPacketCost.Text);
                    decimal discount = Convert.ToDecimal(txtDiscount.Text);
                    decimal netamount = (total + labourCost + agentCost + otherCost + PacketCost) - discount;
                    lblNetAmount.Text = netamount.ToString();
                    decimal paid = Convert.ToDecimal(txtPaidAmount.Text);
                    decimal due = ((total + labourCost + agentCost + otherCost + PacketCost) - discount) - paid;
                    lblDueAmount.Text = due.ToString();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        protected void txtOtherCost_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtPacketCost.Text != "" && txtDiscount.Text != "" && lblDueAmount.Text != "" && txtLabourCost.Text != "" && txtAgentCost.Text != "" && txtPaidAmount.Text != "")
                {
                    decimal total = Convert.ToDecimal(ViewState["sumAmount"]);
                    decimal labourCost = Convert.ToDecimal(txtLabourCost.Text);
                    decimal agentCost = Convert.ToDecimal(txtAgentCost.Text);
                    decimal otherCost = Convert.ToDecimal(txtOtherCost.Text);
                    decimal PacketCost = Convert.ToDecimal(txtPacketCost.Text);
                    decimal discount = Convert.ToDecimal(txtDiscount.Text);
                    decimal netamount = (total + labourCost + agentCost + otherCost + PacketCost) - discount;
                    lblNetAmount.Text = netamount.ToString();
                    decimal paid = Convert.ToDecimal(txtPaidAmount.Text);
                    decimal due = ((total + labourCost + agentCost + otherCost + PacketCost) - discount) - paid;
                    lblDueAmount.Text = due.ToString();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        protected void txtPacketCost_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtPacketCost.Text != "" && txtDiscount.Text != "" && lblDueAmount.Text != "" && txtLabourCost.Text != "" && txtAgentCost.Text != "" && txtPaidAmount.Text != "")
                {
                    decimal total = Convert.ToDecimal(ViewState["sumAmount"]);
                    decimal labourCost = Convert.ToDecimal(txtLabourCost.Text);
                    decimal agentCost = Convert.ToDecimal(txtAgentCost.Text);
                    decimal otherCost = Convert.ToDecimal(txtOtherCost.Text);
                    decimal PacketCost = Convert.ToDecimal(txtPacketCost.Text);
                    decimal discount = Convert.ToDecimal(txtDiscount.Text);
                    decimal netamount = (total + labourCost + agentCost + otherCost + PacketCost) - discount;
                    lblNetAmount.Text = netamount.ToString();
                    decimal paid = Convert.ToDecimal(txtPaidAmount.Text);
                    decimal due = ((total + labourCost + agentCost + otherCost + PacketCost) - discount) - paid;
                    lblDueAmount.Text = due.ToString();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        protected void rgMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ClearControl();
                ClearControlPartial();

                DataRefill();
                ButtonControl("E");
                EnableControl(true);

                if (hfOpBalanceYN.Value == "Y")
                {
                    EnableControl(false);
                    ButtonControl("L");
                }
                else
                {
                    EnableControl(true);
                    ButtonControl("E");
                }

                if (txtPacketCost.Text != "" && txtDiscount.Text != "" && lblDueAmount.Text != "" && txtLabourCost.Text != "" && txtAgentCost.Text != "" && txtPaidAmount.Text != "")
                {
                    //decimal total = Convert.ToDecimal(lblNetAmount.Text); // Convert.ToDecimal(ViewState["sumAmount"]);
                    //decimal labourCost = Convert.ToDecimal(txtLabourCost.Text);
                    //decimal agentCost = Convert.ToDecimal(txtAgentCost.Text);
                    //decimal otherCost = Convert.ToDecimal(txtOtherCost.Text);
                    //decimal PacketCost = Convert.ToDecimal(txtPacketCost.Text);
                    //decimal discount = Convert.ToDecimal(txtDiscount.Text);
                    //decimal netamount = (total + labourCost + agentCost + otherCost + PacketCost) - discount;
                    //lblNetAmount.Text = netamount.ToString();
                    //decimal paid = Convert.ToDecimal(txtPaidAmount.Text);
                    //decimal due = ((total + labourCost + agentCost + otherCost + PacketCost) - discount) - paid;
                    //lblDueAmount.Text = due.ToString();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        protected void rgMain_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            rgMain.DataSource = dtSales;
        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
        protected void txtQty_TextChanged(object sender, EventArgs e)
        {
            if (txtQty.Text != "" && txtUnitPrice.Text != "")
            {
                decimal q = Convert.ToDecimal(txtQty.Text);
                decimal p = Convert.ToDecimal(txtUnitPrice.Text);
                decimal r = q * p;
                txtAmount.Text = r.ToString();
            }
        }
        protected void cmPayMode_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cmPayMode.SelectedValue == "Cash")
            {
                EnableControlPartial(false);
            }
            else
            {
                //txtDepositBank.Text = "";
                EnableControlPartial(true);
            }
        }

        protected void cmShipTo_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
                con.Open();
                cmd = new SqlCommand("Sp_Sales", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 8;
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                foreach (DataRow dataRow in dt.Rows)
                {
                    RadComboBoxItem item = new RadComboBoxItem();
                    item.Text = (string)dataRow["Name"];
                    item.Value = dataRow["CustId"].ToString();

                    string Id = (string)dataRow["CustId"].ToString();
                    item.Attributes.Add("CustId", Id.ToString());

                    cmShipTo.Items.Add(item);
                    item.DataBind();
                }
                con.Close();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        protected void cmReference_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
                con.Open();
                cmd = new SqlCommand("Sp_Sales", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 8;
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                foreach (DataRow dataRow in dt.Rows)
                {
                    RadComboBoxItem item = new RadComboBoxItem();
                    item.Text = (string)dataRow["Name"];
                    item.Value = dataRow["CustId"].ToString();

                    string Id = (string)dataRow["CustId"].ToString();
                    item.Attributes.Add("CustId", Id.ToString());

                    cmReference.Items.Add(item);
                    item.DataBind();
                }
                con.Close();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        protected void cmCustomerName_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
                con.Open();
                cmd = new SqlCommand("Sp_Sales", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.VarChar).Value = 8;
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                foreach (DataRow dataRow in dt.Rows)
                {
                    RadComboBoxItem item = new RadComboBoxItem();
                    item.Text = (string)dataRow["Name"].ToString();
                    item.Value = dataRow["CustId"].ToString();

                    string Id = (string)dataRow["CustId"].ToString();
                    item.Attributes.Add("CustId", Id.ToString());

                    cmCustomerName.Items.Add(item);
                    item.DataBind();
                }
                con.Close();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        protected void cmItemName_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
                con.Open();
                cmd = new SqlCommand("Sp_ReportManager", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 2;
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                foreach (DataRow dataRow in dt.Rows)
                {
                    RadComboBoxItem item = new RadComboBoxItem();
                    item.Text = (string)dataRow["ItemName"];
                    item.Value = dataRow["ItemCode"].ToString();

                    string ItemCode = (string)dataRow["ItemCode"].ToString();
                    item.Attributes.Add("ItemCode", ItemCode.ToString());

                    cmItemName.Items.Add(item);
                    item.DataBind();
                }
                con.Close();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        protected void cmCustomerName_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                if (cmCustomerName.SelectedValue != "")
                {
                    con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
                    con.Open();
                    cmd = new SqlCommand("Sp_MoneyReceived", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 5;
                    cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                    cmd.Parameters.Add("@CustId", SqlDbType.Int).Value = Convert.ToInt32(cmCustomerName.SelectedValue);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    lblPreviousDue.Text = dt.Rows[0]["Balance"].ToString(); //Convert.ToDecimal(dt.Rows[0]["Balance"].ToString()).ToString("0.00");
                    lblMessage.Text = "";
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                lblPreviousDue.Text = "0.00";
                lblMessage.Text = ex.Message;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
            //    con.Open();

            //    cmd = new SqlCommand("Sp_Sales", con);
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 2;
            //    cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
            //    cmd.Parameters.Add("@SalesId", SqlDbType.VarChar).Value = txtSalesID.Text;
            //    cmd.Parameters.Add("@CustId", SqlDbType.Int).Value = Convert.ToInt32(cmCustomerName.SelectedValue);
            //    cmd.Parameters.Add("@Paid", SqlDbType.Decimal).Value = txtPaidAmount.Text;
            //    cmd.Parameters.Add("@NetAmount", SqlDbType.Decimal).Value = lblNetAmount.Text;
            //    cmd.ExecuteNonQuery();

            //    cmd = new SqlCommand("Sp_Sales", con);
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 3;
            //    cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
            //    cmd.Parameters.Add("@SalesId", SqlDbType.VarChar).Value = txtSalesID.Text;
            //    cmd.Parameters.Add("@CustId", SqlDbType.Int).Value = Convert.ToInt32(cmCustomerName.SelectedValue);
            //    cmd.ExecuteNonQuery();

            //    con.Close();

            //    ButtonControl("L");
            //    ClearControl();
            //    EnableControl(false);
            //    lblMessage.Text = "Delete Successfull.";
            //}
            //catch (Exception ex)
            //{
            //    lblMessage.Text = ex.Message.ToString();
            //}

            //ReloadMainGrid();
        }

        protected void cmCompany_ItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
                con.Open();
                cmd = new SqlCommand("Sp_Login", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Mode", SqlDbType.Int).Value = 2;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                foreach (DataRow dataRow in dt.Rows)
                {
                    RadComboBoxItem item = new RadComboBoxItem();
                    item.Text = (string)dataRow["ShortName"];
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
        protected void btnPrintPreview_Click(object sender, EventArgs e)
        {
            PrintInvoice();
        }
        protected void PrintInvoice()
        {
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
                con.Open();

                if (AppEnv.Current.p_rptSource != null)
                {
                    AppEnv.Current.p_rptSource.Close();
                    AppEnv.Current.p_rptSource.Dispose();
                }
                AppEnv.Current.p_rptSource = new ReportDocument();
                cmd = new SqlCommand("Sp_ReportManager", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.VarChar).Value = 7;
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                cmd.Parameters.Add("@SalesId", SqlDbType.VarChar).Value = txtSalesID.Text;
                SqlDataAdapter Dap = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                Dap.Fill(dt);
                cmd.Dispose();

                string tempPath = "";
                string reportName = "";
                if (CheckBox1.Checked == true)
                {
                    reportName = "Challan-" + cmCustomerName.Text;
                    AppEnv.Current.p_rptObject = "~/Reports/Challan.rpt";
                    tempPath = @System.IO.Path.GetTempPath() + "Challan";
                }
                else
                {
                    reportName = "Invoice-" + cmCustomerName.Text;
                    AppEnv.Current.p_rptObject = "~/Reports/ImpInvoice.rpt";
                    tempPath = @System.IO.Path.GetTempPath() + "Invoice";
                }

                AppEnv.Current.p_rptSource.Load(Server.MapPath(AppEnv.Current.p_rptObject.ToString()));
                AppEnv.Current.p_rptSource.SetDataSource(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    //=========common
                    ((TextObject)AppEnv.Current.p_rptSource.ReportDefinition.Sections["Section1"].ReportObjects["txtCompanyName"]).Text = Session["Name"].ToString();
                    //==========Eco=======================
                    ((TextObject)AppEnv.Current.p_rptSource.ReportDefinition.Sections["Section1"].ReportObjects["txtAddress"]).Text = Session["Address"].ToString();
                    ((TextObject)AppEnv.Current.p_rptSource.ReportDefinition.Sections["Section1"].ReportObjects["txtContact1"]).Text = Session["Contact1"].ToString();
                    ((TextObject)AppEnv.Current.p_rptSource.ReportDefinition.Sections["Section1"].ReportObjects["txtEmail"]).Text = Session["Email"].ToString();

                    ((TextObject)AppEnv.Current.p_rptSource.ReportDefinition.Sections["Section4"].ReportObjects["txtPreviousDue"]).Text = String.Format("{0:n}", ViewState["PreviousDue"]);
                    ((TextObject)AppEnv.Current.p_rptSource.ReportDefinition.Sections["Section4"].ReportObjects["txtTotalAmount"]).Text = String.Format("{0:n}", ViewState["TotalAmount"]);
                    //String.Format("{0:n}", 1234); 
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

                    //if (rbtnCrystal.Checked == true)
                    //{
                    //    string URL = "~/CRpreview.aspx";
                    //    URL = Page.ResolveClientUrl(URL);
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick", "javascript:window.open('" + URL + "','_blank','height=700,width=1200,toolbar=no,location=no, directories=no,status=no,menubar=no,scrollbars=no,resizable=no');", true);
                    //}
                }
                else
                {
                    lblMessage.Text = "Data is not available.";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
    }
}