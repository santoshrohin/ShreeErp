﻿<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="TaxInvoice.aspx.cs"
    Inherits="Transactions_ADD_TaxInvoice" Title="Tax Invoice" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="Ajaxified" Assembly="Ajaxified" Namespace="Ajaxified" %>
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
        function oknumber(sender, e) {
            $find('ModalPopupExtenderConfirm').hide();
            __doPostBack('btnOk1', e);
        }

        function oknumber1(sender, e) {
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

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(evt, args) {
            jQuery("#<%=ddlCustomer.ClientID %>").select2();
            jQuery("#<%=ddlItemCode.ClientID %>").select2();
            jQuery("#<%=ddlItemName.ClientID %>").select2();
            jQuery("#<%=ddlPONo.ClientID %>").select2();
            jQuery("#<%=ddlTray.ClientID %>").select2();
            jQuery("#<%=ddlState.ClientID %>").select2();
        });
    </script>

    <script type="text/javascript">
        function validateFloatKeyPress(el, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            if (charCode == 46 && el.value.indexOf(".") !== -1) {
                return false;
            }
            if (el.value.indexOf(".") !== -1) {
                var range = document.selection.createRange();
                if (range.text != "") {
                }
                else {
                    var number = el.value.split('.');
                    if (number.length == 2 && number[1].length > 1)
                        return false;
                }
            }
            return true;
        }
    </script>

    <div class="page-content-wrapper">
        <div class="page-content">
            <!-- BEGIN PAGE CONTENT-->
            <div class="row">
                <div id="MSG" class="col-md-12">
                    <asp:UpdatePanel ID="UpdatePanel16" runat="server">
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
            <div class="row">
                <div class="col-md-12">
                    <div id="Avisos">
                    </div>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-12">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Tax Invoice
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="linkBtn" runat="server" CssClass="remove" OnClick=" btnCancel_Click"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    <span class="required">*</span>Invoice Date
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="upddate" runat="server">
                                                        <ContentTemplate>
                                                            <div class="input-group">
                                                                <asp:TextBox CssClass="form-control" placeholder="dd MMM yyyy" ID="txtDate" TabIndex="1"
                                                                    runat="server" MsgObrigatorio="Invoice Date" TextMode="SingleLine" ReadOnly="false"
                                                                    AutoPostBack="True" OnTextChanged="txtDate_TextChanged"></asp:TextBox>
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <cc1:CalendarExtender ID="txtDate_CalendarExtender2" runat="server" Enabled="True"
                                                                    Format="dd MMM yyyy" TargetControlID="txtDate">
                                                                </cc1:CalendarExtender>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label label-sm">
                                                    Invoice No.
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtInvoiceNo" placeholder="No"
                                                        TabIndex="2" runat="server" Enabled="false"></asp:TextBox>
                                                </div>
                                                <label class="col-md-2 control-label">
                                                    Is Supplementary
                                                </label>
                                                <div class="col-md-1">
                                                    <asp:UpdatePanel ID="UpdatePanel41" runat="server">
                                                        <ContentTemplate>
                                                            <label class="checkbox">
                                                                <asp:CheckBox ID="chkIsSuppliement" runat="server" CssClass="checker" TabIndex="3"
                                                                    Checked="false" OnCheckedChanged="chkIsSuppliement_CheckedChanged" AutoPostBack="true" />
                                                            </label>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    <span class="required">*</span> Customer Name
                                                </label>
                                                <div class="col-md-7">
                                                    <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlCustomer" AutoPostBack="true" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged"
                                                                CssClass="select2" Width="100%" runat="server" MsgObrigatorio="Customer" TabIndex="4">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    Address
                                                </label>
                                                <div class="col-md-9">
                                                    <asp:UpdatePanel ID="UpdatePanel50" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control" placeholder="Address" ID="txtAddress" TabIndex="5"
                                                                runat="server" MsgObrigatorio="Address" TextMode="MultiLine" ReadOnly="false"
                                                                AutoPostBack="True" MaxLength="350"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label">
                                                    <span class="required">*</span>State
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel51" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlState" CssClass="select2" Width="280px" runat="server" TabIndex="6"
                                                                AutoPostBack="True" MsgObrigatorio="State Code" OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-1">
                                                    GST Number
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel52" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control" placeholder="GST Number" ID="txtGstNo" TabIndex="7"
                                                                runat="server" MsgObrigatorio="GST Number" TextMode="SingleLine" ReadOnly="false"
                                                                Enabled="true" AutoPostBack="True"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-1">
                                                    E WAY Bill Number
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel57" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control" placeholder="E WAY Bill Number" ID="txtEWayBill"
                                                                TabIndex="8" runat="server" MsgObrigatorio="E WAY Bill Number" TextMode="SingleLine"
                                                                ReadOnly="false" MaxLength="20" Enabled="true" AutoPostBack="True"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender35" TargetControlID="txtEWayBill"
                                                                ValidChars=" " FilterType="Custom,UppercaseLetters,LowercaseLetters ,Numbers"
                                                                runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <hr />
                                </div>
                            </div>
                            <div class="horizontal-form">
                                <div class="form-body">
                                    <!--/row-->
                                    <div class="col-md-12">
                                        <div class="row">
                                            <%-- <div class="col-md-12">--%>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <label class="control-label label-sm">
                                                        <span class="required">*</span> Item Code</label>
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlItemCode" CssClass="select2" Width="280px" runat="server"
                                                                TabIndex="9" AutoPostBack="True" MsgObrigatorio="Item Code" OnSelectedIndexChanged="ddlItemCode_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <!--/span-->
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label class="control-label label-sm">
                                                        <span class="required">*</span> Item Name</label>
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlItemName" CssClass="select2" Width="380px" runat="server"
                                                                TabIndex="10" AutoPostBack="True" MsgObrigatorio="Item Name" OnSelectedIndexChanged="ddlItemName_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <!--/span-->
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <label class="control-label label-sm">
                                                        <span class="required">*</span> PO Number</label>
                                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlPONo" CssClass="select2" Width="280px" runat="server" TabIndex="11"
                                                                AutoPostBack="True" MsgObrigatorio="PO Number" OnSelectedIndexChanged="ddlPONo_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <!--/span-->
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label class="control-label label-sm">
                                                        Unit</label>
                                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtUOM" placeholder="Unit" ReadOnly="true"
                                                                runat="server" TabIndex="12"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <!--/span-->
                                            <%-- </div>--%>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label class="control-label label-sm">
                                                        Stock Qty</label>
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtStockQty" placeholder="0.000"
                                                                runat="server" AutoPostBack="true" ReadOnly="true"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" TargetControlID="txtStockQty"
                                                                ValidChars="0123456789.-" runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label class="control-label label-sm">
                                                        Pending Qty</label>
                                                    <asp:UpdatePanel ID="UpdatePanel18" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtPendingQty" placeholder="0.000"
                                                                ReadOnly="true" runat="server" AutoPostBack="true"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" TargetControlID="txtPendingQty"
                                                                ValidChars="0123456789." runat="server" />
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtPendingOrderQty"
                                                                placeholder="0.000" ReadOnly="true" runat="server" AutoPostBack="true" Visible="false"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlPONo" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <!--/span-->
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label class="control-label label-sm">
                                                        <span class="required">*</span> Invoice Qty</label>
                                                    <asp:UpdatePanel ID="UpdatePanel24" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtVQty" placeholder="0.000"
                                                                TabIndex="13" runat="server" AutoPostBack="true" MsgObrigatorio="Invoice Qty"
                                                                OnTextChanged="txtVQty_OnTextChanged" MaxLength="15" onkeypress="return validateFloatKeyPress(this,event);"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="txtVQty"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlPONo" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <!--/span-->
                                            <!--/span-->
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label class="control-label label-sm">
                                                        Act. Wght.
                                                    </label>
                                                    <asp:UpdatePanel ID="UpdatePanel20" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtActWght" placeholder="0.00"
                                                                runat="server" ReadOnly="true" AutoPostBack="true" MsgObrigatorio="Actual Wght"
                                                                MaxLength="8"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" TargetControlID="txtActWght"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="chkIsSuppliement" EventName="CheckedChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <!--/span-->
                                            <div class="col-md-2">
                                                <label class="control-label label-sm">
                                                    Rate</label>
                                                <asp:UpdatePanel ID="UpdatePanel67" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control text-right input-sm" ID="txtRate" Text="" placeholder="0.00"
                                                            ReadOnly="true" runat="server" AutoPostBack="true" MsgObrigatorio="Rate" MaxLength="8"
                                                            OnTextChanged="txtRate_TextChanged" onkeypress="return validateFloatKeyPress(this,event);"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" TargetControlID="txtRate"
                                                            ValidChars="0123456789." runat="server" />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="chkIsSuppliement" EventName="CheckedChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlPONo" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <!--/span-->
                                            <div class="col-md-2">
                                                <label class="control-label label-sm">
                                                    Amount</label>
                                                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control text-right input-sm" ID="txtAmount" placeholder="0.00"
                                                            runat="server" ReadOnly="true" MsgObrigatorio="Amount"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtAmount"
                                                            ValidChars="0123456789." runat="server" />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="txtVQty" EventName="TextChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="txtRate" EventName="TextChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlPONo" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label class="control-label label-sm">
                                                        Amort Rate</label>
                                                    <asp:UpdatePanel ID="UpdatePanel43" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtAmortrate" placeholder="0.000"
                                                                runat="server" AutoPostBack="true" ReadOnly="true"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender31" TargetControlID="txtAmortrate"
                                                                ValidChars="0123456789.-" runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlPONo" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label class="control-label label-sm">
                                                        Amort Amount
                                                    </label>
                                                    <asp:UpdatePanel ID="UpdatePanel44" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtAmortAmount" placeholder="0.000"
                                                                ReadOnly="true" runat="server" AutoPostBack="true"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender32" TargetControlID="txtAmortAmount"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtVQty" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlPONo" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label class="control-label label-sm">
                                                        HSN Code</label>
                                                    <asp:UpdatePanel ID="UpdatePanel53" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtHSNCode" placeholder="HSN Code"
                                                                runat="server" AutoPostBack="true" ReadOnly="true"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label class="control-label  label-sm">
                                                        No. of Packages</label>
                                                    <asp:UpdatePanel ID="UpdatePanel30" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" OnTextChanged="txtNoPackaeg_TextChanged"
                                                                AutoPostBack="true" ID="txtNoPackaeg" Text="" TabIndex="14" placeholder="0" runat="server"
                                                                MsgObrigatorio="No Of Packages" MaxLength="10"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" TargetControlID="txtNoPackaeg"
                                                                ValidChars="0123456789" runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <!--/span-->
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label class="control-label label-sm">
                                                        Qty Per Pack
                                                    </label>
                                                    <asp:UpdatePanel ID="UpdatePanel31" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" Enabled="false" ID="txtQtyPerPack"
                                                                Text="" placeholder="0.00" runat="server" MsgObrigatorio="Qty Per Pack" AutoPostBack="true"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" TargetControlID="txtQtyPerPack"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtNoPackaeg" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtVQty" EventName="TextChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <label class="control-label label-sm">
                                                    Pack Description
                                                </label>
                                                <asp:UpdatePanel ID="UpdatePanel42" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control input-sm" TabIndex="15" ID="txtPackDesc" MsgObrigatorio="Pack Description"
                                                            placeholder="Pack Description" runat="server" MaxLength="100"></asp:TextBox>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <%--Visible False--%>
                                            <div id="dvVisibleFalse" runat="server" visible="false">
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <label class="control-label label-sm">
                                                            Sr. No.
                                                        </label>
                                                        <asp:UpdatePanel ID="UpdatePanel49" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right input-sm" ID="txtsrNo" placeholder="Sr. No."
                                                                    runat="server" AutoPostBack="true" MaxLength="20" onkeypress="return validateFloatKeyPress(this,event);"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender36" TargetControlID="txtsrNo"
                                                                    ValidChars="0123456789" runat="server" />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <!--/span-->
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <label class="control-label label-sm">
                                                            Batch No.</label>
                                                        <asp:UpdatePanel ID="UpdatePanel29" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right input-sm" ID="txtBatchNo" placeholder="Batch No."
                                                                    runat="server" MsgObrigatorio="Batch No" AutoPostBack="true" MaxLength="50" onkeypress="return validateFloatKeyPress(this,event);"></asp:TextBox>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                                <div class="form-group">
                                                    <label class="control-label label-sm">
                                                        &nbsp</label><br />
                                                    <%-- <asp:LinkButton ID="btnInsert" CssClass="btn blue  btn-sm" TabIndex="16" runat="server"
                                                        OnClick="btnInsert_Click" OnClientClick="return VerificaCamposObrigatorios();"><i class="fa fa-arrow-circle-down"> </i>  Insert </asp:LinkButton>--%>
                                                    <asp:UpdatePanel ID="UpdatePanel63" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Button ID="btnInsert" OnClientClick="this.disabled=true; this.value='Insert ... Please Wait.';"
                                                                UseSubmitBehavior="false" CssClass="btn blue" TabIndex="16" runat="server" Text="Insert"
                                                                OnClick="btnInsert_Click" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemCode" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlItemName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtNoPackaeg" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtVQty" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:UpdatePanel ID="UpdatePanel28" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control input-sm" ID="txtSubHeading" placeholder="Sub Heading"
                                                                runat="server" AutoPostBack="true" MaxLength="50" Visible="false"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" style="overflow: auto; width: 100%">
                                            <div class="col-md-12">
                                                <asp:UpdatePanel ID="UpdatePanel26" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="dgInvoiceAddDetail" runat="server" TabIndex="16" Style="width: 100%;"
                                                            AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-advance table-hover"
                                                            CellPadding="4" GridLines="Both" OnRowCommand="dgInvoiceAddDetail_RowCommand"
                                                            OnRowDeleting="dgInvoiceAddDetail_Deleting">
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
                                                                            CausesValidation="False" OnClientClick="return confirm('Are you sure to Delete?');"
                                                                            CommandName="Delete" CommandArgument='<%# ((GridViewRow)Container).RowIndex%>'
                                                                            Text="Delete"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="ItemCode" SortExpression="temCode" Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIND_I_CODE" runat="server" Text='<%# Bind("IND_I_CODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Item Code" SortExpression="ItemCode" Visible="True">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIND_I_CODENO" runat="server" Text='<%# Bind("IND_I_CODENO") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Item Name" SortExpression="ItemCode" Visible="True">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIND_I_NAME" runat="server" Text='<%# Bind("IND_I_NAME") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Unit" SortExpression="UOM" Visible="True">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblUOM" runat="server" Text='<%# Bind("UOM") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="POCODE" SortExpression="POCODE" Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPO_CODE" runat="server" Text='<%# Bind("PO_CODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="PO. No." SortExpression="PO No" Visible="True">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPO_NO" runat="server" Text='<%# Bind("PO_NO") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Stock Qty" SortExpression="StockQty" Visible="True">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSTOCK_QTY" runat="server" Text='<%# Bind("STOCK_QTY") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass=" Control-text text-right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Pending Qty" SortExpression="PendingQty" Visible="True">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPEND_QTY" runat="server" Text='<%# Bind("PEND_QTY") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass=" Control-text text-right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Inv. Qty" SortExpression="Inv.Qty">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblINV_QTY" runat="server" Text='<%# Bind("INV_QTY") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass=" Control-text text-right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Act. Wght." SortExpression="Act. Wght.">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblACT_WGHT" runat="server" Text='<%# Bind("ACT_WGHT") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass=" Control-text text-right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Rate" SortExpression="Rate" HeaderStyle-HorizontalAlign="Left"
                                                                    HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRATE" runat="server" Text='<%# Eval("RATE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass=" Control-text text-right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Amount" SortExpression="Amount" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAMT" runat="server" Text='<%# Eval("AMT") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass=" Control-text text-right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Amort Rate" SortExpression="AmortRate" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAmortRate" runat="server" Text='<%# Eval("AmortRate") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass=" Control-text text-right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Amort Amount" SortExpression="AmortAmount" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAmortAmount" runat="server" Text='<%# Eval("AmortAmount") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass=" Control-text text-right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Sub Title" SortExpression="Subtitle" Visible="false"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSubHeading" runat="server" Text='<%# Eval("IND_SUBHEADING") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Batch No." SortExpression="BatchNo" Visible="true"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblBatch" runat="server" Text='<%# Eval("IND_BACHNO") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="No. Of Packages" SortExpression="NoOfPackes" Visible="true"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblNoPack" runat="server" Text='<%# Eval("IND_NO_PACK") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Qty Per Pack" SortExpression="NoOfPackes" Visible="false"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPakQty" runat="server" Text='<%# Eval("IND_PAK_QTY") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Right" CssClass=" Control-text text-right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Pack Desc." SortExpression="NOOF_PACK_DESC">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIND_PACK_DESC" runat="server" Text='<%# Bind("IND_PACK_DESC") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Sr. No." SortExpression="IND_SR_NO">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIND_SR_NO" runat="server" Text='<%# Bind("IND_SR_NO") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="CGST Per" SortExpression="NoOfPackes" Visible="true"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCentralTaxPer" runat="server" Text='<%# Eval("E_BASIC") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="SGST Per" SortExpression="NoOfPackes" Visible="true"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblStatePer" runat="server" Text='<%# Eval("E_EDU_CESS") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="IGST Per" SortExpression="NoOfPackes" Visible="true"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIntegratedTaxPer" runat="server" Text='<%# Eval("E_H_EDU") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="CGST Amt" SortExpression="NoOfPackes" Visible="true"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEBasic" runat="server" Text='<%# Eval("IND_EX_AMT") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText=" SGST Amt" SortExpression="NoOfPackes" Visible="true"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEEcess" runat="server" Text='<%# Eval("IND_E_CESS_AMT") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="IGST Amt" SortExpression="NoOfPackes" Visible="true"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEHEcess" runat="server" Text='<%# Eval("IND_SH_CESS_AMT") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="HSN No." SortExpression="NoOfPackes" Visible="true"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblHSN_NO" runat="server" Text='<%# Eval("IND_HSN_CODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <PagerStyle CssClass="pgr" />
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="chkIsSuppliement" EventName="CheckedChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                                <asp:Panel ID="Panel1" runat="server">
                                                </asp:Panel>
                                            </div>
                                        </div>
                                        <br />
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <!--Basic Amount-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    Amort Amount
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel45" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtstroreloc" placeholder="Amort Amount"
                                                                runat="server" MaxLength="150" ReadOnly="true"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label label-sm">
                                                    Basic Amount
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtNetAmount" Text=""
                                                                placeholder="0.00" TabIndex="22" runat="server" Enabled="false"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" TargetControlID="txtNetAmount"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--Discount Amount-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    Veh. No.
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtVechicleNo" placeholder="Vehicle No."
                                                        TabIndex="16" runat="server" MaxLength="30"></asp:TextBox>
                                                </div>
                                                <label class="col-md-2 control-label label-sm">
                                                    Discount %
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtDiscPer" Text=""
                                                                placeholder="0.00" TabIndex="17" runat="server" AutoPostBack="true" OnTextChanged="txtDiscPer_TextChanged"
                                                                onkeypress="return validateFloatKeyPress(this,event);" MaxLength="6"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" TargetControlID="txtDiscPer"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtVQty" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtDiscPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel21" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtDiscAmt" Text=""
                                                                placeholder="0.00" TabIndex="18" Enabled="false" runat="server" AutoPostBack="true"
                                                                OnTextChanged="txtDiscAmt_TextChanged" onkeypress="return validateFloatKeyPress(this,event);"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" TargetControlID="txtDiscAmt"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtVQty" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtDiscPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--Packaging Amount-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    Transport
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:TextBox CssClass="form-control input-sm" ID="txtTransport" placeholder="Transport"
                                                        TabIndex="19" runat="server" MaxLength="50"></asp:TextBox>
                                                </div>
                                                <label class="col-md-2 control-label label-sm">
                                                    Packing Amount
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel15" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtPackAmt" Text=""
                                                                placeholder="0.00" TabIndex="20" runat="server" AutoPostBack="true" OnTextChanged="txtPackAmt_TextChanged"
                                                                onkeypress="return validateFloatKeyPress(this,event);" MaxLength="8"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" TargetControlID="txtPackAmt"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtDiscPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtBasicExcPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtducexcper" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtSHEExcPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtSHEExcPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--Accessable Value-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    Freight
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel35" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtFreight" Text=""
                                                                placeholder="0.00" TabIndex="21" runat="server" AutoPostBack="true" OnTextChanged="txtFreight_TextChanged"
                                                                onkeypress="return validateFloatKeyPress(this,event);" MaxLength="10"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender25" TargetControlID="txtFreight"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtDiscPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtBasicExcPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtducexcper" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtSHEExcPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtSHEExcPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label label-sm">
                                                    Accessable Amount
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtAccessableAmt" Text=""
                                                                placeholder="0.00" TabIndex="28" Enabled="false" runat="server" AutoPostBack="true"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender22" TargetControlID="txtAccessableAmt"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtDiscPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtBasicExcPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtducexcper" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtSHEExcPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtSHEExcPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtPackAmt" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtOtherCharges" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtFreight" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtIncurance" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtTransportAmt" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtOctri" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtTCSAmt" EventName="TextChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="divVis" runat="server" visible="false">
                                        <!--Basic Exceise Duty-->
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label class="col-md-2 control-label label-sm">
                                                        Issue Date
                                                    </label>
                                                    <div class="col-md-3">
                                                        <asp:UpdatePanel ID="updissueddate" runat="server">
                                                            <ContentTemplate>
                                                                <div class="input-group">
                                                                    <asp:TextBox CssClass="form-control" placeholder="dd MMM yyyy" ID="txtIssuedate"
                                                                        TabIndex="29" runat="server" MsgObrigatorio="Please Enter Issue Date" TextMode="SingleLine"
                                                                        ReadOnly="false"></asp:TextBox>
                                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd MMM yyyy hh:mm"
                                                                        TargetControlID="txtIssuedate">
                                                                    </cc1:CalendarExtender>
                                                                </div>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="txtDate" EventName="TextChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div id="dvInsurance" runat="server" visible="false">
                                                        <label class="col-md-2 control-label label-sm">
                                                            Insurance
                                                        </label>
                                                        <div class="col-md-4">
                                                            <asp:UpdatePanel ID="UpdatePanel36" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control text-right input-sm" ID="txtIncurance" Text=""
                                                                        placeholder="0.00" TabIndex="48" runat="server" AutoPostBack="true" OnTextChanged="txtIncurance_TextChanged"
                                                                        onkeypress="return validateFloatKeyPress(this,event);" MaxLength="10"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender26" TargetControlID="txtIncurance"
                                                                        ValidChars="0123456789." runat="server" />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtDiscPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtBasicExcPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtducexcper" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtSHEExcPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtSHEExcPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!--Education-->
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label class="col-md-2 control-label label-sm">
                                                        Issue Time
                                                    </label>
                                                    <div class="col-md-3">
                                                        <div class="input-group">
                                                            <asp:TextBox CssClass="form-control" ID="txtIssuetime" TabIndex="32" runat="server"
                                                                MsgObrigatorio="Please Enter Issue Time" TextMode="SingleLine" ReadOnly="false"></asp:TextBox>
                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                            <Ajaxified:TimePicker ID="TimePicker1" runat="server" TargetControlID="txtIssuetime">
                                                            </Ajaxified:TimePicker>
                                                        </div>
                                                    </div>
                                                    <div id="dvTransport" runat="server" visible="false">
                                                        <label class="col-md-2 control-label label-sm">
                                                            Transport
                                                        </label>
                                                        <div class="col-md-4">
                                                            <asp:UpdatePanel ID="UpdatePanel37" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control text-right input-sm" ID="txtTransportAmt" Text=""
                                                                        placeholder="0.00" TabIndex="49" runat="server" AutoPostBack="true" OnTextChanged="txtTransportAmt_TextChanged"
                                                                        onkeypress="return validateFloatKeyPress(this,event);" MaxLength="10"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender27" TargetControlID="txtTransportAmt"
                                                                        ValidChars="0123456789." runat="server" />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtDiscPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtBasicExcPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtducexcper" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtSHEExcPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtSHEExcPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!--HigherEdu-->
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label class="col-md-2 control-label label-sm">
                                                        Removal Date
                                                    </label>
                                                    <div class="col-md-3">
                                                        <asp:UpdatePanel ID="uppnlRemovaldate" runat="server">
                                                            <ContentTemplate>
                                                                <div class="input-group">
                                                                    <asp:TextBox CssClass="form-control" placeholder="dd MMM yyyy" ID="txtremovaldate"
                                                                        TabIndex="35" runat="server" MsgObrigatorio="Please Enter Issue Date" TextMode="SingleLine"
                                                                        ReadOnly="false" OnTextChanged="txtducexcper_TextChanged"></asp:TextBox>
                                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" Format="dd MMM yyyy hh:mm"
                                                                        TargetControlID="txtremovaldate">
                                                                    </cc1:CalendarExtender>
                                                                </div>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="txtDate" EventName="TextChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div id="dvOctri" runat="server" visible="false">
                                                        <label class="col-md-2 control-label label-sm">
                                                            Octri
                                                        </label>
                                                        <div class="col-md-4">
                                                            <asp:UpdatePanel ID="UpdatePanel38" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control text-right input-sm" ID="txtOctri" Text="" placeholder="0.00"
                                                                        TabIndex="50" runat="server" AutoPostBack="true" OnTextChanged="txtOctri_TextChanged"
                                                                        onkeypress="return validateFloatKeyPress(this,event);" MaxLength="10"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender28" TargetControlID="txtOctri"
                                                                        ValidChars="0123456789." runat="server" />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtDiscPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtBasicExcPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtducexcper" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtSHEExcPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtSHEExcPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!--Taxable Value-->
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label class="col-md-2 control-label label-sm">
                                                        Removal Time
                                                    </label>
                                                    <div class="col-md-3">
                                                        <div class="input-group">
                                                            <asp:TextBox CssClass="form-control" ID="txtRemoveltime" TabIndex="38" runat="server"
                                                                MsgObrigatorio="Please Enter Removel Time" TextMode="SingleLine" ReadOnly="false"></asp:TextBox>
                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                            <Ajaxified:TimePicker ID="TimePicker2" runat="server" TargetControlID="txtRemoveltime">
                                                            </Ajaxified:TimePicker>
                                                        </div>
                                                    </div>
                                                    <div id="dvTCS" runat="server" visible="false">
                                                        <label class="col-md-2 control-label label-sm">
                                                            TCS
                                                        </label>
                                                        <div class="col-md-4">
                                                            <asp:UpdatePanel ID="UpdatePanel39" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:TextBox CssClass="form-control text-right input-sm" ID="txtTCSAmt" Text="" placeholder="0.00"
                                                                        TabIndex="51" runat="server" AutoPostBack="true" OnTextChanged="txtTCSAmt_TextChanged"
                                                                        onkeypress="return validateFloatKeyPress(this,event);" MaxLength="10"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender29" TargetControlID="txtTCSAmt"
                                                                        ValidChars="0123456789." runat="server" />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtDiscPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtBasicExcPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtducexcper" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtSHEExcPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtSHEExcPer" EventName="TextChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--Tax Amt-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    Other Charges
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel34" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtOtherCharges" Text=""
                                                                placeholder="0.00" TabIndex="22" runat="server" AutoPostBack="true" OnTextChanged="txtOtherCharges_TextChanged"
                                                                onkeypress="return validateFloatKeyPress(this,event);" MaxLength="10"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender24" TargetControlID="txtOtherCharges"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtDiscPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtBasicExcPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtducexcper" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtSHEExcPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtSHEExcPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <%--Visible False--%>
                                                <div id="pnlhide" runat="server" visible="false">
                                                    <label class="col-md-2 control-label label-sm">
                                                        <%--Sales tax %--%>
                                                    </label>
                                                    <div class="col-md-2">
                                                        <asp:UpdatePanel ID="UpdatePanel13" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right input-sm" ID="txtSalesTaxPer" placeholder="0.00"
                                                                    TabIndex="41" runat="server" Text="" OnTextChanged="txtSalesTaxPer_TextChanged"
                                                                    Enabled="false" Visible="false"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender19" TargetControlID="txtSalesTaxPer"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlTaxName" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                                <asp:AsyncPostBackTrigger ControlID="txtBasicExcAmt" EventName="TextChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="txtEdueceAmt" EventName="TextChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="txtSHEExcAmt" EventName="TextChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="txtTaxableAmt" EventName="TextChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:UpdatePanel ID="UpdatePanel25" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right input-sm" ID="txtSalesTaxAmount" placeholder="0.00"
                                                                    TabIndex="42" runat="server" Enabled="false" Visible="false"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender20" TargetControlID="txtSalesTaxAmount"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlTaxName" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="txtDiscPer" EventName="TextChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="txtBasicExcPer" EventName="TextChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="txtBasicExcAmt" EventName="TextChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="txtducexcper" EventName="TextChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="txtEdueceAmt" EventName="TextChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="txtSHEExcPer" EventName="TextChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="txtSHEExcAmt" EventName="TextChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="txtPackAmt" EventName="TextChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="txtTaxableAmt" EventName="TextChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="txtSalesTaxPer" EventName="TextChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <label class="col-md-2 control-label label-sm">
                                                    Taxable Amount
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel33" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtTaxableAmt" Text=""
                                                                placeholder="0.00" TabIndex="39" runat="server" AutoPostBack="true" OnTextChanged="txtTaxableAmt_TextChanged"
                                                                onkeypress="return validateFloatKeyPress(this,event);" MaxLength="10" Enabled="false"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender23" TargetControlID="txtTaxableAmt"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtDiscPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtBasicExcPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtducexcper" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtSHEExcPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtBasicExcAmt" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtEdueceAmt" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtSHEExcAmt" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtPackAmt" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtOtherCharges" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtFreight" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtIncurance" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtTransportAmt" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtOctri" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtTCSAmt" EventName="TextChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                </label>
                                                <div class="col-md-2">
                                                </div>
                                                <label class="col-md-3 control-label label-sm">
                                                    CGST
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox Visible="true" CssClass="form-control text-right input-sm" ID="txtBasicExcPer"
                                                                placeholder="0.00" TabIndex="30" runat="server" AutoPostBack="true" Text="" OnTextChanged="txtBasicExcPer_TextChanged"
                                                                Enabled="true" MaxLength="6"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" TargetControlID="txtBasicExcPer"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="chkIsSuppliement" EventName="CheckedChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel22" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtBasicExcAmt" Text=""
                                                                placeholder="0.00" TabIndex="31" runat="server" Enabled="true"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" TargetControlID="txtBasicExcAmt"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtBasicExcPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtDiscAmt" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtDiscPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtPackAmt" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtOtherCharges" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtFreight" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtIncurance" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtTransportAmt" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtOctri" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtTCSAmt" EventName="TextChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--Trasport Value-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                </label>
                                                <div class="col-md-3">
                                                    <div class="col-md-7">
                                                        <asp:UpdatePanel ID="UpdatePanel46" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlTray" AutoPostBack="true" OnSelectedIndexChanged="ddlTray_SelectedIndexChanged"
                                                                    CssClass="select2" Width="300px" runat="server" MsgObrigatorio="Tray" TabIndex="15"
                                                                    Visible="false">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <label class="col-md-2 control-label label-sm">
                                                    SGST
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox Visible="true" CssClass="form-control text-right input-sm" ID="txtducexcper"
                                                                placeholder="0.00" TabIndex="33" runat="server" Text="" OnTextChanged="txtducexcper_TextChanged"
                                                                AutoPostBack="true" Enabled="true"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" TargetControlID="txtducexcper"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="chkIsSuppliement" EventName="CheckedChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel23" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtEdueceAmt" Text=""
                                                                placeholder="0.00" TabIndex="34" runat="server" Enabled="true" OnTextChanged="txtducexcper_TextChanged"
                                                                AutoPostBack="true"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" TargetControlID="txtEdueceAmt"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtducexcper" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtBasicExcPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtDiscAmt" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtDiscPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtPackAmt" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtBasicExcAmt" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtOtherCharges" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtFreight" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtIncurance" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtTransportAmt" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtOctri" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtTCSAmt" EventName="TextChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--Octri Value-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel ID="UpdatePanel47" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtTrayStock" placeholder="0.000"
                                                                runat="server" AutoPostBack="true" ReadOnly="true" TabIndex="16" Visible="false"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender33" TargetControlID="txtTrayStock"
                                                                ValidChars="0123456789.-" runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlTray" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label label-sm">
                                                    IGST
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox Visible="true" CssClass="form-control text-right input-sm" ID="txtSHEExcPer"
                                                                placeholder="0.00" TabIndex="36" AutoPostBack="true" runat="server" Text="" OnTextChanged="txtSHEExcPer_TextChanged"
                                                                Enabled="false"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" TargetControlID="txtSHEExcPer"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="chkIsSuppliement" EventName="CheckedChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel27" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtSHEExcAmt" Text=""
                                                                placeholder="0.00" TabIndex="37" runat="server" Enabled="false"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" TargetControlID="txtSHEExcAmt"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtducexcper" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtBasicExcPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtSHEExcPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtDiscAmt" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtDiscPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtPackAmt" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtBasicExcAmt" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtOtherCharges" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtFreight" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtIncurance" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtTransportAmt" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtOctri" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtTCSAmt" EventName="TextChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--TCS Value-->
                                    <div id="AvDivFalse" runat="server" visible="false">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label class="col-md-2 control-label label-sm">
                                                    </label>
                                                    <div class="col-md-3">
                                                        <asp:UpdatePanel ID="UpdatePanel48" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-right input-sm" ID="txtTrayQty" placeholder="0.000"
                                                                    TabIndex="21" runat="server" AutoPostBack="true" MsgObrigatorio="Tray Qty" OnTextChanged="txtTrayQty_OnTextChanged"
                                                                    Visible="false"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender34" TargetControlID="txtTrayQty"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlTray" EventName="SelectedIndexChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--TCS Value-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                </label>
                                                <div class="col-md-2">
                                                </div>
                                                <label class="col-md-3 control-label label-sm">
                                                    TCS
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel60" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:UpdatePanel ID="UpdatePanel62" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:CheckBox ID="chkTCS" runat="server" CssClass="checker" TabIndex="3" Checked="false"
                                                                        OnCheckedChanged="chkTCS_CheckedChanged" AutoPostBack="true" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="chkIsSuppliement" EventName="CheckedChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel61" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtTCSAmount" Text=""
                                                                placeholder="0.00" ReadOnly="true" TabIndex="31" runat="server" Enabled="true"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender38" TargetControlID="txtTCSAmount"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="chkTCS" EventName="CheckedChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtBasicExcPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtDiscAmt" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtDiscPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtPackAmt" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtOtherCharges" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtFreight" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtIncurance" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtTransportAmt" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtOctri" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtTCSAmt" EventName="TextChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    
                                    <%--TCS PERCENTAGE VALUE --%>
                                    
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                </label>
                                                <div class="col-md-3">
                                                    <div class="col-md-7">
                                                        
                                                    </div>
                                                </div>
                                                <label class="col-md-2 control-label label-sm">
                                                    TCS Per
                                                </label>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel65" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox Visible="true" CssClass="form-control text-right input-sm" ID="txtTCSPer"
                                                                placeholder="0.00" TabIndex="33" runat="server" Text="" 
                                                                AutoPostBack="true" Enabled="true"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender37" TargetControlID="txtTCSPer"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="chkIsSuppliement" EventName="CheckedChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel ID="UpdatePanel66" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtPercAmt" Text=""
                                                                placeholder="0.00" TabIndex="34" runat="server" Enabled="true" 
                                                                AutoPostBack="true"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender39" TargetControlID="txtPercAmt"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtducexcper" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtBasicExcPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtDiscAmt" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtDiscPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtPackAmt" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtBasicExcAmt" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtOtherCharges" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtFreight" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtIncurance" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtTransportAmt" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtOctri" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtTCSAmt" EventName="TextChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    
                                    <!--Rounding Value-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                </label>
                                                <div class="col-md-3">
                                                </div>
                                                <label class="col-md-2 control-label label-sm">
                                                    Rounding(+/-)
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel40" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtRoundingAmt" Text=""
                                                                placeholder="0.00" TabIndex="23" runat="server" AutoPostBack="true" MaxLength="10"
                                                                OnTextChanged="txtRoundingAmt_TextChanged"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender30" TargetControlID="txtRoundingAmt"
                                                                ValidChars="0123456789.-" runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="chkTCS" EventName="CheckedChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtDiscPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtBasicExcPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtducexcper" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtSHEExcPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtSHEExcPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--Grand Total-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-7 control-label label-sm">
                                                    Grand Amount
                                                </label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel19" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox CssClass="form-control text-right input-sm" ID="txtGrandAmt" placeholder="0.00"
                                                                TabIndex="53" runat="server" Enabled="false" MaxLength="10"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" TargetControlID="txtGrandAmt"
                                                                ValidChars="0123456789." runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="chkTCS" EventName="CheckedChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtOtherCharges" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtFreight" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtIncurance" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtTransportAmt" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtOctri" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtPackAmt" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtTCSAmt" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtDiscPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtBasicExcPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtducexcper" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtSHEExcPer" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtBasicExcAmt" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtEdueceAmt" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtSHEExcAmt" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtPackAmt" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlTaxName" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtRoundingAmt" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtTaxableAmt" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="txtAccessableAmt" EventName="TextChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="VisFalse" runat="server" visible="false">
                                        <!--Payment Terms And Authorized Signature-->
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label class="col-md-2 control-label label-sm">
                                                        L.R. Date
                                                    </label>
                                                    <div class="col-md-3">
                                                        <div class="input-group">
                                                            <asp:TextBox CssClass="form-control" placeholder="dd MMM yyyy" ID="txtLRDate" TabIndex="45"
                                                                ReadOnly="false" runat="server" MsgObrigatorio="Please L.R Date" TextMode="SingleLine"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" Format="dd MMM yyyy"
                                                                TargetControlID="txtLRDate">
                                                            </cc1:CalendarExtender>
                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        </div>
                                                    </div>
                                                    <label class="col-md-2 control-label label-sm">
                                                        L.R. No.
                                                    </label>
                                                    <div class="col-md-2">
                                                        <asp:TextBox CssClass="form-control" placeholder="L R No." ID="txtLRNo" TabIndex="43"
                                                            MaxLength="50" runat="server" MsgObrigatorio="LR No" TextMode="SingleLine"></asp:TextBox>
                                                    </div>
                                                    <label class="col-md-2 control-label label-sm">
                                                        Remark
                                                    </label>
                                                    <div class="col-md-3">
                                                        <asp:UpdatePanel ID="UpdatePanel58" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control" placeholder="Remark" ID="txtRemark" TabIndex="47"
                                                                    runat="server" MsgObrigatorio="Remark" TextMode="SingleLine" MaxLength="255"></asp:TextBox>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <label class="col-md-2 control-label label-sm">
                                                        Electronic Reference Number
                                                    </label>
                                                    <div class="col-md-3">
                                                        <asp:UpdatePanel ID="UpdatePanel54" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-left input-sm" ID="txtElectrRefNum" placeholder="Reference Number"
                                                                    TabIndex="21" runat="server" AutoPostBack="true" MsgObrigatorio="Tray Qty" MaxLength="50"></asp:TextBox>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <label class="col-md-2 control-label label-sm">
                                                        <span class="required">*</span> Tax Name
                                                    </label>
                                                    <div class="col-md-3">
                                                        <asp:UpdatePanel ID="UpdatePanel14" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlTaxName" CssClass="select2" Width="300px" Enabled="false"
                                                                    runat="server" TabIndex="40" AutoPostBack="True" OnSelectedIndexChanged="ddlTaxName_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnInsert" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="chkIsSuppliement" EventName="CheckedChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="dgInvoiceAddDetail" EventName="RowCommand" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label class="col-md-2 control-label label-sm">
                                                        Payment Terms and Conditions
                                                    </label>
                                                    <div class="col-md-3">
                                                        <asp:UpdatePanel ID="UpdatePanel55" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-left input-sm" ID="txtTermsNConditions"
                                                                    placeholder="Payment Terms And Conditions" TabIndex="21" runat="server" TextMode="MultiLine"
                                                                    AutoPostBack="true" MsgObrigatorio="Payment Terms And Conditions" MaxLength="350"></asp:TextBox>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <label class="col-md-2 control-label label-sm">
                                                        Authorized Signature Name
                                                    </label>
                                                    <div class="col-md-4">
                                                        <asp:UpdatePanel ID="UpdatePanel56" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control text-left input-sm" ID="txtAuthorizedName" Text=""
                                                                    placeholder="Authorized Name" TabIndex="52" runat="server" AutoPostBack="true"
                                                                    MaxLength="50"></asp:TextBox>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-actions fluid">
                                <div class="col-md-offset-4 col-md-9">
                                    <%--<asp:UpdatePanel runat="server" ID="UpdatePanel59">
                                        <ContentTemplate>
                                            <asp:Button ID="btnSubmit" CssClass="btn green" TabIndex="24" runat="server" OnClick="btnSubmit_Click"
                                                OnClientClick="this.disabled=true; this.value='Saving ... Please Wait.';" Text="Save">  </asp:Button>
                                            <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="25" runat="server"
                                                OnClick="btnCancel_Click"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>--%>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel59" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="btnSubmit" OnClientClick="this.disabled=true; this.value='Saving ... Please Wait.';"
                                                UseSubmitBehavior="false" CssClass="btn green" TabIndex="24" runat="server" Text="Save"
                                                OnClick="btnSubmit_Click" />
                                            <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="25" runat="server"
                                                OnClick="btnCancel_Click"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:UpdatePanel runat="server" ID="UpdatePanel32">
                        <ContentTemplate>
                            <asp:LinkButton ID="CheckCondition" runat="server" BackColor="" CssClass="formlabel"></asp:LinkButton>
                            <cc1:ModalPopupExtender runat="server" ID="ModalPopupPrintSelection" BackgroundCssClass="modalBackground"
                                OnOkScript="oknumber1()" OnCancelScript="oncancel()" DynamicServicePath="" Enabled="True"
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
                </div>
            </div>
        </div>
    </div>
    <!-- END PAGE CONTENT-->
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

    <script src="../../assets/plugins/select2/select2.js" type="text/javascript"></script>

    <!-- END CORE PLUGINS -->
    <!-- BEGIN PAGE LEVEL PLUGINS -->

    <script src="../../assets/plugins/bootstrap-daterangepicker/moment.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-daterangepicker/daterangepicker.js" type="text/javascript"></script>

    <script src="../../assets/plugins/gritter/js/jquery.gritter.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->

    <script type="text/javascript">
        function VerificaCamposObrigatorios() {
            try {
                if (VerificaObrigatorio('#<%=ddlCustomer.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtDate.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=ddlItemCode.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=ddlItemCode.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=ddlItemName.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=ddlPONo.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtVQty.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtActWght.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtRate.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtRate.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtAmount.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=ddlState.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtGstNo.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else {
                    return true;
                }
            }
            catch (err) {
                alert('Erro in Required Fields: ' + err.description);
                return false;
            }
        }
    </script>

</asp:Content>