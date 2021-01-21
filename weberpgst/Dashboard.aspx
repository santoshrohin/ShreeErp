<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="Dashboard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
  <meta charset="utf-8">
  <meta http-equiv="X-UA-Compatible" content="IE=edge">
  <title>Dashboard</title>
  <!-- Tell the browser to be responsive to screen width -->
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <!-- Bootstrap 4 -->

  <!-- Font Awesome -->
  <link rel="stylesheet" href="plugins/fontawesome-free/css/all.min.css">
  <!-- Ionicons -->
  <link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css">
  <!-- DataTables -->
  <link rel="stylesheet" href="plugins/datatables-bs4/css/dataTables.bootstrap4.min.css">
  <link rel="stylesheet" href="plugins/datatables-responsive/css/responsive.bootstrap4.min.css">
  <!-- Theme style -->

  <link rel="stylesheet" href="dist/css/adminlte.min.css">
  <!-- Google Font: Source Sans Pro -->
  <link href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700" rel="stylesheet">

  <style>
    .color-palette {
      height: 35px;
      line-height: 35px;
      text-align: right;
      padding-right: .75rem;
    }
    
    .color-palette.disabled {
      text-align: center;
      padding-right: 0;
      display: block;
    }
    
    .color-palette-set {
      margin-bottom: 15px;
    }

    .color-palette span {
      display: none;
      font-size: 12px;
    }

    .color-palette:hover span {
      display: block;
    }

    .color-palette.disabled span {
      display: block;
      text-align: left;
      padding-left: .75rem;
    }

    .color-palette-box h4 {
      position: absolute;
      left: 1.25rem;
      margin-top: .75rem;
      color: rgba(255, 255, 255, 0.8);
      font-size: 12px;
      display: block;
      z-index: 7;
    }
  </style>
</head>
<body class="hold-transition sidebar-mini">
<div class="wrapper">
  <!-- Navbar -->
  <nav class="main-header navbar navbar-expand navbar-white navbar-light">
    <!-- Left navbar links -->
    <ul class="navbar-nav">
      <li class="nav-item">
        <a class="nav-link" data-widget="pushmenu" href="#" role="button"><i class="fas fa-bars"></i></a>
      </li>
      <li class="nav-item d-none d-sm-inline-block">
        <a href="../../index3.html" class="nav-link">Home</a>
      </li>
      
    </ul>

    
  </nav>
  <!-- /.navbar -->

  <!-- Main Sidebar Container -->
  <aside class="main-sidebar sidebar-dark-primary elevation-4">
    <!-- Brand Logo -->
    <a href="../../index3.html" class="brand-link">
      <img src="dist/img/AdminLTELogo.png"
           alt="AdminLTE Logo"
           class="brand-image img-circle elevation-3"
           style="opacity: .8">
      <span class="brand-text font-weight-light">One Click ERP</span>
    </a>

    <!-- Sidebar -->
    <div class="sidebar">
      

      <!-- Sidebar Menu -->
      <nav class="mt-2">
        <ul class="nav nav-pills nav-sidebar flex-column" data-widget="treeview" role="menu" data-accordion="false">
         
          <li class="nav-item has-treeview">
            <a href="#" class="nav-link">
              <i class="nav-icon fas fa-chart-pie"></i>
              <p>
                Charts
                <i class="right fas fa-angle-left"></i>
              </p>
            </a>
            <ul class="nav nav-treeview">
              <li class="nav-item">
                <a href="../charts/chartjs.html" class="nav-link">
                  <i class="far fa-circle nav-icon"></i>
                  <p>ChartJS</p>
                </a>
              </li>
              <li class="nav-item">
                <a href="../charts/flot.html" class="nav-link">
                  <i class="far fa-circle nav-icon"></i>
                  <p>Flot</p>
                </a>
              </li>
              <li class="nav-item">
                <a href="../charts/inline.html" class="nav-link">
                  <i class="far fa-circle nav-icon"></i>
                  <p>Inline</p>
                </a>
              </li>
            </ul>
          </li>
          <li class="nav-item has-treeview menu-open">
            <a href="#" class="nav-link active">
              <i class="nav-icon fas fa-tree"></i>
              <p>
                All Dashboard
                <i class="fas fa-angle-left right"></i>
              </p>
            </a>
            <ul class="nav nav-treeview">
              <li class="nav-item">
                <a href="../UI/general.html" class="nav-link active">
                  <i class="far fa-circle nav-icon"></i>
                  <p>Dashboard</p>
                </a>
              </li>
              
            </ul>
          </li>
          
          
        </ul>
      </nav>
      <!-- /.sidebar-menu -->
    </div>
    <!-- /.sidebar -->
  </aside>
  
  <!-- Content Wrapper. Contains page content -->
  <div class="content-wrapper">
    <!-- Content Header (Page header) -->
    <section class="content-header">
      <div class="container-fluid">
        <div class="row mb-2">
          <div class="col-sm-6">
            <h1>Dashboard Details</h1>
          </div>
          
        </div>
      </div><!-- /.container-fluid -->
    </section>

    <!-- Main content -->
    <section class="content">
      <div class="container-fluid">
        
        
        <h5 class="mt-4 mb-2">Sales & Purchase in Tabs</h5>

        <div class="row">
          <div class="col-12">
            <!-- Custom Tabs -->
            
<div class="col-md-12">
            <div class="card card-primary">
              <div class="card-header">
                <h3 class="card-title">Till Date Sale Of Entire Group Year 2020-21 <span>2500.00</span></h3>

                <div class="card-tools">
                  <button type="button" class="btn btn-tool" data-card-widget="collapse"><i class="fas fa-minus"></i>
                  </button>
                </div>
                <!-- /.card-tools -->
              </div>
              <!-- /.card-header -->
              <div class="card-body" style="display: block;">
                <div class="card">
              <div class="card-header d-flex p-0">
                <h3 class="card-title p-3">Tabs</h3>
                <ul class="nav nav-pills ml-auto p-2">
                  <li class="nav-item"><a class="nav-link active" href="#tab_1" data-toggle="tab">Entire Group</a></li>
                  <li class="nav-item"><a class="nav-link" href="#tab_2" data-toggle="tab">SPC</a></li>
                  <li class="nav-item"><a class="nav-link" href="#tab_3" data-toggle="tab">Qualitat</a></li>
				  <li class="nav-item"><a class="nav-link" href="#tab_4" data-toggle="tab">Calidad</a></li>
                  
                    
                  </li>
                </ul>
              </div><!-- /.card-header -->
              <div class="card-body">
                <div class="tab-content">
                  <div class="tab-pane active" id="tab_1">
<div class="row">
          <div class="col-md-12">
            <div class="card card-primary collapsed-card">
              <div class="card-header">
                <h3 class="card-title">Sale Summary</h3>

                <div class="card-tools">
                  <button type="button" class="btn btn-tool" data-card-widget="collapse"><i class="fas fa-plus"></i>
                  </button>
                </div>
                <!-- /.card-tools -->
              </div>
              <!-- /.card-header -->
              <div class="card-body" style="display: none;">
                <div class="row bg-gradient-olive"><span >Last Month Sale</span></div>
                <div class="row">
                  <div class="col-md-3 col-sm-6 col-12">
                    <div class="info-box">
                      <span class="info-box-icon bg-info elevation-1"><i class="fas fa-cog"></i></span>
        
                      <div class="info-box-content">
                        <span class="info-box-text">Group Basic Sale LM</span>
                        <span class="info-box-number"> <asp:Label runat="server"  ID="totalLM"></asp:Label>                          </span>
                      </div>
                      <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                  </div>
                  <!-- /.col -->
                  <div class="col-md-3 col-sm-6 col-12">
                    <div class="info-box">
                      <span class="info-box-icon bg-info elevation-1"><i class="fas fa-cog"></i></span>
        
                      <div class="info-box-content">
                        <span class="info-box-text">SPC Basic Sale LM</span>
                        <span class="info-box-number"><asp:Label runat="server"  ID="SPCLM"></asp:Label>  
                        </span>
                      </div>
                      <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                  </div>
                  <!-- /.col -->
                  <div class="col-md-3 col-sm-6 col-12">
                    <div class="info-box">
                      <span class="info-box-icon bg-info elevation-1"><i class="fas fa-cog"></i></span>
        
                      <div class="info-box-content">
                        <span class="info-box-text">Qualitat Basic Sale LM</span>
                        <span class="info-box-number"><asp:Label runat="server"  ID="QualitatLM"></asp:Label>  
                        </span>
                      </div>
                      <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                  </div>
                  <!-- /.col -->
                  <div class="col-md-3 col-sm-6 col-12">
                    <div class="info-box">
                      <span class="info-box-icon bg-info elevation-1"><i class="fas fa-cog"></i></span>
        
                      <div class="info-box-content">
                        <span class="info-box-text">Calidad Basic Sale LM</span>
                        <span class="info-box-number"><asp:Label runat="server"  ID="CalidadLM"></asp:Label>  
                        </span>
                      </div>
                      <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                  </div>
                  <!-- /.col -->
                
                
                
                </div>
                <div class="row">
                  <div class="col-md-3 col-sm-6 col-12">
                    <div class="info-box">
                      <span class="info-box-icon bg-info elevation-1"><i class="fas fa-cog"></i></span>
        
                      <div class="info-box-content">
                        <span class="info-box-text">Group Sales With Tax LM</span>
                        <span class="info-box-number"><asp:Label runat="server"  ID="totalTaxSales"></asp:Label>  
                        </span>
                      </div>
                      <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                  </div>
                  <!-- /.col -->
                  <div class="col-md-3 col-sm-6 col-12">
                    <div class="info-box">
                      <span class="info-box-icon bg-info elevation-1"><i class="fas fa-cog"></i></span>
        
                      <div class="info-box-content">
                        <span class="info-box-text">SPC Sales With Tax LM</span>
                        <span class="info-box-number"><asp:Label runat="server"  ID="SPCTaxLM"></asp:Label>  
                        </span>
                      </div>
                      <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                  </div>
                  <!-- /.col -->
                  <div class="col-md-3 col-sm-6 col-12">
                    <div class="info-box">
                      <span class="info-box-icon bg-info elevation-1"><i class="fas fa-cog"></i></span>
        
                      <div class="info-box-content">
                        <span class="info-box-text">Qualitat Sales With Tax LM</span>
                        <span class="info-box-number"><asp:Label runat="server"  ID="QualitatTaxLM"></asp:Label>  
                        </span>
                      </div>
                      <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                  </div>
                  <!-- /.col -->
                  <div class="col-md-3 col-sm-6 col-12">
                    <div class="info-box">
                      <span class="info-box-icon bg-info elevation-1"><i class="fas fa-cog"></i></span>
        
                      <div class="info-box-content">
                        <span class="info-box-text">Calidad Sales With Tax LM</span>
                        <span class="info-box-number"><asp:Label runat="server"  ID="CalidadTaxLM"></asp:Label>  
                        </span>
                      </div>
                      <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                  </div>
                  <!-- /.col -->
                
                
                
                </div>
                <div class="row" runat="server" visible="false">
                  <div class="col-md-3 col-sm-6 col-12">
                    <div class="info-box">
                      <span class="info-box-icon bg-info elevation-1"><i class="fas fa-cog"></i></span>
        
                      <div class="info-box-content">
                        <span class="info-box-text">Group Part Sales With Tax LM</span>
                        <span class="info-box-number">12345678901234<small>%</small>
                        </span>
                      </div>
                      <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                  </div>
                  <!-- /.col -->
                  <div class="col-md-3 col-sm-6 col-12">
                    <div class="info-box">
                      <span class="info-box-icon bg-info elevation-1"><i class="fas fa-cog"></i></span>
        
                      <div class="info-box-content">
                        <span class="info-box-text">SPC Part Sales With Tax LM</span>
                        <span class="info-box-number">12345678901234<small>%</small>
                        </span>
                      </div>
                      <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                  </div>
                  <!-- /.col -->
                  <div class="col-md-3 col-sm-6 col-12">
                    <div class="info-box">
                      <span class="info-box-icon bg-info elevation-1"><i class="fas fa-cog"></i></span>
        
                      <div class="info-box-content">
                        <span class="info-box-text">Qualitat Parts Sales With Tax LM</span>
                        <span class="info-box-number">12345678901234<small>%</small>
                        </span>
                      </div>
                      <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                  </div>
                  <!-- /.col -->
                  <div class="col-md-3 col-sm-6 col-12">
                    <div class="info-box">
                      <span class="info-box-icon bg-info elevation-1"><i class="fas fa-cog"></i></span>
        
                      <div class="info-box-content">
                        <span class="info-box-text">Calidad Parts  Sales With Tax LM</span>
                        <span class="info-box-number">12345678901234<small>%</small>
                        </span>
                      </div>
                      <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                  </div>
                  <!-- /.col -->
                
                
                
                </div>
                <div class="row bg-gradient-olive"><span >Current Month Sale</span></div>
                <hr style="width: 100%;color:brown;border-top: 5px solid;">
                <div class="row">
                  <div class="col-md-3 col-sm-6 col-12">
                    <div class="info-box">
                      <span class="info-box-icon bg-info elevation-1"><i class="fas fa-cog"></i></span>
        
                      <div class="info-box-content">
                        <span class="info-box-text">Group Basic Sale TM</span>
                        <span class="info-box-number"><asp:Label runat="server"  ID="totalCM"></asp:Label>  
                        </span>
                      </div>
                      <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                  </div>
                  <!-- /.col -->
                  <div class="col-md-3 col-sm-6 col-12">
                    <div class="info-box">
                      <span class="info-box-icon bg-info elevation-1"><i class="fas fa-cog"></i></span>
        
                      <div class="info-box-content">
                        <span class="info-box-text">SPC Basic Sale TM</span>
                        <span class="info-box-number"><asp:Label runat="server"  ID="SPCCM"></asp:Label>  
                        </span>
                      </div>
                      <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                  </div>
                  <!-- /.col -->
                  <div class="col-md-3 col-sm-6 col-12">
                    <div class="info-box">
                      <span class="info-box-icon bg-info elevation-1"><i class="fas fa-cog"></i></span>
        
                      <div class="info-box-content">
                        <span class="info-box-text">Qualitat Basic Sale TM</span>
                        <span class="info-box-number"><asp:Label runat="server"  ID="QualitatCM"></asp:Label>  
                        </span>
                      </div>
                      <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                  </div>
                  <!-- /.col -->
                  <div class="col-md-3 col-sm-6 col-12">
                    <div class="info-box">
                      <span class="info-box-icon bg-info elevation-1"><i class="fas fa-cog"></i></span>
        
                      <div class="info-box-content">
                        <span class="info-box-text">Calidad Basic Sale TM</span>
                        <span class="info-box-number"><asp:Label runat="server"  ID="CalidadCM"></asp:Label>  
                        </span>
                      </div>
                      <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                  </div>
                  <!-- /.col -->
                
                
                
                </div>
                <div class="row">
                  <div class="col-md-3 col-sm-6 col-12">
                    <div class="info-box">
                      <span class="info-box-icon bg-info elevation-1"><i class="fas fa-cog"></i></span>
        
                      <div class="info-box-content">
                        <span class="info-box-text">Group Sales With Tax TM</span>
                        <span class="info-box-number"><asp:Label runat="server"  ID="TotalTaxCM"></asp:Label>  
                        </span>
                      </div>
                      <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                  </div>
                  <!-- /.col -->
                  <div class="col-md-3 col-sm-6 col-12">
                    <div class="info-box">
                      <span class="info-box-icon bg-info elevation-1"><i class="fas fa-cog"></i></span>
        
                      <div class="info-box-content">
                        <span class="info-box-text">SPC Sales With Tax TM</span>
                        <span class="info-box-number"><asp:Label runat="server"  ID="SPCTaxCM"></asp:Label>  
                        </span>
                      </div>
                      <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                  </div>
                  <!-- /.col -->
                  <div class="col-md-3 col-sm-6 col-12">
                    <div class="info-box">
                      <span class="info-box-icon bg-info elevation-1"><i class="fas fa-cog"></i></span>
        
                      <div class="info-box-content">
                        <span class="info-box-text">Qualitat Sales With Tax TM</span>
                        <span class="info-box-number"><asp:Label runat="server"  ID="QualitatTaxCM"></asp:Label>  
                        </span>
                      </div>
                      <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                  </div>
                  <!-- /.col -->
                  <div class="col-md-3 col-sm-6 col-12">
                    <div class="info-box">
                      <span class="info-box-icon bg-info elevation-1"><i class="fas fa-cog"></i></span>
        
                      <div class="info-box-content">
                        <span class="info-box-text">Calidad Sales With Tax TM</span>
                        <span class="info-box-number"><asp:Label runat="server"  ID="CalidadTaxCM"></asp:Label>  
                        </span>
                      </div>
                      <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                  </div>
                  <!-- /.col -->
                
                
                
                </div>
                <div class="row" runat="server" visible="false">
                  <div class="col-md-3 col-sm-6 col-12">
                    <div class="info-box">
                      <span class="info-box-icon bg-info elevation-1"><i class="fas fa-cog"></i></span>
        
                      <div class="info-box-content">
                        <span class="info-box-text">Group Part Sales With Tax LM</span>
                        <span class="info-box-number">12345678901234<small>%</small>
                        </span>
                      </div>
                      <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                  </div>
                  <!-- /.col -->
                  <div class="col-md-3 col-sm-6 col-12">
                    <div class="info-box">
                      <span class="info-box-icon bg-info elevation-1"><i class="fas fa-cog"></i></span>
        
                      <div class="info-box-content">
                        <span class="info-box-text">SPC Part Sales With Tax LM</span>
                        <span class="info-box-number">12345678901234<small>%</small>
                        </span>
                      </div>
                      <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                  </div>
                  <!-- /.col -->
                  <div class="col-md-3 col-sm-6 col-12">
                    <div class="info-box">
                      <span class="info-box-icon bg-info elevation-1"><i class="fas fa-cog"></i></span>
        
                      <div class="info-box-content">
                        <span class="info-box-text">Qualitat Parts Sales With Tax LM</span>
                        <span class="info-box-number">12345678901234<small>%</small>
                        </span>
                      </div>
                      <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                  </div>
                  <!-- /.col -->
                  <div class="col-md-3 col-sm-6 col-12">
                    <div class="info-box">
                      <span class="info-box-icon bg-info elevation-1"><i class="fas fa-cog"></i></span>
        
                      <div class="info-box-content">
                        <span class="info-box-text">Calidad Parts  Sales With Tax LM</span>
                        <span class="info-box-number">12345678901234<small>%</small>
                        </span>
                      </div>
                      <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                  </div>
                  <!-- /.col -->
                
                
                
                </div>  
                <div class="row bg-gradient-olive"><span >Sale Diff</span></div>
                <hr style="width: 100%;color:brown;border-top: 5px solid;">
                <div class="row">
                  <div class="col-md-3 col-sm-6 col-12">
                    <div class="info-box">
                      <span class="info-box-icon bg-warning"><i class="fas fa-bell"></i></span>
        
                      <div class="info-box-content">
                        <span class="info-box-text">Group Basic Sale DIff</span>
                        <span class="info-box-number"><asp:Label runat="server"  ID="basicSalesDiff"></asp:Label>  </span>
                      </div>
                      <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                  </div>
                  <!-- /.col -->
                  <div class="col-md-3 col-sm-6 col-12">
                    <div class="info-box">
                      <span class="info-box-icon bg-warning"><i class="fas fa-bell"></i></span>
        
                      <div class="info-box-content">
                        <span class="info-box-text">Group Basic Sale Diff</span>
                        <span class="info-box-number"><asp:Label runat="server"  ID="SPCSalesDiff"></asp:Label>  </span>
                      </div>
                      <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                  </div>
                  <!-- /.col -->
                  <div class="col-md-3 col-sm-6 col-12">
                    <div class="info-box">
                      <span class="info-box-icon bg-warning"><i class="fas fa-bell"></i></span>
        
                      <div class="info-box-content">
                        <span class="info-box-text">Group Basic Sale Diff</span>
                        <span class="info-box-number"><asp:Label runat="server"  ID="qualitatSalesDiff"></asp:Label>  </span>
                      </div>
                      <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                  </div>
                  <div class="col-md-3 col-sm-6 col-12">
                    <div class="info-box">
                      <span class="info-box-icon bg-warning"><i class="fas fa-bell"></i></span>
        
                      <div class="info-box-content">
                        <span class="info-box-text">Group Basic Sale Diff</span>
                        <span class="info-box-number"><asp:Label runat="server"  ID="CalidadSalesDiff"></asp:Label>  </span>
                      </div>
                      <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                  </div>
                </div>
                <div class="row">
                  <div class="col-md-3 col-sm-6 col-12">
                    <div class="info-box">
                      <span class="info-box-icon bg-warning"><i class="fas fa-bell"></i></span>
        
                      <div class="info-box-content">
                        <span class="info-box-text">Group Basic Sale Diff</span>
                        <span class="info-box-number"><asp:Label runat="server"  ID="TotalSalesDiffTax"></asp:Label>  </span>
                      </div>
                      <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                  </div>
                  <!-- /.col -->
                  <div class="col-md-3 col-sm-6 col-12">
                    <div class="info-box">
                      <span class="info-box-icon bg-warning"><i class="fas fa-bell"></i></span>
        
                      <div class="info-box-content">
                        <span class="info-box-text">Group Part Sale Diff</span>
                        <span class="info-box-number"><asp:Label runat="server"  ID="SPCSalesDiffTax"></asp:Label>  </span> 
                      </div>
                      <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                  </div>
                  <!-- /.col -->
                  <div class="col-md-3 col-sm-6 col-12">
                    <div class="info-box">
                      <span class="info-box-icon bg-warning"><i class="fas fa-bell"></i></span>
        
                      <div class="info-box-content">
                        <span class="info-box-text">Group Part Sale Diff</span>
                        <span class="info-box-number"><asp:Label runat="server"  ID="QualitatSalesDiffTax"></asp:Label>  </span></span>
                      </div>
                      <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                  </div>
                  <div class="col-md-3 col-sm-6 col-12">
                    <div class="info-box">
                      <span class="info-box-icon bg-warning"><i class="fas fa-bell"></i></span>
        
                      <div class="info-box-content">
                        <span class="info-box-text">Group Part Sale Diff</span>
                        <span class="info-box-number"><asp:Label runat="server"  ID="CalidatSalesDiffTax"></asp:Label>  </span></span>
                      </div>
                      <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                  </div>
                </div>          
              </div>
              <!-- /.card-body -->
            </div>
            <!-- /.card -->
          </div>
          
        </div>
                  </div>
                  <!-- /.tab-pane -->
                  <div class="tab-pane" id="tab_2">
                    <div class="row">
                      <div class="col-md-4 col-sm-4 col-12">
                        <div class="row">
                          <div class="col-12">
                            <div class="card">
                              <div class="card-header">
                                <h3 class="card-title">Last 8 Days Sales</h3>
                
                                <div class="card-tools">
                                  <div class="input-group input-group-sm" style="width: 150px;">
                                    <input type="text" name="table_search" class="form-control float-right" placeholder="Search">
                
                                    <div class="input-group-append">
                                      <button type="submit" class="btn btn-default"><i class="fas fa-search"></i></button>
                                    </div>
                                  </div>
                                </div>
                              </div>
                              <!-- /.card-header -->
                              <div class="card-body table-responsive p-0">
                              <asp:Repeater ID="RepterDetails" runat="server"> 
                              <HeaderTemplate> 
                                <table class="table table-hover text-nowrap">
                                  <thead>
                                    <tr>
                                      <th>ID</th>
                                      
                                      <th>Date</th>
                                      <th>Amount</th>
                                      
                                    </tr>
                                  </thead>
                                  </HeaderTemplate> 
                                  <ItemTemplate> 
                                  <tbody>
                                    <tr>
                                      <td><%#Eval("RowNum")%></td>
                                      
                                      <td><%#Eval("INM_DATE")%></td>
                                      <td><%#Eval("GrandAmt")%></td>
                                    </tr>
                                    
                                  </tbody>
                                  </ItemTemplate> 
                                  <FooterTemplate> 
                                </table>
                                </FooterTemplate> 
                                </asp:Repeater> 
                              </div>
                              <!-- /.card-body -->
                            </div>
                            <!-- /.card -->
                          </div>
                        </div>
                      </div>
                      <!-- /.col -->
                      <div class="col-md-4 col-sm-4 col-12">
                        <div class="row">
                          <div class="col-12">
                            <div class="card">
                              <div class="card-header">
                                <h3 class="card-title">Last 8 Days Purchase</h3>
                
                                <div class="card-tools">
                                  <div class="input-group input-group-sm" style="width: 150px;">
                                    <input type="text" name="table_search" class="form-control float-right" placeholder="Search">
                
                                    <div class="input-group-append">
                                      <button type="submit" class="btn btn-default"><i class="fas fa-search"></i></button>
                                    </div>
                                  </div>
                                </div>
                              </div>
                              <!-- /.card-header -->
                              <div class="card-body table-responsive p-0">
                              <asp:Repeater ID="Repeaterpurchase" runat="server"> 
                              <HeaderTemplate> 
                                <table class="table table-hover text-nowrap">
                                  <thead>
                                    <tr>
                                      <th>ID</th>
                                      
                                      <th>Date</th>
                                      <th>Amount</th>
                                      
                                    </tr>
                                  </thead>
                                  </HeaderTemplate> 
                                  <ItemTemplate> 
                                  <tbody>
                                    <tr>
                                      <td><%#Eval("RowNum")%></td>
                                      
                                      <td><%#Eval("IWM_CHAL_DATE")%></td>
                                      <td><%#Eval("GrandAmt")%></td>
                                    </tr>
                                    
                                  </tbody>
                                  </ItemTemplate> 
                                  <FooterTemplate> 
                                </table>
                                </FooterTemplate> 
                                </asp:Repeater> 
                              </div>
                              <!-- /.card-body -->
                            </div>
                            <!-- /.card -->
                          </div>
                        </div>
                      </div>
                      <!-- /.col -->
                      <div class="col-md-4 col-sm-4 col-12">
                        <div class="row">
                          <div class="col-12">
                            <div class="card">
                              <div class="card-header">
                                <h3 class="card-title">Last 8 Days Raw Purchase</h3>
                
                                <div class="card-tools">
                                  <div class="input-group input-group-sm" style="width: 150px;">
                                    <input type="text" name="table_search" class="form-control float-right" placeholder="Search">
                
                                    <div class="input-group-append">
                                      <button type="submit" class="btn btn-default"><i class="fas fa-search"></i></button>
                                    </div>
                                  </div>
                                </div>
                              </div>
                              <!-- /.card-header -->
                              <div class="card-body table-responsive p-0">
                              <asp:Repeater ID="Repeaterpurchaseraw" runat="server"> 
                              <HeaderTemplate> 
                                <table class="table table-hover text-nowrap">
                                  <thead>
                                    <tr>
                                      <th>ID</th>
                                      
                                      <th>Date</th>
                                      <th>Amount</th>
                                      
                                    </tr>
                                  </thead>
                                  </HeaderTemplate> 
                                  <ItemTemplate> 
                                  <tbody>
                                    <tr>
                                      <td><%#Eval("RowNum")%></td>
                                      
                                      <td><%#Eval("IWM_CHAL_DATE")%></td>
                                      <td><%#Eval("GrandAmt")%></td>
                                    </tr>
                                    
                                  </tbody>
                                  </ItemTemplate> 
                                  <FooterTemplate> 
                                </table>
                                </FooterTemplate> 
                                </asp:Repeater> 
                              </div>
                              <!-- /.card-body -->
                            </div>
                            <!-- /.card -->
                          </div>
                        </div>
                      </div>
                    </div>
                    <div class="row">
          <div class="col-md-12">
            <div class="card card-primary collapsed-card">
              <div class="card-header">
                <h3 class="card-title">Schedule Compliance</h3>

                <div class="card-tools">
                  <button type="button" class="btn btn-tool" data-card-widget="collapse"><i class="fas fa-plus"></i>
                  </button>
                </div>
                <!-- /.card-tools -->
              </div>
              <!-- /.card-header -->
              <div class="card-body" style="display: none;">
                
                
                
                
                
                
                
                
                  
                
                
                <div class="row">
                      <div class="col-12">
                        <!-- Main content -->
    <section class="content">
      <div class="container-fluid">
        <div class="row">
          <div class="col-12">
            
            <div class="card">
              <div class="card-header">
                <h3 class="card-title">Schedule Compliance ALL</h3>
              </div>
              <!-- /.card-header -->
              <div class="card-body">
              <asp:Repeater ID="RepeaterShceduleCOmpliance" runat="server"> 
              <HeaderTemplate> 
                <table id="example1" class="table table-bordered table-striped">
                  <thead>
                  <tr>
                    <th>Party Name </th>
                    <th>Scheduled Qty</th>
                    <th>Invoice Qty</th>
                    <th>Schedule compliance %</th>
                    <th>Scheduled Qty In Values</th>
                    <th>Invoice Qty In Values</th>
                    
                  </tr>
                  </thead>
                  </HeaderTemplate> 
                  <ItemTemplate>
                  <tbody>
                  <tr>
                    <td><%#Eval("p_name")%></td>
                    <td><%#Eval("schdeuletQty")%>
                    </td>
                    <td><%#Eval("invoiceQty")%></td>
                    <td> <%#Eval("Schedulecompliance")%></td>
                    <td><%#Eval("schdeuletQtyInValue")%></td>
                    <td><%#Eval("invoiceQtyInvalue")%></td>
                    
                  </tr>
                  
                  </tbody>
                  </ItemTemplate>
                  <FooterTemplate> 
                  <tfoot>
                    <tr>
                      <th>Party Name </th>
                    <th>Scheduled Qty</th>
                    <th>Invoice Qty</th>
                    <th>Schedule compliance %</th>
                    <th>Scheduled Qty In Values</th>
                    <th>Invoice Qty In Values</th>
                    </tr>
                  </tfoot>
                </table>
                </FooterTemplate> 
                  </asp:Repeater> 
              </div>
              <!-- /.card-body -->
            </div>
            <!-- /.card -->
          </div>
          <!-- /.col -->
        </div>
        <!-- /.row -->
      </div>
      <!-- /.container-fluid -->
    </section>
    <!-- /.content -->
                      </div>
                    </div>
                  <div class="row">
                      <div class="col-12">
                        <!-- Main content -->
    <section class="content">
      <div class="container-fluid">
        <div class="row">
          <div class="col-12">
            
            <div class="card">
              <div class="card-header">
                <h3 class="card-title">Schedule Compliance 0- 25%</h3>
              </div>
              <!-- /.card-header -->
              <div class="card-body">
              <asp:Repeater ID="repeater025" runat="server"> 
              <HeaderTemplate> 
                <table id="example1" class="table table-bordered table-striped">
                  <thead>
                  <tr>
                    <th>Party Name </th>
                    <th>Scheduled Qty</th>
                    <th>Invoice Qty</th>
                    <th>Schedule compliance %</th>
                    <th>Scheduled Qty In Values</th>
                    <th>Invoice Qty In Values</th>
                    
                  </tr>
                  </thead>
                  </HeaderTemplate> 
                  <ItemTemplate>
                  <tbody>
                  <tr>
                    <td><%#Eval("p_name")%></td>
                    <td><%#Eval("schdeuletQty")%>
                    </td>
                    <td><%#Eval("invoiceQty")%></td>
                    <td> <%#Eval("Schedulecompliance")%></td>
                    <td><%#Eval("schdeuletQtyInValue")%></td>
                    <td><%#Eval("invoiceQtyInvalue")%></td>
                    
                  </tr>
                  
                  </tbody>
                  </ItemTemplate>
                  <FooterTemplate> 
                  <tfoot>
                    <tr>
                      <th>Party Name </th>
                    <th>Scheduled Qty</th>
                    <th>Invoice Qty</th>
                    <th>Schedule compliance %</th>
                    <th>Scheduled Qty In Values</th>
                    <th>Invoice Qty In Values</th>
                    </tr>
                  </tfoot>
                </table>
                </FooterTemplate> 
                  </asp:Repeater> 
              </div>
              <!-- /.card-body -->
            </div>
            <!-- /.card -->
          </div>
          <!-- /.col -->
        </div>
        <!-- /.row -->
      </div>
      <!-- /.container-fluid -->
    </section>
    <!-- /.content -->
                      </div>
                    </div>  
                    <div class="row">
                      <div class="col-12">
                        <!-- Main content -->
    <section class="content">
      <div class="container-fluid">
        <div class="row">
          <div class="col-12">
            
            <div class="card">
              <div class="card-header">
                <h3 class="card-title">Schedule Compliance 25- 50%</h3>
              </div>
              <!-- /.card-header -->
              <div class="card-body">
              <asp:Repeater ID="repeater050" runat="server"> 
              <HeaderTemplate> 
                <table id="example1" class="table table-bordered table-striped">
                  <thead>
                  <tr>
                    <th>Party Name </th>
                    <th>Scheduled Qty</th>
                    <th>Invoice Qty</th>
                    <th>Schedule compliance %</th>
                    <th>Scheduled Qty In Values</th>
                    <th>Invoice Qty In Values</th>
                    
                  </tr>
                  </thead>
                  </HeaderTemplate> 
                  <ItemTemplate>
                  <tbody>
                  <tr>
                    <td><%#Eval("p_name")%></td>
                    <td><%#Eval("schdeuletQty")%>
                    </td>
                    <td><%#Eval("invoiceQty")%></td>
                    <td> <%#Eval("Schedulecompliance")%></td>
                    <td><%#Eval("schdeuletQtyInValue")%></td>
                    <td><%#Eval("invoiceQtyInvalue")%></td>
                    
                  </tr>
                  
                  </tbody>
                  </ItemTemplate>
                  <FooterTemplate> 
                  <tfoot>
                    <tr>
                      <th>Party Name </th>
                    <th>Scheduled Qty</th>
                    <th>Invoice Qty</th>
                    <th>Schedule compliance %</th>
                    <th>Scheduled Qty In Values</th>
                    <th>Invoice Qty In Values</th>
                    </tr>
                  </tfoot>
                </table>
                </FooterTemplate> 
                  </asp:Repeater> 
              </div>
              <!-- /.card-body -->
            </div>
            <!-- /.card -->
          </div>
          <!-- /.col -->
        </div>
        <!-- /.row -->
      </div>
      <!-- /.container-fluid -->
    </section>
    <!-- /.content -->
                      </div>
                    </div>  
                        
                    <div class="row">
                      <div class="col-12">
                        <!-- Main content -->
    <section class="content">
      <div class="container-fluid">
        <div class="row">
          <div class="col-12">
            
            <div class="card">
              <div class="card-header">
                <h3 class="card-title">Schedule Compliance 50- 75%</h3>
              </div>
              <!-- /.card-header -->
              <div class="card-body">
              <asp:Repeater ID="repeater075" runat="server"> 
              <HeaderTemplate> 
                <table id="example1" class="table table-bordered table-striped">
                  <thead>
                  <tr>
                    <th>Party Name </th>
                    <th>Scheduled Qty</th>
                    <th>Invoice Qty</th>
                    <th>Schedule compliance %</th>
                    <th>Scheduled Qty In Values</th>
                    <th>Invoice Qty In Values</th>
                    
                  </tr>
                  </thead>
                  </HeaderTemplate> 
                  <ItemTemplate>
                  <tbody>
                  <tr>
                    <td><%#Eval("p_name")%></td>
                    <td><%#Eval("schdeuletQty")%>
                    </td>
                    <td><%#Eval("invoiceQty")%></td>
                    <td> <%#Eval("Schedulecompliance")%></td>
                    <td><%#Eval("schdeuletQtyInValue")%></td>
                    <td><%#Eval("invoiceQtyInvalue")%></td>
                    
                  </tr>
                  
                  </tbody>
                  </ItemTemplate>
                  <FooterTemplate> 
                  <tfoot>
                    <tr>
                      <th>Party Name </th>
                    <th>Scheduled Qty</th>
                    <th>Invoice Qty</th>
                    <th>Schedule compliance %</th>
                    <th>Scheduled Qty In Values</th>
                    <th>Invoice Qty In Values</th>
                    </tr>
                  </tfoot>
                </table>
                </FooterTemplate> 
                  </asp:Repeater> 
              </div>
              <!-- /.card-body -->
            </div>
            <!-- /.card -->
          </div>
          <!-- /.col -->
        </div>
        <!-- /.row -->
      </div>
      <!-- /.container-fluid -->
    </section>
    <!-- /.content -->
                      </div>
                    </div> 
                    <div class="row">
                      <div class="col-12">
                        <!-- Main content -->
    <section class="content">
      <div class="container-fluid">
        <div class="row">
          <div class="col-12">
            
            <div class="card">
              <div class="card-header">
                <h3 class="card-title">Schedule Compliance 75- 100%</h3>
              </div>
              <!-- /.card-header -->
              <div class="card-body">
              <asp:Repeater ID="repeater100" runat="server"> 
              <HeaderTemplate> 
                <table id="example1" class="table table-bordered table-striped">
                  <thead>
                  <tr>
                    <th>Party Name </th>
                    <th>Scheduled Qty</th>
                    <th>Invoice Qty</th>
                    <th>Schedule compliance %</th>
                    <th>Scheduled Qty In Values</th>
                    <th>Invoice Qty In Values</th>
                    
                  </tr>
                  </thead>
                  </HeaderTemplate> 
                  <ItemTemplate>
                  <tbody>
                  <tr>
                    <td><%#Eval("p_name")%></td>
                    <td><%#Eval("schdeuletQty")%>
                    </td>
                    <td><%#Eval("invoiceQty")%></td>
                    <td> <%#Eval("Schedulecompliance")%></td>
                    <td><%#Eval("schdeuletQtyInValue")%></td>
                    <td><%#Eval("invoiceQtyInvalue")%></td>
                    
                  </tr>
                  
                  </tbody>
                  </ItemTemplate>
                  <FooterTemplate> 
                  <tfoot>
                    <tr>
                      <th>Party Name </th>
                    <th>Scheduled Qty</th>
                    <th>Invoice Qty</th>
                    <th>Schedule compliance %</th>
                    <th>Scheduled Qty In Values</th>
                    <th>Invoice Qty In Values</th>
                    </tr>
                  </tfoot>
                </table>
                </FooterTemplate> 
                  </asp:Repeater> 
              </div>
              <!-- /.card-body -->
            </div>
            <!-- /.card -->
          </div>
          <!-- /.col -->
        </div>
        <!-- /.row -->
      </div>
      <!-- /.container-fluid -->
    </section>
    <!-- /.content -->
                      </div>
                    </div> 
                     <div class="row">
                      <div class="col-12">
                        <!-- Main content -->
    <section class="content">
      <div class="container-fluid">
        <div class="row">
          <div class="col-12">
            
            <div class="card">
              <div class="card-header">
                <h3 class="card-title">Schedule Compliance > 100%</h3>
              </div>
              <!-- /.card-header -->
              <div class="card-body">
              <asp:Repeater ID="repeaterG100" runat="server"> 
              <HeaderTemplate> 
                <table id="example1" class="table table-bordered table-striped">
                  <thead>
                  <tr>
                    <th>Party Name </th>
                    <th>Scheduled Qty</th>
                    <th>Invoice Qty</th>
                    <th>Schedule compliance %</th>
                    <th>Scheduled Qty In Values</th>
                    <th>Invoice Qty In Values</th>
                    
                  </tr>
                  </thead>
                  </HeaderTemplate> 
                  <ItemTemplate>
                  <tbody>
                  <tr>
                    <td><%#Eval("p_name")%></td>
                    <td><%#Eval("schdeuletQty")%>
                    </td>
                    <td><%#Eval("invoiceQty")%></td>
                    <td> <%#Eval("Schedulecompliance")%></td>
                    <td><%#Eval("schdeuletQtyInValue")%></td>
                    <td><%#Eval("invoiceQtyInvalue")%></td>
                    
                  </tr>
                  
                  </tbody>
                  </ItemTemplate>
                  <FooterTemplate> 
                  <tfoot>
                    <tr>
                      <th>Party Name </th>
                    <th>Scheduled Qty</th>
                    <th>Invoice Qty</th>
                    <th>Schedule compliance %</th>
                    <th>Scheduled Qty In Values</th>
                    <th>Invoice Qty In Values</th>
                    </tr>
                  </tfoot>
                </table>
                </FooterTemplate> 
                  </asp:Repeater> 
              </div>
              <!-- /.card-body -->
            </div>
            <!-- /.card -->
          </div>
          <!-- /.col -->
        </div>
        <!-- /.row -->
      </div>
      <!-- /.container-fluid -->
    </section>
    <!-- /.content -->
                      </div>
                    </div>
              </div>
              <!-- /.card-body -->
            </div>
            <!-- /.card -->
          </div>
          
        </div>
                    
                  </div>
                  <!-- /.tab-pane -->
                  <div class="tab-pane" id="tab_3">
                    <div class="row">
                      <div class="col-md-6 col-sm-6 col-12">
                        <div class="row">
                          <div class="col-12">
                            <div class="card">
                              <div class="card-header">
                                <h3 class="card-title">Last 8 Days Sales</h3>
                
                                <div class="card-tools">
                                  <div class="input-group input-group-sm" style="width: 150px;">
                                    <input type="text" name="table_search" class="form-control float-right" placeholder="Search">
                
                                    <div class="input-group-append">
                                      <button type="submit" class="btn btn-default"><i class="fas fa-search"></i></button>
                                    </div>
                                  </div>
                                </div>
                              </div>
                              <!-- /.card-header -->
                              <div class="card-body table-responsive p-0">
                                <table class="table table-hover text-nowrap">
                                  <thead>
                                    <tr>
                                      <th>ID</th>
                                      
                                      <th>Date</th>
                                      <th>Amount</th>
                                      
                                    </tr>
                                  </thead>
                                  <tbody>
                                    <tr>
                                      <td>183</td>
                                      
                                      <td>11-7-2014</td>
                                      <td>Amount.</td>
                                    </tr>
                                    <tr>
                                      <td>183</td>
                                      
                                      <td>11-7-2014</td>
                                      <td>Amount.</td>
                                    </tr>
                                  </tbody>
                                </table>
                              </div>
                              <!-- /.card-body -->
                            </div>
                            <!-- /.card -->
                          </div>
                        </div>
                      </div>
                      <!-- /.col -->
                      <div class="col-md-6 col-sm-6 col-12">
                        <div class="row">
                          <div class="col-12">
                            <div class="card">
                              <div class="card-header">
                                <h3 class="card-title">Last 8 Days Purchase</h3>
                
                                <div class="card-tools">
                                  <div class="input-group input-group-sm" style="width: 150px;">
                                    <input type="text" name="table_search" class="form-control float-right" placeholder="Search">
                
                                    <div class="input-group-append">
                                      <button type="submit" class="btn btn-default"><i class="fas fa-search"></i></button>
                                    </div>
                                  </div>
                                </div>
                              </div>
                              <!-- /.card-header -->
                              <div class="card-body table-responsive p-0">
                                <table class="table table-hover text-nowrap">
                                  <thead>
                                    <tr>
                                      <th>ID</th>
                                      
                                      <th>Date</th>
                                      <th>Amount</th>
                                      
                                    </tr>
                                  </thead>
                                  <tbody>
                                    <tr>
                                      <td>183</td>
                                      
                                      <td>11-7-2014</td>
                                      <td>Amount.</td>
                                    </tr>
                                    <tr>
                                      <td>183</td>
                                      
                                      <td>11-7-2014</td>
                                      <td>Amount.</td>
                                    </tr>
                                  </tbody>
                                </table>
                              </div>
                              <!-- /.card-body -->
                            </div>
                            <!-- /.card -->
                          </div>
                        </div>
                      </div>
                      <!-- /.col -->
                      
                    </div>
                  </div>
				  <div class="tab-pane" id="tab_4">Tab 4
            <div class="row">
              <div class="col-md-6 col-sm-6 col-12">
                <div class="row">
                  <div class="col-12">
                    <div class="card">
                      <div class="card-header">
                        <h3 class="card-title">Last 8 Days Sales</h3>
        
                        <div class="card-tools">
                          <div class="input-group input-group-sm" style="width: 150px;">
                            <input type="text" name="table_search" class="form-control float-right" placeholder="Search">
        
                            <div class="input-group-append">
                              <button type="submit" class="btn btn-default"><i class="fas fa-search"></i></button>
                            </div>
                          </div>
                        </div>
                      </div>
                      <!-- /.card-header -->
                      <div class="card-body table-responsive p-0">
                        <table class="table table-hover text-nowrap">
                          <thead>
                            <tr>
                              <th>ID</th>
                              
                              <th>Date</th>
                              <th>Amount</th>
                              
                            </tr>
                          </thead>
                          <tbody>
                            <tr>
                              <td>183</td>
                              
                              <td>11-7-2014</td>
                              <td>Amount.</td>
                            </tr>
                            <tr>
                              <td>183</td>
                              
                              <td>11-7-2014</td>
                              <td>Amount.</td>
                            </tr>
                          </tbody>
                        </table>
                      </div>
                      <!-- /.card-body -->
                    </div>
                    <!-- /.card -->
                  </div>
                </div>
              </div>
              <!-- /.col -->
              <div class="col-md-6 col-sm-6 col-12">
                <div class="row">
                  <div class="col-12">
                    <div class="card">
                      <div class="card-header">
                        <h3 class="card-title">Last 8 Days Purchase</h3>
        
                        <div class="card-tools">
                          <div class="input-group input-group-sm" style="width: 150px;">
                            <input type="text" name="table_search" class="form-control float-right" placeholder="Search">
        
                            <div class="input-group-append">
                              <button type="submit" class="btn btn-default"><i class="fas fa-search"></i></button>
                            </div>
                          </div>
                        </div>
                      </div>
                      <!-- /.card-header -->
                      <div class="card-body table-responsive p-0">
                        <table class="table table-hover text-nowrap">
                          <thead>
                            <tr>
                              <th>ID</th>
                              
                              <th>Date</th>
                              <th>Amount</th>
                              
                            </tr>
                          </thead>
                          <tbody>
                            <tr>
                              <td>183</td>
                              
                              <td>11-7-2014</td>
                              <td>Amount.</td>
                            </tr>
                            <tr>
                              <td>183</td>
                              
                              <td>11-7-2014</td>
                              <td>Amount.</td>
                            </tr>
                          </tbody>
                        </table>
                      </div>
                      <!-- /.card-body -->
                    </div>
                    <!-- /.card -->
                  </div>
                </div>
              </div>
              <!-- /.col -->
              
            </div>
                  </div>
                  <!-- /.tab-pane -->
                </div>
                <!-- /.tab-content -->
              </div><!-- /.card-body -->
            </div>
              </div>
              <!-- /.card-body -->
            </div>
            <!-- /.card -->
          </div>

            <!-- ./card -->
          </div>
          <!-- /.col -->
        </div>
        

      
      </div><!-- /.container-fluid -->
    </section>
    <!-- /.content -->
  </div>
  <!-- /.content-wrapper -->
  <footer class="main-footer">
    <div class="float-right d-none d-sm-block">
      <b>Version</b> 3.0.5
    </div>
    <strong>Copyright &copy; 2010 <a href="">One Click ERP</a>.</strong> All rights
    reserved.
  </footer>

  <!-- Control Sidebar -->
  <aside class="control-sidebar control-sidebar-dark">
    <!-- Control sidebar content goes here -->
  </aside>
  <!-- /.control-sidebar -->
</div>
<!-- ./wrapper -->

<!-- jQuery -->
<script src="plugins/jquery/jquery.min.js"></script>
<!-- Bootstrap 4 -->
<script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
<!-- DataTables -->
<script src="plugins/datatables/jquery.dataTables.min.js"></script>
<script src="plugins/datatables-bs4/js/dataTables.bootstrap4.min.js"></script>
<script src="plugins/datatables-responsive/js/dataTables.responsive.min.js"></script>
<script src="plugins/datatables-responsive/js/responsive.bootstrap4.min.js"></script>
<!-- AdminLTE App -->

<!-- AdminLTE App -->
<script src="dist/js/adminlte.min.js"></script>
<!-- AdminLTE for demo purposes -->
<script src="dist/js/demo.js"></script>
<script>
  $(function () {
    $("#example1").DataTable({
      "responsive": true,
      "autoWidth": false,
    });
    $('#example2').DataTable({
      "paging": true,
      "lengthChange": false,
      "searching": false,
      "ordering": true,
      "info": true,
      "autoWidth": false,
      "responsive": true,
    });
  });
</script>
</body>
</html>

