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
    public partial class RatioCalculation : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        public DataTable dtItemDetailsRatio
        {
            get
            {
                object obj = this.Session["dtItemDetailsRatio"];
                if (obj != null)
                {
                    return (DataTable)obj;
                }
                DataTable dt1 = new DataTable();
                dt1.Columns.Add("rowid", typeof(Int32));
                dt1.Columns.Add("SupplierId", typeof(string));
                dt1.Columns.Add("SupplierName", typeof(string));
                dt1.Columns.Add("ItemCode", typeof(string));
                dt1.Columns.Add("ItemName", typeof(string));
                dt1.Columns.Add("TotalSacks", typeof(decimal));
                dt1.Columns.Add("TotalMon", typeof(decimal));
                dt1.Columns.Add("UnitPrice", typeof(decimal));
                dt1.Columns.Add("Amount", typeof(decimal));
                dt1.PrimaryKey = new DataColumn[] { dt1.Columns["rowid"] };
                this.Session["dtItemDetailsRatio"] = dt1;
                return dtItemDetailsRatio;
            }
        }
        public DataTable dtRatioCalculation
        {
            get
            {
                object obj = this.Session["dtRatioCalculation"];
                if (obj != null)
                {
                    return (DataTable)obj;
                }
                DataTable dt1 = new DataTable();
                dt1.Columns.Add("rowid", typeof(Int32));
                dt1.Columns.Add("RatioId", typeof(string));
                dt1.Columns.Add("CrashingDate", typeof(DateTime));
                dt1.Columns.Add("PurchaseCarryingCost", typeof(decimal));
                dt1.Columns.Add("MillCrashingCost", typeof(decimal));
                dt1.Columns.Add("SalesCarryingCost", typeof(decimal));
                dt1.Columns.Add("EmptySackCost", typeof(decimal));
                dt1.Columns.Add("OtherCost", typeof(decimal));
                dt1.Columns.Add("TotalCost", typeof(decimal));
                dt1.Columns.Add("Remarks", typeof(string));
                dt1.PrimaryKey = new DataColumn[] { dt1.Columns["rowid"] };
                this.Session["dtRatioCalculation"] = dt1;
                return dtRatioCalculation;
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

        public void ClearControl()
        {
            txtAmount.Text = "";
            txtQty.Text = "";
            txtRemarks.Text = "";
            txtUnitPrice.Text = "";
            cmItemName.Text = "";
            cmItemName.SelectedValue = "";
            cmPartyName.Text = "";
            cmPartyName.SelectedValue = "";

            txtCrashingCost.Text = "0.00";
            txtEmptySackCost.Text = "0.00";
            txtSackSewingCost.Text = "0.00";
            txtSutliCost.Text = "0.00";
            txtKuliCost.Text = "0.00";
            txtKayelCost.Text = "0.00";
            txtTrolleyCost.Text = "0.00";
            txtChadaCost.Text = "0.00";
            txtKhajnaCost.Text = "0.00";
            txtOtherCost.Text = "0.00";
            txtPurchaseCarryingCost.Text = "0.00";
            txtSalesCarryingCost.Text = "0.00";
            txtTotalSack.Text = "0.00";
            txtTotalProducedQty.Text = "0.00";
            txtCostPerSack.Text = "0.00";
            txtProducedPerUnit.Text = "0.00";
            lblTotalCost.Text = "0.00";
            lblMessage.Text = "0.00";
        }
        public void EnableControl(bool ec)
        {
            txtQty.Enabled = ec;
            txtRemarks.Enabled = ec;
            txtUnitPrice.Enabled = ec;
            cmItemName.Enabled = ec;
            cmPartyName.Enabled = ec;
            dpCrashingDate.Enabled = ec;
            txtAmount.Enabled = ec;
            txtCrashingCost.Enabled = ec;
            txtPurchaseCarryingCost.Enabled = ec;
            txtTotalSack.Enabled = ec;
        }

        public void ClearControlPartial()
        {
            cmPartyName.SelectedValue = "";
            cmPartyName.Text = "";
            cmItemName.SelectedValue = "";
            cmItemName.Text = "";
            txtTotalSack.Text = "";
            txtQty.Text = "";
            txtUnitPrice.Text = "";
            txtAmount.Text = "";
        }
        public void EnableControlPartial(bool ec)
        {

        }

        public void ClearControlProduction()
        {

        }
        public void EnableControlProduction(bool ec)
        {
            txtTotalProducedQty.Enabled = ec;
            txtEmptySackCost.Enabled = ec;
            txtSalesCarryingCost.Enabled = ec;
            txtOtherCost.Enabled = ec;
            txtSackSewingCost.Enabled = ec;
            txtSutliCost.Enabled = ec;
            txtKuliCost.Enabled = ec;
            txtKayelCost.Enabled = ec;
            txtTrolleyCost.Enabled = ec;
            txtChadaCost.Enabled = ec;
            txtKhajnaCost.Enabled = ec;
            txtOtherCost.Enabled = ec;
            //txtCostPerSack.Enabled = ec;
            //txtProducedPerUnit.Enabled = ec;
        }

        public void ButtonControl(string bc)
        {
            //// Which button you click control according by button
            if (bc == "L")
            {
                btnNew.Enabled = true;
                btnSave.Enabled = false;
                btnUpdateProduction.Enabled = false;
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
                btnUpdateProduction.Enabled = false;
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
                btnUpdateProduction.Enabled = false;
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
                btnUpdateProduction.Enabled = true;
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
                cmd = new SqlCommand("Sp_LotWiseProduction", con); // insert to header table
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 1;
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                cmd.Parameters.Add("@RatioId", SqlDbType.VarChar).Value = txtLotNo.Text;
                cmd.Parameters.Add("@CrashingDate", SqlDbType.DateTime).Value = dpCrashingDate.SelectedDate;
                cmd.Parameters.Add("@PurchaseCarryingCost", SqlDbType.Decimal).Value = Convert.ToDecimal(txtPurchaseCarryingCost.Text);
                cmd.Parameters.Add("@MillCrashingCost", SqlDbType.Decimal).Value = Convert.ToDecimal(txtCrashingCost.Text);
                cmd.Parameters.Add("@TotalCost", SqlDbType.Decimal).Value = Convert.ToDecimal(lblTotalCost.Text);
                cmd.Parameters.Add("@Userid", SqlDbType.NVarChar).Value = AppEnv.Current.p_UserName.ToString();
                cmd.Transaction = myTran;
                cmd.ExecuteNonQuery();

                if (dtItemDetailsRatio.Rows.Count > 0)
                {
                    for (int i = 0; i < dtItemDetailsRatio.Rows.Count; i++)
                    {
                        cmd = new SqlCommand("Sp_LotWiseProduction", con); // insert to details table
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 11;
                        cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                        cmd.Parameters.Add("@RatioId", SqlDbType.Int).Value = Convert.ToInt32(txtLotNo.Text);
                        cmd.Parameters.Add("@SupplierId", SqlDbType.Int).Value = dtItemDetailsRatio.Rows[i]["SupplierId"].ToString();
                        cmd.Parameters.Add("@ItemCode", SqlDbType.Int).Value = dtItemDetailsRatio.Rows[i]["ItemCode"].ToString();
                        cmd.Parameters.Add("@TotalSacks", SqlDbType.Decimal).Value = dtItemDetailsRatio.Rows[i]["TotalSacks"].ToString();
                        cmd.Parameters.Add("@TotalMon", SqlDbType.Decimal).Value = dtItemDetailsRatio.Rows[i]["TotalMon"].ToString();
                        cmd.Parameters.Add("@UnitPrice", SqlDbType.Decimal).Value = dtItemDetailsRatio.Rows[i]["UnitPrice"].ToString();
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = dtItemDetailsRatio.Rows[i]["Amount"].ToString();
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
                dtItemDetailsRatio.Clear();
                RadGrid1.Rebind();
                lblMessage.Text = "Data Saved Successfully.";
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
        private void UpdateProducedData()
        {

            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
                con.Open();
                cmd = new SqlCommand("Sp_LotWiseProduction", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 6; // update produced products and others
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                cmd.Parameters.Add("@TotalProducedQty", SqlDbType.Decimal).Value = Convert.ToDecimal(txtTotalProducedQty.Text);
                cmd.Parameters.Add("@EmptySackCost", SqlDbType.Decimal).Value = Convert.ToDecimal(txtEmptySackCost.Text);
                cmd.Parameters.Add("@SalesCarryingCost", SqlDbType.Decimal).Value = Convert.ToDecimal(txtSalesCarryingCost.Text);
                cmd.Parameters.Add("@SackSewingCost", SqlDbType.Decimal).Value = Convert.ToDecimal(txtSackSewingCost.Text);
                cmd.Parameters.Add("@SutliCost", SqlDbType.Decimal).Value = Convert.ToDecimal(txtSutliCost.Text);
                cmd.Parameters.Add("@KuliCost", SqlDbType.Decimal).Value = Convert.ToDecimal(txtKuliCost.Text);
                cmd.Parameters.Add("@KayelCost", SqlDbType.Decimal).Value = Convert.ToDecimal(txtKayelCost.Text);
                cmd.Parameters.Add("@TrolleyCost", SqlDbType.Decimal).Value = Convert.ToDecimal(txtTrolleyCost.Text);
                cmd.Parameters.Add("@ChadaCost", SqlDbType.Decimal).Value = Convert.ToDecimal(txtChadaCost.Text);
                cmd.Parameters.Add("@KhajnaCost", SqlDbType.Decimal).Value = Convert.ToDecimal(txtKhajnaCost.Text);
                cmd.Parameters.Add("@OtherCost", SqlDbType.Decimal).Value = Convert.ToDecimal(txtOtherCost.Text);
                cmd.Parameters.Add("@CostPerSack", SqlDbType.Decimal).Value = Convert.ToDecimal(txtCostPerSack.Text);
                cmd.Parameters.Add("@ProducedPerUnit", SqlDbType.Decimal).Value = Convert.ToDecimal(txtProducedPerUnit.Text);
                cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = txtRemarks.Text;
                cmd.Parameters.Add("@TotalCost", SqlDbType.Decimal).Value = Convert.ToDecimal(lblTotalCost.Text);
                cmd.Parameters.Add("@RatioId", SqlDbType.VarChar).Value = txtLotNo.Text;
                cmd.ExecuteNonQuery();
                con.Close();

                ButtonControl("L");
                ClearControlProduction();
                EnableControlProduction(false);
                lblMessage.Text = "Data Updated Successfully.";
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message.ToString();
            }
        }
        private void ReloadMainGrid()
        {
            try
            {
                dtRatioCalculation.Clear();
                rgMain.DataSource = null;
                rgMain.Rebind();

                con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
                con.Open();
                cmd = new SqlCommand("Sp_LotWiseProduction", con);
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
                    DataRow newRow = this.dtRatioCalculation.NewRow();
                    newRow["rowid"] = this.dtRatioCalculation.Rows.Count + 1;
                    newRow["RatioId"] = dt.Rows[i]["RatioId"].ToString();
                    if (dt.Rows[i]["CrashingDate"].ToString() != "")
                        newRow["CrashingDate"] = dt.Rows[i]["CrashingDate"].ToString();
                    newRow["PurchaseCarryingCost"] = dt.Rows[i]["PurchaseCarryingCost"].ToString();
                    newRow["MillCrashingCost"] = dt.Rows[i]["MillCrashingCost"].ToString();
                    newRow["SalesCarryingCost"] = dt.Rows[i]["SalesCarryingCost"].ToString();
                    newRow["EmptySackCost"] = dt.Rows[i]["EmptySackCost"].ToString();
                    newRow["OtherCost"] = dt.Rows[i]["OtherCost"].ToString();
                    newRow["TotalCost"] = dt.Rows[i]["TotalCost"].ToString();
                    newRow["Remarks"] = dt.Rows[i]["Remarks"].ToString();
                    this.dtRatioCalculation.Rows.Add(newRow);
                    this.dtRatioCalculation.AcceptChanges();
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
                dtItemDetailsRatio.Clear();
                RadGrid1.DataSource = null;
                RadGrid1.Rebind();

                GridDataItem selectedItem = (GridDataItem)rgMain.SelectedItems[0];

                con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
                con.Open();
                cmd = new SqlCommand("Sp_LotWiseProduction", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 5;
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                cmd.Parameters.Add("@RatioId", SqlDbType.VarChar).Value = selectedItem["RatioId"].Text;
                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                adpt.Fill(ds);
                dt1 = ds.Tables[0];
                dt2 = ds.Tables[1];

                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    DataRow newRow = this.dtItemDetailsRatio.NewRow();
                    newRow["rowid"] = this.dtItemDetailsRatio.Rows.Count + 1;
                    newRow["SupplierId"] = dt2.Rows[i]["SupplierId"].ToString();
                    newRow["SupplierName"] = dt2.Rows[i]["SupplierName"].ToString();
                    newRow["ItemCode"] = dt2.Rows[i]["ItemCode"].ToString();
                    newRow["ItemName"] = dt2.Rows[i]["ItemName"].ToString();
                    newRow["TotalSacks"] = dt2.Rows[i]["TotalSacks"].ToString();
                    newRow["TotalMon"] = dt2.Rows[i]["TotalMon"].ToString();
                    newRow["UnitPrice"] = dt2.Rows[i]["UnitPrice"].ToString();
                    newRow["Amount"] = dt2.Rows[i]["Amount"].ToString();
                    this.dtItemDetailsRatio.Rows.Add(newRow);
                    this.dtItemDetailsRatio.AcceptChanges();
                }
                RadGrid1.Rebind();

                txtLotNo.Text = dt1.Rows[0]["RatioId"].ToString();
                if (dt1.Rows[0]["CrashingDate"].ToString() != "")
                    dpCrashingDate.SelectedDate = Convert.ToDateTime(dt1.Rows[0]["CrashingDate"].ToString());
                txtPurchaseCarryingCost.Text = dt1.Rows[0]["PurchaseCarryingCost"].ToString();
                txtCrashingCost.Text = dt1.Rows[0]["MillCrashingCost"].ToString();
                lblTotalCost.Text = dt1.Rows[0]["TotalCost"].ToString();

                txtTotalProducedQty.Text = dt1.Rows[0]["TotalProducedQty"].ToString();
                txtEmptySackCost.Text = dt1.Rows[0]["EmptySackCost"].ToString();
                txtSalesCarryingCost.Text = dt1.Rows[0]["SalesCarryingCost"].ToString();
                txtSackSewingCost.Text = dt1.Rows[0]["SackSewingCost"].ToString();
                txtSutliCost.Text = dt1.Rows[0]["SutliCost"].ToString();
                txtKuliCost.Text = dt1.Rows[0]["KuliCost"].ToString();
                txtKayelCost.Text = dt1.Rows[0]["KayelCost"].ToString();
                txtTrolleyCost.Text = dt1.Rows[0]["TrolleyCost"].ToString();
                txtChadaCost.Text = dt1.Rows[0]["ChadaCost"].ToString();
                txtKhajnaCost.Text = dt1.Rows[0]["KhajnaCost"].ToString();
                txtOtherCost.Text = dt1.Rows[0]["OtherCost"].ToString();
                txtCostPerSack.Text = dt1.Rows[0]["CostPerSack"].ToString();
                txtProducedPerUnit.Text = dt1.Rows[0]["ProducedPerUnit"].ToString();
                txtRemarks.Text = dt1.Rows[0]["Remarks"].ToString();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                EnableControl(false);
                EnableControlProduction(false);
                ClearControl();
                ClearControlPartial();
                ButtonControl("L");
                this.dtItemDetailsRatio.Clear();
                ReloadMainGrid();
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                int max = Convert.ToInt32(GetAutoNumber("RatioId", "tblRatioCalculationHDR"));
                txtLotNo.Text = max.ToString();
                EnableControl(true);
                ClearControl();
                ClearControlPartial();
                ButtonControl("N");
                dpCrashingDate.SelectedDate = System.DateTime.Now;
                dtItemDetailsRatio.Clear();
                RadGrid1.Rebind();
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
                if (txtLotNo.Text == "")
                {
                    lblMessage.Text = "Lot No can not be blank.";
                }
                else if (dpCrashingDate.SelectedDate == null)
                {
                    lblMessage.Text = "Select Crashing Date.";
                }
                else if (dtItemDetailsRatio.Rows.Count <= 0)
                {
                    lblMessage.Text = "Add Item.";
                }
                else
                {
                    if (lblOperationMode.Text == "Save Mode")
                    {
                        SaveData();
                    }
                    else if (lblOperationMode.Text == "Edit Mode")
                    {

                    }
                }
                ReloadMainGrid();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        protected void btnUpdateProduction_Click(object sender, EventArgs e)
        {
            if (txtCostPerSack.Text == "0")
            {
                lblMessage.Text = "Cost/Sack can not be 0.00";
            }
            else if (txtProducedPerUnit.Text == "0")
            {
                lblMessage.Text = "Produced Per Unit can not be 0.00";
            }
            else if (txtTotalProducedQty.Text == "0")
            {
                lblMessage.Text = "Total Produced Qty can not be 0.00";
            }
            else
            {
                UpdateProducedData();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtLotNo.Text == "")
            {
                lblMessage.Text = "Lot No is Blank!";
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
            dtItemDetailsRatio.Clear();
            RadGrid1.Rebind();
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
                cmd = new SqlCommand("Sp_LotWiseProduction", con); // insert to header table
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 3;
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                cmd.Parameters.Add("@RatioId", SqlDbType.VarChar).Value = txtLotNo.Text;
                cmd.Transaction = myTran;
                cmd.ExecuteNonQuery();

                myTran.Commit();
                con.Close();

                ButtonControl("L");
                ClearControl();
                EnableControl(false);
                ReloadMainGrid();
                dtItemDetailsRatio.Clear();
                RadGrid1.Rebind();
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
        }
        protected void btnPrintPreview_Click(object sender, EventArgs e)
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
                cmd.Parameters.Add("@mode", SqlDbType.VarChar).Value = 24;
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                cmd.Parameters.Add("@RatioId", SqlDbType.VarChar).Value = txtLotNo.Text;
                SqlDataAdapter Dap = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                Dap.Fill(dt);
                cmd.Dispose();

                string tempPath = "";
                string reportName = "";

                reportName = "LotWiseProduction";
                AppEnv.Current.p_rptObject = "~/Reports/LotWiseProduction.rpt";
                tempPath = @System.IO.Path.GetTempPath() + "LotWiseProduction";

                AppEnv.Current.p_rptSource.Load(Server.MapPath(AppEnv.Current.p_rptObject.ToString()));
                AppEnv.Current.p_rptSource.SetDataSource(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    ((TextObject)AppEnv.Current.p_rptSource.ReportDefinition.Sections["Section1"].ReportObjects["txtCompanyName"]).Text = Session["Name"].ToString();
                    ((TextObject)AppEnv.Current.p_rptSource.ReportDefinition.Sections["Section1"].ReportObjects["txtAddress"]).Text = Session["Address"].ToString();
                    ((TextObject)AppEnv.Current.p_rptSource.ReportDefinition.Sections["Section1"].ReportObjects["txtContact"]).Text = Session["Contact1"].ToString();

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

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (cmItemName.Text == "")
            {
                lblMessage.Text = "Select Item.";
            }
            else if (cmPartyName.SelectedValue == "")
            {
                lblMessage.Text = "Select Supplier.";
            }
            else if (txtAmount.Text == "")
            {
                lblMessage.Text = "Amount can not be Blank.";
            }
            else if (txtQty.Text == "" || Convert.ToDecimal(txtQty.Text) <= 0)
            {
                lblMessage.Text = "Qty can not be Blank or Zero.";
            }
            else if (txtUnitPrice.Text == "" || Convert.ToDecimal(txtUnitPrice.Text) <= 0)
            {
                lblMessage.Text = "Unit Price can not be Blank.";
            }
            else
            {
                try
                {
                    if (cmItemName.SelectedValue != "")
                    {
                        DataRow newRow = this.dtItemDetailsRatio.NewRow();
                        newRow["rowid"] = this.dtItemDetailsRatio.Rows.Count + 1;
                        newRow["SupplierId"] = cmPartyName.SelectedValue;
                        newRow["SupplierName"] = cmPartyName.Text;
                        newRow["ItemCode"] = cmItemName.SelectedValue;
                        newRow["ItemName"] = cmItemName.Text;
                        newRow["TotalSacks"] = Convert.ToDecimal(txtTotalSack.Text);
                        newRow["TotalMon"] = Convert.ToDecimal(txtQty.Text);
                        newRow["UnitPrice"] = Convert.ToDecimal(txtUnitPrice.Text);
                        newRow["Amount"] = Convert.ToDecimal(txtAmount.Text);
                        this.dtItemDetailsRatio.Rows.Add(newRow);
                        this.dtItemDetailsRatio.AcceptChanges();
                        RadGrid1.Rebind();
                        ClearControlPartial();
                        lblMessage.Text = "";

                        CostCalculation();
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = ex.Message;
                }
            }
        }

        public void CostCalculation()
        {
            if (txtPurchaseCarryingCost.Text != "" && txtOtherCost.Text != "" && txtCrashingCost.Text != "" && txtSalesCarryingCost.Text != "" && txtSackSewingCost.Text != "" && txtSutliCost.Text != "" && txtKuliCost.Text != "" && txtKayelCost.Text != "" && txtTrolleyCost.Text != "" && txtChadaCost.Text != "" && txtKhajnaCost.Text != "" && txtOtherCost.Text != "" && txtEmptySackCost.Text != "")
            {
                decimal total = Convert.ToDecimal(ViewState["sumAmount"]);
                decimal PurchaseCarryingCost = Convert.ToDecimal(txtPurchaseCarryingCost.Text);
                decimal CrashingCost = Convert.ToDecimal(txtCrashingCost.Text);
                decimal SalesCarryingCost = Convert.ToDecimal(txtSalesCarryingCost.Text);
                decimal SackSewingCost = Convert.ToDecimal(txtSackSewingCost.Text);
                decimal SutliCost = Convert.ToDecimal(txtSutliCost.Text);
                decimal KuliCost = Convert.ToDecimal(txtKuliCost.Text);
                decimal KayelCost = Convert.ToDecimal(txtKayelCost.Text);
                decimal TrolleyCost = Convert.ToDecimal(txtTrolleyCost.Text);
                decimal ChadaCost = Convert.ToDecimal(txtChadaCost.Text);
                decimal KhajnaCost = Convert.ToDecimal(txtKhajnaCost.Text);
                decimal OtherCost = Convert.ToDecimal(txtOtherCost.Text);
                decimal EmptySackCost = Convert.ToDecimal(txtEmptySackCost.Text);
                decimal TotalCost = (total + PurchaseCarryingCost + CrashingCost + SalesCarryingCost + SackSewingCost + SutliCost + KuliCost + KayelCost + TrolleyCost + ChadaCost + KhajnaCost + OtherCost + EmptySackCost);
                lblTotalCost.Text = TotalCost.ToString();
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
                cmd = new SqlCommand("select a.PurRate, a.SalesRate,a.ItemUnit,b.Unit from tblItems a left join tblWeights b on a.ItemUnit=b.WgtId where ItemCode='" + cmItemName.SelectedValue + "'", con);
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    txtUnitPrice.Text = dr["PurRate"].ToString();
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

        decimal sumQty = 0;
        decimal sumSack = 0;
        decimal sumAmount = 0;
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = this.dtItemDetailsRatio;
        }
        protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    sumSack += Convert.ToDecimal(dataItem["TotalSacks"].Text);
                    sumQty += Convert.ToDecimal(dataItem["TotalMon"].Text);
                    sumAmount += Convert.ToDecimal(dataItem["Amount"].Text);
                    ViewState["sumSack"] = sumSack;
                    ViewState["sumQty"] = sumQty;
                    ViewState["sumAmount"] = sumAmount;
                }
                else if (e.Item is GridFooterItem)
                {
                    GridFooterItem footerItem = e.Item as GridFooterItem;
                    footerItem["TotalSacks"].Text = sumSack.ToString();
                    footerItem["TotalMon"].Text = sumQty.ToString();
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
                    DataRow[] Drow = this.dtItemDetailsRatio.Select("rowid='" + RID + "'");
                    if (Drow.Length > 0)
                    {
                        Drow[0].Delete();
                        this.dtItemDetailsRatio.AcceptChanges();
                        RadGrid1.Rebind();
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

        protected void rgMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ClearControl();
                ClearControlPartial();

                DataRefill();
                ButtonControl("E");
                EnableControl(true);
                EnableControlProduction(true);
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
        protected void rgMain_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            rgMain.DataSource = dtRatioCalculation;
        }

        protected void cmPartyName_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {

        }

        protected void txtPurchaseCarryingCost_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //if (txtPurchaseCarryingCost.Text != "" && txtOtherCost.Text != "" && txtCrashingCost.Text != "" && txtSalesCarryingCost.Text != "" && txtOtherCost.Text != "" && txtEmptySackCost.Text != "")
                //{
                //    decimal total = Convert.ToDecimal(ViewState["sumAmount"]);
                //    decimal PurchaseCarryingCost = Convert.ToDecimal(txtPurchaseCarryingCost.Text);
                //    decimal CrashingCost = Convert.ToDecimal(txtCrashingCost.Text);

                //    decimal EmptySackCost = Convert.ToDecimal(txtEmptySackCost.Text);
                //    decimal SalesCarryingCost = Convert.ToDecimal(txtSalesCarryingCost.Text);
                //    decimal OtherCost = Convert.ToDecimal(txtOtherCost.Text);
                //    decimal TotalCost = (total + PurchaseCarryingCost + CrashingCost + SalesCarryingCost + OtherCost + EmptySackCost);
                //    lblTotalCost.Text = TotalCost.ToString();
                //    txtCostPerSack.Text = "0.00";
                //}
                CostCalculation();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        protected void txtCrashingCost_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //if (txtPurchaseCarryingCost.Text != "" && txtOtherCost.Text != "" && txtCrashingCost.Text != "" && txtSalesCarryingCost.Text != "" && txtOtherCost.Text != "" && txtEmptySackCost.Text != "")
                //{
                //    decimal total = Convert.ToDecimal(ViewState["sumAmount"]);
                //    decimal PurchaseCarryingCost = Convert.ToDecimal(txtPurchaseCarryingCost.Text);
                //    decimal CrashingCost = Convert.ToDecimal(txtCrashingCost.Text);
                //    decimal SalesCarryingCost = Convert.ToDecimal(txtSalesCarryingCost.Text);
                //    decimal OtherCost = Convert.ToDecimal(txtOtherCost.Text);
                //    decimal EmptySackCost = Convert.ToDecimal(txtEmptySackCost.Text);
                //    decimal TotalCost = (total + PurchaseCarryingCost + CrashingCost + SalesCarryingCost + OtherCost + EmptySackCost);
                //    lblTotalCost.Text = TotalCost.ToString();
                //    txtCostPerSack.Text = "0.00";
                //}
                CostCalculation();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        protected void txtSalesCarryingCost_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //if (txtPurchaseCarryingCost.Text != "" && txtOtherCost.Text != "" && txtCrashingCost.Text != "" && txtSalesCarryingCost.Text != "" && txtOtherCost.Text != "" && txtEmptySackCost.Text != "")
                //{
                //    decimal total = Convert.ToDecimal(ViewState["sumAmount"]);
                //    decimal PurchaseCarryingCost = Convert.ToDecimal(txtPurchaseCarryingCost.Text);
                //    decimal CrashingCost = Convert.ToDecimal(txtCrashingCost.Text);
                //    decimal SalesCarryingCost = Convert.ToDecimal(txtSalesCarryingCost.Text);
                //    decimal OtherCost = Convert.ToDecimal(txtOtherCost.Text);
                //    decimal EmptySackCost = Convert.ToDecimal(txtEmptySackCost.Text);
                //    decimal TotalCost = (total + PurchaseCarryingCost + CrashingCost + SalesCarryingCost + OtherCost + EmptySackCost);
                //    lblTotalCost.Text = TotalCost.ToString();
                //    txtCostPerSack.Text = "0.00";
                //}
                CostCalculation();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        protected void txtEmptySackCost_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //if (txtPurchaseCarryingCost.Text != "" && txtOtherCost.Text != "" && txtCrashingCost.Text != "" && txtSalesCarryingCost.Text != "" && txtOtherCost.Text != "" && txtEmptySackCost.Text != "")
                //{
                //    decimal total = Convert.ToDecimal(ViewState["sumAmount"]);
                //    decimal PurchaseCarryingCost = Convert.ToDecimal(txtPurchaseCarryingCost.Text);
                //    decimal CrashingCost = Convert.ToDecimal(txtCrashingCost.Text);
                //    decimal SalesCarryingCost = Convert.ToDecimal(txtSalesCarryingCost.Text);
                //    decimal OtherCost = Convert.ToDecimal(txtOtherCost.Text);
                //    decimal EmptySackCost = Convert.ToDecimal(txtEmptySackCost.Text);
                //    decimal TotalCost = (total + PurchaseCarryingCost + CrashingCost + SalesCarryingCost + OtherCost + EmptySackCost);
                //    lblTotalCost.Text = TotalCost.ToString();
                //    txtCostPerSack.Text = "0.00";
                //}
                CostCalculation();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        protected void txtSackSewingCost_TextChanged(object sender, EventArgs e)
        {
            CostCalculation();
        }
        protected void txtSutliCost_TextChanged(object sender, EventArgs e)
        {
            CostCalculation();
        }
        protected void txtKuliCost_TextChanged(object sender, EventArgs e)
        {
            CostCalculation();
        }
        protected void txtKayelCost_TextChanged(object sender, EventArgs e)
        {
            CostCalculation();
        }
        protected void txtTrolleyCost_TextChanged(object sender, EventArgs e)
        {
            CostCalculation();
        }
        protected void txtChadaCost_TextChanged(object sender, EventArgs e)
        {
            CostCalculation();
        }
        protected void txtKhajnaCost_TextChanged(object sender, EventArgs e)
        {
            CostCalculation();
        }
        protected void txtOtherCost_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //if (txtPurchaseCarryingCost.Text != "" && txtOtherCost.Text != "" && txtCrashingCost.Text != "" && txtSalesCarryingCost.Text != "" && txtOtherCost.Text != "" && txtEmptySackCost.Text != "")
                //{
                //    decimal total = Convert.ToDecimal(ViewState["sumAmount"]);
                //    decimal PurchaseCarryingCost = Convert.ToDecimal(txtPurchaseCarryingCost.Text);
                //    decimal CrashingCost = Convert.ToDecimal(txtCrashingCost.Text);
                //    decimal SalesCarryingCost = Convert.ToDecimal(txtSalesCarryingCost.Text);
                //    decimal OtherCost = Convert.ToDecimal(txtOtherCost.Text);
                //    decimal EmptySackCost = Convert.ToDecimal(txtEmptySackCost.Text);
                //    decimal TotalCost = (total + PurchaseCarryingCost + CrashingCost + SalesCarryingCost + OtherCost + EmptySackCost);
                //    lblTotalCost.Text = TotalCost.ToString();
                //    txtCostPerSack.Text = "0.00";
                //}
                CostCalculation();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }


        protected void txtTotalProducedQty_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtTotalProducedQty.Text != "")
                {
                    decimal TotalCost = Convert.ToDecimal(lblTotalCost.Text);
                    decimal TotalProducedQty = Convert.ToDecimal(txtTotalProducedQty.Text);
                    txtProducedPerUnit.Text = ((TotalProducedQty * 50) / Convert.ToDecimal(ViewState["sumQty"])).ToString();
                    txtCostPerSack.Text = (TotalCost / TotalProducedQty).ToString();
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        protected void txtCostPerSack_TextChanged(object sender, EventArgs e)
        {

        }
        protected void txtProducedPerUnit_TextChanged(object sender, EventArgs e)
        {
        }

    }
}