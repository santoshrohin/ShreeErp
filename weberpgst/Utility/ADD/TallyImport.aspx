<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="TallyImport.aspx.cs" Inherits="Account_ReportForms_VIEW_TallyImport" Title="Tally Import" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">



<asp:Label ID="lblmessage"  runat="server"></asp:Label>
<div>Import Status</div>
<asp:label id="myLabel" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
 
   
      <div style="background: aqua;">
         <h3> File Upload:</h3>
         <br />
         <asp:FileUpload ID="FileUpload1" runat="server" />
         <br /><br />
         <asp:Button ID="btnsave" runat="server" onclick="btnsave_Click"  Text="Upload" style="width:85px" />
         <br /><br />
         
         <br /><br /><br /><br /><br /><br />
         <asp:Button ID="btnTransaction" runat="server" onclick="btnTransaction_Click"  Text="Submit" style="width:85px" />
         <br /><br />
         <asp:Label ID="Label1" runat="server" />
         
           <br /><br /><br /><br /><br /><br />
         <asp:Button ID="btnBankImport" runat="server" onclick="btnBankImport_Click"  Text="Bank Import" style="width:85px" />
         <br /><br />
      </div>
      
   


<div style="background: aqua;">Import Status Second</div>
<asp:label id="lblmsz" runat="server" />
</asp:Content>

