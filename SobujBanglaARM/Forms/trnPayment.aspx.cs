using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Telerik.Web.UI;

namespace SobujBanglaARM.Forms
{
    public partial class trnPayment : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        public DataTable dtItemDetails
        {
            get
            {
                object obj = this.Session["dtItemDetails"];
                if (obj != null)
                {
                    return (DataTable)obj;
                }
                DataTable dt1 = new DataTable();

                dt1.Columns.Add("rowid", typeof(Int16));
                dt1.Columns.Add("ItemCode", typeof(string));
                dt1.Columns.Add("ItemName", typeof(string));
                dt1.Columns.Add("BundleQty", typeof(decimal));
                dt1.Columns.Add("YardQty", typeof(decimal));
                dt1.Columns.Add("UnitRate", typeof(decimal));
                dt1.Columns.Add("Amount", typeof(decimal));

                dt1.PrimaryKey = new DataColumn[] { dt1.Columns["ItemCode"] };

                this.Session["dtItemDetails"] = dt1;
                return dtItemDetails;
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
            //txtACVoucherNo.Enabled = ec;
            txtBankName.Enabled = ec;
            txtChequeNo.Enabled = ec;
            //txtDepositBank.Enabled = ec;
            txtCheckDetails.Enabled = ec;
            txtRemarks.Enabled = ec;
            txtPaidAmount.Enabled = ec;
            //txtVoucherNo.Enabled = ec;
            //txtVoucherType.Enabled = ec;
            cmPayMode.Enabled = ec;
            dpCheckDate.Enabled = ec;
            dpPayDate.Enabled = ec;
        }
        public void EnableControlPartial(bool ec)
        {
            txtChequeNo.Enabled = ec;
            //txtDepositBank.Enabled = ec;
            dpCheckDate.Enabled = ec;
            txtBankName.Enabled = ec;
            txtCheckDetails.Enabled = ec;
        }
        public void ClearControl()
        {
            //txtACVoucherNo.Text = "";
            //txtDepositBank.Text = "";
            txtRemarks.Text = "";
            txtPaidAmount.Text = "";
            //txtNeedToPay.Text = "";
            //txtVoucherNo.Text = "";
            //txtVoucherType.Text = "";
            cmPayMode.Text = "";
            cmPayMode.SelectedValue = "";
            dpPayDate.SelectedDate = null;
            hfTrackId.Value = null;
        }
        public void ClearControlPartial()
        {
            txtBankName.Text = "";
            txtChequeNo.Text = "";
            txtCheckDetails.Text = "";
            dpCheckDate.SelectedDate = null;
        }
        public void ButtonControl(string bc)
        {
            //// Which button you click control according by button
            if (bc == "L")
            {
                btnNew.Enabled = true;
                btnSave.Enabled = false;
                btnSearch.Enabled = true;
                btnEdit.Enabled = false;
                btnCancel.Enabled = false;
                lblOperationMode.Text = "";
                lblMessage.Text = "";
            }
            else if (bc == "N")
            {
                btnNew.Enabled = false;
                btnSave.Enabled = true;
                btnSearch.Enabled = false;
                btnEdit.Enabled = false;
                btnCancel.Enabled = true;
                lblOperationMode.Text = "Save Mode";
                lblMessage.Text = "";
            }
            else if (bc == "F")
            {
                btnNew.Enabled = false;
                btnSave.Enabled = false;
                btnSearch.Enabled = true;
                btnEdit.Enabled = true;
                btnCancel.Enabled = true;
                lblOperationMode.Text = "";
                lblMessage.Text = "";
            }
            else if (bc == "E")
            {
                btnNew.Enabled = false;
                btnSave.Enabled = true;
                btnEdit.Enabled = false;
                btnEdit.Enabled = false;
                btnCancel.Enabled = true;
                lblOperationMode.Text = "Edit Mode";
                lblMessage.Text = "";
            }
        }
        private void SaveData()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
            con.Open();
            try
            {
                cmd = new SqlCommand("Sp_Payment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 1;
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                cmd.Parameters.Add("@SupplierId", SqlDbType.VarChar).Value = cmPartyName.SelectedValue;
                cmd.Parameters.Add("@PayDate", SqlDbType.DateTime).Value = dpPayDate.SelectedDate;
                cmd.Parameters.Add("@PayAmount", SqlDbType.Decimal).Value = txtPaidAmount.Text;
                cmd.Parameters.Add("@PayMode", SqlDbType.VarChar).Value = cmPayMode.SelectedValue;
                if (cmPayMode.SelectedValue != "Cash")
                {
                    cmd.Parameters.Add("@ChequeNo", SqlDbType.VarChar).Value = txtChequeNo.Text;
                    if (dpCheckDate.SelectedDate != null)
                        cmd.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = dpCheckDate.SelectedDate;
                    //cmd.Parameters.Add("@DepositBank", SqlDbType.VarChar).Value = txtDepositBank.Text;
                    cmd.Parameters.Add("@BankName", SqlDbType.VarChar).Value = txtBankName.Text;
                    cmd.Parameters.Add("@ChequeDetails", SqlDbType.VarChar).Value = txtCheckDetails.Text;
                }
                cmd.Parameters.Add("@Remarks", SqlDbType.VarChar).Value = txtRemarks.Text;
                cmd.Parameters.Add("@userid", SqlDbType.VarChar).Value = AppEnv.Current.p_UserName;
                if (hfTrackId.Value != "")
                    cmd.Parameters.Add("@track_id", SqlDbType.Int).Value = hfTrackId.Value;

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();

                ReloadMainGrid();
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
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
                con.Open();
                cmd = new SqlCommand("Sp_Payment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.VarChar).Value = "9";
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                cmd.Parameters.Add("@SupplierId", SqlDbType.VarChar).Value = cmPartyName.SelectedValue;
                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                adpt.Fill(ds);
                dt = ds.Tables[0];
                RadGrid1.DataSource = dt;
                RadGrid1.DataBind();
                con.Close();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        private void DataRefillForGrid()
        {
            try
            {
                GridDataItem selectedItem = (GridDataItem)RadGrid1.SelectedItems[0];
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
                con.Open();
                cmd = new SqlCommand("Sp_Payment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 11;
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                cmd.Parameters.Add("@track_id", SqlDbType.VarChar).Value = selectedItem["TrackId"].Text;
                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                adpt.Fill(ds);
                dt1 = ds.Tables[0];
                if (dt1.Rows[0]["PayMode"].ToString() == "Cash")
                {
                    EnableControlPartial(false);
                }
                else
                {
                    EnableControlPartial(true);
                }
                txtBankName.Text = dt1.Rows[0]["BankName"].ToString();
                txtChequeNo.Text = dt1.Rows[0]["ChequeNo"].ToString();
                //txtDepositBank.Text = dt1.Rows[0]["DepositBank"].ToString();
                //lblDue.Text = dt1.Rows[0]["DueAmount"].ToString();
                txtPaidAmount.Text = dt1.Rows[0]["PayAmount"].ToString();
                txtRemarks.Text = dt1.Rows[0]["Remarks"].ToString();
                cmPartyName.SelectedValue = dt1.Rows[0]["SupplierId"].ToString();
                cmPartyName.Text = dt1.Rows[0]["SupplierName"].ToString();
                cmPayMode.SelectedValue = dt1.Rows[0]["PayMode"].ToString();

                txtCheckDetails.Text = dt1.Rows[0]["ChequeDetails"].ToString();
                if (dt1.Rows[0]["ChequeDate"].ToString() != "")
                    dpCheckDate.SelectedDate = Convert.ToDateTime(dt1.Rows[0]["ChequeDate"].ToString());
                if (dt1.Rows[0]["PayDate"].ToString() != null)
                    dpPayDate.SelectedDate = Convert.ToDateTime(dt1.Rows[0]["PayDate"].ToString());

                hfTrackId.Value = dt1.Rows[0]["TrackId"].ToString();
                hfPreviousDue.Value = selectedItem["PayAmount"].Text;
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        public void PrintVoucher()
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
                cmd.Parameters.Add("@mode", SqlDbType.VarChar).Value = 27;
                cmd.Parameters.Add("@SupplierId", SqlDbType.VarChar).Value = cmPartyName.SelectedValue;
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                SqlDataAdapter Dap = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                Dap.Fill(dt);
                cmd.Dispose();

                string tempPath = "";
                string reportName = "";

                reportName = "PV-" + Convert.ToDateTime(dpPayDate.SelectedDate).ToString("ddMMMyyyy") + "_" + cmPartyName.Text;
                AppEnv.Current.p_rptObject = "~/Reports/PaymentVoucher.rpt";
                tempPath = @System.IO.Path.GetTempPath() + "PaymentVoucher";

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
                    ((TextObject)AppEnv.Current.p_rptSource.ReportDefinition.Sections["Section1"].ReportObjects["txtRemarks"]).Text = txtRemarks.Text;

                    ((TextObject)AppEnv.Current.p_rptSource.ReportDefinition.Sections["Section4"].ReportObjects["txtPreviousDue"]).Text = String.Format("{0:N}", Convert.ToDecimal(txtNeedToPay.Text));
                    ((TextObject)AppEnv.Current.p_rptSource.ReportDefinition.Sections["Section4"].ReportObjects["txtPaidAmount"]).Text = String.Format("{0:n}", Convert.ToDecimal(txtPaidAmount.Text));
                    ((TextObject)AppEnv.Current.p_rptSource.ReportDefinition.Sections["Section4"].ReportObjects["txtTotalDue"]).Text = String.Format("{0:n}", Convert.ToDecimal(lblDue.Text));


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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                EnableControl(false);
                EnableControlPartial(false);
                ClearControl();
                ButtonControl("L");
                this.dtItemDetails.Clear();
            }
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (cmPartyName.SelectedValue == "")
            //    {
            //        lblMessage.Text = "Select Supplier.";
            //    }
            //    else
            //    {
            //        EnableControl(true);
            //        ClearControl();
            //        ButtonControl("N");

            //        cmPartyName.Enabled = false;
            //        //int max = Convert.ToInt32(GetAutoNumber("invoiceno", "tblPayment"));
            //        //cmInvoiceNo.Text = max.ToString();
            //        //cmInvoiceNo.SelectedValue = max.ToString();
            //        dpPayDate.SelectedDate = System.DateTime.Now;
            //        lblOperationMode.Text = "Save Mode";
            //        lblMessage.Text = "";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    lblMessage.Text = ex.Message;
            //}
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            if (cmPartyName.SelectedValue == "")
            {
                lblMessage.Text = "Select Party Name.";
            }
            else if (cmPayMode.SelectedValue == "")
            {
                lblMessage.Text = "Select Pay Mode.";
            }
            else if (dpPayDate.SelectedDate == null)
            {
                lblMessage.Text = "Select Pay Date.";
            }
            else if (txtPaidAmount.Text == "")
            {
                lblMessage.Text = "Paid Amount can not be Blank.";
            }
            //else if (Convert.ToDecimal(txtPaidAmount.Text) == 0)
            //{
            //    lblMessage.Text = "Paid Amount can not be Zero.";
            //}
            //else if (Convert.ToDecimal(lblDue.Text) < 0)
            //{
            //    lblMessage.Text = "Paid Amount can not be greater than Need to Pay.";
            //}
            else
            {
                try
                {
                    SaveData();
                    PrintVoucher();
                    ButtonControl("L");
                    ClearControl();
                    cmPartyName.SelectedValue = "";
                    cmPartyName.Text = "";
                    EnableControl(false);
                    cmPartyName.Enabled = true;
                    txtNeedToPay.Text = "";
                    lblDue.Text = "";
                    //ReloadMainGrid();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = ex.Message;
                }
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {

        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ButtonControl("L");
            EnableControl(false);
            ClearControl();
            cmPartyName.Enabled = true;
            ClearControlPartial();
            txtNeedToPay.Text = "";
            lblDue.Text = "";
            cmPartyName.SelectedValue = "";
            cmPartyName.Text = "";
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = this.dtItemDetails;
        }
        protected void cmPayMode_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cmPayMode.SelectedValue == "Cash")
            {
                EnableControlPartial(false);
                ClearControlPartial();
            }
            else
            {
                EnableControlPartial(true);
                ClearControlPartial();
            }
        }
        protected void cmPartyName_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
                con.Open();
                cmd = new SqlCommand("Sp_Payment", con);
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
        protected void txtPaidAmount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (hfPreviousDue.Value == "")
                    hfPreviousDue.Value = "0";
                if (txtPaidAmount.Text != "" && txtNeedToPay.Text != "" && hfPreviousDue.Value != "")
                {
                    decimal NeedToPay = Convert.ToDecimal(txtNeedToPay.Text);
                    decimal PaidAmt = Convert.ToDecimal(txtPaidAmount.Text);
                    decimal PreviousDue = Convert.ToDecimal(hfPreviousDue.Value);
                    decimal Due = (NeedToPay - PaidAmt + PreviousDue);
                    lblDue.Text = Due.ToString();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        protected void txtPaid_TextChanged(object sender, EventArgs e)  // getting help
        {
            try
            {
                if (txtNeedToPay.Text != "" && txtPaidAmount.Text != "")
                {
                    decimal q = Convert.ToDecimal(txtNeedToPay.Text);
                    decimal p = Convert.ToDecimal(txtPaidAmount.Text);
                    decimal r = q - p;
                    lblDue.Text = r.ToString();
                    lblMessage.Text = "";
                    if (q < p)
                    {
                        txtPaidAmount.Text = "";
                        lblDue.Text = "";
                        lblMessage.Text = "Paid Amount must be equal or less than Need to Pay.";
                    }
                }
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
                if (cmPartyName.SelectedValue == "")
                {
                    lblMessage.Text = "Select Supplier.";
                }
                else
                {
                    EnableControl(true);
                    ClearControl();
                    ButtonControl("N");

                    dpPayDate.SelectedDate = System.DateTime.Now;
                    lblOperationMode.Text = "Save Mode";
                    lblMessage.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }

            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
                con.Open();
                cmd = new SqlCommand("Sp_Payment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 5;
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                cmd.Parameters.Add("@SupplierId", SqlDbType.VarChar).Value = cmPartyName.SelectedValue;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                txtNeedToPay.Text = dt.Rows[0]["Balance"].ToString();
                con.Close();
                txtPaidAmount.Text = "";
                lblDue.Text = "0.00";

                ReloadMainGrid();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        protected void RadGrid1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearControl();
            DataRefillForGrid();
            ButtonControl("E");
            EnableControl(true);
            cmPartyName.Enabled = false;
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
                cmd.Parameters.Add("@mode", SqlDbType.VarChar).Value = 27;
                cmd.Parameters.Add("@SupplierId", SqlDbType.VarChar).Value = cmPartyName.SelectedValue;
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                SqlDataAdapter Dap = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                Dap.Fill(dt);
                cmd.Dispose();

                string tempPath = "";
                string reportName = "";

                reportName = "PV-" + Convert.ToDateTime(dpPayDate.SelectedDate).ToString("ddMMMyyyy") + "_" + cmPartyName.Text;
                AppEnv.Current.p_rptObject = "~/Reports/PaymentVoucher.rpt";
                tempPath = @System.IO.Path.GetTempPath() + "PaymentVoucher";

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
                    ((TextObject)AppEnv.Current.p_rptSource.ReportDefinition.Sections["Section1"].ReportObjects["txtRemarks"]).Text = txtRemarks.Text;

                    ((TextObject)AppEnv.Current.p_rptSource.ReportDefinition.Sections["Section4"].ReportObjects["txtPreviousDue"]).Text = String.Format("{0:n}", (Convert.ToDecimal(txtNeedToPay.Text) + Convert.ToDecimal(txtPaidAmount.Text)));
                    ((TextObject)AppEnv.Current.p_rptSource.ReportDefinition.Sections["Section4"].ReportObjects["txtPaidAmount"]).Text = String.Format("{0:n}", Convert.ToDecimal(txtPaidAmount.Text));
                    ((TextObject)AppEnv.Current.p_rptSource.ReportDefinition.Sections["Section4"].ReportObjects["txtTotalDue"]).Text = String.Format("{0:N}", Convert.ToDecimal(txtNeedToPay.Text));


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
    }
}