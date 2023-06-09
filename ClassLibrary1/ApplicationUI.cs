﻿using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Windows;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;
using Tuna.Revit.Extension.Revit;
using UIFramework;

namespace BIMBOX.Revit.Tuna
{
    public class ApplicationUI : IExternalApplication
    {
        internal static ApplicationUI? Current;
        internal Services.ExternalEventService? _service;
        private const string _tab = "Briumn";
        private const string panelName1 = "族库管理";
        private const string panelName2 = "建筑防火设计";
        private const string panelName3 = "数据交互";
        private const string panelName4 = "机电交互";
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            RibbonControl ribbonControl = RevitRibbonControl.RibbonControl;
            //创建选项卡RibbonTab, 名称已经定义在构造函数；
            application.CreateRibbonTab(_tab);
            //创建第一个RibbonPanel ；
            //创建第一个RibbonPanel ；
            //创建第一个RibbonPanel ；
            //创建第一个RibbonPanel ；
            //创建第一个RibbonPanel ；
            Autodesk.Revit.UI.RibbonPanel panelFamily = application.CreateRibbonPanel(_tab, panelName1);

            var numberDoorPBD = new PushButtonData("门编号0", "门编号1", typeof(ApplicationUI).Assembly.Location, "NumberDoorsCommand");
            numberDoorPBD.LargeImage = Properties.Resources.NumberDoors.ConvertToBitmapSource();
            numberDoorPBD.Image = Properties.Resources.NumberDoors_16.ConvertToBitmapSource();

            PushButtonData bimObjectButtonData = new PushButtonData(
                "BIMObjectButton",
                "BIMObject",
                Assembly.GetExecutingAssembly().Location,
                "BIMBOX.Revit.Tuna.Commands.OpenURLCommand");
            bimObjectButtonData.Image = Properties.Resources.bimobject_16.ConvertToBitmapSource();
            bimObjectButtonData.LargeImage = Properties.Resources.bimobject_32.ConvertToBitmapSource();
            bimObjectButtonData.ToolTip = "公开免费族库网站BIMObject.";
            bimObjectButtonData.ToolTipImage = Properties.Resources.BIMObjectScreen.ConvertToBitmapSource();
            bimObjectButtonData.LongDescription = "BIMObject.com is a global marketplace for the construction industry. We provide design inspiration and digital product information to the world's architects and engineers while giving building product manufacturers a better way to reach, influence, and understand them.";


            PushButtonData familyManagerbtn = new PushButtonData(
               "族管理器",
               "族管理器",
               Assembly.GetExecutingAssembly().Location,
               "OpenURLCommand");
            familyManagerbtn.Image = Properties.Resources.bimobject_16.ConvertToBitmapSource();
            familyManagerbtn.LargeImage = Properties.Resources.bimobject_32.ConvertToBitmapSource();
            familyManagerbtn.ToolTip = "公开免费族库网站BIMObject.";
            familyManagerbtn.ToolTipImage = Properties.Resources.BIMObjectScreen.ConvertToBitmapSource();

            //创建Stack并加入3个按钮
            panelFamily.AddStackedItems(familyManagerbtn, bimObjectButtonData, numberDoorPBD);

            panelFamily.CreateButton<Commands.ModelessCommand>((ur) =>
            {
                ur.Text = "BIMObject";
                ur.Image = Properties.Resources.bimobject_16.ConvertToBitmapSource();
                ur.LargeImage = Properties.Resources.bimobject_32.ConvertToBitmapSource();
                ur.ToolTip = "公开免费族库网站BIMObject.";
                ur.ToolTipImage = Properties.Resources.BIMObjectScreen.ConvertToBitmapSource();
                ur.LongDescription = "BIMObject.com is a global marketplace for the construction industry.We provide design inspiration and digital product information to the world's architects and engineers while giving building product manufacturers a better way to reach, influence, and understand them.";
            });
            panelFamily.CreateButton<Commands.MaterialsCommand>((ma) =>
            {
                ma.Text = "材质管理器";
                ma.LargeImage = Properties.Resources.Materials.ConvertToBitmapSource();
                ma.ToolTip = "用于文件内材质的增、改、删、查。";
            });
            panelFamily.CreateButton<Commands.PickWallAndCreateNewWall>((Oo) =>
            {
                Oo.Text = "垂直墙";
                Oo.LargeImage = Properties.Resources.FloorPlan6.ConvertToBitmapSource();
                Oo.ToolTip = "公开免费族库网站BIMObject.";
                Oo.LongDescription = "BIMObject.com is a global marketplace for the construction industry.We provide design inspiration and digital product information to the world's architects and engineers while giving building product manufacturers a better way to reach, influence, and understand them.";
            });

            // 创建第2个PANEL
            // 创建第2个PANEL
            // 创建第2个PANEL
            // 创建第2个PANEL
            // 创建第2个PANEL
            Autodesk.Revit.UI.RibbonPanel panel = application.CreateRibbonPanel(_tab, panelName2);
            // 创建第一个面板并添加第二个按钮
            panel.CreateButton<Commands.Face2Face>((c) =>
            {
                c.Text = "添加面层";
                c.LargeImage = Properties.Resources.FaceWall.ConvertToBitmapSource();
                c.ToolTipImage = Properties.Resources.Snipaste_2023_05_12_11_02_03.ConvertToBitmapSource();
                c.ToolTip = "添加附着在墙面的面层";
                c.LongDescription = "将面层添加到面表面，如有必要请自行添加墙类型及材质。";
            });

            // 创建第一个面板并添加第3个按钮
            panel.CreateButton<Commands.SwitchBackGround>((D) =>
            {
                D.Text = "黑白背景";
                D.LargeImage = Properties.Resources.CanvasChange.ConvertToBitmapSource();
                D.Image = Properties.Resources.black_and_white_16.ConvertToBitmapSource();
                D.ToolTipImage = Properties.Resources.CanvasChange.ConvertToBitmapSource();
                D.ToolTip = "点击切换黑白背景。";
                D.LongDescription = "。";
            });

            panel.CreateButton<Commands.NumberDoorsCommand>((Do) =>
            {
                Do.Text = "门编号";
                Do.LargeImage = Properties.Resources.NumberDoors.ConvertToBitmapSource();
                Do.Image = Properties.Resources.NumberDoors_16.ConvertToBitmapSource();
                Do.ToolTipImage = Properties.Resources.CanvasChange.ConvertToBitmapSource();
                Do.ToolTip = "点击切换黑白背景。";
                Do.LongDescription = "。";
            });

            //添加一个分隔线
            panel.AddSeparator();

            // 创建第一个面板并添加第4个按钮
            panel.CreateButton<Commands.Pt2RoomPath>((S) =>
            {
                S.Text = "Pt2RoomPath";
                S.LargeImage = Properties.Resources.Pt2RoomPath.ConvertToBitmapSource();
                S.ToolTipImage = Properties.Resources.CanvasChange.ConvertToBitmapSource();
                S.ToolTip = "Create path from selected pt to all room(s)。";
                S.LongDescription = "。";
            });

            // 创建第一个面板并添加第5个按钮
            panel.CreateButton<Commands.Geometry2>((C) =>
            {
                C.Text = "WallGeometry";
                C.LargeImage = Properties.Resources.Wall.ConvertToBitmapSource();
                C.ToolTipImage = Properties.Resources.CanvasChange.ConvertToBitmapSource();
                C.ToolTip = "统计墙的基本信息。";
                C.LongDescription = "。";
            });

            // 创建第一个面板并添加第6个按钮
            panel.CreateButton<Commands.FireProofArea>((W) =>
            {
                W.Text = "防火分区";
                W.LargeImage = Properties.Resources.FloorPlan1.ConvertToBitmapSource();
                W.ToolTip = "统计墙的基本信息。";
                W.LongDescription = "。";
            });

            // 创建第一个面板并添加第7个按钮
            panel.CreateButton<Commands.RoomBoundaryLocation>((R) =>
            {
                R.Text = "房间边界位置";
                R.LargeImage = Properties.Resources.FloorPlan7.ConvertToBitmapSource();
                R.ToolTip = "统计墙的基本信息。";
                R.LongDescription = "。";
            });

            // 创建第一个面板并添加第8个按钮
            panel.CreateButton<Commands.ModelessCommand>((A) =>
            {
                A.Text = "ModelessCommand";
                A.LargeImage = Properties.Resources.FloorPlan3.ConvertToBitmapSource();
                A.ToolTip = "ModelessCommand";
                A.LongDescription = "。";
            });
            // 创建第一个面板并添加第9个按钮
            panel.CreateButton<Commands.Combination>((B) =>
            {
                B.Text = "创建路径网";
                B.LargeImage = Properties.Resources.NetWeb.ConvertToBitmapSource();
                B.ToolTip = "ModelessCommand";
                B.LongDescription = "。";
            });
            // 创建第一个面板并添加第10个按钮
            panel.CreateButton<Commands.NearestExit_4>((y) =>
            {
                y.Text = "清理路网";
                y.LargeImage = Properties.Resources.CleanPath.ConvertToBitmapSource();
                y.ToolTip = "ModelessCommand";
                y.LongDescription = "。";
            });

            // 创建第一个面板并添加第11个按钮
            panel.CreateButton<Commands.MainUIPanelOpen>(YY =>
            {
                YY.Text = "设计面板";
                YY.LargeImage = Properties.Resources.CleanPath.ConvertToBitmapSource();
                YY.ToolTip = "打开模型设计面板";
                YY.LongDescription = "补全建设指标";
            });

            panel.CreateButton<Commands.Demo>(YYY =>
            {
                YYY.Text = "demo";
                YYY.LargeImage = Properties.Resources.FloorPlan6.ConvertToBitmapSource();
                YYY.ToolTip = "打开模型设计面板";
                YYY.LongDescription = "什么都没有。";
            });

            panel.CreateButton<Commands.Export2glTFCommand>(WE =>
            {
                WE.Text = "导出gltf";
                WE.LargeImage = Properties.Resources.gltf.ConvertToBitmapSource();
                WE.ToolTip = "将Revit文件导出为glTf格式";
                WE.ToolTipImage = Properties.Resources.glTFModel.ConvertToBitmapSource();
                WE.LongDescription =    "由现有 OpenGL 的维护组织 Khronos 推出，目的就是为了统一用于应用程序渲染的 3D 格式，更适用于基于 OpenGL 的引擎；" +
                                        "\r\n减少了 3D 格式中除了与渲染无关的冗余信息，最小化 3D 文件资源；" +
                                        "\r\n优化了应用程序读取效率和和减少渲染模型的运行时间；";
            });

            panel.CreateButton<Commands.FloorFinish>(YU =>
            {
                YU.Text = "Floor Finish";
                YU.LargeImage = Properties.Resources.FloorFinish.ConvertToBitmapSource();
                YU.ToolTip = "将Revit文件导出为glTf格式";
                YU.LongDescription = "这是一个新的LongDescription；";
            });











            // 创建第二个面板并添加按钮
            var ribbonPanel = application.CreateRibbonPanel(_tab, panelName4);


            PushButtonData buttonData = new PushButtonData(
               "翻管",
               "管道翻弯",
               typeof(ApplicationUI).Assembly.Location,
               "PipTurnOver");
            buttonData.Image = Properties.Resources.Pipes_16.ConvertToBitmapSource();
            buttonData.LargeImage = Properties.Resources.Pipes.ConvertToBitmapSource();
            buttonData.ToolTip = "手动选择管道翻弯的位置/高度。";
            buttonData.ToolTipImage = Properties.Resources.BIMObjectScreen.ConvertToBitmapSource();





            var buttonData5 = new PushButtonData("全体起立！", "全体起立！", typeof(ApplicationUI).Assembly.Location, "MultiSelectAndGetBoundingBox_Good");
           
            // 指定按钮图标的路径
            buttonData.LargeImage = Properties.Resources.Pipes.ConvertToBitmapSource();
            buttonData5.LargeImage = Properties.Resources.FloorPlan2.ConvertToBitmapSource();
            var button = ribbonPanel.AddItem(buttonData);
            var button3 = ribbonPanel.AddItem(buttonData5);
            RibbonToolTip toolTip = new RibbonToolTip()
            {
                Title = "HVAC翻弯",
                Content = "将管道指定段从上方翻过，避免碰撞。",
                ExpandedContent = "如有必要将开发下方翻弯及单侧升降的功能（此功能保留了小黑子播放脚本请勿删除）。",
                ExpandedVideo = new  Uri("C:\\Program Files\\Autodesk\\Revit 2023\\videos\\tooltip.mp4"),
            };

            // 设置按钮的工具提示
            SetRibbonItemToolTip(button, toolTip);
           


            //窗口停靠的按键，默认打开 true/false 在 DockablePaneProvider中修改
            var ribbonPanel2 = application.CreateRibbonPanel(_tab, "窗口停靠");
            var type = typeof(BIMBOX.Revit.Tuna.Commands.DockablePaneCommand);
            PushButtonData button2 = new PushButtonData("dockablePane", "窗口", type.Assembly.Location, type.FullName)
            {
                LargeImage= Properties.Resources.FloorPlan7.ConvertToBitmapSource(),
                Image = Properties.Resources.window16.ConvertToBitmapSource(),
            };
            ribbonPanel2.AddItem(button2);
            application.RegisterDockablePane(DockablePanes.DockablePaneProvider.Id, "DockablePane", new DockablePanes.DockablePaneProvider());

            //创建Stack并加入
            panel.AddStackedItems(button2,buttonData,buttonData5);

            return Result.Succeeded;
        }
        public BitmapImage GetBitmapImage(string imagePath)
        {
            if (File.Exists(imagePath))
                return new BitmapImage(new Uri(imagePath));
            else
                return null;
        }
        // 设置RibbonItem的工具提示
        void SetRibbonItemToolTip(Autodesk.Revit.UI.RibbonItem item, RibbonToolTip toolTip)
        {
            var ribbonItem = GetRibbonItem(item);
            if (ribbonItem == null)
                return;
            ribbonItem.ToolTip = toolTip;
        }
        // 获取RibbonItem
        private Autodesk.Windows.RibbonItem? GetRibbonItem(Autodesk.Revit.UI.RibbonItem item)
        {
            Type itemType = item.GetType();
            var mi = itemType.GetMethod("getRibbonItem",
              BindingFlags.NonPublic | BindingFlags.Instance);
            var windowRibbonItem = mi.Invoke(item, null);
            return windowRibbonItem as Autodesk.Windows.RibbonItem;
        }
    }
}

