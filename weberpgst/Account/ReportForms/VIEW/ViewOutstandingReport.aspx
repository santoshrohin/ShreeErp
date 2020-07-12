<%@ Page Title="Outstanding Report" Language="C#" MasterPageFile="~/main.master"
    AutoEventWireup="true" CodeFile="ViewOutstandingReport.aspx.cs" Inherits="RoportForms_VIEW_ViewOutstandingReport" %>

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
        jQuery("#<%=ddlPartyName.ClientID %>").select2();
        jQuery("#<%=ddlType.ClientID %>").select2();
        jQuery("#<%=ddlBillType.ClientID %>").select2();
        
        });
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
                <div class="col-md-10">
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
                <div class="col-md-1">
                </div>
                <div class="col-md-10">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Outstanding Report
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="CncLnk" CssClass="remove" OnClick="btnCancel_Click" runat="server"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="form-group">
                                            
                                            <label class="col-md-2 control-label text-right">
                                                Till Date
                                            </label>
                                            <div class="col-md-3">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                                                    <ContentTemplate>
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtToDate" runat="server" placeholder="dd MMM yyyy" CssClass="form-control"
                                                                MsgObrigatorio="Please Select To Date" TabIndex="1" ValidationGroup="Save"></asp:TextBox>
                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                            <cc1:CalendarExtender ID="CalendarExtender1" BehaviorID="calendar2" runat="server"
                                                                Enabled="True" TargetControlID="txtToDate" PopupButtonID="txtToDate" Format="dd MMM yyyy">
                                                            </cc1:CalendarExtender>
                                                        </div>
                                                    </ContentTemplate>
                                                    
                                                </asp:UpdatePanel>
                                            </div>
                                            
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-2 control-label text-right">
                                                Type
                                            </label>
                                            <div class="col-md-8">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel8">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="True" CssClass="select2"
                                                            MsgObrigatorio="Please Select Type" Width="100%" TabIndex="2" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                                                            <asp:ListItem Value="0" Text="All"> </asp:ListItem>
                                                            <asp:ListItem Value="1" Text="Customer"> </asp:ListItem>
                                                            <asp:ListItem Value="2" Text="Supplier"> </asp:ListItem>
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-2 control-label text-right">
                                               Bill Type
                                            </label>
                                            <div class="col-md-8">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel10">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlBillType" runat="server" AutoPostBack="True" CssClass="select2"
                                                            MsgObrigatorio="Please Select Bill Type" Width="100%" TabIndex="3" >
                                                            <asp:ListItem Value="0" Text="All Balances"> </asp:ListItem>
                                                            <asp:ListItem Value="1" Text="Due Bills Only"> </asp:ListItem>
                                                            
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                        </div>
                                    </div>
                                    
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-2 control-label text-right">
                                                Party Name
                                            </label>
                                            <div class="col-md-8">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlPartyName" runat="server" AutoPostBack="True" CssClass="select2"
                                                            MsgObrigatorio="Please Select Supplier" Width="100%" TabIndex="4">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="chkAllParty" EventName="CheckedChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                                    <ContentTemplate>
                                                        <asp:CheckBox ID="chkAllParty" runat="server" CssClass="checker" Text="All" AutoPostBack="True"
                                                            TabIndex="5" OnCheckedChanged="chkAllParty_CheckedChanged" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-md-1">
                                            </div>
                                            <label class="col-md-3 control-label text-right">
                                            </label>
                                            <div class="col-md-5">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel6">
                                                    <ContentTemplate>
                                                        <asp:RadioButtonList ID="rbtGroup" runat="server" AutoPostBack="True" TabIndex="6"
                                                            RepeatDirection="Horizontal" CssClass="checker">
                                                            <asp:ListItem Value="0" Selected="True">&nbsp Monthwise&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</asp:ListItem>
                                                            <asp:ListItem Value="1">&nbsp Partywise&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-md-1">
                                            </div>
                                            <label class="col-md-3 control-label text-right">
                                            </label>
                                            <div class="col-md-5">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel7">
                                                    <ContentTemplate>
                                                        <asp:RadioButtonList ID="rbtReportType" runat="server" AutoPostBack="True" TabIndex="7"
                                                            RepeatDirection="Horizontal" CssClass="checker">
                                                            <asp:ListItem Value="0" Selected="True">&nbsp Detail&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</asp:ListItem>
                                                            <asp:ListItem Value="1">&nbsp Summary &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-actions fluid">
                                    <div class="col-md-offset-5 col-md-9">
                                        <asp:LinkButton ID="btnShow" CssClass="btn green" TabIndex="8" runat="server" OnClientClick="return VerificaCamposObrigatorios();"
                                            OnClick="btnShow_Click"><i class="fa fa-check-square"> </i>  Show </asp:LinkButton>
                                        <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="9" runat="server"
                                            OnClick="btnCancel_Click"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- END PAGE CONTENT-->
    <!-- BEGIN JAVASCRIPTS(Load javascripts at bottom, this will reduce page load time) -->
    <!-- BEGIN CORE PLUGINS -->
    <link href="../../../assets/Avisos/Avisos.css" rel="stylesheet" type="text/css" />

    <script src="../../../assets/Avisos/Avisos.js" type="text/javascript"></script>

    <script src="../../../assets/JS/Util.js" type="text/javascript"></script>

    <script src="../../../assets/scripts/jquery-1.7.1.min.js" type="text/javascript"></script>

    <script src="../../../assets/plugins/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../../../assets/plugins/jquery-migrate-1.2.1.min.js" type="text/javascript"></script>

    <script src="../../../assets/plugins/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>

    <script src="../../../assets/plugins/bootstrap-hover-dropdown/twitter-bootstrap-hover-dropdown.min.js"
        type="text/javascript"></script>

    <script src="../../../assets/plugins/jquery-slimscroll/jquery.slimscroll.min.js" type="text/javascript"></script>

    <script src="../../../assets/plugins/jquery.blockui.min.js" type="text/javascript"></script>

    <script src="../../../assets/plugins/jquery.cokie.min.js" type="text/javascript"></script>

    <script src="../../../assets/plugins/uniform/jquery.uniform.min.js" type="text/javascript"></script>

    <script src="../../../assets/plugins/select2/select2.js" type="text/javascript"></script>

    <!-- END CORE PLUGINS -->
    <!-- BEGIN PAGE LEVEL PLUGINS -->

    <script src="../../../assets/plugins/bootstrap-daterangepicker/moment.min.js" type="text/javascript"></script>

    <script src="../../../assets/plugins/bootstrap-daterangepicker/daterangepicker.js" type="text/javascript"></script>

    <script src="../../../assets/plugins/gritter/js/jquery.gritter.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../../../assets/scripts/app.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->
</asp:Content>
