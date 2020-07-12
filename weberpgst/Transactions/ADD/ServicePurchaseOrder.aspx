<%@ Page Title="Service Purchase Order" Language="C#" MasterPageFile="~/main.master"
    AutoEventWireup="true" CodeFile="ServicePurchaseOrder.aspx.cs" EnableViewState="true"
    Inherits="Transactions_ADD_ServicePurchaseOrder" %>

<%@ Register TagPrefix="Ajaxified" Assembly="Ajaxified" Namespace="Ajaxified" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
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

    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(evt, args) {
            jQuery("#<%=ddlSupplier.ClientID %>").select2();
            jQuery("#<%=ddlServiceCode.ClientID %>").select2();
            jQuery("#<%=ddlServiceName.ClientID %>").select2();
            jQuery("#<%=ddlPOType.ClientID %>").select2();
            jQuery("#<%=ddlProjectCode.ClientID %>").select2();
        });
    </script>

    <script type="text/javascript">
        function UploadFile(fileUpload) {
            if (fileUpload.value != '') {
                document.getElementById("<%=btnUpload.ClientID %>").click();
            }
        }
    </script>

    <script type="text/javascript">
        function oknumber(sender, e) {
            $find('ModalPopupPrintSelection').hide();
            __doPostBack('Button5', e);
        }
        function oncancel(sender, e) {
            $find('ModalPopupPrintSelection').hide();
            __doPostBack('Button6', e);
        }
        
    </script>

    <script type="text/javascript">
        function Showalert1() {
            $('#Avisos').fadeIn(6000)
            $('#Avisos').fadeOut(6000)
            $("#up").click();
        }
    </script>

    <script type="text/javascript">
        function Showalert() {
            $('#MSG').fadeIn(6000)
            $('#MSG').fadeOut(6000)
            $("#up").click();
        }
    </script>

    <div class="page-content-wrapper">
        <div class="page-content">
            <!-- BEGIN PAGE CONTENT-->
            <div class="row">
                <div class="col-md-1">
                </div>
                <div class="col-md-10">
                    <div id="Avisos">
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-1">
                </div>
                <div id="MSG" class="col-md-10">
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="PanelMsg" runat="server" Visible="false" Style="background-color: #feefb3;
                                height: 50px; width: 100%; margin-bottom: 10px; border: 1px solid #9f6000">
                                <div style="vertical-align: middle; margin-top: 10px;">
                                    <asp:Label ID="lblmsg" runat="server" Style="color: #9f6000; font-size: medium; font-weight: bold;
                                        margin-top: 50px; margin-left: 10px;"></asp:Label>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="row">
                <div class="col-md-1">
                </div>
                <div class="col-md-10">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Service Purchase Order
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="linkBtn" runat="server" CssClass="remove" OnClick="btnCancel_Click"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <ul class="nav nav-tabs">
                                <li class="active"><a href="#tab_1_1" data-toggle="tab">Service Po </a></li>
                                <li class=""><a href="#tab_2_2" data-toggle="tab">Term And Condition</a> </li>
                            </ul>
                            <div class="tab-content">
                                <!-- T1 Info Tab -->
                                <div class="tab-pane fade active in" id="tab_1_1">
                                    <div class="form-horizontal">
                                        <div class="form-body">
                                            <!--/row-->
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-2 control-label">
                                                            <span class="required">*</span> PO Type</label>
                                                        <div class="col-md-4">
                                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:DropDownList ID="ddlPOType" CssClass="select2" runat="server" MsgObrigatorio="Please Select PO Type"
                                                                        Width="100%" TabIndex="1" OnSelectedIndexChanged="ddlPOType_SelectedIndexChanged"
                                                                        AutoPostBack="true">
                                                                    </asp:DropDownList>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <label class="col-md-2 control-label">
                                                            Po No.
                                                        </label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <asp:TextBox CssClass="form-control" ID="txtPoNo" TabIndex="2212" runat="server"
                                                                    Enabled="false" TextMode="SingleLine"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-2 control-label">
                                                            <span class="required">*</span> Supplier</label>
                                                        <div class="col-md-4">
                                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:DropDownList ID="ddlSupplier" CssClass="select2" Width="350px" runat="server"
                                                                        MsgObrigatorio="Please Select Service" TabIndex="2" OnSelectedIndexChanged="ddlSupplier_SelectedIndexChanged"
                                                                        AutoPostBack="true">
                                                                    </asp:DropDownList>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <label class="col-md-2 control-label">
                                                            <span class="required">*</span> PO Date
                                                        </label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <asp:TextBox CssClass="form-control" placeholder="dd MMM yyyy" ID="txtPoDate" TabIndex="3"
                                                                    runat="server" MsgObrigatorio="Please Enter Po Date" TextMode="SingleLine"></asp:TextBox>
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <cc1:CalendarExtender ID="txtDate_CalendarExtender2" runat="server" Enabled="True"
                                                                    Format="dd MMM yyyy" TargetControlID="txtPoDate">
                                                                </cc1:CalendarExtender>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-2 control-label">
                                                            Quotation Ref.</label>
                                                        <div class="col-md-4">
                                                            <asp:TextBox CssClass="form-control" Style="text-transform: uppercase" placeholder="Quotation Ref"
                                                                ID="txtServiceRef" TabIndex="4" runat="server" TextMode="SingleLine"></asp:TextBox>
                                                        </div>
                                                        <label class="col-md-2 control-label text-right">
                                                            Quotation Ref Date
                                                        </label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <asp:TextBox CssClass="form-control " placeholder="dd MMM yyyy" ID="txtServiceRefDate"
                                                                    TabIndex="5" runat="server" TextMode="SingleLine"></asp:TextBox>
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd MMM yyyy"
                                                                    TargetControlID="txtServiceRefDate">
                                                                </cc1:CalendarExtender>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-2 control-label">
                                                            Project Code</label>
                                                        <div class="col-md-4">
                                                            <asp:UpdatePanel ID="UpdatePanel34" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:DropDownList ID="ddlProjectCode" MsgObrigatorio="Project Code" Width="100%"
                                                                        AutoPostBack="true" CssClass="select2" TabIndex="6" runat="server">
                                                                    </asp:DropDownList>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <label class="col-md-2 control-label text-right">
                                                            Valid Upto
                                                        </label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <asp:TextBox CssClass="form-control " placeholder="dd MMM yyyy" ID="txtValid" TabIndex="7"
                                                                    runat="server" TextMode="SingleLine"></asp:TextBox>
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" Format="dd MMM yyyy"
                                                                    TargetControlID="txtValid">
                                                                </cc1:CalendarExtender>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-2 control-label">
                                                            Project Name</label>
                                                        <div class="col-md-4">
                                                            <asp:TextBox CssClass="form-control" placeholder="Project Name" Style="text-transform: uppercase"
                                                                ID="txtProjName" MaxLength="50" TabIndex="8" runat="server" TextMode="SingleLine"></asp:TextBox>
                                                        </div>
                                                        <label class="col-md-2 control-label text-right">
                                                            <%--Valid Upto--%>
                                                        </label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div runat="server" id="divCurrancy" visible="false">
                                                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Panel runat="server" ID="pnlCurrancy" Visible="false">
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <div class="form-group">
                                                                        <label class="col-md-2 control-label label-sm">
                                                                            Currency
                                                                        </label>
                                                                        <div class="col-md-3">
                                                                            <asp:UpdatePanel ID="UpdatePanel32" runat="server">
                                                                                <ContentTemplate>
                                                                                    <asp:DropDownList ID="ddlCurrency" MsgObrigatorio="Currancy" TabIndex="9" AutoPostBack="true"
                                                                                        OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged" CssClass="select2_category form-control"
                                                                                        runat="server">
                                                                                    </asp:DropDownList>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </div>
                                                                        <label class="col-md-3 control-label label-sm">
                                                                            Currency Rate
                                                                        </label>
                                                                        <div class="col-md-3">
                                                                            <asp:UpdatePanel ID="UpdatePanel35" runat="server">
                                                                                <ContentTemplate>
                                                                                    <asp:TextBox CssClass="form-control text-right input-sm" TabIndex="10" ID="txtCurrencyRate"
                                                                                        placeholder="0.00" runat="server"></asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" TargetControlID="txtCurrencyRate"
                                                                                        ValidChars="0123456789." runat="server" />
                                                                                </ContentTemplate>
                                                                                <Triggers>
                                                                                    <asp:AsyncPostBackTrigger ControlID="ddlCurrency" EventName="SelectedIndexChanged" />
                                                                                </Triggers>
                                                                            </asp:UpdatePanel>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </asp:Panel>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <hr />
                                        </div>
                                    </div>
                                    <div class="horizontal-form">
                                        <div class="form-body">
                                            <div class="row">
                                                <div class="col-md-1">
                                                </div>
                                                <div class="form-group">
                                                    <div class="col-md-3">
                                                        <label class="control-label label-sm">
                                                            <span class="required">*</span> Service Code</label>
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlServiceCode" CssClass="select2" Width="100%" runat="server"
                                                                    TabIndex="11" OnSelectedIndexChanged="ddlServiceCode_SelectedIndexChanged" MsgObrigatorio="Please Select Service Code"
                                                                    AutoPostBack="True">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlServiceName" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgServicePurchaseOrder" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <%-- <!--/span-->--%>
                                                    <div class="col-md-5">
                                                        <label class="control-label label-sm">
                                                            <span class="required">*</span> Service Name</label>
                                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlServiceName" CssClass="select2" Width="100%" TabIndex="12"
                                                                    runat="server" MsgObrigatorio="Please Select Service Name" AutoPostBack="True"
                                                                    OnSelectedIndexChanged="ddlServiceName_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlServiceCode" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgServicePurchaseOrder" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <!--/span-->
                                                    <div class="col-md-2">
                                                        <label class="control-label label-sm" style="display: none">
                                                            Stock Unit</label>
                                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlStockUOM" Visible="false" CssClass="select2_category form-control input-sm"
                                                                    runat="server" TabIndex="13" Enabled="false" OnSelectedIndexChanged="ddlStockUOM_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlServiceCode" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="ddlServiceName" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgServicePurchaseOrder" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <!--/span-->
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-1">
                                                </div>
                                                <div class="form-group">
                                                    <div class="col-md-2">
                                                        <label class="control-label label-sm">
                                                            <aspn class="required">*</aspn>
                                                            Order Qty</label>
                                                        <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right input-sm" ID="txtOrderQty" placeholder="0.000"
                                                                    TabIndex="14" runat="server" MaxLength="20" AutoPostBack="true" OnTextChanged="txtOrderQty_TextChanged">
                                                                </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtOrderQty"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgServicePurchaseOrder" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <label class="control-label label-sm">
                                                            <span class="required">*</span> Rate</label>
                                                        <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right input-sm" ID="txtRate" placeholder="0.00"
                                                                    TabIndex="15" MsgObrigatorio="Please Enter Rate" MaxLength="20" runat="server"
                                                                    AutoPostBack="true" OnTextChanged="txtRate_TextChanged">
                                                                </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="txtRate"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlServiceCode" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="ddlServiceName" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgServicePurchaseOrder" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <!--/span-->
                                                    <div class="col-md-2">
                                                        <label class="control-label label-sm" style="display: none">
                                                            Rate Unit</label>
                                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlRateUOM" Visible="false" CssClass="select2_category form-control input-sm"
                                                                    placeholder="" runat="server" TabIndex="16" MsgObrigatorio="Please Select Item Name"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlRateUOM_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <label class="control-label label-sm">
                                                            Total Amount</label>
                                                        <asp:UpdatePanel ID="UpdatePanel13" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right input-sm" ID="txtTotalAmount" placeholder="0.00"
                                                                    TabIndex="17" ReadOnly="true" runat="server"></asp:TextBox>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="txtOrderQty" EventName="TextChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="txtRate" EventName="TextChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="ddlServiceCode" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="ddlServiceName" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgServicePurchaseOrder" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <label class="control-label label-sm" style="display: none">
                                                                Conv.Ratio</label>
                                                            <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control text-right input-sm" Visible="false" ID="txtConversionRetio"
                                                                        placeholder="0" TabIndex="18" runat="server" AutoPostBack="True" OnTextChanged="txtConversionRetio_TextChanged"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" TargetControlID="txtConversionRetio"
                                                                        ValidChars="0123456789." runat="server" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                    <!--/span-->
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-1">
                                                </div>
                                                <div class="col-md-2">
                                                    <label class="control-label label-sm">
                                                        Disc.Per</label>
                                                    <asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtDescPerc" placeholder="0.00"
                                                                TabIndex="19" runat="server" AutoPostBack="true" OnTextChanged="txtDescPerc_TextChanged"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" TargetControlID="txtDescPerc"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="txtDescAmount" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgServicePurchaseOrder" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <label class="control-label label-sm">
                                                            Disc Amount
                                                        </label>
                                                        <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right input-sm" ID="txtDescAmount" placeholder="0.00"
                                                                    TabIndex="20" runat="server" Enabled="false" AutoPostBack="true"></asp:TextBox>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="txtDescPerc" EventName="TextChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="txtOrderQty" EventName="TextChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="txtRate" EventName="TextChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgServicePurchaseOrder" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <!--/span-->
                                                <div class="col-md-1">
                                                    <%--<div class="form-group">--%>
                                                    <label class="control-label  label-sm" style="display: none">
                                                        Exc.Inclusive</label>
                                                    <asp:UpdatePanel ID="UpdatePanel27" runat="server">
                                                        <ContentTemplate>
                                                            <label class="checkbox">
                                                                <asp:CheckBox ID="chkExcInclusive" Visible="false" runat="server" Text="" CssClass="checker"
                                                                    AutoPostBack="True" TabIndex="21" OnCheckedChanged="chkExcInclusive_CheckedChanged" />
                                                            </label>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgServicePurchaseOrder" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                    <%--</div>--%>
                                                </div>
                                                <div class="col-md-1">
                                                    <label class="control-label  label-sm" style="display: none">
                                                        Exc.Per</label>
                                                    <asp:UpdatePanel ID="UpdatePanel37" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control" Visible="false" ID="txtExcisePer" placeholder="0.00"
                                                                TabIndex="22" runat="server"></asp:TextBox>
                                                            </label>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgServicePurchaseOrder" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <!--/span-->
                                                <div class="col-md-3">
                                                    <div class="form-group">
                                                        <label class="control-label label-sm">
                                                            <span class="required">*</span> SAC Code</label><br />
                                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlGSTTax" CssClass="select2_category form-control input-sm"
                                                                    runat="server" TabIndex="23" Enabled="false" MsgObrigatorio="Please Select SAC Code">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="ddlServiceName" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="ddlServiceCode" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgServicePurchaseOrder" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <label class="control-label label-sm">
                                                        <%-- Active Ind--%></label><br />
                                                    <asp:UpdatePanel ID="UpdatePanel30" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <label class="checkbox">
                                                                <asp:CheckBox ID="chkActiveInd" runat="server" Text="" CssClass="checker" AutoPostBack="True"
                                                                    TabIndex="24" Checked="true" Visible="false" />
                                                            </label>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgServicePurchaseOrder" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-1">
                                                </div>
                                                <div class="form-group">
                                                    <div class="col-md-4">
                                                        <label class="control-label label-sm">
                                                            Specification</label><br />
                                                        <asp:UpdatePanel ID="UpdatePanel22" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control" ID="txtSpecification" placeholder="Specification"
                                                                    TabIndex="25" runat="server"></asp:TextBox>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgServicePurchaseOrder" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label class="control-label label-sm" style="display: none">
                                                            Attach Document
                                                        </label>
                                                        <asp:FileUpload ID="imgUpload" runat="server" TabIndex="22" CssClass="btn blue" Visible="false" />
                                                        <asp:Button ID="btnUpload" Text="Upload" runat="server" TabIndex="26" OnClick="Upload"
                                                            Style="display: none" />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <label class="control-label label-sm">
                                                            <%-- Miss. Item Name--%></label><br />
                                                        <asp:UpdatePanel ID="UpdatePanel31" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control" ID="txtMissItemName" placeholder="Miss. Item Name"
                                                                    TabIndex="27" Visible="false" runat="server" MaxLength="100">0</asp:TextBox>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgServicePurchaseOrder" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-1">
                                                    </div>
                                                    <div class="col-md-1">
                                                        <label class="control-label label-sm">
                                                            &nbsp</label><br />
                                                        <asp:LinkButton ID="btnInsert" CssClass="btn blue" TabIndex="28" OnClick="btnInsert_Click"
                                                            runat="server">
                                                    <i class="fa fa-arrow-circle-down" > Insert </i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                            <br />
                                            <%-- Start Comment --%>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="control-group">
                                                        <div class="col-md-12" style="overflow-x: auto;">
                                                            <asp:UpdatePanel ID="UpdatePanel26" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:GridView ID="dgServicePurchaseOrder" runat="server" TabIndex="29" Style="width: 100%;"
                                                                        AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-advance table-hover"
                                                                        CellPadding="4" GridLines="Both" OnRowCommand="dgServicePurchaseOrder_RowCommand"
                                                                        OnRowDeleting="dgServicePurchaseOrder_RowDeleting">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Modify" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                                                HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton ID="lnkModify" BorderStyle="None" runat="server" CssClass="btn green btn-xs"
                                                                                        CausesValidation="False" CommandName="Modify" Text="Modify" CommandArgument='<%# ((GridViewRow)Container).RowIndex%>'></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Delete" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                                                HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton ID="lnkDelete" BorderStyle="None" runat="server" CssClass="btn red btn-xs"
                                                                                        CausesValidation="False" CommandName="Delete" CommandArgument='<%# ((GridViewRow)Container).RowIndex%>'
                                                                                        Text="Delete"></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Service Code" SortExpression="ItemCode">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblServiceCode" runat="server" Text='<%# Bind("ServiceCode") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Item Code" SortExpression="ItemCode" Visible="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblServiceCode1" runat="server" Text='<%# Bind("ServiceCode1") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Service Name" SortExpression="ItemCode" Visible="True">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblServiceName" runat="server" Text='<%# Bind("ServiceName") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Order Qty" SortExpression="POCODE" Visible="True">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblOrderQty" runat="server" Text='<%# Bind("OrderQty") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Rate" SortExpression="PO No" Visible="True">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblRate" runat="server" Text='<%# Bind("Rate") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Total Amount" SortExpression="Inv.Qty">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblTotalAmount" runat="server" Text='<%# Bind("TotalAmount") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Disc Perc" SortExpression="Act. Wght.">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblDiscPerc" runat="server" Text='<%# Bind("DiscPerc") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Disc Amount" SortExpression="Rate" HeaderStyle-HorizontalAlign="Left"
                                                                                HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblDiscAmount" runat="server" Text='<%# Eval("DiscAmount") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" CssClass="text-right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="SAC Code" SortExpression="NoOfPackes" Visible="True"
                                                                                HeaderStyle-HorizontalAlign="Left">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblSalesTax" runat="server" Text='<%# Eval("SalesTax") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="SAC Code" SortExpression="NoOfPackes" Visible="false"
                                                                                HeaderStyle-HorizontalAlign="Left">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblSalesTax1" runat="server" Text='<%# Eval("SalesTax1") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Specification" SortExpression="NoOfPackes" Visible="True"
                                                                                HeaderStyle-HorizontalAlign="Left">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblSpecification" runat="server" Text='<%# Eval("Specification") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Central Tax" SortExpression="E_BASIC" HeaderStyle-HorizontalAlign="Left"
                                                                                Visible="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblE_BASIC" runat="server" Text='<%# Eval("E_BASIC") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="State/Union Territory Tax" SortExpression="E_EDU_CESS"
                                                                                HeaderStyle-HorizontalAlign="Left" Visible="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblE_EDU_CESS" runat="server" Text='<%# Eval("E_EDU_CESS") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Integrated Tax" SortExpression="E_H_EDU" HeaderStyle-HorizontalAlign="Left"
                                                                                Visible="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblE_H_EDU" runat="server" Text='<%# Eval("E_H_EDU") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="CGST Tax" SortExpression="ExisDuty" Visible="true"
                                                                                HeaderStyle-HorizontalAlign="Left">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblExisDuty" runat="server" Text='<%# Eval("ExisDuty") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="SGST tax" SortExpression="EduCess" Visible="true"
                                                                                HeaderStyle-HorizontalAlign="Left">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblEduCess" runat="server" Text='<%# Eval("EduCess") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="IGST Tax" SortExpression="SHECess" Visible="true"
                                                                                HeaderStyle-HorizontalAlign="Left">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblSHECess" runat="server" Text='<%# Eval("SHECess") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Document View" HeaderStyle-HorizontalAlign="Left"
                                                                                HeaderStyle-Width="35px" HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton ID="lnkView" BorderStyle="None" runat="server" CausesValidation="False"
                                                                                        CommandName="ViewPDF" Text='<%# Eval("DocName") %>' CommandArgument='<%#Container.DataItemIndex%>'></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <AlternatingRowStyle CssClass="alt" />
                                                                        <PagerStyle CssClass="pgr" />
                                                                    </asp:GridView>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                    <asp:AsyncPostBackTrigger ControlID="ddlSupplier" EventName="SelectedIndexChanged" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <%--End Comment--%>
                                            <%-- Start Comment --%>
                                            <br />
                                            <div class="row">
                                                <div class="col-md-7">
                                                </div>
                                                <label class="col-md-2 control-label">
                                                    Total Amount
                                                </label>
                                                <div class="col-md-3">
                                                    <div class="form-group">
                                                        <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right input-sm" ID="txtFinalTotalAmount"
                                                                    placeholder="" TabIndex="30" Enabled="false" runat="server"></asp:TextBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-7">
                                                </div>
                                                <label class="col-md-2 control-label">
                                                    CGST Tax
                                                </label>
                                                <div class="col-md-3">
                                                    <div class="form-group">
                                                        <asp:UpdatePanel ID="UpdatePanel36" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right input-sm" ID="txtCGSTTax" placeholder=""
                                                                    TabIndex="31" Enabled="false" runat="server"></asp:TextBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-7">
                                                </div>
                                                <label class="col-md-2 control-label">
                                                    SGST Tax
                                                </label>
                                                <div class="col-md-3">
                                                    <div class="form-group">
                                                        <asp:UpdatePanel ID="UpdatePanel38" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right input-sm" ID="txtSGSTTax" placeholder=""
                                                                    TabIndex="32" Enabled="false" runat="server"></asp:TextBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-7">
                                                </div>
                                                <label class="col-md-2 control-label">
                                                    IGST Tax
                                                </label>
                                                <div class="col-md-3">
                                                    <div class="form-group">
                                                        <asp:UpdatePanel ID="UpdatePanel39" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right input-sm" ID="txtIGSTTax" placeholder=""
                                                                    TabIndex="34" Enabled="false" runat="server"></asp:TextBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-7">
                                                </div>
                                                <label class="col-md-2 control-label">
                                                    Net Total Amount
                                                </label>
                                                <div class="col-md-3">
                                                    <div class="form-group">
                                                        <asp:UpdatePanel ID="UpdatePanel40" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right input-sm" ID="txtNetTotal" placeholder=""
                                                                    TabIndex="35" Enabled="false" runat="server"></asp:TextBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                            <h4 class="form-section">
                                                <%-- Attach Documents--%></h4>
                                            <div id="Div1" class="row" runat="server" visible="false">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-2 control-label">
                                                            Attach Document
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:LinkButton ID="btnAddDoc" CssClass="btn blue" TabIndex="36" runat="server" OnClick="btnAddDoc_Click"
                                                                Visible="True"><i class="fa fa-plus"></i> Add Document</asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <br />
                                            <div id="Div2" class="row" runat="server" visible="false">
                                                <div class="col-md-12">
                                                    <div class="control-group">
                                                        <div class="col-md-12" style="overflow-x: auto;">
                                                            <asp:GridView ID="dgDocView" runat="server" TabIndex="37" Style="width: 100%;" AutoGenerateColumns="False"
                                                                CssClass="table table-striped table-bordered table-advance table-hover" CellPadding="4"
                                                                GridLines="Both" OnRowDeleting="dgDocView_RowDeleting" OnRowCommand="dgDocView_RowCommand"
                                                                OnRowUpdating="dgDocView_RowUpdating">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Item Code" SortExpression="SPOA_SRPOM_CODE" Visible="False">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSPOA_SRPOM_CODE" runat="server" Text='<%# Bind("SPOA_SRPOM_CODE") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Browse">
                                                                        <ItemTemplate>
                                                                            <asp:FileUpload ID="imgUpload" runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="File Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblfilename" CssClass="" runat="server" Text='<%# Bind("SPOA_DOC_NAME") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="File Name" SortExpression="SPOA_DOC_NAME" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSPOA_DOC_NAME" runat="server" Text='<%# Bind("SPOA_DOC_NAME") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Upload">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="btnupload" runat="server" CommandArgument='<%#Container.DataItemIndex%>'
                                                                                CommandName="Update" Text="Upload" CssClass="btn green btn-xs" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Download" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                                        HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkDownload" BorderStyle="None" runat="server" CssClass="btn blue btn-xs"
                                                                                CausesValidation="False" CommandName="Download" Text="Download" CommandArgument='<%#Container.DataItemIndex%>'
                                                                                OnClick="lnkDownload_Click"></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Document View" HeaderStyle-HorizontalAlign="Left"
                                                                        HeaderStyle-Width="35px" HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkView" BorderStyle="None" runat="server" CssClass="btn green btn-xs"
                                                                                CausesValidation="False" CommandName="View" Text="View" CommandArgument='<%#Container.DataItemIndex%>'></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Item Code" SortExpression="SPOA_DOC_PATH" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSPOA_DOC_PATH" runat="server" Text='<%# Bind("SPOA_DOC_PATH") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Delete" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                                        HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkDelete" BorderStyle="None" runat="server" CssClass="btn red btn-xs"
                                                                                CausesValidation="False" CommandName="Delete" CommandArgument='<%#Container.DataItemIndex%>'
                                                                                Text="Delete"></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <AlternatingRowStyle CssClass="alt" />
                                                                <PagerStyle CssClass="pgr" />
                                                            </asp:GridView>
                                                            <asp:Panel ID="Panel2" runat="server">
                                                            </asp:Panel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <%--End Comment--%>
                                        </div>
                                    </div>
                                </div>
                                <div class="tab-pane fade " id="tab_2_2">
                                    <div class="form-horizontal">
                                        <div class="form-body">
                                            <!--row-->
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label label-sm">
                                                            Pre Dispatch Terms
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control" ID="txtDeliverySchedule" placeholder=" Delivery at"
                                                                        TabIndex="38" runat="server" Text="INSPECTION AT OUR END"></asp:TextBox>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <label class="col-md-2 control-label label-sm">
                                                            Delivery Inst
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control" ID="txtTranspoter" placeholder=" Delivery" TabIndex="39"
                                                                        runat="server" Text="DELIVERY AT HEIMATEC INDIA PRIVATE LTD"></asp:TextBox>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--/row-->
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label label-sm">
                                                            <span class="required">*</span>Payment Term
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control" ID="txtPaymentTerm" placeholder="Payment Term"
                                                                        TabIndex="40" runat="server" TextMode="SingleLine"></asp:TextBox>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <label class="col-md-2 control-label label-sm">
                                                            <%-- Special Clause 1--%>
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:UpdatePanel ID="UpdatePanel19" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox Visible="false" CssClass="form-control input-sm" ID="txtDeliveryTo"
                                                                        placeholder="Special Clause 1" TabIndex="41" runat="server"></asp:TextBox>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div id="Div3" class="row" runat="server" visible="false">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label label-sm">
                                                            Freight Terms
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control" ID="txtFreightTermsg" placeholder="Freight Terms"
                                                                        TabIndex="42" runat="server" TextMode="SingleLine">
                                                                    </asp:TextBox>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <label class="col-md-2 control-label label-sm">
                                                            Octroi
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control" ID="txtCIF" placeholder="Octroi" TabIndex="43"
                                                                        runat="server" TextMode="SingleLine">
                                                                    </asp:TextBox>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label label-sm">
                                                            Special Inst
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:UpdatePanel ID="UpdatePanel24" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtGuranteeWaranty" placeholder=" Special Remark"
                                                                        TabIndex="44" runat="server"></asp:TextBox>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <label class="col-md-2 control-label label-sm">
                                                            <%-- Additional Specification--%>
                                                        </label>
                                                        <div class="col-md-3">
                                                            <asp:UpdatePanel ID="UpdatePanel25" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox Visible="false" CssClass="form-control input-sm" ID="txtPacking" placeholder="Additional Specification"
                                                                        TabIndex="45" runat="server"></asp:TextBox>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label label-sm">
                                                            Note
                                                        </label>
                                                        <div class="col-md-8">
                                                            <asp:UpdatePanel ID="UpdatePanel29" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtNote" placeholder="Note" TabIndex="46"
                                                                        runat="server" TextMode="MultiLine" Rows="6"></asp:TextBox>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--/row-->
                                        </div>
                                    </div>
                                </div>
                                <div class="form-actions fluid">
                                    <div class="col-md-offset-4 col-md-9">
                                        <asp:LinkButton ID="btnSubmit" CssClass="btn green" TabIndex="47" runat="server"
                                            OnClick="btnSubmit_Click"><i class="fa fa-check-square"> </i> Save </asp:LinkButton>
                                        <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="48" runat="server"
                                            OnClick="btnCancel_Click"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                            <asp:UpdatePanel runat="server" ID="UpdatePanel28">
                                <ContentTemplate>
                                    <asp:LinkButton ID="CheckCondition" runat="server" BackColor="" CssClass="formlabel"></asp:LinkButton>
                                    <cc1:ModalPopupExtender runat="server" ID="ModalPopupPrintSelection" BackgroundCssClass="modalBackground"
                                        OnOkScript="oknumber()" OnCancelScript="oncancel()" DynamicServicePath="" Enabled="True"
                                        PopupControlID="popUpPanel5" TargetControlID="CheckCondition">
                                    </cc1:ModalPopupExtender>
                                    <asp:Panel ID="popUpPanel5" runat="server" Style="display: none;">
                                        <div class="portlet box blue">
                                            <div class="portlet-title">
                                                <div class="captionPopup">
                                                    Alert
                                                </div>
                                            </div>
                                            <div class="portlet-body form">
                                                <div class="form-horizontal">
                                                    <div class="form-body">
                                                        <div class="row">
                                                            <label class="col-md-12 control-label">
                                                                Do you want to cancel record ?
                                                            </label>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-offset-3 col-md-9">
                                                                <asp:LinkButton ID="Button5" CssClass="btn blue" TabIndex="26" runat="server" Visible="true"
                                                                    OnClick="btnOk_Click">  Yes </asp:LinkButton>
                                                                <asp:LinkButton ID="Button6" CssClass="btn default" TabIndex="28" runat="server"
                                                                    OnClick="btnCancel1_Click"> No</asp:LinkButton>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:UpdatePanel runat="server" ID="UpdatePanel33">
                                <ContentTemplate>
                                    <asp:LinkButton ID="LnkDoc" runat="server" BackColor="" CssClass="formlabel"></asp:LinkButton>
                                    <cc1:ModalPopupExtender runat="server" ID="ModalPopDocument" BackgroundCssClass="modalBackground"
                                        OnOkScript="oknumber()" OnCancelScript="oncancel()" DynamicServicePath="" Enabled="True"
                                        PopupControlID="PopDocument" TargetControlID="LnkDoc">
                                    </cc1:ModalPopupExtender>
                                    <asp:Panel ID="PopDocument" runat="server" Style="display: none;">
                                        <div class="portlet box blue">
                                            <div class="portlet-title">
                                                <div class="captionPopup">
                                                    Document View
                                                </div>
                                            </div>
                                            <div class="portlet-body form">
                                                <div class="form-horizontal">
                                                    <div class="form-body">
                                                        <div class="row">
                                                            <label class="col-md-12 control-label">
                                                            </label>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <iframe runat="server" id="IframeViewPDF" width="900px" height="600px"></iframe>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-offset-5 col-md-9">
                                                                <asp:LinkButton ID="btnOk" CssClass="btn green" TabIndex="28" runat="server" OnClick="btnCancel1_Click"> Ok</asp:LinkButton>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- END PAGE CONTENT-->
    </div>
    <!-- BEGIN JAVASCRIPTS(Load javascripts at bottom, this will reduce page load time) -->
    <!-- BEGIN CORE PLUGINS -->
    <link href="../../assets/Avisos/Avisos.css" rel="stylesheet" type="text/css" />

    <script src="../../assets/Avisos/Avisos.js" type="text/javascript"></script>

    <script src="../../assets/JS/Util.js" type="text/javascript"></script>

    <script src="../../assets/scripts/jquery-1.7.1.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery-migrate-1.2.1.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-hover-dropdown/twitter-bootstrap-hover-dropdown.min.js"
        type="text/javascript"></script>

    <script src="../../assets/plugins/jquery-slimscroll/jquery.slimscroll.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery.blockui.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery.cokie.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/uniform/jquery.uniform.min.js" type="text/javascript"></script>

    <!-- END CORE PLUGINS -->
    <!-- BEGIN PAGE LEVEL PLUGINS -->

    <script src="../../assets/plugins/bootstrap-daterangepicker/moment.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-daterangepicker/daterangepicker.js" type="text/javascript"></script>

    <script src="../../assets/plugins/gritter/js/jquery.gritter.js" type="text/javascript"></script>

    <script src="../../assets/plugins/select2/select2.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->

    <script type="text/javascript">
    function VerificaCamposObrigatorios() { try { if (VerificaValorCombo('#<%=ddlPOType.ClientID%>',
    '#Avisos') == false) { $("#Avisos").fadeOut(6000); $("#up").click(); return false; } else if (VerificaObrigatorio('#<%=txtPoDate.ClientID%>',
    '#Avisos') == false) { $("#Avisos").fadeOut(6000); $("#up").click(); return false; } else if (VerificaValorCombo('#<%=ddlSupplier.ClientID%>',
    '#Avisos') == false) { return false; } // else if (VerificaValorCombo('#<%=ddlServiceCode.ClientID%>',
    '#Avisos') == false) { // return false; // } // else if (VerificaValorCombo('#<%=ddlServiceName.ClientID%>',
    '#Avisos') == false) { // return false; // } // else if (VerificaObrigatorio('#<%=txtOrderQty.ClientID%>',
    '#Avisos') == false) { // return false; // } // else if (VerificaObrigatorio('#<%=txtRate.ClientID%>',
    '#Avisos') == false) { // return false; // } // else if (VerificaValorCombo('#<%=ddlGSTTax.ClientID%>',
    '#Avisos') == false) { // return false; // } else { return true; } } catch (err)
    { alert('Erro in Required Fields: ' + err.description); return false; } } //-->
    </script>
</asp:Content>
