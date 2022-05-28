using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SobujBanglaARM.Forms
{
    public partial class StockEntry : System.Web.UI.Page
    {
        SobujBanglaARMEntities db = new SobujBanglaARMEntities();
        SqlConnection con;
        SqlCommand cmd;
        public string GetAutoNumber(string fieldName, string tableName)
        {
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
            dpEntryDate.Enabled = ec;
            txtDetails.Enabled = ec;
        }
        public void ClearControl()
        {
            txtQty.Text = "";
            txtDetails.Text = "";
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
        private void ReloadMainGrid()
        {
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
                con.Open();
                cmd = new SqlCommand("Sp_Stocks", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 4;
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                adpt.Fill(ds);
                dt = ds.Tables[0];
                RadGrid1.DataSource = dt;
                RadGrid1.DataBind();
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
                RadGrid1.DataSource = null;
                RadGrid1.Rebind();

                GridDataItem selectedItem = (GridDataItem)RadGrid1.SelectedItems[0];
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

                //for (int i = 0; i < dt2.Rows.Count; i++)
                //{
                //    DataRow newRow = this.dtItemDetailSales.NewRow();
                //    newRow["rowid"] = this.dtItemDetailSales.Rows.Count + 1;
                //    newRow["ItemCode"] = dt2.Rows[i]["ItemCode"].ToString();
                //    newRow["ItemName"] = dt2.Rows[i]["ItemName"].ToString();
                //    newRow["Qty"] = dt2.Rows[i]["Qty"].ToString();
                //    newRow["UnitPrice"] = dt2.Rows[i]["UnitPrice"].ToString();
                //    newRow["Amount"] = dt2.Rows[i]["Amount"].ToString();
                //    this.dtItemDetailSales.Rows.Add(newRow);
                //    this.dtItemDetailSales.AcceptChanges();
                //}
                //RadGrid1.Rebind();

                ////cmPurchaseFor.SelectedValue = dt1.Rows[0]["Projcode"].ToString();
                //txtSalesID.Text = dt1.Rows[0]["SalesId"].ToString();
                //txtVoucherNo.Text = dt1.Rows[0]["VoucherNo"].ToString();
                //txtBillNo.Text = dt1.Rows[0]["BillNo"].ToString();
                //dpSalesDt.SelectedDate = Convert.ToDateTime(dt1.Rows[0]["SalesDate"].ToString());
                //cmCustomerName.SelectedValue = dt1.Rows[0]["CustId"].ToString();
                //cmCustomerName.Text = dt1.Rows[0]["CustName"].ToString();
                //txtRemarks.Text = dt1.Rows[0]["Remarks"].ToString();
                //cmReference.SelectedValue = dt1.Rows[0]["Reference"].ToString();
                //cmShipTo.SelectedValue = dt1.Rows[0]["ShipTo"].ToString();
                ////= dt1.Rows[0]["TotalAmount"].ToString();
                //txtLabourCost.Text = dt1.Rows[0]["LabourCost"].ToString();
                //txtOtherCost.Text = dt1.Rows[0]["OtherCost"].ToString();
                //txtPaidAmount.Text = dt1.Rows[0]["Paid"].ToString();
                //txtTax.Text = dt1.Rows[0]["TaxVAT"].ToString();
                //txtDiscount.Text = dt1.Rows[0]["Discount"].ToString();
                //lblDueAmount.Text = dt1.Rows[0]["Due"].ToString();

                //cmPayMode.SelectedValue = dt1.Rows[0]["PayMode"].ToString();
                //txtChequeNo.Text = dt1.Rows[0]["ChequeNo"].ToString();
                //txtChequeDetails.Text = dt1.Rows[0]["ChequeDetails"].ToString();
                ////dpCheckDate.SelectedDate = Convert.ToDateTime(dt1.Rows[0]["ChequeDt"]);
                ////txtDepositBank.Text = dt1.Rows[0]["DepositBank"].ToString();
                //txtBankName.Text = dt1.Rows[0]["BankName"].ToString();
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
            try
            {
                cmd = new SqlCommand("Sp_Stocks", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 1;
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                cmd.Parameters.Add("@Details", SqlDbType.VarChar).Value = txtDetails.Text;
                cmd.Parameters.Add("@EntryDate", SqlDbType.DateTime).Value = Convert.ToDateTime(dpEntryDate.SelectedDate);
                cmd.Parameters.Add("@StockQty", SqlDbType.Decimal).Value = Convert.ToDecimal(txtQty.Text);
                cmd.Parameters.Add("@Itemcode", SqlDbType.Int).Value = cmItemName.SelectedValue;
                cmd.Parameters.Add("@UserId", SqlDbType.VarChar).Value = AppEnv.Current.p_UserName.ToString();
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message.ToString();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReloadMainGrid();
                EnableControl(false);
                ClearControl();
                ButtonControl("L");
            }
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            if (cmItemName.SelectedValue == "")
            {
                lblMessage.Text = "Select Item Name.";
            }
            else
            {
                EnableControl(true);
                ClearControl();
                ButtonControl("N");
                dpEntryDate.SelectedDate = System.DateTime.Now;
                cmItemName.Enabled = false;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (cmItemName.SelectedValue == "")
            {
                lblMessage.Text = "Select Item Name.";
            }
            else if (dpEntryDate.SelectedDate == null)
            {
                lblMessage.Text = "Select Entry Date.";
            }
            else if (txtQty.Text == "")
            {
                lblMessage.Text = "Quantity can not be Zoro or Blank.";
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
                        EnableControl(false);
                        RadGrid1.DataBind();
                        cmItemName.Enabled = true;
                        lblMessage.Text = "Stock Quantity Added Successfully.";
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = ex.Message;
                    }
                }
                else if (lblOperationMode.Text == "Edit Mode")
                {
                    try
                    {
                        con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
                        con.Open();
                        cmd = new SqlCommand("Sp_tblPurchase", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 3;
                        cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                        //cmd.Parameters.Add("@PurID", SqlDbType.VarChar).Value = txtStockId.Text;
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        SaveData();
                        ButtonControl("L");
                        ClearControl();
                        EnableControl(false);
                        cmItemName.Enabled = true;
                        //dtItemDetails.Clear();
                        RadGrid1.Rebind();
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = ex.Message;
                    }
                }
            }
            ReloadMainGrid();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {

        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            EnableControl(true);
            ButtonControl("E");
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControl();
            EnableControl(false);
            ButtonControl("L");
            cmItemName.Enabled = true;
        }
        protected void RadGrid1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearControl();
            ButtonControl("F");
            EnableControl(false);
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
        protected void cmItemName_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["sbcon"].ConnectionString);
                con.Open();
                cmd = new SqlCommand("Sp_Stocks", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.Int).Value = 5;
                cmd.Parameters.Add("@IdClient", SqlDbType.VarChar).Value = AppEnv.Current.p_IdClient;
                cmd.Parameters.Add("@ItemCode", SqlDbType.VarChar).Value = cmItemName.SelectedValue;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                RadGrid1.DataSource = dt;
                RadGrid1.DataBind();
                con.Close();
                //ReloadMainGrid();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        protected void rbtDeleteGrid_Click(object sender, ImageClickEventArgs e)
        {

        }
        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {

        }
        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {

        }
        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = db.tblStocks.ToList();
        }
    }
}