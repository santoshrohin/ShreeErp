<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="AccountDefault.aspx.cs"
    Inherits="Masters_AccountDefault" Title="Account" %>

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
                                            OnClick="btnOk_Click"> OK </asp:LinkButton>
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
                                <li class="active"><a href="#" data-toggle="tab"><i class="fa fa-briefcase"></i>Masters</a>
                                    <span class="after"></span></li>
                                <li id="LiAccountMaster" runat="server" visible="true"><a href="../../Account/Masters/VIEW/ViewCoreInventoryRegister.aspx">
                                    <i class="fa fa-tasks"></i>Account Master</a> </li>
                                <li id="LiAccntLedger" runat="server" visible="true"><a href="../../Account/Masters/VIEW/ViewAccountLedgerMaster.aspx">
                                    <i class="fa fa-tasks"></i>Account Ledger</a> </li>
                                <li id="LiCashBookEntry" runat="server" visible="true"><a href="../../Account/Masters/VIEW/ViewCashBookEntry.aspx">
                                    <i class="fa fa-tasks"></i>Cash Book Entry</a> </li>
                                <li id="LiNewReferenceEntry" runat="server" visible="true"><a href="../../Account/Masters/VIEW/ViewReferenceMaster.aspx">
                                    <i class="fa fa-tasks"></i>Reference Entry</a> </li>
                                <li id="Li1" runat="server" visible="true"><a href="../../Account/Masters/VIEW/ViewRemark.aspx">
                                    <i class="fa fa-tasks"></i>Remark Master</a> </li>
                            </ul>
                        </div>
                        <div class="col-md-4 sidebar-content ">
                            <ul class="ver-inline-menu tabbable margin-bottom-25">
                                <li class="active"><a href="#" data-toggle="tab"><i class="fa fa-briefcase"></i>Transactions</a>
                                    <span class="after"></span></li>
                                <li id="CreditNote" runat="server"><a href="../../Transactions/VIEW/ViewCreditNote.aspx"
                                    onserverclick="btnCreditNote_click"><i class="fa fa-tasks"></i>Credit Note </a>
                                </li>
                                <li id="DebitNote" runat="server"><a href="../../Transactions/VIEW/ViewDebitNote.aspx"
                                    onserverclick="btnDebitNote_click"><i class="fa fa-tasks"></i>Debit Note </a>
                                </li>
                                <li id="LiOutstandingTrans" runat="server" visible="true"><a href="../../Account/Transactions/VIEW/ViewRecieptEntry.aspx">
                                    <i class="fa fa-tasks"></i>Reciept Entry</a> </li>
                                <li id="Lipaymententry" runat="server" visible="true"><a href="../../Account/Transactions/VIEW/ViewPaymentEntry.aspx">
                                    <i class="fa fa-tasks"></i>Payment Entry</a> </li>
                                <li id="Li2" runat="server" visible="true"><a href="../../Account/Transactions/VIEW/ViewOpening.aspx">
                                    <i class="fa fa-tasks"></i>Opening Balance Entry</a> </li>
                            </ul>
                        </div>
                        <div class="col-md-4 sidebar-content ">
                            <ul class="ver-inline-menu tabbable margin-bottom-25">
                                <li class="active"><a href="#" data-toggle="tab"><i class="fa fa-briefcase"></i>Reports</a>
                                    <span class="after"></span></li>
                                <li id="LiOutstandingReport" runat="server" visible="true"><a href="../../Account/ReportForms/VIEW/ViewOutstandingReport.aspx">
                                    <i class="fa fa-tasks"></i>Outstanding Report</a> </li>
                                <li id="LiViewCashBookRegister" runat="server" visible="true"><a href="../../Account/ReportForms/VIEW/ViewCashBookRegister.aspx">
                                    <i class="fa fa-tasks"></i>Cash Book Register</a> </li>
                                <li id="LiReceiptRegister" runat="server" visible="true"><a href="../../Account/ReportForms/VIEW/ViewReceiptRegister.aspx">
                                    <i class="fa fa-tasks"></i>Receipt Register</a> </li>
                                <li id="LiPaymentRegister" runat="server" visible="true"><a href="../../Account/ReportForms/VIEW/ViewPaymentRegister.aspx">
                                    <i class="fa fa-tasks"></i>Payment Register</a> </li>
                                <li id="LiAccountLedger" runat="server" visible="true"><a href="../../Account/ReportForms/VIEW/ViewAccountLedger.aspx">
                                    <i class="fa fa-tasks"></i>Account Ledger</a> </li>
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
