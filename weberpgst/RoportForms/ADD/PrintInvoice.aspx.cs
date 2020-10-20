using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Text;
using System.Net; 
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
public partial class RoportForms_ADD_PrintInvoice : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //pdf();
        GenerateInvoicePDF();
    }
    public void pdf()
    {
        try
        {
            string companyName = "ASPSnippets";
            int orderNo = 2303;
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[5] {
                            new DataColumn("ProductId", typeof(string)),
                            new DataColumn("Product", typeof(string)),
                            new DataColumn("Price", typeof(int)),
                            new DataColumn("Quantity", typeof(int)),
                            new DataColumn("Total", typeof(int))});
            dt.Rows.Add(101, "Sun Glasses", 200, 5, 1000);
            dt.Rows.Add(102, "Jeans", 400, 2, 800);
            dt.Rows.Add(103, "Trousers", 300, 3, 900);
            dt.Rows.Add(104, "Shirts", 550, 2, 1100);
            StringBuilder sb = new StringBuilder();

            //Generate Invoice (Bill) Header.
            sb.Append("<table border = '5' Style='position: absolute;  height: 100%; width: 100%'>");
            sb.Append("<tr>");
            sb.Append("<td>");
            sb.Append("<table border = '1' >");
            sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '2'><b>Order Sheet</b></td></tr>");
            sb.Append("<tr><td colspan = '2'></td></tr>");
            sb.Append("<tr><td><b>Order No: </b>");
            sb.Append(orderNo);
            sb.Append("</td><td align = 'right'><b>Date: </b>");
            sb.Append(DateTime.Now);
            sb.Append(" </td></tr>");
            sb.Append("<tr><td colspan = '2'><b>Company Name: </b>");
            sb.Append(companyName);
            sb.Append("</td></tr>");
            sb.Append("</table>");
            sb.Append("<br />");

            //Generate Invoice (Bill) Items Grid.
            sb.Append("<table border = '1'>");
            sb.Append("<tr>");
            foreach (DataColumn column in dt.Columns)
            {
                sb.Append("<th style = 'background-color: #D20B0C;color:#ffffff'>");
                sb.Append(column.ColumnName);
                sb.Append("</th>");
            }
            sb.Append("</tr>");
            foreach (DataRow row in dt.Rows)
            {
                sb.Append("<tr>");
                foreach (DataColumn column in dt.Columns)
                {
                    sb.Append("<td>");
                    sb.Append(row[column]);
                    sb.Append("</td>");
                }
                sb.Append("</tr>");
            }
            sb.Append("<tr><td align = 'right' colspan = '");
            sb.Append(dt.Columns.Count - 1);
            sb.Append("'>Total</td>");
            sb.Append("<td>");
            sb.Append(dt.Compute("sum(Total)", ""));
            sb.Append("</td>");
            sb.Append("</td>");
            sb.Append("</tr></table>");
            sb.Append("</tr></table>");
            Document document = new Document();
            PdfWriter.GetInstance(document, new FileStream("E:/Santosh/test.pdf", FileMode.Create));
            document.Open();
            Paragraph p1 = new Paragraph(sb.ToString());
            document.Add(p1);//Add the paragarh to the document  
            document.NewPage();
            Paragraph p2 = new Paragraph("This is Second Page");
            document.Add(p2);//Add the paragarh to the document  
            document.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }  
    }
    protected void GenerateInvoicePDF()
    {
        //Dummy data for Invoice (Bill).
        string companyName = "ASPSnippets";
        int orderNo = 2303;
        DataTable dt = new DataTable();
        dt.Columns.AddRange(new DataColumn[5] {
                            new DataColumn("ProductId", typeof(string)),
                            new DataColumn("Product", typeof(string)),
                            new DataColumn("Price", typeof(int)),
                            new DataColumn("Quantity", typeof(int)),
                            new DataColumn("Total", typeof(int))});
        dt.Rows.Add(101, "Sun Glasses", 200, 5, 1000);
        dt.Rows.Add(102, "Jeans", 400, 2, 800);
        dt.Rows.Add(103, "Trousers", 300, 3, 900);
        dt.Rows.Add(104, "Shirts", 550, 2, 1100);

        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter hw = new HtmlTextWriter(sw))
            {
                StringBuilder sb = new StringBuilder();

                //Generate Invoice (Bill) Header.
                sb.Append("<table border = '1' Style='position: absolute;  height: 100%; width: 100%'>");
                sb.Append("<tr ><td align = 'right'></td></tr>");
                sb.Append("<tr border = '0' style='text-align:center;' ><td><h4>Tax Invoice</h4></td></tr>");
                sb.Append("<tr border = '0'><td align = 'right'>Original</td></tr>");
                sb.Append("<tr>");
                sb.Append("<td>");
                sb.Append("<table border = '1' >");
                sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '2'><b>Order Sheet</b></td></tr>");
                sb.Append("<tr><td colspan = '2'></td></tr>");
                sb.Append("<tr><td><b>Order No: </b>");
                sb.Append(orderNo);
                sb.Append("</td><td align = 'right'><b>Date: </b>");
                sb.Append(DateTime.Now);
                sb.Append(" </td></tr>");
                sb.Append("<tr><td colspan = '2'><b>Company Name: </b>");
                sb.Append(companyName);
                sb.Append("</td></tr>");
                sb.Append("</table>");
                sb.Append("<br />");

                //Generate Invoice (Bill) Items Grid.
                sb.Append("<table border = '1'>");
                sb.Append("<tr>");
                foreach (DataColumn column in dt.Columns)
                {
                    sb.Append("<th style = 'background-color: #D20B0C;color:#ffffff'>");
                    sb.Append(column.ColumnName);
                    sb.Append("</th>");
                }
                sb.Append("</tr>");
                foreach (DataRow row in dt.Rows)
                {
                    sb.Append("<tr>");
                    foreach (DataColumn column in dt.Columns)
                    {
                        sb.Append("<td>");
                        sb.Append(row[column]);
                        sb.Append("</td>");
                    }
                    sb.Append("</tr>");
                }
                sb.Append("<tr><td align = 'right' colspan = '");
                sb.Append(dt.Columns.Count - 1);
                sb.Append("'>Total</td>");
                sb.Append("<td>");
                sb.Append(dt.Compute("sum(Total)", ""));
                sb.Append("</td>");
                sb.Append("</td>");
                sb.Append("</tr></table>");
                sb.Append("</tr></table>");
                //Export HTML String as PDF.
                StringReader sr = new StringReader(sb.ToString());
                StringReader sr2 = new StringReader("Hello");
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();
                htmlparser.Parse(sr);
                pdfDoc.NewPage();

                htmlparser.Parse(sr2);
                pdfDoc.Close();
                Response.ContentType = "application/pdf";
                //Response.AddHeader("content-disposition", "attachment;filename=Invoice_" + orderNo + ".pdf");
                Response.AddHeader("content-length", "attachment;filename=Invoice_" + orderNo + ".pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(pdfDoc);
                Response.End();

                  

            }
        }
    }
}
