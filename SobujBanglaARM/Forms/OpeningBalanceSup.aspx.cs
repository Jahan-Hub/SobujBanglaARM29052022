﻿using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Telerik.Web.UI;

namespace SobujBanglaARM.Forms
{
    public partial class OpeningBalanceSup : System.Web.UI.Page
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
            txtTotalAmt.Enabled = ec;
            txtInvoiceNo.Enabled = ec;
            txtRemarks.Enabled = ec;
            txtTotalAmt.Enabled = ec;
        }
        public void ClearControl()
        {
            txtRemarks.Text = "";
            txtInvoiceNo.Text = "";
            txtRemarks.Text = "";
            txtTotalAmt.Text = "";
        }
        public void ButtonControl(string bc)
        {
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
                cmd = new SqlCommand("Sp_OpeningBalanceSup", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 1;
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                cmd.Parameters.Add("@PurId", SqlDbType.VarChar).Value = hfPurId.Value;
                cmd.Parameters.Add("@SupplierId", SqlDbType.VarChar).Value = cmPartyName.SelectedValue;
                cmd.Parameters.Add("@BillNo", SqlDbType.Int).Value = txtInvoiceNo.Text;
                cmd.Parameters.Add("@PayDate", SqlDbType.DateTime).Value = dpBillDate.SelectedDate;
                cmd.Parameters.Add("@NetAmount", SqlDbType.Decimal).Value = txtTotalAmt.Text;
                cmd.Parameters.Add("@Remarks", SqlDbType.VarChar).Value = txtRemarks.Text;
                cmd.Parameters.Add("@userid", SqlDbType.VarChar).Value = AppEnv.Current.p_UserName;
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
                cmd = new SqlCommand("Sp_OpeningBalanceSup", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.VarChar).Value = "9";
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                cmd.Parameters.Add("@custcode", SqlDbType.VarChar).Value = cmPartyName.SelectedValue;
                cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar).Value = txtInvoiceNo.Text;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ClearControl();
                ButtonControl("L");
                EnableControl(false);
                this.dtItemDetails.Clear();
            }
        }
        protected void btnNew_Click(object sender, EventArgs e)
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
                    cmPartyName.Enabled = false;
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
            else if (txtInvoiceNo.Text == "")
            {
                lblMessage.Text = "Invoice No can not be Blank.";
            }
            else if (dpBillDate.SelectedDate == null)
            {
                lblMessage.Text = "Select Bill Date.";
            }
            else if (txtTotalAmt.Text == "" || Convert.ToDecimal(txtTotalAmt.Text) <= 0)
            {
                lblMessage.Text = "Total Amount can not be Blank or Zero or Less than Zero";
            }
            else
            {
                if (lblOperationMode.Text == "Save Mode")
                {
                    try
                    {
                        SaveData();
                        ButtonControl("L");
                        ClearControl();
                        cmPartyName.SelectedValue = "";
                        cmPartyName.Text = "";
                        EnableControl(false);
                        cmPartyName.Enabled = true;
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = ex.Message;
                    }
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
        }

        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = this.dtItemDetails;
        }
        protected void cmPartyName_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
                con.Open();
                cmd = new SqlCommand("Sp_OpeningBalanceSup", con);
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

                    string Id = (string)dataRow["SupplierId"].ToString();
                    item.Attributes.Add("SupplierId", Id.ToString());

                    string Mobile = (string)dataRow["Mobile"].ToString();
                    item.Attributes.Add("Mobile", Mobile.ToString());

                    string VillageName = (string)dataRow["VillageName"].ToString();
                    item.Attributes.Add("VillageName", VillageName.ToString());

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
        protected void cmPartyName_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                ClearControl();
                ButtonControl("N");
                cmPartyName.Enabled = false;
                lblOperationMode.Text = "Save Mode";
                lblMessage.Text = "";

                int max = Convert.ToInt32(GetAutoNumber("PurId", "tblPurchaseHDR"));
                hfPurId.Value = max.ToString();
                txtInvoiceNo.Text = max.ToString();
                EnableControl(true);
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
    }
}