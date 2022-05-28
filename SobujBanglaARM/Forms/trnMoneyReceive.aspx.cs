using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Telerik.Web.UI;

namespace SobujBanglaARM.Forms
{
    public partial class trnMoneyReceive : System.Web.UI.Page
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
            txtReceiveAmt.Enabled = ec;
            //txtVoucherNo.Enabled = ec;
            //txtVoucherType.Enabled = ec;
            cmReceiveMode.Enabled = ec;
            dpCheckDate.Enabled = ec;
            dpReceiveDate.Enabled = ec;
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
            txtReceiveAmt.Text = "";
            //txtNeedToReceive.Text = "";
            //txtVoucherNo.Text = "";
            //txtVoucherType.Text = "";
            cmReceiveMode.Text = "";
            cmReceiveMode.SelectedValue = "";
            dpReceiveDate.SelectedDate = null;
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
                cmd = new SqlCommand("Sp_MoneyReceived", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 1;
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                cmd.Parameters.Add("@custcode", SqlDbType.VarChar).Value = cmPartyName.SelectedValue;
                cmd.Parameters.Add("@receivedmode", SqlDbType.VarChar).Value = cmReceiveMode.SelectedValue;
                cmd.Parameters.Add("@receiveddate", SqlDbType.DateTime).Value = dpReceiveDate.SelectedDate;
                cmd.Parameters.Add("@ReceiveAmount", SqlDbType.Decimal).Value = txtReceiveAmt.Text;
                cmd.Parameters.Add("@PayMode", SqlDbType.VarChar).Value = cmReceiveMode.SelectedValue;
                if (cmReceiveMode.SelectedValue != "Cash")
                {
                    cmd.Parameters.Add("@cheqno", SqlDbType.VarChar).Value = txtChequeNo.Text;
                    if (dpCheckDate.SelectedDate != null)
                        cmd.Parameters.Add("@cheqdate", SqlDbType.DateTime).Value = dpCheckDate.SelectedDate;
                    cmd.Parameters.Add("@BankName", SqlDbType.VarChar).Value = txtBankName.Text;
                    cmd.Parameters.Add("@ChequeDetails", SqlDbType.VarChar).Value = txtCheckDetails.Text;
                }
                cmd.Parameters.Add("@Remarks", SqlDbType.VarChar).Value = txtRemarks.Text;
                cmd.Parameters.Add("@userid", SqlDbType.VarChar).Value = AppEnv.Current.p_UserName;
                if(hfTrackId.Value!="")
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
                cmd = new SqlCommand("Sp_MoneyReceived", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.VarChar).Value = "9";
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                cmd.Parameters.Add("@custcode", SqlDbType.VarChar).Value = cmPartyName.SelectedValue;
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
            GridDataItem selectedItem = (GridDataItem)RadGrid1.SelectedItems[0];
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
            con.Open();
            cmd = new SqlCommand("Sp_MoneyReceived", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 11;
            cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
            cmd.Parameters.Add("@track_id", SqlDbType.VarChar).Value = selectedItem["track_id"].Text;
            SqlDataAdapter adpt = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            adpt.Fill(ds);
            dt1 = ds.Tables[0];

            txtBankName.Text = dt1.Rows[0]["BankName"].ToString();
            txtCheckDetails.Text = dt1.Rows[0]["ChequeDetails"].ToString();
            txtChequeNo.Text = dt1.Rows[0]["ChequeNo"].ToString();
            txtReceiveAmt.Text = dt1.Rows[0]["ReceiveAmount"].ToString();
            txtRemarks.Text = dt1.Rows[0]["Remarks"].ToString();
            cmPartyName.SelectedValue = dt1.Rows[0]["CustId"].ToString();
            cmPartyName.Text = dt1.Rows[0]["CustomerName"].ToString();
            cmReceiveMode.SelectedValue = dt1.Rows[0]["PayMode"].ToString();
            if(dt1.Rows[0]["ChequeDt"].ToString()!="")
                dpCheckDate.SelectedDate = Convert.ToDateTime(dt1.Rows[0]["ChequeDt"].ToString());
            if (dt1.Rows[0]["ReceivedDate"].ToString() != null)
                dpReceiveDate.SelectedDate = Convert.ToDateTime(dt1.Rows[0]["ReceivedDate"].ToString());
            hfTrackId.Value = dt1.Rows[0]["track_id"].ToString();
            hfPreviousDue.Value = selectedItem["ReceiveAmount"].Text;
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
                cmd.Parameters.Add("@mode", SqlDbType.VarChar).Value = 28;
                cmd.Parameters.Add("@CustomerId", SqlDbType.VarChar).Value = cmPartyName.SelectedValue;
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                SqlDataAdapter Dap = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                Dap.Fill(dt);
                cmd.Dispose();

                string tempPath = "";
                string reportName = "";

                reportName = "RV_" + Convert.ToDateTime(dpReceiveDate.SelectedDate).ToString("ddMMMyyyy")+"_" + cmPartyName.Text;
                AppEnv.Current.p_rptObject = "~/Reports/ReceiveVoucher.rpt";
                tempPath = @System.IO.Path.GetTempPath() + "ReceiveVoucher";

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

                    ((TextObject)AppEnv.Current.p_rptSource.ReportDefinition.Sections["Section4"].ReportObjects["txtPreviousDue"]).Text = String.Format("{0:n}", Convert.ToDecimal(txtNeedToReceive.Text));
                    ((TextObject)AppEnv.Current.p_rptSource.ReportDefinition.Sections["Section4"].ReportObjects["txtPaidAmount"]).Text = String.Format("{0:n}", Convert.ToDecimal(txtReceiveAmt.Text));
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
            try
            {
                if (cmPartyName.SelectedValue == "")
                {
                    lblMessage.Text = "Select Customer.";
                }
                else
                {
                    EnableControl(true);
                    //ClearControl();
                    ButtonControl("N");
                    cmPartyName.Enabled = false;

                    //int max = Convert.ToInt32(GetAutoNumber("invoiceno", "tblMoneyReceived"));
                    //cmInvoiceNo.Text = max.ToString();
                    //cmInvoiceNo.SelectedValue = max.ToString();
                    dpReceiveDate.SelectedDate = System.DateTime.Now;
                    lblOperationMode.Text = "Save Mode";
                    lblMessage.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (cmPartyName.SelectedValue == "")
            {
                lblMessage.Text = "Select Party Name.";
            }
            else if (cmReceiveMode.SelectedValue == "")
            {
                lblMessage.Text = "Select Receive Mode.";
            }
            else if (dpReceiveDate.SelectedDate == null)
            {
                lblMessage.Text = "Select Receive Date.";
            }
            else if (txtReceiveAmt.Text == "")
            {
                lblMessage.Text = "Receive Amount can not be Blank.";
            }
            //else if (Convert.ToDecimal(txtReceiveAmt.Text) == 0)
            //{
            //    lblMessage.Text = "Receive Amount can not be Zero.";
            //}
            //else if (Convert.ToDecimal(lblDue.Text) < 0)
            //{
            //    lblMessage.Text = "Receive Amount can not be greater than Need to Receive.";
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
                    txtNeedToReceive.Text = "";
                    lblDue.Text = "0.00";
                    ReloadMainGrid();
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
            ClearControlPartial();
            txtNeedToReceive.Text = "";
            lblDue.Text = "";
            cmPartyName.SelectedValue = "";
            cmPartyName.Text = "";
            cmPartyName.Enabled = true;
        }

        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = this.dtItemDetails;
        }
        protected void cmReceiveMode_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cmReceiveMode.SelectedValue == "Cash")
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
                cmd = new SqlCommand("Sp_MoneyReceived", con);
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
                    item.Value = dataRow["CustCode"].ToString();

                    string Id = (string)dataRow["CustCode"].ToString();
                    item.Attributes.Add("CustCode", Id.ToString());

                    string Address = (string)dataRow["Address"].ToString();
                    item.Attributes.Add("Address", Address.ToString());

                    string Mobile = (string)dataRow["Mobile"].ToString();
                    item.Attributes.Add("Mobile", Mobile.ToString());

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
        protected void txtReceiveAmt_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (hfPreviousDue.Value == "")
                    hfPreviousDue.Value = "0";
                if (txtNeedToReceive.Text != "" && txtReceiveAmt.Text != "" && hfPreviousDue.Value!="")
                {
                    decimal NeedToReceive = Convert.ToDecimal(txtNeedToReceive.Text);
                    decimal ReceiveAmt = Convert.ToDecimal(txtReceiveAmt.Text);
                    decimal PreviousDue = Convert.ToDecimal(hfPreviousDue.Value);
                    decimal Due = (NeedToReceive - ReceiveAmt + PreviousDue);
                    lblDue.Text = Due.ToString();
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
                if (cmPartyName.SelectedValue != "")
                {
                    con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
                    con.Open();
                    cmd = new SqlCommand("Sp_MoneyReceived", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 5;
                    cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                    cmd.Parameters.Add("@CustId", SqlDbType.Int).Value = Convert.ToInt32(cmPartyName.SelectedValue);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    txtNeedToReceive.Text = dt.Rows[0]["Balance"].ToString();
                    con.Close();
                    txtReceiveAmt.Text = "";
                    lblDue.Text = "0.00";

                    ReloadMainGrid();
                }
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
                cmd = new SqlCommand("Sp_MoneyReceived", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.VarChar).Value = 12;
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                cmd.Parameters.Add("@track_id", SqlDbType.VarChar).Value = hfTrackId.Value;
                cmd.Parameters.Add("@CustCode", SqlDbType.VarChar).Value = cmPartyName.SelectedValue;
                SqlDataAdapter Dap = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                Dap.Fill(dt);
                cmd.Dispose();

                string tempPath = "";
                string reportName = "";

                reportName = "RV-" + cmPartyName.Text;
                AppEnv.Current.p_rptObject = "~/Reports/ReceiveVoucher.rpt";
                tempPath = @System.IO.Path.GetTempPath() + "RV";

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

                    ((TextObject)AppEnv.Current.p_rptSource.ReportDefinition.Sections["Section4"].ReportObjects["txtPreviousDue"]).Text = String.Format("{0:n}", (Convert.ToDecimal(txtNeedToReceive.Text) + Convert.ToDecimal(txtReceiveAmt.Text)));
                    ((TextObject)AppEnv.Current.p_rptSource.ReportDefinition.Sections["Section4"].ReportObjects["txtPaidAmount"]).Text = String.Format("{0:n}", Convert.ToDecimal(txtReceiveAmt.Text));
                    ((TextObject)AppEnv.Current.p_rptSource.ReportDefinition.Sections["Section4"].ReportObjects["txtTotalDue"]).Text = String.Format("{0:n}", Convert.ToDecimal(txtNeedToReceive.Text));
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
                cmd = new SqlCommand("Sp_MoneyReceived", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.VarChar).Value = 12;
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                cmd.Parameters.Add("@track_id", SqlDbType.VarChar).Value = hfTrackId.Value;
                cmd.Parameters.Add("@CustCode", SqlDbType.VarChar).Value = cmPartyName.SelectedValue;
                SqlDataAdapter Dap = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                Dap.Fill(dt);
                cmd.Dispose();

                string tempPath = "";
                string reportName = "";

                reportName = "RV-" + cmPartyName.Text;
                AppEnv.Current.p_rptObject = "~/Reports/ReceiveVoucher.rpt";
                tempPath = @System.IO.Path.GetTempPath() + "RV";

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

                    ((TextObject)AppEnv.Current.p_rptSource.ReportDefinition.Sections["Section4"].ReportObjects["txtPreviousDue"]).Text = String.Format("{0:n}", Convert.ToDecimal(txtNeedToReceive.Text));
                    ((TextObject)AppEnv.Current.p_rptSource.ReportDefinition.Sections["Section4"].ReportObjects["txtPaidAmount"]).Text = String.Format("{0:n}", Convert.ToDecimal(txtReceiveAmt.Text));
                    ((TextObject)AppEnv.Current.p_rptSource.ReportDefinition.Sections["Section4"].ReportObjects["txtTotalDue"]).Text = String.Format("{0:n}", (Convert.ToDecimal(txtNeedToReceive.Text) - Convert.ToDecimal(txtReceiveAmt.Text)));
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