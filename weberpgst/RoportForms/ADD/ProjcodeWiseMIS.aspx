<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="ProjcodeWiseMIS.aspx.cs"
    Inherits="RoportForms_ADD_ProjcodeWiseMIS" Title="Project Code wise Supplier Purchase Order - MIS Valuation Report" %>

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
    <div class="page-content-wrapper">
        <div class="page-content">
            <!-- BEGIN PAGE CONTENT-->
            <div id="Avisos">
            </div>
            <div class="row">
                <div class="col-md-1">
                </div>
                <div class="col-md-10">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Project Code wise Supplier Purchase Order
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                              <%--  <asp:LinkButton ID="LinkDtn" CssClass="remove" OnClick="btnCancel_Click" runat="server"></asp:LinkButton>--%>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:UpdatePanel runat="server" ID="UpdatePanel33">
                                                <ContentTemplate>
                                                    <asp:LinkButton ID="CheckCondition" runat="server" BackColor="" CssClass="formlabel"></asp:LinkButton>
                                                    <cc1:modalpopupextender runat="server" id="ModalCancleConfirmation" backgroundcssclass="modalBackground"
                                                        onokscript="oknumber1()" oncancelscript="oncancel()" dynamicservicepath="" enabled="True"
                                                        popupcontrolid="popUpPanel5" targetcontrolid="CheckCondition">
                                                     </cc1:modalpopupextender>
                                                    <asp:Panel ID="popUpPanel5" runat="server" CssClass="table-responsive" Height="400px"
                                                        Width="1200px">
                                                        <div class="portlet box blue">
                                                            <div class="portlet-title">
                                                                <div class="captionPopup">
                                                                Project Code wise 
                                                                </div>
                                                            </div>
                                                            <div class="portlet-body form">
                                                                <div class="form-horizontal">
                                                                    <div class="form-body">
                                                                        <div class="row">
                                                                            <div class="col-md-12">
                                                                                <div class="row">
                                                                                    <div class="col-md-12" id="Div2">
                                                                                        <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                                                                                            <ContentTemplate>
                                                                                                <asp:Panel ID="PnlMsgDailyAct" runat="server" Visible="false" Style="background-color: #feefb3;
                                                                                                    height: 40px; width: 100%; border: 1px solid #9f6000">
                                                                                                    <div style="vertical-align: middle; margin-top: 10px;">
                                                                                                        <asp:Label ID="lblDailyActMsg" runat="server" Style="color: #9f6000; font-size: medium;
                                                                                                            font-weight: bold; margin-top: 5px; margin-left: 10px;"></asp:Label>
                                                                                                    </div>
                                                                                                </asp:Panel>
                                                                                            </ContentTemplate>
                                                                                        </asp:UpdatePanel>
                                                                                    </div>
                                                                                </div>
                                                                                <br />
                                                                                <div class="row">
                                                                                    <div class="col-md-12">
                                                                                        <div class="form-group">
                                                                                            <div class="col-md-1">
                                                                                            </div>
                                                                                            <label class="col-md-1 control-label label-sm">
                                                                                                Status
                                                                                            </label>
                                                                                            
                                                                                            
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="row">
                                                                                    <div class="col-md-12">
                                                                                        <div style="overflow: auto;">
                                                                                            <div class="table-responsive">
                                                                                                <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                                                                                                    <ContentTemplate>
                                                                                                        <asp:GridView ID="dgActivity_Task" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                                                                            Font-Size="12px" ShowFooter="false" Font-Names="Verdana" GridLines="Both" CssClass="table table-striped table-bordered table-advance table-hover"
                                                                                                            DataKeyNames="ACT_CODE" OnRowCommand="dgActivity_Task_RowCommand"  
                                                                                                            AllowPaging="true" PageSize="5">
                                                                                                            <Columns>
                                                                                                                <asp:TemplateField HeaderText="ACT_CODE" SortExpression="ACT_CODE" Visible="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblACT_CODE" runat="server" Text='<%# Bind("ACT_CODE") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField Visible="true" HeaderText="Activity No">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:LinkButton ID="lnkModify" runat="server" CausesValidation="False" CommandName="Modify"
                                                                                                                            Text='<%# Bind("ACT_NO") %>' CommandArgument='<%# Bind("ACT_CODE") %>'><i class="fa fa-edit"></i></asp:LinkButton>
                                                                                                                        <%-- <asp:LinkButton ID="LnkBOCModify" runat="server" OnClick="LnkBOCModify_Click">BOC</asp:LinkButton>--%>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Activity No" Visible="false" SortExpression="ACT_NO"
                                                                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblACT_NO" CssClass="" runat="server" Text='<%# Eval("ACT_NO") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Activity Date" SortExpression="ACT_DATE" HeaderStyle-HorizontalAlign="Left">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblACT_DATE" CssClass="" runat="server" Text='<%# Eval("ACT_DATE") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Activity Status" SortExpression="ACT_STATUS" HeaderStyle-HorizontalAlign="Left">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblACT_STATUS" CssClass="" runat="server" Text='<%# Eval("ACT_STATUS") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Customer Name" SortExpression="CE_P_NAME" HeaderStyle-HorizontalAlign="Left">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblCE_P_NAME" CssClass="" runat="server" Text='<%# Eval("CE_P_NAME") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Contact Person" SortExpression="ACT_PERSON_NAME" HeaderStyle-HorizontalAlign="Left">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblACT_PERSON_NAME" CssClass="" runat="server" Text='<%# Eval("ACT_PERSON_NAME") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Schedule Date" SortExpression="ACT_SCH_DATE" HeaderStyle-HorizontalAlign="Left">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblACT_SCH_DATE" CssClass="" runat="server" Text='<%# Eval("ACT_SCH_DATE") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Assigned To" SortExpression="ACT_U_NAME" HeaderStyle-HorizontalAlign="Left">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblACT_U_NAME" CssClass="" runat="server" Text='<%# Eval("ACT_U_NAME") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Created By" SortExpression="ACT_CREATED_BY" HeaderStyle-HorizontalAlign="Left">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblACT_CREATED_BY" CssClass="" runat="server" Text='<%# Eval("ACT_CREATED_BY") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Activity Type" SortExpression="ACT_CODE" HeaderStyle-HorizontalAlign="Left">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblActivityType" CssClass="" runat="server" Text='<%# Eval("ACT_TYPE") %>'></asp:Label>
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
                                                                    <div class="row">
                                                                        <div class="col-md-offset-5 col-md-9">
                                                                            <asp:LinkButton ID="Button6" CssClass="btn blue" TabIndex="26" runat="server" Visible="true"
                                                                                OnClick="btnConfirm_Click"> Confirm </asp:LinkButton>
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
                    </div>
                </div>
            </div>
        </div>
        <!-- END PAGE CONTENT-->
    </div>
    <!-- BEGIN JAVASCRIPTS(Load javascripts at bottom, this will reduce page load time) -->
    <!-- BEGIN CORE PLUGINS -->
    <!--[if lt IE 9]>
<script src="assets/plugins/respond.min.js"></script>
<script src="assets/plugins/excanvas.min.js"></script>
<![endif]-->
    <link href="../../assets/Avisos/Avisos.css" rel="stylesheet" type="text/css" />

    <script src="../../assets/Avisos/Avisos.js" type="text/javascript"></script>

    <script src="../../assets/JS/Util.js" type="text/javascript"></script>

    <script src="../../assets/scripts/jquery-1.7.1.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery-migrate-1.2.1.min.js" type="text/javascript"></script>

    <!-- IMPORTANT! Load jquery-ui-1.10.3.custom.min.js before bootstrap.min.js to fix bootstrap tooltip conflict with jquery ui tooltip -->

    <script src="../../../assets/plugins/jquery-ui/jquery-ui-1.10.3.custom.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-hover-dropdown/twitter-bootstrap-hover-dropdown.min.js"
        type="text/javascript"></script>

    <script src="../../assets/plugins/jquery-slimscroll/jquery.slimscroll.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery.blockui.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery.cokie.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/uniform/jquery.uniform.min.js" type="text/javascript"></script>

    <!-- END CORE PLUGINS -->
    <!-- BEGIN PAGE LEVEL PLUGINS -->

    <script src="../../../assets/plugins/flot/jquery.flot.js" type="text/javascript"></script>

    <script src="../../../assets/plugins/flot/jquery.flot.resize.js" type="text/javascript"></script>

    <script src="../../../assets/plugins/jquery.pulsate.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-daterangepicker/moment.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-daterangepicker/daterangepicker.js" type="text/javascript"></script>

    <script src="../../assets/plugins/gritter/js/jquery.gritter.js" type="text/javascript"></script>

    <!-- IMPORTANT! fullcalendar depends on jquery-ui-1.10.3.custom.min.js for drag & drop support -->

    <script src="../../../assets/plugins/fullcalendar/fullcalendar/fullcalendar.min.js"
        type="text/javascript"></script>

    <script src="../../../assets/plugins/jquery-easy-pie-chart/jquery.easy-pie-chart.js"
        type="text/javascript"></script>

    <script src="../../../assets/plugins/jquery.sparkline.min.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <script src="../../../assets/scripts/index.js" type="text/javascript"></script>

    <script src="../../../assets/scripts/tasks.js" type="text/javascript"></script>

    <script src="../../../assets/plugins/bootstrap-sessiontimeout/jquery.sessionTimeout.min.js"
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
    <%--<script type="text/javascript">
        function VerificaCamposObrigatorios() {
            try {
                if (VerificaObrigatorio('#<%=txtUnitName.ClientID%>', '#Avisos') == false) {
                    return false;
                }
                else if(VerificaObrigatorio('#<%=txtUnitDesc.ClientID%>', '#Avisos') == false) {
                    {
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
        //-->
    </script>--%>
    <%--<link href="../../assets/css/template.css" rel="stylesheet" type="text/css" />
    <link href="../../assets/css/validationEngine.jquery.css" rel="stylesheet" type="text/css" />
    
    <script src="../../assets/scripts/jquery-1.6.min.js" type="text/javascript"></script>
 
    <script src="../../assets/scripts/jquery.validationEngine-en.js" type="text/javascript"></script>

    <script src="../../assets/scripts/jquery.validationEngine.js" type="text/javascript"></script>
       
    <script type="text/javascript">
    jQuery(document).ready(function () {
                                    jQuery('#' + '<%=Master.FindControl("form1").ClientID %>').validationEngine();
              
                                  
              });
           </script>--%>
    <!-- END PAGE LEVEL SCRIPTS -->
</asp:Content>
