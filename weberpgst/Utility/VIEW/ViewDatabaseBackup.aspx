<%@ Page Title="Database Backup Utility" Language="C#" MasterPageFile="~/main.master" 
    AutoEventWireup="true" CodeFile="ViewDatabaseBackup.aspx.cs" Inherits="Utility_VIEW_ViewDatabaseBackup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">

    

    <div class="page-content-wrapper">
        <div class="page-content">
            <div class="row">
                <div class="col-md-2">
                </div>
                <div class="col-md-8">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="PanelMsg" runat="server" Visible="false" 
                                Style="background-color: #feefb3; height: 50px; width: 100%; border: 1px solid #9f6000">
                                <div style="vertical-align: middle; margin-top: 10px;">
                                    <asp:Label ID="lblmsg" runat="server" Style="color: #9f6000; font-size: medium; font-weight: bold; margin-left: 10px;"></asp:Label>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-2">
                </div>
                <div class="col-md-8">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-database"></i>Database Backup Utility
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                            </div>
                        </div>
                        <div class="portlet-body">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="btn-group">
                                        <label class="control-label">
                                            Database Backup
                                        </label>
                                    </div>
                                    <div class="btn-group">
                                        <asp:Button ID="btnBackup" runat="server" Text="Backup Database" 
                                            CssClass="btn green" OnClick="btnBackup_Click" />
                                    </div>
                                    <div class="btn-group pull-right">
                                        <asp:Button ID="btnDownload" runat="server" Text="Download Backup" 
                                            CssClass="btn blue" OnClick="btnDownload_Click" />
                                    </div>
                                    <div class="pull-right">
                                        &nbsp;
                                    </div>
                                </div>
                            </div>
                            <asp:UpdatePanel ID="UpdatePanel100" runat="server">
                                <ContentTemplate>
                                    <!-- Add any relevant messages or logs here -->
                                    <asp:Label ID="lblBackupStatus" runat="server" Text="Backup status will be shown here."
                                        Style="font-size: medium; color: green;"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script src="../../assets/JS/Util.js" type="text/javascript"></script>
    <script src="../../assets/scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../../assets/plugins/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../../assets/plugins/jquery.blockui.min.js" type="text/javascript"></script>
    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

</asp:Content>
