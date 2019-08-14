using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;
using System.Security.Cryptography.Xml;

namespace BankingSignature
{
    public partial class admin : System.Web.UI.Page
    {
        //setting the database
        string conn = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["user_id"] != null)
            {

                string s = Request.QueryString["user_id"];
                Session["user_id"] = s;


                // txtValueA.Text = showData(s);

            }
            else
            {

                Response.Redirect("login.aspx?text=you must be login to continue");

            }

            using (SqlConnection con = new SqlConnection(conn))
            {
                SqlDataAdapter sda = new SqlDataAdapter("select * from [dbo].[enc_file]", con);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                GridView1.DataSource = ds;
                GridView1.DataBind();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //Generating and exporting RSA keys
            int user = Convert.ToInt32(userId.Text);
            //string privateKeyPath = @"C:/Users/Acer PC/source/repos/BankingSignature/BankingSignature/pri.xml";
            string privateKeyPath = @"F:/CSE-400/BankingSignature/BankingSignature/pri.xml";
            //string publicKeyPath = @"C:/Users/Acer PC/source/repos/BankingSignature/BankingSignature/pub.xml";
            string publicKeyPath = @"F:/CSE-400/BankingSignature/BankingSignature/pub.xml";
            int size = 1024;
            GenerateRsa(privateKeyPath, publicKeyPath, size);


            //Generating various parameters for the storing it in the
            //database
            XmlDocument document = new XmlDocument();
            document.Load(privateKeyPath); // Load XML File

            XmlNode node = document.SelectSingleNode("/RSAKeyValue/Modulus");
            string modulus = node.InnerText;

            node = document.SelectSingleNode("/RSAKeyValue/Exponent");
            string exp = node.InnerText;

            node = document.SelectSingleNode("/RSAKeyValue/P");
            string p = node.InnerText;

            node = document.SelectSingleNode("/RSAKeyValue/Q");
            string q = node.InnerText;

            node = document.SelectSingleNode("/RSAKeyValue/DP");
            string dp = node.InnerText;

            node = document.SelectSingleNode("/RSAKeyValue/DQ");
            string dq = node.InnerText;

            node = document.SelectSingleNode("/RSAKeyValue/InverseQ");
            string inv = node.InnerText;

            node = document.SelectSingleNode("/RSAKeyValue/D");
            string d = node.InnerText;

            SqlConnection sqlCon = new SqlConnection(conn);
            sqlCon.Open();
            if (sqlCon.State == System.Data.ConnectionState.Open)
            {

                // for inserting new data 
                string query = "INSERT INTO [dbo].[user_key]([id] ,[mod] ,[p],[exp],[q],[dq],[inv],[dp],[d]) VALUES('" + user
                    + "','" + modulus + "','" + p + "','" + exp + "','" + q + "','" + dq + "','" + inv + "','" + dp + "','" + d + "')";


                SqlCommand cmd = new SqlCommand(query, sqlCon);
                int t = cmd.ExecuteNonQuery();


                if (t == 1)
                {

                    //Response.Write("Key Generation Successful!!");
                    Response.Write("<script>alert('Key Generation Successful!!');</script>");

                }
                else
                {
                    Response.Write("<script>alert('There was a error !!!');</script>");
                    //Response.Write("There was a error !!!");
                }

                sqlCon.Close();
            }

        }

        //Key Regeneration
        protected void Button2_Click(object sender, EventArgs e)
        {
            //Generating and exporting RSA keys
            int user = Convert.ToInt32(userId.Text);
            //string privateKeyPath = @"C:/Users/Acer PC/source/repos/BankingSignature/BankingSignature/pri.xml";
            //string publicKeyPath = @"C:/Users/Acer PC/source/repos/BankingSignature/BankingSignature/pub.xml";

            string privateKeyPath = @"F:/CSE-400/BankingSignature/BankingSignature/pri.xml";
            //string publicKeyPath = @"C:/Users/Acer PC/source/repos/BankingSignature/BankingSignature/pub.xml";
            string publicKeyPath = @"F:/CSE-400/BankingSignature/BankingSignature/pub.xml";
            int size = 1024;
            GenerateRsa(privateKeyPath, publicKeyPath, size);


            //Generating various parameters for the storing it in the
            //database
            XmlDocument document = new XmlDocument();
            document.Load(privateKeyPath); // Load XML File

            XmlNode node = document.SelectSingleNode("/RSAKeyValue/Modulus");
            string modulus = node.InnerText;

            node = document.SelectSingleNode("/RSAKeyValue/Exponent");
            string exp = node.InnerText;

            node = document.SelectSingleNode("/RSAKeyValue/P");
            string p = node.InnerText;

            node = document.SelectSingleNode("/RSAKeyValue/Q");
            string q = node.InnerText;

            node = document.SelectSingleNode("/RSAKeyValue/DP");
            string dp = node.InnerText;

            node = document.SelectSingleNode("/RSAKeyValue/DQ");
            string dq = node.InnerText;

            node = document.SelectSingleNode("/RSAKeyValue/InverseQ");
            string inv = node.InnerText;

            node = document.SelectSingleNode("/RSAKeyValue/D");
            string d = node.InnerText;

            SqlConnection sqlCon = new SqlConnection(conn);
            sqlCon.Open();
            if (sqlCon.State == System.Data.ConnectionState.Open)
            {

                string query = "UPDATE[dbo].[user_key] SET [mod] = '" + modulus + "',[p] = '" + p + "',[exp] = '" + exp + "',[q] = '" + q + "',[dq] = '" + dq + "',[inv] = '" + inv + "',[dp] = '" + dp + "',[d] = '" + d + "' WHERE [id]='" + user + "' ";



                SqlCommand cmd = new SqlCommand(query, sqlCon);
                int t = cmd.ExecuteNonQuery();


                if (t == 1)
                {

                    //Response.Write("Key ReGeneration Successful!!");
                    Response.Write("<script>alert('Key Regeneration Successful!!');</script>");


                }
                else
                {
                    //Response.Write("There was an error !!!");
                    Response.Write("<script>alert('There was a error !!!');</script>");
                }

                sqlCon.Close();
            }
        }

        public static void GenerateRsa(string privateKeyPath, string publicKeyPath, int size)
        {
            //stream to save the keys
            FileStream fs = null;
            StreamWriter sw = null;

            //create RSA provider
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(size);
            try
            {
                //save private key
                fs = new FileStream(privateKeyPath, FileMode.Create, FileAccess.Write);
                sw = new StreamWriter(fs);
                sw.Write(rsa.ToXmlString(true));
                sw.Flush();
            }
            finally
            {
                if (sw != null) sw.Close();
                if (fs != null) fs.Close();
            }

            try
            {
                //save public key
                fs = new FileStream(publicKeyPath, FileMode.Create, FileAccess.Write);
                sw = new StreamWriter(fs);
                sw.Write(rsa.ToXmlString(false));
                sw.Flush();
            }
            finally
            {
                if (sw != null) sw.Close();
                if (fs != null) fs.Close();
            }
            rsa.Clear();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "validate")
            {
                int crow;
                crow = Convert.ToInt32(e.CommandArgument.ToString());
                string v = GridView1.Rows[crow].Cells[0].Text;



                Label1.Text = "User Id of Selected Customer is " + v;



                //Reading key from database
                SqlConnection sqlCon = new SqlConnection(conn);
                SqlDataReader dr;
                SqlCommand cmd,cmd1;

                sqlCon.Open();
                if (sqlCon.State == System.Data.ConnectionState.Open)
                {
                    string FetchData = "Select * from user_key where id='" + v + "'";
                   // string Fetch = "Select file from enc_file where id='"+ v +"'";
                    cmd = new SqlCommand(FetchData, sqlCon);
                    dr = cmd.ExecuteReader();


                    if (dr.Read())
                    {

                        string modulus = dr[1].ToString();
                        string exp = dr[3].ToString();


                        string p = dr[2].ToString();

                        string q = dr[4].ToString();


                        string dp = dr[7].ToString();


                        string dq = dr[5].ToString();


                        string inv = dr[6].ToString();


                        string d = dr[8].ToString();

                        CspParameters cspParams = new CspParameters();
                        cspParams.KeyContainerName = "XML_DSIG_RSA_KEY";

                        RSACryptoServiceProvider rsaKey = new RSACryptoServiceProvider(cspParams);

                        RSAParameters lRSAKeyInfo = new RSAParameters();

                        lRSAKeyInfo.Modulus = Convert.FromBase64String(modulus);
                        lRSAKeyInfo.Exponent = Convert.FromBase64String(exp);
                        lRSAKeyInfo.P = Convert.FromBase64String(p);
                        lRSAKeyInfo.Q = Convert.FromBase64String(q);
                        lRSAKeyInfo.DP = Convert.FromBase64String(dp);
                        lRSAKeyInfo.DQ = Convert.FromBase64String(dq);
                        lRSAKeyInfo.InverseQ = Convert.FromBase64String(inv);
                        lRSAKeyInfo.D = Convert.FromBase64String(d);

                        rsaKey.ImportParameters(lRSAKeyInfo);

                        XmlDocument xmlDoc = new XmlDocument();
                        string filename = Server.MapPath("test.xml");
                        xmlDoc.PreserveWhitespace = true;
                        xmlDoc.Load(filename);

                        bool result = VerifyXml(xmlDoc, rsaKey);

                        // Display the results of the signature verification to 
                        // the console.
                        if (result)
                        {
                            //Response.Write("The XML signature is valid.");
                            Response.Write("<script>alert('The XML signature is valid.');</script>");
                        }
                        else
                        {
                            //Response.Write("The XML signature is not valid.");
                            Response.Write("<script>alert('The XML signature is not valid.');</script>");

                        }


                    }
                    sqlCon.Close();
                }

            }

        }

        public static Boolean VerifyXml(XmlDocument xmlDoc, RSA key)
        {
            // Check arguments.
            if (xmlDoc == null)
                throw new ArgumentException("xmlDoc");
            if (key == null)
                throw new ArgumentException("key");

            // Create a new SignedXml object and pass it
            // the XML document class.
            SignedXml signedXml = new SignedXml(xmlDoc);

            // Find the "Signature" node and create a new
            // XmlNodeList object.
            XmlNodeList nodeList = xmlDoc.GetElementsByTagName("Signature");

            // Throw an exception if no signature was found.
            if (nodeList.Count <= 0)
            {
                //throw new CryptographicException("Verification failed: No Signature was found in the document.");
            }

            // This example only supports one signature for
            // the entire XML document.  Throw an exception 
            // if more than one signature was found.
            if (nodeList.Count >= 2)
            {

                // MessageBox.Show("Verification failed: No Signature was found in the document.");
                //throw new CryptographicException("Verification failed: More that one signature was found for the document.");
            }

            // Load the first <signature> node.  
            signedXml.LoadXml((XmlElement)nodeList[0]);

            // Check the signature and return the result.
            return signedXml.CheckSignature(key);

        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            Session["user_id"] = null;
            Response.Redirect("login.aspx");
        }
    }
}