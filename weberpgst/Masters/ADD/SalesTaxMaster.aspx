<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="SalesTaxMaster.aspx.cs"
    Inherits="Masters_ADD_SectorMaster" Title="Sales Tax Master" %>

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
        }
    </script>
    <script type="text/javascript">
        function Showalert1() {
            $('#MSG').fadeIn(6000)
            $('#MSG').fadeOut(6000)
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
                    <asp:UpdatePanel ID="UpdatePanel22" runat="server">
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
                                <i class="fa fa-reorder"></i>Sales Tax Master
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <%--<a href="javascript:;" class="remove"></a>--%>
                                <asp:LinkButton ID="LinkDtn" CssClass="remove" OnClick="btnCancel_Click" runat="server"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-5 control-label text-right">
                                                <span class="required">*</span> Tax Name
                                            </label>
                                            <div class="col-md-4">
                                                <asp:TextBox CssClass="form-control " ID="txtTaxName" placeholder="TaxName" TabIndex="1"
                                                    runat="server" MaxLength="50" MsgObrigatorio="Tax Name"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-5 control-label text-right">
                                                Alias(Short Name)
                                            </label>
                                            <div class="col-md-4">
                                                <asp:TextBox CssClass="form-control " ID="txtAlias" placeholder="Alias" TabIndex="2"
                                                    runat="server" MaxLength="10"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-5 control-label text-right">
                                                <span class="required">*</span> Sales Tax (%)
                                            </label>
                                            <div class="col-md-4">
                                                <asp:UpdatePanel runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control text-right " ID="txtSalesTax" placeholder="0.00"
                                                            TabIndex="3"  runat="server" OnTextChanged="txtSalesTax_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" TargetControlID="txtSalesTax"
                                                            ValidChars="0123456789." runat="server" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-5 control-label text-right">
                                                TCS (%)
                                            </label>
                                            <div class="col-md-4">
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox CssClass="form-control text-right " ID="txtTCSTax" placeholder="0.00"
                                                            TabIndex="4" runat="server" OnTextChanged="txtTCSTax_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtTCSTax"
                                                            ValidChars="0123456789." runat="server" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                    <%-- <div class="row">
                                        <div class="form-group">
                                            
                                            <label class="col-md-5 control-label text-right">
                                                Set Off
                                            </label>
                                            <div class="col-md-4">
                                                <asp:TextBox CssClass="form-control text-right" ID="txtSetOff" placeholder="0.00" TabIndex="5" runat="server" MaxLength="50" ></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="txtSetOff"
ValidChars="0123456789." runat="server" />
                                            </div>
                                        </div>
                                    </div>--%>
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-5 control-label text-right">
                                                Form No
                                            </label>
                                            <div class="col-md-4">
                                                <asp:TextBox CssClass="form-control " ID="txtFormNo" placeholder="Form Number" TabIndex="5"
                                                    runat="server"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" TargetControlID="txtFormNo"
                                                    ValidChars="0123456789." runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-5 control-label text-right">
                                               <span class="required">*</span> Tally Acc. Head (Sales)
                                            </label>
                                            <div class="col-md-4">
                                                <%--<asp:TextBox CssClass="form-control " ID="txtSalesAccHead" placeholder="Sales Acc. Head" TabIndex="7" runat="server" MaxLength="50" ></asp:TextBox>--%>
                                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlTaxAccHeadSales" CssClass="select2_category form-control input-sm"
                                                            runat="server" MsgObrigatorio="Tax Acc Head for Purchase" TabIndex="6">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-5 control-label text-right">
                                               <span class="required">*</span> Tax Acc. Head (Purchase)
                                            </label>
                                            <div class="col-md-4">
                                                <%--<asp:TextBox CssClass="form-control " ID="txtTaxAccHead" placeholder="Tax Acc. Head" TabIndex="8" runat="server" MaxLength="50" ></asp:TextBox>--%>
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlTaxAccHeadPurchase" CssClass="select2_category form-control input-sm"
                                                            runat="server" MsgObrigatorio="Tax Acc Head for Purchase" TabIndex="7">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                     <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-5 control-label text-right">
                                               <span class="required">*</span> Tally Tax Acc (Sales)
                                            </label>
                                            <div class="col-md-4">
                                                <%--<asp:TextBox CssClass="form-control " ID="txtSalesAccHead" placeholder="Sales Acc. Head" TabIndex="7" runat="server" MaxLength="50" ></asp:TextBox>--%>
                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlTaxSales" CssClass="select2_category form-control input-sm"
                                                            runat="server" MsgObrigatorio="Tally Tax Acc For Sales" TabIndex="8">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-md-5 control-label text-right">
                                               <span class="required">*</span> Tally Tax Acc (Purchase)
                                            </label>
                                            <div class="col-md-4">
                                                <%--<asp:TextBox CssClass="form-control " ID="txtTaxAccHead" placeholder="Tax Acc. Head" TabIndex="8" runat="server" MaxLength="50" ></asp:TextBox>--%>
                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlTaxPurchase" CssClass="select2_category form-control input-sm"
                                                            runat="server" MsgObrigatorio="Tally Tax Acc For Purchase" TabIndex="9">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-actions fluid">
                                <div class="col-md-offset-5 col-md-9">
                                    <asp:LinkButton ID="btnSubmit" CssClass="btn green" TabIndex="10" runat="server" OnClick="btnSubmit_Click"
                                        OnClientClick="return VerificaCamposObrigatorios();"><i class="fa fa-check-square"> </i>  Save </asp:LinkButton>
                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="11" runat="server"
                                        OnClick="btnCancel_Click"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
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
                                                        Do you Want to Cancel Record?
                                                    </label>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <%-- <asp:UpdatePanel runat="server" ID="UpdatePanel7">
                                                                        <ContentTemplate>
                                                                <asp:RadioButtonList ID="rbtType" runat="server" TabIndex="1" RepeatDirection="Vertical"
                                                                    CssClass="checker" CellPadding="15">
                                                                    <asp:ListItem Value="0" Selected="True">Export Invoice</asp:ListItem>
                                                                    <asp:ListItem Value="1">Export Invoice - Domestic Print</asp:ListItem>
                                                                    <asp:ListItem Value="2">ARE 1 Invoice</asp:ListItem>
                                                                    <asp:ListItem Value="3">Packaging List</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                                   </ContentTemplate>
                                                                    </asp:UpdatePanel>--%>
                                                    </div>
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
                                                    <div class="col-md-9">
                                                    </div>
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
                                                    <div class="col-md-9">
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-9">
                                                    </div>
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

    <script type="text/javascript">
        function VerificaCamposObrigatorios() {
            try {
                if (VerificaObrigatorio('#<%=txtTaxName.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }              
                else if (VerificaValorCombo('#<%=ddlTaxAccHeadSales.ClientID%>', '#Avisos') == false) {

                $("#Avisos").fadeOut(6000); 
                return false;
                }

                else if (VerificaValorCombo('#<%=ddlTaxAccHeadPurchase.ClientID%>', '#Avisos') == false) {
                $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaValorCombo('#<%=ddlTaxSales.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    return false;
                }
                else if (VerificaValorCombo('#<%=ddlTaxPurchase.ClientID%>', '#Avisos') == false) {
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
        //-->
    </script>

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
