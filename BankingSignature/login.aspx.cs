using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace BankingSignature
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void submit_Click(object sender, EventArgs e)
        {
            string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {

                string q1 = "select count(*) from [tbl_login] where username='" + username.Text + "' and password='" + pwd.Text.ToString() + "'";
                string q2 = "select id from [tbl_login] where username='" + username.Text + "' and password='" + pwd.Text.ToString() + "'";
                SqlCommand cmd1 = new SqlCommand(q1, con);
                SqlCommand cmd2 = new SqlCommand(q2, con);
                con.Open();
                int temp = Convert.ToInt32(cmd1.ExecuteScalar().ToString());
                HiddenField1.Value = cmd2.ExecuteScalar().ToString();
                int i = Convert.ToInt32(HiddenField1.Value);

                if (temp == 1)
                {
                    if (i == 1)
                    {
                        //Response.Redirect("#");
                        Response.Redirect("admin.aspx?user_id=" + HiddenField1.Value);
                    }
                    else
                    {
                        //Response.Redirect("#");
                        Response.Redirect("TransactInfo.aspx?user_id=" + HiddenField1.Value);
                    }

                }
                else
                {
                    // Response.Write("Invalid login id or password !");
                    Response.Write("<script>alert('Invalid login id or password !')</script>");
                }
            }
        }
    }
}