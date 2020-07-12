<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="Default.aspx.cs"
    Inherits="Admin_Default" Title="Administration" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <style type="text/css">
        .ajax__calendar_container
        {
            z-index: 1000;
        }
        .ModalPopupBG
        {
            background-color: #666699;
            filter: alpha(opacity=20);
            opacity: 0.2;
        }
    </style>

    <script type="text/javascript">
        function oknumber(sender, e) {
            $find('ModalPopupMsg').hide();
            __doPostBack('Button5', e);
        }
        
    </script>

    <!-- BEGIN CONTENT -->
    <div class="page-content-wrapper">
        <div class="page-content">
            <!-- BEGIN PAGE CONTENT-->
            <%--<div class="row">
                <div class="col-md-1">
                </div>
                <div class="col-md-10">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="PanelMsg" runat="server" Visible="false" Style="background-color: #feefb3; margin-bottom:10px; height: 50px; width: 100%; border: 1px solid #9f6000">
                                <div style="vertical-align: middle; margin-top: 10px;">
                                    <asp:Label ID="lblmsg" runat="server" Style="color: #9f6000; font-size: medium; font-weight: bold;
                                        margin-top: 50px; margin-left: 10px;"></asp:Label>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>--%>
            <%-- <div class="row">
                <div class="col-md-12">
                    <ul class="page-breadcrumb breadcrumb">
                        <li>
                            <i class="fa fa-home"></i>
                            <a href="../Masters/ADD/Dashboard.aspx" runat="server">Home</a>
                            <i class="fa fa-angle-right"></i>
                        </li>
                        <li>
                            <a href="Default.aspx">Administration</a>
                        </li>
                    </ul>
                </div>
            </div>--%>
            <div class="tiles">
                <div class="col-md-1">
                </div>
                <%--<asp:LinkButton ID="LinkButton1" OnClientClick CssClass="tile bg-green" runat="server"> Company Information</asp:LinkButton>     --%>
                <asp:UpdatePanel runat="server" ID="UpdatePane22">
                    <ContentTemplate>
                        <a href="#" id="comp1" runat="server" onserverclick="btnCompany_click">
                            <div class="tile bg-green">
                                <div class="tile-body">
                                    <i class="fa fa-home"></i>
                                </div>
                                <div class="tile-object">
                                    <div class="name">
                                        Company Information
                                    </div>
                                </div>
                            </div>
                        </a>
                        <div class="col-md-1">
                        </div>
                        <a href="#" id="A1" runat="server" onserverclick="btnSysConfig_click">
                            <div class="tile bg-green">
                                <div class="tile-body">
                                    <i class="fa fa-home"></i>
                                </div>
                                <div class="tile-object">
                                    <div class="name">
                                        System Configration
                                    </div>
                                </div>
                            </div>
                        </a>
                        <div class="col-md-1">
                        </div>
                        <a href="#" id="comp2" runat="server" onserverclick="btnUserMaster_click">
                            <div class="tile bg-red">
                                <div class="corner">
                                </div>
                                <div class="tile-body">
                                    <i class="fa fa-user"></i>
                                </div>
                                <div class="tile-object">
                                    <div class="name text-center">
                                        User Master
                                    </div>
                                </div>
                            </div>
                        </a>
                        <div class="col-md-1">
                        </div>
                        <a href="#" id="comp8" runat="server" onserverclick="btnUserRights_click">
                            <div class="tile bg-purple">
                                <div class="tile-body">
                                    <i class="fa fa-briefcase"></i>
                                </div>
                                <div class="tile-object">
                                    <div class="name">
                                        User Rights
                                    </div>
                                </div>
                            </div>
                        </a>
                        <%--<div class="col-md-1">
                </div>
                <div class="tile bg-green">
                    <div class="tile-body">
                        <i class="fa fa-bar-chart-o"></i>
                    </div>
                    <div class="tile-object">
                        <div class="name">
                            Reports
                        </div>
                        <div class="number">
                        </div>
                    </div>
                </div>--%>
                        <div class="col-md-1">
                        </div>
                        <a href="#" id="comp3" runat="server" onserverclick="btnUnlockRecords_click">
                            <div class="tile bg-green">
                                <div class="tile-body">
                                    <i class="fa fa-unlock"></i>
                                </div>
                                <div class="tile-object">
                                    <div class="name text-center">
                                        Unlock Records
                                    </div>
                                </div>
                            </div>
                        </a>
                        <div class="col-md-1">
                        </div>
                        <a href="#" id="comp12" runat="server" onserverclick="btnDatabaseBackup_click">
                            <div class="tile bg-red">
                                <div class="tile-body">
                                    <i class="fa fa-briefcase"></i>
                                </div>
                                <div class="tile-object">
                                    <div class="name text-center">
                                        Database Backup
                                    </div>
                                </div>
                            </div>
                        </a>
                        <div class="col-md-1">
                        </div>
                        <a href="#" id="comp4" runat="server" onserverclick="btnCountryMaster_click">
                            <div class="tile bg-red">
                                <div class="tile-body">
                                    <i class="fa fa-briefcase"></i>
                                </div>
                                <div class="tile-object">
                                    <div class="name text-center">
                                        Country Master
                                    </div>
                                </div>
                            </div>
                        </a>
                        <div class="col-md-1">
                        </div>
                        <a href="#" id="comp5" runat="server" onserverclick="btnStateMaster_click">
                            <div class="tile bg-green">
                                <div class="tile-body">
                                    <i class="fa fa-briefcase"></i>
                                </div>
                                <div class="tile-object">
                                    <div class="name text-center">
                                        State Master
                                    </div>
                                </div>
                            </div>
                        </a>
                        <div class="col-md-1">
                        </div>
                        <a href="#" id="comp6" runat="server" onserverclick="btnCityMaster_click">
                            <div class="tile bg-red">
                                <div class="tile-body">
                                    <i class="fa fa-briefcase"></i>
                                </div>
                                <div class="tile-object">
                                    <div class="name">
                                        City Master
                                    </div>
                                </div>
                            </div>
                        </a>
                        <div class="col-md-1">
                        </div>
                        <a href="#" id="comp7" runat="server" onserverclick="btnCurrencyMaster_click">
                            <div class="tile bg-purple">
                                <div class="tile-body">
                                    <i class="fa fa-briefcase"></i>
                                </div>
                                <div class="tile-object">
                                    <div class="name">
                                        Currency Master
                                    </div>
                                </div>
                            </div>
                        </a>
                        <div class="col-md-1">
                        </div>
                        <a href="#" id="comp10" runat="server" onserverclick="btnUnitMaster_click">
                            <div class="tile bg-purple">
                                <div class="tile-body">
                                    <i class="fa fa-briefcase"></i>
                                </div>
                                <div class="tile-object">
                                    <div class="name">
                                        Unit Master
                                    </div>
                                </div>
                            </div>
                        </a>
                        <div class="col-md-1">
                        </div>
                        <a href="#" id="compArea" runat="server" onserverclick="btnAreaMaster_click">
                            <div class="tile bg-purple">
                                <div class="tile-body">
                                    <i class="fa fa-briefcase"></i>
                                </div>
                                <div class="tile-object">
                                    <div class="name">
                                        Area Master
                                    </div>
                                </div>
                            </div>
                        </a>
                        <div class="col-md-1">
                        </div>
                        <a href="#" id="comp11" runat="server" onserverclick="btnLogMaster_click" visible="true">
                            <div class="tile bg-green">
                                <div class="tile-body">
                                    <i class="fa fa-briefcase"></i>
                                </div>
                                <div class="tile-object">
                                    <div class="name">
                                        Log Master
                                    </div>
                                </div>
                            </div>
                        </a>
                        <div class="col-md-1">
                        </div>
                        <a href="#" id="comp13" runat="server" onserverclick="btnISOMaster_click" visible="true">
                            <div class="tile bg-green">
                                <div class="tile-body">
                                    <i class="fa fa-briefcase"></i>
                                </div>
                                <div class="tile-object">
                                    <div class="name">
                                        ISO Master
                                    </div>
                                </div>
                            </div>
                        </a>
                        <div class="col-md-1">
                        </div>
                        <a href="#" id="comp9" runat="server" onserverclick="btnUserMasterReport_click">
                            <div class="tile bg-dark">
                                <div class="tile-body">
                                    <i class="fa fa-briefcase"></i>
                                </div>
                                <div class="tile-object">
                                    <div class="name">
                                        User Master Report
                                    </div>
                                </div>
                            </div>
                        </a>
                         <div class="col-md-1">
                        </div>
                        <a href="#" id="A2" runat="server" onserverclick="btnTransportMaster_click">
                            <div class="tile bg-purple">
                                <div class="tile-body">
                                    <i class="fa fa-briefcase"></i>
                                </div>
                                <div class="tile-object">
                                    <div class="name">
                                        Transport Master
                                    </div>
                                </div>
                            </div>
                        </a>
                         <div class="col-md-1">
                        </div>
                        <a href="#" id="A3" runat="server" onserverclick="btnVehicleMaster_click">
                            <div class="tile bg-purple">
                                <div class="tile-body">
                                    <i class="fa fa-briefcase"></i>
                                </div>
                                <div class="tile-object">
                                    <div class="name">
                                        Vehicle Master
                                    </div>
                                </div>
                            </div>
                        </a>
                        <asp:LinkButton ID="CheckCondition" runat="server" BackColor="" CssClass="formlabel"></asp:LinkButton>
                        <cc1:ModalPopupExtender runat="server" ID="ModalPopupMsg" BackgroundCssClass="ModalPopupBG"
                            OnOkScript="oknumber()" CancelControlID="Button7" Enabled="True" PopupControlID="popUpPanel5"
                            TargetControlID="CheckCondition">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="popUpPanel5" runat="server" Style="display: none;">
                            <div class="col-md-12">
                                <div class="portlet box blue">
                                    <div class="portlet-title">
                                        <div style="font-size: medium;" class="captionPopup">
                                            Warning
                                        </div>
                                    </div>
                                    <div class="portlet-body form">
                                        <div class="form-horizontal">
                                            <div class="form-body">
                                                <div class="row">
                                                    <label style="font-size: medium;" class="col-md-12 control-label">
                                                        You Have No Right To View
                                                    </label>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-9">
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-9">
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-offset-2 col-md-9">
                                                        <asp:LinkButton ID="Button5" CssClass="btn blue" TabIndex="26" runat="server" Visible="true"
                                                            OnClick="btnOk_Click">  OK </asp:LinkButton>
                                                        <asp:LinkButton ID="Button7" CssClass="btn default" TabIndex="28" runat="server"> Cancel</asp:LinkButton>
                                                    </div>
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
        <!-- END PAGE CONTENT-->
    </div>
    <!-- END CONTENT -->
    <!-- BEGIN JAVASCRIPTS(Load javascripts at bottom, this will reduce page load time) -->
    <!-- BEGIN CORE PLUGINS -->
    <!--[if lt IE 9]>
<script src="assets/plugins/respond.min.js"></script>
<script src="assets/plugins/excanvas.min.js"></script>
<![endif]-->

    <script src="../assets/plugins/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../assets/plugins/jquery-migrate-1.2.1.min.js" type="text/javascript"></script>

    <!-- IMPORTANT! Load jquery-ui-1.10.3.custom.min.js before bootstrap.min.js to fix bootstrap tooltip conflict with jquery ui tooltip -->

    <script src="../assets/plugins/jquery-ui/jquery-ui-1.10.3.custom.min.js" type="text/javascript"></script>

    <script src="../assets/plugins/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>

    <script src="../assets/plugins/bootstrap-hover-dropdown/twitter-bootstrap-hover-dropdown.min.js"
        type="text/javascript"></script>

    <script src="../assets/plugins/jquery-slimscroll/jquery.slimscroll.min.js" type="text/javascript"></script>

    <script src="../assets/plugins/jquery.blockui.min.js" type="text/javascript"></script>

    <script src="../assets/plugins/jquery.cokie.min.js" type="text/javascript"></script>

    <script src="../assets/plugins/uniform/jquery.uniform.min.js" type="text/javascript"></script>

    <!-- END CORE PLUGINS -->
    <!-- BEGIN PAGE LEVEL PLUGINS -->

    <script src="../assets/plugins/bootstrap-daterangepicker/moment.min.js" type="text/javascript"></script>

    <script src="../assets/plugins/bootstrap-daterangepicker/daterangepicker.js" type="text/javascript"></script>

    <script src="../assets/plugins/gritter/js/jquery.gritter.js" type="text/javascript"></script>

    <!-- IMPORTANT! fullcalendar depends on jquery-ui-1.10.3.custom.min.js for drag & drop support -->

    <script src="../assets/plugins/fullcalendar/fullcalendar/fullcalendar.min.js" type="text/javascript"></script>

    <script src="../assets/plugins/jquery-easy-pie-chart/jquery.easy-pie-chart.js" type="text/javascript"></script>

    <script src="../assets/plugins/jquery.sparkline.min.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../assets/scripts/app.js" type="text/javascript"></script>

    <script src="../assets/scripts/index.js" type="text/javascript"></script>

    <script src="../assets/scripts/tasks.js" type="text/javascript"></script>

    <script src="../assets/plugins/bootstrap-sessiontimeout/jquery.sessionTimeout.min.js"
        type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->

    <script>
        jQuery(document).ready(function () {
            App.init();

            // initialize session timeout settings
            $.sessionTimeout({
                title: 'Session Timeout Notification',
                message: 'Your session is about to expire.',
                keepAliveUrl: 'demo/timeout-keep-alive.php',
                redirUrl: '../Lock.aspx',
                logoutUrl: '../Default.aspx',
                //        warnAfter: 5000, //warn after 5 seconds
                //redirAfter: 10000, //redirect after 10 secons
            });
        });
    </script>

    <!-- END JAVASCRIPTS -->
</asp:Content>
