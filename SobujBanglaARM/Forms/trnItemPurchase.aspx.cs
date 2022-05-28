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
    public partial class trnItemPurchase : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        public DataTable dtItemDetailsPur
        {
            get
            {
                object obj = this.Session["dtItemDetailsPur"];
                if (obj != null)
                {
                    return (DataTable)obj;
                }
                DataTable dt1 = new DataTable();
                dt1.Columns.Add("rowid", typeof(Int16));
                dt1.Columns.Add("ItemCode", typeof(string));
                dt1.Columns.Add("ItemName", typeof(string));
                dt1.Columns.Add("Qty", typeof(decimal));
                dt1.Columns.Add("UnitRate", typeof(decimal));
                dt1.Columns.Add("Amount", typeof(decimal));
                dt1.PrimaryKey = new DataColumn[] { dt1.Columns["ItemCode"] };
                this.Session["dtItemDetailsPur"] = dt1;
                return dtItemDetailsPur;
            }
        }
        public DataTable dtPurchase
        {
            get
            {
                object obj = this.Session["dtPurchase"];
                if (obj != null)
                {
                    return (DataTable)obj;
                }
                DataTable dt1 = new DataTable();
                dt1.Columns.Add("rowid", typeof(Int32));
                dt1.Columns.Add("PurId", typeof(string));
                dt1.Columns.Add("BillDate", typeof(DateTime));
                dt1.Columns.Add("SupplierId", typeof(string));
                dt1.Columns.Add("SupplierName", typeof(string));
                dt1.Columns.Add("NetAmount", typeof(decimal));
                dt1.Columns.Add("Balance", typeof(decimal));
                dt1.PrimaryKey = new DataColumn[] { dt1.Columns["rowid"] };
                this.Session["dtPurchase"] = dt1;
                return dtPurchase;
            }
        }
        public string GetAutoNumber(string fieldName, string tableName)
        {
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
                string ss = "Select convert(int,isnull(Max(" + fieldName + "),0)) from " + tableName;
                SqlCommand cmd = new SqlCommand(ss, con);
                con.Open();
                int x = (int)cmd.ExecuteScalar() + 1;
                return x.ToString();
            }
            catch (Exception)
            {
                return "1";
            }
            finally
            {
                con.Close();
            }
        }
        public void EnableControl(bool ec)
        {
            txtQty.Enabled = ec;
            txtBillNo.Enabled = ec;
            txtRemarks.Enabled = ec;
            txtUnitPrice.Enabled = ec;
            cmItemName.Enabled = ec;
            cmPartyName.Enabled = ec;
            dpPurchaseDate.Enabled = ec;
            txtAmount.Enabled = ec;
            txtDiscount.Enabled = ec;
            txtPackageCost.Enabled = ec;
            txtLabourCost.Enabled = ec;
            txtAgentCost.Enabled = ec;
            txtOtherCost.Enabled = ec;
            txtPaidAmount.Enabled = ec;
        }
        public void ClearControl()
        {
            txtAmount.Text = "";
            txtQty.Text = "";
            txtBillNo.Text = "";
            txtRemarks.Text = "";
            txtUnitPrice.Text = "";
            cmItemName.Text = "";
            cmItemName.SelectedValue = "";
            cmPartyName.Text = "";
            cmPartyName.SelectedValue = "";
            dpPurchaseDate.SelectedDate = null;

            txtPaidAmount.Text = "0";
            txtPackageCost.Text = "0";
            txtDiscount.Text = "0";
            txtLabourCost.Text = "0";
            txtAgentCost.Text = "0";
            txtOtherCost.Text = "0";
            lblDueAmount.Text = "0.00";
            lblNetAmount.Text = "0.00";
            lblPreviousDue.Text = "0.00";
        }
        public void ClearControlPartial()
        {
            cmItemName.SelectedValue = "";
            cmItemName.Text = "";
            txtQty.Text = "";
            txtUnitPrice.Text = "";
            txtAmount.Text = "";
        }
        public void EnableControlPartial(bool ec)
        {
            txtChequeNo.Enabled = ec;
            //txtDepositBank.Enabled = ec;
            dpCheckDate.Enabled = ec;
            txtBankName.Enabled = ec;
            txtChequeDetails.Enabled = ec;
        }
        public void ButtonControl(string bc)
        {
            //// Which button you click control according by button
            if (bc == "L")
            {
                btnNew.Enabled = true;
                btnSave.Enabled = false;
                //btnSearch.Enabled = true;
                //btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnCancel.Enabled = false;
                lblOperationMode.Text = "";
                lblMessage.Text = "";
            }
            else if (bc == "N")
            {
                btnNew.Enabled = false;
                btnSave.Enabled = true;
                //btnSearch.Enabled = false;
                //btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnCancel.Enabled = true;
                lblOperationMode.Text = "Save Mode";
                lblMessage.Text = "";
            }
            //else if (bc == "S")
            //{
            //    btnNew.Enabled = false;
            //    btnSave.Enabled = false;
            //    btnEdit.Enabled = false;
            //    btnCancel.Enabled = false;
            //    lblOperationMode.Text = "";
            //    lblMessage.Text = "";

            //}
            else if (bc == "F")
            {
                btnNew.Enabled = false;
                btnSave.Enabled = false;
                //btnSearch.Enabled = true;
                //btnEdit.Enabled = true;
                btnDelete.Enabled = true;
                btnCancel.Enabled = true;
                lblOperationMode.Text = "";
                lblMessage.Text = "";
            }
            else if (bc == "E")
            {
                btnNew.Enabled = false;
                btnSave.Enabled = true;
                //btnEdit.Enabled = false;
                btnDelete.Enabled = true;
                btnCancel.Enabled = true;
                lblOperationMode.Text = "Edit Mode";
                lblMessage.Text = "";
            }

            //else if (bc == "C")
            //{
            //    btnNew.Enabled = true;
            //    btnSave.Enabled = false;
            //    btnEdit.Enabled = false;
            //    btnCancel.Enabled = false;
            //    lblOperationMode.Text = "";
            //    lblMessage.Text = "";
            //}
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
                cmd = new SqlCommand("Sp_Purchase", con); // insert to header table
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 1;
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                cmd.Parameters.Add("@PurId", SqlDbType.VarChar).Value = txtPurchaseNo.Text;
                cmd.Parameters.Add("@BillNo", SqlDbType.NVarChar).Value = txtBillNo.Text;
                cmd.Parameters.Add("@BillDate", SqlDbType.DateTime).Value = dpPurchaseDate.SelectedDate;
                cmd.Parameters.Add("@SupplierId", SqlDbType.Int).Value = Convert.ToInt32(cmPartyName.SelectedValue);
                cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = txtRemarks.Text;

                cmd.Parameters.Add("@PacketCost", SqlDbType.Decimal).Value = Convert.ToDecimal(txtPackageCost.Text);
                cmd.Parameters.Add("@LabourCost", SqlDbType.Decimal).Value = Convert.ToDecimal(txtLabourCost.Text);
                cmd.Parameters.Add("@AgentCost", SqlDbType.Decimal).Value = Convert.ToDecimal(txtAgentCost.Text);
                cmd.Parameters.Add("@OtherCost", SqlDbType.Decimal).Value = Convert.ToDecimal(txtOtherCost.Text);
                cmd.Parameters.Add("@Discount", SqlDbType.Decimal).Value = Convert.ToDecimal(txtDiscount.Text);
                cmd.Parameters.Add("@NetAmount", SqlDbType.Decimal).Value = lblNetAmount.Text;
                cmd.Parameters.Add("@Paid", SqlDbType.Decimal).Value = Convert.ToDecimal(txtPaidAmount.Text);
                cmd.Parameters.Add("@Due", SqlDbType.Decimal).Value = Convert.ToDecimal(lblDueAmount.Text);

                cmd.Parameters.Add("@PayMode", SqlDbType.VarChar).Value = cmPayMode.SelectedValue;
                cmd.Parameters.Add("@ChequeNo", SqlDbType.VarChar).Value = txtChequeNo.Text;
                if (dpCheckDate.SelectedDate != null)
                    cmd.Parameters.Add("@ChequeDt", SqlDbType.DateTime).Value = Convert.ToDateTime(dpCheckDate.SelectedDate);
                cmd.Parameters.Add("@BankName", SqlDbType.VarChar).Value = txtBankName.Text;
                cmd.Parameters.Add("@ChequeDetails", SqlDbType.VarChar).Value = txtChequeDetails.Text;
                cmd.Parameters.Add("@Userid", SqlDbType.NVarChar).Value = AppEnv.Current.p_UserName.ToString();
                cmd.Transaction = myTran;
                cmd.ExecuteNonQuery();

                ///////////////////////update or opening Payment Table and insert it for the first time (check)
                cmd = new SqlCommand("Sp_Purchase", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 7;
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                cmd.Parameters.Add("@PurID", SqlDbType.NVarChar).Value = txtPurchaseNo.Text;
                cmd.Parameters.Add("@SupplierId", SqlDbType.NVarChar).Value = cmPartyName.SelectedValue;
                cmd.Parameters.Add("@BillNo", SqlDbType.NVarChar).Value = txtBillNo.Text;
                cmd.Parameters.Add("@PayDate", SqlDbType.DateTime).Value = dpPurchaseDate.SelectedDate;
                cmd.Parameters.Add("@PayAmount", SqlDbType.Decimal).Value = Convert.ToDecimal(txtPaidAmount.Text);
                cmd.Parameters.Add("@PayMode", SqlDbType.NVarChar).Value = cmPayMode.SelectedValue;
                cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = txtRemarks.Text;
                cmd.Parameters.Add("@ChequeNo", SqlDbType.NVarChar).Value = txtChequeNo.Text;
                cmd.Parameters.Add("@ChequeDetails", SqlDbType.NVarChar).Value = txtChequeDetails.Text;
                if (dpCheckDate.SelectedDate != null)
                    cmd.Parameters.Add("@ChequeDt", SqlDbType.DateTime).Value = Convert.ToDateTime(dpCheckDate.SelectedDate);
                cmd.Parameters.Add("@BankName", SqlDbType.NVarChar).Value = txtBankName.Text;
                cmd.Parameters.Add("@UserId", SqlDbType.NVarChar).Value = AppEnv.Current.p_UserName.ToString();
                cmd.Transaction = myTran;
                cmd.ExecuteNonQuery();

                if (dtItemDetailsPur.Rows.Count > 0)
                {
                    for (int i = 0; i < dtItemDetailsPur.Rows.Count; i++)
                    {
                        cmd = new SqlCommand("Sp_Purchase", con); // insert to details table
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 11;
                        cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                        cmd.Parameters.Add("@PurID", SqlDbType.Int).Value = Convert.ToInt32(txtPurchaseNo.Text);
                        cmd.Parameters.Add("@SupplierId", SqlDbType.Int).Value = cmPartyName.SelectedValue;
                        cmd.Parameters.Add("@ItemCode", SqlDbType.Int).Value = dtItemDetailsPur.Rows[i]["ItemCode"].ToString();
                        cmd.Parameters.Add("@UnitPrice", SqlDbType.Decimal).Value = dtItemDetailsPur.Rows[i]["UnitRate"].ToString();
                        cmd.Parameters.Add("@Qty", SqlDbType.Decimal).Value = dtItemDetailsPur.Rows[i]["Qty"].ToString();
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = dtItemDetailsPur.Rows[i]["Amount"].ToString();
                        cmd.Parameters.Add("@Userid", SqlDbType.NVarChar).Value = AppEnv.Current.p_UserName.ToString();
                        cmd.Transaction = myTran;
                        cmd.ExecuteNonQuery();
                    }
                }
                myTran.Commit();
                con.Close();

                ButtonControl("L");
                ClearControl();
                EnableControl(false);
                dtItemDetailsPur.Clear();
                RadGrid1.Rebind();
                lblMessage.Text = "Purchase Successfull.";
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
        private void EditData()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
            con.Open();
            SqlTransaction myTran;
            SqlCommand cmd = con.CreateCommand();
            myTran = con.BeginTransaction();
            cmd.Connection = con;
            cmd.Transaction = myTran;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd = new SqlCommand("Sp_Purchase", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 3;
            cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
            cmd.Parameters.Add("@SupplierId", SqlDbType.NVarChar).Value = cmPartyName.SelectedValue;
            cmd.Parameters.Add("@PurID", SqlDbType.NVarChar).Value = txtPurchaseNo.Text;
            cmd.Transaction = myTran;
            cmd.ExecuteNonQuery();

            try
            {
                cmd = new SqlCommand("Sp_Purchase", con); // insert to header table
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 1;
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                cmd.Parameters.Add("@PurId", SqlDbType.VarChar).Value = txtPurchaseNo.Text;
                cmd.Parameters.Add("@BillNo", SqlDbType.NVarChar).Value = txtBillNo.Text;
                cmd.Parameters.Add("@BillDate", SqlDbType.DateTime).Value = dpPurchaseDate.SelectedDate;
                cmd.Parameters.Add("@SupplierId", SqlDbType.Int).Value = Convert.ToInt32(cmPartyName.SelectedValue);
                cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = txtRemarks.Text;

                cmd.Parameters.Add("@PacketCost", SqlDbType.Decimal).Value = Convert.ToDecimal(txtPackageCost.Text);
                cmd.Parameters.Add("@LabourCost", SqlDbType.Decimal).Value = Convert.ToDecimal(txtLabourCost.Text);
                cmd.Parameters.Add("@AgentCost", SqlDbType.Decimal).Value = Convert.ToDecimal(txtAgentCost.Text);
                cmd.Parameters.Add("@OtherCost", SqlDbType.Decimal).Value = Convert.ToDecimal(txtOtherCost.Text);
                cmd.Parameters.Add("@Discount", SqlDbType.Decimal).Value = Convert.ToDecimal(txtDiscount.Text);
                cmd.Parameters.Add("@NetAmount", SqlDbType.Decimal).Value = lblNetAmount.Text;
                cmd.Parameters.Add("@Paid", SqlDbType.Decimal).Value = Convert.ToDecimal(txtPaidAmount.Text);
                cmd.Parameters.Add("@Due", SqlDbType.Decimal).Value = Convert.ToDecimal(lblDueAmount.Text);

                cmd.Parameters.Add("@PayMode", SqlDbType.VarChar).Value = cmPayMode.SelectedValue;
                cmd.Parameters.Add("@ChequeNo", SqlDbType.VarChar).Value = txtChequeNo.Text;
                if (dpCheckDate.SelectedDate != null)
                    cmd.Parameters.Add("@ChequeDt", SqlDbType.DateTime).Value = Convert.ToDateTime(dpCheckDate.SelectedDate);
                cmd.Parameters.Add("@BankName", SqlDbType.VarChar).Value = txtBankName.Text;
                cmd.Parameters.Add("@ChequeDetails", SqlDbType.VarChar).Value = txtChequeDetails.Text;
                cmd.Parameters.Add("@Userid", SqlDbType.NVarChar).Value = AppEnv.Current.p_UserName.ToString();
                cmd.Transaction = myTran;
                cmd.ExecuteNonQuery();

                ///////////////////////update or opening Payment Table and insert it for the first time (check)
                cmd = new SqlCommand("Sp_Purchase", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 7;
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                cmd.Parameters.Add("@PurID", SqlDbType.NVarChar).Value = txtPurchaseNo.Text;
                cmd.Parameters.Add("@SupplierId", SqlDbType.NVarChar).Value = cmPartyName.SelectedValue;
                cmd.Parameters.Add("@BillNo", SqlDbType.NVarChar).Value = txtBillNo.Text;
                cmd.Parameters.Add("@PayDate", SqlDbType.DateTime).Value = dpPurchaseDate.SelectedDate;
                cmd.Parameters.Add("@Paid", SqlDbType.Decimal).Value = Convert.ToDecimal(txtPaidAmount.Text);
                cmd.Parameters.Add("@PayMode", SqlDbType.NVarChar).Value = cmPayMode.SelectedValue;
                cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = txtRemarks.Text;
                cmd.Parameters.Add("@ChequeNo", SqlDbType.NVarChar).Value = txtChequeNo.Text;
                cmd.Parameters.Add("@ChequeDetails", SqlDbType.NVarChar).Value = txtChequeDetails.Text;
                if (dpCheckDate.SelectedDate != null)
                    cmd.Parameters.Add("@ChequeDt", SqlDbType.DateTime).Value = Convert.ToDateTime(dpCheckDate.SelectedDate);
                cmd.Parameters.Add("@BankName", SqlDbType.NVarChar).Value = txtBankName.Text;
                cmd.Parameters.Add("@UserId", SqlDbType.NVarChar).Value = AppEnv.Current.p_UserName.ToString();
                cmd.Transaction = myTran;
                cmd.ExecuteNonQuery();

                if (dtItemDetailsPur.Rows.Count > 0)
                {
                    for (int i = 0; i < dtItemDetailsPur.Rows.Count; i++)
                    {
                        cmd = new SqlCommand("Sp_Purchase", con); // insert to details table
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 11;
                        cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                        cmd.Parameters.Add("@PurID", SqlDbType.Int).Value = Convert.ToInt32(txtPurchaseNo.Text);
                        cmd.Parameters.Add("@SupplierId", SqlDbType.Int).Value = cmPartyName.SelectedValue;
                        cmd.Parameters.Add("@ItemCode", SqlDbType.Int).Value = dtItemDetailsPur.Rows[i]["ItemCode"].ToString();
                        cmd.Parameters.Add("@UnitPrice", SqlDbType.Decimal).Value = dtItemDetailsPur.Rows[i]["UnitRate"].ToString();
                        cmd.Parameters.Add("@Qty", SqlDbType.Decimal).Value = dtItemDetailsPur.Rows[i]["Qty"].ToString();
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = dtItemDetailsPur.Rows[i]["Amount"].ToString();
                        cmd.Parameters.Add("@Userid", SqlDbType.NVarChar).Value = AppEnv.Current.p_UserName.ToString();
                        cmd.Transaction = myTran;
                        cmd.ExecuteNonQuery();
                    }
                }
                myTran.Commit();
                con.Close();

                ButtonControl("L");
                ClearControl();
                EnableControl(false);
                dtItemDetailsPur.Clear();
                RadGrid1.Rebind();
                lblMessage.Text = "Purchase Successfull.";
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
        private void ReloadMainGrid()
        {
            dtPurchase.Clear();
            rgMain.DataSource = null;
            rgMain.Rebind();

            con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
            con.Open();
            cmd = new SqlCommand("Sp_Purchase", con);
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
                DataRow newRow = this.dtPurchase.NewRow();
                newRow["rowid"] = this.dtPurchase.Rows.Count + 1;
                newRow["PurId"] = dt.Rows[i]["PurId"].ToString();
                newRow["BillDate"] = dt.Rows[i]["BillDate"].ToString();
                newRow["SupplierId"] = dt.Rows[i]["SupplierId"].ToString();
                newRow["SupplierName"] = dt.Rows[i]["SupplierName"].ToString();
                newRow["NetAmount"] = dt.Rows[i]["NetAmount"].ToString();
                newRow["Balance"] = dt.Rows[i]["Balance"].ToString();
                this.dtPurchase.Rows.Add(newRow);
                this.dtPurchase.AcceptChanges();
            }
            rgMain.Rebind();
        }
        private void DataRefill()
        {
            dtItemDetailsPur.Clear();
            RadGrid1.DataSource = null;
            RadGrid1.Rebind();

            GridDataItem selectedItem = (GridDataItem)rgMain.SelectedItems[0];

            con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
            con.Open();
            cmd = new SqlCommand("Sp_Purchase", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 6;
            cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
            cmd.Parameters.Add("@PurID", SqlDbType.VarChar).Value = selectedItem["PurID"].Text;
            SqlDataAdapter adpt = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            adpt.Fill(ds);
            dt1 = ds.Tables[0];
            dt2 = ds.Tables[1];

            try
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    DataRow newRow = this.dtItemDetailsPur.NewRow();
                    newRow["rowid"] = this.dtItemDetailsPur.Rows.Count + 1;
                    newRow["ItemCode"] = dt2.Rows[i]["ItemCode"].ToString();
                    newRow["ItemName"] = dt2.Rows[i]["ItemName"].ToString();
                    newRow["Qty"] = dt2.Rows[i]["Qty"].ToString();
                    newRow["UnitRate"] = dt2.Rows[i]["UnitPrice"].ToString();
                    newRow["Amount"] = dt2.Rows[i]["Amount"].ToString();
                    this.dtItemDetailsPur.Rows.Add(newRow);
                    this.dtItemDetailsPur.AcceptChanges();
                }
                RadGrid1.Rebind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
            txtPurchaseNo.Text = dt1.Rows[0]["PurID"].ToString();
            cmPartyName.SelectedValue = dt1.Rows[0]["SupplierId"].ToString();
            cmPartyName.Text = dt1.Rows[0]["SupplierName"].ToString();
            txtBillNo.Text = dt1.Rows[0]["BillNo"].ToString();
            dpPurchaseDate.SelectedDate = Convert.ToDateTime(dt1.Rows[0]["BillDate"].ToString());
            txtRemarks.Text = dt1.Rows[0]["Remarks"].ToString();
            txtLabourCost.Text = dt1.Rows[0]["LabourCost"].ToString();
            txtAgentCost.Text = dt1.Rows[0]["AgentCost"].ToString();
            txtOtherCost.Text = dt1.Rows[0]["OtherCost"].ToString();
            txtPaidAmount.Text = dt1.Rows[0]["Paid"].ToString();
            lblNetAmount.Text = dt1.Rows[0]["NetAmount"].ToString();
            txtDiscount.Text = dt1.Rows[0]["Discount"].ToString();
            txtPackageCost.Text = dt1.Rows[0]["PacketCost"].ToString();
            lblPreviousDue.Text = dt1.Rows[0]["PreviousDue"].ToString();
            lblDueAmount.Text = dt1.Rows[0]["Due"].ToString();
            txtPaidAmount.Text = dt1.Rows[0]["Paid"].ToString();
            hfOpBalanceYN.Value = dt1.Rows[0]["OpBalance"].ToString();

            cmPayMode.SelectedValue = dt1.Rows[0]["PayMode"].ToString();
            txtChequeNo.Text = dt1.Rows[0]["ChequeNo"].ToString();
            if (dt1.Rows[0]["ChequeDt"].ToString() != "")
                dpCheckDate.SelectedDate = Convert.ToDateTime(dt1.Rows[0]["ChequeDt"].ToString());
            txtBankName.Text = dt1.Rows[0]["BankName"].ToString();
            txtChequeDetails.Text = dt1.Rows[0]["ChequeDetails"].ToString();

            ViewState["PreviousDue"] = Convert.ToDecimal(dt1.Rows[0]["PreviousDue"].ToString());
            ViewState["TotalAmount"] = (Convert.ToDecimal(lblPreviousDue.Text) + Convert.ToDecimal(lblNetAmount.Text));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                EnableControl(false);
                ClearControl();
                ButtonControl("L");
                this.dtItemDetailsPur.Clear();
                ReloadMainGrid();
                ViewState["PreviousDue"] = "0";
                ViewState["TotalAmount"] = "0";
            }
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                int max = Convert.ToInt32(GetAutoNumber("PurId", "tblPurchaseDTL"));
                txtPurchaseNo.Text = max.ToString();
                EnableControl(true);
                ClearControl();
                ButtonControl("N");
                dpPurchaseDate.SelectedDate = System.DateTime.Now;
                dtItemDetailsPur.Clear();
                RadGrid1.Rebind();
                lblPreviousDue.Text = "0.00";
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPurchaseNo.Text == "")
                {
                    lblMessage.Text = "Purchase No can not be blank.";
                }
                else if (txtBillNo.Text == "")
                {
                    lblMessage.Text = "Bill No can not be blank.";
                }
                else if (cmPartyName.SelectedValue == "")
                {
                    lblMessage.Text = "Select Party Name.";
                }
                else if (dpPurchaseDate.SelectedDate == null)
                {
                    lblMessage.Text = "Select Purchase Date.";
                }
                else if (dtItemDetailsPur.Rows.Count <= 0)
                {
                    lblMessage.Text = "Add Item.";
                }
                else if (cmPayMode.SelectedValue == "")
                {
                    lblMessage.Text = "Select Pay Mode.";
                }
                else
                {
                    if (lblOperationMode.Text == "Save Mode")
                    {
                        ViewState["PreviousDue"] = lblPreviousDue.Text;
                        decimal netAmt = Convert.ToDecimal(lblNetAmount.Text);
                        decimal blnce = Convert.ToDecimal(lblPreviousDue.Text);
                        ViewState["TotalAmount"] = netAmt + blnce;

                        SaveData();
                        PrintInvoice();
                    }
                    //else if (lblOperationMode.Text == "Edit Mode")
                    //{
                    //    EditData();
                    //}
                }
                ReloadMainGrid();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtPurchaseNo.Text == "")
            {
                lblMessage.Text = "Purchase No is Blank!";
            }
            else
            {

            }
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            ButtonControl("E");
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ButtonControl("L");
            EnableControl(false);
            ClearControl();
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (cmItemName.Text == "")
            {
                lblMessage.Text = "Select Item.";
            }
            else if (txtQty.Text == "" || Convert.ToDecimal(txtQty.Text) <= 0)
            {
                lblMessage.Text = "Qty can not be Blank or Zero.";
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
                        DataRow newRow = this.dtItemDetailsPur.NewRow();
                        newRow["rowid"] = this.dtItemDetailsPur.Rows.Count + 1;
                        newRow["ItemCode"] = cmItemName.SelectedValue;
                        newRow["ItemName"] = cmItemName.Text;
                        newRow["Qty"] = Convert.ToDecimal(txtQty.Text);
                        newRow["UnitRate"] = Convert.ToDecimal(txtUnitPrice.Text);
                        newRow["Amount"] = Convert.ToDecimal(txtAmount.Text);
                        this.dtItemDetailsPur.Rows.Add(newRow);
                        this.dtItemDetailsPur.AcceptChanges();
                        RadGrid1.Rebind();
                        ClearControlPartial();
                        lblMessage.Text = "";

                        txtPaidAmount.Text = "0";
                        txtPackageCost.Text = "0";
                        txtDiscount.Text = "0";
                        txtLabourCost.Text = "0";
                        txtAgentCost.Text = "0";
                        txtOtherCost.Text = "0";

                        if (txtPackageCost.Text != "" && txtOtherCost.Text != "" && txtDiscount.Text != "" && lblPreviousDue.Text != "" && lblDueAmount.Text != "" && txtLabourCost.Text != "" && txtAgentCost.Text != "" && txtPaidAmount.Text != "")
                        {
                            decimal total = Convert.ToDecimal(ViewState["sumAmount"]);
                            decimal labourCost = Convert.ToDecimal(txtLabourCost.Text);
                            decimal agentCost = Convert.ToDecimal(txtAgentCost.Text);
                            decimal PackageCost = Convert.ToDecimal(txtPackageCost.Text);
                            decimal discount = Convert.ToDecimal(txtDiscount.Text);
                            decimal otherCost = Convert.ToDecimal(txtOtherCost.Text);
                            decimal balance = Convert.ToDecimal(lblPreviousDue.Text);
                            decimal netamount = (total + labourCost + agentCost + PackageCost + otherCost) - discount;
                            lblNetAmount.Text = netamount.ToString();
                            decimal paid = Convert.ToDecimal(txtPaidAmount.Text);
                            decimal due = ((total + labourCost + agentCost + PackageCost + otherCost + balance) - discount) - paid;
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
                cmd = new SqlCommand("select a.PurRate,b.Name PackName from tblItems a left join tblPacking b on a.Pack=b.Id where ItemCode='" + cmItemName.SelectedValue + "'", con);
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    txtUnitPrice.Text = dr["PurRate"].ToString();
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
            try
            {
                if (txtQty.Text != "" && txtUnitPrice.Text != "")
                {
                    decimal q = Convert.ToDecimal(txtQty.Text);
                    decimal p = Convert.ToDecimal(txtUnitPrice.Text);
                    decimal r = q * p;
                    txtAmount.Text = r.ToString();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        protected void txtQty_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtQty.Text != "" && txtUnitPrice.Text != "")
                {
                    decimal q = Convert.ToDecimal(txtQty.Text);
                    decimal p = Convert.ToDecimal(txtUnitPrice.Text);
                    decimal r = q * p;
                    txtAmount.Text = r.ToString();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = this.dtItemDetailsPur;
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
                    DataRow[] Drow = this.dtItemDetailsPur.Select("rowid='" + RID + "'");
                    if (Drow.Length > 0)
                    {
                        Drow[0].Delete();
                        this.dtItemDetailsPur.AcceptChanges();
                        RadGrid1.Rebind();
                    }

                    txtPaidAmount.Text = "0";
                    txtPackageCost.Text = "0";
                    txtDiscount.Text = "0";
                    txtLabourCost.Text = "0";
                    txtAgentCost.Text = "0";
                    //lblNetAmount.Text = "0";
                    //lblDueAmount.Text = "0";
                    if (txtPackageCost.Text != "" && txtOtherCost.Text != "" && txtDiscount.Text != "" && lblDueAmount.Text != "" && txtLabourCost.Text != "" && txtAgentCost.Text != "" && txtPaidAmount.Text != "")
                    {
                        decimal total = Convert.ToDecimal(ViewState["sumAmount"]);
                        decimal labourCost = Convert.ToDecimal(txtLabourCost.Text);
                        decimal agentCost = Convert.ToDecimal(txtAgentCost.Text);
                        decimal PackageCost = Convert.ToDecimal(txtPackageCost.Text);
                        decimal discount = Convert.ToDecimal(txtDiscount.Text);
                        decimal otherCost = Convert.ToDecimal(txtOtherCost.Text);
                        decimal netamount = (total + labourCost + agentCost + PackageCost + otherCost) - discount;
                        lblNetAmount.Text = netamount.ToString();
                        decimal paid = Convert.ToDecimal(txtPaidAmount.Text);
                        decimal due = ((total + labourCost + agentCost + PackageCost + otherCost) - discount) - paid;
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
        decimal sumQty = 0;
        decimal sumAmount = 0;
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
                    txtBillNo.Enabled = false;
                    EnableControl(false);
                    ButtonControl("L");
                }
                else
                {
                    EnableControl(true);
                    ButtonControl("E");
                }
                //if (txtPackageCost.Text != "" && txtOtherCost.Text != "" && txtDiscount.Text != "" && lblDueAmount.Text != "" && txtLabourCost.Text != "" && txtAgentCost.Text != "" && txtPaidAmount.Text != "")
                //{
                //    decimal total = Convert.ToDecimal(lblNetAmount.Text); //Convert.ToDecimal(ViewState["sumAmount"]);
                //    decimal labourCost = Convert.ToDecimal(txtLabourCost.Text);
                //    decimal agentCost = Convert.ToDecimal(txtAgentCost.Text);
                //    decimal PackageCost = Convert.ToDecimal(txtPackageCost.Text);
                //    decimal discount = Convert.ToDecimal(txtDiscount.Text);
                //    decimal otherCost = Convert.ToDecimal(txtOtherCost.Text);
                //    decimal netamount = (total + labourCost + agentCost + PackageCost + otherCost) - discount;
                //    lblNetAmount.Text = netamount.ToString();
                //    decimal paid = Convert.ToDecimal(txtPaidAmount.Text);
                //    decimal due = ((total + labourCost + agentCost + PackageCost + otherCost) - discount) - paid;
                //    lblDueAmount.Text = due.ToString();
                //}
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        protected void btnPurchaseStatement_Click(object sender, EventArgs e)
        {
            Response.Redirect("Forms\rptReportManager.aspx");
        }
        protected void cmPayMode_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cmPayMode.SelectedValue == "Cash")
            {
                txtChequeNo.Text = "";
                dpCheckDate.SelectedDate = null;
                //txtDepositBank.Text = "";
                txtBankName.Text = "";
                txtChequeDetails.Text = "";

                EnableControlPartial(false);
            }
            else
            {
                //txtDepositBank.Text = "";
                EnableControlPartial(true);
            }
        }
        protected void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtPackageCost.Text != "" && txtOtherCost.Text != "" && txtDiscount.Text != "" && lblPreviousDue.Text != "" && lblDueAmount.Text != "" && txtLabourCost.Text != "" && txtAgentCost.Text != "" && txtPaidAmount.Text != "")
                {
                    decimal total = Convert.ToDecimal(ViewState["sumAmount"]);
                    decimal labourCost = Convert.ToDecimal(txtLabourCost.Text);
                    decimal agentCost = Convert.ToDecimal(txtAgentCost.Text);
                    decimal PackageCost = Convert.ToDecimal(txtPackageCost.Text);
                    decimal discount = Convert.ToDecimal(txtDiscount.Text);
                    decimal otherCost = Convert.ToDecimal(txtOtherCost.Text);
                    decimal balance = Convert.ToDecimal(lblPreviousDue.Text);
                    decimal netamount = (total + labourCost + agentCost + PackageCost + otherCost) - discount;
                    lblNetAmount.Text = netamount.ToString();
                    decimal paid = Convert.ToDecimal(txtPaidAmount.Text);
                    decimal due = ((total + labourCost + agentCost + PackageCost + otherCost + balance) - discount) - paid;
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
                if (txtPackageCost.Text != "" && txtOtherCost.Text != "" && txtDiscount.Text != "" && lblPreviousDue.Text != "" && lblDueAmount.Text != "" && txtLabourCost.Text != "" && txtAgentCost.Text != "" && txtPaidAmount.Text != "")
                {
                    decimal total = Convert.ToDecimal(ViewState["sumAmount"]);
                    decimal labourCost = Convert.ToDecimal(txtLabourCost.Text);
                    decimal agentCost = Convert.ToDecimal(txtAgentCost.Text);
                    decimal PackageCost = Convert.ToDecimal(txtPackageCost.Text);
                    decimal discount = Convert.ToDecimal(txtDiscount.Text);
                    decimal otherCost = Convert.ToDecimal(txtOtherCost.Text);
                    decimal balance = Convert.ToDecimal(lblPreviousDue.Text);
                    decimal netamount = (total + labourCost + agentCost + PackageCost + otherCost) - discount;
                    lblNetAmount.Text = netamount.ToString();
                    decimal paid = Convert.ToDecimal(txtPaidAmount.Text);
                    decimal due = ((total + labourCost + agentCost + PackageCost + otherCost + balance) - discount) - paid;
                    lblDueAmount.Text = due.ToString();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        protected void txtPackageCost_TextChanged(object sender, EventArgs e)
        {
            if (txtPackageCost.Text != "" && txtOtherCost.Text != "" && txtDiscount.Text != "" && lblPreviousDue.Text != "" && lblDueAmount.Text != "" && txtLabourCost.Text != "" && txtAgentCost.Text != "" && txtPaidAmount.Text != "")
            {
                decimal total = Convert.ToDecimal(ViewState["sumAmount"]);
                decimal labourCost = Convert.ToDecimal(txtLabourCost.Text);
                decimal agentCost = Convert.ToDecimal(txtAgentCost.Text);
                decimal PackageCost = Convert.ToDecimal(txtPackageCost.Text);
                decimal discount = Convert.ToDecimal(txtDiscount.Text);
                decimal otherCost = Convert.ToDecimal(txtOtherCost.Text);
                decimal balance = Convert.ToDecimal(lblPreviousDue.Text);
                decimal netamount = (total + labourCost + agentCost + PackageCost + otherCost) - discount;
                lblNetAmount.Text = netamount.ToString();
                decimal paid = Convert.ToDecimal(txtPaidAmount.Text);
                decimal due = ((total + labourCost + agentCost + PackageCost + otherCost + balance) - discount) - paid;
                lblDueAmount.Text = due.ToString();
            }
        }
        protected void txtOtherCost_TextChanged(object sender, EventArgs e)
        {
            if (txtPackageCost.Text != "" && txtOtherCost.Text != "" && txtDiscount.Text != "" && lblPreviousDue.Text != "" && lblDueAmount.Text != "" && txtLabourCost.Text != "" && txtAgentCost.Text != "" && txtPaidAmount.Text != "")
            {
                decimal total = Convert.ToDecimal(ViewState["sumAmount"]);
                decimal labourCost = Convert.ToDecimal(txtLabourCost.Text);
                decimal agentCost = Convert.ToDecimal(txtAgentCost.Text);
                decimal PackageCost = Convert.ToDecimal(txtPackageCost.Text);
                decimal discount = Convert.ToDecimal(txtDiscount.Text);
                decimal otherCost = Convert.ToDecimal(txtOtherCost.Text);
                decimal balance = Convert.ToDecimal(lblPreviousDue.Text);
                decimal netamount = (total + labourCost + agentCost + PackageCost + otherCost) - discount;
                lblNetAmount.Text = netamount.ToString();
                decimal paid = Convert.ToDecimal(txtPaidAmount.Text);
                decimal due = ((total + labourCost + agentCost + PackageCost + otherCost + balance) - discount) - paid;
                lblDueAmount.Text = due.ToString();
            }
        }
        protected void txtPaidAmount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtPackageCost.Text != "" && txtOtherCost.Text != "" && txtDiscount.Text != "" && lblPreviousDue.Text != "" && lblDueAmount.Text != "" && txtLabourCost.Text != "" && txtAgentCost.Text != "" && txtPaidAmount.Text != "")
                {
                    decimal total = Convert.ToDecimal(ViewState["sumAmount"]);
                    decimal labourCost = Convert.ToDecimal(txtLabourCost.Text);
                    decimal agentCost = Convert.ToDecimal(txtAgentCost.Text);
                    decimal PackageCost = Convert.ToDecimal(txtPackageCost.Text);
                    decimal discount = Convert.ToDecimal(txtDiscount.Text);
                    decimal otherCost = Convert.ToDecimal(txtOtherCost.Text);
                    decimal balance = Convert.ToDecimal(lblPreviousDue.Text);
                    decimal netamount = (total + labourCost + agentCost + PackageCost + otherCost) - discount;
                    lblNetAmount.Text = netamount.ToString();
                    decimal paid = Convert.ToDecimal(txtPaidAmount.Text);
                    decimal due = ((total + labourCost + agentCost + PackageCost + otherCost + balance) - discount) - paid;
                    lblDueAmount.Text = due.ToString();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        protected void cmPartyName_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
                con.Open();
                cmd = new SqlCommand("Sp_Purchase", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.VarChar).Value = 9;
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                foreach (DataRow dataRow in dt.Rows)
                {
                    RadComboBoxItem item = new RadComboBoxItem();
                    item.Text = (string)dataRow["Name"].ToString();
                    item.Value = dataRow["SupplierId"].ToString();

                    string ItemCode = (string)dataRow["SupplierId"].ToString();
                    item.Attributes.Add("SupplierId", ItemCode.ToString());

                    cmPartyName.Items.Add(item);
                    item.DataBind();
                }
                con.Close();
                //ReloadMainGrid();
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
        protected void cmPartyName_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                if (cmPartyName.SelectedValue != "")
                {
                    con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
                    con.Open();
                    cmd = new SqlCommand("Sp_Purchase", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 10;
                    cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                    cmd.Parameters.Add("@SupplierId", SqlDbType.Int).Value = Convert.ToInt32(cmPartyName.SelectedValue);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    lblPreviousDue.Text = Convert.ToDecimal(dt.Rows[0]["Balance"].ToString()).ToString("0.00");
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

        protected void rgMain_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            rgMain.DataSource = dtPurchase;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
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
                cmd = new SqlCommand("Sp_Purchase", con); // insert to header table
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 3;
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                cmd.Parameters.Add("@PurId", SqlDbType.VarChar).Value = txtPurchaseNo.Text;
                cmd.Parameters.Add("@SupplierId", SqlDbType.Int).Value = Convert.ToInt32(cmPartyName.SelectedValue);
                cmd.Transaction = myTran;
                cmd.ExecuteNonQuery();

                myTran.Commit();
                con.Close();

                ButtonControl("L");
                ClearControl();
                EnableControl(false);
                lblMessage.Text = "Delete Successfull.";
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

            ReloadMainGrid();
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
                cmd.Parameters.Add("@mode", SqlDbType.VarChar).Value = 23;
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                cmd.Parameters.Add("@PurID", SqlDbType.VarChar).Value = txtPurchaseNo.Text;
                SqlDataAdapter Dap = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                Dap.Fill(dt);
                cmd.Dispose();

                string tempPath = "";
                string reportName = "";

                reportName = "PurInvoice-" + Convert.ToDateTime(dpPurchaseDate.SelectedDate).ToString("ddMMMyyyy") + "_" + cmPartyName.Text;
                AppEnv.Current.p_rptObject = "~/Reports/PurchaseInvoice.rpt";
                tempPath = @System.IO.Path.GetTempPath() + "PurchaseInvoice";

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

                    ((TextObject)AppEnv.Current.p_rptSource.ReportDefinition.Sections["Section4"].ReportObjects["txtPreviousDue"]).Text = String.Format("{0:n}", Convert.ToDecimal(ViewState["PreviousDue"]).ToString("#,##0.00"));
                    ((TextObject)AppEnv.Current.p_rptSource.ReportDefinition.Sections["Section4"].ReportObjects["txtTotalAmount"]).Text = String.Format("{0:n}", Convert.ToDecimal(ViewState["TotalAmount"]).ToString("#,##0.00"));

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