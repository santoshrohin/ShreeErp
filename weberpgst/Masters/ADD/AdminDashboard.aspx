<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="AdminDashboard.aspx.cs" Inherits="Masters_ADD_AdminDashboard" Title="Dashboard" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <!-- BEGIN CONTENT -->
    <style type="text/css">
        .ajax__calendar_container
        {
            z-index: 1000;
        }
        .modalBackground
        {
            background-color: #8B8B8B;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
    </style>
    <!--JS for nested griview-->

    <script src="../../assets/JS/nested-grid-1.8.3-jquery.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        $("[src*=plus]").live("click", function() {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "../../assets/img/minus.gif");
        });
        $("[src*=minus]").live("click", function() {
            $(this).attr("src", "../../assets/img/plus.gif");
            $(this).closest("tr").next().remove();
        });
    </script>

    <!--End of JS for nested griview-->

    <script type="text/javascript">
        function Showalert() {
            $('#Avisos').fadeIn(6000)
            $('#Avisos').fadeOut(6000)
            $("#up").click();
        }
    </script>

    <div class="page-content-wrapper">
        <div class="page-content">
            <div class="row">
                <div id="MSG" class="col-md-6">
                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="PanelMsg" runat="server" Visible="false" Style="background-color: #feefb3;
                                height: 50px; width: 100%; border: 1px solid #9f6000">
                                <div style="vertical-align: middle; margin-top: 10px;">
                                    <asp:Label ID="lblmsg" runat="server" Style="color: #9f6000; font-size: medium; font-weight: bold;
                                        margin-top: 50px; margin-left: 10px;"></asp:Label>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <br />
            
            <div class="row">
            <div id="Div10" runat="server" class="row">
                <div class="col-md-12">
                    <div class="col-md-12">
                        <div class="portlet box green">
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="fa fa-reorder"></i>Turnover
                                </div>
                                <div class="tools">
                                    <a href="javascript:;" class="collapse"></a>
                                </div>
                            </div>
                            <div class="portlet-body">
                                <div class="form-horizontal">
                                    
                                    <div class="row">
                                        <div class="col-md-12">
                                           
                                <table class="table table-hover text-nowrap">
                                  <thead>
                                    <tr>
                                     <th style="color: black;font-size: xx-large;">Gross Turnover (<b><asp:Label runat="server" style="color: black;font-size: xx-large;"   ID="lblFinYear1"></asp:Label></b>)</th>
                                      <th style="color: black;font-size: xx-large;">Basic Turnover (<b><asp:Label runat="server" style="color: black;font-size: xx-large;"   ID="lblFinYear"></asp:Label></b>)</th>
                                     
                                    </tr>
                                  </thead>
                                  
                                  <tbody>
                                    <tr>
                                   <td>(<b><asp:Label runat="server" style="color: black;font-size: xx-large;"   ID="lbltilldateSale"></asp:Label></b>)</td>
                                   <td>(<b><asp:Label runat="server" style="color: black;font-size: xx-large;"   ID="lblBasictilldateSale"></asp:Label></b>)</td>
                                      <td style="display: none;"><asp:Label runat="server" Visible="false" style="display: none;" ID="totalLM"></asp:Label> </td>
                                      
                                     
                                      
                                      <td style="display: none;"><asp:Label runat="server" Visible="false" style="display: none;" ID="CalidadLM"></asp:Label> </td>
                                    </tr>
                                    
                                  </tbody>
                                  
                                </table>
                               
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                           
                                <table style="display: none;" class="table table-hover text-nowrap">
                                  <thead>
                                    <tr>
                                      <th style="display: none;"></th>
                                      <th style="display: none;"></th>
                                     
                                      <th style="display: none;"></th>
                                      <th style="display: none;"></th>
                                    </tr>
                                  </thead>
                                  
                                  <tbody>
                                    <tr>
                                      <td style="display: none;"><asp:Label runat="server" Visible="false" style="display: none;" ID="totalTaxSales"></asp:Label> </td>
                                      
                                     <td style="display: none;"><asp:Label runat="server" Visible="false" style="display: none;" ID="QualitatLM"></asp:Label> </td>
                                      <td style="display: none;"><asp:Label runat="server" Visible="false" style="display: none;" ID="QualitatTaxLM"></asp:Label> </td>
                                      <td style="display: none;"><asp:Label runat="server" Visible="false" style="display: none;"  ID="CalidadTaxLM"></asp:Label> </td>
                                    </tr>
                                    
                                  </tbody>
                                  
                                </table>
                               
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    
                   
                </div>
                
            </div>
         
              
            <div id="Div2" runat="server" class="row">
                <div class="col-md-12">
                    <div class="col-md-4">
                        <div class="portlet box green">
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="fa fa-reorder"></i>Sales
                                </div>
                                <div class="tools">
                                    <a href="javascript:;" class="collapse"></a>
                                </div>
                            </div>
                            <div class="portlet-body">
                                <div class="form-horizontal">
                                    
                                    <div class="row">
                                        <div class="col-md-12">
                                            
                                <table class="table table-hover text-nowrap">
                                  <thead>
                                    <tr>
                                      <th>Basic Sale <asp:Label runat="server"  ID="lblLMName"></asp:Label></th>
                                      
                                      <th>Gross Sale <asp:Label runat="server"  ID="lblLMName1"></asp:Label></th>
                                      
                                    </tr>
                                  </thead>
                                 
                                  <tbody>
                                    <tr>
                                      <td><asp:Label runat="server"  ID="SPCLM"></asp:Label>  </td>
                                     <td><asp:Label runat="server"  ID="SPCTaxLM"></asp:Label>  </td>
                                        
                                    </tr>
                                    
                                  </tbody>
                                 
                                </table>
                               
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="portlet box green">
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="fa fa-reorder"></i>Sales
                                </div>
                                <div class="tools">
                                    <a href="javascript:;" class="collapse"></a>
                                </div>
                            </div>
                            <div class="portlet-body">
                                <div class="form-horizontal">
                                    
                                    <div class="row">
                                        <div class="col-md-12">
                                            
                                <table class="table table-hover text-nowrap">
                                  <thead>
                                    <tr>
                                      
                                      <th>Basic Sale <asp:Label runat="server"  ID="lblTMName"></asp:Label></th>
                                      
                                      <th>Gross Sale <asp:Label runat="server"  ID="lblTMName1"></asp:Label></th>
                                      
                                    </tr>
                                  </thead>
                                 
                                  <tbody>
                                    <tr>
                                    
                                        <td><asp:Label runat="server"  ID="SPCCM"></asp:Label>  </td>
                                      <td><asp:Label runat="server"  ID="SPCTaxCM"></asp:Label>  </td>
                                      
                                       
                                     
                                    </tr>
                                    
                                  </tbody>
                                 
                                </table>
                               
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
					<div class="col-md-4">
                        <div class="portlet box green">
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="fa fa-reorder"></i>Sales
                                </div>
                                <div class="tools">
                                    <a href="javascript:;" class="collapse"></a>
                                </div>
                            </div>
                            <div class="portlet-body">
                                <div class="form-horizontal">
                                    
                                    <div class="row">
                                        <div class="col-md-12">
                                            
                                <table class="table table-hover text-nowrap">
                                  <thead>
                                    <tr>
                                      
                                      
                                      <th>Basic Sale Diff</th>
                                      
                                      <th>Gross Sale Diff</th>
                                    </tr>
                                  </thead>
                                 
                                  <tbody>
                                    <tr>
                                    
                                     
                                      
                                       
                                      <td><asp:Label runat="server"  ID="SPCSalesDiff"></asp:Label>  </td>
                                      <td><asp:Label runat="server"  ID="SPCSalesDiffTax"></asp:Label>  </td>
                                    </tr>
                                    
                                  </tbody>
                                 
                                </table>
                               
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                   
                </div>
                
            </div>
            
           
            
            <br />
            <div id="Div3" runat="server" class="row">
                <div class="col-md-12">
                    <div class="col-md-3">
                        <div class="portlet box green">
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="fa fa-reorder"></i>Last 8 Days Sales <br />(<asp:Label ID="lbltotal8dayssale" style="color: white;" runat="server"></asp:Label>)
                                </div>
                                <div class="tools">
                                    <a href="javascript:;" class="collapse"></a>
                                </div>
                            </div>
                            <div class="portlet-body">
                                <div class="form-horizontal">
                                    
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Repeater ID="RepterDetails" runat="server"> 
                              <HeaderTemplate> 
                                <table class="table table-hover text-nowrap">
                                  <thead>
                                    <tr>
                                      <th>ID</th>
                                      
                                      <th>Date</th>
                                      <th>Amount</th>
                                      
                                    </tr>
                                  </thead>
                                  </HeaderTemplate> 
                                  <ItemTemplate> 
                                  <tbody>
                                    <tr>
                                      <td><%#Eval("RowNum")%></td>
                                      
                                      <td><%#Eval("INM_DATE")%></td>
                                      <td><%#Eval("GrandAmt")%></td>
                                    </tr>
                                    
                                  </tbody>
                                  </ItemTemplate> 
                                  <FooterTemplate> 
                                </table>
                                </FooterTemplate> 
                                </asp:Repeater>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    
                    <div class="col-md-3">
                        <div class="portlet box green">
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="fa fa-reorder"></i>Last 8 Days Purchase <br />(<asp:Label ID="lbltotal8daysPurchase" style="color: white;" runat="server"></asp:Label>)

                                </div>
                                <div class="tools">
                                    <a href="javascript:;" class="collapse"></a>
                                </div>
                            </div>
                            <div class="portlet-body">
                                <div class="form-horizontal">
                                    
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Repeater ID="Repeaterpurchase" runat="server"> 
                              <HeaderTemplate> 
                                <table class="table table-hover text-nowrap">
                                  <thead>
                                    <tr>
                                      <th>ID</th>
                                      
                                      <th>Date</th>
                                      <th>Amount</th>
                                      
                                    </tr>
                                  </thead>
                                  </HeaderTemplate> 
                                  <ItemTemplate> 
                                  <tbody>
                                    <tr>
                                      <td><%#Eval("RowNum")%></td>
                                      
                                      <td><%#Eval("INM_DATE")%></td>
                                      <td><%#Eval("GrandAmt")%></td>
                                    </tr>
                                    
                                  </tbody>
                                  </ItemTemplate> 
                                  <FooterTemplate> 
                                </table>
                                </FooterTemplate> 
                                </asp:Repeater>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    
                    
                    <div class="col-md-3">
                        <div class="portlet box green">
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="fa fa-reorder"></i>Last 8 Days RAW <br />(<asp:Label ID="lbltotal8daysRawPurchase" style="color: white;" runat="server"></asp:Label>)

                                </div>
                                <div class="tools">
                                    <a href="javascript:;" class="collapse"></a>
                                </div>
                            </div>
                            <div class="portlet-body">
                                <div class="form-horizontal">
                                    
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Repeater ID="Repeaterpurchaseraw" runat="server"> 
                              <HeaderTemplate> 
                                <table class="table table-hover text-nowrap">
                                  <thead>
                                    <tr>
                                      <th>ID</th>
                                      
                                      <th>Date</th>
                                      <th>Amount</th>
                                      
                                    </tr>
                                  </thead>
                                  </HeaderTemplate> 
                                  <ItemTemplate> 
                                  <tbody>
                                    <tr>
                                      <td><%#Eval("RowNum")%></td>
                                      
                                      <td><%#Eval("INM_DATE")%></td>
                                      <td><%#Eval("GrandAmt")%></td>
                                    </tr>
                                    
                                  </tbody>
                                  </ItemTemplate> 
                                  <FooterTemplate> 
                                </table>
                                </FooterTemplate> 
                                </asp:Repeater>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>    
                    
                    <div class="col-md-3">
                        <div class="portlet box green">
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="fa fa-reorder"></i>Last 8 Days BOUGHT OUT <br />(<asp:Label ID="lbltotal8daysBOUGHTOUTPurchase" style="color: white;" runat="server"></asp:Label>)

                                </div>
                                <div class="tools">
                                    <a href="javascript:;" class="collapse"></a>
                                </div>
                            </div>
                            <div class="portlet-body">
                                <div class="form-horizontal">
                                    
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Repeater ID="RepeaterBOUGHTOUTraw" runat="server"> 
                              <HeaderTemplate> 
                                <table class="table table-hover text-nowrap">
                                  <thead>
                                    <tr>
                                      <th>ID</th>
                                      
                                      <th>Date</th>
                                      <th>Amount</th>
                                      
                                    </tr>
                                  </thead>
                                  </HeaderTemplate> 
                                  <ItemTemplate> 
                                  <tbody>
                                    <tr>
                                      <td><%#Eval("RowNum")%></td>
                                      
                                      <td><%#Eval("INM_DATE")%></td>
                                      <td><%#Eval("GrandAmt")%></td>
                                    </tr>
                                    
                                  </tbody>
                                  </ItemTemplate> 
                                  <FooterTemplate> 
                                </table>
                                </FooterTemplate> 
                                </asp:Repeater>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div> 
                    <!-- END PAGE CONTENT-->
                </div>
                
            </div>
            <br />
            <div id="Div13" runat="server" class="row">
                <div class="col-md-12">
                <div class="col-md-4">
                        <div class="portlet box green">
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="fa fa-reorder"></i>Purchase Ratio <br />(<asp:Label ID="lbltotalSaleamout" style="color: white;" runat="server"></asp:Label>)

                                </div>
                                <div class="tools">
                                    <a href="javascript:;" class="collapse"></a>
                                </div>
                            </div>
                            <div class="portlet-body">
                                <div class="form-horizontal">
                                    
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Repeater ID="repeaterratio" runat="server"> 
                              <HeaderTemplate> 
                                <table class="table table-hover text-nowrap">
                                  <thead>
                                    <tr>
                                      <th>Po Type</th>
                                      
                                      <th>Purchase Amout</th>
                                      <th>Purchase Ratio</th>
                                      
                                    </tr>
                                  </thead>
                                  </HeaderTemplate> 
                                  <ItemTemplate> 
                                  <tbody>
                                    <tr>
                                      <td><%#Eval("potypename")%></td>
                                      
                                      <td><%#Eval("purchaseamt")%></td>
                                      <td><%#Eval("ratio")%></td>
                                    </tr>
                                    
                                  </tbody>
                                  </ItemTemplate> 
                                  <FooterTemplate> 
                                </table>
                                </FooterTemplate> 
                                </asp:Repeater>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                 
                 <div class="col-md-8" style="display:none">
                        <div class="portlet box green">
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="fa fa-reorder"></i>Yesterday's / Today's Sale <br />

                                </div>
                                <div class="tools">
                                    <a href="javascript:;" class="collapse"></a>
                                </div>
                            </div>
                            <div class="portlet-body">
                                <div class="form-horizontal">
                                    
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Repeater ID="repeateryesterdaydispatch" runat="server"> 
                              <HeaderTemplate> 
                                <table class="table table-hover text-nowrap">
                                  <thead>
                                    <tr>
                                      <th>Item Name</th>
                                      
                                      <th>Yesterdays Grand Dispatch</th>
                                      <th>Yesterdays Baisc Dispatch</th>
                                      <th>Todays Grand Dispatch</th>
                                      <th>Todays Baisc Dispatch</th>
                                      
                                    </tr>
                                  </thead>
                                  </HeaderTemplate> 
                                  <ItemTemplate> 
                                  <tbody>
                                    <tr>
                                      <td><%#Eval("iname")%></td>
                                      
                                      <td><%#Eval("yerterdayinvgrandamt")%></td>
                                      <td><%#Eval("yerterdayinvbaiscamt")%></td>
                                      <td><%#Eval("todayinvgrandamt")%></td>
                                      <td><%#Eval("todayinvbasicamt")%></td>
                                    </tr>
                                    
                                  </tbody>
                                  </ItemTemplate> 
                                  <FooterTemplate> 
                                </table>
                                </FooterTemplate> 
                                </asp:Repeater>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
             </div>
                
            </div>
            
            
            
            
            <div id="Div4" runat="server" class="row">
                <div class="col-md-12">
                    <div class="col-md-12">
                        <div class="portlet box green">
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="fa fa-reorder"></i>Schedule Compliance All
                                </div>
                                <div class="tools">
                                    <a href="javascript:;" class="collapse"></a>
                                </div>
                            </div>
                            <div class="portlet-body">
                                <div class="form-horizontal">
                                    
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Repeater ID="RepeaterShceduleCOmpliance" runat="server"> 
              <HeaderTemplate> 
                <table id="example1" class="table table-bordered table-striped">
                  <thead>
                  <tr>
                    <th>Item Name </th>
                    <th>Scheduled Qty</th>
                    <th>Invoice Qty</th>
                    <th>Schedule compliance %</th>
                    <th>Scheduled Qty In Values</th>
                    <th>Invoice Qty In Values</th>
                    
                  </tr>
                  </thead>
                  </HeaderTemplate> 
                  <ItemTemplate>
                  <tbody>
                  <tr>
                    <td><%#Eval("p_name")%></td>
                    <td><%#Eval("schdeuletQty")%>
                    </td>
                    <td><%#Eval("invoiceQty")%></td>
                    <td> <%#Eval("Schedulecompliance")%></td>
                    <td><%#Eval("schdeuletQtyInValue")%></td>
                    <td><%#Eval("invoiceQtyInvalue")%></td>
                    
                  </tr>
                  
                  </tbody>
                  </ItemTemplate>
                  <FooterTemplate> 
                  <tfoot>
                    <tr>
                      <th>Item Name </th>
                    <th>Scheduled Qty</th>
                    <th>Invoice Qty</th>
                    <th>Schedule compliance %</th>
                    <th>Scheduled Qty In Values</th>
                    <th>Invoice Qty In Values</th>
                    </tr>
                  </tfoot>
                </table>
                </FooterTemplate> 
                  </asp:Repeater>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    
                   
                </div>
                
            </div>
            <div id="Div5" runat="server" class="row">
                <div class="col-md-12">
                    <div class="col-md-12">
                        <div class="portlet box green">
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="fa fa-reorder"></i>Schedule Compliance 0- 25%
                                </div>
                                <div class="tools">
                                    <a href="javascript:;" class="collapse"></a>
                                </div>
                            </div>
                            <div class="portlet-body">
                                <div class="form-horizontal">
                                    
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Repeater ID="repeater025" runat="server"> 
              <HeaderTemplate> 
                <table id="example1" class="table table-bordered table-striped">
                  <thead>
                  <tr>
                    <th>Item  Name </th>
                    <th>Scheduled Qty</th>
                    <th>Invoice Qty</th>
                    <th>Schedule compliance %</th>
                    <th>Scheduled Qty In Values</th>
                    <th>Invoice Qty In Values</th>
                    
                  </tr>
                  </thead>
                  </HeaderTemplate> 
                  <ItemTemplate>
                  <tbody>
                  <tr>
                    <td><%#Eval("p_name")%></td>
                    <td><%#Eval("schdeuletQty")%>
                    </td>
                    <td><%#Eval("invoiceQty")%></td>
                    <td> <%#Eval("Schedulecompliance")%></td>
                    <td><%#Eval("schdeuletQtyInValue")%></td>
                    <td><%#Eval("invoiceQtyInvalue")%></td>
                    
                  </tr>
                  
                  </tbody>
                  </ItemTemplate>
                  <FooterTemplate> 
                  <tfoot>
                    <tr>
                      <th>Item Name </th>
                    <th>Scheduled Qty</th>
                    <th>Invoice Qty</th>
                    <th>Schedule compliance %</th>
                    <th>Scheduled Qty In Values</th>
                    <th>Invoice Qty In Values</th>
                    </tr>
                  </tfoot>
                </table>
                </FooterTemplate> 
                  </asp:Repeater>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    
                   
                </div>
                
            </div>
            <div id="Div6" runat="server" class="row">
                <div class="col-md-12">
                    <div class="col-md-12">
                        <div class="portlet box green">
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="fa fa-reorder"></i>Schedule Compliance 25- 50%
                                </div>
                                <div class="tools">
                                    <a href="javascript:;" class="collapse"></a>
                                </div>
                            </div>
                            <div class="portlet-body">
                                <div class="form-horizontal">
                                    
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Repeater ID="repeater050" runat="server"> 
              <HeaderTemplate> 
                <table id="example1" class="table table-bordered table-striped">
                  <thead>
                  <tr>
                    <th>Item  Name </th>
                    <th>Scheduled Qty</th>
                    <th>Invoice Qty</th>
                    <th>Schedule compliance %</th>
                    <th>Scheduled Qty In Values</th>
                    <th>Invoice Qty In Values</th>
                    
                  </tr>
                  </thead>
                  </HeaderTemplate> 
                  <ItemTemplate>
                  <tbody>
                  <tr>
                    <td><%#Eval("p_name")%></td>
                    <td><%#Eval("schdeuletQty")%>
                    </td>
                    <td><%#Eval("invoiceQty")%></td>
                    <td> <%#Eval("Schedulecompliance")%></td>
                    <td><%#Eval("schdeuletQtyInValue")%></td>
                    <td><%#Eval("invoiceQtyInvalue")%></td>
                    
                  </tr>
                  
                  </tbody>
                  </ItemTemplate>
                  <FooterTemplate> 
                  <tfoot>
                    <tr>
                      <th>Item  Name </th>
                    <th>Scheduled Qty</th>
                    <th>Invoice Qty</th>
                    <th>Schedule compliance %</th>
                    <th>Scheduled Qty In Values</th>
                    <th>Invoice Qty In Values</th>
                    </tr>
                  </tfoot>
                </table>
                </FooterTemplate> 
                  </asp:Repeater>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    
                   
                </div>
                
            </div>
            <div id="Div7" runat="server" class="row">
                <div class="col-md-12">
                    <div class="col-md-12">
                        <div class="portlet box green">
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="fa fa-reorder"></i>Schedule Compliance 50- 75%
                                </div>
                                <div class="tools">
                                    <a href="javascript:;" class="collapse"></a>
                                </div>
                            </div>
                            <div class="portlet-body">
                                <div class="form-horizontal">
                                    
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Repeater ID="repeater075" runat="server"> 
              <HeaderTemplate> 
                <table id="example1" class="table table-bordered table-striped">
                  <thead>
                  <tr>
                    <th>Item  Name </th>
                    <th>Scheduled Qty</th>
                    <th>Invoice Qty</th>
                    <th>Schedule compliance %</th>
                    <th>Scheduled Qty In Values</th>
                    <th>Invoice Qty In Values</th>
                    
                  </tr>
                  </thead>
                  </HeaderTemplate> 
                  <ItemTemplate>
                  <tbody>
                  <tr>
                    <td><%#Eval("p_name")%></td>
                    <td><%#Eval("schdeuletQty")%>
                    </td>
                    <td><%#Eval("invoiceQty")%></td>
                    <td> <%#Eval("Schedulecompliance")%></td>
                    <td><%#Eval("schdeuletQtyInValue")%></td>
                    <td><%#Eval("invoiceQtyInvalue")%></td>
                    
                  </tr>
                  
                  </tbody>
                  </ItemTemplate>
                  <FooterTemplate> 
                  <tfoot>
                    <tr>
                      <th>Item  Name </th>
                    <th>Scheduled Qty</th>
                    <th>Invoice Qty</th>
                    <th>Schedule compliance %</th>
                    <th>Scheduled Qty In Values</th>
                    <th>Invoice Qty In Values</th>
                    </tr>
                  </tfoot>
                </table>
                </FooterTemplate> 
                  </asp:Repeater>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    
                   
                </div>
                
            </div>
            <div id="Div8" runat="server" class="row">
                <div class="col-md-12">
                    <div class="col-md-12">
                        <div class="portlet box green">
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="fa fa-reorder"></i>Schedule Compliance 75- 100%
                                </div>
                                <div class="tools">
                                    <a href="javascript:;" class="collapse"></a>
                                </div>
                            </div>
                            <div class="portlet-body">
                                <div class="form-horizontal">
                                    
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Repeater ID="repeater100" runat="server"> 
              <HeaderTemplate> 
                <table id="example1" class="table table-bordered table-striped">
                  <thead>
                  <tr>
                    <th>Item  Name </th>
                    <th>Scheduled Qty</th>
                    <th>Invoice Qty</th>
                    <th>Schedule compliance %</th>
                    <th>Scheduled Qty In Values</th>
                    <th>Invoice Qty In Values</th>
                    
                  </tr>
                  </thead>
                  </HeaderTemplate> 
                  <ItemTemplate>
                  <tbody>
                  <tr>
                    <td><%#Eval("p_name")%></td>
                    <td><%#Eval("schdeuletQty")%>
                    </td>
                    <td><%#Eval("invoiceQty")%></td>
                    <td> <%#Eval("Schedulecompliance")%></td>
                    <td><%#Eval("schdeuletQtyInValue")%></td>
                    <td><%#Eval("invoiceQtyInvalue")%></td>
                    
                  </tr>
                  
                  </tbody>
                  </ItemTemplate>
                  <FooterTemplate> 
                  <tfoot>
                    <tr>
                      <th>Item  Name </th>
                    <th>Scheduled Qty</th>
                    <th>Invoice Qty</th>
                    <th>Schedule compliance %</th>
                    <th>Scheduled Qty In Values</th>
                    <th>Invoice Qty In Values</th>
                    </tr>
                  </tfoot>
                </table>
                </FooterTemplate> 
                  </asp:Repeater>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    
                   
                </div>
                
            </div>
           <div id="Div9" runat="server" class="row">
                <div class="col-md-12">
                    <div class="col-md-12">
                        <div class="portlet box green">
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="fa fa-reorder"></i>Schedule Compliance > 100%
                                </div>
                                <div class="tools">
                                    <a href="javascript:;" class="collapse"></a>
                                </div>
                            </div>
                            <div class="portlet-body">
                                <div class="form-horizontal">
                                    
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Repeater ID="repeaterG100" runat="server"> 
              <HeaderTemplate> 
                <table id="example1" class="table table-bordered table-striped">
                  <thead>
                  <tr>
                    <th>Item  Name </th>
                    <th>Scheduled Qty</th>
                    <th>Invoice Qty</th>
                    <th>Schedule compliance %</th>
                    <th>Scheduled Qty In Values</th>
                    <th>Invoice Qty In Values</th>
                    
                  </tr>
                  </thead>
                  </HeaderTemplate> 
                  <ItemTemplate>
                  <tbody>
                  <tr>
                    <td><%#Eval("p_name")%></td>
                    <td><%#Eval("schdeuletQty")%>
                    </td>
                    <td><%#Eval("invoiceQty")%></td>
                    <td> <%#Eval("Schedulecompliance")%></td>
                    <td><%#Eval("schdeuletQtyInValue")%></td>
                    <td><%#Eval("invoiceQtyInvalue")%></td>
                    
                  </tr>
                  
                  </tbody>
                  </ItemTemplate>
                  <FooterTemplate> 
                  <tfoot>
                    <tr>
                      <th>Item Name </th>
                    <th>Scheduled Qty</th>
                    <th>Invoice Qty</th>
                    <th>Schedule compliance %</th>
                    <th>Scheduled Qty In Values</th>
                    <th>Invoice Qty In Values</th>
                    </tr>
                  </tfoot>
                </table>
                </FooterTemplate> 
                  </asp:Repeater>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    
                   
                </div>
                
            </div>
            
            <br />
            <div id="Div1" runat="server" class="row">
                <div class="col-md-12">
                    <div class="col-md-12">
                        <div class="portlet box green">
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="fa fa-reorder"></i>Selected Item
                                </div>
                                <div class="tools">
                                    <a href="javascript:;" class="collapse"></a>
                                </div>
                            </div>
                            <div class="portlet-body">
                                <div class="form-horizontal">
                                    
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Repeater ID="RepeaterShceduleCOmplianceSelectItem" runat="server"> 
              <HeaderTemplate> 
                <table id="example1" class="table table-bordered table-striped">
                  <thead>
                  <tr>
                  <th>Sr No</th>
                    <th>Item Name </th>
                    <th>Scheduled Qty</th>
                    <th>Invoice Qty</th>
                    <th>Yesteday's Dispatch</th>
                    <th>Todays's Dispatch</th>
                    <th>Schedule compliance %</th>
                    <th>Scheduled Qty In Values</th>
                    <th>Invoice Qty In Values</th>
                    
                  </tr>
                  </thead>
                  </HeaderTemplate> 
                  <ItemTemplate>
                  <tbody>
                  <tr>
                  <td><%#Eval("SrNO")%></td>
                    <td><%#Eval("p_name")%></td>
                    <td><%#Eval("schdeuletQty")%>
                    </td>
                    <td><%#Eval("invoiceQty")%></td>
                     <td><%#Eval("YesterdayDispatch")%></td>
                      <td><%#Eval("todaysDispatch")%></td>
                    <td> <%#Eval("Schedulecompliance")%></td>
                    <td><%#Eval("schdeuletQtyInValue")%></td>
                    <td><%#Eval("invoiceQtyInvalue")%></td>
                    
                  </tr>
                  
                  </tbody>
                  </ItemTemplate>
                  <FooterTemplate> 
                  <tfoot>
                    <tr>
                      <th>Sr No</th>
                    <th>Item Name </th>
                    <th>Scheduled Qty</th>
                    <th>Invoice Qty</th>
                    <th>Yesteday's Dispatch</th>
                    <th>Todays's Dispatch</th>
                    <th>Schedule compliance %</th>
                    <th>Scheduled Qty In Values</th>
                    <th>Invoice Qty In Values</th>
                    </tr>
                  </tfoot>
                </table>
                </FooterTemplate> 
                  </asp:Repeater>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    
                   
                </div>
                
            </div>
            
            
            <div id="Div11" runat="server" style="display: none;" class="row">
                <div class="col-md-12">
                    <div class="col-md-12">
                        <div class="portlet box green">
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="fa fa-reorder"></i>Current Month Sale
                                </div>
                                <div class="tools">
                                    <a href="javascript:;" class="collapse"></a>
                                </div>
                            </div>
                            <div class="portlet-body">
                                <div class="form-horizontal">
                                    
                                    <div class="row">
                                        <div class="col-md-12">
                                           
                                <table class="table table-hover text-nowrap">
                                  <thead>
                                    <tr>
                                     
                                      
                                      <th>SPC Basic Sale TM</th>
                                       <th>SPC Sales With Tax TM</th>
                                      <th style="display: none;"></th>
                                      <th style="display: none;"></th>
                                    </tr>
                                  </thead>
                                  
                                  <tbody>
                                    <tr>
                                     
                                      
                                    
                                      <td style="display: none;"><asp:Label runat="server" Visible="false" style="display: none;" ID="QualitatCM"></asp:Label> </td>
                                      <td style="display: none;"><asp:Label runat="server" Visible="false" style="display: none;" ID="CalidadCM"></asp:Label> </td>
                                    </tr>
                                    
                                  </tbody>
                                  
                                </table>
                               
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                           
                                <table style="display: none;" class="table table-hover text-nowrap">
                                  <thead>
                                    <tr>
                                      <th style="display: none;"></th>
                                       <th style="display: none;"></th>
                                     
                                      <th style="display: none;"></th>
                                      <th style="display: none;"></th>
                                    </tr>
                                  </thead>
                                  
                                  <tbody>
                                    <tr>
                                      <td style="display: none;"><asp:Label runat="server" Visible="false" style="display: none;" ID="TotalTaxCM"></asp:Label> </td>
                                      
                                       <td style="display: none;"><asp:Label runat="server" Visible="false" style="display: none;" ID="totalCM"></asp:Label> </td>
                                      <td style="display: none;"><asp:Label runat="server" Visible="false" style="display: none;" ID="QualitatTaxCM"></asp:Label> </td>
                                      <td style="display: none;"><asp:Label runat="server" Visible="false" style="display: none;" ID="CalidadTaxCM"></asp:Label> </td>
                                    </tr>
                                    
                                  </tbody>
                                  
                                </table>
                               
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    
                   
                </div>
                
            </div>
             <br />
            
            
            <br />
            
            <div id="Div12" runat="server" style="display: none;" class="row">
                <div class="col-md-12">
                    <div class="col-md-12">
                        <div class="portlet box green">
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="fa fa-reorder"></i>Sale Diff
                                </div>
                                <div class="tools">
                                    <a href="javascript:;" class="collapse"></a>
                                </div>
                            </div>
                            <div class="portlet-body">
                                <div class="form-horizontal">
                                    
                                    <div class="row">
                                        <div class="col-md-12">
                                           
                                <table class="table table-hover text-nowrap">
                                  <thead>
                                    <tr>
                                      
                                      
                                      <th>SPC Basic Sale DIff</th>
                                      <th>SPC Sales With Tax DIff</th>
                                      <th style="display: none;"></th>
                                      <th style="display: none;"></th>
                                    </tr>
                                  </thead>
                                  
                                  <tbody>
                                    <tr>
                                   
                                    
                                      <td style="display: none;"><asp:Label runat="server" Visible="false" style="display: none;" ID="qualitatSalesDiff"></asp:Label> </td>
                                      <td style="display: none;"><asp:Label runat="server" Visible="false" style="display: none;" ID="CalidadSalesDiff"></asp:Label> </td>
                                    </tr>
                                    
                                  </tbody>
                                  
                                </table>
                               
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                           
                                <table style="display: none;" class="table table-hover text-nowrap">
                                  <thead>
                                    <tr>
                                      <th style="display: none;"></th>
                                      <th style="display: none;"></th>
                                      
                                      <th  style="display: none;"></th>
                                      <th style="display: none;"></th>
                                    </tr>
                                  </thead>
                                  
                                  <tbody>
                                    <tr>
                                      <td style="display: none;"><asp:Label runat="server" Visible="false" style="display: none;"  ID="TotalSalesDiffTax"></asp:Label> </td>
                                         <td style="display: none;"><asp:Label runat="server" Visible="false" style="display: none;" ID="basicSalesDiff"></asp:Label> </td>
                                      
                                      <td style="display: none;"><asp:Label runat="server" Visible="false" style="display: none;" ID="QualitatSalesDiffTax"></asp:Label> </td>
                                      <td style="display: none;"><asp:Label runat="server" Visible="false" style="display: none;" ID="CalidatSalesDiffTax"></asp:Label> </td>
                                    </tr>
                                    
                                  </tbody>
                                  
                                </table>
                               
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    
                   
                </div>
                
            </div>
        </div>
        <!-- END CONTENT -->
        <!-- BEGIN JAVASCRIPTS(Load javascripts at bottom, this will
    reduce page load time) -->
        <!-- BEGIN CORE PLUGINS -->

        <script src="../../assets/plugins/jquery-1.10.2.min.js" type="text/javascript"></script>

        <script src="../../assets/plugins/jquery-migrate-1.2.1.min.js" type="text/javascript"></script>

        <!-- IMPORTANT! Load jquery-ui-1.10.3.custom.min.js before bootstrap.min.js to fix
    bootstrap tooltip conflict with jquery ui tooltip -->

        <script src="../../assets/plugins/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>

        <script src="../../assets/plugins/bootstrap-hover-dropdown/twitter-bootstrap-hover-dropdown.min.js"
            type="text/javascript"></script>

        <script src="../../assets/plugins/jquery-slimscroll/jquery.slimscroll.min.js" type="text/javascript"></script>

        <script src="../../assets/plugins/jquery.blockui.min.js" type="text/javascript"></script>

        <script src="../../assets/plugins/jquery.cokie.min.js" type="text/javascript"></script>

        <script src="../../assets/plugins/uniform/jquery.uniform.min.js" type="text/javascript"></script>

        <!-- END CORE PLUGINS -->
        <!-- BEGIN PAGE LEVEL PLUGINS -->

        <script src="../../assets/plugins/jquery.pulsate.min.js" type="text/javascript"></script>

        <script src="../../assets/plugins/bootstrap-daterangepicker/moment.min.js" type="text/javascript"></script>

        <script src="../../assets/plugins/bootstrap-daterangepicker/daterangepicker.js" type="text/javascript"></script>

        <script src="../../assets/plugins/gritter/js/jquery.gritter.js" type="text/javascript"></script>

        <script src="../../assets/plugins/jquery-easy-pie-chart/jquery.easy-pie-chart.js"
            type="text/javascript"></script>

        <!-- END PAGE LEVEL PLUGINS -->
        <!-- BEGIN PAGE LEVEL SCRIPTS -->

        <script src="../../assets/scripts/app.js" type="text/javascript"></script>

        <!-- END PAGE LEVEL SCRIPTS -->
</asp:Content>
