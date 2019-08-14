using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using System.Xml;

using System;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Xml;

using System.Configuration;
using System.Data.SqlClient;

namespace BankingSignature
{
    public partial class TransactInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["user_id"] != null)
            {

                string s = Request.QueryString["user_id"];
                Session["user_id"] = s;


                txtValueA.Text = showData(s);

            }
            else
            {

                Response.Redirect("login.aspx?text=you must be login to continue");

            }
        }

        public string conn = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        public string showData(string s)
        {
            SqlConnection sqlCon = new SqlConnection(conn);
            sqlCon.Open();
            string p = null;
            if (sqlCon.State == System.Data.ConnectionState.Open)
            {

                string query = "select username from [tbl_login] where id='" + s + "' ";
                SqlCommand cmd = new SqlCommand(query, sqlCon);

                //p = Convert.ToString(cmd.ExecuteScalar());
                p = cmd.ExecuteScalar().ToString();


            }
            return p;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string filename = Server.MapPath("test.xml");

            if (File.Exists(filename) == true)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filename);
                doc.DocumentElement.RemoveAll();
                string result = doc.OuterXml;
                //create new file & structure

                XmlTextWriter xtw = new XmlTextWriter(filename, null);

                xtw.WriteStartDocument();

                xtw.WriteStartElement("User");

                xtw.WriteStartElement("TransactInfo");

                xtw.WriteElementString("Acc.No", txtAcc.Text);
                xtw.WriteElementString("TransactionDate", txtDate.Text);
                xtw.WriteElementString("TransactionType", rbtType.SelectedItem.Text);
                xtw.WriteElementString("Amount", txtAmount.Text);

                xtw.WriteEndElement();

                xtw.WriteEndElement();

                xtw.WriteEndDocument();

                xtw.Close();
            }
            else
            {
                //create new file & structure

                XmlTextWriter xtw = new XmlTextWriter(filename, null);

                xtw.WriteStartDocument();

                xtw.WriteStartElement("User");

                xtw.WriteStartElement("TransactInfo");

                xtw.WriteElementString("Acc.No", txtAcc.Text);
                xtw.WriteElementString("TransactionDate", txtDate.Text);
                xtw.WriteElementString("TransactionType", rbtType.SelectedItem.Text);
                xtw.WriteElementString("Amount", txtAmount.Text);

                xtw.WriteEndElement();

                xtw.WriteEndElement();

                xtw.WriteEndDocument();

                xtw.Close();
            }

            //Reading key from database
            SqlConnection sqlCon = new SqlConnection(conn);
            SqlDataReader dr;
            SqlCommand cmd, cmd1;

            sqlCon.Open();
            if (sqlCon.State == System.Data.ConnectionState.Open)
            {

                string FetchData = "Select * from user_key where id='" + Convert.ToInt32(Request.QueryString["user_id"]) + "' ";
                
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
                    //Create a new instance of the RSACryptoServiceProvider class.
                    RSACryptoServiceProvider rsaKey = new RSACryptoServiceProvider(cspParams);


                    //Create a new instance of the RSAParameters structure.
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

                    xmlDoc.PreserveWhitespace = true;
                    xmlDoc.Load(filename);

                    Encrypt(xmlDoc, "User", "EncryptedElement1", rsaKey, "rsaKey");
                    xmlDoc.Save(filename);

                    SignXml(xmlDoc, rsaKey);

                    xmlDoc.Save(filename);
                    sqlCon.Close();
                   // Response.Write("Verifying signature...");
                    Response.Write("<script>alert('Verifying signature...');</script>");

                    string query = "INSERT INTO [dbo].[enc_file] ([user_id], [file]) VALUES('" + Convert.ToInt32(Request.QueryString["user_id"]) + "',@xml)";
                    cmd = new SqlCommand(query, sqlCon);
                    sqlCon.Open();
                    cmd.Parameters.AddWithValue("@xml", xmlDoc.InnerXml);
                    cmd.ExecuteNonQuery();
                }
                sqlCon.Close();
            }
        }

        public static void Encrypt(XmlDocument Doc, string ElementToEncrypt, string EncryptionElementID, RSA Alg, string KeyName)
        {
            if (Doc == null)
                throw new ArgumentNullException("Doc");
            if (ElementToEncrypt == null)
                throw new ArgumentNullException("ElementToEncrypt");
            if (EncryptionElementID == null)
                throw new ArgumentNullException("EncryptionElementID");
            if (Alg == null)
                throw new ArgumentNullException("Alg");
            if (KeyName == null)
                throw new ArgumentNullException("KeyName");


            XmlElement elementToEncrypt = Doc.GetElementsByTagName(ElementToEncrypt)[0] as XmlElement;

            RijndaelManaged sessionKey = null;

            try
            {
                //////////////////////////////////////////////////
                // Create a new instance of the EncryptedXml class
                // and use it to encrypt the XmlElement with the
                // a new random symmetric key.
                //////////////////////////////////////////////////

                // Create a 256 bit Rijndael key.
                sessionKey = new RijndaelManaged();
                sessionKey.KeySize = 256;

                EncryptedXml eXml = new EncryptedXml();

                byte[] encryptedElement = eXml.EncryptData(elementToEncrypt, sessionKey, false);
                ////////////////////////////////////////////////
                // Construct an EncryptedData object and populate
                // it with the desired encryption information.
                ////////////////////////////////////////////////

                EncryptedData edElement = new EncryptedData();
                edElement.Type = EncryptedXml.XmlEncElementUrl;
                edElement.Id = EncryptionElementID;
                // Create an EncryptionMethod element so that the
                // receiver knows which algorithm to use for decryption.

                edElement.EncryptionMethod = new EncryptionMethod(EncryptedXml.XmlEncAES256Url);
                // Encrypt the session key and add it to an EncryptedKey element.
                EncryptedKey ek = new EncryptedKey();

                byte[] encryptedKey = EncryptedXml.EncryptKey(sessionKey.Key, Alg, false);

                ek.CipherData = new CipherData(encryptedKey);

                ek.EncryptionMethod = new EncryptionMethod(EncryptedXml.XmlEncRSA15Url);

                // Create a new DataReference element
                // for the KeyInfo element.  This optional
                // element specifies which EncryptedData
                // uses this key.  An XML document can have
                // multiple EncryptedData elements that use
                // different keys.
                DataReference dRef = new DataReference();

                // Specify the EncryptedData URI.
                dRef.Uri = "#" + EncryptionElementID;

                // Add the DataReference to the EncryptedKey.
                ek.AddReference(dRef);
                // Add the encrypted key to the
                // EncryptedData object.

                edElement.KeyInfo.AddClause(new KeyInfoEncryptedKey(ek));
                // Set the KeyInfo element to specify the
                // name of the RSA key.


                // Create a new KeyInfoName element.
                KeyInfoName kin = new KeyInfoName();

                // Specify a name for the key.
                kin.Value = KeyName;

                // Add the KeyInfoName element to the
                // EncryptedKey object.
                ek.KeyInfo.AddClause(kin);
                // Add the encrypted element data to the
                // EncryptedData object.
                edElement.CipherData.CipherValue = encryptedElement;
                ////////////////////////////////////////////////////
                // Replace the element from the original XmlDocument
                // object with the EncryptedData element.
                ////////////////////////////////////////////////////
                EncryptedXml.ReplaceElement(elementToEncrypt, edElement, false);
            }
            catch (Exception e)
            {
                // re-throw the exception.
                // throw e;
            }
            finally
            {
                if (sessionKey != null)
                {
                    sessionKey.Clear();
                }

            }

        }

        public static void SignXml(XmlDocument xmlDoc, RSA rsaKey)
        {

            // Check arguments.
            if (xmlDoc == null)
                throw new ArgumentException(nameof(xmlDoc));
            if (rsaKey == null)
                throw new ArgumentException(nameof(rsaKey));

            // Create a SignedXml object.
            SignedXml signedXml = new SignedXml(xmlDoc);

            // Add the key to the SignedXml document.
            signedXml.SigningKey = rsaKey;

            // Create a reference to be signed.
            Reference reference = new Reference();
            reference.Uri = "";

            // Add an enveloped transformation to the reference.
            XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
            reference.AddTransform(env);

            // Add the reference to the SignedXml object.
            signedXml.AddReference(reference);

            KeyInfo keyInfo = new KeyInfo();
            keyInfo.AddClause(new RSAKeyValue((RSA)rsaKey));
            signedXml.KeyInfo = keyInfo;

            // Compute the signature.
            signedXml.ComputeSignature();

            // Get the XML representation of the signature and save
            // it to an XmlElement object.
            XmlElement xmlDigitalSignature = signedXml.GetXml();

            // Append the element to the XML document.
            xmlDoc.DocumentElement.AppendChild(xmlDoc.ImportNode(xmlDigitalSignature, true));
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Session["user_id"] = null;
            Response.Redirect("login.aspx");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtAcc.Text = txtDate.Text = txtAmount.Text = null;
        }
    }
}