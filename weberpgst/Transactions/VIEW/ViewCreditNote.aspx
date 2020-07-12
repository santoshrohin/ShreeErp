<%@ Page Title="Credit Note" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true"
    CodeFile="ViewCreditNote.aspx.cs" Inherits="Transactions_VIEW_ViewCreditNote" %>

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
        function RefreshUpdatePanel() {
            __doPostBack('<%= txtString.ClientID %>', '');
        };
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
                                <i class="fa fa-reorder"></i>Credit Note
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="linkBtn" runat="server" CssClass="remove" OnClick=" btnCancel_Click"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="btn-group">
                                        <label class="control-label">
                                            Search
                                        </label>
                                    </div>
                                    <div class="btn-group">
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtString" runat="server" CssClass="form-control input-medium" TabIndex="1"
                                                OnTextChanged="txtString_TextChanged" onkeyup="RefreshUpdatePanel();"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="btn-group pull-right">
                                        <asp:LinkButton ID="btnAddNew" TabIndex="2" CssClass="btn green" runat="server" OnClientClick="()"
                                            OnClick="btnAddNew_Click">Add New  <i class="fa fa-plus"></i></asp:LinkButton>
                                    </div>
                                    <div class="pull-right">
                                        &nbsp
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div style="overflow-x: auto;">
                                        <asp:UpdatePanel ID="UpdatePanel100" runat="server">
                                            <ContentTemplate>
                                                <asp:GridView ID="dgCreditNote" TabIndex="3" runat="server" AutoGenerateColumns="False" Width="100%"
                                                    CellPadding="4" Font-Size="12px" Font-Names="Verdana" GridLines="Both" DataKeyNames="CNM_CODE"
                                                    CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="True"
                                                    OnRowCommand="dgCreditNote_RowCommand" OnRowDeleting="dgCreditNote_RowDeleting"
                                                    OnPageIndexChanging="dgCreditNote_PageIndexChanging" PageSize="15">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <div class="clearfix">
                                                                    <div class="btn-group">
                                                                        <button type="button" TabIndex="4" class="btn blue btn-xs dropdown-toggle" data-toggle="dropdown">
                                                                            Select <i class="fa fa-angle-down"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu" role="menu">
                                                                            <li>
                                                                                <asp:LinkButton ID="lnkView" runat="server" CausesValidation="False" CommandName="View"
                                                                                    Text="View" CommandArgument='<%# Bind("CNM_CODE") %>'><i class="fa fa-search"></i>View</asp:LinkButton>
                                                                            </li>
                                                                            <li>
                                                                                <asp:LinkButton ID="lnkModify" runat="server" CausesValidation="False" CommandName="Modify"
                                                                                    Text="Modify" CommandArgument='<%# Bind("CNM_CODE") %>'><i class="fa fa-edit"></i> Modify</asp:LinkButton>
                                                                            </li>
                                                                            <li>
                                                                                <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                                                                    Text="Delete" OnClientClick="return confirm('Are you sure,you want to Delete?');"><i class="fa fa-trash-o"></i> Delete</asp:LinkButton>
                                                                            </li>
                                                                            <li>
                                                                                <asp:LinkButton ID="lnkPrint" runat="server" CausesValidation="False" CommandName="Print"
                                                                                    Text="Print" CommandArgument='<%# Bind("CNM_CODE") %>'><i class="fa fa-print"></i> Print</asp:LinkButton>
                                                                            </li>
                                                                        </ul>
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="CNM_CODE" SortExpression="CNM_CODE" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCNM_CODE" CssClass="formlabel" runat="server" Text='<%# Bind("CNM_CODE")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="SERIAL NO." SortExpression="CNM_CODE" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCNM_SERIAL_NO" runat="server" Text='<%# Eval("CNM_SERIAL_NO")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Customer Name" SortExpression="CNM_CODE" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblP_NAME" runat="server" Text='<%# Eval("P_NAME") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Customer Name" Visible="false" SortExpression="CNM_CODE" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblP_CODE" runat="server" Text='<%# Eval("P_CODE") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Credit Note Date" SortExpression="CNM_CODE" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCNM_DATE" CssClass=" Control-label" runat="server" Text='<%# Eval("CNM_DATE") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <AlternatingRowStyle CssClass="alt" />
                                                    <PagerStyle CssClass="pgr" />
                                                </asp:GridView>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="txtString" EventName="TextChanged" />
                                            </Triggers>
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
    <link href="../../assets/Avisos/Avisos.css" rel="stylesheet" type="text/css" />
    <script src="../../assets/Avisos/Avisos.js" type="text/javascript"></script> <script
    src="../../assets/JS/Util.js" type="text/javascript"></script> <script src="../../assets/scripts/jquery-1.7.1.min.js"
    type="text/javascript"></script> <script src="../../assets/plugins/jquery-1.10.2.min.js"
    type="text/javascript"></script> <script src="../../assets/plugins/jquery-migrate-1.2.1.min.js"
    type="text/javascript"></script> <script src="../../assets/plugins/bootstrap/js/bootstrap.min.js"
    type="text/javascript"></script> <script src="../../assets/plugins/bootstrap-hover-dropdown/twitter-bootstrap-hover-dropdown.min.js"
    type="text/javascript"></script> <script src="../../assets/plugins/jquery-slimscroll/jquery.slimscroll.min.js"
    type="text/javascript"></script> <script src="../../assets/plugins/jquery.blockui.min.js"
    type="text/javascript"></script> <script src="../../assets/plugins/jquery.cokie.min.js"
    type="text/javascript"></script> <script src="../../assets/plugins/uniform/jquery.uniform.min.js"
    type="text/javascript"></script> <!-- END CORE PLUGINS --> <!-- BEGIN PAGE LEVEL
    PLUGINS --> <script src="../../assets/plugins/bootstrap-daterangepicker/moment.min.js"
    type="text/javascript"></script> <script src="../../assets/plugins/bootstrap-daterangepicker/daterangepicker.js"
    type="text/javascript"></script> <script src="../../assets/plugins/gritter/js/jquery.gritter.js"
    type="text/javascript"></script> <script src="../../assets/scripts/app.js" type="text/javascript"></script>
    <!-- END PAGE LEVEL SCRIPTS -->
</asp:Content>
