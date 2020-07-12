<%@ Page Title="PDI Details" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true"
    CodeFile="PDIDetails.aspx.cs" Inherits="Transactions_ADD_PDIDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <style type="text/css">
        .ajax__calendar_container
        {
            z-index: 1000;
        }
    </style>

    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(evt, args) {
            jQuery("#<%=ddlInvoiceNo.ClientID %>").select2();
            jQuery("#<%=ddlItemName.ClientID %>").select2();
        });
    </script>

    <script type="text/javascript">

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
        function Showalert() {
            $('#Avisos').fadeIn(6000)
            $('#Avisos').fadeOut(6000)
            $("#up").click();
        }
    </script>

    <script type="text/javascript">
        function Showalert1() {
            $('#MSG').fadeIn(6000)
            $('#MSG').fadeOut(6000)
            $("#up").click();
        }
    </script>

    <div class="page-content-wrapper">
        <div class="page-content">
            <!-- BEGIN PAGE CONTENT-->
            <div id="Avisos">
            </div>
            <div class="row">
                <div id="MSG" class="col-md-12">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
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
                <div class="col-md-12">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>PDI Details
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
                                        <div class="form-group">
                                            <div class="col-md-2">
                                            </div>
                                            <label class="col-md-3 control-label text-right">
                                            </label>
                                            <div class="col-md-4">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel10">
                                                    <ContentTemplate>
                                                        <asp:RadioButtonList ID="rbtType" runat="server" AutoPostBack="True" TabIndex="1"
                                                            RepeatDirection="Horizontal" CssClass="checker" OnSelectedIndexChanged="rbtType_SelectedIndexChanged">
                                                            <asp:ListItem Value="0" Selected="True">Tax Invoice &nbsp&nbsp</asp:ListItem>
                                                            <asp:ListItem Value="1">Labour Charge Invoice &nbsp&nbsp</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="dgPDIDEtail" EventName="RowCommand" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    Invoice No.
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlInvoiceNo" CssClass="select2" Width="100%" runat="server"
                                                                TabIndex="2" AutoPostBack="True" OnSelectedIndexChanged="ddlInvoiceNo_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="dgPDIDEtail" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class=" col-md-2 control-label label-sm">
                                                    Item Name</label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlItemName" CssClass="select2" Width="100%" runat="server"
                                                                TabIndex="3" AutoPostBack="True" OnSelectedIndexChanged="ddlItemName_SelectedIndexChanged">
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
                                                <label class="col-md-2 control-label label-sm">
                                                    Parameters
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtparameter" MaxLength="50" runat="server" CssClass="form-control"
                                                                Text="" ValidationGroup="Save" TabIndex="4"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class=" col-md-2 control-label label-sm">
                                                    Specification</label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel6">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtSpecification" MaxLength="500" TextMode="MultiLine" Columns="4"
                                                                Rows="4" Enabled="true" TabIndex="5" runat="server" CssClass="form-control"></asp:TextBox>
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
                                                    Inspection
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel5">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtInspection" MaxLength="50" Enabled="true" TabIndex-="6" runat="server"
                                                                CssClass="form-control"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class="col-md-2 control-label label-sm">
                                                    Observations
                                                </label>
                                                <label class="col-md-1 control-label text-right">
                                                    <label class="control-label label-sm">
                                                        1</label>
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel7">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtObservation1" MaxLength="50" runat="server" TabIndex-="7" CssClass="form-control"
                                                                ValidationGroup="Save" TabIndex="5"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </label>
                                                <label class="col-md-1 control-label text-right">
                                                    <label class="control-label label-sm">
                                                        2</label>
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel9">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtObservation2" MaxLength="50" runat="server" TabIndex-="8" CssClass="form-control"
                                                                ValidationGroup="Save" TabIndex="5"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </label>
                                                <label class="col-md-1 control-label text-right">
                                                    <label class="control-label label-sm">
                                                        3</label>
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel11">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtObservation3" MaxLength="50" runat="server" TabIndex-="9" CssClass="form-control"
                                                                ValidationGroup="Save" TabIndex="5"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </label>
                                                <label class="col-md-1 control-label text-right">
                                                    <label class="control-label label-sm">
                                                        4</label>
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel12">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtObservation4" MaxLength="50" runat="server" TabIndex-="10" CssClass="form-control"
                                                                ValidationGroup="Save" TabIndex="5"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </label>
                                                <label class="col-md-1 control-label text-right">
                                                    <label class="control-label label-sm">
                                                        5</label>
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel13">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtObservation5" MaxLength="50" runat="server" TabIndex-="11" CssClass="form-control"
                                                                ValidationGroup="Save" TabIndex="5"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-2 control-label label-sm">
                                                    Disposition
                                                </label>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel14">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtDisposition" MaxLength="50" runat="server" TabIndex-="12" CssClass="form-control"
                                                                Text="" ValidationGroup="Save" TabIndex="11"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <label class=" col-md-2 control-label label-sm">
                                                    Remark</label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel8">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtRemarkPDI" MaxLength="250" Enabled="true" TabIndex-="13" runat="server"
                                                                CssClass="form-control"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class=" col-md-2 control-label label-sm">
                                                    Note</label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel16">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtNote" MaxLength="500" TextMode="MultiLine" Columns="2" Enabled="true"
                                                                TabIndex-="14" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-8 control-label label-sm">
                                                </label>
                                                <div class="col-md-1">
                                                    <asp:LinkButton ID="btnInsert" CssClass="btn green" TabIndex="14" runat="server"
                                                        OnClick="btnInsert_Click"><i class="fa fa-check-square"></i> Insert</asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div style="overflow: auto;">
                                                <div class="col-md-12">
                                                    <div class="form-body">
                                                        <asp:UpdatePanel runat="server">
                                                            <ContentTemplate>
                                                                <asp:GridView ID="dgPDIDEtail" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                    CellPadding="4" Font-Size="12px" ShowFooter="false" Font-Names="Verdana" GridLines="Both"
                                                                    CssClass="table table-striped table-bordered table-advance table-hover" OnPageIndexChanging="dgPDIDEtail_PageIndexChanging"
                                                                    OnRowDeleting="dgPDIDEtail_RowDeleting" OnRowCommand="dgPDIDEtail_RowCommand"
                                                                    AllowPaging="false" PageSize="15">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Modify" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                                            HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkModify" CssClass="btn green btn-xs" BorderStyle="None" runat="server"
                                                                                    CausesValidation="False" CommandName="Modify" Text="" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'><i class="fa fa-edit"></i> Modify</asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Delete" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35px"
                                                                            HeaderStyle-Font-Size="12px" HeaderStyle-Font-Names="Verdana">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkDelete" CssClass="btn red btn-xs" BorderStyle="None" runat="server"
                                                                                    CausesValidation="False" CommandName="Delete" Text="" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'><i class="fa fa-trash-o"></i> Delete</asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle Font-Names="Verdana" Font-Size="12px" HorizontalAlign="Left" Width="35px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Item Code No.-Name" SortExpression="INSPDI_I_CODE"
                                                                            Visible="true" HeaderStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblINSPDI_I_CODE" CssClass="" runat="server" Text='<%# Eval("INSPDI_I_CODE") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Parameters" SortExpression="INSPDI_PARAMETERS" HeaderStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblINSPDI_PARAMETERS" CssClass="" runat="server" Text='<%# Eval("INSPDI_PARAMETERS") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Specification" SortExpression="INSPDI_SPECIFTION"
                                                                            HeaderStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblINSPDI_SPECIFTION" Width="300px" CssClass="" runat="server" Text='<%# Eval("INSPDI_SPECIFTION") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Inspection" SortExpression="IWD_I_CODE" HeaderStyle-HorizontalAlign="Left"
                                                                            Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblINSPDI_INSPECTION" CssClass="" runat="server" Text='<%# Eval("INSPDI_INSPECTION") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Observation 1" SortExpression="INSPDI_OBSR1" HeaderStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblINSPDI_OBSR1" CssClass="" runat="server" Text='<%# Eval("INSPDI_OBSR1") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Observation 2" SortExpression="INSPDI_OBSR2" HeaderStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblINSPDI_OBSR2" CssClass="" runat="server" Text='<%# Eval("INSPDI_OBSR2") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Observation 3" SortExpression="INSPDI_OBSR3" HeaderStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblINSPDI_OBSR3" CssClass="" runat="server" Text='<%# Eval("INSPDI_OBSR3") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Observation 4" SortExpression="INSPDI_OBSR4" Visible="true"
                                                                            HeaderStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblINSPDI_OBSR4" CssClass="" runat="server" Text='<%# Eval("INSPDI_OBSR4") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Observation 5" SortExpression="INSPDI_OBSR5" Visible="true"
                                                                            HeaderStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblINSPDI_OBSR5" CssClass="" runat="server" Text='<%# Eval("INSPDI_OBSR5") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Disposition" SortExpression="INSPDI_DSPOSITION" Visible="true"
                                                                            HeaderStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblIINSPDI_DSPOSITION" CssClass="" runat="server" Text='<%# Eval("INSPDI_DSPOSITION") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Remark" SortExpression="INSPDI_REMARK" Visible="true"
                                                                            HeaderStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblINSPDI_REMARK" CssClass="" runat="server" Text='<%# Eval("INSPDI_REMARK") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Item Code No.-Name" SortExpression="INSPDI_I_CODE"
                                                                            Visible="false" HeaderStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblINSPDI_I_CODEVAL" CssClass="" runat="server" Text='<%# Eval("INSPDI_I_CODEVAL") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Invoice Code" SortExpression="INSPDI_INSM_CODE" Visible="false"
                                                                            HeaderStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblINSPDI_INSM_CODE" CssClass="" runat="server" Text='<%# Eval("INSPDI_INSM_CODE") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Note" SortExpression="INSPDI_INSM_CODE" Visible="true"
                                                                            HeaderStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblINSPDI_NOTE" CssClass="" runat="server" Text='<%# Eval("INSPDI_NOTE") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <AlternatingRowStyle CssClass="alt" />
                                                                    <PagerStyle CssClass="pgr" />
                                                                </asp:GridView>
                                                            </ContentTemplate>
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
                                    <asp:LinkButton ID="btnSubmit" CssClass="btn green" TabIndex="15" runat="server"
                                        OnClick="btnSave_Click" OnClientClick="return VerificaCamposObrigatorios();"><i class="fa fa-check-square"> </i>  Save </asp:LinkButton>
                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="16" runat="server"
                                        OnClick="btnCancel_Click"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
                                </div>
                            </div>
                        </div>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel33">
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
                                                            Do you Want to Cancel Record?
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
    </div>
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
</asp:Content>
