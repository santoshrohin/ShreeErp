<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="ProductionDefault.aspx.cs"
    Inherits="Masters_ProductionDefault" Title="Production" %>

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

    <asp:UpdatePanel runat="server" ID="UpdatePanel2">
        <ContentTemplate>
            <asp:LinkButton ID="CheckCondition" runat="server" BackColor="" CssClass="formlabel"></asp:LinkButton>
            <cc1:ModalPopupExtender runat="server" ID="ModalPopupMsg" BackgroundCssClass="ModalPopupBG"
                OnOkScript="oknumber()" CancelControlID="Button7" DynamicServicePath="" Enabled="True"
                PopupControlID="popUpPanel5" TargetControlID="CheckCondition">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="popUpPanel5" runat="server" Style="display: none;">
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
                                    <div class="col-md-offset-2 col-md-10">
                                        <asp:LinkButton ID="Button5" CssClass="btn blue" TabIndex="26" runat="server" Visible="true"
                                            OnClick="btnOk_Click">  OK </asp:LinkButton>
                                        <asp:LinkButton ID="Button7" CssClass="btn default" TabIndex="28" runat="server"> Cancel</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="page-content-wrapper">
        <div class="page-content">
            <!-- BEGIN PAGE CONTENT-->
            <div class="row">
                <div class="col-md-12">
                    <div class="row">
                        <div class="col-md-4 sidebar-content ">
                            <ul class="ver-inline-menu tabbable margin-bottom-25">
                                <li class="active"><a href="#" data-toggle="tab"><i class="fa fa-briefcase"></i>Inward
                                    Transactions</a> <span class="after"></span></li>
                                <li id="Inwrd" runat="server"><a id="A13" href="#" onserverclick="btnMaterialInward_click"
                                    runat="server"><i class="fa fa-tasks"></i>Material Inward</a> </li>
                                <li id="Li1" runat="server"><a id="A7" href="#" onserverclick="btnSubContractorInward_click"
                                    runat="server"><i class="fa fa-tasks"></i>Sub Contractor Inward</a> </li>
                                <li id="Li2" runat="server"><a id="A10" href="#" onserverclick="btnCashInward_click"
                                    runat="server"><i class="fa fa-tasks"></i>Cash Inward</a> </li>
                                <li id="Li3" runat="server"><a id="A11" href="#" onserverclick="btnForProcessInward_click"
                                    runat="server"><i class="fa fa-tasks"></i>For Process Inward</a> </li>
                                <li id="CustRej" runat="server"><a id="A2" href="#" runat="server" onserverclick="btnCustomerRejection_click">
                                    <i class="fa fa-tasks"></i>Customer Rejection Inward</a> </li>
                                <li id="TurIWD" runat="server" visible="false"><a id="A23" href="#" runat="server" onserverclick="btnTurningIWD_click">
                                    <i class="fa fa-tasks"></i>Turning Inward</a> </li>
                                <li id="Li10" runat="server"><a id="A25" href="#" runat="server" onserverclick="btnServiceInward_click">
                                    <i class="fa fa-tasks"></i>Service Inward</a> </li>
                                 <li id="LiWithoutPOInward" runat="server"><a id="A26" href="#" runat="server" onserverclick="btnWithoutPOInward_click">
                                    <i class="fa fa-tasks"></i>Without PO Inward</a> </li>
                            </ul>
                        </div>
                        <div class="col-md-4 sidebar-content ">
                            <ul class="ver-inline-menu tabbable margin-bottom-25">
                                <li class="active"><a href="#" data-toggle="tab"><i class="fa fa-briefcase"></i>Other
                                    Transactions</a> <span class="after"></span></li>
                                <li id="MatReq" runat="server"><a id="A3" href="#" visible="false" onserverclick="btnMaterialRequisition_click"
                                    runat="server"><i class="fa fa-tasks"></i>Material Requisition</a> </li>
                                <li id="FillOffSheet" runat="server"><a id="A5" visible="false" href="#" runat="server"
                                    onserverclick="btnFillOffSheet_click"><i class="fa fa-tasks"></i>Fill Off Sheet</a></li>
                                <li id="IssueProd" runat="server"><a href="#" onserverclick="btnIssueToProduction_click"
                                    runat="server"><i class="fa fa-tasks"></i>Issue to Production</a> </li>
                                <li id="ProdStore" runat="server"><a id="A8" href="#" onserverclick="btnProductionToStore_click"
                                    runat="server"><i class="fa fa-tasks"></i>Production to Store</a> </li>
                                <li id="Inspection" runat="server"><a id="A14" href="#" onserverclick="btnMaterialInspection_click"
                                    runat="server"><i class="fa fa-tasks"></i>Material Inspection</a> </li>
                                <li id="Li11" runat="server"><a id="A9" href="#" runat="server" onserverclick="btnDispatchToSub_click">
                                    <i class="fa fa-tasks"></i>Dispatch To Sub Contracter</a> </li>
                                <li id="IssueFillOffSheet" visible="false" runat="server"><a id="A6" href="#" runat="server"
                                    onserverclick="btnIssueFillOffSheet_click"><i class="fa fa-tasks"></i>Issue Fill
                                    Off Sheet</a> </li>
                                <li id="StockAdj" runat="server"><a href="#" onserverclick="btnStockAdjustment_click"
                                    runat="server"><i class="fa fa-tasks"></i>Stock Adjustment</a> </li>
                                <li id="Li5" runat="server"><a id="A16" href="#" runat="server" onserverclick="btnDCRetrun_click">
                                    <i class="fa fa-tasks"></i>Delivery Challan Return</a> </li>
                                <li id="Li9" runat="server" visible="false"><a id="A24" href="#" runat="server" onserverclick="btnTrayDCRetrun_click">
                                    <i class="fa fa-tasks"></i>Tray Delivery Challan Return</a> </li>
                            </ul>
                        </div>
                        <div class="col-md-4 sidebar-content ">
                            <ul class="ver-inline-menu tabbable margin-bottom-25">
                                <li class="active"><a href="#" data-toggle="tab"><i class="fa fa-briefcase"></i>Reports</a>
                                    <span class="after"></span></li>
                                <li><a href="../../RoportForms/VIEW/ViewProdcutionToStoreRegister.aspx"><i class="fa fa-tasks">
                                </i>Production To Store Register</a> </li>
                                <li><a href="../../RoportForms/VIEW/ViewManufactureOrderRegister.aspx" runat="server"
                                    visible="false"><i class="fa fa-tasks"></i>Manufacture Order Register</a> </li>
                                <li id="MatReqReg" visible="false" runat="server"><a href="#" onserverclick="btnMaterialRequisitionRegister_Click"
                                    runat="server"><i class="fa fa-tasks"></i>Material Requisition Register</a></li>
                                <li id="IssueProdReg" runat="server"><a href="#" onserverclick="btnIssueToProductionRegister_Click"
                                    runat="server"><i class="fa fa-tasks"></i>Issue To Production Register</a> </li>
                                <li id="ProdReg" runat="server"><a href="#" onserverclick="btnProdcutionToStoreRegister_Click"
                                    runat="server"><i class="fa fa-tasks"></i>Production To Store Register</a> </li>
                                <li id="MatreqMisReg" visible="false" runat="server"><a id="A1" href="#" onserverclick="btnMaterialRequisitionMISReport_Click"
                                    runat="server"><i class="fa fa-tasks"></i>Material Requisition MIS Report</a></li>
                                <li id="Li4" runat="server"><a id="A12" href="#" runat="server" onserverclick="btnDispatchToSubReport_click">
                                    <i class="fa fa-tasks"></i>Dispatch To Sub Contracter Report</a> </li>
                                <li id="InwdReg" runat="server"><a id="A20" href="#" onserverclick="btnInwardSuppWise_click"
                                    runat="server"><i class="fa fa-tasks"></i>Material Inward Register</a> </li>
                                <li id="Inspreg" runat="server"><a id="A21" href="#" onserverclick="btnInspectionRegisterReport_click"
                                    runat="server"><i class="fa fa-tasks"></i>Inspection Register</a> </li>
                                <li id="StockAsjreg" runat="server"><a href="#" onserverclick="btnStockAdjustmentRegister_Click"
                                    runat="server"><i class="fa fa-tasks"></i>Stock Adjustment Register</a> </li>
                                <li id="CustRejReg" runat="server"><a id="A4" href="#" runat="server" onserverclick="btnCustomerRejectionRegister_click">
                                    <i class="fa fa-tasks"></i>Customer Rejection Register</a></li>
                                <li id="SubContStock" runat="server"><a id="A15" href="#" runat="server" onserverclick="btnSubContStockRegister_click">
                                    <i class="fa fa-tasks"></i>Subcontractor Stock Ledger</a></li>
                                <li id="StockReport" runat="server"><a id="A15ty" href="#" runat="server" onserverclick="btnStockLedger_click">
                                    <i class="fa fa-tasks"></i>Stock Register</a></li>
                                <li id="Li6" runat="server"><a id="A17" href="#" onserverclick="btnDCRetrunRegister_Click"
                                    runat="server"><i class="fa fa-tasks"></i>Delivery Challan Return Register</a></li>
                                <li id="Li7" runat="server" visible="false"><a id="A18" href="#" runat="server" onserverclick="btnTrayStockLedger_click">
                                    <i class="fa fa-tasks"></i>Tray Stock Register</a></li>
                                <li id="Li8" runat="server"><a id="A19" href="#" runat="server" onserverclick="btnVendorStockLedger_click">
                                    <i class="fa fa-tasks"></i>Vendor Stock Register</a></li>
                                <li id="TurningRegister" runat="server" visible="false"><a id="A22" href="#" runat="server" onserverclick="btnTurning_click">
                                    <i class="fa fa-tasks"></i>Turning Register</a></li>
                                <li id="CustStockReg" runat="server"><a id="CustStockReg23" href="#" runat="server"
                                    onserverclick="btnCustStockReg_click"><i class="fa fa-tasks"></i>Customer Stock
                                    Register</a></li>
                            </ul>
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

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->
</asp:Content>
