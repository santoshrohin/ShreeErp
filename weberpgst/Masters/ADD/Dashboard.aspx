<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs"
    Inherits="Masters_ADD_Dashboard" Title="Dashboard" %>

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